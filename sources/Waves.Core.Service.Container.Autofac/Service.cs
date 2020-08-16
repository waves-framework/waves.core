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
    [Export(typeof(IService))]
    public class Service : Base.Service, IContainerService
    {
        private ContainerBuilder _builder;

        private IContainer _container;

        private Dictionary<Type, object> _registeredInstances = new Dictionary<Type, object>();

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("06C226BC-E6FC-40A5-A419-540235CC63F0");

        /// <inheritdoc />
        public override string Name { get; set; } = "Container service (Autofac)";

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            if (IsInitialized) return;

            Core = core;

            _builder = new ContainerBuilder();

            OnMessageReceived(this,
                new Message("Initialization", 
                    "Service has been initialized.", 
                    Name, 
                    MessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            OnMessageReceived(this, 
                new Message("Loading configuration", 
                    "There is nothing to load.",
                Name,
                MessageType.Information));
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            OnMessageReceived(this, 
                new Message("Saving configuration", 
                    "There is nothing to save.",
                Name,
                MessageType.Information));
        }

        /// <inheritdoc />
        [Reactive]
        public bool IsBuilt { get; private set; }

        /// <inheritdoc />
        public void Build()
        {
            if (_builder != null)
            {
                _container = _builder.Build();
                IsBuilt = true;
            }
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
                    new Message("Getting instance",
                        "Error occured while getting instance from container:\r\n" + e,
                        Name,
                        e,
                        false));

                return null;
            }
        }

        /// <inheritdoc />
        public ICollection<T> GetInstances<T>() where T : class
        {
            throw new NotImplementedException();
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
                OnMessageReceived(this,
                    new Message("Registering instance",
                        "Error occured while registering instance from container:\r\n" + e,
                        Name,
                        e,
                        false));
            }
        }

        /// <inheritdoc />
        public void RegisterInstances<T>(ICollection<T> instances) where T : class
        {
            if (!IsBuilt)
            {
                foreach (var instance in instances)
                {
                    RegisterInstance(instance);
                }
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
