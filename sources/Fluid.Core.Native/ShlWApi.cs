using System;
using System.Runtime.InteropServices;
using System.Text;
using Fluid.Core.Native.Structs.Shell;

namespace Fluid.Core.Native
{
    public static class ShlWApi
    {
        private const string Dll = "shlwapi.dll";

        [DllImport(Dll)]
        public static extern Int32 StrRetToBuf(ref STRRET pstr, IntPtr pidl,
            StringBuilder pszBuf,
            UInt32 cchBuf);
    }
}