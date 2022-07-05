using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akces.Unity.Models.Communication;

namespace Akces.Unity.Models
{
    public interface ISaleChannelService : IDisposable
    {
        AccountType SaleChannelType { get; }
        Task<List<Order>> GetOrdersAsync();
        Task<ProductsContainer> GetProductsAsync(int pageIndex);
        Task<Order> GetOrderAsync(object id);
        Task<bool> UpdateOrderAsync(object id, Order orderToUpdate);
        Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice);
        Task<bool> AuthenticateAsync();
        Task<bool> TestConnectionAsync();
        void SaveConfiguration();
    }
}
