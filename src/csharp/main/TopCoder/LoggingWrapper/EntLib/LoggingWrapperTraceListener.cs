/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace TopCoder.LoggingWrapper.EntLib
{
    /// <summary>
    /// <para>
    /// The LoggingWrapperTraceListener class is an extension of the CustomTraceListener class that is
    /// designed to be plugged into the Enterprise Library Logging Application Block. All messages logged
    /// through the Logging Application Block will be directed to this trace listener, which will then
    /// relay them to a Logger from this component, which will then relay them to a backend logging
    /// solution.
    /// </para>
    /// <para>
    /// Unfortunately, a TraceListener does not receive much information about the logging events. All the
    /// level (called priority in the Logging Application Block) and parameter information has already
    /// been dealt with by the time the trace listener is notified, and is not available. All that is
    /// available is the message to be logged. We just send this message on to the TopCoder LoggingWrapper
    /// logger, to be logged at the default level.
    /// </para>
    /// <para>
    /// Note that if the instance is created using default constructor, the logger will be lazily
    /// initialized on the first call to Write or WriteLine method using the namespace stored in Attributes
    /// dictionary with key "loggerNamespace". The namespace is set via the Attributes property
    /// instead of constructor.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is supposed to be used inside the Enterprise Library - Logging Application Block which
    /// guarantee the thread-safety and the LoggingWrapper logger is also thread safe. Hence this class is
    /// thread-safe.
    /// </para>
    /// </threadsafety>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class LoggingWrapperTraceListener : CustomTraceListener
    {
        /// <summary>
        /// <para>
        /// Represents the line terminator.
        /// </para>
        /// </summary>
        private const string LINE_BREAK = "\n";

        /// <summary>
        /// <para>
        /// Represents the key of logger namespace in Attributes.
        /// </para>
        /// </summary>
        private const string LOGGER_NAMESPACE = "loggerNamespace";

        /// <summary>
        /// <para>
        /// The Logging Wrapper logger to which log messages are forwarded. When calls to the Write or
        /// WriteLine methods are made by the Logging Application Block code, they are translated into
        /// calls to the Log method of this logger.
        /// </para>
        /// <para>
        /// This field can be set in the constructor or the first time Write or WriteLine method is called.
        /// Must be non-null.
        /// </para>
        /// </summary>
        private Logger logger;

        /// <summary>
        /// <para>
        /// Creates a new LoggingWrapperTraceListener with given logger as backend logging solution.
        /// </para>
        /// </summary>
        /// <param name="logger">The Logging Wrapper Logger to redirect writes to.</param>
        /// <exception cref="ArgumentNullException">If logger is null.</exception>
        public LoggingWrapperTraceListener(Logger logger)
        {
            Helper.ValidateNotNull(logger, "logger");
            this.logger = logger;
        }

        /// <summary>
        /// <para>
        /// Default constructor. Does nothing.
        /// </para>
        /// <para>
        /// Note that the logger will be lazily initialized on the first call to a Write method using the
        /// namespace stored in Attributes dictionary with key "logNamespace".
        /// </para>
        /// </summary>
        public LoggingWrapperTraceListener()
        {
            // does nothing
        }

        /// <summary>
        /// <para>
        /// Closes the trace listener.
        /// </para>
        /// </summary>
        public override void Close()
        {
            logger.Dispose();
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void Write(object message)
        {
            try
            {
                CreateLogger();
                logger.Log(message.ToString());
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void Write(string message)
        {
            try
            {
                CreateLogger();
                logger.Log(message);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener with given category.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="category">The category of the message.</param>
        public override void Write(object message, string category)
        {
            try
            {
                CreateLogger();
                logger.Log("[" + category + "]" + message.ToString());
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener with given category.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="category">The category of the message.</param>
        public override void Write(string message, string category)
        {
            try
            {
                CreateLogger();
                logger.Log("[" + category + "]" + message);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener, followed by a line terminator.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void WriteLine(object message)
        {
            try
            {
                CreateLogger();
                logger.Log(message.ToString() + LINE_BREAK);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener, followed by a line terminator.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        public override void WriteLine(string message)
        {
            try
            {
                CreateLogger();
                logger.Log(message + LINE_BREAK);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener with given category, followed by a line terminator.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="category">The category of the message.</param>
        public override void WriteLine(object message, string category)
        {
            try
            {
                CreateLogger();
                logger.Log("[" + category + "]" + message.ToString() + LINE_BREAK);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// <para>
        /// Writes a message to the trace listener with given category, followed by a line terminator.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="category">The category of the message.</param>
        public override void WriteLine(string message, string category)
        {
            try
            {
                CreateLogger();
                logger.Log("[" + category + "]" + message + LINE_BREAK);
            }
            catch
            {
                // any exception from the logger is swallowed and ignored.
                // this is necessary to meet the contract of the TraceListener interface
            }
        }

        /// <summary>
        /// Gets the custom attributes supported by the trace listener. Only "loggerNamespace" is supported.
        /// </summary>
        /// <returns>the custom attributes supported by the trace listener.</returns>
        protected override string[] GetSupportedAttributes()
        {
            return new string[] {LOGGER_NAMESPACE};
        }

        /// <summary>
        /// <para>
        /// Creates the logger via LogManager using the namespace in Attributes dictionary. The logger
        /// will be initialized if it is not set in constructor.
        /// </para>
        /// <para>
        /// bugr-fix 427
        /// </para>
        /// </summary>
        /// <exception cref="ConfigException">If any error occurs when creating logger.</exception>
        private void CreateLogger()
        {
            if (logger == null)
            {
                logger = LogManager.CreateLogger();
            }
        }
    }
}
