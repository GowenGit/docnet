using System;
using System.Collections.Generic;
using System.Drawing;
using Docnet.Core.Converters;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    public interface IPageReader : IDisposable
    {
        /// <summary>
        /// Gets page index.
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
        byte[] GetImage(RenderFlags flags);

        /// <summary>
        /// Return a byte representation
        /// of the page image.
        /// Byte array is formatted as
        /// B-G-R-A ordered list.
        /// </summary>
        byte[] GetImage();

        /// <summary>
        /// Return a byte representation
        /// of the page image.
        /// Byte array is formatted as
        /// B-G-R-A ordered list. Then it
        /// applies a predefined byte transformation
        /// to modify the image.
        /// </summary>
        byte[] GetImage(IImageBytesConverter converter);

        /// <summary>
        /// Return a byte representation
        /// of the page image.
        /// Byte array is formatted as
        /// B-G-R-A ordered list. Then it
        /// applies a predefined byte transformation
        /// to modify the image.
        /// </summary>
        byte[] GetImage(IImageBytesConverter converter, RenderFlags flags);

        /// <summary>
        /// Renders the page onto a device context.
        /// </summary>
        void RenderDeviceContext(IntPtr deviceContext, Rectangle bounds);

        /// <summary>
        /// Renders the page onto a device context.
        /// </summary>
        void RenderDeviceContext(IntPtr deviceContext, Rectangle bounds, RenderFlags flags);
    }
}
