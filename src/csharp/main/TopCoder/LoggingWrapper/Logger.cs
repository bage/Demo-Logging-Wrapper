/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// Logger abstract class should be extended by classes that wish to provide a custom logging
    /// implementation. The <c>Log</c> method is used to log a message using the underlying implementation,
    /// and the <c>IsLevelEnabled</c> method is used to determine if a specific logging level is supported
    /// by underlying implementation.
    /// </para>
    /// <para>
    /// Logger classes are responsible to load implementation specific data by themselves. All
    /// implementations of Logger that are intended to be instantiated dynamically (i.e. everything except
    /// ExceptionSafeLog and LevelFilteredLofferclass) should have a constructor that takes an
    /// IConfiguration argument. They should also have a static InitializeZeroConfiguration method that
    /// takes a ZeroConfigurationOption argument and an IConfiguration argument.
    /// </para>
    /// <para>
    /// Changes in 3.0: The constructor invoked by reflection has changed from taking an IDictionary to
    /// taking an IConfiguration argument; the LogNamedMessage methods have been added, as has the
    /// supported namedMessages property; log methods now throw exceptions. These can be suppressed (to
    /// maintain backward compatible behavior) by wrapping a logger in an ExceptionSafeLogger. This can
    /// be specified by setting the propagate_exceptions configuration value to false.
    /// </para>
    /// <para>
    /// Changes in 2.0: The Logname property is moving upward from its two subclasses, and the Log,
    /// IsLevelEnabled method are made public; a new constructor taking a properties argument is added.
    /// Its subclasses should always have a constructor taking a properties argument in order to be created
    /// dynamically from the LogManager.CreateLogger method. The constructor should be responsible for
    /// loading necessary configuration values from the properties retrieved from ConfigManager to
    /// correctly initialize the logger instance. After the logger is created, we are expected to use the
    /// Logger.Log method to log the message directly, so the messages are logged per logger instance. The
    /// ConfigFile property and Level property are obsolete and kept for back compatibility.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// The ConfigFile and Level properties are obsolete (which should never be used). This class is
    /// immutable, so it is thread safe inherently, and its subclasses are expected to be immutable if
    /// possible, otherwise, its subclasses should lock carefully to ensure thread-safety.
    /// </para>
    /// </threadsafety>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public abstract class Logger : IDisposable
    {
        /// <summary>
        /// <para>
        /// Represents the default level to be used when the attribute is not provided in constructor.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const Level DEFAULT_LEVEL_VALUE = Level.DEBUG;

        /// <summary>
        /// <para>
        /// Represents the NamedMessages child configuration to load from configuration.
        /// </para>
        /// <para>
        /// It is used by the constructor to get the value to initialize the namedMessages variable.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const string NAMED_MESSAGES = "NamedMessages";

        /// <summary>
        /// <para>
        /// Represents the text property key to load from NamedMessages child configuration.
        /// </para>
        /// <para>
        /// It is used by the constructor to get the value to initialize the namedMessage instance.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const string TEXT = "text";

        /// <summary>
        /// <para>
        /// Represents the parameters property key to load from NamedMessages child configuration.
        /// </para>
        /// <para>
        /// It is used by the constructor taking a IConfiguration argument to get the value to initialize
        /// the namedMessage instance.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const string PARAMETERS = "parameters";

        /// <summary>
        /// <para>
        /// Represents the default_level property key to load from configuration.
        /// </para>
        /// <para>
        /// It is used by the constructor taking a namespace argument to get the value to assign to the
        /// level variable.
        /// </para>
        /// <para>
        /// Changes in 3.0: As of this version, this is the key used to load the value from
        /// IConfiguration instead of ConfigManager.
        /// </para>
        /// </summary>
        public const string DEFAULT_LEVEL = "default_level";

        /// <summary>
        /// <para>
        /// Represents the logger_name property key to load from configuration.
        /// </para>
        /// <para>
        /// It is used by the constructor taking a namespace argument to get the value to assign to the
        /// logname variable.
        /// </para>
        /// <para>
        /// Changes in 3.0: As of this version, this is the key used to load the value from
        /// IConfiguration instead of ConfigManager.
        /// </para>
        /// </summary>
        public const string LOGGER_NAME = "logger_name";

        /// <summary>
        /// <para>
        /// Represents the default logging level.
        /// </para>
        /// <para>
        /// This variable defaults to Level.DEBUG if not set. It can be set in the constructor.
        /// </para>
        /// </summary>
        private Level defaultLevel;

        /// <summary>
        /// <para>
        /// Represents the property to get the default logging level.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is made virtual.
        /// </para>
        /// </summary>
        /// <value>The default logging level.</value>
        public virtual Level DefaultLevel
        {
            get
            {
                return defaultLevel;
            }
        }

        /// <summary>
        /// <para>
        /// The log name of the logger.
        /// </para>
        /// <para>
        /// It is initialized in the constructor and never changed.
        /// </para>
        /// </summary>
        private readonly string logname;

        /// <summary>
        /// <para>
        /// Represents the property to get the log name of the logger.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is made virtual.
        /// </para>
        /// </summary>
        /// <value>The log name of the logger.</value>
        public virtual string Logname
        {
            get
            {
                return logname;
            }
        }

        #region Obsolete Members and Properties

        /// <summary>
        /// Represents the configuration file name which might be useful for its subclasses.
        /// It is obsolete now and should not be used.
        /// </summary>
        [Obsolete]
        private string configfile = null;

        /// <summary>
        /// Represents the default logging level. This variable is default to Level.DEBUG if not set,
        /// it can be assigned in the constructor or the corresponding setter method. The getter method
        /// will simply return the defaultLevel variable, and the setter will simply assign the value to
        /// the defaultValue.
        /// </summary>
        /// <value>The default logging level.</value>
        public Level Level
        {
            get
            {
                return defaultLevel;
            }
            set
            {
                defaultLevel = value;
            }
        }

        /// <summary>
        /// Represents the configuration file name which might be useful for its subclasses.
        /// The getter will simply return configFile variable, and the setter will simply assign the value
        /// to the configFile variable.
        /// It is obsolete now and should not be used.
        /// </summary>
        /// <exception cref="ArgumentNullException">If given value is null.</exception>
        /// <value>The configuration file name which might be useful for its subclasses.</value>
        [Obsolete]
        public string ConfigFile
        {
            get
            {
                return configfile;
            }
            set
            {
                Helper.ValidateNotNull(value, "ConfigFile");
                configfile = value;
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// The set of named messages that this logger can use when the LogNamedMessage method is called.
        /// </para>
        /// <para>
        /// This is a dictionary from the message identifiers to the NamedMessage instances containing the
        /// data needed to do the logging. This map is initialized in the constructor and is immutable
        /// (also, values are never added/removed/changed). Will never be null, but may be empty. Used in
        /// the LogNamedMessage methods, and also by subclasses.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private readonly IDictionary<string, NamedMessage> namedMessages;

        /// <summary>
        /// <para>
        /// Represents the property to get a copy of the set of named messages that can be used with this
        /// logger.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <value>The set of named messages that can be used with this logger.</value>
        public virtual IDictionary<string, NamedMessage> NamedMessages
        {
            get
            {
                return new Dictionary<string, NamedMessage>(namedMessages);
            }
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the given log name. Initially the default level of the
        /// logger is set to Level.DEBUG.
        /// </para>
        /// <para>
        /// Changes in 3.0: no named messages is provided.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <exception cref="ArgumentNullException">If logname is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string.</exception>
        protected Logger(string logname)
            : this(logname, DEFAULT_LEVEL_VALUE)
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the given log name and default level.
        /// </para>
        /// <para>
        /// Changes in 3.0: no named messages is provided.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <param name="defaultLevel">The default logging level.</param>
        /// <exception cref="ArgumentNullException">If logname is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string.</exception>
        protected Logger(string logname, Level defaultLevel)
            : this(logname, defaultLevel, new Dictionary<string, NamedMessage>())
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance of Logger with the log name and the default level and set of named
        /// messages.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="logname">The name of the logger.</param>
        /// <param name="defaultLevel">The default logging level.</param>
        /// <param name="namedMessages">The dictionary mapping identifiers to named messages that should be
        /// used by this logger.</param>
        /// <exception cref="ArgumentNullException">If logname or namedMessages is null.</exception>
        /// <exception cref="ArgumentException">If logname is an empty string, or namedMessages contains an
        /// empty key or any null values.</exception>
        protected Logger(string logname, Level defaultLevel, IDictionary<string, NamedMessage> namedMessages)
        {
            Helper.ValidateNotNullNotEmptyString(logname, "logname");
            Helper.ValidateNotNull(namedMessages, "namedMessages");
            if (namedMessages.ContainsKey(""))
            {
                throw new ArgumentException("namedMessages cannot contain null key.");
            }
            if (namedMessages.Values.Contains(null))
            {
                throw new ArgumentException("namedMessages cannot contain null values.");
            }

            this.logname = logname;
            this.defaultLevel = defaultLevel;
            this.namedMessages = new Dictionary<string, NamedMessage>(namedMessages);
        }

        #region Obsolete Methods

        /// <summary>
        /// <para>
        /// Creates a new instance of the Logger with setting loaded from the given properties dictionary.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is made obsolete, replaced by constructor with IConfiguration as parameter;
        /// no named messages will be provided.
        /// </para>
        /// </summary>
        /// <param name="properties">The configuration dictionary.</param>
        /// <exception cref="ArgumentNullException">If properties is null.</exception>
        /// <exception cref="ConfigException">
        /// If logger_name or default_level is not correctly retrieved from the properties.
        /// </exception>
        [Obsolete]
        protected Logger(IDictionary properties)
        {
            Helper.ValidateNotNull(properties, "properties");

            // load the logger name from the properties dictionary (required)
            logname = properties[LOGGER_NAME] as string;

            if (logname == null)
            {
                throw new ConfigException("Unable to load '" + LOGGER_NAME +
                    "' property from properties dictionary");
            }

            if (logname == string.Empty)
            {
                throw new ConfigException("'" + LOGGER_NAME +
                    "' property from properties dictionary can not be the empty string");
            }

            // load the default level from the properties dictionary (optional, default to Level.DEBUG)
            try
            {
                // Ensure the key exists
                if (properties[DEFAULT_LEVEL] != null)
                {
                    // Attempt to parse it
                    defaultLevel = (Level) Enum.Parse(typeof (Level), (string) properties[DEFAULT_LEVEL],
                        true);
                }
                else
                {
                    defaultLevel = DEFAULT_LEVEL_VALUE;
                }
            }
            catch (ArgumentException e)
            {
                // Unable to parse the Level Enum from the string in properties[]
                throw new ConfigException("'" + DEFAULT_LEVEL +
                    "' property from dictionary can not be correctly converted to a Level enum type.",
                    e);
            }

            // assign the namedMessages to empty
            namedMessages = new Dictionary<string, NamedMessage>();
        }

        #endregion

        /// <summary>
        /// <para>
        /// Creates a new instance of the Logger with setting loaded from the given configuration.
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
        protected Logger(IConfiguration configuration)
        {
            Helper.ValidateNotNull(configuration, "configuration");

            // load the logger name from configuration (required)
            logname = Helper.GetStringAttribute(configuration, LOGGER_NAME, true);

            // load the default level from the properties dictionary (optional, default to Level.DEBUG)
            defaultLevel = Helper.GetLevelAttribute(configuration, DEFAULT_LEVEL, DEFAULT_LEVEL_VALUE);

            // load NamedMessages child configuration
            IConfiguration namedMessagesConfig = configuration.GetChild(NAMED_MESSAGES);

            namedMessages = new Dictionary<string, NamedMessage>();

            // for each child configuration in namedMessagesConfig,
            // create a NamedMessage with attributes loaded from configuration and add to namedMessages
            if (namedMessagesConfig != null)
            {
                foreach (IConfiguration child in namedMessagesConfig.Children)
                {
                    AddNamedMessage(child);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Loads the attributes in given configuration to create a NamedMessage object and add it to
        /// namedMessages dictionary with name of configuration as key.
        /// </para>
        /// <para>
        /// If default_level is not provided in configuration, the default level of logger will be used
        /// as default level of named message.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to load settings from.</param>
        /// <exception cref="ConfigException">
        ///  If any of the configuration settings are missing or are invalid values.
        /// </exception>
        private void AddNamedMessage(IConfiguration config)
        {
            // load the text from configuration (required)
            string text = Helper.GetStringAttribute(config, TEXT, true);

            // load the default level from configuration (optional, default to the default level of logger)
            Level msgDefaultLevel = Helper.GetLevelAttribute(config, DEFAULT_LEVEL, defaultLevel);

            // load the parameters from configuration (optional)
            IList<string> parameters = Helper.GetStringListAttribute(config, PARAMETERS, false);

            // create a NamedMessage object
            NamedMessage msg = new NamedMessage(text, config.Name, parameters, msgDefaultLevel);

            // add to namedMessages dictionary
            namedMessages.Add(config.Name, msg);
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the logger.
        /// </para>
        /// <para>
        /// If the implementation holds resources such as a file, etc. then they should be released here if
        /// the logger will never be used again.
        /// </para>
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the specified logging level.
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
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        public abstract void Log(Level level, string message, params object[] param);

        /// <summary>
        /// <para>
        /// Logs a message using the underlying implementation with the default logging level.
        /// </para>
        /// <para>
        /// Changes in 3.0: Exceptions are now allowed and documented. Exceptions can be suppressed by
        /// wrapping the logger in an ExceptionSafeLogger; This method has been made virtual.
        /// </para>
        /// </summary>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message with
        /// params.</exception>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        public virtual void Log(string message, params object[] param)
        {
            Log(defaultLevel, message, param);
        }

        /// <summary>
        /// <para>
        /// Used to determine if a specific logging level is supported by underlying implementation.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true if the level is supported by the logger.</returns>
        public abstract bool IsLevelEnabled(Level level);

        /// <summary>
        /// <para>
        /// Logs a named message at its default level.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="messageIdentifier">The string identifying which named message to log.</param>
        /// <param name="param">The parameters to use to format the message.</param>
        /// <exception cref="ArgumentNullException">If messageIdentifier or param is null.</exception>
        /// <exception cref="ArgumentException">If messageIdentifier is the empty string, or
        /// messageIdentifier is not in the keys of the namedMessages dictionary.</exception>
        /// <exception cref="MessageFormattingException">If there is an error formatting the message from
        /// the params.</exception>
        /// <exception cref="LoggingException">If there is a failure in the backend logging system.
        /// </exception>
        public virtual void LogNamedMessage(string messageIdentifier, params object[] param)
        {
            Helper.ValidateNotNullNotEmptyString(messageIdentifier, "messageIdentifier");
            if (!namedMessages.ContainsKey(messageIdentifier))
            {
                throw new ArgumentException(string.Format("no message found for message identifier '{0}'.",
                    messageIdentifier));
            }
            LogNamedMessage(namedMessages[messageIdentifier].DefaultLevel, namedMessages[messageIdentifier],
                param);
        }

        /// <summary>
        /// <para>
        /// Logs a named message at the given level.
        /// </para>
        /// <para>
        /// New in 3.0.
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
        public virtual void LogNamedMessage(Level level, string messageIdentifier, params object[] param)
        {
            Helper.ValidateNotNullNotEmptyString(messageIdentifier, "messageIdentifier");
            if (!namedMessages.ContainsKey(messageIdentifier))
            {
                throw new ArgumentException(string.Format("no message found for message identifier '{0}'.",
                    messageIdentifier));
            }
            LogNamedMessage(level, namedMessages[messageIdentifier], param);
        }

        /// <summary>
        /// <para>
        /// Logs the named message to the underlying implementation with the specified logging level.
        /// </para>
        /// <para>
        /// Subclasses may override this method to perform appropriate backend specific processing, if the
        /// backend supports message parameters in a superior way to the normal way it is handled in the
        /// Log method.
        /// </para>
        /// <para>
        /// New in 3.0.
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
        protected internal virtual void LogNamedMessage(Level level, NamedMessage message,
            params object[] param)
        {
            Log(level, message.Text, param);
        }
    }
}