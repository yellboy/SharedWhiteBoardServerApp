using System;
using System.Drawing;
using AForge.Imaging.Filters;
using WhiteBoardDetection.Interfaces;
using Image = AForge.Imaging.Image;

namespace WhiteBoardDetection
{
    public class SimilarityChecker : ISimilarityChecker
    {
        public double CheckSimilarity(Bitmap originalImage, Bitmap templateImage)
        {
            var originalImageCopy = Image.Clone(originalImage);
            var templateImageCopy = Image.Clone(templateImage);

            var resizeFilter = new ResizeBilinear(originalImageCopy.Width, originalImageCopy.Height);
            templateImage = resizeFilter.Apply(templateImageCopy);

            var height = originalImageCopy.Height < templateImageCopy.Height ? originalImageCopy.Height : templateImageCopy.Height;
            var width = originalImageCopy.Width < templateImageCopy.Width ? originalImageCopy.Width : templateImageCopy.Width;

            var count = 0;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel1 = originalImageCopy.GetPixel(x, y);
                    var pixel2 = templateImageCopy.GetPixel(x, y);

                    if (Math.Abs(pixel1.GetHue() - pixel2.GetHue()) <= 50)
                    {
                        count++;
                    }
                }
            }

            return (double) count / (height * width);
        }
    }
}