using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Typewriter.LongPaths
{
    public static class FileWriter
    {
        public static void WriteAllText(
            string path,
            string contents)
        {
            var modifiedPath = path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase)
                ? $@"\\?\UNC\{path.Substring(2, path.Length - 2)}"
                : $@"\\?\{path}";

            using (var safeFileHandle = NativeMethods.CreateFileW(
                       modifiedPath,
                       FileAccess.GenericRead | FileAccess.GenericWrite,
                       FileShare.Read | FileShare.Write,
                       IntPtr.Zero,
                       CreationDisposition.CreateAlways,
                       FileAttributes.Normal,
                       IntPtr.Zero))
            {
                var errorCode = Marshal.GetLastWin32Error();
                if (errorCode != 0 && errorCode != NativeMethods.ErrorAlreadyExists)
                {
                    throw new Win32Exception(errorCode);
                }

                using (var fileStream = new FileStream(safeFileHandle, System.IO.FileAccess.Write))
                using (var streamWriter = new StreamWriter(
                           fileStream,
                           new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true),
                           bufferSize: 1024,
                           leaveOpen: true))
                {
                    streamWriter.Write(contents);
                }
            }
        }
    }
}