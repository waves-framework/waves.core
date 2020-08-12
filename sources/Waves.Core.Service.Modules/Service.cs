using System;
using System.Composition;
using System.IO;
using System.Linq;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Service.Modules
{
    /// <summary>
    /// Module service.
    /// </summary>
    [Export(typeof(IService))]
    public class Service : MefLoaderService<IModule>, IModuleService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        protected override string ObjectsName => "Modules";

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            base.Initialize(core);

            var modulesDefaultPath = Path.Combine(_currentDirectory, "modules");

            if (!Directory.Exists(modulesDefaultPath))
                Directory.CreateDirectory(modulesDefaultPath);
        }

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