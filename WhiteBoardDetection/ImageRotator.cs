using System;
using System.Drawing;
using AForge;
using AForge.Imaging.Filters;
using WhiteBoardDetection.Interfaces;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection
{
    public class ImageRotator : IImageRotator
    {
        public Bitmap RotateImageAccordingToRectangularContour(Bitmap image, RectangularContour rectangle)
        {
            var angle1 = FindAngle(rectangle.UpperLeft, rectangle.UpperRight);
            var angle2 = FindAngle(rectangle.UpperRight, rectangle.BottomRight);
            var angle3 = FindAngle(rectangle.BottomRight, rectangle.BottomLeft);
            var angle4 = FindAngle(rectangle.BottomLeft, rectangle.UpperLeft);

            var angle = (angle1 + angle2 + angle3 + angle4) / 4;

            var rotationFilter = new RotateBilinear(angle);
            return rotationFilter.Apply(image);
        }

        private static double FindAngle(IntPoint point1, IntPoint point2)
        {
            var dx = point1.X - point2.X;
            var dy = point1.Y - point2.Y;

            var arctg = Math.Abs(dx) > Math.Abs(dy) ? Math.Atan((double)dy / dx) : Math.Atan((double)dx / dy);

            return arctg * 360 / (2 * Math.PI);
        }

        public Bitmap RotateImage(Bitmap image, double angle)
        {
            var rotationFilter = new RotateBilinear(angle);
            return rotationFilter.Apply(image);
        }
    }
}