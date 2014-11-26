using System;
using System.Linq;

namespace Dirac.Logging
{
    /// <summary>
    /// LogRouter class that routes messages to appropriate log-targets.
    /// </summary>
    internal static class LogRouter
    {
        /// <summary>
        /// Routes a message to appropriate log-targets.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        public static void RouteMessage(Logger.Level level, string logger, string message)
        {
            if (!LogManager.Enabled) // if we logging is not enabled,
                return; // just skip.

            if (LogManager.LogTargets.Count == 0) // if we don't have any active log-targets,
                return; // just skip

            // loop through all available logs targets and route the messages that meet the filters.
            foreach (var target in LogManager.LogTargets.Where(target => level >= target.MinimumLevel && level <= target.MaximumLevel))
            {
                target.LogMessage(level, logger, message);
            }
        }

        /// <summary>
        /// Routes a message to appropriate log-targets.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Exception to be included with log message.</param>
        public static void RouteException(Logger.Level level, string logger, string message, Exception exception)
        {
            if (!LogManager.Enabled) // if we logging is not enabled,
                return;

            if (LogManager.LogTargets.Count == 0) // if we don't have any active log-targets,
                return; 

            foreach (var target in LogManager.LogTargets.Where(target => level >= target.MinimumLevel && level <= target.MaximumLevel))
            {
                target.LogException(level, logger, message, exception);
            }
        }
    }
}
