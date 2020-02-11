using System.ComponentModel;
using System.Windows;

namespace Fluid.Core.Presentation.Interfaces
{
    public interface IPresentation : INotifyPropertyChanged
    {
        /// <summary>
        /// Контекст данных представления
        /// </summary>
        object DataContext { get; }

        /// <summary>
        /// Представление
        /// </summary>
        FrameworkElement View { get; }

        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();
    }
}