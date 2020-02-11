using Fluid.Core.Native.Enums.Shell;

namespace Fluid.Core.Native.Structs.Shell
{
    public struct PropertyVariant
    {
        /// <summary>
        /// Variant type
        /// </summary>
        public VARTYPE vt;

        /// <summary>
        /// unused
        /// </summary>
        public ushort wReserved1;

        /// <summary>
        /// unused
        /// </summary>
        public ushort wReserved2;

        /// <summary>
        /// unused
        /// </summary>
        public ushort wReserved3;

        /// <summary>
        /// union where the actual variant value lives
        /// </summary>
        public PropVariantUnion union;
    }
}