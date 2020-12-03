using System;
using System.Collections.Generic;
using System.Composition;
using Autofac;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Container.Autofac
{
    /// <summary>
    /// Autofac container service.
    /// </summary>
    [Export(typeof(IWavesService))]
    public class Service : WavesService, IContainerService
    {
        private readonly Dictionary<Type, object> _registeredInstances = new Dictionary<Type, object>();
        
        private ContainerBuilder _builder;

        private IContainer _container;

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("06C226BC-E6FC-40A5-A419-540235CC63F0");

        /// <inheritdoc />
        public override string Name { get; set; } = "Container service (Autofac)";

        /// <inheritdoc />
        public override void Initialize(IWavesCore core)
        {
            if (IsInitialized) return;

            Core = core;

            _builder = new ContainerBuilder();

            OnMessageReceived(this,
                new WavesMessage("Initialization", 
                    "Service has been initialized.", 
                    Name, 
                    WavesMessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            OnMessageReceived(
                this, 
                new WavesMessage(
                    "Loading configuration", 
                    "There is nothing to load.",
                Name,
                WavesMessageType.Information));
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            OnMessageReceived(
                this, 
                new WavesMessage(
                    "Saving configuration", 
                    "There is nothing to save.",
                Name,
                WavesMessageType.Information));
        }

        /// <inheritdoc />
        [Reactive]
        public bool IsBuilt { get; private set; }

        /// <inheritdoc />
        public void Build()
        {
            if (_builder == null) return;
            _container = _builder.Build();
            IsBuilt = true;
        }

        /// <inheritdoc />
        public T GetInstance<T>() where T : class
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Getting instance",
                        "Error occured while getting instance from container:\r\n" + e,
                        Name,
                        e,
                        false));

                return null;
            }
        }

        /// <inheritdoc />
        public void RegisterInstance<T>(T instance) where T : class
        {
            try
            {
                if (IsBuilt)
                {
                    RebuildContainer();

                    _builder.RegisterInstance(instance).As<T>();

                    _registeredInstances.Add(typeof(T), instance);

                    Build();
                }
                else
                {
                    _builder.RegisterInstance(instance).As<T>();

                    _registeredInstances.Add(typeof(T), instance);
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(
                    this,
                    new WavesMessage(
                        "Registering instance",
                        "Error occured while registering instance from container:\r\n" + e,
                        Name,
                        e,
                        false));
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _container?.Dispose();

            IsBuilt = false;
        }

        /// <summary>
        /// Rebuilds container.
        /// </summary>
        private void RebuildContainer()
        {
            _container?.Dispose();
            _builder = new ContainerBuilder();

            IsBuilt = false;

            var method = typeof(ContainerBuilder).GetMethod("RegisterInstance");

            foreach (var (key, value) in _registeredInstances)
            {
                var genericMethod = method?.MakeGenericMethod(key);
                genericMethod?.Invoke(_builder, new[] { value });
            }
        }
    }
}
