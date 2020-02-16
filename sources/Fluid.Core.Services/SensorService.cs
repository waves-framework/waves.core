using System;
using System.Collections.Generic;
using System.Linq;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.IoC;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    public class SensorService : Service, ISensorService
    {
        private List<ISensor> _sensors = new List<ISensor>();

        /// <inheritdoc />
        public List<ISensor> Sensors
        {
            get => _sensors;
            private set
            {
                if (Equals(value, _sensors))
                {
                    return;
                }

                _sensors = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public event EventHandler SensorsUpdated;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("51627584-891E-4C35-A7A1-C0DDF2E53662");

        /// <inheritdoc />
        public override string Name { get; set; } = "Sensor Library Service";

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

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
            Sensors.Clear();

            var sensorsPathKey = "SensorService-Sensors";

            Sensors.Add(
                new Sensor(Guid.Empty)
                {
                    Name = "Линия",
                    Manufacturer = "(отсутствует)",
                    Sensitivity = 1.0f,
                    SerialNumber = "(отсутствует)",
                    Type = SensorType.Line
                });

            if (!configuration.Contains(sensorsPathKey))
            {
                configuration.AddProperty(
                    new Property(sensorsPathKey, new List<ISensor>(), false));
            }
            else
            {
                var sensors =
                    (List<ISensor>)configuration.GetPropertyValue(
                        sensorsPathKey);

                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);
                }

                OnSensorsUpdated();
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            configuration.SetPropertyValue(
                "SensorService-Sensors",
                Sensors.GetRange(1, Sensors.Count - 1));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            foreach (var sensor in Sensors)
            {
            }
        }

        /// <inheritdoc />
        public void AddSensor(ISensor sensor)
        {
            if (Sensors.Any(s => s.Id.Equals(sensor.Id)))
            {
                return;
            }

            if (Sensors.Contains(sensor))
            {
                return;
            }

            Sensors.Add(sensor);
        }

        /// <inheritdoc />
        public void RemoveSensor(ISensor sensor)
        {
            if (!Sensors.Contains(sensor))
            {
                return;
            }

            Sensors.Remove(sensor);
        }

        /// <inheritdoc />
        public void UpdateSensor(ISensor sensor)
        {
            for (var i = 0; i < Sensors.Count; i++)
            {
                if (Sensors[i].Id != sensor.Id)
                {
                    continue;
                }

                Sensors[i] = sensor;
                break;
            }
        }

        /// <summary>
        ///     Уведомление об обновлении списка датчиков.
        /// </summary>
        protected virtual void OnSensorsUpdated()
        {
            SensorsUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
