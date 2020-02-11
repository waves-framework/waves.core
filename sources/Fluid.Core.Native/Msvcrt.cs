using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native
{
    public static class Msvcrt
    {
        private const string Dll = "msvcrt.dll";

        // Win32 memory copy function
        [DllImport(Dll, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        internal static extern unsafe byte* MemCpy(
            byte* dst,
            byte* src,
            int count);

        // Win32 memory set function
        [DllImport(Dll, EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        internal static extern void MemSet(
            IntPtr dst,
            int filler,
            int count);
    }
}