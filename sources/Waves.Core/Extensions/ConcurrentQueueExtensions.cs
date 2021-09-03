using System.Collections.Concurrent;

namespace Waves.Core.Extensions
{
    /// <summary>
    /// ConcurrentQueue extensions.
    /// </summary>
    public static class ConcurrentQueueExtensions
    {
        /// <summary>
        /// Clear ConcurrentQueue.
        /// </summary>
        /// <param name="queue">ConcurrentQueue.</param>
        /// <typeparam name="T">Type.</typeparam>
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out _))
            {
            }
        }
    }
}