using System;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    public interface IDocReader : IDisposable
    {
        /// <summary>
        /// PDF document version e.g. 1.7.
        /// </summary>
        /// <returns>Version.</returns>
        PdfVersion GetPdfVersion();

        /// <summary>
        /// Reads page count.
        /// </summary>
        /// <returns>Page count.</returns>
        int GetPageCount();

        /// <summary>
        /// Get page reader.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <returns>Page reader.</returns>
        IPageReader GetPageReader(int pageIndex);
    }
}
