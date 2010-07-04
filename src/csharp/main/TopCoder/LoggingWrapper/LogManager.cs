/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using TopCoder.Configuration;
using System.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// LogManager contains methods Log and IsLevelEnabled to provide access to the pluggable logging
    /// solution and method GetLogger to retrieve the logging solution currently in use.
    /// </para>
    /// <para>
    /// Changes in 3.0: Refactored to use Configuration API and Filed Based Configuration instead of
    /// ConfigManager; Added support to use ExceptionSafeLogger and FilteredLevelLogger
    /// </para>
    /// </summary>
    /// <remarks>
    /// An example of creating a Logger with the following ConfigManager file:
    /// <pre>
    /// &lt;ConfigManager&gt;
    ///
    ///   &lt;!-- Example default configuration  --&gt;
    ///   &lt;namespace name="TopCoder.LoggingWrapper.LogManager"&gt;
    ///
    ///     &lt;!-- the full classname of the Logger subclass, required --&gt;
    ///     &lt;property name="logger_class"&gt;
    ///       &lt;value&gt;TopCoder.LoggingWrapper.SimpleLogger&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- name of assembly, optional --&gt;
    ///     &lt;property name="logger_assembly"&gt;
    ///       &lt;value&gt;TopCoder.LoggingWrapper.Test.dll&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- log name of Logger, required --&gt;
    ///     &lt;property name="logger_name"&gt;
    ///       &lt;value&gt;DefaultLogger&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- propagate exceptions, optional --&gt;
    ///     &lt;property name="propagate_exceptions"&gt;
    ///       &lt;value&gt;false&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- child namespace for exception logger, optional --&gt;
    ///     &lt;reference&gt;ExceptionLogger&lt;/reference&gt;
    ///
    ///     &lt;!-- child namespace for named messages, optional --&gt;
    ///     &lt;reference&gt;NamedMessages&lt;/reference&gt;
    ///
    ///     &lt;!-- default zero-configuration option, optional --&gt;
    ///     &lt;property name="default_config"&gt;
    ///       &lt;value&gt;Test&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- filtered levels, optional --&gt;
    ///     &lt;property name="filtered_levels"&gt;
    ///       &lt;value&gt;FATAL&lt;/value&gt;
    ///       &lt;value&gt;ERROR&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- default log level, optional --&gt;
    ///     &lt;property name="default_level"&gt;
    ///       &lt;value&gt;FATAL&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    ///   &lt;!-- nested logger for logging exceptions --&gt;
    ///   &lt;namespace name="ExceptionLogger"&gt;
    ///     &lt;!-- the full classname of the Logger subclass, required --&gt;
    ///     &lt;property name="logger_class"&gt;
    ///       &lt;value&gt;TopCoder.LoggingWrapper.DiagnosticImpl&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- log name of Logger, required --&gt;
    ///     &lt;property name="logger_name"&gt;
    ///       &lt;value&gt;ExceptionLogger&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- propagate exceptions, optional --&gt;
    ///     &lt;property name="propagate_exceptions"&gt;
    ///       &lt;value&gt;true&lt;/value&gt;
    ///     &lt;/property&gt;
    ///
    ///     &lt;!-- source for the EventLog, required --&gt;
    ///     &lt;property name="source"&gt;
    ///       &lt;value&gt;SourceForExceptionLogger&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    ///   &lt;!-- Example configuration for named messages --&gt;
    ///   &lt;namespace name="NamedMessages"&gt;
    ///     &lt;reference&gt;SimpleMessage&lt;/reference&gt;
    ///     &lt;reference&gt;Log4NetMessage&lt;/reference&gt;
    ///   &lt;/namespace&gt;
    ///
    ///   &lt;!-- Demonstrates a named message --&gt;
    ///   &lt;namespace name="SimpleMessage"&gt;
    ///     &lt;property name="text"&gt;
    ///       &lt;value&gt;The parameters are {0} and {1}&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="default_level"&gt;
    ///       &lt;value&gt;INFO&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="parameters"&gt;
    ///       &lt;value&gt;myParam1&lt;/value&gt;
    ///       &lt;value&gt;myParam2&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    ///   &lt;!-- Demonstrates a named message --&gt;
    ///   &lt;namespace name="Log4NetMessage"&gt;
    ///     &lt;property name="text"&gt;
    ///       &lt;value&gt;The parameters is %property{myParam}&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="default_level"&gt;
    ///       &lt;value&gt;WARN&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="parameters"&gt;
    ///       &lt;value&gt;param&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    /// &lt;/ConfigManager&gt;
    /// </pre>
    /// Would be like this:
    /// <code>
    /// Logger logger = LogManager.CreateLogger();
    /// </code>
    /// If you wished to specify a custom namespace, then it would look like this:
    /// <code>
    /// Logger logger = LogManager.CreateLogger("TopCoder.LoggingWrapper.LogManager");
    /// </code>
    /// Or you can specify the configuration object directly. The configuration object can be loaded
    /// from file, created manually or in other ways:
    /// <code>
    /// IConfiguration config =
    ///     new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");
    /// Logger logger = LogManager.CreateLogger(config);
    /// </code>
    /// Then you may use the logger implementation freely.
    /// </remarks>
    /// <threadsafety>
    /// <para>
    /// This class maintains no state, so it is thread-safe inherently.
    /// </para>
    /// </threadsafety>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class LogManager
    {
        #region Obsolete Members and Properties

        /// <summary>
        /// Represents the classname property to load the from the configuration.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        public static readonly string CLASS_PARAMETER = "PluginClassName";

        /// <summary>
        /// Represents the classname of the Logger subclass to instantiate.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        private static string classname = null;

        /// <summary>
        /// Represents the instance of Logger subclass to instantiate, it is used in the LogManager.Log
        /// methods, and created in the LoadConfiguration method, returned in the GetLogger methods.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        private static Logger log = null;

        /// <summary>
        /// Represents the configuration values to create the Logger instance to log the information.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        private static NameValueCollection configuration = null;

        /// <summary>
        /// Represents the configuration values to create the Logger instance to log the information.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        public static NameValueCollection Configuration
        {
            get
            {
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// Represents the default namespace to load the configuration values from configuration to create
        /// the Logger.
        /// </para>
        /// </summary>
        public const string DEFAULT_NAMESPACE = "TopCoder.LoggingWrapper.LogManager";

        /// <summary>
        /// <para>
        /// Represents the key for class name of the Logger subclass in configuration.
        /// </para>
        /// </summary>
        public const string LOGGER_CLASS = "logger_class";

        /// <summary>
        /// <para>
        /// Represents the key for assembly name of the Logger subclass in configuration. This is an
        /// optional argument.
        /// </para>
        /// </summary>
        public const string LOGGER_ASSEMBLY = "logger_assembly";

        /// <summary>
        /// <para>
        /// Represents the key for "default_config" in configuration. This is an optional argument.
        /// </para>
        /// </summary>
        private const string DEFAULT_CONFIG = "default_config";

        /// <summary>
        /// <para>
        /// Represents the key for "filtered_levels" in configuration. This is an optional argument.
        /// </para>
        /// </summary>
        private const string FILTERED_LEVELS = "filtered_levels";

        /// <summary>
        /// <para>
        /// Represents the key for "propagate_exceptions" in configuration. This is an optional argument.
        /// </para>
        /// </summary>
        private const string PROPAGATE_EXCEPTIONS = "propagate_exceptions";

        /// <summary>
        /// <para>
        /// Represents the key for "ExceptionLogger" in configuration. This is an optional argument.
        /// </para>
        /// </summary>
        private const string EXCEPTION_LOGGER = "ExceptionLogger";

        /// <summary>
        /// <para>
        /// Represent the defalut logger class used to be created.
        /// </para>
        /// <para>
        /// Bugr-fix 427
        /// </para>
        /// </summary>
        private const string DEFAULT_LOGGER_CLASS_APP_SETTING_NAME = "DEFAULT_LOGGER_CLASS"; 

        #region Obsolete Methods

        /// <summary>
        /// Static constructor for configuration loading before application starts.
        /// It is now Obsolete.
        /// </summary>
        [Obsolete]
        static LogManager()
        {
            LoadConfiguration();
        }

        /// <summary>
        /// Loads/reloads configuration file and loads pluggable logging solution
        /// It is now Obsolete.
        /// </summary>
        /// <exception cref="ConfigException">If there is an error loading the configuration file.
        /// </exception>
        [Obsolete]
        public static void LoadConfiguration()
        {
            lock (typeof (LogManager))
            {
                if (configuration != null && configuration.HasKeys())
                {
                    classname = Configuration[CLASS_PARAMETER];

                    if (classname == null)
                    {
                        throw new ConfigException("No name is given for the implementation.");
                    }

                    LoadPlugin(classname);
                }
            }
        }

        /// <summary>
        /// Loads pluggable logging solution with defined classname.
        /// It is now Obsolete.
        /// </summary>
        /// <param name="classname">The classname of the Logger subclass to load.</param>
        /// <exception cref="ArgumentNullException">If classname is null.</exception>
        /// <exception cref="ConfigException">
        /// If the classname can not be found in the assembly, or a general error loading the plugin occurs.
        /// </exception>
        /// <exception cref="InvalidPluginException">
        /// When the specified plugin's constructor can not be found.
        /// </exception>
        [Obsolete]
        public static void LoadPlugin(string classname)
        {
            if (classname == null)
            {
                throw new ArgumentNullException("classname");
            }

            try
            {
                LogManager.classname = classname;

                Type type = Type.GetType(LogManager.classname);

                if (type == null)
                {
                    throw new ConfigException(classname + " is not present in the default assembly.");
                }

                Hashtable properties = new Hashtable();

                foreach (string key in configuration)
                {
                    properties[key] = configuration.GetValues(key)[0];
                }

                // Convert the old style keys to the new ones so that the loggers can understand them
                properties[Logger.LOGGER_NAME] = properties["LogName"];
                properties[DiagnosticImpl.SOURCE] = properties["Source"];
                properties[Log4NETImpl.CONFIG_FILE] = properties["Log4NetConfiguration"];

                // Create the Logger instance, passing properties to it's constructor
                log = (Logger) Activator.CreateInstance(type, new object[] {properties});
            }
            catch (ConfigException exception)
            {
                throw exception;
            }
            catch (MissingMethodException)
            {
                throw new InvalidPluginException(
                    "There is no class or public constructor that exists.");
            }
            catch (Exception exception)
            {
                throw new ConfigException("Configuration error loading plugin.", exception);
            }

            if (log == null)
            {
                throw new ConfigException("Incorrect name for logging implementation.");
            }
        }

        /// <summary>
        /// Retrieves the Logger instance that is currently in use.
        /// It is now obsolete.
        /// </summary>
        /// <returns>The Logger instance currently in use.</returns>
        [Obsolete]
        public static Logger GetLogger()
        {
            return log;
        }

        /// <summary>
        /// Retrieves the full class name of logging solution currently in use.
        /// It is now obsolete.
        /// </summary>
        /// <returns>The full class name of logging solution currently in use.</returns>
        [Obsolete]
        public static string GetLoggerClassName()
        {
            return classname;
        }

        /// <summary>
        /// Loads and retrieve the logging solution with class name <c>classname</c>
        /// It is now obsolete.
        /// </summary>
        /// <param name="classname">Full name of class of pluggable logging solution.</param>
        /// <returns>The created Logger instance from the classname.</returns>
        /// <exception cref="ArgumentNullException">If classname is null.</exception>
        [Obsolete]
        public static Logger GetLogger(string classname)
        {
            if (classname == null)
            {
                throw new ArgumentNullException("classname");
            }

            // Load the Logger given the classname
            LoadPlugin(classname);

            return log;
        }

        /// <summary>
        /// Provides access to current pluggable logging solution
        /// It is now obsolete.
        /// </summary>
        /// <param name="level">logging level</param>
        /// <param name="message">logging message (can be contains {0},{1} for parameter inserting)</param>
        /// <param name="param">parameters for logging message (if needed)</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="ConfigException">If no logger has been set up yet.</exception>
        /// <exception cref="PluginException">If an exception occurs while Logging.</exception>
        [Obsolete]
        public static void Log(Level level, string message, params object[] param)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            if (log == null)
            {
                throw new ConfigException("Incorrect name of logging implementation");
            }

            try
            {
                lock (typeof (LogManager))
                {
                    log.Log(level, message, param);
                }
            }
            catch (Exception e)
            {
                throw new PluginException(
                    string.Format("Error in class {0} : {1}", classname, e.Message));
            }
        }

        /// <summary>
        /// Provides access to current pluggable logging solution with its default logging level
        /// </summary>
        /// <param name="message">logging message (can be contains {0},{1} for parameter inserting)</param>
        /// <param name="param">parameters for logging message (if needed)</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="ConfigException">If no logger has been set up yet.</exception>
        /// <exception cref="PluginException">If an exception occurs while Logging.</exception>
        [Obsolete]
        public static void Log(string message, params object[] param)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            if (log == null)
            {
                throw new ConfigException("Incorrect name of logging implementation");
            }

            try
            {
                lock (typeof (LogManager))
                {
                    log.Log(message, param);
                }
            }
            catch (Exception e)
            {
                throw new PluginException(
                    string.Format("Error in logging implementation class {0} : {1}",
                        classname, e.Message));
            }
        }

        /// <summary>
        /// Is used to determine if a specific logging level is enabled for current logging solution
        /// </summary>
        /// <param name="level">logging level</param>
        /// <returns>is enabled or not enabled</returns>
        /// <exception cref="ConfigException">If no logger has been set up yet.</exception>
        /// <exception cref="PluginException">If an exception occurs when calling IsLevelEnabled.</exception>
        [Obsolete]
        public static bool IsLevelEnabled(Level level)
        {
            if (log == null)
            {
                throw new ConfigException("Incorrect name of logging implementation");
            }

            try
            {
                return log.IsLevelEnabled(level);
            }
            catch (Exception e)
            {
                throw new PluginException(e.Message);
            }
        }

        #endregion

        /// <summary>
        /// <para>
        ///  It will default to returning a new implementation of "DiagnosticImpl", but will also allow
        ///  the user to override the implementing class simply by adding the fully qualified type name
        ///  to the appSetting section of web.config or app.config
        /// </para>
        /// <para>
        /// bugr-fix 427
        /// </para>
        /// </summary>
        /// <returns>The created Logger instance.</returns>
        public static Logger CreateLogger()
        {
            string loggerClass = ConfigurationManager.AppSettings[DEFAULT_LOGGER_CLASS_APP_SETTING_NAME];

            if (loggerClass != null)
            {
                Type loggerType = Type.GetType(loggerClass, false, true);

                if (loggerType != null)
                {
                    ConstructorInfo loggerCtor = loggerType.GetConstructor(new Type[0]);

                    if (loggerCtor != null)
                    {
                        Logger log = loggerCtor.Invoke(new object[0]) as Logger;

                        if (log != null)
                        {
                            return log;
                        }
                    }
                }
            }
            return new DiagnosticImpl(); 
        }

        /// <summary>
        /// <para>
        /// Creates a new Logger using the settings loaded from given configuration. Different
        /// implementations of the Logger abstract class may require different configuration values, each
        /// will load the implementation specific data from the properties.
        /// </para>
        /// <para>
        /// If the namespace is the default namespace for the component and the "default_config" property
        /// is provided in configuration, the zero-configuration setting will be used. Note that we only
        /// allow zero-configuration for the default namespace. Zero-configuration isn't allowed for
        /// multiple loggers because you're no longer in zero-configuration mode if you're using multiple
        /// loggers, and we don't want to risk two zero-configuration settings being contradictory.
        /// </para>
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        /// <param name="configuration">The configuration object to load settings from.</param>
        /// <returns>The created Logger instance.</returns>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <exception cref="ConfigException">If there is an error in instantiating the logger.
        /// </exception>
        public static Logger CreateLogger(IConfiguration configuration)
        {
            Helper.ValidateNotNull(configuration, "configuration");

            // get the class type through reflection
            Type type = GetClassType(configuration);

            // apply the zero configuration if default namespace is used and 'default_config' is provided
            // in configuration
            ApplyZeroConfiguration(configuration, type);

            // create the logger instance using reflection on the constructor taking an IConfiguration
            // argument
            Logger logger;
            try
            {
                logger = (Logger) Activator.CreateInstance(type, new object[] {configuration});
            }
            catch (Exception e)
            {
                throw new ConfigException("fail to create logger through reflection", e);
            }

            // apply policy about filtering levels to logger
            logger = ApplyFilterLevelsPolicy(configuration, logger);

            // apply policy about handling exception to logger
            return ApplyExceptionPolicy(configuration, logger);
        }

        /// <summary>
        /// <para>
        /// Returns the class type with given name. If assemblyName is not null, the assembly will be load
        /// for searching the type, otherwise the default assembly will be used.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to load settings from.</param>
        /// <returns>the class type with given name.</returns>
        /// <exception cref="ConfigException">If the class type can not be loaded.</exception>
        private static Type GetClassType(IConfiguration config)
        {
            // load logger class name from configuration (required)
            string className = Helper.GetStringAttribute(config, LOGGER_CLASS, true);

            // load logger assembly name from configuration (optional)
            string assemblyName = Helper.GetStringAttribute(config, LOGGER_ASSEMBLY, false);

            Type type;
            try
            {
                // attempt to load the assembly
                if (assemblyName != null)
                {
                    Assembly assembly;
                    try
                    {
                        // attempt to use method LoadFrom first for a filename assembly loading
                        assembly = Assembly.LoadFrom(assemblyName);
                    }
                    catch
                    {
                        // if that fails, attempt to use method Load
                        assembly = Assembly.Load(assemblyName);
                    }

                    // get the Logger Type from the specified assembly
                    type = assembly.GetType(className);
                }
                else
                {
                    // get the Logger Type from the default assembly
                    type = Type.GetType(className);
                }
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to load '" + className + "' because exception thrown.", e);
            }

            // check if the type was not found in the default or specified assembly
            if (type == null)
            {
                if (assemblyName == null)
                {
                    throw new ConfigException(className + " class is not present in the current assembly.");
                }
                else
                {
                    throw new ConfigException(className + " class is not present in the assembly '" +
                        assemblyName + "'.");
                }
            }

            return type;
        }

        /// <summary>
        /// <para>
        /// Applies the zero configuration by calling the static method InitializeZeroConfiguration of
        /// logger. The zero configuration will only be used if default namespace is used and property
        /// "default_config" is provided in configuration.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to load settings from.</param>
        /// <param name="type">The class type of logger.</param>
        /// <exception cref="ConfigException">If the value of propety is malformed, or any error occurs
        /// when loading the configuration or calling the static method.</exception>
        private static void ApplyZeroConfiguration(IConfiguration config, Type type)
        {
            // load zero configuration option from configuration (optional)
            string defaultConfig = Helper.GetStringAttribute(config, DEFAULT_CONFIG, false);

            if (defaultConfig == null)
            {
                return;
            }

            // parse the option
            ZeroConfigurationOption option;
            try
            {
                option = (ZeroConfigurationOption) Enum.Parse(typeof (ZeroConfigurationOption),
                    defaultConfig, true);
            }
            catch (Exception e)
            {
                throw new ConfigException("Invalid value '" + defaultConfig + "' of property '"
                    + DEFAULT_CONFIG + "' in configuration", e);
            }

            // use zero configuration only if default namespace is used and "default_config" is provided
            if (config.Name == DEFAULT_NAMESPACE)
            {
                // call the public static method InitializeZeroConfiguration
                try
                {
                    type.InvokeMember("InitializeZeroConfiguration",
                        BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                        null, null, new object[] {option, config});
                }
                catch (Exception e)
                {
                    throw new ConfigException("Error occurs when initializing the zero configuration", e);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns a LevelFilteredLogger wrapping the given logger with specified filtered levels loaded
        /// from configuration. If "filtered_levels" property is not provided in configuration, the given
        /// logger is returned directly.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to load settings from.</param>
        /// <param name="logger">The logger to be wrapped.</param>
        /// <returns>a LevelFilteredLogger wrapping given logger with specified filtered levels loaded
        /// from configuration, or given logger if "filtered_levels" property is not provided.</returns>
        /// <exception cref="ConfigException">If the value of property is malformed, or duplicate values
        /// are found in configuration, or any error occurs when loading the configuration.</exception>
        private static Logger ApplyFilterLevelsPolicy(IConfiguration config, Logger logger)
        {
            // load filtered levels from configuration (optional)
            IList<string> levelStrings = Helper.GetStringListAttribute(config, FILTERED_LEVELS, false);

            if (levelStrings.Count == 0)
            {
                return logger;
            }

            // parse the levels
            IList<Level> levels = new List<Level>();
            foreach (string levelString in levelStrings)
            {
                Level level;
                try
                {
                    level = (Level) Enum.Parse(typeof (Level), levelString, true);
                }
                catch (Exception e)
                {
                    throw new ConfigException("Invalid value '" + levelString + "' in property '"
                        + FILTERED_LEVELS + "' in configuration", e);
                }

                // check duplicate
                if (levels.Contains(level))
                {
                    throw new ConfigException("Duplicate values is not allowed in property '"
                        + FILTERED_LEVELS + "' in configuration");
                }

                // add to list
                levels.Add(level);
            }

            return new LevelFilteredLogger(logger, levels);
        }

        /// <summary>
        /// <para>
        /// Returns an ExceptionSafeLogger wrapping the given logger as underlying logger if the value of
        /// "propagate_exceptions" property is false or the property doesn't exist. Otherwise, the given
        /// logger will be returned directly.
        /// </para>
        /// <para>
        /// If child configuration "ExceptionLogger" is provided, an logger will be created using the
        /// child configuration and used as exception logger in the ExceptionSafeLogger to be returned.
        /// Otherwise, the given logger will be used as exception logger instead.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to load settings from.</param>
        /// <param name="logger">The logger to be wrapped.</param>
        /// <returns>an ExceptionSafeLogger wrapping the given logger as underlying logger, or given logger
        /// if the value of "propagate_exceptions" property is true.</returns>
        /// <exception cref="ConfigException">If the value of property is malformed, or any error occurs
        /// when loading the configuration or creating the exception logger.</exception>
        private static Logger ApplyExceptionPolicy(IConfiguration config, Logger logger)
        {
            // load propagate exceptions option from configuration (optional)
            bool propagate = Helper.GetBooleanAttribute(config, PROPAGATE_EXCEPTIONS, false);

            if (propagate)
            {
                return logger;
            }

            IConfiguration childConfig = config.GetChild(EXCEPTION_LOGGER);
            if (childConfig == null)
            {
                // if child configuration "ExceptionLogger" is not provided, return an ExceptionSafeLogger
                // with given logger as both underlying logger and exception logger
                return new ExceptionSafeLogger(logger, logger);
            }
            else
            {
                // otherwise, create the exception logger from child configuration
                // Note that the default level will be set to WARN instead of DEBUG

                // here we use a trick: if the default level is not set,
                // we add it before using it and remove it after it is used
                bool exist = (childConfig.GetSimpleAttribute(Logger.DEFAULT_LEVEL) != null);
                if (!exist)
                {
                    childConfig.SetSimpleAttribute(Logger.DEFAULT_LEVEL, Level.WARN.ToString());
                }

                Logger exceptionLogger;
                try
                {
                    exceptionLogger = CreateLogger(childConfig);
                }
                catch (Exception e)
                {
                    throw new ConfigException("Fail to create exception logger", e);
                }

                if (!exist)
                {
                    childConfig.RemoveAttribute(Logger.DEFAULT_LEVEL);
                }

                // return an ExceptionSafeLogger with given logger as underlying logger and created
                // exception logger
                return new ExceptionSafeLogger(logger, exceptionLogger);
            }
        }
    }
}
