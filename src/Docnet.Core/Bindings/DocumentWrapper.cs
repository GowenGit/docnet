using System;
using System.Runtime.InteropServices;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Bindings
{
    internal sealed class DocumentWrapper : IDisposable
    {
        private readonly IntPtr _ptr;

        public FpdfDocumentT Instance { get; private set; }

        private FPDF_FORMFILLINFO _formInfo;
        private FpdfFormHandleT _formHandle;

        public DocumentWrapper(string filePath, string password)
        {
            Instance = fpdf_view.FPDF_LoadDocument(filePath, password);

            if (Instance == null)
            {
                throw new DocnetLoadDocumentException("unable to open the document", fpdf_view.FPDF_GetLastError());
            }
        }

        public DocumentWrapper(byte[] bytes, string password)
        {
            _ptr = Marshal.AllocHGlobal(bytes.Length);

            Marshal.Copy(bytes, 0, _ptr, bytes.Length);

            Instance = fpdf_view.FPDF_LoadMemDocument(_ptr, bytes.Length, password);

            if (Instance == null)
            {
                throw new DocnetLoadDocumentException("unable to open the document", fpdf_view.FPDF_GetLastError());
            }
        }

        public DocumentWrapper(FpdfDocumentT instance)
        {
            Instance = instance;

            if (Instance == null)
            {
                throw new DocnetLoadDocumentException("unable to open the document");
            }
        }

        public FpdfFormHandleT GetFormHandle()
        {
            if (_formHandle != null)
            {
                return _formHandle;
            }

            _formInfo = new FPDF_FORMFILLINFO();

            for (var i = 1; i <= 2; i++)
            {
                _formInfo.version = i;

                _formHandle = fpdf_view.FPDFDOCInitFormFillEnvironment(Instance, _formInfo);

                if (_formHandle != null)
                {
                    break;
                }
            }

            return _formHandle;
        }

        public void Dispose()
        {
            if (Instance == null)
            {
                return;
            }

            if (_formHandle != null)
            {
                fpdf_view.FPDF_ExitFormFillEnvironment(_formHandle);
            }

            fpdf_view.FPDF_CloseDocument(Instance);

            Marshal.FreeHGlobal(_ptr);

            Instance = null;
        }
    }
}
