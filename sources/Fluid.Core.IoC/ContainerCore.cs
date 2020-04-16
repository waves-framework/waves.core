using System;
using System.Collections.Generic;

namespace Fluid.Core.IoC
{
    /// <summary>
    ///     Container core.
    /// </summary>
    public static class ContainerCore
    {
        /// <summary>
        ///     Gets container.
        /// </summary>
        private static readonly SimpleContainer Container = new SimpleContainer();

        /// <summary>
        ///     Creates new instance of container core.
        /// </summary>
        static ContainerCore()
        {
            Start();
        }

        /// <summary>
        ///     Gets whether core is initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        ///     Начальная загрузка
        /// </summary>
        public static void Start()
        {
            if (IsInitialized)
                return;

            IoC.GetInstance = GetInstance;
            IoC.GetAllInstances = GetAllInstances;
            IoC.BuildUp = BuildUp;

            Container.RegisterInstance<SimpleContainer>(Container);

            IsInitialized = true;
        }

        /// <summary>
        ///     Registers new service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="instance">Instance of service.</param>
        public static void RegisterService<T>(T instance)
        {
            Container.RegisterInstance<T>(instance);
        }

        /// <summary>
        ///     Registers new service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="instance">Instance of service.</param>
        /// <param name="key">Key.</param>
        public static void RegisterService<T>(T instance, string key)
        {
            Container.RegisterInstance<T>(instance, key);
        }

        /// <summary>
        ///     Registers new service.
        /// </summary>
        /// <typeparam name="TInterface">Service type.</typeparam>
        /// <typeparam name="TImpl">Implementation type.</typeparam>
        public static void RegisterService<TInterface, TImpl>()
        {
            Container.RegisterSingleton<TInterface, TImpl>();
        }

        /// <summary>
        ///     Gets service instance.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <returns>The instance.</returns>
        public static T GetInstance<T>()
        {
            return (T) Container.GetInstance(typeof(T), null);
        }

        /// <summary>
        ///     Gets service instance.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="key">Key.</param>
        /// <returns>The instance.</returns>
        public static object GetInstance(Type serviceType, string key)
        {
            return Container.GetInstance(serviceType, key);
        }

        /// <summary>
        ///     Gets all of services by current type.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>All of service instances.</returns>
        public static IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.GetAllInstances(serviceType);
        }

        /// <summary>
        ///     Gets all of services.
        /// </summary>
        /// <returns>All of services.</returns>
        public static IEnumerable<object> GetAllInstances()
        {
            return Container.GetAllInstances();
        }

        /// <summary>
        ///     Build up IoC action.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void BuildUp(object instance)
        {
            Container.BuildUp(instance);
        }
    }
}