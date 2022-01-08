using AspNetCore.AsyncInitialization;

namespace Waves.Core.Base.Interfaces;

/// <summary>
///     Interface for initializable plugin.
/// </summary>
public interface IWavesPluginInitializable :
    IWavesObject, IAsyncInitializer
{
}
