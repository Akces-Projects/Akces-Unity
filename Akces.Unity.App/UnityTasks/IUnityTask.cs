using System.Threading.Tasks;
using System.Threading;
using Akces.Unity.Models;
using System;

namespace Akces.Unity.App.Operations
{
    public delegate void OnOperationFinished(TaskReport operationReport, HarmonogramPosition harmonogramPosition = null);
    public delegate void OnOperationStarted(HarmonogramPosition harmonogramPosition = null);
    public delegate void OnOperationProgress(int progress, string description);

    public interface IUnityTask : IDisposable
    {
        OnOperationFinished OnOperationExecuted { get; set; }
        OnOperationStarted OnOperationStarted { get; set; }
        OnOperationProgress OnOperationProgress { get; set; }
        TaskReport TaskReport { get; }
        int Processes { get; }
        Task ExecuteAsync(CancellationToken? cancellationToken = null);
    }
}
