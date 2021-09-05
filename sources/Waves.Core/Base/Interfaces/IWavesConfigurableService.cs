namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for configurable services.
    /// </summary>
    public interface IWavesConfigurableService :
        IWavesService,
        IWavesConfigurablePlugin
    {
    }
}
