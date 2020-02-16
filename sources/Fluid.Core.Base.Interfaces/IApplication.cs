using System;
using System.Collections.Generic;

namespace Fluid.Core.Base.Interfaces
{
    public interface IApplication : IObject, IDisposable
    {
        /// <summary>
        ///     Инициализирован ли приложение.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Цвет фона иконки.
        /// </summary>
        IColor IconBackgroundColor { get; }

        /// <summary>
        /// Цвет иконки.
        /// </summary>
        IColor IconForegroundColor { get; }

        /// <summary>
        ///     Конфигурация приложения.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        ///     Иконка приложения.
        /// </summary>
        string Icon { get; }

        /// <summary>
        ///     Описание приложения.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Производитель приложения.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Версия приложения.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Список доступных действия для приложения.
        /// </summary>
        ICollection<IApplicationAction> Actions { get; }

        /// <summary>
        ///     Событие отправки системного сообщения.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Событие обновления списка доступных действий.
        /// </summary>
        event EventHandler ActionsUpdated;

        /// <summary>
        ///     Инициализация приложения.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Сохранение конфигурации.
        /// </summary>
        void SaveConfiguration();
    }
}