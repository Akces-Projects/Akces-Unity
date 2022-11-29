using Akces.Unity.DataAccess.Managers;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akces.Unity.DataAccess.SaleChannelsServices
{
    internal class MainSaleChannelService : ISaleChannelService
    {
        private readonly Account account;
        private readonly ISaleChannelService saleChannelService;
        private readonly AccountFunctionsManager accountFunctionsManager;
        public AccountType SaleChannelType => account.AccountType;

        public MainSaleChannelService(Account account)
        {
            this.account = account;
            this.saleChannelService = account.CreatePartialService();
            this.accountFunctionsManager = new AccountFunctionsManager();
        }

        public async Task<Order> GetOrderAsync(object id)
        {
            var order = await saleChannelService.GetOrderAsync(id);

            var func1 = accountFunctionsManager.GetCalculateOrderPositionQuantityScriptMethod(account.Id);
            var func2 = accountFunctionsManager.GetConcludeProductSymbolMethod(account.Id);

            foreach (var product in order.Products)
            {
                try
                {
                    var fullPrice = product.Price * product.Quantity;
                    product.Quantity = func1.Invoke(product);
                    product.Symbol = func2.Invoke(product);
                    product.Price = Math.Round(fullPrice / product.RepeatPosition / (product.Quantity == 0 ? 1 : product.Quantity), 2, MidpointRounding.AwayFromZero);
                }
                catch
                {
                }
            }

            return order;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await saleChannelService.GetOrdersAsync();

            var func1 = accountFunctionsManager.GetCalculateOrderPositionQuantityScriptMethod(account.Id);
            var func2 = accountFunctionsManager.GetConcludeProductSymbolMethod(account.Id);

            foreach (var order in orders)
            {

                foreach (var product in order.Products)
                {
                    try
                    {
                        var fullPrice = product.Price * product.Quantity;
                        product.Quantity = func1.Invoke(product);
                        product.Symbol = func2.Invoke(product);
                        product.Price = Math.Round(fullPrice / product.RepeatPosition / (product.Quantity == 0 ? 1 : product.Quantity), 2, MidpointRounding.AwayFromZero);
                    }
                    catch
                    {
                    }
                }
            }

            return orders;
        }

        public async Task<ProductsContainer> GetProductsAsync(bool all, int pageIndex = 0)
        {
            var products = await saleChannelService.GetProductsAsync(all, pageIndex);

            var func2 = accountFunctionsManager.GetConcludeProductSymbolMethod(account.Id);

            foreach (var product in products.Products)
            {
                product.Symbol = func2.Invoke(product);
            }

            return products;
        }

        public Task<bool> AuthenticateAsync() => saleChannelService.AuthenticateAsync();
        public void SaveConfiguration() => saleChannelService.SaveConfiguration();
        public Task<bool> UpdateOrderAsync(object id, Order orderToUpdate) => saleChannelService.UpdateOrderAsync(id, orderToUpdate);
        public Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice) => saleChannelService.UpdateProductPriceAsync(id, currency, newPrice);
        public Task<bool> ValidateConnectionAsync() => saleChannelService.ValidateConnectionAsync();
        public void Dispose() => saleChannelService.Dispose();
    }
}
