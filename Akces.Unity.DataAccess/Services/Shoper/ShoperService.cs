using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        public Task<List<Order>> GetOrdersAsync(DateTime from)
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
