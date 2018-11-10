using System;
using Docnet.Core.Bindings;
using Docnet.Core.Readers;

namespace Docnet.Core
{
    public sealed class DocLib : IDocLib
    {
        private static readonly object Lock = new object();

        private static DocLib _instance;

        private DocLib()
        {
            fpdf_view.FPDF_InitLibrary();
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
        public IDocReader GetDocReader(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IDocReader GetDocReader(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public byte[] Merge(string fileOne, string fileTwo)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public byte[] Unlock(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            fpdf_view.FPDF_DestroyLibrary();
        }
    }
}
