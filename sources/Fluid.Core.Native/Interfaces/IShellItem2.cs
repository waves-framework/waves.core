using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using Fluid.Core.Native.Enums.Shell;
using Fluid.Core.Native.Structs.Shell;

namespace Fluid.Core.Native.Interfaces
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
    public interface IShellItem2 : IShellItem
    {
        /// <summary>
        /// // Not supported: IBindCtx.
        /// </summary>
        /// <param name="pbc"></param>
        /// <param name="bhid"></param>
        /// <param name="riid"></param>
        /// <param name="ppv"></param>
        /// <returns></returns>
        HResult BindToHandler(
            [In] IntPtr pbc,
            [In] ref Guid bhid,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

        /// <summary>
        /// Gets a property store object for specified property store flags.
        /// </summary>
        /// <param name="flags">The GETPROPERTYSTOREFLAGS constants that modify the property store object.</param>
        /// <param name="riid">A reference to the IID of the object to be retrieved.</param>
        /// <param name="store">When this method returns, contains the address of an IPropertyStore interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        HResult GetPropertyStore(
            GetPropertyStoreFlag flags,
            [In] ref Guid riid,
            [Out] out IPropertyStore store);

        /// <summary>
        /// Uses the specified ICreateObject instead of CoCreateInstance to create an instance of the property handler associated with the Shell item on which this method is called. Most calling applications do not need to call this method, and can call IShellItem2::GetPropertyStore instead.
        /// </summary>
        /// <param name="flags">The GETPROPERTYSTOREFLAGS constants that modify the property store object.</param>
        /// <param name="punkCreateObject">A pointer to a factory for low-rights creation of type ICreateObject.</param>
        /// <param name="riid">A reference to the IID of the object to be retrieved.</param>
        /// <param name="store">When this method returns, contains the address of the requested IPropertyStore interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        HResult GetPropertyStoreWithCreateObject(
            GetPropertyStoreFlag flags,
            [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject,   // factory for low-rights creation of type ICreateObject
            [In] ref Guid riid,
            [Out] out IPropertyStore store);

        /// <summary>
        /// Gets property store object for specified property keys.
        /// </summary>
        /// <param name="rgKeys">A pointer to an array of PROPERTYKEY structures. Each structure contains a unique identifier for each property used in creating the property store.</param>
        /// <param name="cKeys">The number of PROPERTYKEY structures in the array pointed to by rgKeys.</param>
        /// <param name="flags">The GETPROPERTYSTOREFLAGS constants that modify the property store object.</param>
        /// <param name="riid">A reference to the IID of the object to be retrieved.</param>
        /// <param name="store">When this method returns, contains the address of an IPropertyStore interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        HResult GetPropertyStoreForKeys(
            IntPtr rgKeys,
            uint cKeys,
            GetPropertyStoreFlag flags,
            [In] ref Guid riid,
            [Out] out IPropertyStore store);

        /// <summary>
        /// Gets a property description list object given a reference to a property key.
        /// </summary>
        /// <param name="keyType">A reference to a PROPERTYKEY structure.</param>
        /// <param name="riid">A reference to a desired IID.</param>
        /// <param name="list">Contains the address of an IPropertyDescriptionList interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        HResult GetPropertyDescriptionList(
            IntPtr keyType,
            [In] ref Guid riid, 
            [Out] out IPropertyDescriptionList list);

        /// <summary>
        /// Ensures any cached information in this item is up to date, or returns __HRESULT_FROM_WIN32(ERROR_FILE_NOT_FOUND) if the item does not exist.
        /// </summary>
        /// <param name="pbc">A pointer to an IBindCtx interface on a bind context object.</param>
        HResult Update(IBindCtx pbc);

        /// <summary>
        /// Gets a PROPVARIANT structure from a specified property key.
        /// </summary>
        /// <param name="key">A reference to a PROPERTYKEY structure.</param>
        /// <param name="pv">Contains a pointer to a PROPVARIANT structure.</param>
        void GetProperty(IntPtr key, [In, Out] PropertyVariant pv);

        /// <summary>
        /// Gets the class identifier (CLSID) value of specified property key.
        /// </summary>
        /// <param name="key">A reference to a PROPERTYKEY structure.</param>
        /// <param name="clsid">A pointer to a CLSID value.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        HResult GetCLSID(IntPtr key, [Out] out Guid clsid);

        /// <summary>
        /// Gets the date and time value of a specified property key.
        /// </summary>
        /// <param name="key">A reference to a PROPERTYKEY structure.</param>
        /// <param name="fileTime">A pointer to a date and time value.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        HResult GetFileTime(IntPtr key, [Out] out FILETIME fileTime);

        /// <summary>
        /// Gets the Int32 value of specified property key.
        /// </summary>
        /// <param name="key">A reference to a PROPERTYKEY structure.</param>
        /// <param name="pi">A pointer to an Int32 value.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        HResult GetInt32(IntPtr key, [Out] int pi);

        /// <summary>
        /// Gets the string value of a specified property key.
        /// </summary>
        /// <param name="key">A reference to a PROPERTYKEY structure.</param>
        /// <returns>Unicode string value.</returns>
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetString(IntPtr key);

        /// <summary>
        /// Gets the UInt32 value of a specified property key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        uint GetUInt32(IntPtr key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ulong GetUInt64(IntPtr key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetBool(IntPtr key);
    }
}