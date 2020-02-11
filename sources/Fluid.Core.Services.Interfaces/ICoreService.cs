using System.Collections.Generic;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface ICoreService : IService
    {
        /// <summary>
        ///     Основная директория.
        /// </summary>
        string CurrentDirectory { get; }

        /// <summary>
        /// Коллекция всех зарегистрированных сервисов.
        /// </summary>
        /// <returns></returns>
        List<IService> Services { get; }

        /// <summary>
        ///     Получение сервиса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();

        /// <summary>
        ///     Регистрация сервиса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterService<T>(T instance);
    }
}