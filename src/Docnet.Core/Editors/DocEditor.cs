using System.IO;
using Docnet.Core.Bindings;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Editors
{
    internal class DocEditor : IDocEditor
    {
        public byte[] Merge(string fileOne, string fileTwo)
        {
            lock (DocLib.Lock)
            {
                using (var docOneWrapper = new DocumentWrapper(fileOne, null))
                using (var docTwoWrapper = new DocumentWrapper(fileTwo, null))
                using (var stream = new MemoryStream())
                {
                    var pageCountOne = fpdf_view.FPDF_GetPageCount(docOneWrapper.Instance);

                    var success = fpdf_ppo.FPDF_ImportPages(
                                      docOneWrapper.Instance,
                                      docTwoWrapper.Instance,
                                      null, pageCountOne) == 1;

                    if (!success)
                    {
                        throw new DocnetException("failed to merge files");
                    }

                    return GetBytes(stream, docOneWrapper);
                }
            }
        }

        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            lock (DocLib.Lock)
            {
                using (var newWrapper = new DocumentWrapper(fpdf_edit.FPDF_CreateNewDocument()))
                using (var srcWrapper = new DocumentWrapper(filePath, null))
                using (var stream = new MemoryStream())
                {
                    var success = fpdf_ppo.FPDF_ImportPages(
                                      newWrapper.Instance,
                                      srcWrapper.Instance,
                                      $"{pageFromIndex + 1} - {pageToIndex + 1}", 0) == 1;

                    if (!success)
                    {
                        throw new DocnetException("failed to split file");
                    }

                    return GetBytes(stream, newWrapper);
                }
            }
        }

        public byte[] Unlock(string filePath, string password)
        {
            lock (DocLib.Lock)
            {
                using (var docWrapper = new DocumentWrapper(filePath, password))
                using (var stream = new MemoryStream())
                {
                    return GetBytes(stream, docWrapper);
                }
            }
        }

        private static byte[] GetBytes(MemoryStream stream, DocumentWrapper docWrapper)
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