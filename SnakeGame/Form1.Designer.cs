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
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.livesLabel = new System.Windows.Forms.Label();
            this.DialogPanel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 357);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.toolStripTextBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 27);
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
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 23);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newSingleplayerGameToolStripMenuItem
            // 
            this.newSingleplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.newSingleplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newSingleplayerGameToolStripMenuItem.Name = "newSingleplayerGameToolStripMenuItem";
            this.newSingleplayerGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSingleplayerGameToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.newSingleplayerGameToolStripMenuItem.Text = "New singleplayer game";
            this.newSingleplayerGameToolStripMenuItem.Click += new System.EventHandler(this.newSingleplayerGameToolStripMenuItem_Click);
            // 
            // newMultiplayerGameToolStripMenuItem
            // 
            this.newMultiplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.newMultiplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newMultiplayerGameToolStripMenuItem.Name = "newMultiplayerGameToolStripMenuItem";
            this.newMultiplayerGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.newMultiplayerGameToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.newMultiplayerGameToolStripMenuItem.Text = "New multiplayer game";
            this.newMultiplayerGameToolStripMenuItem.Click += new System.EventHandler(this.newMultiplayerGameToolStripMenuItem_Click);
            // 
            // joinMultiplayerGameToolStripMenuItem
            // 
            this.joinMultiplayerGameToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.joinMultiplayerGameToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.joinMultiplayerGameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.joinMultiplayerGameToolStripMenuItem.Name = "joinMultiplayerGameToolStripMenuItem";
            this.joinMultiplayerGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.N)));
            this.joinMultiplayerGameToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.joinMultiplayerGameToolStripMenuItem.Text = "Join multiplayer game";
            this.joinMultiplayerGameToolStripMenuItem.Click += new System.EventHandler(this.joinMultiplayerGameToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.Color.Black;
            this.toolStripTextBox1.ForeColor = System.Drawing.Color.White;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "UserName";
            this.toolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(14, 31);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(35, 13);
            this.scoreLabel.TabIndex = 2;
            this.scoreLabel.Text = "Score";
            // 
            // livesLabel
            // 
            this.livesLabel.AutoSize = true;
            this.livesLabel.Location = new System.Drawing.Point(14, 48);
            this.livesLabel.Name = "livesLabel";
            this.livesLabel.Size = new System.Drawing.Size(32, 13);
            this.livesLabel.TabIndex = 3;
            this.livesLabel.Text = "Lives";
            // 
            // DialogPanel
            // 
            this.DialogPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DialogPanel.AutoSize = true;
            this.DialogPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DialogPanel.Location = new System.Drawing.Point(200, 228);
            this.DialogPanel.Name = "DialogPanel";
            this.DialogPanel.Size = new System.Drawing.Size(200, 100);
            this.DialogPanel.TabIndex = 0;
            this.DialogPanel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.DialogPanel);
            this.Controls.Add(this.livesLabel);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snake Game";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
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
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label livesLabel;
        private System.Windows.Forms.Panel DialogPanel;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}

