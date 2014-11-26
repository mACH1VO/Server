using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dirac.Logging
{
    /// <summary>
    /// Log manager class.
    /// </summary>
    public static class LogManager
    {
        public static bool Initialized { get; set; }

        /// <summary>
        /// Is logging enabled?
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// log targets.
        /// </summary>
        internal readonly static List<LogTarget> LogTargets = new List<LogTarget>();


        private static Logger _defaultLogger;
        public static Logger DefaultLogger { get { return _defaultLogger; } }

        /// <summary>
        /// Attachs a new log target.
        /// </summary>
        /// <param name="target"></param>
        public static void AttachLogTarget(LogTarget target)
        {
            LogTargets.Add(target);
        }

        public static void InitLoggers()
        {
            if (!Initialized)
            {
                LogManager.Enabled = true; // enable logger by default.

                if (WindowLogConfig.Instance.Enabled)
                {
                    LogTarget target = null;
                    switch (WindowLogConfig.Instance.Target.ToLower())
                    {
                        case "console":
                            target = new WindowTarget(WindowLogConfig.Instance.MinimumLevel, WindowLogConfig.Instance.MaximumLevel,
                                                       WindowLogConfig.Instance.IncludeTimeStamps);
                            break;
                        case "file":
                            target = new FileTarget(WindowLogConfig.Instance.FileName, WindowLogConfig.Instance.MinimumLevel,
                                                    WindowLogConfig.Instance.MaximumLevel, WindowLogConfig.Instance.IncludeTimeStamps,
                                                    WindowLogConfig.Instance.ResetOnStartup);
                            break;
                    }

                    if (target != null)
                        LogManager.AttachLogTarget(target);
                }

                if (PacketLogConfig.Instance.Enabled)
                {
                    LogTarget target = null;
                    switch (PacketLogConfig.Instance.Target.ToLower())
                    {
                        case "console":
                            target = new WindowTarget(PacketLogConfig.Instance.MinimumLevel, PacketLogConfig.Instance.MaximumLevel,
                                                       PacketLogConfig.Instance.IncludeTimeStamps);
                            break;
                        case "file":
                            target = new FileTarget(PacketLogConfig.Instance.FileName, PacketLogConfig.Instance.MinimumLevel,
                                                    PacketLogConfig.Instance.MaximumLevel, PacketLogConfig.Instance.IncludeTimeStamps,
                                                    PacketLogConfig.Instance.ResetOnStartup);
                            break;
                    }

                    if (target != null)
                        LogManager.AttachLogTarget(target);
                }

                if (ServerLogConfig.Instance.Enabled)
                {
                    LogTarget target = null;
                    switch (ServerLogConfig.Instance.Target.ToLower())
                    {
                        case "console":
                            target = new WindowTarget(ServerLogConfig.Instance.MinimumLevel, ServerLogConfig.Instance.MaximumLevel,
                                                       ServerLogConfig.Instance.IncludeTimeStamps);
                            break;
                        case "file":
                            target = new FileTarget(ServerLogConfig.Instance.FileName, ServerLogConfig.Instance.MinimumLevel,
                                                    ServerLogConfig.Instance.MaximumLevel, ServerLogConfig.Instance.IncludeTimeStamps,
                                                    ServerLogConfig.Instance.ResetOnStartup);
                            break;
                    }

                    if (target != null)
                        LogManager.AttachLogTarget(target);
                }

                _defaultLogger = new Logger("DefaultLogger");
                Initialized = true;
            }
        }
    }
}
