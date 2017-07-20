using System.Collections.Generic;
using System.Drawing;
using AForge;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IDarkAreaExtractor
    {
        void ExtractDarkAreas(string image);
    }
}