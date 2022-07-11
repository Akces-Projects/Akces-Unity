using System.Threading.Tasks;
using System.Threading;
using Akces.Unity.Models;
using System;

namespace Akces.Unity.App.Operations
{
    public delegate void OnTaskFinished(TaskReport taskReport, HarmonogramPosition harmonogramPosition = null);
    public delegate void OnTaskStarted(HarmonogramPosition harmonogramPosition = null);
    public delegate void OnTaskProgress(int progress, string description);

    public interface IUnityTask : IDisposable
    {
        OnTaskFinished OnTaskExecuted { get; set; }
        OnTaskStarted OnTaskStarted { get; set; }
        OnTaskProgress OnTaskProgress { get; set; }
        TaskReport TaskReport { get; }
        int Processes { get; }
        Task ExecuteAsync(CancellationToken? cancellationToken = null);
    }
}
