using System.Collections.Generic;
using Docnet.Core.Bindings;
using Docnet.Core.Editors;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using Docnet.Core.Validation;
using Docnet.Core.Exceptions;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace Docnet.Core
{
    public sealed class DocLib : IDocLib
    {
        /// <summary>
        /// PDFium is not thread-safe
        /// so we need to lock every native
        /// call. We might implement
        /// Command patter or something similar
        /// to get around this in the future.
        /// </summary>
        internal static readonly object Lock = new object();

        private static DocLib _instance;

        private readonly IDocEditor _editor;

        private DocLib()
        {
            fpdf_view.FPDF_InitLibrary();

            _editor = new DocEditor();
        }

        public static DocLib Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DocLib();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(string filePath, PageDimensions dimensionOptions)
        {
            return GetDocReader(filePath, null, dimensionOptions);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(string filePath, string password, PageDimensions dimensionOptions)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));

            return new DocReader(filePath, password, dimensionOptions);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(byte[] bytes, PageDimensions dimensionOptions)
        {
            return GetDocReader(bytes, null, dimensionOptions);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(byte[] bytes, string password, PageDimensions dimensionOptions)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));

            return new DocReader(bytes, password, dimensionOptions);
        }

        /// <inheritdoc />
        public byte[] Merge(string fileOne, string fileTwo)
        {
            Validator.CheckFilePathNotNull(fileOne, nameof(fileOne));
            Validator.CheckFilePathNotNull(fileTwo, nameof(fileTwo));

            return _editor.Merge(fileOne, fileTwo);
        }

        /// <inheritdoc />
        public byte[] Merge(byte[] fileOne, byte[] fileTwo)
        {
            Validator.CheckBytesNullOrZero(fileOne, nameof(fileOne));
            Validator.CheckBytesNullOrZero(fileTwo, nameof(fileTwo));

            return _editor.Merge(fileOne, fileTwo);
        }

        /// <inheritdoc />
        public byte[] Merge(IReadOnlyList<byte[]> files)
        {
            Validator.CheckCollectionNotEmpty(files, nameof(files));

            foreach (var file in files)
            {
                Validator.CheckBytesNullOrZero(file, nameof(files));
            }

            return _editor.Merge(files);
        }

        /// <inheritdoc />
        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));
            Validator.ValidatePageIndices(pageFromIndex, pageToIndex);

            return _editor.Split(filePath, pageFromIndex, pageToIndex);
        }

        /// <inheritdoc />
        public byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));
            Validator.ValidatePageIndices(pageFromIndex, pageToIndex);

            return _editor.Split(bytes, pageFromIndex, pageToIndex);
        }

        /// <inheritdoc />
        public byte[] Split(string filePath, string pageRange)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));
            Validator.ValidatePageNumbers(pageRange, nameof(pageRange));

            return _editor.Split(filePath, pageRange);
        }

        /// <inheritdoc />
        public byte[] Split(byte[] bytes, string pageRange)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));
            Validator.ValidatePageNumbers(pageRange, nameof(pageRange));

            return _editor.Split(bytes, pageRange);
        }

        /// <inheritdoc />
        public byte[] Unlock(string filePath, string password)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));

            return _editor.Unlock(filePath, password);
        }

        /// <inheritdoc />
        public byte[] Unlock(byte[] bytes, string password)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));

            return _editor.Unlock(bytes, password);
        }

        public byte[] JpegToPdf(IReadOnlyList<JpegImage> files)
        {
            foreach (var jpegImage in files)
            {
                Validator.CheckBytesNullOrZero(jpegImage.Bytes, nameof(jpegImage.Bytes));

                Validator.CheckNotLessThanZero(jpegImage.Width, nameof(jpegImage.Width));
                Validator.CheckNotLessThanZero(jpegImage.Height, nameof(jpegImage.Height));
            }

            return _editor.JpegToPdf(files);
        }

        public string GetLastError()
        {
            lock (Lock)
            {
                var code = fpdf_view.FPDF_GetLastError();

                return LastError.ErrorCodePhrase(code);
            }
        }

        public void Dispose()
        {
            lock (Lock)
            {
                fpdf_view.FPDF_DestroyLibrary();
            }

            _instance = null;
        }
    }
}
