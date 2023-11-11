using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.ConcurrentUniTaskHandler
{
    public struct ProcessTask<TResult>
    {
        public Func<CancellationToken, UniTask> WaitTask { get; }
        public Func<CancellationToken, UniTask<TResult>> OnPassedTask { get; }

        internal ProcessTask(Func<CancellationToken, UniTask> waitTask, Func<CancellationToken, UniTask<TResult>> onPassedTask)
        {
            WaitTask = waitTask;
            OnPassedTask = onPassedTask;
        }
    }
    
    public struct ProcessTask
    {
        public static ProcessTask<TResult> Create<TResult>(Func<CancellationToken, UniTask> waitTask, Func<CancellationToken, UniTask<TResult>> onPassedTask)
        {
            return new ProcessTask<TResult>(waitTask, onPassedTask);
        }
    }
}
