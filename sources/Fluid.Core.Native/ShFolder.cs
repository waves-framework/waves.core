using System;
using System.Runtime.InteropServices;
using System.Text;
using Fluid.Core.Native.Enums.Shell;

namespace Fluid.Core.Native
{
    public static class ShFolder
    {
        private const string Dll = "shfolder.dll";

        [DllImport(Dll)]
        public static extern HResult SHGetFolderPath(
            [In] IntPtr hwndOwner,
            [In] CSIDL nFolder,
            [In] IntPtr hToken,
            [In] uint dwFlags,
            [Out] StringBuilder pszPath);
    }
}