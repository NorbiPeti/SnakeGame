using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                if (value && !timerenabled) //Only start if not running already
                {
                    Timer.Start();
                    SpeedTimer.Start();
                }
                timerenabled = value;
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
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
                MSGBox.CloseMSGBox();
            }
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
    }
}
