using System;
using System.IO;

namespace Docnet.Tests.Integration.Utils
{
    public sealed class TempFile : IDisposable
    {
        public string FilePath { get; }

        public TempFile(byte[] bytes)
        {
            FilePath = Path.GetTempFileName();

            File.WriteAllBytes(FilePath, bytes);
        }

        public void Dispose()
        {
            File.Delete(FilePath);
        }
    }
}
