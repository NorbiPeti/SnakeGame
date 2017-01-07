namespace SnakeGame
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSingleplayerGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMultiplayerGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinMultiplayerGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 380);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSingleplayerGameToolStripMenuItem,
            this.newMultiplayerGameToolStripMenuItem,
            this.joinMultiplayerGameToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newSingleplayerGameToolStripMenuItem
            // 
            this.newSingleplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.newSingleplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newSingleplayerGameToolStripMenuItem.Name = "newSingleplayerGameToolStripMenuItem";
            this.newSingleplayerGameToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.newSingleplayerGameToolStripMenuItem.Text = "New singleplayer game";
            // 
            // newMultiplayerGameToolStripMenuItem
            // 
            this.newMultiplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.newMultiplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newMultiplayerGameToolStripMenuItem.Name = "newMultiplayerGameToolStripMenuItem";
            this.newMultiplayerGameToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.newMultiplayerGameToolStripMenuItem.Text = "New multiplayer game";
            // 
            // joinMultiplayerGameToolStripMenuItem
            // 
            this.joinMultiplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.joinMultiplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.joinMultiplayerGameToolStripMenuItem.Name = "joinMultiplayerGameToolStripMenuItem";
            this.joinMultiplayerGameToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.joinMultiplayerGameToolStripMenuItem.Text = "Join multiplayer game";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snake Game";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSingleplayerGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMultiplayerGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinMultiplayerGameToolStripMenuItem;
    }
}

