/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;
using log4net.Config;
using TopCoder.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// Log4NETImpl extends from the Logger abstract class to log messages using the logger from the
    /// log4net 3rd party component.
    /// </para>
    /// <para>
    /// Changes in 3.0: A new constructor is added while the old constructor is made obsolete; The
    /// InitializeZeroConfiguration method is added; Named message support is added; Immutable fields are
    /// marked readonly. Exceptions are now allowed and documented.
    /// </para>
    /// <para>
    /// Creating this Logger implementation is done by code like this:
    /// </para>
    /// <code>
    /// Logger logger = LogManager.CreateLogger("TopCoder.LoggingWrapper.Log4NETImpl");
    /// </code>
    /// <para>
    /// Once created, you may log messages at any of the levels defined in Level. For example, to log a
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
    /// Since the log4net is thread safe, and this class is immutable itself. So it is thread safe
    /// inherently.
    /// </para>
    /// </threadsafety>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class Log4NETImpl : Logger
    {
        /// <summary>
        /// <para>
        /// Logging level for the Log4NET component.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Enum is thread safe.
        /// </para>
        /// </remarks>
        /// <author>TCSDEVELOPER, Mikhail_T</author>
        /// <author>aubergineanode, TCSDEVELOPER</author>
        /// <version>3.0</version>
        /// <since>2.0</since>
        /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
        public enum Log4NETLevel
        {
            /// <summary>
            /// <para>
            /// Logging level OFF designates a lower level priority than all the rest.
            /// </para>
            /// </summary>
            OFF,

            /// <summary>
            /// <para>
            /// Logging level DEBUG designates fine-grained informational events that are most useful to
            /// debug an application.
            /// </para>
            /// </summary>
            DEBUG,

            /// <summary>
            /// <para>
            /// Logging level INFO  designates informational messages that highlight the progress of the
            /// application at coarse-grained level.
            /// </para>
            /// </summary>
            INFO,

            /// <summary>
            /// <para>
            /// Logging level WARN designates potentially harmful situations.
            /// </para>
            /// </summary>
            WARN,

            /// <summary>
            /// <para>
            /// Logging level ERROR designates error events that might still allow the application to
            /// continue running.
            /// </para>
            /// </summary>
            ERROR,

            /// <summary>
            /// <para>
            /// Logging level FATAL designates very severe error events that will presumably lead the
            /// application to abort.
            /// </para>
            /// </summary>
            FATAL
        }

        /// <summary>
        /// <para>
        /// Represents the "config_file" property key in the configuration. It is used by constructor to
        /// get the value to configure log4net.
        /// </para>
        /// <para>
        /// Changes in 3.0: This is the key used to load the value from IConfiguration instead of
        /// ConfigManager.
        /// </para>
        /// </summary>
        public const string CONFIG_FILE = "config_file";

        /// <summary>
        /// <para>
        /// The default value to set for "config_file" property in configuration. It is used in method
        /// InitializeZeroConfiguration.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        private const string DEFAULT_CONFIG_FILE = "log4net.config";

        /// <summary>
        /// <para>
        /// Represents the name of log4net element in xml configuration.
        /// </para>
        /// </summary>
        private const string LOG4NET_ELEMENT = "log4net";

        /// <summary>
        /// <para>
        /// Represents the name of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_ELEMENT = "appender";

        /// <summary>
        /// <para>
        /// Represents the name of name attribute of log4net element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_NAME_ATTRIBUTE = "name";

        /// <summary>
        /// <para>
        /// Represents the value of name attribute of log4net element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_NAME = "DefaultAppender";

        /// <summary>
        /// <para>
        /// Represents the name of root element in xml configuration.
        /// </para>
        /// </summary>
        private const string ROOT_ELEMENT = "root";

        /// <summary>
        /// <para>
        /// Represents the name of appender-ref element of root element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_REF_ELEMENT = "appender-ref";

        /// <summary>
        /// <para>
        /// Represents the name of ref attribute of appender-ref element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_REF_ATTRIBUTE = "ref";

        /// <summary>
        /// <para>
        /// Represents the name of level element in xml configuration.
        /// </para>
        /// </summary>
        private const string LEVEL_ELEMENT = "level";

        /// <summary>
        /// <para>
        /// Represents the name of type attribute of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_TYPE_ATTRIBUTE = "type";

        /// <summary>
        /// <para>
        /// Represents the name of file element of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_FILE_ELEMENT = "file";

        /// <summary>
        /// <para>
        /// Represents the name of dataPattern element of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_DATAPATTERN_ELEMENT = "datePattern";

        /// <summary>
        /// <para>
        /// Represents the name of staticLogFileName element of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_STATICLOGFILENAME_ELEMENT = "staticLogFileName";

        /// <summary>
        /// <para>
        /// Represents the name of rollingStyle element of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_ROLLINGSTYLE_ELEMENT = "rollingStyle";

        /// <summary>
        /// <para>
        /// Represents the name of maxSizeRollBackups element of appender element in xml configuration.
        /// </para>
        /// </summary>
        private const string APPENDER_MAXSIZEROLLBACKUPS_ELEMENT = "maxSizeRollBackups";

        /// <summary>
        /// <para>
        /// Represents the name of layout element in xml configuration.
        /// </para>
        /// </summary>
        private const string LAYOUT_ELEMENT = "layout";

        /// <summary>
        /// <para>
        /// Represents the name of type attribute of layout element in xml configuration.
        /// </para>
        /// </summary>
        private const string LAYOUT_TYPE_ATTRIBUTE = "type";

        /// <summary>
        /// <para>
        /// Represents the name of conversionPattern element of layout element in xml configuration.
        /// </para>
        /// </summary>
        private const string LAYOUT_CONVERSIONPATTERN_ELEMENT = "conversionPattern";

        /// <summary>
        /// <para>
        /// Represents the name of value element in xml configuration.
        /// </para>
        /// </summary>
        private const string VALUE_ELEMENT = "value";

        /// <summary>
        /// <para>
        /// Represents the level mapping from the Level enumeration to that of Log4NETLevel.
        /// </para>
        /// <para>
        /// The key is the Level value, and the value is the corresponding level in Log4NETLevel. It is
        /// populated in static constructor.
        /// </para>
        /// <para>
        /// Changes in 3.0: generic collection is used. And it is readonly now.
        /// </para>
        /// </summary>
        private readonly static IDictionary<Level, Log4NETLevel> levelMapping
            = new Dictionary<Level, Log4NETLevel>();

        /// <summary>
        /// <para>
        /// Represents ILog instance retrieved from the log4net.LogManager to log the message.
        /// </para>
        /// <para>
        /// It is initialized in the constructor, can not be null, and is immutable.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is readonly now.
        /// </para>
        /// </summary>
        private readonly ILog log = null;

        #region Obsolete Methods

        /// <summary>
        /// <para>
        /// Create a new instance of Log4NETImpl from the configuration values defined in the given
        /// properties dictionary.
        /// </para>
        /// <para>
        /// Changes in 3.0: It is made obsolete, replaced by constructor with IConfiguration as parameter.
        /// </para>
        /// </summary>
        /// <param name="properties">The configuration dictionary.</param>
        /// <exception cref="ArgumentNullException">If properties is null.</exception>
        /// <exception cref="ConfigException">If any failure creating the log4net instance is encountered.
        /// </exception>
        [Obsolete]
        public Log4NETImpl(IDictionary properties)
            : base(properties)
        {
            try
            {
                string loggerFilename = properties[CONFIG_FILE] as string;

                if (loggerFilename == null)
                {
                    throw new ConfigException("Property '" + CONFIG_FILE + "' is not a string.");
                }

                // check if the configuration file exists, before attempting to load it
                FileInfo file = new FileInfo(loggerFilename);

                if (!file.Exists)
                {
                    throw new ConfigException("Unable to load '" + loggerFilename + "' for Log4NET");
                }

                // configure the log4net implementation
                XmlConfigurator.Configure(file);

                // get the log4net implementation to use for the life of this class
                log = log4net.LogManager.GetLogger(Logname);

                if (log == null)
                {
                    throw new ConfigException("Unable to create '" + Logname + "' using log4net.");
                }
            }
            catch (ConfigException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to initialize log4net", e);
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// Create a new instance of Log4NETImpl with default logger name.
        /// </para>
        /// </summary>
        public Log4NETImpl()
            : base(Process.GetCurrentProcess().ProcessName)
        {
            try
            {
                string loggerFilename = ConfigurationManager.AppSettings["DEFAULT_LOG4NET_FILENAME"];
                if (loggerFilename == null)
                {
                    loggerFilename = DEFAULT_CONFIG_FILE;
                }

                DefaultConfiguration config = new DefaultConfiguration("log4net");
                config.SetSimpleAttribute(CONFIG_FILE, loggerFilename);
                InitializeZeroConfiguration(ZeroConfigurationOption.Certification, config);

                // check if the configuration file exists, before attempting to load it 
                FileInfo file = new FileInfo(loggerFilename);

                if (!file.Exists)
                {
                    throw new ConfigException("Unable to load '" + loggerFilename + "' for Log4NET");
                }

                // configure the log4net implementation 
                XmlConfigurator.Configure(file);

                // get the log4net implementation to use for the life of this class 
                log = log4net.LogManager.GetLogger(Logname);

                if (log == null)
                {
                    throw new ConfigException("Unable to create '" + Logname + "' using log4net.");
                }
            }
            catch (ConfigException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to initialize log4net", e);
            }
        }

        /// <summary>
        /// <para>
        /// Create a new instance of Log4NETImpl with setting loaded from the given configuration.
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
        public Log4NETImpl(IConfiguration configuration)
            : base(configuration)
        {
            // load config file from configuration (required)
            string loggerFilename = Helper.GetStringAttribute(configuration, CONFIG_FILE, true);

            try
            {
                // check if the configuration file exists, before attempting to load it
                FileInfo file = new FileInfo(loggerFilename);

                if (!file.Exists)
                {
                    throw new ConfigException("Unable to load '" + loggerFilename + "' for Log4NET");
                }

                // configure the log4net implementation
                XmlConfigurator.Configure(file);

                // get the log4net implementation to use for the life of this class
                log = log4net.LogManager.GetLogger(Logname);

                if (log == null)
                {
                    throw new ConfigException("Unable to create '" + Logname + "' using log4net.");
                }
            }
            catch (ConfigException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to initialize log4net", e);
            }
        }

        /// <summary>
        /// <para>
        /// Disposes the resources held by the Log4NETImpl instance.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            // does nothing
        }

        /// <summary>
        /// <para>
        /// Returns true if the level is supported by the Log4NETImpl instance. All levels in Level are
        /// supported by the current implementation.
        /// </para>
        /// </summary>
        /// <param name="level">The Level to check.</param>
        /// <returns>true for all levels in Level.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return true;
        }

        /// <summary>
        /// <para>
        /// Logs a message to the ILog instance used by this class.
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

            // log the formatted message
            LogMessage(level, formatted);
        }

        /// <summary>
        /// <para>
        /// Logs the named message to the ILog instance used by this class with the specified logging level.
        /// </para>
        /// <para>
        /// The params array will be used for both pattern like {0}, {1}, etc. in the text of named message,
        /// and the custom pattern like %property{name} in conversion pattern in log4net.
        /// </para>
        /// <para>
        /// If the level is not supported by this class, or the level is off, the message will not be logged.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="level">The level at which to log the message.</param>
        /// <param name="message">The named message to log.</param>
        /// <param name="param">The parameters to use in formatting the message.</param>
        /// <exception cref="ArgumentNullException">If message or params is null.</exception>
        /// <exception cref="MessageFormattingException">If the number of parameters in the params array
        /// does not match the number of parameters for the NamedMessage.</exception>
        /// <exception cref="LoggingException">If there is a failure in the log4net call.</exception>
        protected internal override void LogNamedMessage(Level level, NamedMessage message,
            params object[] param)
        {
            Helper.ValidateNotNull(message, "message");
            Helper.ValidateNotNull(param, "param");

            if (level == LoggingWrapper.Level.OFF || !IsLevelEnabled(level))
            {
                return;
            }

            // check the number of parameters match
            if (message.ParameterNames.Count != param.Length)
            {
                throw new MessageFormattingException("The number of parameters doesn't match");
            }

            // put the params in the log4Net context
            for (int i = 0; i < param.Length; i++)
            {
                LogicalThreadContext.Properties[message.ParameterNames[i]] = param[i];
            }

            // format the message with params
            string formatted;
            try
            {
                formatted = string.Format(message.Text, param);
            }
            catch (FormatException e)
            {
                throw new MessageFormattingException(
                    "Error occurs when formatting the message with params", e);
            }

            // Log the message
            LogMessage(level, formatted);

            // remove the params from log4Net context
            for (int i = 0; i < param.Length; i++)
            {
                LogicalThreadContext.Properties.Remove(message.ParameterNames[i]);
            }

        }

        /// <summary>
        /// <para>
        /// Logs the named message to the ILog instance used by this class with the specified logging level.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="level">The level at which to log the message.</param>
        /// <param name="message">The named message to log.</param>
        /// <exception cref="LoggingException">If an error occurs in the backend.</exception>
        private void LogMessage(Level level, string message)
        {
            try
            {
                switch (levelMapping[level])
                {
                    case Log4NETLevel.WARN:
                        log.Warn(message);
                        break;

                    case Log4NETLevel.DEBUG:
                        log.Debug(message);
                        break;

                    case Log4NETLevel.ERROR:
                        log.Error(message);
                        break;

                    case Log4NETLevel.FATAL:
                        log.Fatal(message);
                        break;

                    case Log4NETLevel.INFO:
                        log.Info(message);
                        break;
                }
            }
            catch (Exception e)
            {
                throw new LoggingException("Error occurs when logging", e);
            }
        }

        /// <summary>
        /// <para>
        /// Initializes the log4Net backend for the zero configuration logging option.
        /// </para>
        /// <para>
        /// If the "config_file" property is not in the configuration, the value "log4net.config" will be
        /// put as the value of this property into the configuration.
        /// </para>
        /// <para>
        /// If the config file does not exist, an appropriate log4Net configuration file will be written
        /// out to this location according to the zero-configuration option:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <term>Test</term>
        /// <description>The configuration uses a FileAppender that writes to the file
        /// ../../test_files/log.txt.</description>
        /// </item>
        /// <item>
        /// <term>Component</term>
        /// <description>The configuration uses a FileAppender that writes to the file ./log.txt.
        /// </description>
        /// </item>
        /// <item>
        /// <term>Certification</term>
        /// <description>The configuration uses a RollingFileAppender to write to the logs folder.
        /// Files will be rolled over daily, using a pattern of ¡°yyyy-mm-dd" for the files. The
        /// configuration is set up so as never to delete old logs.
        /// </description>
        /// </item>
        /// <item>
        /// <term> Client Debug</term>
        /// <description>This is the same as certification, but configured to keep only the 30 most recent
        /// logs.</description>
        /// </item>
        /// <item>
        /// <term>Client Stress</term>
        /// <description>This is the same as client debug, but configured so that only messages at the
        /// Level.Error (this is the log4net level) or higher are recorded.</description>
        /// </item>
        /// <item>
        /// <term>Release</term>
        /// <description>This is the same as client debug, but configured so that only messages at the
        /// Level.Warn (this.is the log4net level) or higher are recorded.</description>
        /// </item>
        /// </list>
        /// <para>
        /// If the config file does exist, we assume it is from a previous run and do not alter it.
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
        /// If any error occurs when accessing the configuration or writing the configuration file.
        /// </exception>
        public static void InitializeZeroConfiguration(ZeroConfigurationOption option,
            IConfiguration configuration)
        {
            Helper.ValidateNotNull(configuration, "configuration");

            // load the config file from configuration (optional)
            string configFile = Helper.GetStringAttribute(configuration, CONFIG_FILE, false);

            // set the property to default value in configuration if not present
            if (configFile == null)
            {
                configFile = DEFAULT_CONFIG_FILE;
                try
                {
                    configuration.SetSimpleAttribute(CONFIG_FILE, configFile);
                }
                catch (Exception e)
                {
                    throw new ConfigException("Error occurs when accessing the configuration", e);
                }
            }

            // check the config file exist or not
            if (!File.Exists(configFile))
            {
                try
                {
                    // create a log4Net configuration file
                    using (XmlTextWriter writer = new XmlTextWriter(configFile, null))
                    {
                        // enable indent
                        writer.Formatting = Formatting.Indented;

                        // write the configuration file
                        WriteConfigFile(writer, option);

                        writer.Flush();
                    }
                }
                catch (Exception e)
                {
                    throw new ConfigException("Fail to write the configuration file", e);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Writes the configuration file using given writer according to the zero-configuration option.
        /// </para>
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        /// <param name="option">The option about which zero-configuration setup should be configured in
        /// the backend.</param>
        private static void WriteConfigFile(XmlTextWriter writer, ZeroConfigurationOption option)
        {
            // write log4net element
            writer.WriteStartElement(LOG4NET_ELEMENT);

            // write appender element
            writer.WriteStartElement(APPENDER_ELEMENT);

            // write name attribute of appender element
            writer.WriteAttributeString(APPENDER_NAME_ATTRIBUTE, APPENDER_NAME);

            // write type attribute and the children elements of appender element
            if (option == ZeroConfigurationOption.Test)
            {
                WriteFileAppenderConfig(writer, @"..\..\test_files\log.txt");
            }
            else if (option == ZeroConfigurationOption.Component)
            {
                WriteFileAppenderConfig(writer, @"log.txt");
            }
            else
            {
                WriteRollingFileAppenderConfig(writer, option);
            }

            // write layout element
            WriteLayoutConfig(writer);

            // end append element
            writer.WriteEndElement();

            // write root element
            writer.WriteStartElement(ROOT_ELEMENT);

            // write appender-ref element of root element
            writer.WriteStartElement(APPENDER_REF_ELEMENT);

            // write ref attribute of appender-ref element
            writer.WriteAttributeString(APPENDER_REF_ATTRIBUTE, APPENDER_NAME);

            // end appender-ref element
            writer.WriteEndElement();

            // write level element
            if (option == ZeroConfigurationOption.Component)
            {
                WriteElement(writer, LEVEL_ELEMENT, "INFO");
            }
            else if (option == ZeroConfigurationOption.ClientStress)
            {
                WriteElement(writer, LEVEL_ELEMENT, "ERROR");
            }
            else if (option == ZeroConfigurationOption.Release)
            {
                WriteElement(writer, LEVEL_ELEMENT, "WARN");
            }

            // end root element
            writer.WriteEndElement();

            // end log4net element
            writer.WriteEndElement();
        }

        /// <summary>
        /// <para>
        /// Writes the type attribute and children elements of FileAppender element.
        /// </para>
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        /// <param name="file">The location of log file.</param>
        private static void WriteFileAppenderConfig(XmlTextWriter writer, string file)
        {
            // write type attribute
            writer.WriteAttributeString(APPENDER_TYPE_ATTRIBUTE, "log4net.Appender.FileAppender");

            // write file element
            WriteElement(writer, APPENDER_FILE_ELEMENT, file);
        }

        /// <summary>
        /// <para>
        /// Writes the type attribute and children elements of RollingFileAppender element.
        /// </para>
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        /// <param name="option">The option about which zero-configuration setup should be configured in
        /// the backend.</param>
        private static void WriteRollingFileAppenderConfig(XmlTextWriter writer,
            ZeroConfigurationOption option)
        {
            // write type attribute
            writer.WriteAttributeString(APPENDER_TYPE_ATTRIBUTE, "log4net.Appender.RollingFileAppender");

            // write file element
            WriteElement(writer, APPENDER_FILE_ELEMENT, @"logs\log");

            // write datePattern element (the file will be like log.2008-01-01.txt)
            WriteElement(writer, APPENDER_DATAPATTERN_ELEMENT, @".yyyy-MM-dd.\tx\t");

            // write staticLogFileName element (set to false)
            WriteElement(writer, APPENDER_STATICLOGFILENAME_ELEMENT, "false");

            if (option == ZeroConfigurationOption.Certification)
            {
                // for Certification option, roll daily with no limit on number of logs

                // write rollingStyle element
                WriteElement(writer, APPENDER_ROLLINGSTYLE_ELEMENT, "Date");

                // write maxSizeRollBackups element
                WriteElement(writer, APPENDER_MAXSIZEROLLBACKUPS_ELEMENT, "-1");
            }
            else
            {
                // for other options, roll daily and keep only the 30 most recent logs

                // write rollingStyle element
                WriteElement(writer, APPENDER_ROLLINGSTYLE_ELEMENT, "Composite");

                // write maxSizeRollBackups element
                WriteElement(writer, APPENDER_MAXSIZEROLLBACKUPS_ELEMENT, "30");
            }
        }

        /// <summary>
        /// <para>
        /// Writes a layout element for appender.
        /// </para>
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        private static void WriteLayoutConfig(XmlTextWriter writer)
        {
            // write layout element
            writer.WriteStartElement(LAYOUT_ELEMENT);

            // write type attribute
            writer.WriteAttributeString(LAYOUT_TYPE_ATTRIBUTE, "log4net.Layout.PatternLayout");

            // write conversionPattern element
            WriteElement(writer, LAYOUT_CONVERSIONPATTERN_ELEMENT,
                "%date [%thread] %-5level %logger [%ndc] - %message%newline");

            // end layout element
            writer.WriteEndElement();
        }

        /// <summary>
        /// <para>
        /// Writes an element with given name containing an attribute named "value" with given value.
        /// </para>
        /// </summary>
        /// <param name="writer">The writer to write.</param>
        /// <param name="name">The name of element.</param>
        /// <param name="value">The value of attribute named "value".</param>
        private static void WriteElement(XmlTextWriter writer, string name, string value)
        {
            // write the element with given name
            writer.WriteStartElement(name);

            // write the value attribute of element
            writer.WriteAttributeString(VALUE_ELEMENT, value);

            // end the element
            writer.WriteEndElement();
        }

        /// <summary>
        /// <para>
        /// This static constructor initializes the level mapping for all Log4NETImpl instances.
        /// </para>
        /// </summary>
        static Log4NETImpl()
        {
            levelMapping[Level.FATAL] = Log4NETLevel.FATAL;
            levelMapping[Level.ERROR] = Log4NETLevel.ERROR;
            levelMapping[Level.FAILUREAUDIT] = Log4NETLevel.DEBUG;
            levelMapping[Level.SUCCESSAUDIT] = Log4NETLevel.DEBUG;
            levelMapping[Level.WARN] = Log4NETLevel.WARN;
            levelMapping[Level.INFO] = Log4NETLevel.INFO;
            levelMapping[Level.DEBUG] = Log4NETLevel.DEBUG;
            levelMapping[Level.OFF] = Log4NETLevel.OFF;
        }
    }
}
