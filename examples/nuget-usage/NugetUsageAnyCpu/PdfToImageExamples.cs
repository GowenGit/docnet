using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Docnet.Core.Converters;
using Docnet.Core.Models;
using Xunit;

namespace NugetUsageAnyCpu
{
    [Collection("Example collection")]
    public class PdfToImageExamples
    {
        private const string Path = "Assets/annotations_01.pdf";

        private readonly ExampleFixture _fixture;

        public PdfToImageExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ConvertPageToSimpleImageWithLetterOutlines_WhenCalled_ShouldSucceed()
        {
            using var docReader = _fixture.DocNet.GetDocReader(
                Path,
                new PageDimensions(1080, 1920));

            using var pageReader = docReader.GetPageReader(0);

            var rawBytes = pageReader.GetImage();

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            var characters = pageReader.GetCharacters();

            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            AddBytes(bmp, rawBytes);
            DrawRectangles(bmp, characters);

            using var stream = new MemoryStream();

            bmp.Save(stream, ImageFormat.Png);

            File.WriteAllBytes("../../../output_image.png", stream.ToArray());
        }

        [Fact]
        public void ConvertPageToSimpleImageWithLetterOutlinesUsingScaling_WhenCalled_ShouldSucceed()
        {
            using var docReader = _fixture.DocNet.GetDocReader(
                Path,
                new PageDimensions(1080, 1920));

            using var pageReader = docReader.GetPageReader(0);

            var rawBytes = pageReader.GetImage();

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            var characters = pageReader.GetCharacters();

            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            AddBytes(bmp, rawBytes);
            DrawRectangles(bmp, characters);

            using var stream = new MemoryStream();

            bmp.Save(stream, ImageFormat.Png);

            File.WriteAllBytes("../../../output_image.png", stream.ToArray());
        }

        [Fact]
        public void ConvertPageToSimpleImageWithoutTransparency_WhenCalled_ShouldSucceed()
        {
            using var docReader = _fixture.DocNet.GetDocReader(
                Path,
                new PageDimensions(1080, 1920));

            using var pageReader = docReader.GetPageReader(0);

            var rawBytes = pageReader.GetImage(new NaiveTransparencyRemover(120, 120, 0));

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            var characters = pageReader.GetCharacters();

            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            AddBytes(bmp, rawBytes);
            DrawRectangles(bmp, characters);

            using var stream = new MemoryStream();

            bmp.Save(stream, ImageFormat.Png);

            File.WriteAllBytes("../../../output_image.png", stream.ToArray());
        }

        [Fact]
        public void ConvertPageToGreyscaleImageIncludeAnnotations_WhenCalled_ShouldSucceed()
        {
            using var docReader = _fixture.DocNet.GetDocReader(
                Path,
                new PageDimensions(1080, 1920));

            using var pageReader = docReader.GetPageReader(0);

            var rawBytes = pageReader.GetImage(RenderFlags.RenderAnnotations | RenderFlags.Grayscale);

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            AddBytes(bmp, rawBytes);

            using var stream = new MemoryStream();

            bmp.Save(stream, ImageFormat.Png);

            File.WriteAllBytes("../../../output_image.png", stream.ToArray());
        }

        private static void AddBytes(Bitmap bmp, byte[] rawBytes)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var pNative = bmpData.Scan0;

            Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
            bmp.UnlockBits(bmpData);
        }

        private static void DrawRectangles(Bitmap bmp, IEnumerable<Character> characters)
        {
            var pen = new Pen(Color.Red);

            using var graphics = Graphics.FromImage(bmp);

            foreach (var c in characters)
            {
                var rect = new Rectangle(c.Box.Left, c.Box.Top, c.Box.Right - c.Box.Left, c.Box.Bottom - c.Box.Top);
                graphics.DrawRectangle(pen, rect);
            }
        }
    }
}