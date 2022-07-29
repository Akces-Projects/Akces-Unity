using System.Linq;
using System.Threading.Tasks;
using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Unity.Models;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using System.Threading;
using System;

namespace Akces.Unity.App.Operations
{
    public class ImportOrdersTask : IUnityTask
    {
        private readonly Account account;
        private readonly TaskReportsManager taskReportsManager;
        private readonly HarmonogramPosition harmonogramPosition;

        public OnTaskFinished OnTaskExecuted { get; set; }
        public OnTaskProgress OnTaskProgress { get; set; }
        public OnTaskStarted OnTaskStarted { get; set; }
        public TaskReport TaskReport { get; private set; }
        public int Processes { get; private set; }

        public ImportOrdersTask(Account account, HarmonogramPosition harmonogramPosition = null)
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

            var saleChannelService = account.CreateMainService();
            var orders = await saleChannelService.GetOrdersAsync();
            var nexoOrdersManager = ServicesProvider.GetService<NexoContext>().GetManager<NexoOrdersManager>();

            Processes = orders.Count;

            var executed = 0;
            var succeeded = 0;
            var warns = 0;
            var failed = 0;

            using (var reportBO = taskReportsManager.Create(TaskType.ImportZamowien))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition?.Id;
                reportBO.Data.Description = $"Import zamówień {account.Name} ({account.AccountType})";

                foreach (var order in orders)
                {
                    if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                        break;

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

                TaskReport = reportBO.Data;
                OnTaskExecuted.Invoke(reportBO.Data, harmonogramPosition);
            }
        }
        public void Dispose() 
        {
        }
    }
}
