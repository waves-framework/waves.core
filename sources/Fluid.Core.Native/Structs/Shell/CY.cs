using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Structs.Shell
{
    /// <summary>
    /// CY, used in PropVariantUnion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct CY
    {
        public uint Lo;
        public int Hi;
    }
}