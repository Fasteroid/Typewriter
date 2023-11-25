using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Typewriter.LongPaths
{
    internal static class NativeMethods
    {
        /// <summary>
        /// If the file exists: CreateFileW opens the file and overwrites it.
        /// However, GetLastWin32Error returns ERROR_ALREADY_EXISTS (which is 183 in Win32 API).
        /// This is not an error in the traditional sense but rather an informational message
        /// indicating the state of the file (i.e., it already existed).
        /// </summary>
        public const int ErrorAlreadyExists = 183;

        [DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            FileAccess dwDesiredAccess,
            FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            CreationDisposition dwCreationDisposition,
            FileAttributes dwFlagsAndAttributes,
            IntPtr hTemplateFile);
    }
}