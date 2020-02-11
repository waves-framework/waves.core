using System;
using System.Runtime.InteropServices;
using Fluid.Core.Native.Enums.Window;

namespace Fluid.Core.Native.Structs.Window
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}