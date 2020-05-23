namespace Ambertape.Core.IO.Enums
{
    /// <summary>
    ///     Enum of directory types.
    /// </summary>
    public enum DirectoryType
    {
        /// <summary>
        ///     Simple directory.
        /// </summary>
        Directory,

        /// <summary>
        ///     Network share directory.
        /// </summary>
        NetworkDirectory,

        /// <summary>
        ///     HDD.
        /// </summary>
        HardDrive,

        /// <summary>
        ///     Optical drive.
        /// </summary>
        OpticalDrive,

        /// <summary>
        ///     Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        ///     Directory with no root.
        /// </summary>
        NoRootDirectory,

        /// <summary>
        ///     Removable device.
        /// </summary>
        Removable,

        /// <summary>
        ///     RAM.
        /// </summary>
        Ram,

        /// <summary>
        ///     Desktop directory.
        /// </summary>
        Desktop,

        /// <summary>
        ///     PC directory.
        /// </summary>
        Computer,

        /// <summary>
        ///     User directory.
        /// </summary>
        User
    }
}