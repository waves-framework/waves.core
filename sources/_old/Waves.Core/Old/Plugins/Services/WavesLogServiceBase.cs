using System;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Old.Plugins.Services.Interfaces;

namespace Waves.Core.Old.Plugins.Services
{
    /// <summary>
    /// Abstraction for log service.
    /// </summary>
    public abstract class WavesLogServiceBase : WavesService, IWavesLogService
    {
        /// <inheritdoc />
        public abstract Task WriteLogAsync(string text);

        /// <inheritdoc />
        public abstract Task WriteLogAsync(IWavesMessageObject message);

        /// <inheritdoc />
        public Task WriteLogAsync(string title, string text, IWavesObject sender, WavesMessageType type)
        {
            return WriteLogAsync(new WavesTextMessage(text, title, sender, type));
        }

        /// <inheritdoc />
        public Task WriteLogAsync(Exception exception, IWavesObject sender, bool isFatal = false)
        {
            return WriteLogAsync(new WavesExceptionMessage(sender, exception, isFatal));
        }
    }
}
