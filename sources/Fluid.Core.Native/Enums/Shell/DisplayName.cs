namespace Fluid.Core.Native.Enums.Shell
{
    /// <summary>
    /// Requests the form of an item's display name to retrieve through IShellItem.GetDisplayName and SHGetNameFromIDList.
    /// (SIGDN)
    /// </summary>
    public enum DisplayName : uint
    {
        Normal = 0,
        ParentRelativeParsing = 0x80018001,
        ParentRelativeForAddressBar = 0x8001c001,
        DesktopAbsoluteParsing = 0x80028000,
        ParentRelativeEditing = 0x80031001,
        DesktopAbsoluteEditing = 0x8004c000,
        FileSystemPath = 0x80058000,
        Url = 0x80068000
    }
}