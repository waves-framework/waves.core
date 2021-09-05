using System;
using System.Threading.Tasks;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for observable plugin.
    /// </summary>
    public interface IWavesObservablePlugin : 
        IWavesObservableObject,
        IWavesPlugin
    {
    }
}
