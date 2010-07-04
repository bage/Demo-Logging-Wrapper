/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.EntLib
{
    /// <summary>
    /// <para>
    /// The EnterpriseLibraryLogger is a TopCoder logger that allows using the Enterprise Library: Logging
    /// Application Block as the backend logging destination. This class performs the inverse function to
    /// LoggingWrapperTraceListener. All messages logged to this class are redirected to the Logging
    /// Application Block, using the specified category.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable and uses only thread-safe methods in the logging application block. Hence
    /// this class is also thread-safe.
    /// </para>
    /// </threadsafety>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class EnterpriseLibraryLogger : Logger
    {
        /// <summary>
        /// <para>
        /// Represents the "Category" property to load from IConfiguration.
        /// </para>
        /// <para>
        /// It is used by the constructor to get the value of category of log entry.
        /// </para>
        /// </summary>
        private const string CATEGORY = "Category";

        /// <summary>
        /// <para>
        /// The default value to set for "Category" property in configuration.
        /// </para>
        /// <para>
        /// It is used in method InitializeZeroConfiguration.
        /// </para>
        /// </summary>
        private const string DEFAULT_CATEGORY = "TopCoder Logger";

        /// <summary>
        /// <para>
        /// The category to use when logging the message to the Logging Application Block.
        /// </para>
        /// <para>
        /// This field is set in the constructor, is immutable, and can not be null or an empty string.
        /// It is used in the Log method.
        /// </para>
        /// </summary>
        private readonly string category;

        /// <summary>
        /// <para>
        /// Represents the mapping from the Level enumeration to that of TraceEventType.
        /// </para>
        /// <para>
        /// The key is the Level value, and the value is the corresponding level in TraceEventType. It is
        /// populated in static constructor.
        /// </para>
        /// </summary>
        private static readonly IDictionary<Level, TraceEventType> levelMapping
            = new Dictionary<Level, TraceEventType>();

        /// <summary>
        /// <para>
        /// Represents the property to get the category to use when logging the message to the Logging
        /// Application Block.
        /// </para>
        /// </summary>
        /// <value>The category to use when logging the message to the Logging Application Block.</value>
        public string Category
        {
            get
            {
                return category;
            }
        }

        /// <summary>
        /// <para>
        /// Creates a new EnterpriseLibraryLogger with settings loaded from given configuration.
        /// </para>
        /// </summary>
        /// <param name="configuration">The configuration object to load settings from.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <exception cref="ConfigException">
        ///  If any of the configuration settings are missing or are invalid values.
        /// </exception>
        public EnterpriseLibraryLogger(IConfiguration configuration)
            : base(configuration)
        {
            // load category from configuration (required)
            category = Helper.GetStringAttribute(configuration, CATEGORY, true);
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the logger.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            // does nothing
        }

        /// <summary>
        /// <para>
        /// Returns true if the level is supported by the logger. All levels except OFF are supported.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true if the level is supported by the logger, false otherwise.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return level != LoggingWrapper.Level.OFF;
        }

        /// <summary>
        /// <para>
        /// Logs a message with the created inner EventLog variable. If the level is OFF, the message will
        /// not be logged.
        /// </para>
        /// <para>
        /// If the level is not supported by this class, or the level is off, the message will not be logged.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="param">The params to use to format the message.</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        public override void Log(Level level, string message, params object[] param)
        {
            Helper.ValidateNotNull(message, "message");
            Helper.ValidateNotNull(param, "param");

            if (level == LoggingWrapper.Level.OFF || !IsLevelEnabled(level))
            {
                return;
            }

            // format the message with params
            string formatted;
            try
            {
                formatted = string.Format(message, param);
            }
            catch (FormatException e)
            {
                throw new MessageFormattingException(
                    "Error occurs when formatting the message with params", e);
            }

            // create a LogEntry
            LogEntry entry = new LogEntry();
            // set the message
            entry.Message = formatted;
            // add the category
            entry.Categories.Add(category);
            // set the severity
            entry.Severity = levelMapping[level];

            // log the entry
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
            }
            catch (Exception e)
            {
                throw new LoggingException("Error occurs when logging the message.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Initializes the application logging block backend for the zero configuration logging option.
        /// </para>
        /// <para>
        /// The Application Logging Block uses app.config for configuration of its internal workings, and
        /// app.config is only read once at startup. Hence, any changes we make to app.config will not be
        /// visible to the Application Logging Block until the application is restarted. Additionally,
        /// since app.config contains all sorts of configuration, we would have to properly merge any
        /// configuration settings for the logging block into the existing file. Because of these
        /// difficulties, the current version of this component will not attempt to configure the
        /// underlying Logging Application Block when using zero-configuration mode. This may be a future
        /// enhancement.
        /// </para>
        /// <para>
        /// The one action we do take is if the "Category" property is not in the configuration, we put the
        /// value "TopCoder Logger" as the value of this property into the configuration.
        /// </para>
        /// </summary>
        /// <param name="option">The option about which zero-configuration setup should be configured in
        /// the backend.</param>
        /// <param name="configuration">The configuration object to which needed configuration settings are
        /// added.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <exception cref="ConfigException">
        /// If any error occurs when accessing the configuration.
        /// </exception>
        public static void InitializeZeroConfiguration(ZeroConfigurationOption option,
            IConfiguration configuration)
        {
            Helper.ValidateNotNull(configuration, "configuration");

            try
            {
                if (configuration.GetSimpleAttribute(CATEGORY) == null)
                {
                    // set the property to default value in configuration
                    configuration.SetSimpleAttribute(CATEGORY, DEFAULT_CATEGORY);
                }
            }
            catch (Exception e)
            {
                throw new ConfigException("Error occurs when accessing the configuration", e);
            }
        }

        /// <summary>
        /// <para>
        /// This static constructor initializes the level mapping from Level to TraceEventType..
        /// </para>
        /// </summary>
        static EnterpriseLibraryLogger()
        {
            levelMapping[Level.DEBUG] = TraceEventType.Verbose;
            levelMapping[Level.INFO] = TraceEventType.Information;
            levelMapping[Level.WARN] = TraceEventType.Warning;
            levelMapping[Level.SUCCESSAUDIT] = TraceEventType.Information;
            levelMapping[Level.FAILUREAUDIT] = TraceEventType.Warning;
            levelMapping[Level.ERROR] = TraceEventType.Error;
            levelMapping[Level.FATAL] = TraceEventType.Critical;
        }
    }
}