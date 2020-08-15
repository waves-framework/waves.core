using System;
using System.Collections.Generic;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core
{
    /// <summary>
    /// Service loader.
    /// </summary>
    public class ServiceLoader : MefLoaderService<IService>, IServiceLoader
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("5022BA07-754E-465F-B3DA-A5B2F37361AE");

        /// <inheritdoc />
        public override string Name { get; set; } = "Service Loader";

        /// <inheritdoc />
        protected override string ObjectsName => "Services";

        /// <inheritdoc />
        public override void Update()
        {
            AddPath(_currentDirectory);

            base.Update();

            RemovePath(_currentDirectory);
        }

        /// <inheritdoc />
        public ICollection<T> GetService<T>()
        {
            var collection = new List<T>();

            if (Objects == null)
            {
                OnMessageReceived(this,
                    new Message(
                        "Service Manager",
                        "Services not loaded.", 
                        "Service manager",
                        MessageType.Warning));

                return null;
            }

            try
            {
                foreach (var service in Objects)
                    if (service is T currentService)
                        collection.Add(currentService);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Getting service", "Error getting service (" + typeof(T) + ").",
                    "Service manager", e, false));
            }

            return collection;
        }
    }
}