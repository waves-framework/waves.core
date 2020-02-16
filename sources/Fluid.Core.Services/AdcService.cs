using System;
using System.Collections.Generic;
using System.Linq;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.EventArgs;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Devices.Interfaces.Input.ADC;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    public class AdcService : Service, IAdcService
    {
        private short _numberOfChannels;

        private IModuleService _moduleService;

        private Dictionary<IAdcEntryPoint, int> _channelIndexesDictionary =
            new Dictionary<IAdcEntryPoint, int>();

        private bool _isDevicesRunning;

        /// <summary>
        ///     Новый экземпляр сервиса работы с АЦП.
        /// </summary>
        /// <param name="moduleService"></param>
        public AdcService(IModuleService moduleService)
        {
            ModuleService = moduleService;
        }

        /// <inheritdoc />
        public bool IsDevicesRunning
        {
            get => _isDevicesRunning;
            private set
            {
                if (value == _isDevicesRunning)
                {
                    return;
                }

                _isDevicesRunning = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public short NumberOfChannels
        {
            get => _numberOfChannels;
            private set
            {
                if (value == _numberOfChannels)
                {
                    return;
                }

                _numberOfChannels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Сервис модулей.
        /// </summary>
        public IModuleService ModuleService
        {
            get => _moduleService;
            set
            {
                if (Equals(value, _moduleService))
                {
                    return;
                }

                _moduleService = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public List<IAdc> Devices { get; } = new List<IAdc>();

        /// <inheritdoc />
        public List<IAdc> SelectedDevices { get; } = new List<IAdc>();

        /// <inheritdoc />
        public List<IAdcEntryPoint> Channels { get; } = new List<IAdcEntryPoint>();

        /// <inheritdoc />
        public List<IAdcEntryPoint> SelectedChannels { get; } = new List<IAdcEntryPoint>();

        /// <inheritdoc />
        public event EventHandler<DataReceivedEventArgs> ChannelDataReceived;

        /// <inheritdoc />
        public event EventHandler DevicesUpdated;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("6F2A6CDC-D4BF-4C54-8478-89DDA34F3389");

        /// <inheritdoc />
        public override string Name { get; set; } = "Adc Devices Service";

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            _moduleService.ModulesUpdated += OnModuleServiceModulesUpdated;

            UpdateCollections();

            IsInitialized = true;

            OnMessageReceived(
                this,
                new Message(
                    "Информация",
                    "Сервис инициализирован.",
                    Name,
                    MessageType.Information));
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            var selectedDevicesIdsPathKey = "AdcService-SelectedDevicesIds";
            var selectedChannelsIdsPathKey = "AdcService-SelectedChannelsIds";

            if (!configuration.Contains(selectedDevicesIdsPathKey))
            {
                configuration.AddProperty(
                    new Property(selectedDevicesIdsPathKey, new List<Guid>(), false));
            }
            else
            {
                SelectedDevices.Clear();

                var list = (List<Guid>)configuration.GetPropertyValue(selectedDevicesIdsPathKey);
                foreach (var device in from device in Devices
                    from id in list.Where(id => device.Id == id)
                    select device)
                {
                    SelectedDevices.Add(device);
                }
            }

            if (!configuration.Contains(selectedChannelsIdsPathKey))
            {
                configuration.AddProperty(
                    new Property(selectedChannelsIdsPathKey, new List<Guid>(), false));
            }
            else
            {
                SelectedChannels.Clear();

                var list = (List<Guid>)configuration.GetPropertyValue(selectedChannelsIdsPathKey);
                foreach (var channel in from channel in Channels
                    from id in list.Where(id => channel.Id == id)
                    select channel)
                {
                    channel.DataReceived += OnChannelDataReceived;
                    SelectedChannels.Add(channel);
                }

                UpdateIndexesDictionary();
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            var selectedDevicesIdsPathKey = "AdcService-SelectedDevicesIds";
            var selectedChannelsIdsPathKey = "AdcService-SelectedChannelsIds";

            var selectedDevicesIdsList = SelectedDevices.Select(device => device.Id).ToList();
            var selectedChannelsIdsList = SelectedChannels.Select(channel => channel.Id).ToList();

            configuration.SetPropertyValue(selectedDevicesIdsPathKey, selectedDevicesIdsList);
            configuration.SetPropertyValue(selectedChannelsIdsPathKey, selectedChannelsIdsList);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDevicesRunning)
            {
                StopDevices();
            }
        }

        /// <inheritdoc />
        public void StartDevices()
        {
            foreach (var device in SelectedDevices)
            {
                device.Start();
            }

            IsDevicesRunning = true;
        }

        /// <inheritdoc />
        public void StopDevices()
        {
            foreach (var device in SelectedDevices)
            {
                device.Stop();
            }

            IsDevicesRunning = false;
        }

        /// <inheritdoc />
        public void SelectDevice(IAdc device)
        {
            if (SelectedDevices.Contains(device))
            {
                return;
            }

            foreach (var d in Devices.Where(d => d.Equals(device)))
            {
                SelectedDevices.Add(d);
            }
        }

        /// <inheritdoc />
        public void SelectChannel(IAdcEntryPoint channel)
        {
            if (SelectedChannels.Contains(channel))
            {
                return;
            }

            foreach (var c in Channels.Where(c => c.Equals(channel)))
            {
                SelectedChannels.Add(c);
                c.DataReceived += OnChannelDataReceived;
            }

            UpdateIndexesDictionary();
        }

        /// <inheritdoc />
        public void UnselectDevice(IAdc device)
        {
            if (!SelectedDevices.Contains(device))
            {
                return;
            }

            SelectedDevices.Remove(device);
        }

        /// <inheritdoc />
        public void UnselectChannel(IAdcEntryPoint channel)
        {
            if (!SelectedChannels.Contains(channel))
            {
                return;
            }

            SelectedChannels.Remove(channel);
            channel.DataReceived -= OnChannelDataReceived;
            UpdateIndexesDictionary();
        }

        /// <summary>
        ///     Обновление коллекций.
        /// </summary>
        private void UpdateCollections()
        {
            UpdateDevicesCollection();
            UpdateChannelsCollection();
        }

        /// <summary>
        ///     Обновление коллекций устройств.
        /// </summary>
        private void UpdateDevicesCollection()
        {
            foreach (var device in Devices)
            {
                device.Close();
            }

            Devices.Clear();

            foreach (var module in ModuleService.Modules)
            {
                if (module is IAdc adc)
                {
                    Devices.Add(adc);
                    adc.Open();
                }
            }

            OnDevicesUpdated();
        }

        /// <summary>
        ///     Обновление коллекции каналов.
        /// </summary>
        private void UpdateChannelsCollection()
        {
            NumberOfChannels = 0;

            Channels.Clear();

            foreach (var channel in Devices.SelectMany(device => device.Outputs))
            {
                Channels.Add((IAdcEntryPoint)channel);
                NumberOfChannels++;
            }
        }

        /// <summary>
        ///     Обновление индексов каналов.
        /// </summary>
        private void UpdateIndexesDictionary()
        {
            _channelIndexesDictionary = new Dictionary<IAdcEntryPoint, int>();

            for (var i = 0; i < SelectedChannels.Count; i++)
            {
                var channel = SelectedChannels[i];
                _channelIndexesDictionary.Add(channel, i);
            }
        }

        /// <summary>
        ///     Действия при обновлении списка модулей в сервисе модулей.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModuleServiceModulesUpdated(object sender, System.EventArgs e)
        {
            UpdateCollections();
        }

        /// <summary>
        ///     Уведомление о приеме данных с канала.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        protected virtual void OnChannelDataReceived(object sender, object data)
        {
            var channel = sender as IAdcEntryPoint;
            if (channel == null)
            {
                return;
            }

            var index = _channelIndexesDictionary[channel];

            OnChannelDataReceived(new DataReceivedEventArgs(channel, index, data));
        }

        /// <summary>
        ///     Уведомление об обновлении списка устройств.
        /// </summary>
        protected virtual void OnDevicesUpdated()
        {
            DevicesUpdated?.Invoke(this, System.EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление о приеме данных с канала.
        /// </summary>
        protected virtual void OnChannelDataReceived(DataReceivedEventArgs e)
        {
            ChannelDataReceived?.Invoke(this, e);
        }
    }
}
