using System;
using System.Runtime.InteropServices;
using Fluid.Core.Native.Enums.Shell;
using Fluid.Core.Native.Structs.Shell;
using Fluid.Core.Native.Structs.Window;

namespace Fluid.Core.Native
{
    public static class User32
    {
        private const string Dll = "user32.dll";

        [DllImport(Dll)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hWnd, ref WindowCompositionAttributeData data);

        [DllImport(Dll)]
        public static extern bool DeleteMenu(IntPtr hMenu, int uPosition,
            MF uFlags);

        [DllImport(Dll)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport(Dll)]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport(Dll)]
        public static extern IntPtr EnumChildWindows(IntPtr parentHandle,
            Win32Callback callback, IntPtr lParam);

        [DllImport(Dll)]
        public static extern bool GetMenuInfo(IntPtr hmenu,
            ref MENUINFO lpcmi);

        [DllImport(Dll)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport(Dll)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem,
            bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport(Dll)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport(Dll)]
        public static extern int SendMessage(IntPtr hWnd, MSG Msg,
            int wParam, int lParam);

        [DllImport(Dll)]
        public static extern int SendMessage(IntPtr hWnd, MSG Msg,
            int wParam, ref LVITEMA lParam);

        [DllImport(Dll)]
        public static extern int SendMessage(IntPtr hWnd, MSG Msg,
            int wParam, IntPtr lParam);

        [DllImport(Dll)]
        public static extern int SendMessage(IntPtr hWnd, MSG Msg,
            int wParam, ref TVITEMW lParam);

        [DllImport(Dll)]
        public static extern bool SetMenuInfo(IntPtr hmenu,
            ref MENUINFO lpcmi);

        [DllImport(Dll)]
        public static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        [DllImport(Dll)]
        public static extern int TrackPopupMenuEx(IntPtr hmenu,
            TPM fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);
    }
}