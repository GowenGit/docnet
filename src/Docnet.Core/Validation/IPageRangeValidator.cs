namespace Docnet.Core.Validation
{
    public interface IPageRangeValidator
    {
        void ValidatePageIndices(int pageFromIndex, int pageToIndex, string nameOne, string nameTwo);

        void ValidatePageNumbers(string pageRange, string argName);
    }
}