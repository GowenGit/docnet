using System;
using System.Runtime.InteropServices;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Bindings
{
    internal sealed class DocumentWrapper : IDisposable
    {
        public FpdfDocumentT Instance { get; private set; }

        public DocumentWrapper(string filePath, string password)
        {
            Instance = fpdf_view.FPDF_LoadDocument(filePath, password);

            if (Instance == null)
            {
                throw new DocnetException("unable to open the document");
            }
        }

        public DocumentWrapper(byte[] bytes, string password)
        {
            var ptr = Marshal.AllocHGlobal(bytes.Length);

            try
            {
                Marshal.Copy(bytes, 0, ptr, bytes.Length);

                Instance = fpdf_view.FPDF_LoadMemDocument(ptr, bytes.Length, password);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            if (Instance == null)
            {
                throw new DocnetException("unable to open the document");
            }
        }

        public DocumentWrapper(FpdfDocumentT instance)
        {
            Instance = instance;

            if (Instance == null)
            {
                throw new DocnetException("unable to open the document");
            }
        }

        public void Dispose()
        {
            if (Instance == null)
            {
                return;
            }

            fpdf_view.FPDF_CloseDocument(Instance);

            Instance = null;
        }
    }
}
