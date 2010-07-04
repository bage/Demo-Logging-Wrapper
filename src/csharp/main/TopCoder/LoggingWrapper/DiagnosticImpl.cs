/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// DiagnosticImpl extends from Logger abstract class to log messages using EventLog from the
    /// System.Diagnostics namespace.
    /// </para>
    /// <para>
    /// Changes in 3.0: A new constructor is added while the old constructor is made obsolete; The
    /// InitializeZeroConfiguration method is added; Immutable fields are marked readonly. Exceptions are
    /// now allowed and documented
    /// </para>
    /// <para>
    /// Creating this Logger implementation is done by code like this:
    /// </para>
    /// <code>
    /// Logger logger = LogManager.CreateLogger("TopCoder.LoggingWrapper.DiagnosticImpl");
    /// </code>
    /// <para>
    /// Once created, you may log messages at any of the levels defined in Level.  For example, to log a
    /// message at the level of INFO, you would use the logger as follows:
    /// </para>
    /// <code>
    /// logger.Log(Level.INFO, "Hello world!");
    /// </code>
    /// <para>
    /// You can also log a named message that has been defined in configuration. The actual message
    /// will be generated according the the mapped NamedMessage object with given name:
    /// </para>
    /// <code>
    /// logger.LogNamedMessage(Level.DEBUG, "Name of Message", "param1", "param2");
    /// </code>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// The EventLog is not thread safe, so the Log method should locked on the inner log instance to
    /// ensure the thread-safe. All the other methods are immutable, so thread-safe is not an issue here.
    /// </para>
    /// </threadsafety>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class DiagnosticImpl : Logger
    {
        /// <summary>
        /// <para>
        /// Represents the "source" property key to load from the configuration. It is used by the
        /// constructors to get the value to create the EventLog instance.
        /// </para>
        /// <para>
        /// Changes in 3.0: This is the key used to load the value from IConfiguration instead of
        /// ConfigManager.
        /// </para>
        /// </summary>
        public const string SOURCE = "source";

        /// <summary>
        /// <para>
        /// The default value to set for "source" property in configuration. It is used in method
        /// InitializeZeroConfiguration.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const string DEFAULT_SOURCE = "TopCoder Logger";

        /// <summary>
        /// <para>
        /// Represent the defalut logger name.
        /// </para>
        /// <para>
        /// Bugr-fix 427
        /// </para>
        /// </summary>
        private const string DEFAULT_EVENT_LOG = "Application";

        /// <summary>
        /// <para>
        /// Represents the level mapping from the Level enumeration to that of EventLog. The key is the
        /// Level value, and the value is the corresponding level in EventLog. It is populated in static
        /// constructor.
        /// </para>
        /// <para>
        /// Changes in 3.0: generic collection is used. And it is readonly now.
        /// </para>
        /// </summary>
        private readonly static IDictionary<Level, EventLogEntryType> levelMapping
            = new Dictionary<Level, EventLogEntryType>();

        /// <summary>
        /// <para>
        /// Represents the EventLog instance to log the messages to.
        /// </para>
        /// <para>
        /// It is initialized in the constructor, is immutable, and can not be null.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is readonly now.
        /// </para>
        /// </summary>
        private readonly EventLog log = null;

        /// <summary>
        /// <para>
        /// Represents the source of the EventLog. It will be used to create the EventLog variable.
        /// </para>
        /// <para>
        /// Set in the constructor, immutable, and can not be null or an empty string.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is readonly now.
        /// </para>
        /// </summary>
        private readonly string source = null;

        #region Obsolete Methods

        /// <summary>
        /// <para>
        /// Create a new instance of DiagnosticImpl from the configuration values loaded from the
        /// properties IDictionary.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is made obsolete, replaced by constructor with IConfiguration as parameter.
        /// </para>
        /// </summary>
        /// <param name="properties">The configuration dictionary.</param>
        /// <exception cref="ArgumentNullException">If properties is null.</exception>
        /// <exception cref="ConfigException">
        /// If there is a failure to load configuration values, or create the instance of the class.
        /// </exception>
        [Obsolete]
        public DiagnosticImpl(IDictionary properties)
            : base(properties)
        {
            source = properties[SOURCE] as string;

            if (source == null)
            {
                throw new ConfigException("Property '" + SOURCE +
                    "' does not exist in properties dictionary.");
            }

            if (source == string.Empty)
            {
                throw new ConfigException("Property '" + SOURCE +
                    "' is an empty string.");
            }

            try
            {
                // create the event log source if it doesn't already exist
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, Logname);
                }

                log = new EventLog(Logname, ".", source);

                log.Source = source;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to create event log.", e);
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// Creates a new instance of DiagnosticImpl with defalut logger name.
        /// </para>
        /// <para>
        /// Bugr-fix 427
        /// </para>
        /// </summary>
        public DiagnosticImpl()
            : base(DEFAULT_EVENT_LOG)
        {
            source = Process.GetCurrentProcess().ProcessName;
            try
            {
                // create the event log source if it doesn't already exist 
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, Logname);
                }

                log = new EventLog(Logname, ".", source);

                log.Source = source;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to create event log.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of DiagnosticImpl with setting loaded from the given configuration.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="configuration">The configuration object to load settings from.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <exception cref="ConfigException">
        ///  If any of the configuration settings are missing or are invalid values.
        /// </exception>
        public DiagnosticImpl(IConfiguration configuration)
            : base(configuration)
        {
            // load source from configuration (required)
            source = Helper.GetStringAttribute(configuration, SOURCE, true);

            try
            {
                // create the event log source if it doesn't already exist
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, Logname);
                }

                log = new EventLog(Logname, ".", source);

                log.Source = source;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to create event log.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the DiagnosticImpl instance.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            lock (log)
            {
                try
                {
                    log.Close();
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns true if the level is supported by the DiagnosticImpl instance. All levels in Level are
        /// supported by the current implementations.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true for all levels in Level.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return true;
        }

        /// <summary>
        /// <para>
        /// Logs a message to the EventLog instance used by this class.
        /// </para>
        /// <para>
        /// If the level is not supported by this class, or the level is off, the message will not be logged.
        /// </para>
        /// <para>
        /// Changes in 3.0: Exceptions are now allowed and documented. Exceptions can be suppressed by
        /// wrapping the logger in an ExceptionSafeLogger.
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

            try
            {
                lock (log)
                {
                    // Log the formatted message
                    log.WriteEntry(formatted, levelMapping[level]);
                }
            }
            catch (Exception e)
            {
                throw new LoggingException("Error occurs when logging", e);
            }
        }

        /// <summary>
        /// <para>
        /// Initializes the diagnostic backend for the zero configuration logging option.
        /// </para>
        /// <para>
        /// System.Diagnostic output does not go to log files, nor is there the same level of control
        /// (rolling logs, etc) as offered by log4NET. So all zero-configuration options are treated the
        /// same by this class, and no setup of System.Diagnostics is performed.
        /// </para>
        /// <para>
        /// The one action we do take is if the "source" property is not in the configuration, we put the
        /// value "TopCoder Logger" as the value of this property into the configuration.
        /// </para>
        /// <para>
        /// New in 3.0.
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
                if (configuration.GetSimpleAttribute(SOURCE) == null)
                {
                    // set the property to default value in configuration
                    configuration.SetSimpleAttribute(SOURCE, DEFAULT_SOURCE);
                }
            }
            catch (Exception e)
            {
                throw new ConfigException("Error occurs when accessing the configuration", e);
            }
        }

        /// <summary>
        /// <para>
        /// This static constructor initializes the level mapping for all DiagnosticImpl instances.
        /// </para>
        /// </summary>
        static DiagnosticImpl()
        {
            levelMapping[Level.DEBUG] = EventLogEntryType.Information;
            levelMapping[Level.FAILUREAUDIT] = EventLogEntryType.FailureAudit;
            levelMapping[Level.SUCCESSAUDIT] = EventLogEntryType.SuccessAudit;
            levelMapping[Level.ERROR] = EventLogEntryType.Error;
            levelMapping[Level.FATAL] = EventLogEntryType.Error;
            levelMapping[Level.WARN] = EventLogEntryType.Warning;
            levelMapping[Level.INFO] = EventLogEntryType.Information;
        }
    }
}
