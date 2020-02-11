using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Structs.Shell
{
    /// <summary>
    /// CArray, used in PropVariantUnion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct CArray
    {
        public uint cElems;
        public IntPtr pElems;
    }
}