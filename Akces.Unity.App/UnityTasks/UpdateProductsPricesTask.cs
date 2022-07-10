using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.Operations
{
    public class UpdateProductsPricesTask : IUnityTask
    {
        private readonly Account account;
        private readonly TaskReportsManager reportsManager;
        private readonly HarmonogramPosition harmonogramPosition;
        private readonly List<Product> products;
        private readonly ISaleChannelService saleChannelService;

        public OnOperationFinished OnOperationExecuted { get; set; }
        public OnOperationProgress OnOperationProgress { get; set; }
        public OnOperationStarted OnOperationStarted { get; set; }
        public TaskReport TaskReport { get; private set; }
        public int Processes { get; private set; }

        public UpdateProductsPricesTask(Account account, List<Product> products, HarmonogramPosition harmonogramPosition = null)
        {
            this.account = account;
            this.products = products;
            this.harmonogramPosition = harmonogramPosition;
            reportsManager = new TaskReportsManager();
            OnOperationStarted = new OnOperationStarted((e) => { });
            OnOperationProgress = new OnOperationProgress((e,s) => { });
            OnOperationExecuted = new OnOperationFinished((e,s) => { });
            Processes = products.Count;
            saleChannelService = account.CreateService();
        }

        public async Task ExecuteAsync(CancellationToken? cancellationToken)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnOperationStarted.Invoke(harmonogramPosition);

            using (var reportBO = reportsManager.Create(OperationType.WyslanieCen))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition?.Id;
                reportBO.Data.Description = $"Aktualizacja cen {account.Name} ({account.AccountType})";
                var progress = 0;
                var description = string.Empty;

                foreach (var product in products)
                {
                    try
                    {
                        if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                            break;

                        await saleChannelService.UpdateProductPriceAsync(product.Id, product.Currency, product.Price);
                        description = $"Zaktualizowano cenę produktu {product.Name} ({product.Id}) - {product.Price} {product.Currency}";
                        reportBO.AddInfo(product.Id, description);
                    }
                    catch (Exception e)
                    {
                        description = $"Wystąpił błąd: {e.Message}";
                        reportBO.AddError(product.Id, description);
                    }

                    progress++;
                    OnOperationProgress.Invoke(progress, description);
                }

                reportBO.Save();
                TaskReport = reportBO.Data;
                OnOperationExecuted.Invoke(reportBO.Data, harmonogramPosition);
            }
        }
        public void Dispose() 
        {
            saleChannelService?.Dispose();
        }
    }
}
