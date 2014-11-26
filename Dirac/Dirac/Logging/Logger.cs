using System;
using System.Globalization;
using System.Text;
using Dirac.Extensions;
using Dirac.GameServer.Network.Message;

namespace Dirac.Logging
{
    /// <summary>
    /// Logger class that can log messages and exceptions to available log-targets.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Name of the logger.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Creates a new logger with given name.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        public Logger(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Available log levels.
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// Trace messages.
            /// </summary>
            Trace = 0,
            /// <summary>
            /// Debug messages.
            /// </summary>
            Debug = 1,
            /// <summary>
            /// Info messages.
            /// </summary>
            Info = 2,
            /// <summary>
            /// Warning messages.
            /// </summary>
            Warn = 3,
            /// <summary>
            /// Error messages.
            /// </summary>
            Error = 4,
            /// <summary>
            /// Fatal error messages.
            /// </summary>
            Fatal = 5,
            /// <summary>
            /// Packet dumps.
            /// </summary>
            PacketDump = 6,
        }

        #region message loggers

        /// <summary>
        /// Logs a trace message. (this is, the same as Trace)
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Add(string message) { this.Trace(message); }

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Add(string message, params object[] args) { this.Trace(message, args); }

        /// <summary>
        /// Logs a trace message. (this is, the same as Trace)
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Trace(string message) { _log(Level.Trace, message, null); }

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Trace(string message, params object[] args) { _log(Level.Trace, message, args); }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Debug(string message) { _log(Level.Debug, message, null); }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Debug(string message, params object[] args) { _log(Level.Debug, message, args); }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Info(string message) { _log(Level.Info, message, null); }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Info(string message, params object[] args) { _log(Level.Info, message, args); }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Warn(string message) { _log(Level.Warn, message, null); }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Warn(string message, params object[] args) { _log(Level.Warn, message, args); }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Error(string message) { _log(Level.Error, message, null); }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Error(string message, params object[] args) { _log(Level.Error, message, args); }

        /// <summary>
        /// Logs an fatal error message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Fatal(string message) { _log(Level.Fatal, message, null); }

        /// <summary>
        /// Logs an fatal error message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void Fatal(string message, params object[] args) { _log(Level.Fatal, message, args); }

        #endregion

        #region message loggers with additional exception info included

        /// <summary>
        /// Logs a trace message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void TraceException(Exception exception, string message) { _logException(Level.Trace, message, null, exception); }

        /// <summary>
        /// Logs a trace message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void TraceException(Exception exception, string message, params object[] args) { _logException(Level.Trace, message, args, exception); }

        /// <summary>
        /// Logs a debug message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void DebugException(Exception exception, string message) { _logException(Level.Debug, message, null, exception); }

        /// <summary>
        /// Logs a debug message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void DebugException(Exception exception, string message, params object[] args) { _logException(Level.Debug, message, args, exception); }

        /// <summary>
        /// Logs an info message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void InfoException(Exception exception, string message) { _logException(Level.Info, message, null, exception); }

        /// <summary>
        /// Logs an info message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void InfoException(Exception exception, string message, params object[] args) { _logException(Level.Info, message, args, exception); }

        /// <summary>
        /// Logs a warning message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void WarnException(Exception exception, string message) { _logException(Level.Warn, message, null, exception); }

        /// <summary>
        /// Logs a warning message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void WarnException(Exception exception, string message, params object[] args) { _logException(Level.Warn, message, args, exception); }

        /// <summary>
        /// Logs an error message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void ErrorException(Exception exception, string message) { _logException(Level.Error, message, null, exception); }

        /// <summary>
        /// Logs an error message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void ErrorException(Exception exception, string message, params object[] args) { _logException(Level.Error, message, args, exception); }

        /// <summary>
        /// Logs a fatal error message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        public void FatalException(Exception exception, string message) { _logException(Level.Fatal, message, null, exception); }

        /// <summary>
        /// Logs a fatal error message with an exception included.
        /// </summary>
        /// <param name="exception">The exception to include in log line.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Additional arguments.</param>
        public void FatalException(Exception exception, string message, params object[] args) { _logException(Level.Fatal, message, args, exception); }

        #endregion

        #region packet loggers

        /// <summary>
        /// Logs an incoming game-server packet.
        /// </summary>
        /// <param name="message">Gameserver packet to log.</param>
        public void LogIncomingPacket(GameMessage message)
        {
            _log(Level.PacketDump, "[I] " + message.AsText(), null);
        }

        /// <summary>
        /// Logs an outgoing game-server packet.
        /// </summary>
        /// <param name="message">Gameserver packet to log.</param>
        public void LogOutgoingPacket(GameMessage message)
        {
            _log(Level.PacketDump, "[O] " + message.AsText(), null);
        }

        #endregion

        #region utility functions

        private void _log(Level level, string message, object[] args) // sends logs to log-router.
        {
            LogRouter.RouteMessage(level, this.Name, args == null ? message : string.Format(CultureInfo.InvariantCulture, message, args));
        }

        private void _logException(Level level, string message, object[] args, Exception exception) // sends logs to log-router.
        {
            LogRouter.RouteException(level, this.Name, args == null ? message : string.Format(CultureInfo.InvariantCulture, message, args), exception);
        }

        #endregion

    }
}
