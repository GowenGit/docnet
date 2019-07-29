using System;
using System.IO;
using Docnet.Core.Editors;
using Docnet.Tests.Integration.Utils;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class JpegTests
    {
        private const int Threshold = 10000;

        private readonly LibFixture _fixture;

        public JpegTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithOneEmptyFile_ShouldThrow()
        {
            var image = new JpegImage
            {
                Bytes = new byte[] { },
                Width = 1,
                Height = 1
            };

            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.JpegToPdf(new[] { image }));
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithOneFile_ShouldSucceed()
        {
            var image = new JpegImage
            {
                Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                Width = 580,
                Height = 387
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(new[] { image });

            Assert.True(pdfBytes.Length > Threshold);
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithTwoFiles_ShouldSucceed()
        {
            var images = new[]
            {
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                    Width = 580,
                    Height = 387
                },
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_1.jpeg"),
                    Width = 880,
                    Height = 480
                }
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(images);

            Assert.True(pdfBytes.Length > Threshold);
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithTwoFilesAndCustomSize_ShouldSucceed()
        {
            var images = new[]
            {
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                    Width = 600,
                    Height = 1000
                },
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_1.jpeg"),
                    Width = 600,
                    Height = 1000
                }
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(images);

            Assert.True(pdfBytes.Length > Threshold);
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithOnePngFile_ShouldFail()
        {
            var image = new JpegImage
            {
                Bytes = File.ReadAllBytes("Docs/image_2.png"),
                Width = 1000,
                Height = 1000
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(new[] { image });

            Assert.True(pdfBytes.Length < Threshold);
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithMixedFormatFiles_ShouldFail()
        {
            var images = new[]
            {
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                    Width = 600,
                    Height = 1000
                },
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_1.jpeg"),
                    Width = 600,
                    Height = 1000
                },
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_2.png"),
                    Width = 600,
                    Height = 1000
                }
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(images);

            images = new[]
            {
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                    Width = 600,
                    Height = 1000
                },
                new JpegImage
                {
                    Bytes = File.ReadAllBytes("Docs/image_1.jpeg"),
                    Width = 600,
                    Height = 1000
                }
            };

            var pdfBytesTwo = _fixture.Lib.JpegToPdf(images);

            Assert.True(pdfBytes.Length - pdfBytesTwo.Length < Threshold);
        }

        [Fact]
        public void JpegToPdf_WhenCalledWithOneLargeFile_ShouldSucceed()
        {
            var image = new JpegImage
            {
                Bytes = File.ReadAllBytes("Docs/image_4.jpeg"),
                Width = 3600,
                Height = 2874
            };

            var pdfBytes = _fixture.Lib.JpegToPdf(new[] { image });

            Assert.True(pdfBytes.Length > Threshold);
        }

        // ReSharper disable once UnusedMember.Global
        private void JpegToPdf_MemLeak_Test()
        {
            while (true)
            {
                var images = new[]
                {
                    new JpegImage
                    {
                        Bytes = File.ReadAllBytes("Docs/image_0.jpeg"),
                        Width = 600,
                        Height = 1000
                    },
                    new JpegImage
                    {
                        Bytes = File.ReadAllBytes("Docs/image_1.jpeg"),
                        Width = 600,
                        Height = 1000
                    }
                };

                var pdfBytes = _fixture.Lib.JpegToPdf(images);

                Assert.True(pdfBytes.Length > Threshold);
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}