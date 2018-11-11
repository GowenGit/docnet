using System;

namespace Docnet.Core.Readers
{
    public interface IPageReader : IDisposable
    {
        int PageIndex { get; }

        int GetPageWidth();

        int GetPageHeight();
    }
}
