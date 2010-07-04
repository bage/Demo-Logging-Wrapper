/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// The ExceptionSafeLogger wraps another Logger to guarantee that none of the Log methods (and the
    /// IsLevelEnabled method) throw any exception. Each method of this class forwards to the corresponding
    /// method of underlyingLogger. If an exception results, it tries to log the resulting exception to
    /// exceptionLogger. If this attempt to log the exception throws any exception, this second exception
    /// is caught and ignored. This class is used by LogManager. This class is not designed to be created
    /// dynamically, so it does not have the IConfiguration constructor or the InitializeZeroConfiguration
    /// method.
    /// </para>
    /// <para>
    /// The class is a bit unwieldy, because it doesn't use most of the functionality of
    /// the Logger class. However, we need this class to be a Logger instance, and because the earlier
    /// versions of this component (short-sightedly) decided to make Logger an abstract class instead of an
    /// interface, we are forced with inheriting all of the Logger apparatus. It is not practical to change
    /// Logger to an interface at this point, as it would require recompiling all components that make use
    /// of Logging Wrapper.
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
    public class ExceptionSafeLogger : Logger
    {
        /// <summary>
        /// <para>
        /// The logger to which all logging requests are to be forwarded.
        /// </para>
        /// <para>
        /// This field is set in the constructor, is immutable, and can never be null. It is used in all
        /// the Log and LogNamedMessage methods. All methods of this class forward to the corresponding
        /// method of underlyingLogger.
        /// </para>
        /// </summary>
        private readonly Logger underlyingLogger;

        /// <summary>
        /// <para>
        /// The logger to which any exceptions thrown by underlyingLogger are logged.
        /// </para>
        /// <para>
        /// This field is set in the constructor, is immutable, and can never be null. It is used in all
        /// the Log and LogNamedMessage methods.
        /// </para>
        /// </summary>
        private readonly Logger exceptionLogger;

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
        /// Creates a new ExceptionSafeLogger that ensures all log calls do not throw any exception.
        /// </para>
        /// </summary>
        /// <param name="underlyingLogger">The logger to which all logging requests are to be forwarded.
        /// </param>
        /// <param name="exceptionLogger">The logger to log any exceptions thrown by underlyingLogger.
        /// </param>
        /// <exception cref="ArgumentNullException">If underlyingLogger or exceptionLogger is null.
        /// </exception>
        public ExceptionSafeLogger(Logger underlyingLogger, Logger exceptionLogger)
            : base(Helper.ValidateLoggerNotNull(underlyingLogger, "underlyingLogger").Logname,
                underlyingLogger.DefaultLevel)
        {
            Helper.ValidateNotNull(exceptionLogger, "exceptionLogger");

            this.underlyingLogger = underlyingLogger;
            this.exceptionLogger = exceptionLogger;
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the logger.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            try
            {
                underlyingLogger.Dispose();
                exceptionLogger.Dispose();
            }
            catch (Exception)
            {
                // ignore any exception
            }
        }

        /// <summary>
        /// <para>
        /// Determines whether the logger supports the given level.
        /// </para>
        /// <para>
        /// If any error occurs, false will be returned. This method guarantees that no exception will ever
        /// be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true if the level is supported by the logger, false if not.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            try
            {
                return underlyingLogger.IsLevelEnabled(level);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
                return false;
            }
        }

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        public override void Log(Level level, string message, params object[] param)
        {
            try
            {
                underlyingLogger.Log(level, message, param);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
            }
        }

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the default logging level.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        public override void Log(string message, params object[] param)
        {
            try
            {
                underlyingLogger.Log(message, param);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
            }
        }

        /// <summary>
        /// <para>
        /// Logs a named message using the underlying implementation with the default logging level.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        /// <param name="messageIdentifier">The string identifying which named message to log.</param>
        /// <param name="param">The parameters to use to format the message.</param>
        public override void LogNamedMessage(string messageIdentifier, params object[] param)
        {
            try
            {
                underlyingLogger.LogNamedMessage(messageIdentifier, param);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
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
        public override void LogNamedMessage(Level level, string messageIdentifier, params object[] param)
        {
            try
            {
                underlyingLogger.LogNamedMessage(level, messageIdentifier, param);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
            }
        }

        /// <summary>
        /// <para>
        /// Logs the named message to the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// This method guarantees that no exception will ever be thrown.
        /// </para>
        /// </summary>
        /// <param name="level">The level at which to log the message.</param>
        /// <param name="message">The named message to log.</param>
        /// <param name="param">The parameters to use in formatting the message.</param>
        protected internal override void LogNamedMessage(Level level, NamedMessage message,
            params object[] param)
        {
            try
            {
                underlyingLogger.LogNamedMessage(level, message, param);
            }
            catch (Exception e)
            {
                // try to log the exception to exceptionLogger
                LogException(e);
            }
        }

        /// <summary>
        /// <para>
        /// Attempts to log the exception. If any error occurs, it will be ignored.
        /// </para>
        /// </summary>
        /// <param name="e">The exception to log.</param>
        private void LogException(Exception e)
        {
            try
            {
                exceptionLogger.Log(e.ToString());
            }
            catch
            {
                // ignore if any error occurs
            }
        }
    }
}