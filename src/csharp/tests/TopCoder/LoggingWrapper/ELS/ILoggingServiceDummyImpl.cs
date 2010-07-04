/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.ServiceModel;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// A dummy implementation of <see cref="ILoggingService"/> used during
    /// testing.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class ILoggingServiceDummyImpl : DiagnosticImpl, ILoggingService, IDisposable
    {
        /// <summary>
        /// <para>The flag which when it is set, causes the log method to fail.</para>
        /// </summary>
        public static bool fail = false;

        /// <summary>
        /// A string as name of logger for testing.
        /// </summary>
        private new const string LOGGER_NAME = "LogTest";

        /// <summary>
        /// A level as level of logger for testing.
        /// </summary>
        private new const Level DEFAULT_LEVEL = Level.INFO;

        /// <summary>
        /// A string as source of log for testing.
        /// </summary>
        private new const string SOURCE = "ELSImplSource";

        /// <summary>
        /// <para>Default constructor.</para>
        /// </summary>
        public ILoggingServiceDummyImpl()
            : this(CreateConfig())
        {
        }

        /// <summary>
        /// <para>Creates a configuration to pass to the constructor.</para>
        /// </summary>
        /// <returns>The created configuration</returns>
        private static IConfiguration CreateConfig()
        {
            IConfiguration config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", LOGGER_NAME);
            config.SetSimpleAttribute("default_level", DEFAULT_LEVEL.ToString());
            config.SetSimpleAttribute("source", SOURCE);
            return config;
        }

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="config">The <see cref="IConfiguration"/> to configure
        /// object from.</param>
        public ILoggingServiceDummyImpl(IConfiguration config)
            : base(config)
        {
        }

        /// <summary>
        /// <para>Flag indicating whether the <see cref="Dispose"/> method is called.</para>
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, logging level, and message parameters.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="level">
        /// The logging level.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void Log(string originator, string[] context, Level level, string message, string[] parameters)
        {

            if (disposed || fail)
            {
                throw new Exception();
            }

            string c = (context == null ? "null" : string.Join(",", context));
            string newMessage = string.Join(":", new string[] { originator, c, message, level.ToString() });

            if (parameters != null)
            {
                base.Log(Level.WARN, newMessage, parameters);
            }
            else
            {
                base.Log(Level.WARN, newMessage);
            }
        }

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.DEBUG</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void LogDebug(string originator, string[] context, string message, string[] parameters)
        {
        }

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.INFO</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void LogInfo(string originator, string[] context, string message, string[] parameters)
        {
        }

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.WARN</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void LogWarning(string originator, string[] context, string message, string[] parameters)
        {
        }

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.ERROR</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void LogError(string originator, string[] context, string message, string[] parameters)
        {
        }

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.FATAL</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        public void LogFatal(string originator, string[] context, string message, string[] parameters)
        {
        }

        /// <summary>
        /// <para>Sets the disposed flag.</para>
        /// </summary>
        public new void Dispose()
        {
            disposed = true;
        }
    }
}
