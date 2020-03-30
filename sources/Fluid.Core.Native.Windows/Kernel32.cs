using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Windows
{
    public static class Kernel32
    {
        private const string Dll = "kernel32";

        /// <summary>
        ///     Загрузка библиотеки из файла
        /// </summary>
        /// <param name="dllToLoad"></param>
        /// <returns></returns>
        [DllImport(Dll, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(Dll)]
        public static extern IntPtr GlobalLock(IntPtr hMem);
    }
}