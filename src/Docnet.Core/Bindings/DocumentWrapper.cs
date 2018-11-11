using System;
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
