using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public class MenuColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get
            {
                return Color.Black;
            }
        }
        public override Color MenuStripGradientBegin
        {
            get
            {
                return Color.Black;
            }
        }
        private Color prevcolor = Color.Blue;
        private bool blue = false;
        public override Color MenuStripGradientEnd
        {
            get
            {
                //return Color.Blue;
                //return Game.GetRandomColor();
                Color color = prevcolor;
                if (color.G - 10 <= 0/* || color.B + 10 >= byte.MaxValue*/)
                    blue = false;
                else if (/*color.B - 10 <= 0 || */color.G + 10 >= byte.MaxValue)
                    blue = true;
                if (blue)
                    prevcolor = Color.FromArgb(color.R, color.G - 10, color.B);
                else
                    prevcolor = Color.FromArgb(color.R, color.G + 10, color.B);
                return color;
            }
        }
        public override Color MenuBorder
        {
            get
            {
                return Color.Black;
            }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return Color.Aqua;
            }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return Color.Black;
            }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Color.Blue;
            }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Color.Black;
            }
        }
        public override Color MenuItemBorder
        {
            get
            {
                return Color.Blue;
            }
        }
    }
}
