using System;
using System.ComponentModel;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface IService : IObject, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        ///     Инициализирован ли сервисю
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Инициализация сервисаю
        /// </summary>
        void Initialize();

        /// <summary>
        /// Загрузка конфигурации.
        /// </summary>
        void LoadConfiguration(IConfiguration configuration);

        /// <summary>
        ///     Сохранение конфигурации.
        /// </summary>
        void SaveConfiguration(IConfiguration configuration);

        /// <summary>
        ///     Событие отправки сообщенияю
        /// </summary>
        event EventHandler<IMessage> MessageReceived;
    }
}