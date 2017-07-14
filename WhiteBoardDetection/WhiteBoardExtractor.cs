using System.Diagnostics;
using System.Drawing;
using AForge.Imaging.Filters;
using WhiteBoardDetection.Interfaces;
using WhiteBoardDetection.Models;
using Image = AForge.Imaging.Image;


namespace WhiteBoardDetection
{
    public class WhiteBoardExtractor : IWhiteBoardExtractor
    {
        private const string InputImagePath = "\\input\\image.jpg";
        private const string Template1ImagePath = "\\input\\template1.jpg";
        private const string Template2ImagePath = "\\input\\template2.jpg";
        private const string Template3ImagePath = "\\input\\template3.jpg";
        private const string Template4ImagePath = "\\input\\template4.jpg";
        private const string OutputImagePath = "\\output\\image.jpg";

        private readonly ICornerFinder _cornerFinder;
        private readonly IImageRotator _imageRotator;

        public WhiteBoardExtractor(ICornerFinder cornerFinder, IImageRotator imageRotator)
        {
            _cornerFinder = cornerFinder;
            _imageRotator = imageRotator;
        }
        
        public void DetectAndCrop(string storageFolder)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var image = Image.FromFile($"{storageFolder}{InputImagePath}");

            var template1 = Image.FromFile($"{storageFolder}{Template1ImagePath}");
            var template2 = Image.FromFile($"{storageFolder}{Template2ImagePath}");
            var template3 = Image.FromFile($"{storageFolder}{Template3ImagePath}");
            var template4 = Image.FromFile($"{storageFolder}{Template4ImagePath}");

            var corners = _cornerFinder.Find(image, template1, template2, template3, template4);
            var whiteBoardRectangle = new WhiteBoardRectangle(image, corners);

            // TODO Right now, this does nothing. Fix it.
            image = _imageRotator.RotateImageAccordingToCorners(image, corners);

            var cropFilter = new Crop(new Rectangle(whiteBoardRectangle.X, whiteBoardRectangle.Y, whiteBoardRectangle.Width, whiteBoardRectangle.Height));
            image = cropFilter.Apply(image);

            // HoloLens needs image that is upside-down
            image = _imageRotator.RotateImage(image, 180);

            image.Save($"{storageFolder}{OutputImagePath}");

            stopwatch.Stop();

            Debug.WriteLine($"Whiteboard extraction took {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
