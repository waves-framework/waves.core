using System;
using System.Collections.Generic;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface IModuleService : Interfaces.IService
    {
        /// <summary>
        ///     Пути к библиотекам модулей.
        /// </summary>
        List<string> ModulesPaths { get; }

        /// <summary>
        ///     Пути к нативным библиотекам.
        /// </summary>
        List<string> NativeLibrariesPaths { get; }

        /// <summary>
        ///     Список библиотек модулей.
        /// </summary>
        IEnumerable<IModuleLibrary> Libraries { get; }

        /// <summary>
        ///     Список модулей.
        /// </summary>
        List<IModule> Modules { get; }

        /// <summary>
        ///     Список дополнительно загруженных библиотек.
        /// </summary>
        List<string> NativeLibrariesNames { get; }

        /// <summary>
        ///     Получает модуль по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        IModule GetModule(string id);

        /// <summary>
        ///     Добавить путь к библиотекам модулей.
        /// </summary>
        void AddModulePath(string path);

        /// <summary>
        ///     Добавить путь к нативным библиотекам.
        /// </summary>
        /// <param name="path"></param>
        void AddNativeLibraryPath(string path);

        /// <summary>
        ///     Удалить путь к библиотекам модулей.
        /// </summary>
        void RemoveModulePath(string path);

        /// <summary>
        ///     Удалить путь к нативным библиотекам.
        /// </summary>
        /// <param name="path"></param>
        void RemoveNativeLibraryPath(string path);

        /// <summary>
        ///     Обновление списка библиотек.
        /// </summary>
        void UpdateLibraries();

        /// <summary>
        ///     Событие обновления списка модулей.
        /// </summary>
        event EventHandler ModulesUpdated;
    }
}