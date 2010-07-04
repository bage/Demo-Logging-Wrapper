/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using TopCoder.Configuration;
using TopCoder.LoggingWrapper.ELS;
using TopCoder.LoggingWrapper.EntLib;
using TopCoder.Configuration.File;
using CM = System.Configuration.ConfigurationManager;
using System.Diagnostics;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for LogManager.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LogManagerUnitTest
    {
        /// <summary>
        /// Represent the field named "underlyingLogger" in ExceptionSafeLogger.
        /// </summary>
        private static readonly FieldInfo UL_ESL_FIELD =
            typeof (ExceptionSafeLogger).GetField("underlyingLogger",
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Represent the field named "exceptionLogger" in ExceptionSafeLogger.
        /// </summary>
        private static readonly FieldInfo EL_ESL_FIELD =
            typeof (ExceptionSafeLogger).GetField("exceptionLogger",
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Represent the field named "underlyingLogger" in LevelFilteredLogger.
        /// </summary>
        private static readonly FieldInfo UL_LFL_FIELD =
            typeof (LevelFilteredLogger).GetField("underlyingLogger",
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Represent the field named "filteredLevels" in LevelFilteredLogger.
        /// </summary>
        private static readonly FieldInfo FL_LFL_FIELD =
            typeof (LevelFilteredLogger).GetField("filteredLevels",
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// <para>
        /// Clears the custom event logs after all tests.
        /// </para>
        /// </summary>
        [TestFixtureTearDown]
        protected void RunAfterAllTests()
        {
            TestHelper.ClearLogs();
        }

        /// <summary>
        /// Tests CreateLogger() for accuracy.
        /// Ensures the default namespace is used and logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerDefault()
        {
            Logger logger = LogManager.CreateLogger();

            // check the logger
            Assert.AreEqual("Application", logger.Logname, "incorrect logger name");
            Assert.AreEqual(Level.DEBUG, logger.DefaultLevel, "incorrect default level of logger");
        }

        /// <summary>
        /// Tests CreateLogger(string) for accuracy. Only the required attributes are provided.
        /// Ensures the logger is created properly and default values of optional attributes are used.
        /// </summary>
        [Test]
        public void TestCreateLoggerMinimum()
        {
            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.SimpleLogger";

            Logger logger = LogManager.CreateLogger();

            // check the logger (exceptions are ignored by default)
            
            Assert.AreEqual("SimpleLogger", logger.Logname, "incorrect logger name");
            // Level.DEBUG by default
            Assert.AreEqual(Level.DEBUG, logger.DefaultLevel, "incorrect default level of logger");

            Assert.IsInstanceOfType(typeof (SimpleLogger), logger,"incorrect type of  logger");

            // no named messages by default
            Assert.AreEqual(0, logger.NamedMessages.Count, "no named messages expected");
        }

        /// <summary>
        /// Tests CreateLogger(string) for DiagnosticImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerDiagnosticImpl()
        {
            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.DiagnosticImpl";
            Logger logger = LogManager.CreateLogger();

            Assert.AreEqual(typeof (DiagnosticImpl), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("Application", logger.Logname, "incorrect logger name");

            // check source
            CheckField(logger, "source", Process.GetCurrentProcess().ProcessName);
        }

        /// <summary>
        /// Tests CreateLogger(string) for Log4NETImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerLog4NETImpl()
        {
            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.Log4NETImpl";
            Logger logger = LogManager.CreateLogger();

            Assert.AreEqual(typeof (Log4NETImpl), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual(Process.GetCurrentProcess().ProcessName, logger.Logname, "incorrect logger name");
        }

        /// <summary>
        /// Tests CreateLogger(string) for ELSImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerELSImpl()
        {
            //CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
            //    = "TopCoder.LoggingWrapper.ELS.ELSImpl";
            //Assert.IsNotNull(Type.GetType("TopCoder.LoggingWrapper.ELS.ELSImpl"));
            //Logger logger = LogManager.CreateLogger();

            //Assert.AreEqual(typeof (ELSImpl), logger.GetType(), "incorrect type of logger");
            //Assert.AreEqual("LogTest", logger.Logname, "incorrect logger name");
        }

        /// <summary>
        /// Tests CreateLogger(string) for EnterpriseLibraryLogger.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerEnterpriseLibraryLogger()
        {
            //CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
            //    = "TopCoder.LoggingWrapper.EntLib.EnterpriseLibraryLogger";
            //Logger logger = LogManager.CreateLogger();

            //Assert.AreEqual(typeof (EnterpriseLibraryLogger), logger.GetType(), "incorrect type of logger");
            //Assert.AreEqual("EnterpriseLibraryLogger", logger.Logname, "incorrect logger name");

            //// check source
            //CheckField(logger, "category", "MyCategory");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for accuracy. Only the required attributes are provided.
        /// Ensures the logger is created properly and default values of optional attributes are used.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigMinimum()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.Minimum");

            Logger logger = LogManager.CreateLogger(config);

            // check the logger (exceptions are ignored by default)
            Assert.AreEqual(typeof (ExceptionSafeLogger), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("MinimumLogger", logger.Logname, "incorrect logger name");
            // Level.DEBUG by default
            Assert.AreEqual(Level.DEBUG, logger.DefaultLevel, "incorrect default level of logger");

            // check underlying logger
            object underlyingLogger = UL_ESL_FIELD.GetValue(logger);
            Assert.AreEqual(typeof (SimpleLogger), underlyingLogger.GetType(),
                "incorrect type of underlying logger");

            // check exception logger (underlying logger is used by default)
            Assert.AreSame(underlyingLogger, EL_ESL_FIELD.GetValue(logger), "incorrect exception logger");

            // no zero configuration by default
            Assert.IsFalse((underlyingLogger as SimpleLogger).IsInitiaized, "no zero configuration expected");

            // no named messages by default
            Assert.AreEqual(0, logger.NamedMessages.Count, "no named messages expected");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for accuracy.
        /// Namespace "TopCoder.LoggingWrapper.ExceptionPropagated" is used.
        /// Ensures ExceptionSafeLogger will not be created.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigExceptionPropagated()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.ExceptionPropagated");

            Logger logger = LogManager.CreateLogger(config);

            // check the logger (no ExceptionSafeLogger wrapping)
            Assert.AreEqual(typeof (SimpleLogger), logger.GetType(), "incorrect type of logger");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for accuracy.
        /// Namespace "TopCoder.LoggingWrapper.LevelsFiltered" is used.
        /// Ensures LevelFilteredLogger will be created.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigLevelsFiltered()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LevelsFiltered");

            Logger logger = LogManager.CreateLogger(config);

            // check the logger
            Assert.AreEqual(typeof (LevelFilteredLogger), logger.GetType(), "incorrect type of logger");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for accuracy.
        /// Namespace "TopCoder.LoggingWrapper.ZeroConfiguration" is used.
        /// Ensures zero configuration will not be used since this is not default namespace.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigZeroConfiguration()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.ZeroConfiguration");

            Logger logger = LogManager.CreateLogger(config);

            // no zero configuration by default
            Assert.IsFalse((logger as SimpleLogger).IsInitiaized, "no zero configuration expected");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for DiagnosticImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigDiagnosticImpl()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.DiagnosticImpl");

            Logger logger = LogManager.CreateLogger(config);

            Assert.AreEqual(typeof (DiagnosticImpl), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("LogTest", logger.Logname, "incorrect logger name");

            // check source
            CheckField(logger, "source", "SourceForDiagnosticImpl");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for Log4NETImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigLog4NETImpl()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.Log4NETImpl");

            Logger logger = LogManager.CreateLogger(config);

            Assert.AreEqual(typeof (Log4NETImpl), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("Log4NETLogger", logger.Logname, "incorrect logger name");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for ELSImpl.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigELSImpl()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.ELSImpl");

            Logger logger = LogManager.CreateLogger(config);

            Assert.AreEqual(typeof (ELSImpl), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("LogTest", logger.Logname, "incorrect logger name");
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) for EnterpriseLibraryLogger.
        /// Ensures logger is created properly.
        /// </summary>
        [Test]
        public void TestCreateLoggerConfigEnterpriseLibraryLogger()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.EnterpriseLibraryLogger");

            Logger logger = LogManager.CreateLogger(config);

            Assert.AreEqual(typeof (EnterpriseLibraryLogger), logger.GetType(), "incorrect type of logger");
            Assert.AreEqual("EnterpriseLibraryLogger", logger.Logname, "incorrect logger name");

            // check source
            CheckField(logger, "category", "MyCategory");
        }



        /// <summary>
        /// Tests CreateLogger(IConfiguration) with
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCreateLoggerConfigWithNull()
        {
            LogManager.CreateLogger((IConfiguration) null);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with "logger_class" attribute absent.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerNoLoggerClass()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.RemoveAttribute("logger_class");

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with error occurs when creating logger through reflection
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerReflectionError()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.RemoveAttribute("logger_name");

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with error occurs when applying zero configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerZeroConfigurationError()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.SetSimpleAttribute("logger_class", "TopCoder.LoggingWrapper.AnotherSimpleLogger");

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with invalid value of zero configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerInvalidZeroConfigurationOption()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.SetSimpleAttribute("default_config", "unknown");

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with invalid value of filter levels.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerInvalidFilterLevels()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.SetAttribute("filtered_levels", new object[] {"unknown"});

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with duplicate value of filter levels.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerDuplicateFilterLevels()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.SetAttribute("filtered_levels", new object[] {"INFO", "INFO"});

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Tests CreateLogger(IConfiguration) with error occurs when creating exception logger.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCreateLoggerExceptionLoggerError()
        {
            // load the config
            IConfiguration config =
                new ConfigurationManager().GetConfiguration("TopCoder.LoggingWrapper.LogManager");

            config.GetChild("ExceptionLogger").RemoveAttribute("logger_name");

            LogManager.CreateLogger(config);
        }

        /// <summary>
        /// Checks the named messages in given logger is correct.
        /// </summary>
        /// <param name="logger">The logger to check.</param>
        private void CheckNamedMessages(Logger logger)
        {
            Assert.AreEqual(2, logger.NamedMessages.Count, "incorrect number of named messages");

            NamedMessage msg = logger.NamedMessages["SimpleMessage"];
            Assert.AreEqual("The parameters are {0} and {1}", msg.Text, "incorrect text of named message");
            Assert.AreEqual(Level.INFO, msg.DefaultLevel, "incorrect level of named message");
            Assert.AreEqual(2, msg.ParameterNames.Count, "incorrect number of parameters in named message");
            Assert.AreEqual("myParam1", msg.ParameterNames[0], "incorrect parameter in named message");
            Assert.AreEqual("myParam2", msg.ParameterNames[1], "incorrect parameter in named message");

            msg = logger.NamedMessages["Log4NetMessage"];
            Assert.AreEqual("The parameters is %property{myParam}", msg.Text,
                "incorrect text of named message");
            Assert.AreEqual(Level.WARN, msg.DefaultLevel, "incorrect level of named message");
            Assert.AreEqual(1, msg.ParameterNames.Count, "incorrect number of parameters in named message");
            Assert.AreEqual("param", msg.ParameterNames[0], "incorrect parameter in named message");
        }

        /// <summary>
        /// Checks the value the field of given name of logger.
        /// </summary>
        /// <param name="logger">The logger to check.</param>
        /// <param name="name">The name of field.</param>
        /// <param name="value">The expected value of field.</param>
        private static void CheckField(Logger logger, string name, object value)
        {
            FieldInfo field = logger.GetType().GetField(name,
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(value, field.GetValue(logger), "incorrect value of field " + name);
        }
    }
}
