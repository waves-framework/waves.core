using Fluid.Core.Logging.Events;

namespace Fluid.Core.Logging.Core
{
    /// <summary>
    ///     Determine how a simple value is carried through the logging
    ///     pipeline as an immutable <see cref="ScalarValue" />.
    /// </summary>
    internal interface IScalarConversionPolicy
    {
        /// <summary>
        ///     If supported, convert the provided value into an immutable scalar.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">The converted value, or null.</param>
        /// <returns>True if the value could be converted under this policy.</returns>
        bool TryConvertToScalar(object value, out ScalarValue result);
    }
}