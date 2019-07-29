using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Docnet.Core.Bindings
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FileReadBlockHandler(IntPtr ignore, long position, IntPtr buffer, long size);

    [StructLayout(LayoutKind.Sequential)]
    public class FileHandle
    {
        public readonly long _fileLength;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public readonly FileReadBlockHandler _readBlock;

        public readonly IntPtr _param;

        internal FileHandle(int fileLength, FileReadBlockHandler readBlock)
        {
            _fileLength = fileLength;
            _readBlock = readBlock;
            _param = IntPtr.Zero;
        }

        public static FileHandle FromStream(Stream stream, int count = 0)
        {
            if (count <= 0)
            {
                count = (int)(stream.Length - stream.Position);
            }

            var start = stream.Position;

            byte[] data = null;

            var fileRead = new FileHandle(count, (ignore, position, buffer, size) =>
            {
                if (!stream.CanRead) return false;

                stream.Position = start + position;

                if (data == null || data.Length < size)
                {
                    data = new byte[size];
                }

                if (stream.Read(data, 0, (int) size) != size)
                {
                    return false;
                }

                Marshal.Copy(data, 0, buffer, (int) size);

                return true;
            });

            GC.KeepAlive(fileRead);

            return fileRead;
        }
    }

    public class fpdf_custom_edit
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("pdfium", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FPDFImageObj_LoadJpegFileInline")]
        internal static extern unsafe int FPDFImageObjLoadJpegFileInternal(IntPtr* pages, int nCount, IntPtr image_object, FileHandle fileAccess);

        internal static int FPDFImageObjLoadJpegFile(FpdfPageT pages, int nCount, FpdfPageobjectT image_object, FileHandle fileAccess)
        {
            unsafe
            {
                var array = new[] { pages.__Instance };

                fixed (IntPtr* dataPtr = &array[0])
                {
                    return FPDFImageObjLoadJpegFileInternal(dataPtr, nCount, image_object.__Instance, fileAccess);
                }
            }
        }
    }
}
