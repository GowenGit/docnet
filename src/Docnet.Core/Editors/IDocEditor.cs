namespace Docnet.Core.Editors
{
    internal interface IDocEditor
    {
        byte[] Merge(string fileOne, string fileTwo);

        byte[] Merge(byte[] fileOne, byte[] fileTwo);

        byte[] Split(string filePath, int pageFromIndex, int pageToIndex);

        byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex);

        byte[] Unlock(string filePath, string password);

        byte[] Unlock(byte[] bytes, string password);
    }
}