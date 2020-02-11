using System;
using System.Runtime.InteropServices;
using Fluid.Core.Native.Structs.Window;

namespace Fluid.Core.Native
{
    public static class DwmApi
    {
        private const string Dll = "dwmapi.dll";

        [DllImport(Dll, PreserveSig = true)]
        internal static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);

        [DllImport(Dll)]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);
    }
}