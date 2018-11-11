using Docnet.Core.Bindings;
using Docnet.Core.Exceptions;
using Docnet.Core.Models;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Docnet.Core.Readers
{
    internal class DocReader : IDocReader
    {
        private readonly DocumentWrapper _docWrapper;

        private readonly int _dimOne;
        private readonly int _dimTwo;

        public DocReader(string filePath, string password, int dimOne, int dimTwo)
        {
            _dimOne = dimOne;
            _dimTwo = dimTwo;

            _docWrapper = new DocumentWrapper(filePath, password);
        }

        /// <inheritdoc />
        public PdfVersion GetPdfVersion()
        {
            var version = 0;

            var success = fpdf_view.FPDF_GetFileVersion(_docWrapper.Instance, ref version) == 1;

            if (!success)
            {
                throw new DocnetException("failed to get pdf version");
            }

            return new PdfVersion(version);
        }

        /// <inheritdoc />
        public int GetPageCount()
        {
            return fpdf_view.FPDF_GetPageCount(_docWrapper.Instance);
        }

        /// <inheritdoc />
        public IPageReader GetPageReader(int pageIndex)
        {
            return new PageReader(_docWrapper, pageIndex, _dimOne, _dimTwo);
        }

        public void Dispose()
        {
            _docWrapper?.Dispose();
        }
    }
}