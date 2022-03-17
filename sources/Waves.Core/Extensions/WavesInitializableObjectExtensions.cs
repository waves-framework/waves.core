using Waves.Core.Base;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for <see cref="WavesInitializableObject"/>.
/// </summary>
internal static class WavesInitializableObjectExtensions
{
    /// <summary>
    /// Checks that object is <see cref="WavesInitializableObject"/> and initialize it if it is.
    /// </summary>
    /// <param name="obj">Object.</param>
    internal static async void CheckInitializable(this object obj)
    {
        if (obj is IWavesInitializableObject initializableObject)
        {
            await initializableObject.InitializeAsync();
        }
    }
}
