using System;
using System.Linq;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Service.Modules
{
    public class Service : MefLoaderService<IModule>, IModuleService
    {
        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        public IModule GetModule(Guid id)
        {
            return Objects.FirstOrDefault(obj => obj.Id.Equals(id));
        }

        /// <inheritdoc />
        public IModule GetModule(string id)
        {
            return GetModule(Guid.Parse(id));
        }
    }
}