
namespace Dirac.Window
{
    partial class ServerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.button2 = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.nsTheme1 = new NSTheme();
            this.nsTabControl1 = new NSTabControl();
            this.tabPage_General = new System.Windows.Forms.TabPage();
            this.nsGroupBox2 = new NSGroupBox();
            this.nsLabel5 = new NSLabel();
            this.nsLabel4 = new NSLabel();
            this.nsLabel3 = new NSLabel();
            this.nsLabel2 = new NSLabel();
            this.nsLabel1 = new NSLabel();
            this.tabPage_Logger = new System.Windows.Forms.TabPage();
            this.fastColoredTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.nsLabel6 = new NSLabel();
            this.nsButton1 = new NSButton();
            this.tabPage_Debug = new System.Windows.Forms.TabPage();
            this.nsGroupBox1 = new NSGroupBox();
            this.nsRandomPool1 = new NSRandomPool();
            this.tabPage_About = new System.Windows.Forms.TabPage();
            this.nsGroupBox3 = new NSGroupBox();
            this.nsTextBox2 = new NSTextBox();
            this.nsTextBox1 = new NSTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nsControlButton1 = new NSControlButton();
            this.nsTheme1.SuspendLayout();
            this.nsTabControl1.SuspendLayout();
            this.tabPage_General.SuspendLayout();
            this.nsGroupBox2.SuspendLayout();
            this.tabPage_Logger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).BeginInit();
            this.tabPage_Debug.SuspendLayout();
            this.nsGroupBox1.SuspendLayout();
            this.tabPage_About.SuspendLayout();
            this.nsGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(-71, -183);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // nsTheme1
            // 
            this.nsTheme1.AccentOffset = 0;
            this.nsTheme1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.nsTheme1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsTheme1.Colors = new Bloom[0];
            this.nsTheme1.Controls.Add(this.nsTabControl1);
            this.nsTheme1.Controls.Add(this.nsControlButton1);
            this.nsTheme1.Customization = "";
            this.nsTheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsTheme1.Font = new System.Drawing.Font("Wasco Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsTheme1.Image = null;
            this.nsTheme1.Location = new System.Drawing.Point(0, 0);
            this.nsTheme1.Movable = false;
            this.nsTheme1.Name = "nsTheme1";
            this.nsTheme1.NoRounding = false;
            this.nsTheme1.Sizable = false;
            this.nsTheme1.Size = new System.Drawing.Size(702, 539);
            this.nsTheme1.SmartBounds = true;
            this.nsTheme1.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.nsTheme1.TabIndex = 17;
            this.nsTheme1.Text = "MU Server";
            this.nsTheme1.TransparencyKey = System.Drawing.Color.Empty;
            this.nsTheme1.Transparent = false;
            this.nsTheme1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nsTheme1_MouseDown);
            // 
            // nsTabControl1
            // 
            this.nsTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.nsTabControl1.Controls.Add(this.tabPage_General);
            this.nsTabControl1.Controls.Add(this.tabPage_Logger);
            this.nsTabControl1.Controls.Add(this.tabPage_Debug);
            this.nsTabControl1.Controls.Add(this.tabPage_About);
            this.nsTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.nsTabControl1.ItemSize = new System.Drawing.Size(28, 115);
            this.nsTabControl1.Location = new System.Drawing.Point(12, 41);
            this.nsTabControl1.Multiline = true;
            this.nsTabControl1.Name = "nsTabControl1";
            this.nsTabControl1.SelectedIndex = 0;
            this.nsTabControl1.Size = new System.Drawing.Size(678, 486);
            this.nsTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.nsTabControl1.TabIndex = 1;
            // 
            // tabPage_General
            // 
            this.tabPage_General.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabPage_General.Controls.Add(this.nsGroupBox2);
            this.tabPage_General.Location = new System.Drawing.Point(119, 4);
            this.tabPage_General.Name = "tabPage_General";
            this.tabPage_General.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_General.Size = new System.Drawing.Size(555, 478);
            this.tabPage_General.TabIndex = 2;
            this.tabPage_General.Text = "General";
            // 
            // nsGroupBox2
            // 
            this.nsGroupBox2.Controls.Add(this.nsLabel5);
            this.nsGroupBox2.Controls.Add(this.nsLabel4);
            this.nsGroupBox2.Controls.Add(this.nsLabel3);
            this.nsGroupBox2.Controls.Add(this.nsLabel2);
            this.nsGroupBox2.Controls.Add(this.nsLabel1);
            this.nsGroupBox2.DrawSeperator = false;
            this.nsGroupBox2.Location = new System.Drawing.Point(6, 6);
            this.nsGroupBox2.Name = "nsGroupBox2";
            this.nsGroupBox2.Size = new System.Drawing.Size(543, 466);
            this.nsGroupBox2.SubTitle = "Statics";
            this.nsGroupBox2.TabIndex = 0;
            this.nsGroupBox2.Text = "nsGroupBox2";
            this.nsGroupBox2.Title = "General";
            this.nsGroupBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nsGroupBox2_MouseDown);
            // 
            // nsLabel5
            // 
            this.nsLabel5.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsLabel5.Location = new System.Drawing.Point(171, 437);
            this.nsLabel5.Name = "nsLabel5";
            this.nsLabel5.Size = new System.Drawing.Size(196, 26);
            this.nsLabel5.TabIndex = 4;
            this.nsLabel5.Text = "nsLabel5";
            this.nsLabel5.Value1 = "Time Online";
            this.nsLabel5.Value2 = " 13:45:30";
            this.nsLabel5.Click += new System.EventHandler(this.nsLabel5_Click);
            // 
            // nsLabel4
            // 
            this.nsLabel4.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsLabel4.Location = new System.Drawing.Point(3, 141);
            this.nsLabel4.Name = "nsLabel4";
            this.nsLabel4.Size = new System.Drawing.Size(168, 26);
            this.nsLabel4.TabIndex = 3;
            this.nsLabel4.Text = "nsLabel4";
            this.nsLabel4.Value1 = "Warns";
            this.nsLabel4.Value2 = " 0";
            // 
            // nsLabel3
            // 
            this.nsLabel3.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsLabel3.Location = new System.Drawing.Point(3, 109);
            this.nsLabel3.Name = "nsLabel3";
            this.nsLabel3.Size = new System.Drawing.Size(168, 26);
            this.nsLabel3.TabIndex = 2;
            this.nsLabel3.Text = "nsLabel3";
            this.nsLabel3.Value1 = "Errors";
            this.nsLabel3.Value2 = " 0";
            // 
            // nsLabel2
            // 
            this.nsLabel2.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsLabel2.Location = new System.Drawing.Point(3, 45);
            this.nsLabel2.Name = "nsLabel2";
            this.nsLabel2.Size = new System.Drawing.Size(168, 26);
            this.nsLabel2.TabIndex = 1;
            this.nsLabel2.Text = "nsLabel2";
            this.nsLabel2.Value1 = "Players Online";
            this.nsLabel2.Value2 = " 0";
            // 
            // nsLabel1
            // 
            this.nsLabel1.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsLabel1.Location = new System.Drawing.Point(3, 77);
            this.nsLabel1.Name = "nsLabel1";
            this.nsLabel1.Size = new System.Drawing.Size(168, 26);
            this.nsLabel1.TabIndex = 0;
            this.nsLabel1.Text = "nsLabel1";
            this.nsLabel1.Value1 = "FPS";
            this.nsLabel1.Value2 = " 19";
            // 
            // tabPage_Logger
            // 
            this.tabPage_Logger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabPage_Logger.Controls.Add(this.fastColoredTextBox1);
            this.tabPage_Logger.Controls.Add(this.nsLabel6);
            this.tabPage_Logger.Controls.Add(this.nsButton1);
            this.tabPage_Logger.Location = new System.Drawing.Point(119, 4);
            this.tabPage_Logger.Name = "tabPage_Logger";
            this.tabPage_Logger.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Logger.Size = new System.Drawing.Size(555, 478);
            this.tabPage_Logger.TabIndex = 0;
            this.tabPage_Logger.Text = "Logger";
            // 
            // fastColoredTextBox1
            // 
            this.fastColoredTextBox1.AllowDrop = false;
            this.fastColoredTextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBox1.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.fastColoredTextBox1.BackBrush = null;
            this.fastColoredTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.fastColoredTextBox1.CaretVisible = false;
            this.fastColoredTextBox1.CharHeight = 15;
            this.fastColoredTextBox1.CharWidth = 7;
            this.fastColoredTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBox1.DelayedEventsInterval = 10;
            this.fastColoredTextBox1.DelayedTextChangedInterval = 10;
            this.fastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastColoredTextBox1.IsReplaceMode = false;
            this.fastColoredTextBox1.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.fastColoredTextBox1.Location = new System.Drawing.Point(6, 35);
            this.fastColoredTextBox1.Name = "fastColoredTextBox1";
            this.fastColoredTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBox1.ReadOnly = true;
            this.fastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.fastColoredTextBox1.Size = new System.Drawing.Size(543, 437);
            this.fastColoredTextBox1.TabIndex = 3;
            this.fastColoredTextBox1.Zoom = 100;
            // 
            // nsLabel6
            // 
            this.nsLabel6.B1 = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsLabel6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.nsLabel6.Location = new System.Drawing.Point(232, 6);
            this.nsLabel6.Name = "nsLabel6";
            this.nsLabel6.Size = new System.Drawing.Size(116, 23);
            this.nsLabel6.TabIndex = 2;
            this.nsLabel6.Text = "nsLabel6";
            this.nsLabel6.Value1 = "Window";
            this.nsLabel6.Value2 = "Logger";
            // 
            // nsButton1
            // 
            this.nsButton1.Location = new System.Drawing.Point(6, 6);
            this.nsButton1.Name = "nsButton1";
            this.nsButton1.Size = new System.Drawing.Size(51, 23);
            this.nsButton1.TabIndex = 1;
            this.nsButton1.Text = "Clear";
            this.nsButton1.Click += new System.EventHandler(this.nsButton1_Click);
            // 
            // tabPage_Debug
            // 
            this.tabPage_Debug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabPage_Debug.Controls.Add(this.nsGroupBox1);
            this.tabPage_Debug.Location = new System.Drawing.Point(119, 4);
            this.tabPage_Debug.Name = "tabPage_Debug";
            this.tabPage_Debug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Debug.Size = new System.Drawing.Size(555, 478);
            this.tabPage_Debug.TabIndex = 1;
            this.tabPage_Debug.Text = "Debug";
            // 
            // nsGroupBox1
            // 
            this.nsGroupBox1.Controls.Add(this.nsRandomPool1);
            this.nsGroupBox1.DrawSeperator = true;
            this.nsGroupBox1.Location = new System.Drawing.Point(6, 6);
            this.nsGroupBox1.Name = "nsGroupBox1";
            this.nsGroupBox1.Size = new System.Drawing.Size(543, 466);
            this.nsGroupBox1.SubTitle = "Debug";
            this.nsGroupBox1.TabIndex = 0;
            this.nsGroupBox1.Text = "Inventory";
            this.nsGroupBox1.Title = "Inventory";
            // 
            // nsRandomPool1
            // 
            this.nsRandomPool1.Location = new System.Drawing.Point(3, 46);
            this.nsRandomPool1.Name = "nsRandomPool1";
            this.nsRandomPool1.Size = new System.Drawing.Size(537, 417);
            this.nsRandomPool1.TabIndex = 0;
            this.nsRandomPool1.Text = "nsRandomPool1";
            // 
            // tabPage_About
            // 
            this.tabPage_About.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabPage_About.Controls.Add(this.nsGroupBox3);
            this.tabPage_About.Location = new System.Drawing.Point(119, 4);
            this.tabPage_About.Name = "tabPage_About";
            this.tabPage_About.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_About.Size = new System.Drawing.Size(555, 478);
            this.tabPage_About.TabIndex = 3;
            this.tabPage_About.Text = "About";
            // 
            // nsGroupBox3
            // 
            this.nsGroupBox3.Controls.Add(this.nsTextBox2);
            this.nsGroupBox3.Controls.Add(this.nsTextBox1);
            this.nsGroupBox3.Controls.Add(this.pictureBox1);
            this.nsGroupBox3.DrawSeperator = false;
            this.nsGroupBox3.Location = new System.Drawing.Point(6, 6);
            this.nsGroupBox3.Name = "nsGroupBox3";
            this.nsGroupBox3.Size = new System.Drawing.Size(543, 466);
            this.nsGroupBox3.SubTitle = "";
            this.nsGroupBox3.TabIndex = 1;
            this.nsGroupBox3.Text = "nsGroupBox3";
            this.nsGroupBox3.Title = "About";
            // 
            // nsTextBox2
            // 
            this.nsTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nsTextBox2.Enabled = false;
            this.nsTextBox2.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsTextBox2.Location = new System.Drawing.Point(113, 417);
            this.nsTextBox2.MaxLength = 32767;
            this.nsTextBox2.Multiline = false;
            this.nsTextBox2.Name = "nsTextBox2";
            this.nsTextBox2.ReadOnly = true;
            this.nsTextBox2.Size = new System.Drawing.Size(319, 34);
            this.nsTextBox2.TabIndex = 2;
            this.nsTextBox2.Text = "left to right: izzy - chicky habana, mACH1VO";
            this.nsTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nsTextBox2.UseSystemPasswordChar = false;
            // 
            // nsTextBox1
            // 
            this.nsTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nsTextBox1.Enabled = false;
            this.nsTextBox1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nsTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.nsTextBox1.Location = new System.Drawing.Point(78, 42);
            this.nsTextBox1.MaxLength = 32767;
            this.nsTextBox1.Multiline = true;
            this.nsTextBox1.Name = "nsTextBox1";
            this.nsTextBox1.ReadOnly = true;
            this.nsTextBox1.Size = new System.Drawing.Size(371, 35);
            this.nsTextBox1.TabIndex = 1;
            this.nsTextBox1.Text = "MU GAME SERVER ORIGINAL LOGO!";
            this.nsTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.nsTextBox1.UseSystemPasswordChar = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(113, 107);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(319, 295);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // nsControlButton1
            // 
            this.nsControlButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nsControlButton1.ControlButton = NSControlButton.Button.Close;
            this.nsControlButton1.Location = new System.Drawing.Point(680, 5);
            this.nsControlButton1.Margin = new System.Windows.Forms.Padding(0);
            this.nsControlButton1.MaximumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.MinimumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.Name = "nsControlButton1";
            this.nsControlButton1.Size = new System.Drawing.Size(18, 20);
            this.nsControlButton1.TabIndex = 0;
            this.nsControlButton1.Text = "nsControlButton1";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 539);
            this.Controls.Add(this.nsTheme1);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ServerForm";
            this.nsTheme1.ResumeLayout(false);
            this.nsTabControl1.ResumeLayout(false);
            this.tabPage_General.ResumeLayout(false);
            this.nsGroupBox2.ResumeLayout(false);
            this.tabPage_Logger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).EndInit();
            this.tabPage_Debug.ResumeLayout(false);
            this.nsGroupBox1.ResumeLayout(false);
            this.tabPage_About.ResumeLayout(false);
            this.nsGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer updateTimer;
        private NSTheme nsTheme1;
        private NSControlButton nsControlButton1;
        private NSTabControl nsTabControl1;
        private System.Windows.Forms.TabPage tabPage_Logger;
        private System.Windows.Forms.TabPage tabPage_Debug;
        private NSButton nsButton1;
        private NSGroupBox nsGroupBox1;
        private NSRandomPool nsRandomPool1;
        private System.Windows.Forms.TabPage tabPage_General;
        private NSGroupBox nsGroupBox2;
        private NSLabel nsLabel5;
        private NSLabel nsLabel4;
        private NSLabel nsLabel3;
        private NSLabel nsLabel2;
        private NSLabel nsLabel1;
        private NSLabel nsLabel6;
        private System.Windows.Forms.TabPage tabPage_About;
        private NSGroupBox nsGroupBox3;
        private NSTextBox nsTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private NSTextBox nsTextBox2;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBox1;
    }
}