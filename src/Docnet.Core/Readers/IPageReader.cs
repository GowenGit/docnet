using System;
using System.Collections.Generic;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    public interface IPageReader : IDisposable
    {
        /// <summary>
        /// Page index.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Get scaled page width.
        /// </summary>
        int GetPageWidth();

        /// <summary>
        /// Get scaled page high.
        /// </summary>
        int GetPageHeight();

        /// <summary>
        /// Get page text.
        /// </summary>
        string GetText();

        /// <summary>
        /// Get all page characters with
        /// their bounding boxes.
        /// </summary>
        IEnumerable<Character> GetCharacters();

        /// <summary>
        /// Return a byte representation
        /// of the page image.
        /// Byte array is formatted as
        /// B-G-R-A ordered list.
        /// </summary>
        /// <returns></returns>
        byte[] GetImage();
    }
}
