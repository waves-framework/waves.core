using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Structs.Shell
{
    /// <summary>
    /// BLOB, used in PropVariantUnion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Blob
    {
        public uint cbSize;
        public IntPtr pBlobData;
    }
}