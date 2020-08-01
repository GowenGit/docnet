using Docnet.Core;
using System;
using System.IO;

namespace PdfToImage
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(nameof(args));
            }

            using (var library = DocLib.Instance)
            {
                var result = new byte[0];

                switch (args[2]?.ToLower().Trim() ?? "")
                {
                    case "0":
                        result = ExampleOne.ConvertPageToSimpleImageWithLetterOutlines(library, args[0], int.Parse(args[1]));
                        break;
                    case "1":
                        result = ExampleTwo.ConvertPageToSimpleImageWithLetterOutlinesUsingScaling(library, args[0], int.Parse(args[1]));
                        break;
                    case "2":
                        result = ExampleThree.ConvertPageToSimpleImageWithoutTransparency(library, args[0], int.Parse(args[1]));
                        break;
                    case "3":
                        result = ExampleFour.ConvertPageToGrayscaleImageIncludeAnnotations(library, args[0], int.Parse(args[1]));
                        break;
                }

                File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(args[0]), "output.png"), result);
            }
        }
    }
}
