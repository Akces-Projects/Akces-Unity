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
        private readonly TaskReportsManager taskReportsManager;
        private readonly WorkerStatusesManager workerStatusesManager;
        private readonly UnityUser loggedUnityUser;

        public bool Enabled { get; set; }
        public OnTaskStarted OnOperationStarted { get; set; }
        public OnTaskFinished OnOperationFinished { get; set; }

        public HarmonogramWorker()
        {
            workerStatusesManager = new WorkerStatusesManager();
            harmonogramsManager = new HarmonogramsManager();
            taskReportsManager = new TaskReportsManager();
            loggedUnityUser = GetLoggedUnityUser();
            
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
        }
        public void Stop()
        {
            workerStatusesManager.StopWorker(loggedUnityUser);
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var activeHarmonogram = GetActiveHarmonogram();

            if (activeHarmonogram?.Positions == null || !activeHarmonogram.Positions.Any())
                return;

            Enabled = activeHarmonogram?.WorkerEnabled ?? false;

            if (isRunning)
                return;

            isRunning = true;

            foreach (var harmonogramPosition in activeHarmonogram.Positions.Where(x => x.ShouldRun()))
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
        private async Task RunHarmonogramPositionAsync(HarmonogramPosition harmonogramPosition)
        {
            IUnityTask unityOperation = null;

            if (harmonogramPosition.HarmonogramOperation == TaskType.ImportZamowien)
                unityOperation = new ImportOrdersTask(harmonogramPosition.Account, harmonogramPosition);

            if (unityOperation == null)
                return;

            harmonogramPosition.LastLaunchTime = DateTime.Now;
            harmonogramsManager.SaveHarmonogramPosition(harmonogramPosition);

            OnOperationStarted.Invoke(harmonogramPosition);
            await unityOperation.ExecuteAsync();
            OnOperationFinished.Invoke(unityOperation.TaskReport, harmonogramPosition);
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
        private void CreateErrorReport(HarmonogramPosition harmonogramPosition, string error) 
        {
            using (var reportBO = taskReportsManager.Create(harmonogramPosition.HarmonogramOperation))
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
