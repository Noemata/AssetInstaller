using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace AssetInstaller
{
    /// <summary>
    /// 'IAsyncAction' Is said to be defined in an unreferenced assembly, so define it yourself
    /// https://www.moonmile.net/blog/archives/8584
    /// </summary>
    public static class TaskEx
    {
        public static Task<T> AsTask<T>(this IAsyncOperation<T> operation)
        {
            var tcs = new TaskCompletionSource<T>();

            operation.Completed = delegate  //--- Set callback
            {
                switch (operation.Status)   //--- Completion notification according to the status
                {
                    case AsyncStatus.Completed: tcs.SetResult(operation.GetResults()); break;
                    case AsyncStatus.Error: tcs.SetException(operation.ErrorCode); break;
                    case AsyncStatus.Canceled: tcs.SetCanceled(); break;
                }
            };

            return tcs.Task;  //--- Returns a Task that is notified of completion
        }

        public static TaskAwaiter<T> GetAwaiter<T>(this IAsyncOperation<T> operation)
        {
            return operation.AsTask().GetAwaiter();
        }
    }
}
