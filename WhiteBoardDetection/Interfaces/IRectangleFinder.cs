using System.Collections.Generic;
using System.Drawing;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IRectangleFinder
    {
        IReadOnlyCollection<RectangularContour> Find(Bitmap image);
    }
}
