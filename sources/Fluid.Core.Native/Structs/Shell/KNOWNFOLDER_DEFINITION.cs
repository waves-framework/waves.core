using System;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Structs.Shell
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct KNOWNFOLDER_DEFINITION
    {
        public int category;
        public IntPtr pszName;
        public IntPtr pszDescription;
        public Guid fidParent;
        public IntPtr pszRelativePath;
        public IntPtr pszParsingName;
        public IntPtr pszTooltip;
        public IntPtr pszLocalizedName;
        public IntPtr pszIcon;
        public IntPtr pszSecurity;
        public uint dwAttributes;
        public int kfdFlags;
        public Guid ftidType;
    }
}