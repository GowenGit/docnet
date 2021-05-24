using System.Collections.Generic;

namespace Docnet.Core.Editors
{
    internal interface IDocEditor
    {
        byte[] Merge(string fileOne, string fileTwo);

        byte[] Merge(byte[] fileOne, byte[] fileTwo);

        byte[] Merge(byte[] fileOne, ICollection<byte[]> files);

        byte[] Split(string filePath, int pageFromIndex, int pageToIndex);

        byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex);

        byte[] Split(string filePath, string pageRange);

        byte[] Split(byte[] bytes, string pageRange);

        byte[] Unlock(string filePath, string password);

        byte[] Unlock(byte[] bytes, string password);

        byte[] JpegToPdf(IReadOnlyList<JpegImage> files);
    }
}