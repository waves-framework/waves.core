using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fluid.Core.Native.Structs.Shell;

namespace Fluid.Core.Native.Interfaces
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A99400F4-3D84-4557-94BA-1242FB2CC9A6")]
    public interface IPropertyEnumTypeList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCount([Out] out uint pctypes);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAt(
            [In] uint itype,
            [In] ref Guid riid,   // riid may be IID_IPropertyEnumType
            [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetConditionAt(
            [In] uint index,
            [In] ref Guid riid,
            out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void FindMatchingIndex(
            [In] PropertyVariant propvarCmp,
            [Out] out uint pnIndex);
    }
}