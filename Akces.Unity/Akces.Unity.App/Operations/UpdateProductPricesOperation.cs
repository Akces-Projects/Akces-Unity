using System;
using System.Threading.Tasks;
using Akces.Unity.Models;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.Models.Communication;
using System.Collections.Generic;
using System.Threading;

namespace Akces.Unity.App.Operations
{
    public class UpdateProductsPricesOperation : IUnityOperation
    {
        private readonly Account account;
        private readonly OperationReportsManager reportsManager;
        private readonly HarmonogramPosition harmonogramPosition;
        private readonly List<Product> products;

        public OnOperationFinished OnOperationExecuted { get; set; }
        public OnOperationProgress OnOperationProgress { get; set; }
        public OnOperationStarted OnOperationStarted { get; set; }
        public OperationReport OperationReport { get; private set; }
        public bool SaveReport { get; private set; }
        public int Processes { get; private set; }

        public UpdateProductsPricesOperation(Account account, bool saveReport, List<Product> products, HarmonogramPosition harmonogramPosition = null)
        {
            SaveReport = saveReport;
            this.products = products;
            this.account = account;
            this.harmonogramPosition = harmonogramPosition;
            Processes = products.Count;
            reportsManager = new OperationReportsManager();
            OnOperationStarted = new OnOperationStarted((e) => { });
            OnOperationProgress = new OnOperationProgress((e,s) => { });
            OnOperationExecuted = new OnOperationFinished((e,s) => { });
        }

        public async Task ExecuteAsync(CancellationToken? cancellationToken)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnOperationStarted.Invoke(harmonogramPosition);

            var saleChannelService = account.CreateService();

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

                        var result = await saleChannelService.UpdateProductPriceAsync(product.Id, product.Currency, product.Price);

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

                if (SaveReport)
                    reportBO.Save();

                OperationReport = reportBO.Data;
                OnOperationExecuted.Invoke(reportBO.Data, harmonogramPosition);
            }
        }
        public void Dispose() { }
    }
}
