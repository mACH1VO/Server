using System;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

using Dirac.Window;

namespace Dirac.Logging
{
    public static class LoggerOld
    {
        //public String log file name y poner esto abajo
        private static Thread backgroundLogThread;
        private static StreamWriter LogFile = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", true);
        private static StreamWriter LogPacketFile = new StreamWriter(Environment.CurrentDirectory + "\\Packets.txt", false);
        private static ConcurrentQueue<LogStruct> LogQueue = new ConcurrentQueue<LogStruct>();
        private static ConcurrentQueue<LogStruct> LogPacketQueue = new ConcurrentQueue<LogStruct>();
        private static LogStruct dequeuedStruct = new LogStruct();

        private static ServerForm f;

        public static void Add(String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            if (args.Length > 0)
            {
                Loggingstruct.Text = String.Format(format, args);
            }
            else
            {
                Loggingstruct.Text = format;
            }
            Loggingstruct.Type = LogType.Trace;
            LogQueue.Enqueue(Loggingstruct);

            if (LogQueue.Count > 1000)
                new Exception("LogQuque > 1000");
            //f.richTextBox_Main.AppendLine(String.Format(format, args), Color.White);
            //f.richTextBox_Main.ScrollToCaret();
        }

        public static void AddWithColor(Color color, String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            if (args.Length > 0)
            {
                Loggingstruct.Text = String.Format(format, args);
            }
            else
            {
                Loggingstruct.Text = format;
            }
            Loggingstruct.Type = LogType.Color;
            Loggingstruct.Color = color;
            LogQueue.Enqueue(Loggingstruct);
            //f.richTextBox_Main.AppendLine(String.Format(format, args), Color.White);
            //f.richTextBox_Main.ScrollToCaret();
        }

        public static void Warn(String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = "[WARN]" + String.Format(format, args);
            Loggingstruct.Type = LogType.Warn;
            LogQueue.Enqueue(Loggingstruct);
        }

        public static void Hack(String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = "[HACK]" + String.Format(format, args);
            Loggingstruct.Type = LogType.Hack;
            LogQueue.Enqueue(Loggingstruct);
        }

        public static void Error(String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = "[ERROR]" + String.Format(format, args);
            Loggingstruct.Type = LogType.Error;
            LogQueue.Enqueue(Loggingstruct);
        }

        public static void Error(Exception ex)
        {
            String fullExText;

            if (ex.InnerException != null)
                fullExText = ex.Message + Environment.NewLine + ex.InnerException.Message + Environment.NewLine + ex.StackTrace;
            else
                fullExText = ex.Message + ex.Data + ex.StackTrace;

            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = "[ERROR]" + fullExText;
            Loggingstruct.Type = LogType.Error;
            LogQueue.Enqueue(Loggingstruct);
        }

        public static void PacketLog(String format, params Object[] args)
        {
            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = String.Format(format, args);
            Loggingstruct.Type = LogType.LogPacket;
            LogPacketQueue.Enqueue(Loggingstruct);
        }

        public static void PacketLog(String format)
        {
            LogStruct Loggingstruct = new LogStruct();
            Loggingstruct.Text = format;
            Loggingstruct.Type = LogType.LogPacket;
            LogPacketQueue.Enqueue(Loggingstruct);
        }

        public static void Initialize(ServerForm _f)
        {
            backgroundLogThread = new Thread(LogWorker);
            backgroundLogThread.IsBackground = true; // close app -> close this thread
            backgroundLogThread.Start();
            f = _f;
            LogStruct Loggingstruct = new LogStruct();

            Loggingstruct.Text = "[Logger initialized]";
            Loggingstruct.Type = LogType.Trace;
            LogQueue.Enqueue(Loggingstruct);

            Loggingstruct.Text = " PacketLogger initialized";
            Loggingstruct.Type = LogType.Trace;
            LogPacketQueue.Enqueue(Loggingstruct);
            
        }

        private static void LogWorker()
        {
            

            bool ScrollState = true;
            while (true)
            {
                if (f == null)
                    continue;

                while (LogQueue.TryDequeue(out dequeuedStruct))
                {
                    LogFile.WriteLine(dequeuedStruct.Text);
                    switch (dequeuedStruct.Type)
                    {
                        case LogType.Trace:
                            f.Log(dequeuedStruct.Text, f.warnStyle);
                            break;
                        /*case LogType.Color:
                            f.Log(dequeuedStruct.Text, Program.serverForm.customColorStyle1);
                            break;
                        case LogType.Warn:
                            f.Log(dequeuedStruct.Text, f.warningStyle);
                            break;
                        case LogType.Error:
                            f.Log(dequeuedStruct.Text, f.errorStyle);
                            break;
                        case LogType.Hack:
                            f.Log(dequeuedStruct.Text, f.errorStyle);
                            break;*/
                    }
                    ScrollState = true;
                    LogFile.Flush();
                    

                }
                while (LogPacketQueue.TryDequeue(out dequeuedStruct))
                {
                    LogPacketFile.WriteLine(dequeuedStruct.Text);
                    //f.richTextBox_PKTLog.AppendLine(dequeuedStruct.Text, Color.White);
                    LogPacketFile.Flush();
                }

                if (ScrollState)
                {
                }

                Thread.Sleep(30);
                ScrollState = false;
            }
        }

        public static void AppendLine(this RichTextBox box, string text, Color color)
        {
            int start = box.TextLength;
            box.AppendText(text + Environment.NewLine);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            box.SelectionLength = 0; // clear
        }

    }

    public struct LogStruct
    {
        public string Text;
        public LogType Type;
        public Color Color; //only used when logType is Color
    }

    public enum LogType
    {
        Trace,
        Warn,
        Error,
        Hack,
        LogPacket,
        Color,
    }

}
