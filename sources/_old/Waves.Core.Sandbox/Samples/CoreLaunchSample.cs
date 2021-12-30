using System.Threading.Tasks;
using Waves.Core.Sandbox.Samples.Interfaces;

namespace Waves.Core.Sandbox.Samples
{
    /// <summary>
    ///     Sample of core start / stop.
    /// </summary>
    public class CoreLaunchSample : ISample
    {
        /// <inheritdoc />
        public async void Execute()
        {
            // Create new instance, start and build container.
            var core = new Old.Core();
            await core.StartAsync();
            await core.BuildContainerAsync();

            // After a short pause stop core.
            await Task.Delay(5000);
            await core.StopAsync();
        }
    }
}
