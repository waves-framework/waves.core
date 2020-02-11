using System;

namespace Fluid.Core.Native.Enums.Shell
{
    [Flags]
    public enum FOLDERFLAGS : uint
    {
        AUTOARRANGE = 0x1,
        ABBREVIATEDNAMES = 0x2,
        SNAPTOGRID = 0x4,
        OWNERDATA = 0x8,
        BESTFITWINDOW = 0x10,
        DESKTOP = 0x20,
        SINGLESEL = 0x40,
        NOSUBFOLDERS = 0x80,
        TRANSPARENT = 0x100,
        NOCLIENTEDGE = 0x200,
        NOSCROLL = 0x400,
        ALIGNLEFT = 0x800,
        NOICONS = 0x1000,
        SHOWSELALWAYS = 0x2000,
        NOVISIBLE = 0x4000,
        SINGLECLICKACTIVATE = 0x8000,
        NOWEBVIEW = 0x10000,
        HIDEFILENAMES = 0x20000,
        CHECKSELECT = 0x40000
    }
}