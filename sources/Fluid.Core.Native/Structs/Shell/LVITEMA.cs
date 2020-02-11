using System.Runtime.InteropServices;
using Fluid.Core.Native.Enums.Shell;

namespace Fluid.Core.Native.Structs.Shell
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LVITEMA
    {
        public LVIF mask;
        public int iItem;
        public int iSubItem;
        public LVIS state;
        public LVIS stateMask;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszText;
        public int cchTextMax;
        public int iImage;
        public int lParam;
    }
}