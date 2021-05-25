using System;
using System.Collections.Generic;
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
                {
                    return Merge(docOneWrapper, docTwoWrapper);
                }
            }
        }

        public byte[] Merge(byte[] fileOne, byte[] fileTwo)
        {
            lock (DocLib.Lock)
            {
                using (var docOneWrapper = new DocumentWrapper(fileOne, null))
                using (var docTwoWrapper = new DocumentWrapper(fileTwo, null))
                {
                    return Merge(docOneWrapper, docTwoWrapper);
                }
            }
        }

        public byte[] Merge(byte[] fileOne, ICollection<byte[]> files)
        {
            lock (DocLib.Lock)
            {
                using (var docOneWrapper = new DocumentWrapper(fileOne, null))
                {
                    var documentWrappers = new List<DocumentWrapper>();

                    try
                    {
                        foreach (var file in files)
                        {
                            documentWrappers.Add(new DocumentWrapper(file, null));
                        }

                        return Merge(docOneWrapper, documentWrappers);
                    }
                    finally
                    {
                        foreach (DocumentWrapper documentWrapper in documentWrappers)
                        {
                            documentWrapper.Dispose();
                        }
                    }
                }
            }
        }

        private static byte[] Merge(DocumentWrapper docOneWrapper, ICollection<DocumentWrapper> docWrappers)
        {
            using (var stream = new MemoryStream())
            {
                foreach (DocumentWrapper documentWrapper in docWrappers)
                {
                    var pageCountOne = fpdf_view.FPDF_GetPageCount(docOneWrapper.Instance);

                    var success = fpdf_ppo.FPDF_ImportPages(
                                      docOneWrapper.Instance,
                                      documentWrapper.Instance,
                                      null,
                                      pageCountOne) == 1;

                    if (!success)
                    {
                        throw new DocnetException("failed to merge files");
                    }
                }

                return GetBytes(stream, docOneWrapper);
            }
        }

        private static byte[] Merge(DocumentWrapper docOneWrapper, DocumentWrapper docTwoWrapper)
        {
            using (var stream = new MemoryStream())
            {
                var pageCountOne = fpdf_view.FPDF_GetPageCount(docOneWrapper.Instance);

                var success = fpdf_ppo.FPDF_ImportPages(
                                  docOneWrapper.Instance,
                                  docTwoWrapper.Instance,
                                  null,
                                  pageCountOne) == 1;

                if (!success)
                {
                    throw new DocnetException("failed to merge files");
                }

                return GetBytes(stream, docOneWrapper);
            }
        }

        public byte[] Split(string filePath, int pageFromIndex, int pageToIndex)
        {
            lock (DocLib.Lock)
            {
                using (var srcWrapper = new DocumentWrapper(filePath, null))
                {
                    return Split(srcWrapper, pageFromIndex, pageToIndex);
                }
            }
        }

        public byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex)
        {
            lock (DocLib.Lock)
            {
                using (var srcWrapper = new DocumentWrapper(bytes, null))
                {
                    return Split(srcWrapper, pageFromIndex, pageToIndex);
                }
            }
        }

        public byte[] Split(string filePath, string pageRange)
        {
            lock (DocLib.Lock)
            {
                using (var srcWrapper = new DocumentWrapper(filePath, null))
                {
                    return Split(srcWrapper, pageRange);
                }
            }
        }

        public byte[] Split(byte[] bytes, string pageRange)
        {
            lock (DocLib.Lock)
            {
                using (var srcWrapper = new DocumentWrapper(bytes, null))
                {
                    return Split(srcWrapper, pageRange);
                }
            }
        }

        private static byte[] Split(DocumentWrapper srcWrapper, int pageFromIndex, int pageToIndex)
        {
            return Split(srcWrapper, $"{pageFromIndex + 1} - {pageToIndex + 1}");
        }

        private static byte[] Split(DocumentWrapper srcWrapper, string pageRange)
        {
            using (var newWrapper = new DocumentWrapper(fpdf_edit.FPDF_CreateNewDocument()))
            using (var stream = new MemoryStream())
            {
                var success = fpdf_ppo.FPDF_ImportPages(
                                  newWrapper.Instance,
                                  srcWrapper.Instance,
                                  pageRange,
                                  0) == 1;

                if (!success)
                {
                    throw new DocnetException("failed to split file");
                }

                return GetBytes(stream, newWrapper);
            }
        }

        public byte[] Unlock(string filePath, string password)
        {
            lock (DocLib.Lock)
            {
                using (var docWrapper = new DocumentWrapper(filePath, password))
                {
                    return Unlock(docWrapper);
                }
            }
        }

        public byte[] Unlock(byte[] bytes, string password)
        {
            lock (DocLib.Lock)
            {
                using (var docWrapper = new DocumentWrapper(bytes, password))
                {
                    return Unlock(docWrapper);
                }
            }
        }

        public byte[] JpegToPdf(IReadOnlyList<JpegImage> files)
        {
            lock (DocLib.Lock)
            {
                using (var newWrapper = new DocumentWrapper(fpdf_edit.FPDF_CreateNewDocument()))
                using (var outStream = new MemoryStream())
                {
                    var index = 0;

                    foreach (var image in files)
                    {
                        using (var stream = new MemoryStream(image.Bytes))
                        {
                            var page = fpdf_edit.FPDFPageNew(newWrapper.Instance, index, image.Width, image.Height);
                            var imageObj = fpdf_edit.FPDFPageObjNewImageObj(newWrapper.Instance);

                            fpdf_custom_edit.FPDFImageObjLoadJpegFile(page, 1, imageObj, FileHandle.FromStream(stream, image.Bytes.Length));
                            fpdf_edit.FPDFImageObjSetMatrix(imageObj, image.Width, 0, 0, image.Height, 0, 0);
                            fpdf_edit.FPDFPageInsertObject(page, imageObj);
                            fpdf_edit.FPDFPageGenerateContent(page);
                            fpdf_view.FPDF_ClosePage(page);

                            index++;
                        }
                    }

                    return GetBytes(outStream, newWrapper);
                }
            }
        }

        private static byte[] Unlock(DocumentWrapper docWrapper)
        {
            using (var stream = new MemoryStream())
            {
                return GetBytes(stream, docWrapper);
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