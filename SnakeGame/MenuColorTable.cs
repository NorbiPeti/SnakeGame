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
        public override Color MenuStripGradientEnd
        {
            get
            {
                return Color.Blue;
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
