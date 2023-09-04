namespace Docnet.Core.Converters
{
    public interface IImageBytesConverter
    {
        /// <summary>
        /// Input is in B-G-R-A format.
        /// </summary>
        /// <param name="bytes">Image bytes.</param>
        void Convert(byte[] bytes);
    }
}
