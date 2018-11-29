using System;
using System.Runtime.InteropServices;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Bindings
{
    internal sealed class DocumentWrapper : IDisposable
    {
        private readonly IntPtr _ptr;

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
            _ptr = Marshal.AllocHGlobal(bytes.Length);

            Marshal.Copy(bytes, 0, _ptr, bytes.Length);

            Instance = fpdf_view.FPDF_LoadMemDocument(_ptr, bytes.Length, password);

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

            Marshal.FreeHGlobal(_ptr);

            Instance = null;
        }
    }
}
