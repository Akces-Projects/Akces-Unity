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

        public OnTaskFinished OnTaskExecuted { get; set; }
        public OnTaskProgress OnTaskProgress { get; set; }
        public OnTaskStarted OnTaskStarted { get; set; }
        public TaskReport TaskReport { get; private set; }
        public int Processes { get; private set; }

        public UpdateProductsPricesTask(Account account, List<Product> products, HarmonogramPosition harmonogramPosition = null)
        {
            this.account = account;
            this.products = products;
            this.harmonogramPosition = harmonogramPosition;
            reportsManager = new TaskReportsManager();
            OnTaskStarted = new OnTaskStarted((e) => { });
            OnTaskProgress = new OnTaskProgress((e,s) => { });
            OnTaskExecuted = new OnTaskFinished((e,s) => { });
            Processes = products.Count;
            saleChannelService = account.CreateMainService();
        }

        public async Task ExecuteAsync(CancellationToken? cancellationToken)
        {
            if (harmonogramPosition != null)
                harmonogramPosition.LastLaunchTime = DateTime.Now;

            OnTaskStarted.Invoke(harmonogramPosition);

            using (var reportBO = reportsManager.Create(TaskType.WyslanieCen))
            {
                reportBO.Data.HarmonogramPositionId = harmonogramPosition?.Id;
                reportBO.Data.Description = $"Aktualizacja cen {account.Name} ({account.AccountType})";
                var progress = 0;
                var description = string.Empty;

                foreach (var product in products)
                {
                    if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                        break;

                    progress++;
                    OnTaskProgress.Invoke(progress, $"Aktualizacja ceny produktu [{product.Id}]");

                    try
                    {
                        await saleChannelService.UpdateProductPriceAsync(product.Id, product.Currency, product.Price);
                        description = $"Zaktualizowano cenę produktu: {product.Id} - {product.Name} [{product.Symbol}] - {product.OriginalPrice} => {product.Price} [{product.Currency}]";
                        reportBO.AddInfo(product.Id, description);
                    }
                    catch (Exception e)
                    {
                        description = $"Wystąpił błąd: {e.Message}";
                        reportBO.AddError(product.Id, description);
                    }
                }

                reportBO.Save();
                TaskReport = reportBO.Data;
                OnTaskExecuted.Invoke(reportBO.Data, harmonogramPosition);
            }
        }
        public void Dispose() 
        {
            saleChannelService?.Dispose();
        }
    }
}
