using Docnet.Core;
using Docnet.Core.Editors;
using System;
using System.IO;

namespace ImageToPdf
{
    public static class Program
    {
        /// <summary>
        /// Given file path to a JPEG and width, height,
        /// create a PDF file for it.
        /// </summary>
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(nameof(args));
            }

            var file = new JpegImage
            {
                Bytes = File.ReadAllBytes(args[0]),
                Width = int.Parse(args[1]),
                Height = int.Parse(args[1])
            };

            var bytes = DocLib.Instance.JpegToPdf(new[] { file });

            File.WriteAllBytes("file.pdf", bytes);
        }
    }
}
