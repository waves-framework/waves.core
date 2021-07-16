using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core.Plugins.Services
{
    /// <summary>
    ///     Autofac container service.
    /// </summary>
    [WavesService(
        "CDD3F6A1-3B7D-4FE3-944C-99B00C3A9C16",
        typeof(IWavesContainerService))]
    public class WavesContainerService :
        WavesService,
        IWavesContainerService
    {
        private readonly IWavesCore _core;
        private readonly IWavesTypeLoaderService _typeLoaderService;

        private ICollection<WavesContainterRegistration> _registrations;

        /// <summary>
        /// Creates new instance of <see cref="WavesContainerService"/>.
        /// </summary>
        /// <param name="core">Instance of <see cref="IWavesCore"/>.</param>
        /// <param name="typeLoaderService">Instance of <see cref="IWavesTypeLoaderService"/>.</param>
        public WavesContainerService(
            IWavesCore core,
            IWavesTypeLoaderService typeLoaderService)
        {
            _core = core;
            _typeLoaderService = typeLoaderService;
        }

        /// <inheritdoc />
        public bool IsBuilt { get; private set; }

        /// <summary>
        ///     IoC container.
        /// </summary>
        protected IContainer Container { get; set; }

        /// <summary>
        ///     IoC container builder.
        /// </summary>
        protected ContainerBuilder ContainerBuilder { get; set; }

        /// <inheritdoc />
        public override async Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return;
            }

            try
            {
                _registrations = new List<WavesContainterRegistration>();

                ContainerBuilder = new ContainerBuilder();

                IsInitialized = true;

                await RegisterTypesAsync();

                await _core.WriteLogAsync(
                    "Initialization",
                    "Autofac Container Service was initialized.",
                    this,
                    WavesMessageType.Information);
            }
            catch (Exception e)
            {
                await _core.WriteLogAsync(e, this, true);
            }
        }

        /// <inheritdoc />
        public async Task BuildAsync()
        {
            try
            {
                if (ContainerBuilder == null)
                {
                    throw new NullReferenceException("Container builder not initialized.");
                }

                await _core.WriteLogAsync(
                    "Container",
                    "Building container...",
                    this,
                    WavesMessageType.Information);

                Container = ContainerBuilder.Build();
                await InitializeSingletonPluginsAsync();
                IsBuilt = true;

                await _core.WriteLogAsync(
                    "Container",
                    "Autofac IoC container was built.",
                    this,
                    WavesMessageType.Success);
            }
            catch (Exception e)
            {
                await _core.WriteLogAsync(e, this, true);
            }
        }

        /// <inheritdoc />
        public async Task Populate(
            IServiceCollection collection)
        {
            try
            {
                if (ContainerBuilder == null)
                {
                    throw new NullReferenceException("Container builder not initialized.");
                }

                await _core.WriteLogAsync(
                    "Container",
                    "Populating container...",
                    this,
                    WavesMessageType.Information);

                ContainerBuilder.Populate(collection);

                await _core.WriteLogAsync(
                    "Container",
                    $"Autofac IoC container was populate with {collection} services.",
                    this,
                    WavesMessageType.Success);
            }
            catch (Exception e)
            {
                await _core.WriteLogAsync(e, this, true);
            }
        }

        /// <inheritdoc />
        public Task<T> GetInstanceAsync<T>()
            where T : class
        {
            return Task.FromResult(Container.Resolve<T>());
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetInstancesAsync<T>()
            where T : class
        {
            return Task.FromResult(Container.Resolve<IEnumerable<T>>());
        }

        /// <inheritdoc />
        public Task<object> GetInstanceAsync(Type t)
        {
            return Task.FromResult(Container.Resolve(t));
        }

        /// <inheritdoc />
        public Task<T> GetInstanceAsync<T>(object key)
            where T : class
        {
            return Task.FromResult(Container.ResolveKeyed<T>(key));
        }

        /// <inheritdoc />
        public Task<object> GetInstanceAsync(Type type, object key)
        {
            return Task.FromResult(Container.ResolveKeyed(key, type));
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetInstancesAsync<T>(object key)
            where T : class
        {
            return Task.FromResult(Container.ResolveKeyed<IEnumerable<T>>(key));
        }

        /// <inheritdoc />
        public async Task RegisterInstanceAsync<T>(T instance)
            where T : class
        {
            ContainerBuilder.RegisterInstance(instance).As<T>();

            _registrations.Add(new WavesContainterRegistration(instance));

            await _core.WriteLogAsync(
                "Plugins",
                $"{instance} was registered.",
                this,
                WavesMessageType.Information);
        }

        /// <inheritdoc />
        public async Task RegisterSingleInstanceAsync<T>(T instance)
            where T : class
        {
            ContainerBuilder.RegisterInstance(instance).As<T>().SingleInstance();

            _registrations.Add(new WavesContainterRegistration(instance, true));

            await _core.WriteLogAsync(
                "Plugins",
                $"{instance} was registered as single instance.",
                this,
                WavesMessageType.Information);
        }

        /// <inheritdoc />
        public async Task RegisterKeyedTypeAsync<T>(Type type, object key)
            where T : class
        {
            ContainerBuilder.RegisterType(type).Keyed<T>(key);

            _registrations.Add(new WavesContainterRegistration(type, false, key));

            await _core.WriteLogAsync(
                "Plugins",
                $"{type} was registered with key: {key}.",
                this,
                WavesMessageType.Information);
        }

        /// <inheritdoc />
        public async Task RegisterSingleKeyedTypeAsync<T>(Type type, object key)
            where T : class
        {
            ContainerBuilder.RegisterType(type).Keyed<T>(key).SingleInstance();

            _registrations.Add(new WavesContainterRegistration(type, true, key));

            await _core.WriteLogAsync(
                "Plugins",
                $"{type} was registered as single instance with key: {key}.",
                this,
                WavesMessageType.Information);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Autofac Container Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Container?.Dispose();
            ContainerBuilder = null;

            // TODO: your code for release unmanaged resources.
        }

        /// <summary>
        /// Registers types from type loader service.
        /// </summary>
        private async Task RegisterTypesAsync()
        {
            if (_typeLoaderService == null)
            {
                await _core.WriteLogAsync(
                    "Type loading",
                    $"Type loader service not initialized.",
                    this,
                    WavesMessageType.Warning);

                return;
            }

            if (_typeLoaderService.Types == null ||
                _typeLoaderService.Types.Count == 0)
            {
                await _core.WriteLogAsync(
                    "Type loading",
                    $"Can't find types for registering.",
                    this,
                    WavesMessageType.Warning);

                return;
            }

            var types = _typeLoaderService.Types;

            foreach (var type in types)
            {
                try
                {
                    var attributes = type.GetCustomAttributes(false);

                    foreach (var attribute in attributes)
                    {
                        if (!(attribute is WavesPluginAttribute pluginAttribute))
                        {
                            continue;
                        }

                        var pluginType = pluginAttribute.Type;
                        var pluginKey = pluginAttribute.Key;
                        var isSingleInstance = pluginAttribute.IsSingleInstance;

                        if (pluginKey == null)
                        {
                            if (isSingleInstance)
                            {
                                ContainerBuilder.RegisterType(type).As(pluginType).SingleInstance();
                                _registrations.Add(new WavesContainterRegistration(pluginType, true));
                            }
                            else
                            {
                                ContainerBuilder.RegisterType(type).As(pluginType);
                                _registrations.Add(new WavesContainterRegistration(pluginType));
                            }
                        }
                        else
                        {
                            if (isSingleInstance)
                            {
                                ContainerBuilder.RegisterType(type).As(pluginType).Keyed(pluginKey, pluginType).SingleInstance();
                                _registrations.Add(new WavesContainterRegistration(pluginType, true, pluginKey));
                            }
                            else
                            {
                                ContainerBuilder.RegisterType(type).As(pluginType).Keyed(pluginKey, pluginType);
                                _registrations.Add(new WavesContainterRegistration(pluginType, false, pluginKey));
                            }
                        }

                        await _core.WriteLogAsync(
                            "Plugins",
                            $"Type {type} was registered.",
                            this,
                            WavesMessageType.Information);
                    }
                }
                catch (Exception e)
                {
                    await _core.WriteLogAsync(e, this, true);
                }
            }
        }

        /// <summary>
        /// Initializes singleton plugins.
        /// </summary>
        private async Task InitializeSingletonPluginsAsync()
        {
            await _core.WriteLogAsync(
                "Container",
                "Initializing singletons...",
                this,
                WavesMessageType.Information);

            foreach (var registration in _registrations)
            {
                try
                {
                    if (registration.IsType && (Type)registration.Object == typeof(IWavesConfigurationService))
                    {
                        await InitializeRegistrationAsync(registration);
                        _registrations.Remove(registration);
                        break;
                    }
                }
                catch (Exception e)
                {
                    await _core.WriteLogAsync(e, this, true);
                }
            }

            foreach (var registration in _registrations)
            {
                try
                {
                    await InitializeRegistrationAsync(registration);
                }
                catch (Exception e)
                {
                    await _core.WriteLogAsync(e, this, false);
                }
            }

            await _core.WriteLogAsync(
                "Container",
                "Singletons initialized.",
                this,
                WavesMessageType.Success);
        }

        /// <summary>
        /// Initializes registration.
        /// </summary>
        /// <param name="registration">Registration.</param>
        private async Task InitializeRegistrationAsync(WavesContainterRegistration registration)
        {
            if (registration.IsSingleInstance)
            {
                if (registration.IsType)
                {
                    var type = registration.Object as Type;
                    if (type == null)
                    {
                        return;
                    }

                    var instance = await GetInstanceAsync(type);
                    if (instance is IWavesPlugin plugin)
                    {
                        await plugin.InitializeAsync();

                        await _core.WriteLogAsync(
                            "Container",
                            $"Plugin {plugin} initialized.",
                            this,
                            WavesMessageType.Success);
                    }
                }
                else
                {
                    var plugin = registration.Object as IWavesPlugin;
                    if (plugin == null)
                    {
                        return;
                    }

                    await plugin.InitializeAsync();

                    await _core.WriteLogAsync(
                        "Container",
                        $"Plugin {plugin} initialized.",
                        this,
                        WavesMessageType.Success);
                }
            }
        }
    }
}
