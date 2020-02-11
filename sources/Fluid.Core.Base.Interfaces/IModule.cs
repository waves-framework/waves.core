using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fluid.Core.Base.Interfaces
{
    public interface IModule : IObject, ICloneable, IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// Инициализирован ли модуль.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Иконка модуля.
        /// </summary>
        string Icon { get; }

        /// <summary>
        ///     Описание модуля.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Производитель модуля.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Версия модуля.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Инициализация.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Запуск единичного исполнения работы модуля.
        /// </summary>
        void Execute();

        /// <summary>
        ///     Конфигурация модуля.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        ///     Точки входов данных.
        /// </summary>
        ICollection<IEntryPoint> Inputs { get; }

        /// <summary>
        ///     Точки выходов данных.
        /// </summary>
        ICollection<IEntryPoint> Outputs { get; }

        /// <summary>
        ///     Событие отправки системного сообщения.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Получение Hash-кода объекта.
        /// </summary>
        /// <returns></returns>
        int GetHashCode();
    }
}