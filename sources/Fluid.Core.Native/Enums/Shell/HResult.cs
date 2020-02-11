namespace Fluid.Core.Native.Enums.Shell
{
    public enum HResult
    {
        DragDropStateCancel = 0x00040101,
        DragDropStateDrop = 0x00040100,
        DragDropStateUseDefaultCursors = 0x00040102,
        DataStateSameFormatEtc = 0x00040130,
        Ok = 0,
        False = 1,
        ErrorNoInterface = unchecked((int)0x80004002),
        ErrorNotImplemented = unchecked((int)0x80004001),
        OleErrorAdviseNotSupported = unchecked((int)80040003),
        MkErrorNoObject = unchecked((int)0x800401E5),
    }
}