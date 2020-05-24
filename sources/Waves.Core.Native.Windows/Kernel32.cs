using System;
using System.Runtime.InteropServices;

namespace Waves.Core.Native.Windows
{
    /// <summary>
    ///     Kernel32.dll
    /// </summary>
    public static class Kernel32
    {
        private const string Dll = "kernel32";

        /// <summary>
        ///     Loads native library by path.
        /// </summary>
        /// <param name="dllToLoad">Path.</param>
        /// <returns>Pointer.</returns>
        [DllImport(Dll, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);
    }
}