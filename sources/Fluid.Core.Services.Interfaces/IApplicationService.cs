using System;
using System.Collections.Generic;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface IApplicationService : IService
    {
        /// <summary>
        ///     Пути к директориям приложений.
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///     Приложения.
        /// </summary>
        IEnumerable<IApplication> Applications { get; }

        /// <summary>
        ///     Действия приложений.
        /// </summary>
        ICollection<IApplicationAction> ApplicationActions { get; }

        /// <summary>
        ///     Добавление пути к приложению.
        /// </summary>
        /// <param name="path"></param>
        void AddPath(string path);

        /// <summary>
        ///     Удаление пути к приложению.
        /// </summary>
        /// <param name="path"></param>
        void RemovePath(string path);

        /// <summary>
        ///     Обновление коллекции приложений.
        /// </summary>
        void UpdateApplications();

        /// <summary>
        ///     Событие обновление коллекции приложений.
        /// </summary>
        event EventHandler ApplicationsUpdated;

        /// <summary>
        ///     Событие обновления коллекции действий приложений.
        /// </summary>
        event EventHandler ApplicationsActionsUpdated;
    }
}