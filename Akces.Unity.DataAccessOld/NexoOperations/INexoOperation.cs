using System.Threading.Tasks;

namespace Akces.Unity.DataAccess
{
    public interface INexoOperation<T>
    {
        T Data { get; }
        Task<NexoOperationResult> ExecuteAsync();
    }
}
