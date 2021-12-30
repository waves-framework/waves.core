namespace Waves.Core.Old.Extensions
{
    /// <summary>
    /// Extensions for arrays.
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Converts double array to single array.
        /// </summary>
        /// <param name="input">Input double array.</param>
        /// <returns>Output single array.</returns>
        public static float[] ToSingle(this double[] input)
        {
            return System.Array.ConvertAll(input, x => (float)x);
        }
    }
}
