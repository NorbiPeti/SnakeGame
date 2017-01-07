using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPFInput = System.Windows.Input;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private static Timer Timer;
        private static Timer SpeedTimer;
        private static bool timerenabled = false;
        public static Form1 Instance;
        public static bool TimerEnabled
        {
            get
            {
                return timerenabled;
            }
            set
            {
                if (!MSGBox.Shown)
                {
                    if (value && !timerenabled) //Only start if not running already
                    {
                        Timer.Start();
                        SpeedTimer.Start();
                    }
                    timerenabled = value;
                    Instance.toolStripTextBox1.Enabled = !value;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            /*Bitmap img = new Bitmap(1, 1);
            img.SetPixel(0, 0, Color.Black);
            menuStrip1.BackgroundImage = img;
            newSingleplayerGameToolStripMenuItem.BackgroundImage = img;
            newMultiplayerGameToolStripMenuItem.BackgroundImage = img;
            joinMultiplayerGameToolStripMenuItem.BackgroundImage = img;*/
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
            menuStrip1.ForeColor = Color.White;
            GameRenderer.Panel = panel1;
            Game.ScoreLabel = scoreLabel;
            Game.LivesLabel = livesLabel;
            Game.DialogPanel = DialogPanel;
            Instance = this;
            toolStripTextBox1.Text = "Player";
            Timer = new Timer();
            Timer.Interval = Game.UpdateTime;
            Timer.Tick += delegate
            {
                Timer.Stop();
                Game.Refresh();
                Timer.Interval = Game.UpdateTime;
                if (TimerEnabled)
                    Timer.Start();
            };
            //Timer.Start();
            SpeedTimer = new Timer();
            SpeedTimer.Interval = 10000;
            SpeedTimer.Tick += delegate
            {
                if (Game.UpdateTime - 100 > 0)
                    Game.UpdateTime -= 100;
                Game.Length++;
                if (!TimerEnabled)
                    SpeedTimer.Stop();
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Game.Load(panel1.Size);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            GameRenderer.Render();
        }

        //private static bool playerpaused = false;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Down && WPFInput.Keyboard.IsKeyDown(WPFInput.Key.Down))
            if (e.KeyCode == Keys.Down)
                Game.MoveDirection = Direction.Down;
            else if (e.KeyCode == Keys.Up)
                Game.MoveDirection = Direction.Up;
            else if (e.KeyCode == Keys.Left)
                Game.MoveDirection = Direction.Left;
            else if (e.KeyCode == Keys.Right)
                Game.MoveDirection = Direction.Right;
            else if (e.KeyCode == Keys.Enter)
            {
                //MSGBox.CloseMSGBox();
                if (MSGBox.OnCloseEvent != null)
                    MSGBox.OnCloseEvent(sender, e);
            }
            else if (e.KeyCode == Keys.P || e.KeyCode == Keys.Pause)
            {
                Game.Paused = !Game.Paused;
                /*if (Game.Paused)
                    playerpaused = true;
                else
                    playerpaused = false;*/
            }
            else
                return;
            if (!Game.Paused)
                Game.Refresh();
        }

        private void newSingleplayerGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.Start(GameStartMode.SinglePlayer);
        }

        private bool formdeactivated = false;
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (formdeactivated)
                Game.Paused = false;
            formdeactivated = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            formdeactivated = !Game.Paused;
            Game.Paused = true;
        }

        private void newMultiplayerGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.Start(GameStartMode.MultiPlayer);
        }

        private void joinMultiplayerGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.Start(GameStartMode.Connect);
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            Game.Player.Name = toolStripTextBox1.Text;
        }

        //private static Timer resizetimer = new Timer();
        private void Form1_Resize(object sender, EventArgs e)
        {
            GameRenderer.Render();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Graphics gr = panel1.CreateGraphics();
            gr.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(0, 0), panel1.Size));
            GameRenderer.Render();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Network.Leave(); //Stop threads and such
        }
    }
    public static class Ext
    {
        public static void ClearEventInvocations(this object obj, string eventName)
        {
            var fi = obj.GetType().GetEventField(eventName);
            if (fi == null) return;
            fi.SetValue(obj, null);
        }

        private static FieldInfo GetEventField(this Type type, string eventName)
        {
            FieldInfo field = null;
            while (type != null)
            {
                /* Find events defined as field */
                field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                    break;

                /* Find events defined as property { add; remove; } */
                field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                    break;
                type = type.BaseType;
            }
            return field;
        }

        public static TResult Combine<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult, TResult> func)
        {
            TResult combiner = default(TResult);
            foreach (var entry in source)
                combiner = func(entry, combiner);
            return combiner;
        }
    }
}
