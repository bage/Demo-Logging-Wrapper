/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// A simple logger extends abstract class Logger for testing. The passed in arguments is stored
    /// in static fields so that they can be retrieved for testing. By presetting the exception to be
    /// thrown in methods, it can also be used for failure tests.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExclude]
    public class AnotherSimpleLogger : Logger
    {
        /// <summary>
        /// Represents the message argument passed in to the Log method.
        /// </summary>
        private static string lastMessage = null;

        /// <summary>
        /// Represents the flag whether method Dispose is called.
        /// </summary>
        private static bool lastDispose = false;

        /// <summary>
        /// Represents the exception to be thrown in methods.
        /// Note that the exception will be thrown just once everytime it is set so that other tests
        /// won't be affected.
        /// </summary>
        private static Exception ex = null;

        /// <summary>
        /// Represents the property to get the message argument passed in to the Log method.
        /// </summary>
        /// <value>The message argument passed in to the Log method.</value>
        public static string LastMessage
        {
            get
            {
                string tmp = lastMessage;
                lastMessage = null;
                return tmp;
            }
        }

        /// <summary>
        /// Represents the property to get whether method Dispose is called. The property only return
        /// true once for each call to Dispose.
        /// </summary>
        /// <value>The param argument passed in to the Log method.</value>
        public static bool LastDispose
        {
            get
            {
                bool value = lastDispose;
                lastDispose = false;
                return value;
            }
        }

        /// <summary>
        /// Represents the property to set the exception to thrown in methods.
        /// </summary>
        public static Exception Ex
        {
            set
            {
                ex = value;
            }
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the given log name. Delegates for the same named method
        /// in super class.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <exception cref="ArgumentNullException">If logname is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string.</exception>
        public AnotherSimpleLogger(string logname)
            : base(logname)
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the given log name and default level. Delegates for the
        /// same named method in super class.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <param name="defaultLevel">The default logging level.</param>
        /// <exception cref="ArgumentNullException">If logname is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string.</exception>
        public AnotherSimpleLogger(string logname, Level defaultLevel)
            : base(logname, defaultLevel)
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the log name and the default level and set of named
        /// messages. Delegates for the same named method in super class.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <param name="defaultLevel">The default logging level.</param>
        /// <param name="namedMessages">The dictionary mapping identifiers to named messages that should be
        /// used by this logger.</param>
        /// <exception cref="ArgumentNullException">If logname or namedMessages is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string, or namedMessages contains a
        /// null or empty key or any null values.</exception>
        public AnotherSimpleLogger(string logname, Level defaultLevel,
            IDictionary<string, NamedMessage> namedMessages)
            : base(logname, defaultLevel, namedMessages)
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of the Logger with setting loaded from the given configuration.
        /// Delegates for the same named method in super class.
        /// </para>
        /// <para>
        /// If ex is not null, it will be thrown.
        /// </para>
        /// </summary>
        /// <param name="configuration">The configuration object to load settings from.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <exception cref="ConfigException">
        ///  If any of the configuration settings are missing or are invalid values.
        /// </exception>
        public AnotherSimpleLogger(IConfiguration configuration)
            : base(configuration)
        {
            if (ex != null)
            {
                Exception tmpEx = ex;
                ex = null;
                throw tmpEx;
            }
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the logger. Sets lastDispose true.
        /// </para>
        /// <para>
        /// If ex is not null, it will be thrown.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            if (ex != null)
            {
                Exception tmpEx = ex;
                ex = null;
                throw tmpEx;
            }
            lastDispose = true;
        }

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the specified logging level.
        /// All the arguments will be stored in name-like static fields.
        /// </para>
        /// <para>
        /// If ex is not null, it will be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        public override void Log(Level level, string message, params object[] param)
        {
            if (ex != null)
            {
                Exception tmpEx = ex;
                ex = null;
                throw tmpEx;
            }
            lastMessage = message;
        }

        /// <summary>
        /// <para>
        /// Used to determine if a specific logging level is supported by underlying implementation.
        /// Returns true.
        /// </para>
        /// <para>
        /// If ex is not null, it will be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            if (ex != null)
            {
                Exception tmpEx = ex;
                ex = null;
                throw tmpEx;
            }
            return true;
        }

        /// <summary>
        /// <para>
        /// Logs the named message to the underlying implementation with the specified logging level.
        /// Delegates for the same named method in super class.
        /// </para>
        /// <para>
        /// If ex is not null, it will be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The level at which to log the message.</param>
        /// <param name="message">The named message to log.</param>
        /// <param name="param">The parameters to use in formatting the message.</param>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message from
        /// the params.</exception>
        /// <exception cref="LoggingException">If there is a failure in the backend logging system.
        /// </exception>
        public new void LogNamedMessage(Level level, NamedMessage message, params object[] param)
        {
            base.LogNamedMessage(level, message, param);
        }
    }
}