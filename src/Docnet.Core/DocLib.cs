using System;
using Docnet.Core.Bindings;
using Docnet.Core.Editors;
using Docnet.Core.Readers;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Docnet.Core
{
    public sealed class DocLib : IDocLib
    {
        private static readonly object Lock = new object();

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
            CheckFilePathNotNull(filePath, nameof(filePath));

            CheckNotLessOrEqualToZero(dimOne, nameof(dimOne));
            CheckNotLessOrEqualToZero(dimTwo, nameof(dimTwo));

            CheckNotGreaterThan(dimOne, dimTwo, nameof(dimOne), nameof(dimTwo));

            return new DocReader(filePath, password, dimOne, dimTwo);
        }

        /// <inheritdoc />
        public byte[] Merge(string fileOne, string fileTwo)
        {
            CheckFilePathNotNull(fileOne, nameof(fileOne));
            CheckFilePathNotNull(fileTwo, nameof(fileTwo));

            return _editor.Merge(fileOne, fileTwo);
        }

        /// <inheritdoc />
        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            CheckFilePathNotNull(filePath, nameof(filePath));

            CheckNotLessThanZero(pageFromIndex, nameof(pageFromIndex));
            CheckNotLessThanZero(pageToIndex, nameof(pageToIndex));

            CheckNotGreaterThan(pageFromIndex, pageToIndex, nameof(pageFromIndex), nameof(pageToIndex));

            return _editor.Split(filePath, pageFromIndex, pageToIndex);
        }

        /// <inheritdoc />
        public byte[] Unlock(string filePath, string password)
        {
            CheckFilePathNotNull(filePath, nameof(filePath));

            return _editor.Unlock(filePath, password);
        }

        #region Validation
        private static void CheckFilePathNotNull(string filePath, string name)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(name, "file path can't be null");
            }
        }

        private static void CheckNotLessOrEqualToZero(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException("value can't be less or equal to zero", name);
            }
        }

        private static void CheckNotLessThanZero(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentException("value can't be less than zero", name);
            }
        }

        private static void CheckNotGreaterThan(int valueOne, int valueTwo, string nameOne, string nameTwo)
        {
            if (valueOne > valueTwo)
            {
                throw new ArgumentException($"{nameOne} can't be more than {nameTwo}");
            }
        }
        #endregion

        public void Dispose()
        {
            fpdf_view.FPDF_DestroyLibrary();

            _instance = null;
        }
    }
}
