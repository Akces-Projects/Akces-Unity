using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.Models.Communication;
using Akces.Unity.Models.SaleChannels.Shoper;

namespace Akces.Unity.DataAccess.Services
{
    public class ShoperService : ISaleChannelService
    {
        private readonly ShoperConfiguration shoperConfiguration;
        public AccountType SaleChannelType { get => AccountType.Shoper; }


        public ShoperService(ShoperConfiguration shoperConfiguration)
        {
            this.shoperConfiguration = shoperConfiguration;
        }

        public Task<Order> GetOrderAsync(object id)
        {
            throw new NotImplementedException();
        }
        public Task<List<Order>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateOrderAsync(object id, Order orderToUpdate)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        public Task<bool> TestConnectionAsync()
        {
            throw new NotImplementedException();
        }
        public void SaveConfiguration()
        {
            if (shoperConfiguration == null || shoperConfiguration.Id == default)
                return;

            using (var context = new UnityDbContext())
            {
                context.Set<ShoperConfiguration>().Update(shoperConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ProductsContainer> GetProductsAsync(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice)
        {
            throw new NotImplementedException();
        }
    }
}
