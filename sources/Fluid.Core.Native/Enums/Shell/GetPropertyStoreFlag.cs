using System;

namespace Fluid.Core.Native.Enums.Shell
{
    /// <summary>
    /// GetPropertyStoreFlags.  GPS_*.
    /// </summary>
    /// <remarks>
    /// These are new for Vista, but are used in downlevel components
    /// </remarks>
    [Flags]
    public enum GetPropertyStoreFlag
    {
        // If no flags are specified (GPS_DEFAULT), a read-only property store is returned that includes properties for the file or item.
        // In the case that the shell item is a file, the property store contains:
        //     1. properties about the file from the file system
        //     2. properties from the file itself provided by the file's property handler, unless that file is offline,
        //         see GPS_OPENSLOWITEM
        //     3. if requested by the file's property handler and supported by the file system, properties stored in the
        //     alternate property store.
        //
        // Non-file shell items should return a similar read-only store
        //
        // Specifying other GPS_ flags modifies the store that is returned

        Default = 0x00000000,
        HandlerPropertiesOnly = 0x00000001,   // only include properties directly from the file's property handler
        ReadWrite = 0x00000002,   // Writable stores will only include handler properties
        Temporary = 0x00000004,   // A read/write store that only holds properties for the lifetime of the IShellItem object
        FastPropertiesOnly = 0x00000008,   // do not include any properties from the file's property handler (because the file's property handler will hit the disk)
        OpenSlowItem = 0x00000010,   // include properties from a file's property handler, even if it means retrieving the file from offline storage.
        DelayCreation = 0x00000020,   // delay the creation of the file's property handler until those properties are read, written, or enumerated
        BestEffort = 0x00000040,   // For readonly stores, succeed and return all available properties, even if one or more sources of properties fails. Not valid with GPS_READWRITE.
        NoOpLock = 0x00000080,   // some data sources protect the read property store with an oplock, this disables that
        MaskValid = 0x000000FF,
    }
}