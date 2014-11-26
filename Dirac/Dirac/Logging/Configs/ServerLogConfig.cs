using System;

namespace Dirac.Logging
{
    /// <summary>
    /// Holds configuration info for log manager.
    /// </summary>
    public sealed class ServerLogConfig : Config.Config
    {

        /// <summary>
        /// Gets or sets the logging root.
        /// </summary>
        public string Root
        {
            get { return this.GetString("Root", @"logs"); }
            set { this.Set("Root", value); }
        }

        /// <summary>
        /// Is enabled?
        /// </summary>
        public bool Enabled
        {
            get { return this.GetBoolean("Enabled", true); }
            set { this.Set("Enabled", value); }
        }

        /// <summary>
        /// Target type. Valid values are file and console.
        /// </summary>
        public string Target
        {
            get { return this.GetString("Target", "Console"); }
            set { this.GetString("Target", value); }
        }

        /// <summary>
        /// Include timestamps in logs?
        /// </summary>
        public bool IncludeTimeStamps
        {
            get { return this.GetBoolean("IncludeTimeStamps", false); }
            set { this.Set("IncludeTimeStamps", value); }
        }

        /// <summary>
        /// Filename if logtarget is a file based one.
        /// </summary>
        public string FileName
        {
            get { return this.GetString("FileName", ""); }
            set { this.GetString("FileName", value); }
        }

        /// <summary>
        /// Minimum level of messages to emit.
        /// </summary>
        public Logger.Level MinimumLevel
        {
            get { return (Logger.Level)(this.GetInt("MinimumLevel", (int)Logger.Level.Info, true)); }
            set { this.Set("MinimumLevel", (int)value); }
        }

        /// <summary>
        /// Maximum level of messages to emit
        /// </summary>
        public Logger.Level MaximumLevel
        {
            get { return (Logger.Level)(this.GetInt("MaximumLevel", (int)Logger.Level.Fatal, true)); }
            set { this.Set("MaximumLevel", (int)value); }
        }

        /// <summary>
        /// Reset log file on startup?
        /// </summary>
        public bool ResetOnStartup
        {
            get { return this.GetBoolean("ResetOnStartup", false); }
            set { this.Set("ResetOnStartup", value); }
        }

        /// <summary>
        /// Creates a new log config.
        /// </summary>
        public ServerLogConfig() :
            base("ServerLogConfig") // Call the base ctor with section name 'Logging'.
        { }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static ServerLogConfig Instance { get { return _instance; } }

        /// <summary>
        /// The internal instance pointer.
        /// </summary>
        private static readonly ServerLogConfig _instance = new ServerLogConfig();
    }
}
