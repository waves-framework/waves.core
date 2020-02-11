namespace Fluid.Core.Devices.Interfaces.Input.ADC
{
    public interface IMeasuringAdcEntryPoint
    {
        /// <summary>
        ///     Вес младшего разряда.
        /// </summary>
        double DigitalResolution { get; }

        /// <summary>
        ///     Минимальное значение принимаемого напряжения.
        /// </summary>
        double MinimumVoltage { get; }

        /// <summary>
        ///     Максимально значение принимаемого напряжения.
        /// </summary>
        double MaximumVoltage { get; }

        /// <summary>
        ///     Поддерживается ли каналом внешнее питание датчиков.
        /// </summary>
        bool IsPhantomPowerSupported { get; }

        /// <summary>
        ///     Включено ли фантомное питание для датчиков.
        /// </summary>
        bool IsPhantomPowerEnabled { get; set; }
    }
}