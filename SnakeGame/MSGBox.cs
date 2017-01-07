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
        public static EventHandler OnCloseEvent { get; private set; }
        public static bool Shown = false;
        public static void ShowMSGBox(string text, string inputtext, MSGBoxType type, EventHandler<string> doneevent = null)
        {
            if (Shown)
                return;
            Game.DialogPanel.Controls.Add(new Label { Text = text, Location = new Point(10, 10), Font = new Font(FontFamily.GenericSansSerif, 10f), Size = new Size(200, 50) });
            switch (type)
            {
                case MSGBoxType.Text:
                    Game.DialogPanel.Size = new Size(200, 150);
                    slidervalue = 0;
                    break;
                case MSGBoxType.SizeInput:
                    Game.DialogPanel.Size = new Size(200, 200);
                    Game.DialogPanel.Controls.Add(new Label { Text = inputtext, Location = new Point(10, 60), Size = new Size(100, 20) });
                    slidervalue = 20;
                    Graphics gr = Game.DialogPanel.CreateGraphics();
                    gr.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(10, 80, slidervalue, slidervalue));
                    Game.DialogPanel.Paint += delegate
                    {
                        if (slidervalue == 0)
                            return;
                        gr.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(10, 80, slidervalue, slidervalue));
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
                case MSGBoxType.MultipleInput:
                    string[] txts = inputtext.Split('\n');
                    for (int i = 0; i < txts.Length; i++)
                    {
                        Game.DialogPanel.Controls.Add(new Label { Text = txts[i], Location = new Point(10, 60 + i * 30), Size = new Size(80, 30) });
                        Game.DialogPanel.Controls.Add(new TextBox
                        {
                            Text = "",
                            Location = new Point(100, 60 + i * 30),
                            Size = new Size(100, 30),
                            Multiline = false
                        });
                    }
                    Game.DialogPanel.Size = new Size(200, 60 + txts.Length * 30 + 40);
                    break;
                case MSGBoxType.List:
                    Game.DialogPanel.Size = new Size(200, 200);
                    string[] strs;
                    if (inputtext != null) //Combine method can produce null strings
                        strs = inputtext.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        strs = new string[0];
                    ListBox listbox = new ListBox { Location = new Point(10, 60), Size = new Size(190, 100) };
                    for (int i = 0; i < strs.Length; i++)
                    {
                        listbox.Items.Add(strs[i]);
                    }
                    Game.DialogPanel.Controls.Add(listbox);
                    break;
                default:
                    throw new NotImplementedException();
            }
            Game.DialogPanel.Location = new Point(GameRenderer.Panel.Width / 2 - Game.DialogPanel.Width / 2, GameRenderer.Panel.Height / 2 - Game.DialogPanel.Height / 2);
            Game.DialogPanel.Location = Game.DialogPanel.Parent.PointToClient(GameRenderer.Panel.PointToScreen(Game.DialogPanel.Location));
            Button btn = new Button();
            btn.Text = "OK";
            switch (type)
            {
                case MSGBoxType.Text:
                    OnCloseEvent += delegate
                    {
                        Shown = false;
                        if (doneevent != null)
                            doneevent(btn, "");
                        CloseMSGBox();
                    };
                    break;
                case MSGBoxType.SizeInput:
                    OnCloseEvent += delegate
                    {
                        Shown = false;
                        if (doneevent != null)
                            doneevent(btn, slidervalue.ToString());
                        CloseMSGBox();
                    };
                    break;
                case MSGBoxType.MultipleInput:
                    OnCloseEvent += delegate
                    {
                        Shown = false;
                        if (doneevent != null)
                        {
                            string str = "";
                            foreach (Control control in Game.DialogPanel.Controls)
                            {
                                if (control is TextBox)
                                {
                                    str += control.Text + "\n";
                                }
                            }
                            str = str.Remove(str.Length - 1);
                            doneevent(btn, str);
                        }
                        CloseMSGBox();
                    };
                    break;
                case MSGBoxType.List:
                    OnCloseEvent += delegate
                    {
                        Shown = false;
                        if (doneevent != null)
                        {
                            int index = -1;
                            foreach (Control control in Game.DialogPanel.Controls)
                            {
                                if (control is ListBox)
                                {
                                    index = (control as ListBox).SelectedIndex;
                                    break;
                                }
                            }
                            doneevent(btn, index.ToString());
                        }
                        CloseMSGBox();
                    };
                    break;
            }
            btn.Click += OnCloseEvent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Blue;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.CheckedBackColor = Color.DarkBlue;
            btn.FlatAppearance.MouseDownBackColor = Color.DeepSkyBlue;
            btn.Location = new Point(Game.DialogPanel.Size.Width / 2 - 40, Game.DialogPanel.Size.Height - 40);
            btn.Size = new Size(80, 30);
            Game.DialogPanel.Controls.Add(btn);
            pause = Game.Paused;
            Game.Paused = true;
            //pause = pauseafter;
            Game.DialogPanel.Visible = true;
            Shown = true;
        }
        public static void CloseMSGBox()
        {
            Game.DialogPanel.Visible = false;
            Game.DialogPanel.Controls.Clear();
            OnCloseEvent = null;
            Form1.Instance.Focus();
        }
    }
    public enum MSGBoxType
    {
        Text,
        SizeInput,
        MultipleInput,
        List
    }
}
