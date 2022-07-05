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
using Akces.Unity.App.Operations;

namespace Akces.Unity.App
{

    internal class HarmonogramWorker : IDisposable
    {
        private const int WORKER_RUN_INTERVAL_SECONDS = 10;

        private bool isRunning;
        private readonly Timer timer;
        private readonly HarmonogramsManager harmonogramsManager;
        private readonly OperationReportsManager reportsManager;

        public bool Enabled { get; set; }
        public Harmonogram ActiveHarmonogram { get; private set; }
        public OnOperationStarted OnOperationStarted { get; set; }
        public OnOperationFinished OnOperationFinished { get; set; }

        public HarmonogramWorker()
        {
            harmonogramsManager = new HarmonogramsManager();
            reportsManager = new OperationReportsManager();

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
            IUnityOperation unityOperation = null;

            if (harmonogramPosition.HarmonogramOperation == OperationType.ImportZamowien)
                unityOperation = new ImportOrdersOperation(harmonogramPosition.Account, true, harmonogramPosition);

            if (unityOperation == null)
                return;

            harmonogramPosition.LastLaunchTime = DateTime.Now;
            harmonogramsManager.SaveHarmonogramPosition(harmonogramPosition);

            OnOperationStarted.Invoke(harmonogramPosition);
            await unityOperation.ExecuteAsync();
            OnOperationFinished.Invoke(unityOperation.OperationReport, harmonogramPosition);
        }

        public void CreateErrorReport(HarmonogramPosition harmonogramPosition, string error) 
        {
            using (var reportBO = reportsManager.Create(harmonogramPosition.HarmonogramOperation))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition.Id;
                reportBO.Data.Description = $"{harmonogramPosition.HarmonogramOperation} {harmonogramPosition.Account.Name} ({harmonogramPosition.Account.AccountType})";
                reportBO.AddError("", error);
                reportBO.Save();
            }
        }
        public void Dispose()
        {
            Enabled = false;
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
