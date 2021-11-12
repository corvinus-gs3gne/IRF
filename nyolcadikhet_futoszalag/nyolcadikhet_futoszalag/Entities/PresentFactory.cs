using nyolcadikhet_futoszalag.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyolcadikhet_futoszalag.Entities
{
    class PresentFactory : IToyFactory
    {
        public Color Boxcolor { get; set; }
        public Color Ribboncolor { get; set; }
        public Toy CreateNew()
        {
            return new Present(Ribboncolor, Boxcolor);
        }
    }
}
