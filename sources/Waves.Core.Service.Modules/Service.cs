using System;
using System.Composition;
using System.IO;
using System.Linq;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Modules
{
    /// <summary>
    /// Module service.
    /// </summary>
    [Export(typeof(IWavesService))]
    public class Service : WavesMefLoaderService<IWavesModule>, IModuleService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        protected override string ObjectsName => "Modules";

        /// <inheritdoc />
        public override void Initialize(IWavesCore core)
        {
            base.Initialize(core);

            var modulesDefaultPath = Path.Combine(_currentDirectory, "modules");

            if (!Directory.Exists(modulesDefaultPath))
                Directory.CreateDirectory(modulesDefaultPath);
        }

        /// <inheritdoc />
        public IWavesModule GetModule(Guid id)
        {
            return Objects.FirstOrDefault(obj => obj.Id.Equals(id));
        }

        /// <inheritdoc />
        public IWavesModule GetModule(string id)
        {
            return GetModule(Guid.Parse(id));
        }
    }
}