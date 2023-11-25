using System;

namespace Typewriter.LongPaths
{
    [Flags]
    public enum FileAccess
        : uint
    {
        GenericRead = 0x80000000,
        GenericWrite = 0x40000000,
    }
}