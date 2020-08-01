using System.IO;
using Docnet.Core.Editors;
using Xunit;

namespace NugetUsageX86
{
    [Collection("Example collection")]
    public class ImageToPdfExamples
    {
        private readonly ExampleFixture _fixture;

        public ImageToPdfExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Given file path to a JPEG and width, height,
        /// create a PDF file for it.
        /// </summary>
        [Fact]
        public void JpegToPdf_WhenCalledWithJpeg_ShouldSucceed()
        {
            var file = new JpegImage
            {
                Bytes = File.ReadAllBytes("Assets/image_0.jpeg"),
                Width = 1024,
                Height = 1024
            };

            var bytes = _fixture.DocNet.JpegToPdf(new[] { file });

            File.WriteAllBytes("../../../output_file.pdf", bytes);
        }
    }
}