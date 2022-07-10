using System.Threading.Tasks;
using InsERT.Moria.Sfera;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.NexoManagers.Operations;

namespace Akces.Unity.DataAccess.NexoManagers
{
    public class NexoOrdersManager
    {
        private readonly Uchwyt sfera;

        public NexoOrdersManager(Uchwyt sfera)
        {
            this.sfera = sfera;
        }

        public async Task<NexoOperationResult> AddIfNotExistsAsync(Order order, NexoConfiguration configuration)
        {
            var objectName = $"Zamówienie {order.SourceSaleChannelName} {order.Original}";

            var addContractorOperation = new AddContractorOperation(order.Purchaser, sfera);
            var addContractorOperationResult = await addContractorOperation.ExecuteAsync();

            if (!addContractorOperationResult.IsSuccess)
            {
                addContractorOperationResult.ObjectName = objectName;
                return addContractorOperationResult;
            }

            var addOrderOperation = new AddOrderOperation(order, sfera, configuration);
            var addOrderOperationOperationResult = await addOrderOperation.ExecuteAsync();
            addOrderOperationOperationResult.ObjectName = objectName;

            return addOrderOperationOperationResult;
        }
    }
}
