using System.Threading.Tasks;
using InsERT.Moria.Sfera;
using Akces.Unity.Models;
using Akces.Unity.Models.Communication;
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

        public async Task<OperationResult> AddIfNotExistsAsync(Order order, NexoConfiguration configuration)
        {
            var objectName = $"Zamówienie {order.SourceSaleChannelName} {order.Original}";

            var addContractorOperation = new AddContractorOperation(order.Purchaser, sfera, configuration);
            var operationResult1 = await addContractorOperation.ExecuteAsync();

            if (!operationResult1.IsSuccess)
            {
                operationResult1.ObjectName = order.Original + "/" + operationResult1.ObjectName;
                return operationResult1;
            }

            var addOrderOperation = new AddOrderOperation(order, sfera, configuration);
            var operationResult2 = await addOrderOperation.ExecuteAsync();

            return operationResult2;
        }
    }
}
