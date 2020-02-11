using System;
using System.Runtime.InteropServices;
using System.Text;
using Fluid.Core.Native.Enums.Shell;
using Fluid.Core.Native.Interfaces;
using Fluid.Core.Native.Structs.Shell;

namespace Fluid.Core.Native
{
    public static class Shell32
    {
        private const string Dll = "shell32.dll";

        [DllImport(Dll, EntryPoint = "#660")]
        public static extern bool FileIconInit(bool bFullInit);

        [DllImport(Dll, EntryPoint = "#18")]
        public static extern IntPtr ILClone(IntPtr pidl);

        [DllImport(Dll, EntryPoint = "#25")]
        public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);

        [DllImport(Dll)]
        public static extern IntPtr ILCreateFromPath(string pszPath);

        [DllImport(Dll, EntryPoint = "#16")]
        public static extern IntPtr ILFindLastID(IntPtr pidl);

        [DllImport(Dll, EntryPoint = "#155")]
        public static extern void ILFree(IntPtr pidl);

        [DllImport(Dll, EntryPoint = "#21")]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);

        [DllImport(Dll, EntryPoint = "#23")]
        public static extern bool ILIsParent(IntPtr pidl1, IntPtr pidl2,
            bool fImmediate);

        [DllImport(Dll, EntryPoint = "#17")]
        public static extern bool ILRemoveLastID(IntPtr pidl);

        [DllImport(Dll, EntryPoint = "#71")]
        public static extern bool Shell_GetImageLists(out IntPtr lphimlLarge,
            out IntPtr lphimlSmall);

        [DllImport(Dll, EntryPoint = "#2")]
        public static extern uint SHChangeNotifyRegister(IntPtr hWnd,
            SHCNRF fSources, SHCNE fEvents, uint wMsg, int cEntries,
            ref SHChangeNotifyEntry pFsne);

        [DllImport(Dll, EntryPoint = "#4")]
        public static extern bool SHChangeNotifyUnregister(uint hNotify);

        [DllImport(Dll, EntryPoint = "#165", CharSet = CharSet.Unicode)]
        public static extern ERROR SHCreateDirectory(IntPtr hwnd, string pszPath);

        [DllImport(Dll, PreserveSig = false)]
        public static extern IShellItem SHCreateItemFromIDList(
            [In] IntPtr pidl,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        /// <summary>
        /// Creates and initializes a Shell item object from a parsing name.
        /// </summary>
        /// <param name="path">Path to item.</param>
        /// <param name="pbc">A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function. These passed parameters are often specific to the data source and are documented by the data source owners. For example, the file system data source accepts the name being parsed (as a WIN32_FIND_DATA structure), using the STR_FILE_SYS_BIND_DATA bind context parameter.</param>
        /// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItem or IID_IShellItem2.</param>
        /// <returns>When this method returns successfully, contains the interface pointer requested in riid. This is typically IShellItem or IShellItem2.</returns>
        [DllImport(Dll, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IShellItem SHCreateItemFromParsingName(
            [In] string path,
            [In] IntPtr pbc,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        /// <summary>
        /// Creates and initializes a Shell item object from a parsing name.
        /// </summary>
        /// <param name="pszPath">A pointer to a display name.</param>
        /// <param name="pbc">A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function. These passed parameters are often specific to the data source and are documented by the data source owners. For example, the file system data source accepts the name being parsed (as a WIN32_FIND_DATA structure), using the STR_FILE_SYS_BIND_DATA bind context parameter.</param>
        /// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItem or IID_IShellItem2.</param>
        /// <param name="shellItem">When this method returns successfully, contains the interface pointer requested in riid. This is typically IShellItem or IShellItem2.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport(Dll, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern HResult SHCreateItemFromParsingName(
            [In] IntPtr pszPath,
            [In] IntPtr pbc,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out] out IShellItem shellItem);

        /// <summary>
        /// Creates and initializes a Shell item object from a parsing name.
        /// </summary>
        /// <param name="pszPath">A pointer to a display name.</param>
        /// <param name="pbc">A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function. These passed parameters are often specific to the data source and are documented by the data source owners. For example, the file system data source accepts the name being parsed (as a WIN32_FIND_DATA structure), using the STR_FILE_SYS_BIND_DATA bind context parameter.</param>
        /// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItem or IID_IShellItem2.</param>
        /// <param name="shellItem">When this method returns successfully, contains the interface pointer requested in riid. This is typically IShellItem or IShellItem2.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport(Dll, CharSet = CharSet.Unicode, PreserveSig = false, EntryPoint = "SHCreateItemFromParsingName")]
        public static extern HResult SHCreateItem2FromParsingName(
            [In] IntPtr pszPath,
            [In] IntPtr pbc,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out] out IShellItem2 shellItem);


        [DllImport(Dll, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IShellItem SHCreateItemWithParent(
            [In] IntPtr pidlParent,
            [In] IShellFolder psfParent,
            [In] IntPtr pidl,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        [DllImport(Dll, PreserveSig = false)]
        public static extern IShellFolder SHGetDesktopFolder();

        [DllImport(Dll)]
        public static extern IntPtr SHGetFileInfo(IntPtr pszPath,
            int dwFileAttributes, out SHFILEINFO psfi, int cbFileInfo,
            SHGFI uFlags);

        [DllImport(Dll, PreserveSig = false)]
        public static extern IntPtr SHGetIDListFromObject(
            [In, MarshalAs(UnmanagedType.IUnknown)] object punk);

        [DllImport(Dll)]
        public static extern bool SHGetPathFromIDList(
            [In] IntPtr pidl,
            [Out] StringBuilder pszPath);

        [DllImport(Dll)]
        public static extern HResult SHGetSpecialFolderLocation(IntPtr hwndOwner,
            CSIDL nFolder, out IntPtr ppidl);
    }
}