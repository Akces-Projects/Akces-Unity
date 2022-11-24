using Akces.Core.Nexo;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.DataAccess.Services;
using Akces.Unity.Models;
using Akces.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akces.Unity.App.Operations
{
    public class ResetOlxTokenTask : IUnityTask
    {
        private readonly Account account;
        private readonly TaskReportsManager taskReportsManager;
        private readonly HarmonogramPosition harmonogramPosition;

        public OnTaskFinished OnTaskExecuted { get; set; }
        public OnTaskProgress OnTaskProgress { get; set; }
        public OnTaskStarted OnTaskStarted { get; set; }
        public TaskReport TaskReport { get; private set; }
        public int Processes { get; private set; }

        public ResetOlxTokenTask(Account account, HarmonogramPosition harmonogramPosition = null)
        {
            this.account = account;
            this.harmonogramPosition = harmonogramPosition;
            taskReportsManager = new TaskReportsManager();
            OnTaskStarted = new OnTaskStarted((e) => { });
            OnTaskProgress = new OnTaskProgress((e, s) => { });
            OnTaskExecuted = new OnTaskFinished((e, s) => { });
        }

        public async Task ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnTaskStarted.Invoke(harmonogramPosition);

            var saleChannelService = account.CreatePartialService();
            var olxService = (OlxService)saleChannelService;
            var result = await olxService.RefreshTokenAsync();
            
            Processes = 1;

            if (result)
            {
                using (var reportBO = taskReportsManager.Create(TaskType.ResetTokenaOlx))
                {
                    reportBO.Data.HarmonogramPositionId = harmonogramPosition?.Id;
                    reportBO.Data.Description = $"Reset tokena olx {account.Name} ({account.AccountType})";

                    reportBO.AddInfo($"{account.Name} ({account.AccountType})", "Poprawnie zresetowano token");

                    reportBO.Save();
                    TaskReport = reportBO.Data;
                    OnTaskExecuted.Invoke(reportBO.Data, harmonogramPosition);
                }
            }
            else
            {
                using (var reportBO = taskReportsManager.Create(TaskType.ResetTokenaOlx))
                {
                    reportBO.Data.HarmonogramPositionId = harmonogramPosition?.Id;
                    reportBO.Data.Description = $"Reset tokena olx {account.Name} ({account.AccountType})";

                    reportBO.AddError($"{account.Name} ({account.AccountType})", "Nie udało się zresetować token olx. Należy ponownie wykonać autektykacji konta");

                    reportBO.Save();
                    TaskReport = reportBO.Data;
                    OnTaskExecuted.Invoke(reportBO.Data, harmonogramPosition);
                }
            }
        }
        public void Dispose()
        {
        }
    }
}
