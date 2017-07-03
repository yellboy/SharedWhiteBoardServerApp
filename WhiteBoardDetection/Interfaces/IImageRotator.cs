using System.Drawing;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IImageRotator
    {
        Bitmap RotateImageAccordingToRectangularContour(Bitmap image, RectangularContour rectangle);

        Bitmap RotateImage(Bitmap image, double angle);
    }
}