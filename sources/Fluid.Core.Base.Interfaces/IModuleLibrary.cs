using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fluid.Core.Base.Interfaces
{
    public interface IModuleLibrary :  INotifyPropertyChanged
    {
        /// <summary>
        /// Наименование библиотеки.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Описание библиотеки.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Производитель библиотеки.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Версия библиотеки.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Список модулей библиотеки.
        /// </summary>
        ICollection<IModule> Modules { get; }

        /// <summary>
        ///     Обновление списка модулей.
        /// </summary>
        void UpdateModulesList();

        /// <summary>
        ///     Событие отправки системного сообщения.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;
    }
}