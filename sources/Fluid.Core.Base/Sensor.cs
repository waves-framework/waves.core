using System;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public class Sensor : Object, ISensor
    {
        private bool _isEditable;

        private string _name;
        private string _manufacturer;
        private string _serialNumber;

        private float _referenceValue = 1.0f;

        private SensorType _type = SensorType.Line;

        /// <summary>
        ///     Новый экземпляр датчика.
        /// </summary>
        public Sensor()
        {
            IsEditable = true;
        }

        /// <summary>
        ///     Новый экземпляр датчика.
        /// </summary>
        /// <param name="isEditable">Редактируемый ли датчик.</param>
        public Sensor(bool isEditable)
        {
            IsEditable = isEditable;
        }

        /// <summary>
        ///     Новый экземпляр датчика.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public Sensor(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public sealed override string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public bool IsEditable
        {
            get => _isEditable;
            private set
            {
                if (value == _isEditable)
                    return;

                _isEditable = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                if (value == _manufacturer)
                    return;

                _manufacturer = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (value == _serialNumber)
                    return;

                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public SensorType Type
        {
            get => _type;
            set
            {
                if (value == _type)
                    return;

                _type = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public float Sensitivity
        {
            get => _referenceValue;
            set
            {
                if (value.Equals(_referenceValue))
                    return;

                _referenceValue = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public object Clone()
        {
            var sensor = new Sensor(Id)
            {
                IsEditable = IsEditable,
                Name = Name,
                Manufacturer = Manufacturer,
                SerialNumber = SerialNumber,
                Sensitivity = Sensitivity,
                Type = Type
            };

            return sensor;
        }
    }
}