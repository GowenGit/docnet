using System;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    public interface IDocReader : IDisposable
    {
        PdfVersion GetPdfVersion();

        int GetPageCount();
    }
}
