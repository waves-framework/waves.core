using System;
using System.Threading.Tasks;

namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Fires and forget task.
    /// </summary>
    /// <param name="task">Task.</param>
    public static void FireAndForget(this Task task)
    {
        task.FireAndForget(null);
    }

    /// <summary>
    /// Fires and forget task with exception handling.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="onError">Action when error.</param>
    public static async void FireAndForget(this Task task, Action<Exception> onError)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }
    }
}
