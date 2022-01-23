namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for arrays.
/// </summary>
internal static class ArrayExtension
{
    /// <summary>
    /// Converts double array to single array.
    /// </summary>
    /// <param name="input">Input double array.</param>
    /// <returns>Output single array.</returns>
    internal static float[] ToSingle(this double[] input)
    {
        return System.Array.ConvertAll(input, x => (float)x);
    }
}
