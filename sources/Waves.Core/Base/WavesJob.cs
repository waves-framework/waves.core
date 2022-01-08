using System.Threading.Tasks;
using Quartz;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Waves job abstraction.
/// </summary>
public abstract class WavesJob : WavesPlugin, IWavesJob
{
    /// <inheritdoc />
    public abstract Task Execute(IJobExecutionContext context);
}
