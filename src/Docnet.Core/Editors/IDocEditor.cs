namespace Docnet.Core.Editors
{
    internal interface IDocEditor
    {
        byte[] Merge(string fileOne, string fileTwo);

        byte[] Split(string filePath, int pageFromIndex, int pageToIndex);

        byte[] Unlock(string filePath, string password);
    }
}