using System;

namespace Docnet.Core
{
    public sealed class DocLib : IDocLib
    {
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
            throw new NotImplementedException();
        }
    }
}
