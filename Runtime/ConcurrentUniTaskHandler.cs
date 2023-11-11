using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.ConcurrentUniTaskHandler
{
    public readonly ref struct ConcurrentUniTaskHandler<TResult>
    {
        private Span<ProcessTask<TResult>> ProcessTasks { get; }
        
        internal ConcurrentUniTaskHandler(params ProcessTask<TResult>[] processTasks)
        {
            ProcessTasks = new Span<ProcessTask<TResult>>(processTasks);
        }

        public async UniTask<TResult> ProcessFirstCompletedTaskAsync(CancellationToken cancellationToken = default)
        {
            CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            UniTask[] tasks = new UniTask[ProcessTasks.Length];
            for (int i = 0; i < ProcessTasks.Length; i++)
            {
                tasks[i] = ProcessTasks[i].WaitTask(cancellationTokenSource.Token);
            }

            var passedTaskIndex = await UniTask.WhenAny(tasks);
            cancellationTokenSource.Cancel();
            return await ProcessTasks[passedTaskIndex].OnPassedTask(cancellationToken);
        }

        public async UniTask<TResult> LoopProcessFirstCompletedTaskAsync(Func<TResult, bool> checkNeedLoop, CancellationToken cancellationToken = default)
        {
            TResult result = default;
            bool needLoop = true;
            do
            {
                result = await ProcessFirstCompletedTaskAsync(cancellationToken);
                needLoop = checkNeedLoop(result);
            } while (needLoop);

            return result;
        }
    }
    
    public struct ConcurrentUniTaskHandler
    {
        public static ConcurrentUniTaskHandler<TResult> Create<TResult>(params ProcessTask<TResult>[] processTasks)
        {
            return new ConcurrentUniTaskHandler<TResult>(processTasks);
        }
    }
}
