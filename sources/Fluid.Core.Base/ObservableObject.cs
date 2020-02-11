using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    public class ObservableObject : IObservableObject
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Уведомление об изменении параметров.
        /// </summary>
        /// <param name="propertyName">Имя параметра.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}