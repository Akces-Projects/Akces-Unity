using System.Threading.Tasks;
using System.Threading;
using Akces.Unity.Models;
using System;

namespace Akces.Unity.App.Operations
{
    public delegate void OnOperationFinished(OperationReport operationReport, HarmonogramPosition harmonogramPosition = null);
    public delegate void OnOperationStarted(HarmonogramPosition harmonogramPosition = null);
    public delegate void OnOperationProgress(int progress, string description);

    public interface IUnityOperation : IDisposable
    {
        OnOperationFinished OnOperationExecuted { get; set; }
        OnOperationStarted OnOperationStarted { get; set; }
        OnOperationProgress OnOperationProgress { get; set; }
        OperationReport OperationReport { get; }
        int Processes { get; }
        Task ExecuteAsync(CancellationToken? cancellationToken = null);
    }
}
