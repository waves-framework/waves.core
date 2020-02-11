using System;
using System.ComponentModel;
using Fluid.Core.Enums;

namespace Fluid.Core.Interfaces
{
    public interface ISensor : IObject, INotifyPropertyChanged, ICloneable
    {
        /// <summary>
        ///     Редактируемый ли датчик.
        /// </summary>
        bool IsEditable { get; }

        /// <summary>
        ///     Производитель.
        /// </summary>
        string Manufacturer { get; set; }

        /// <summary>
        ///     Серийный номер.
        /// </summary>
        string SerialNumber { get; set; }

        /// <summary>
        ///     Тип датчика.
        /// </summary>
        SensorType Type { get; set; }

        /// <summary>
        ///     Чувствительность.
        /// </summary>
        float Sensitivity { get; set; }
    }
}