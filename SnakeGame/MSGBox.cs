using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public static class MSGBox
    {
        private static int slidervalue;
        private static bool pause;
        public static void ShowMSGBox(string text, string inputtext, MSGBoxType type, EventHandler<int> doneevent = null, bool pauseafter = false)
        {
            Game.DialogPanel.Controls.Add(new Label { Text = text, Location = new Point(10, 10), Font = new Font(FontFamily.GenericSansSerif, 10f), Size = new Size(200, 20) });
            //TextBox input = null;
            switch (type)
            {
                case MSGBoxType.Text:
                    Game.DialogPanel.Size = new Size(200, 100);
                    slidervalue = 0;
                    break;
                case MSGBoxType.SizeInput:
                    Game.DialogPanel.Size = new Size(200, 200);
                    Game.DialogPanel.Controls.Add(new Label { Text = inputtext, Location = new Point(10, 40), Size = new Size(100, 20) });
                    slidervalue = 20;
                    /*Game.DialogPanel.Controls.Add(input = new TextBox
                    {
                        Text = "",
                        Location = new Point(10, 60),
                        Multiline = false,
                        Font = new Font(FontFamily.GenericSansSerif, 10f),
                        Size = new Size(100, 1)
                    });*/
                    Graphics gr = Game.DialogPanel.CreateGraphics();
                    gr.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(10, 60, slidervalue, slidervalue));
                    Game.DialogPanel.Paint += delegate
                    {
                        if (slidervalue == 0)
                            return;
                        gr.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(10, 60, slidervalue, slidervalue));
                    };
                    Game.DialogPanel.Click += delegate
                    {
                        if (slidervalue == 0)
                            return;
                        Point p = Game.DialogPanel.PointToClient(Cursor.Position);
                        if (p.Y > 50 && p.Y < 200)
                        {
                            if (p.X < 210) //p.X - 10 < 100
                                slidervalue = p.X - 10;
                            Game.DialogPanel.Refresh();
                        }
                    };
                    break;
                default:
                    throw new NotImplementedException();
            }
            Game.DialogPanel.Location = new Point(GameRenderer.Panel.Width / 2 - Game.DialogPanel.Width / 2, GameRenderer.Panel.Height / 2 - Game.DialogPanel.Height / 2);
            Game.DialogPanel.Location = Game.DialogPanel.Parent.PointToClient(GameRenderer.Panel.PointToScreen(Game.DialogPanel.Location));
            Button btn = new Button();
            btn.Text = "OK";
            btn.Click += delegate
            {
                CloseMSGBox();
                if (doneevent != null)
                    //doneevent(btn, (input == null ? "" : input.Text));
                    doneevent(btn, slidervalue);
            };
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Blue;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.CheckedBackColor = Color.DarkBlue;
            btn.FlatAppearance.MouseDownBackColor = Color.DeepSkyBlue;
            btn.Location = new Point(Game.DialogPanel.Size.Width / 2 - 40, Game.DialogPanel.Size.Height - 40);
            btn.Size = new Size(80, 30);
            Game.DialogPanel.Controls.Add(btn);
            Game.Paused = true;
            pause = pauseafter;
            Game.DialogPanel.Visible = true;
        }
        public static void CloseMSGBox()
        {
            Game.DialogPanel.Visible = false;
            Game.DialogPanel.Controls.Clear();
            Game.Paused = pause;
            //Form1.Instance.Activate();
            //GameRenderer.Panel.Select();
            //Form1.Instance.Select();
            Form1.Instance.Focus();
        }
    }
    public enum MSGBoxType
    {
        Text,
        SizeInput
    }
}
