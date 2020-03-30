using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluid.Core.IoC
{
    /// <summary>
    /// Simple container.
    /// </summary>
    public class SimpleContainer
    {
        private static readonly Type DelegateType = typeof(Delegate);
        private static readonly Type EnumerableType = typeof(IEnumerable);

        private readonly List<ContainerEntry> _entries;

        /// <summary>
        ///     Creates new instance <see cref="SimpleContainer" /> class.
        /// </summary>
        public SimpleContainer()
        {
            _entries = new List<ContainerEntry>();
        }

        /// <summary>
        ///     Creates new instance <see cref="SimpleContainer" /> class.
        /// </summary>
        private SimpleContainer(IEnumerable<ContainerEntry> entries)
        {
            _entries = new List<ContainerEntry>(entries);
        }

        /// <summary>
        ///     Registers the instance.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        public void RegisterInstance<TService>(object implementation)
        {
            RegisterHandler(typeof(TService), typeof(TService).Name, container => implementation);
        }

        /// <summary>
        ///    Registers the instance.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="implementation">The implenentation.</param>
        /// <param name="key">Key.</param>
        public void RegisterInstance<TService>(object implementation, string key)
        {
            RegisterHandler(typeof(TService), key, container => implementation);
        }

        /// <summary>
        ///     Registers the instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterInstance(Type service, string key, object implementation)
        {
            RegisterHandler(service, key, container => implementation);
        }

        /// <summary>
        ///     Registers the class so that a new instance is created on every request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterPerRequest(Type service, string key, Type implementation)
        {
            RegisterHandler(service, key, container => container.BuildInstance(implementation));
        }

        /// <summary>
        ///     Registers the class so that it is created once, on first request, and the same instance is returned to all
        ///     requestors thereafter.
        /// </summary>
        public void RegisterSingleton<TService>()
        {
            object singleton = null;
            RegisterHandler(typeof(TService), typeof(TService).Name,
                container => singleton ?? (singleton = container.BuildInstance(typeof(TService))));
        }

        /// <summary>
        ///     Registers the class so that it is created once, on first request, and the same instance is returned to all
        ///     requestors thereafter.
        /// </summary>
        public void RegisterSingleton<TInterface, TImpl>()
        {
            object singleton = null;
            RegisterHandler(typeof(TInterface), typeof(TImpl).Name,
                container => singleton ?? (singleton = container.BuildInstance(typeof(TImpl))));
        }

        /// <summary>
        ///     Registers a custom handler for serving requests from the container.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void RegisterHandler(Type service, string key, Func<SimpleContainer, object> handler)
        {
            GetOrCreateEntry(service, key).Add(handler);
        }

        /// <summary>
        ///     Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        public void UnregisterHandler(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null) _entries.Remove(entry);
        }

        /// <summary>
        ///     Requests an instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>The instance, or null if a handler is not found.</returns>
        public object GetInstance(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null) return entry.Single()(this);

            if (service == null) return null;

            if (DelegateType.GetTypeInfo().IsAssignableFrom(service.GetTypeInfo()))
            {
                var typeToCreate = service.GenericTypeArguments[0];
                var factoryFactoryType = typeof(FactoryFactory<>).MakeGenericType(typeToCreate);
                var factoryFactoryHost = Activator.CreateInstance(factoryFactoryType);
                var factoryFactoryMethod =
                    factoryFactoryType.GetRuntimeMethod("Create", new[] {typeof(SimpleContainer)});
                return factoryFactoryMethod.Invoke(factoryFactoryHost, new object[] {this});
            }

            if (EnumerableType.GetTypeInfo().IsAssignableFrom(service.GetTypeInfo()) &&
                service.GetTypeInfo().IsGenericType)
            {
                var listType = service.GenericTypeArguments[0];
                var instances = GetAllInstances(listType).ToList();
                var array = Array.CreateInstance(listType, instances.Count);

                for (var i = 0; i < array.Length; i++) array.SetValue(instances[i], i);

                return array;
            }

            return null;
        }

        /// <summary>
        ///     Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registere; false otherwise.</returns>
        public bool HasHandler(Type service, string key)
        {
            return GetEntry(service, key) != null;
        }

        /// <summary>
        ///     Requests all instances of a given type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>All the instances or an empty enumerable if none are found.</returns>
        public IEnumerable<object> GetAllInstances(Type service)
        {
            var entry = GetEntry(service, null);
            return entry != null ? entry.Select(x => x(this)) : new object[0];
        }

        /// <summary>
        ///     Request all instances.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAllInstances()
        {
            var list = new List<object>();

            foreach (var entry in _entries) list.Add(entry);

            return list;
        }

        /// <summary>
        ///     Pushes dependencies into an existing instance based on interface properties with setters.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void BuildUp(object instance)
        {
            var injectables = from property in instance.GetType().GetRuntimeProperties()
                where property.CanRead && property.CanWrite && property.PropertyType.GetTypeInfo().IsInterface
                select property;

            foreach (var propertyInfo in injectables)
            {
                var injection = GetAllInstances(propertyInfo.PropertyType).ToArray();
                if (injection.Any()) propertyInfo.SetValue(instance, injection.First(), null);
            }
        }

        /// <summary>
        ///     Creates a child container.
        /// </summary>
        /// <returns>A new container.</returns>
        public SimpleContainer CreateChildContainer()
        {
            return new SimpleContainer(_entries);
        }

        private ContainerEntry GetOrCreateEntry(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry == null)
            {
                entry = new ContainerEntry {Service = service, Key = key};
                _entries.Add(entry);
            }

            return entry;
        }

        private ContainerEntry GetEntry(Type service, string key)
        {
            if (service == null) return _entries.FirstOrDefault(x => x.Key == key);

            if (key == null)
                return _entries.FirstOrDefault(x => x.Service == service && x.Key == null)
                       ?? _entries.FirstOrDefault(x => x.Service == service);

            return _entries.FirstOrDefault(x => x.Service == service && x.Key == key);
        }

        /// <summary>
        ///     Actually does the work of creating the instance and satisfying it's constructor dependencies.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object BuildInstance(Type type)
        {
            var args = DetermineConstructorArgs(type);
            return ActivateInstance(type, args);
        }

        /// <summary>
        ///     Creates an instance of the type with the specified constructor arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The constructor args.</param>
        /// <returns>The created instance.</returns>
        protected virtual object ActivateInstance(Type type, object[] args)
        {
            var instance = args.Length > 0 ? Activator.CreateInstance(type, args) : Activator.CreateInstance(type);
            Activated(instance);
            return instance;
        }

        /// <summary>
        ///     Occurs when a new instance is created.
        /// </summary>
        public event Action<object> Activated = delegate { };

        private object[] DetermineConstructorArgs(Type implementation)
        {
            var args = new List<object>();
            var constructor = SelectEligibleConstructor(implementation);

            if (constructor != null)
                args.AddRange(constructor.GetParameters().Select(info => GetInstance(info.ParameterType, null)));

            return args.ToArray();
        }

        private static ConstructorInfo SelectEligibleConstructor(Type type)
        {
            return (from c in type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic)
                orderby c.GetParameters().Length descending
                select c).FirstOrDefault();
        }
    }
}