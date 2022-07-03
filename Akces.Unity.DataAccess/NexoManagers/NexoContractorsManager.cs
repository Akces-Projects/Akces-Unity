using System.Threading.Tasks;
using InsERT.Moria.Sfera;
using Akces.Unity.Models;
using Akces.Unity.Models.Communication;
using Akces.Unity.DataAccess.NexoManagers.Operations;

namespace Akces.Unity.DataAccess.NexoManagers
{
    public class NexoContractorsManager
    {
        private readonly Uchwyt sfera;

        public NexoContractorsManager(Uchwyt sfera)
        {
            this.sfera = sfera;
        }

        public async Task<OperationResult> AddIfNotExistsAsync(Contractor contractor, NexoConfiguration configuration)
        {
            var addContractorOperation = new AddContractorOperation(contractor, sfera);
            var operationResult = await addContractorOperation.ExecuteAsync();

            return operationResult;
        }
    }
}
