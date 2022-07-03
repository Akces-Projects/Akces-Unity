using System;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Unity.Models;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;

namespace Akces.Unity.App
{
    public delegate void OnOperationExecuted(HarmonogramPosition harmonogramPosition, OperationReport operationReport);
    public delegate void OnOperationStarted(HarmonogramPosition harmonogramPosition);
    internal class HarmonogramWorker : IDisposable
    {
        private const int WORKER_RUN_INTERVAL_SECONDS = 10;

        private bool isRunning;
        private readonly Timer timer;
        private readonly NexoContext nexoContext;
        private readonly HarmonogramsManager harmonogramsManager;
        private readonly OperationReportsManager operationReportsManager;

        public bool Enabled { get; set; }
        public Harmonogram ActiveHarmonogram { get; private set; }
        public OnOperationExecuted OnOperationExecuted { get; set; }
        public OnOperationStarted OnOperationStarted { get; set; }

        public HarmonogramWorker()
        {
            harmonogramsManager = new HarmonogramsManager();
            operationReportsManager = new OperationReportsManager();
            nexoContext = ServicesProvider.GetService<NexoContext>();
            OnOperationExecuted = new OnOperationExecuted(OperationExecuted);
            OnOperationStarted = new OnOperationStarted((h) => { });

            ActiveHarmonogram = GetActiveHarmonogram();
            Enabled = ActiveHarmonogram?.WorkerEnabled ?? false;

            timer = new Timer();
            timer.Enabled = true;
            timer.Enabled = true;
            timer.Interval = 1000 * WORKER_RUN_INTERVAL_SECONDS;
            timer.Elapsed += OnTimerElapsed;
        }

        private Harmonogram GetActiveHarmonogram()
        {
            var harmonogramsManager = new HarmonogramsManager();
            var harmonogram = harmonogramsManager.Get().FirstOrDefault(x => x.Active);

            if (harmonogram == null)
                return null;

            var fullHarmonogram = harmonogramsManager.Get(harmonogram.Id);
            return fullHarmonogram;
        }
        public void SetActiveHarmonogram(Harmonogram harmonogram)
        {
            Enabled = false;

            if (ActiveHarmonogram != null)
            {
                using (var harmonogramBO = harmonogramsManager.Find(ActiveHarmonogram))
                {
                    harmonogramBO.Data.Active = false;
                    harmonogramBO.Save();
                }
            }

            using (var harmonogramBO = harmonogramsManager.Find(harmonogram))
            {
                harmonogramBO.Data.Active = true;
                harmonogramBO.Save();
                ActiveHarmonogram = harmonogramBO.Data;
                Enabled = ActiveHarmonogram.WorkerEnabled;
            }
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (ActiveHarmonogram?.Positions == null || !ActiveHarmonogram.Positions.Any())
                return;

            if (isRunning)
                return;

            isRunning = true;

            var positionsToRun = ActiveHarmonogram.Positions
                .Where(x => x.ShouldRun())
                .ToArray();

            foreach (var harmonogramPosition in positionsToRun)
            {
                if (!Enabled)
                    break;

                try
                {
                    await RunHarmonogramPositionAsync(harmonogramPosition);
                }
                catch (Exception ex)
                {
                    CreateErrorReport(harmonogramPosition, ex.Message);
                }
            }

            isRunning = false;
        }
        public async Task RunHarmonogramPositionAsync(HarmonogramPosition harmonogramPosition)
        {
            harmonogramPosition.LastLaunchTime = DateTime.Now;
            OnOperationStarted.Invoke(harmonogramPosition);

            if (harmonogramPosition.HarmonogramOperation == OperationType.ImportZamowien)
            {
                await ExecuteImportAsync(harmonogramPosition);
            }
        }

        public void CreateErrorReport(HarmonogramPosition harmonogramPosition, string error) 
        {
            using (var reportBO = operationReportsManager.Create(harmonogramPosition.HarmonogramOperation))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition.Id;
                reportBO.Data.Description = $"{harmonogramPosition.HarmonogramOperation} {harmonogramPosition.Account.Name} ({harmonogramPosition.Account.AccountType})";
                reportBO.AddError("", error);
                reportBO.Save();
                OnOperationExecuted.Invoke(harmonogramPosition, reportBO.Data);
            }
        }

        private async Task ExecuteImportAsync(HarmonogramPosition harmonogramPosition)
        {
            var account = harmonogramPosition.Account;
            var saleChannelService = account.CreateService();
            var orders = await saleChannelService.GetOrdersAsync();
            var nexoOrdersManager = nexoContext.GetManager<NexoOrdersManager>();

            var executed = 0;
            var succeeded = 0;
            var warns = 0;
            var failed = 0;

            using (var reportBO = operationReportsManager.Create(OperationType.ImportZamowien))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition.Id;
                reportBO.Data.Description = $"Import zamówień {account.Name} ({account.AccountType})";

                foreach (var order in orders)
                {
                    var operationResult = await nexoOrdersManager.AddIfNotExistsAsync(order, account.NexoConfiguration);

                    operationResult.Infos.ForEach(x => reportBO.AddInfo(order.Original, x));
                    operationResult.Warrnings.ForEach(x => reportBO.AddWarn(order.Original, x));
                    operationResult.Errors.ForEach(x => reportBO.AddError(order.Original, x));

                    executed++;

                    if (!operationResult.IsSuccess)
                        failed++;
                    else if (operationResult.Warrnings.Any())
                        warns++;
                    else
                        succeeded++;
                }

                reportBO.Save();
                OnOperationExecuted.Invoke(harmonogramPosition, reportBO.Data);
            }
        }

        private void OperationExecuted(HarmonogramPosition harmonogramPosition, OperationReport report)
        {
            var result = harmonogramsManager.SaveHarmonogramPosition(harmonogramPosition);
        }
        public void Dispose()
        {
            Enabled = false;
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
