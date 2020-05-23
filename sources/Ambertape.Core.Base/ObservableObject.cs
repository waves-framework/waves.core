using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ambertape.Core.Base.Interfaces;

namespace Ambertape.Core.Base
{
    /// <summary>
    ///     Observable object base class.
    /// </summary>
    public class ObservableObject : IObservableObject
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Notifies when property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}