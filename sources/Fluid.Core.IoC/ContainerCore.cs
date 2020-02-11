using System;
using System.Collections.Generic;

namespace Fluid.Core.IoC
{
    public static class ContainerCore
    {
        /// <summary>
        ///     Контейнер сервисов
        /// </summary>
        private static readonly SimpleContainer Container = new SimpleContainer();

        /// <summary>
        ///     Конструктор ядра
        /// </summary>
        static ContainerCore()
        {
            Start();
        }

        /// <summary>
        ///     Инициализировано ли ядро
        /// </summary>
        public static bool IsInitialized { get; set; }

        /// <summary>
        ///     Начальная загрузка
        /// </summary>
        public static void Start()
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            IoC.GetInstance = GetInstance;
            IoC.GetAllInstances = GetAllInstances;
            IoC.BuildUp = BuildUp;

            Container.RegisterInstance<SimpleContainer>(Container);
        }

        /// <summary>
        ///     Регистрация сервиса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void RegisterService<T>(T instance)
        {
            Container.RegisterInstance<T>(instance);
        }

        public static void RegisterService<T>(T instance, string key)
        {
            Container.RegisterInstance<T>(instance, key);
        }

        /// <summary>
        ///     Регистрация сервиса
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        public static void RegisterService<TInterface, TImpl>()
        {
            Container.RegisterSingleton<TInterface, TImpl>();
        }

        /// <summary>
        ///     Получение экземпляра сервиса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return (T) Container.GetInstance(typeof(T), null);
        }

        /// <summary>
        ///     Получение экземпляра сервиса
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetInstance(Type serviceType, string key)
        {
            return Container.GetInstance(serviceType, key);
        }

        /// <summary>
        ///     Получение всех экземпляров сервисов
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.GetAllInstances(serviceType);
        }

        /// <summary>
        /// Получение все экземпляров сервисов.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object> GetAllInstances()
        {
            return Container.GetAllInstances();
        }

        /// <summary>
        ///     ...
        /// </summary>
        /// <param name="instance"></param>
        public static void BuildUp(object instance)
        {
            Container.BuildUp(instance);
        }
    }
}