using System.IO;
using Docnet.Core.Bindings;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Editors
{
    internal class DocEditor : IDocEditor
    {
        public byte[] Merge(string fileOne, string fileTwo)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Unlock(string filePath, string password)
        {
            using (var docWrapper = new DocumentWrapper(filePath, password))
            using (var stream = new MemoryStream())
            {
                var success = fpdf_save.FPDF_SaveAsCopy(docWrapper.Instance, stream);

                if (!success)
                {
                    throw new DocnetException("failed to unlock the document");
                }

                return stream.ToArray();
            }
        }
    }
}