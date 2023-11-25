using System;

namespace Typewriter.LongPaths
{
    [Flags]
    public enum FileShare : uint
    {
        Read = 0x00000001,
        Write = 0x00000002,
    }
}