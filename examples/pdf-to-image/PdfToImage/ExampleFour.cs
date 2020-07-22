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
    public static class ExampleFour
    {
        internal static byte[] ConvertPageToGrayscaleImageIncludeAnnotations(IDocLib library, string path, int pageIndex)
        {
            using (var docReader = library.GetDocReader(path, new PageDimensions(1080, 1920)))
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
            var rawBytes = pageReader.GetImage(RenderFlags.RenderAnnotations | RenderFlags.Grayscale);

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            var characters = pageReader.GetCharacters();

            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                bmp.AddBytes(rawBytes);

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
    }
}
