/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// The LevelFilteredLogger wraps another Logger to filter out and calls to the Log methods that are
    /// logged at any of the filtered levels. Each method of this class forwards to the corresponding
    /// method of underlyingLogger if the level is not filtered. This class is used by LogManager when
    /// creating a logger if the filtered_levels property is present in the configuration. This class is
    /// not designed to be created dynamically, so it does not have the IConfiguration constructor or the
    /// InitializeZeroConfiguration method.
    /// </para>
    /// <para>
    /// The class is a bit unwieldy, because this class bypasses most of the
    /// functionality of the Logger class. However, we need this class to be a Logger instance, and because
    /// the earlier versions of this component (short-sightedly) decided to make Logger an abstract class
    /// instead of an interface, we are forced with inheriting all of the Logger apparatus. It is not
    /// practical to change Logger to an interface at this point, as it would require recompiling all
    /// components that make use of Logging Wrapper.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable and it is assumed that the underlying logger is also thread-safe (as all
    /// loggers are required to be, per the Logger documentation). Hence this class is also thread-safe.
    /// </para>
    /// </threadsafety>
    ///
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class LevelFilteredLogger : Logger
    {
        /// <summary>
        /// <para>
        /// The list of levels that are filtered out by this logger.
        /// </para>
        /// <para>
        /// This field is set in the constructor, is immutable, and can never be null. It may be empty, but
        /// items are never added/removed from it. Messages logged at any of the levels in this list will
        /// be ignored and not passed to the underlying logger.
        /// </para>
        /// </summary>
        private readonly IList<Level> filteredLevels;

        /// <summary>
        /// <para>
        /// The logger to which all logging requests are to be forwarded.
        /// </para>
        /// <para>
        /// This field is set in the constructor, is immutable, and can never be null. It is used in all
        /// the Log and LogNamedMessage methods. All methods of this class forward to the corresponding
        /// method of underlyingLogger, unless the message is being logged at a level which is filtered
        /// out, in which case nothing is done.
        /// </para>
        /// </summary>
        private readonly Logger underlyingLogger;

        /// <summary>
        /// <para>
        /// Represents the property to get a copy of the set of named messages that can be used with this
        /// logger.
        /// </para>
        /// </summary>
        /// <value>The set of named messages that can be used with this logger.</value>
        public override IDictionary<string, NamedMessage> NamedMessages
        {
            get
            {
                return new Dictionary<string, NamedMessage>(underlyingLogger.NamedMessages);
            }
        }

        /// <summary>
        /// <para>
        /// Represents the property to get the default logging level.
        /// </para>
        /// </summary>
        /// <value>The default logging level.</value>
        public override Level DefaultLevel
        {
            get
            {
                return underlyingLogger.DefaultLevel;
            }
        }

        /// <summary>
        /// <para>
        /// Represents the property to get the log name of the logger.
        /// </para>
        /// </summary>
        /// <value>The log name of the logger.</value>
        public override string Logname
        {
            get
            {
                return underlyingLogger.Logname;
            }
        }

        /// <summary>
        /// <para>
        /// Creates a new LevelFilteredLogger that filter out messages of given levels.
        /// </para>
        /// </summary>
        /// <param name="underlyingLogger">The logger to which log messages will be forwarded.</param>
        /// <param name="filteredLevels">The levels that will be filtered out and for which log messages
        /// will not be forwarded.</param>
        /// <exception cref="ArgumentNullException">If underlyingLogger or filteredLevels is null.
        /// </exception>
        public LevelFilteredLogger(Logger underlyingLogger, IList<Level> filteredLevels)
            : base(Helper.ValidateLoggerNotNull(underlyingLogger, "underlyingLogger").Logname,
                underlyingLogger.DefaultLevel)
        {
            Helper.ValidateNotNull(filteredLevels, "filteredLevels");

            this.underlyingLogger = underlyingLogger;
            this.filteredLevels = new List<Level>(filteredLevels);
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the logger.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            underlyingLogger.Dispose();
        }

        /// <summary>
        /// <para>
        /// Determines whether the logger supports the given level.
        /// </para>
        /// <para>
        /// Note that this method indicates whether the level is supported by the logger, not whether the
        /// logging is currently turned on for that level.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true if the level is supported by the logger, false if not.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return underlyingLogger.IsLevelEnabled(level);
        }

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// The message will not be logged if the given level is in the list of levels to be filtered.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        /// <exception cref="ArgumentNullException">If message or params is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        public override void Log(Level level, string message, params object[] param)
        {
            Helper.ValidateNotNull(message, "message");
            Helper.ValidateNotNull(param, "param");

            if (!filteredLevels.Contains(level))
            {
                underlyingLogger.Log(level, message, param);
            }
        }

        /// <summary>
        /// <para>
        /// Logs a named message using the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level at which to log the message.</param>
        /// <param name="messageIdentifier">The string identifying which named message to log.</param>
        /// <param name="param">The parameters to use to format the message.</param>
        /// <exception cref="ArgumentNullException">If messageIdentifier or param is null.</exception>
        /// <exception cref="ArgumentException">If messageIdentifier is the empty string, or
        /// messageIdentifier is not in the keys of the namedMessages dictionary.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message from
        /// the params.</exception>
        /// <exception cref="LoggingException">If there is a failure in the backend logging system.
        /// </exception>
        public override void LogNamedMessage(Level level, string messageIdentifier, params object[] param)
        {
            if (!filteredLevels.Contains(level))
            {
                underlyingLogger.LogNamedMessage(level, messageIdentifier, param);
            }
        }

        /// <summary>
        /// <para>
        /// Logs the named message to the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// The message will not be logged if the given level is in the list of levels to be filtered.
        /// </para>
        /// </summary>
        /// <param name="level">The level at which to log the message.</param>
        /// <param name="message">The named message to log.</param>
        /// <param name="param">The parameters to use in formatting the message.</param>
        /// <exception cref="ArgumentNullException">If message or params is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        protected internal override void LogNamedMessage(Level level, NamedMessage message,
            params object[] param)
        {
            Helper.ValidateNotNull(message, "message");
            Helper.ValidateNotNull(param, "param");

            if (!filteredLevels.Contains(level))
            {
                underlyingLogger.LogNamedMessage(level, message, param);
            }
        }
    }
}
