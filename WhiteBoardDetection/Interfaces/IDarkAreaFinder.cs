using System.Collections.Generic;
using System.Drawing;
using AForge;

namespace WhiteBoardDetection.Interfaces
{
    public interface IDarkAreaFinder
    {
        IEnumerable<IntPoint> Find(Bitmap image);
    }
}