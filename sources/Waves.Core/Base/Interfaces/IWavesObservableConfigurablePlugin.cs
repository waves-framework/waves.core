namespace Waves.Core.Base.Interfaces;

/// <summary>
/// Interface for observable / configurable plugins.
/// </summary>
public interface IWavesObservableConfigurablePlugin :
    IWavesObservableInitializablePlugin,
    IWavesConfigurablePlugin
{
}
