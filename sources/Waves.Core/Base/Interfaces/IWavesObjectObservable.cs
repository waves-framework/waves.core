using ReactiveUI;

namespace Waves.Core.Base.Interfaces;

/// <summary>
/// Interface for observable object to implement INotifyPropertyChanged.
/// </summary>
public interface IWavesObjectObservable :
    IWavesObject,
    IReactiveObject
{
}
