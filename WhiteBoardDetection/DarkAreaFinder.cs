using System.Collections.Generic;
using System.Drawing;
using AForge;
using WhiteBoardDetection.Interfaces;

namespace WhiteBoardDetection
{
    public class DarkAreaFinder : IDarkAreaFinder
    {
        private const double MinimumSaturationOfColoredPixel = 0.21;

        public IEnumerable<IntPoint> Find(Bitmap image)
        {
            var notWhiteAreas = new List<IntPoint>();

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    var saturation = pixel.GetSaturation();

                    if (saturation > MinimumSaturationOfColoredPixel)
                    {
                        notWhiteAreas.Add(new IntPoint(x, y));
                    }
                }
            }

            return notWhiteAreas;;
        }
    }
}