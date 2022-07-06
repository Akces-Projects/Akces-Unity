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
    public class ImportOrdersOperation : IUnityOperation
    {
        private readonly Account account;
        private readonly OperationReportsManager reportsManager;
        private readonly HarmonogramPosition harmonogramPosition;

        public OnOperationFinished OnOperationExecuted { get; set; }
        public OnOperationProgress OnOperationProgress { get; set; }
        public OnOperationStarted OnOperationStarted { get; set; }
        public OperationReport OperationReport { get; private set; }
        public bool SaveReport { get; private set; }
        public int Processes { get; private set; }

        public ImportOrdersOperation(Account account, bool saveReport, HarmonogramPosition harmonogramPosition = null)
        {
            SaveReport = saveReport;
            this.account = account;
            this.harmonogramPosition = harmonogramPosition;
            reportsManager = new OperationReportsManager();
            OnOperationStarted = new OnOperationStarted((e) => { });
            OnOperationProgress = new OnOperationProgress((e, s) => { });
            OnOperationExecuted = new OnOperationFinished((e, s) => { });
        }

        public async Task ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnOperationStarted.Invoke(harmonogramPosition);

            var saleChannelService = account.CreateService();
            var orders = await saleChannelService.GetOrdersAsync();
            var nexoOrdersManager = ServicesProvider.GetService<NexoContext>().GetManager<NexoOrdersManager>();

            Processes = orders.Count;

            var executed = 0;
            var succeeded = 0;
            var warns = 0;
            var failed = 0;

            using (var reportBO = reportsManager.Create(OperationType.ImportZamowien))
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

                if (SaveReport)
                    reportBO.Save();

                OperationReport = reportBO.Data;
                OnOperationExecuted.Invoke(reportBO.Data, harmonogramPosition);
            }
        }
        public void Dispose() 
        {
        }
    }
}
