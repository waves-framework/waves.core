using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Structs.Shell
{
    /// <summary>
    /// BSTRBLOB, used in PropVariantUnion.
    /// Used by some implementations of IPropertyStorage when marshaling BSTRs on systems which don't support BSTR marshaling.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct BstrBlob
    {
        public uint cbSize;
        public IntPtr pData;
    }
}