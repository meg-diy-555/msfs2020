using System;
using System.Runtime.InteropServices;

namespace MSFS_Con
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct SimVarString
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String value;
    };
    public enum DEFINITION
    {
        def,
    };

    public enum REQUEST
    {
        req,
    };
}
