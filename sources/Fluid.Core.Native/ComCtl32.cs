using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native
{
    public class ComCtl32
    {
        private const string Dll = "comctl32.dll";

        [DllImport(Dll)]
        public static extern bool ImageList_Draw(IntPtr himl,
            int i, IntPtr hdcDst, int x, int y, uint fStyle);

        [DllImport(Dll)]
        public static extern bool ImageList_GetIconSize(IntPtr himl, out int cx, out int cy);
    }
}