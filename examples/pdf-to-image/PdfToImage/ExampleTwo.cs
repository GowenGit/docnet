using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfToImage
{
    public static class ExampleTwo
    {
        internal static byte[] ConvertPageToSimpleImageWithLetterOutlinesUsingScaling(IDocLib library, string path, int pageIndex)
        {
            using (var docReader = library.GetDocReader(path, new PageDimensions(1.337)))
            {
                using (var pageReader = docReader.GetPageReader(pageIndex))
                {
                    var bytes = GetModifiedImage(pageReader);

                    return bytes;
                }
            }
        }

        private static byte[] GetModifiedImage(IPageReader pageReader)
        {
            var rawBytes = pageReader.GetImage();

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            var characters = pageReader.GetCharacters();

            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                bmp.AddBytes(rawBytes);

                bmp.DrawRectangles(characters);

                using (var stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Png);

                    return stream.ToArray();
                }
            }
        }

        private static void AddBytes(this Bitmap bmp, byte[] rawBytes)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var pNative = bmpData.Scan0;

            Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
            bmp.UnlockBits(bmpData);
        }

        private static void DrawRectangles(this Bitmap bmp, IEnumerable<Character> characters)
        {
            var pen = new Pen(Color.Red);

            using (var graphics = Graphics.FromImage(bmp))
            {
                foreach (var c in characters)
                {
                    var rect = new Rectangle(c.Box.Left, c.Box.Top, c.Box.Right - c.Box.Left, c.Box.Bottom - c.Box.Top);
                    graphics.DrawRectangle(pen, rect);
                }
            }
        }
    }
}
