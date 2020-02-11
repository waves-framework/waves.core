using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Fluid.Core.Native.Enums.Window;

namespace Fluid.Core.Native.Structs.Window
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }
}
