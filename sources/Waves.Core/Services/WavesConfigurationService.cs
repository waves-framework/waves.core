using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services
{
    /// <summary>
    /// Waves configuration service.
    /// </summary>
    public class WavesConfigurationService : IWavesConfigurationService
    {
        private readonly ILogger<WavesConfigurationService> _logger;

        /// <summary>
        /// Creates new instance of <see cref="WavesConfigurationService"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public WavesConfigurationService(
            ILogger<WavesConfigurationService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public Task LoadConfigurationsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SaveConfigurationsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddObjectAsync(IWavesObject configurable)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveObjectAsync(IWavesObject configurable)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IWavesConfiguration> GetConfigurationAsync(IWavesObject configurable)
        {
            throw new NotImplementedException();
        }
    }
}
