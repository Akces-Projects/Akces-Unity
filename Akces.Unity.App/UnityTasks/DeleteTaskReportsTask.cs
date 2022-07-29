using System.Threading.Tasks;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using System.Threading;
using System;
using System.Collections.Generic;

namespace Akces.Unity.App.Operations
{
    public class DeleteTaskReportsTask : IUnityTask
    {
        private readonly List<TaskReport> taskReports;
        private readonly TaskReportsManager taskReportsManager;
        private readonly HarmonogramPosition harmonogramPosition;

        public OnTaskFinished OnTaskExecuted { get; set; }
        public OnTaskProgress OnTaskProgress { get; set; }
        public OnTaskStarted OnTaskStarted { get; set; }
        public TaskReport TaskReport { get; private set; }
        public int Processes { get; private set; }


        public DeleteTaskReportsTask(List<TaskReport> taskReports, HarmonogramPosition harmonogramPosition = null)
        {
            this.taskReports = taskReports;
            this.harmonogramPosition = harmonogramPosition;
            taskReportsManager = new TaskReportsManager();
            Processes = taskReports.Count;
            OnTaskStarted = new OnTaskStarted((e) => { });
            OnTaskProgress = new OnTaskProgress((e, s) => { });
            OnTaskExecuted = new OnTaskFinished((e, s) => { });
        }
        public async Task ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnTaskStarted.Invoke(harmonogramPosition);

            var progress = 0;

            foreach (var taskReport in taskReports)
            {
                if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                    break;

                progress++;
                OnTaskProgress.Invoke(progress, $"Usuwanie raportu \"{taskReport.OperationType}\" [{taskReport.Id}]");

                await Task.Run(() =>
                {
                    using (var taskReportOB = taskReportsManager.Find(taskReport))
                    {
                        taskReportOB.Delete();
                    }
                });
            }

            OnTaskExecuted.Invoke(null, harmonogramPosition);
        }
        public void Dispose()
        {
            
        }
    }
}
