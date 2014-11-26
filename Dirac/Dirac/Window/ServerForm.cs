using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using System.Net;
using System.Net.Sockets;

using Dirac.GameServer;
using Dirac.Logging;
using Dirac;
using FastColoredTextBoxNS;



namespace Dirac.Window
{
    public partial class ServerForm : Form
    {
        public TextStyle traceStyle = new TextStyle(Brushes.WhiteSmoke, null, FontStyle.Regular);
        public TextStyle debugStyle = new TextStyle(Brushes.Cyan, null, FontStyle.Regular);
        public TextStyle infoStyle = new TextStyle(Brushes.White, null, FontStyle.Regular);
        public TextStyle warnStyle = new TextStyle(Brushes.Yellow, null, FontStyle.Regular);
        public TextStyle errorStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        public TextStyle fatalStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        public static ServerForm Instance { get { return _instance; } }

        /// <summary>
        /// The internal instance pointer.
        /// </summary>
        private static ServerForm _instance;



        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public ServerForm()
        {
            _instance = this;
            InitializeComponent();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            this.Text = "FPS " + Game.CurrentFPS.ToString();
            /*this.richTextBox_Inventory.Text = StatisticsTest.Instance.inventoryText;
            this.richTextBox_Vault.Text = StatisticsTest.Instance.vaultText;*/
        }

        public void AddTextToFastColoredTextBox(String text, Dirac.Logging.Logger.Level level)
        {
            TextStyle style = this._getTextStyleFromLogLevel(level);
            if (this.fastColoredTextBox1.InvokeRequired)
            {
                Action action_append_text = () => 
                {
                    //some stuffs for best performance
                    fastColoredTextBox1.BeginUpdate();
                    fastColoredTextBox1.Selection.BeginUpdate();
                    //remember user selection
                    var userSelection = fastColoredTextBox1.Selection.Clone();
                    //add text with predefined style
                    fastColoredTextBox1.TextSource.CurrentTB = fastColoredTextBox1;
                    fastColoredTextBox1.AppendText(text, style);
                    //restore user selection
                    if (!userSelection.IsEmpty || userSelection.Start.iLine < fastColoredTextBox1.LinesCount - 2)
                    {
                        fastColoredTextBox1.Selection.Start = userSelection.Start;
                        fastColoredTextBox1.Selection.End = userSelection.End;
                    }
                    else
                        fastColoredTextBox1.GoEnd();//scroll to end of the text
                    //
                    fastColoredTextBox1.Selection.EndUpdate();
                    fastColoredTextBox1.EndUpdate();

                    this.fastColoredTextBox1.GoEnd();
                };

                this.Invoke(action_append_text, null);
            }
            else
            {
                //this.fastColoredTextBox1.AppendText(line + Environment.NewLine, textStyle);
                //this.fastColoredTextBox1.GoEnd();
            }
        }

        private TextStyle _getTextStyleFromLogLevel(Dirac.Logging.Logger.Level level)
        {
            switch (level)
            {
                case Logger.Level.Trace:
                case Logger.Level.PacketDump:
                    return this.traceStyle;
                case Logger.Level.Debug:
                    return this.debugStyle;
                case Logger.Level.Info:
                    return this.infoStyle;
                case Logger.Level.Warn:
                    return this.warnStyle;
                case Logger.Level.Error:
                    return this.errorStyle;
                case Logger.Level.Fatal:
                    return this.fatalStyle;
                default:
                    return this.traceStyle;
            }
        }

        private void trackBarUpdateFrequency_ValueChanged(object sender, EventArgs e)
        {
            /*if (trackBarUpdateFrequency.Value == 0)
            {
                this.updateTimer.Enabled = false;
                this.trackBarUpdateFrequency.BackColor = System.Drawing.Color.Silver;
                this.UpdateFrequencyLabel.Text = "FPS " + trackBarUpdateFrequency.Value.ToString();
            }
            else
            {
                this.trackBarUpdateFrequency.BackColor = System.Drawing.Color.WhiteSmoke;
                this.updateTimer.Interval =  1000 / trackBarUpdateFrequency.Value;
                this.UpdateFrequencyLabel.Text = "FPS " + ((int)(1000 / this.updateTimer.Interval)).ToString();
                this.updateTimer.Enabled = true;
            }*/
        }

        private void trackBarUpdateFrequency_Scroll(object sender, EventArgs e)
        {
           /*if (this.trackBarUpdateFrequency.Value == 0)
            {
                this.renderWindow1.EnableDrawWorld = false;
            }
            else
            {
                this.renderWindow1.EnableDrawWorld = true;
            }

            this.renderWindow1.Invalidate();*/
        }

        public void Log(string text, Style style)
        {
            //some stuffs for best performance
        }

        public void LogPacket(string text, TextStyle style)
        {
            /*if (this.fastColoredTextBox1.InvokeRequired)
            {
                LogPacketDelegate d = new LogPacketDelegate(_logPacket);
                this.Invoke(d, new object[] { text, style });
            }*/
            
        }

        delegate void LogPacketDelegate(string text, TextStyle style);


        private void button1_Click(object sender, EventArgs e)
        {
            /*Executor.Execute(1, () =>
                {
                    if (GameServer.Network.GameServer.ClientList.Count > 0)
                    {
                        GameServer.Network.GameClient gc = GameServer.Network.GameServer.ClientList.Values.FirstOrDefault();
                        GameServer.Network.Message.GameSetupMessage gsm = new GameServer.Network.Message.GameSetupMessage();
                        gsm.Field0 = 5;
                        gc.SendMessage(gsm, true);

                    }
                });*/
        }

        private void nsTheme1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); 
        }

        private void nsButton1_Click(object sender, EventArgs e)
        {
            this.fastColoredTextBox1.Clear();
        }

        private void nsGroupBox2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); 
        }

        private void nsLabel5_Click(object sender, EventArgs e)
        {

        }
    }
}
