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
        private readonly AccountsManager accountsManager;
        private readonly HarmonogramsManager harmonogramsManager;
        private readonly TaskReportsManager taskReportsManager;
        private readonly WorkerStatusesManager workerStatusesManager;
        private readonly UnityUser loggedUnityUser;

        public bool Enabled { get; set; }
        public OnTaskStarted OnOperationStarted { get; set; }
        public OnTaskFinished OnOperationFinished { get; set; }

        public HarmonogramWorker()
        {
            accountsManager = new AccountsManager();
            workerStatusesManager = new WorkerStatusesManager();
            harmonogramsManager = new HarmonogramsManager();
            taskReportsManager = new TaskReportsManager();
            loggedUnityUser = GetLoggedUnityUser();
            Enabled = workerStatusesManager.GetCurrent()?.Enabled ?? false;

            if (loggedUnityUser.IsWorker) 
            {
                timer = new Timer();
                timer.Enabled = true;
                timer.Enabled = true;
                timer.Interval = 1000 * WORKER_RUN_INTERVAL_SECONDS;
                timer.Elapsed += OnTimerElapsed;
            }
        }

        public void Start() 
        {
            workerStatusesManager.StartWorker(loggedUnityUser);
            Enabled = workerStatusesManager.GetCurrent()?.Enabled ?? false;
        }
        public void Stop()
        {
            workerStatusesManager.StopWorker(loggedUnityUser);
            Enabled = workerStatusesManager.GetCurrent()?.Enabled ?? false;
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var activeHarmonogram = GetActiveHarmonogram();

            if (activeHarmonogram?.Positions == null || !activeHarmonogram.Positions.Any())
                return;

            if (isRunning)
                return;

            isRunning = true;

            foreach (var harmonogramPosition in activeHarmonogram.Positions.Where(x => x.ShouldRun()))
            {
                if (!Enabled)
                    break;

                await RunHarmonogramPositionAsync(harmonogramPosition);
            }

            isRunning = false;
        }
        private async Task RunHarmonogramPositionAsync(HarmonogramPosition harmonogramPosition)
        {
            try
            {
                IUnityTask unityOperation = null;

                if (harmonogramPosition.HarmonogramOperation == TaskType.ImportZamowien)
                {
                    var account = accountsManager.Get(harmonogramPosition.Account.Id);
                    unityOperation = new ImportOrdersTask(account, harmonogramPosition);
                }

                if (unityOperation == null)
                    return;

                harmonogramPosition.LastLaunchTime = DateTime.Now;
                harmonogramsManager.SaveHarmonogramPosition(harmonogramPosition);

                OnOperationStarted.Invoke(harmonogramPosition);
                await unityOperation.ExecuteAsync();
                OnOperationFinished.Invoke(unityOperation.TaskReport, harmonogramPosition);
            }
            catch (Exception ex)
            {
                var errorReport = CreateErrorReport(harmonogramPosition, ex.Message); 
                OnOperationFinished.Invoke(errorReport, harmonogramPosition);
            }
        }
        private Harmonogram GetActiveHarmonogram()
        {
            var harmonogram = harmonogramsManager.Get().FirstOrDefault(x => x.Active);

            if (harmonogram == null)
                return null;

            var fullHarmonogram = harmonogramsManager.Get(harmonogram.Id);
            return fullHarmonogram;
        }
        private UnityUser GetLoggedUnityUser()
        {
            var nexoContext = ServicesProvider.GetService<NexoContext>();
            var unityUsersManager = new UnityUsersManager();
            var unityUser = unityUsersManager.Get().FirstOrDefault(x => x.Login == nexoContext.NexoUser.Login);
            return unityUser;
        }
        private TaskReport CreateErrorReport(HarmonogramPosition harmonogramPosition, string error) 
        {
            using (var reportBO = taskReportsManager.Create(harmonogramPosition.HarmonogramOperation))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition.Id;
                reportBO.Data.Description = $"{harmonogramPosition.HarmonogramOperation} {harmonogramPosition.Account.Name} ({harmonogramPosition.Account.AccountType})";
                reportBO.AddError("", error);
                reportBO.Save();

                return reportBO.Data;
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
