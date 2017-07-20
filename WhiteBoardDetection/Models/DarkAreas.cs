using System.Collections.Generic;
using System.Drawing;
using AForge;

namespace WhiteBoardDetection.Models
{
    public class DarkAreas
    {
        public long ImageWidth { get; set; }

        public long ImageHeight { get; set; }

        public IList<IntPoint> Points { get; set; }
    }
}
