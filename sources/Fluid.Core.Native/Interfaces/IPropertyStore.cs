using System.Runtime.InteropServices;
using System.Security;
using Fluid.Core.Native.Structs.Shell;

namespace Fluid.Core.Native.Interfaces
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    public interface IPropertyStore
    {
        /// <summary>
        /// 	This method returns a count of the number of properties that are attached to the file.
        /// </summary>
        /// <returns></returns>
        uint GetCount();

        /// <summary>
        /// Gets a property key from the property array of an item.
        /// </summary>
        /// <param name="iProp"></param>
        /// <returns></returns>
        PropertyKey GetAt(uint iProp);

        /// <summary>
        /// This method retrieves the data for a specific property.
        /// </summary>
        /// <param name="pkey"></param>
        /// <param name="pv"></param>
        [SecurityCritical]
        void GetValue([In] ref PropertyKey pkey, [In, Out] PropertyVariant pv);

        /// <summary>
        /// This method sets a property value or replaces or removes an existing value.
        /// </summary>
        /// <param name="pkey"></param>
        /// <param name="pv"></param>
        [SecurityCritical]
        void SetValue([In] ref PropertyKey pkey, PropertyVariant pv);

        /// <summary>
        /// After a change has been made, this method saves the changes.
        /// </summary>
        void Commit();
    }
}