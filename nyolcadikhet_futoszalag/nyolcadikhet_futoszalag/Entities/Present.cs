using nyolcadikhet_futoszalag.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyolcadikhet_futoszalag.Entities
{
    class Present : Toy
    {
        public SolidBrush BoxColor { get; private set; }
        public SolidBrush RibbonColor { get; private set; }
        public Present(Color ribbon,Color  box)
        {
            BoxColor = new SolidBrush(box);
            RibbonColor = new SolidBrush(ribbon);
        }

        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(BoxColor, 0, 0, Width, Height);
            g.FillRectangle(RibbonColor, 0, 0, Width / 5, Height);
            g.FillRectangle(RibbonColor, 0, 0, Width, Height/5);
        }
    }
}
