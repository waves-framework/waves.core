using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fluid.Core.Native.Interfaces
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("1F9FC1D0-C39B-4B26-817F-011967D3440E")]
    public interface IPropertyDescriptionList
    {
        /// <summary>
        /// Gets the number of properties included in the property list.
        /// </summary>
        /// <param name="pcElem"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCount(out uint pcElem);

        /// <summary>
        /// Gets the property description at the specified index in a property description list.
        /// </summary>
        /// <param name="iElem"></param>
        /// <param name="riid"></param>
        /// <param name="ppv"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
    }
}