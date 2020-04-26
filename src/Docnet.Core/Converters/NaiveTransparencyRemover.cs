namespace Docnet.Core.Converters
{
    public class NaiveTransparencyRemover : IImageBytesConverter
    {
        private readonly byte _backgroundRed = byte.MaxValue;
        private readonly byte _backgroundGreen = byte.MaxValue;
        private readonly byte _backgroundBlue = byte.MaxValue;

        public NaiveTransparencyRemover() { }

        /// <summary>
        /// Specify background RGB
        /// </summary>
        public NaiveTransparencyRemover(byte red, byte green, byte blue)
        {
            _backgroundRed = red;
            _backgroundGreen = green;
            _backgroundBlue = blue;
        }

        /// <summary>
        /// Removes full alpha transparency in a very naive way.
        /// </summary>
        /// <param name="bytes">Image bytes</param>
        /// <returns>Same B-G-R-A array with alpha filled with white color</returns>
        public byte[] Convert(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length / 4; i++)
            {
                var j = i * 4;

                var blue = bytes[j];
                var green = bytes[j + 1];
                var red = bytes[j + 2];
                var alpha = bytes[j + 3];

                bytes[j] = (byte)((blue * alpha + _backgroundBlue * (255 - alpha)) >> 8);
                bytes[j + 1] = (byte)((green * alpha + _backgroundGreen * (255 - alpha)) >> 8);
                bytes[j + 2] = (byte)((red * alpha + _backgroundRed * (255 - alpha)) >> 8);
                bytes[j + 3] = byte.MaxValue;
            }

            return bytes;
        }
    }
}