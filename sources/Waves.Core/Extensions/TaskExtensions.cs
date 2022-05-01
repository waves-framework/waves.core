using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

    /// <summary>
    /// Log exceptions for task.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="core">Core.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task LogExceptions(this Task task, WavesCore core)
    {
        task.ContinueWith(
            async t =>
            {
                var logger = await core.GetInstanceAsync<Microsoft.Extensions.Logging.ILogger>();

                if (t.Exception == null)
                {
                    return;
                }

                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    logger.LogError(exception, "An error occured");
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);

        return Task.CompletedTask;
    }
}
