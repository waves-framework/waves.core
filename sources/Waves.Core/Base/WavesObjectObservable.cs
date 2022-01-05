using ReactiveUI;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Waves observable object that implement INotifyPropertyChanged.
/// </summary>
public class WavesObjectObservable :
    ReactiveObject,
    IWavesObjectObservable
{
}
