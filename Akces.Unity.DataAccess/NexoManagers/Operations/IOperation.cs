using System.Threading.Tasks;

namespace Akces.Unity.DataAccess.NexoManagers.Operations
{
    public interface IOperation<T>
    {
        T Data { get; }
        Task<OperationResult> ExecuteAsync();
    }
}
