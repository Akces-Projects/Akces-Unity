using System.Threading.Tasks;

namespace Akces.Unity.DataAccess
{
    public interface IOperation<T>
    {
        T Data { get; }
        Task<OperationResult> ExecuteAsync();
    }
}
