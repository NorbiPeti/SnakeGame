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
        public static Timer Timer;
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
            Timer = new Timer();
            Timer.Interval = Game.UpdateTime;
            Timer.Tick += delegate
            {
                Timer.Stop();
                Game.Refresh();
                Timer.Interval = Game.UpdateTime;
                Timer.Start();
            };
            Timer.Start();
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
        }
    }
}
