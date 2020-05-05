using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

#pragma warning disable SA1300
#pragma warning disable CA1707
#pragma warning disable CA1051
#pragma warning disable SA1401
#pragma warning disable CA1052
#pragma warning disable SA1307
#pragma warning disable SA1214

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Docnet.Core.Bindings
{
    /// <summary>
    /// Flags:
    /// 1 - Incremental
    /// 2 - NoIncremental
    /// 3 - RemoveSecurity.
    /// </summary>
    internal class fpdf_save
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("pdfium", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FPDF_SaveAsCopy")]
        internal static extern int FPDF_SaveAsCopy(IntPtr document, FpdfStreamWriter pFileWrite, uint flags);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("pdfium", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FPDF_SaveWithVersion")]
        private static extern int FPDF_SaveWithVersion(IntPtr document, FpdfStreamWriter pFileWrite, uint flags, int fileVersion);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool StreamWriteHandler(IntPtr writerPtr, IntPtr data, int size);

        [StructLayout(LayoutKind.Sequential)]
        internal class FpdfStreamWriter
        {
            public int version = 1;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            public readonly StreamWriteHandler Handler;

            public FpdfStreamWriter(StreamWriteHandler handler)
            {
                Handler = handler;
            }
        }

        public static bool FPDF_SaveAsCopy(FpdfDocumentT document, Stream stream)
        {
            byte[] buffer = null;

            var fileWrite = new FpdfStreamWriter((writerPtr, data, size) =>
            {
                if (buffer == null || buffer.Length < size)
                {
                    buffer = new byte[size];
                }

                Marshal.Copy(data, buffer, 0, size);

                stream.Write(buffer, 0, size);

                return true;
            });

            var result = FPDF_SaveAsCopy(document.__Instance, fileWrite, 3);

            GC.KeepAlive(fileWrite);

            return result == 1;
        }
    }
}
