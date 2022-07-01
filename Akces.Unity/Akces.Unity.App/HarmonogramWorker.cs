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
    public delegate void OnOperationExecuted(HarmonogramPosition harmonogramPosition, int executed, int succeeded, int warns, int failed);
    internal class HarmonogramWorker : IDisposable
    {
        private const int WORKER_RUN_INTERVAL_SECONDS = 10;

        private bool isRunning;
        private bool workerStopped;
        private readonly Timer timer;
        private readonly NexoContext nexoContext;
        private readonly HarmonogramsManager harmonogramsManager;
        private readonly OperationReportsManager operationReportsManager;

        public Harmonogram ActiveHarmonogram { get; private set; }
        public OnOperationExecuted OnOrdersImported { get; set; }

        public HarmonogramWorker()
        {
            harmonogramsManager = new HarmonogramsManager();
            operationReportsManager = new OperationReportsManager();
            nexoContext = ServicesProvider.GetService<NexoContext>();
            OnOrdersImported = new OnOperationExecuted((i, a, d, n, f) => { });

            ActiveHarmonogram = GetActiveHarmonogram();

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
            var fullHarmonogram = harmonogramsManager.Get(harmonogram.Id);
            return fullHarmonogram;
        }
        public void SetActiveHarmonogram(Harmonogram harmonogram)
        {
            workerStopped = true;

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
            }

            workerStopped = false;
        }
        public async Task RunHarmonogramPositionAsync(HarmonogramPosition harmonogramPosition)
        {
            if (harmonogramPosition.HarmonogramOperation == OperationType.ImportZamowien)
            {
                await ExecuteImportAsync(harmonogramPosition);
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
                if (workerStopped)
                    return;

                await RunHarmonogramPositionAsync(harmonogramPosition);
            }

            isRunning = false;
        }
        private async Task ExecuteImportAsync(HarmonogramPosition harmonogramPosition)
        {
            var account = harmonogramPosition.Account;
            var saleChannelService = account.CreateService();
            var orders = await saleChannelService.GetOrdersAsync(DateTime.Now.AddHours(-1000));
            var nexoOrdersManager = nexoContext.GetManager<NexoOrdersManager>();

            var executed = 0;
            var succeeded = 0;
            var warns = 0;
            var failed = 0;

            foreach (var order in orders)
            {
                var operationResult = await nexoOrdersManager.AddIfNotExistsAsync(order, account.NexoConfiguration);

                using (var reportBO = operationReportsManager.Create(OperationType.ImportZamowien)) 
                {
                    reportBO.Data.HarmonogramPositionId = harmonogramPosition.Id;
                    reportBO.Data.ObjectName = operationResult.ObjectName;

                    operationResult.Infos.ForEach(x => reportBO.AddInfo(x));
                    operationResult.Warrnings.ForEach(x => reportBO.AddWarn(x));
                    operationResult.Errors.ForEach(x => reportBO.AddError(x));

                    reportBO.Save();
                }

                executed++;

                if (!operationResult.IsSuccess)
                    failed++;
                else if (operationResult.Warrnings.Any())
                    warns++;
                else
                    succeeded++;
            }

            OnOrdersImported.Invoke(harmonogramPosition, executed, succeeded, warns, failed);
        }
        public void Dispose()
        {
            workerStopped = true;
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
