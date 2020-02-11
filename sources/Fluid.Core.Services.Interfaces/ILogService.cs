using Fluid.Core.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface ILogService : IService
    {
        /// <summary>
        /// Записывает сообщение в лог.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void WriteLogMessage(IMessage message);
    }
}