using Docnet.Core.Bindings;
using Docnet.Core.Editors;
using Docnet.Core.Readers;
using Docnet.Core.Validation;

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
        public IDocReader GetDocReader(string filePath, int dimOne, int dimTwo)
        {
            return GetDocReader(filePath, null, dimOne, dimTwo);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(string filePath, string password, int dimOne, int dimTwo)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));

            Validator.CheckNotLessOrEqualToZero(dimOne, nameof(dimOne));
            Validator.CheckNotLessOrEqualToZero(dimTwo, nameof(dimTwo));

            Validator.CheckNotGreaterThan(dimOne, dimTwo, nameof(dimOne), nameof(dimTwo));

            return new DocReader(filePath, password, dimOne, dimTwo);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(byte[] bytes, int dimOne, int dimTwo)
        {
            return GetDocReader(bytes, null, dimOne, dimTwo);
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(byte[] bytes, string password, int dimOne, int dimTwo)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));

            Validator.CheckNotLessOrEqualToZero(dimOne, nameof(dimOne));
            Validator.CheckNotLessOrEqualToZero(dimTwo, nameof(dimTwo));

            Validator.CheckNotGreaterThan(dimOne, dimTwo, nameof(dimOne), nameof(dimTwo));

            return new DocReader(bytes, password, dimOne, dimTwo);
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
        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            Validator.CheckFilePathNotNull(filePath, nameof(filePath));

            Validator.CheckNotLessThanZero(pageFromIndex, nameof(pageFromIndex));
            Validator.CheckNotLessThanZero(pageToIndex, nameof(pageToIndex));

            Validator.CheckNotGreaterThan(pageFromIndex, pageToIndex, nameof(pageFromIndex), nameof(pageToIndex));

            return _editor.Split(filePath, pageFromIndex, pageToIndex);
        }

        /// <inheritdoc />
        public byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex)
        {
            Validator.CheckBytesNullOrZero(bytes, nameof(bytes));

            Validator.CheckNotLessThanZero(pageFromIndex, nameof(pageFromIndex));
            Validator.CheckNotLessThanZero(pageToIndex, nameof(pageToIndex));

            Validator.CheckNotGreaterThan(pageFromIndex, pageToIndex, nameof(pageFromIndex), nameof(pageToIndex));

            return _editor.Split(bytes, pageFromIndex, pageToIndex);
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
