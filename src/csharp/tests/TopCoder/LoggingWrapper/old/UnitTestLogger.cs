// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;
    using System.Collections;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests the Logger class.  Uses a specificially derived class to make sure that the concrete Logger methods
    /// perform the correct operations they should.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    [TestFixture]
    [CoverageExclude]
    public class UnitTestLogger
    {
        /// <summary>
        /// Makes sure the Logger.DEFAULT_LEVEL constant is correctly set.
        /// </summary>
        [Test]
        public void TestDefaultLevelStatic()
        {
            Assertion.Assert("Logger.DEFAULT_LEVEL is not set correctly.", Logger.DEFAULT_LEVEL == "default_level");
        }

        /// <summary>
        /// Makes sure the Logger.LOGGER_NAME constant is correctly set.
        /// </summary>
        [Test]
        public void TestLoggerNameStatic()
        {
            Assertion.Assert("Logger.LOGGER_NAME is not set correctly.", Logger.LOGGER_NAME == "logger_name");
        }

        /// <summary>
        /// Attempts to create a Logger with a null logname.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateLoggerNullLogname1()
        {
            new LoggerTester((string)null);
        }

        /// <summary>
        /// Attempts to create a Logger with a null logname.
        /// Should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void CreateLoggerEmptyLogname1()
        {
            new LoggerTester("");
        }

        /// <summary>
        /// Creates a Logger and verifies that the public properties are correctly initialized.
        /// </summary>
        [Test]
        public void CreateLogger1()
        {
            LoggerTester logger = new LoggerTester("test name");

            Assertion.Assert("Logger constructor not initializing LogName correctly.", logger.Logname == "test name");
            Assertion.Assert("Logger constructor not initializing Level correctly.",
                logger.DefaultLevel == Level.DEBUG);
        }

        /// <summary>
        /// Attempts to create a Logger with a null logname, 2 parameter version.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateLoggerNullLogname2()
        {
            new LoggerTester((string)null, Level.DEBUG);
        }

        /// <summary>
        /// Attempts to create a Logger with an empty logname, 2 parameter version.
        /// Should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void CreateLoggerEmptyLogname2()
        {
            new LoggerTester("", Level.DEBUG);
        }

        /// <summary>
        /// Creates a Logger and verifies that the public properties are correctly initialized.
        /// </summary>
        [Test]
        public void CreateLogger2()
        {
            LoggerTester logger = new LoggerTester("test name", Level.FAILUREAUDIT);

            Assertion.Assert("Logger constructor not initializing LogName correctly.", logger.Logname == "test name");
            Assertion.Assert("Logger constructor not initializing Level correctly.",
                logger.DefaultLevel == Level.FAILUREAUDIT);
        }

        /// <summary>
        /// Attempts to create a Logger with a null IDictionary.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateLogger3NullParam()
        {
            new LoggerTester((IDictionary)null);
        }

        /// <summary>
        /// Attempts to create a Logger with no logger_name variable.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLogger3NoLoggerName()
        {
            Hashtable param = new Hashtable();
            param[Logger.DEFAULT_LEVEL] = Level.ERROR.ToString();

            new LoggerTester(param);
        }

        /// <summary>
        /// Attempts to create a Logger with a null logger_name variable.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLogger3NullLoggerName()
        {
            Hashtable param = new Hashtable();
            param[Logger.DEFAULT_LEVEL] = Level.ERROR.ToString();
            param[Logger.LOGGER_NAME] = null;

            new LoggerTester(param);
        }

        /// <summary>
        /// Attempts to create a Logger with an empty logger_name variable.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLogger3EmptyLoggerName()
        {
            Hashtable param = new Hashtable();
            param[Logger.DEFAULT_LEVEL] = Level.ERROR.ToString();
            param[Logger.LOGGER_NAME] = "";

            new LoggerTester(param);
        }

        /// <summary>
        /// Attempts to create a Logger with no default_level variable.  This should succeed as it is optional.
        /// </summary>
        [Test]
        public void CreateLogger3NoDefaultLevel()
        {
            Hashtable param = new Hashtable();
            param[Logger.LOGGER_NAME] = "test name";

            new LoggerTester(param);
        }

        /// <summary>
        /// Attempts to create a Logger with an invalid default_level variable.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLogger3InvalidDefaultLevel()
        {
            Hashtable param = new Hashtable();
            param[Logger.DEFAULT_LEVEL] = "invalid level";
            param[Logger.LOGGER_NAME] = "test name";

            new LoggerTester(param);
        }

        /// <summary>
        /// Creates a valid Logger instance and checks that the properties have been correctly initialized.
        /// </summary>
        [Test]
        public void CreateLogger3()
        {
            Hashtable param = new Hashtable();
            param[Logger.DEFAULT_LEVEL] = Level.ERROR.ToString();
            param[Logger.LOGGER_NAME] = "test name";

            Logger logger = new LoggerTester(param);

            Assertion.Assert("Logger constructor not initializing LogName correctly.", logger.Logname == "test name");
            Assertion.Assert("Logger constructor not initializing Level correctly.",
                logger.DefaultLevel == Level.ERROR);
        }

        /// <summary>
        /// Calls the Log() message and ensures that the parameters are correctly passed through.
        /// </summary>
        [Test]
        public void LogMessagePassParameters()
        {
            LoggerTester logger = new LoggerTester("logname");

            string message = "test {0}";
            object[] param = new object[1] { 5 };

            logger.Log(message, param);

            Assertion.Assert("Derived Log() not called.", logger.LogCalled == true);
            Assertion.Assert("Derived Log() level not correct.", logger.LogLevelCalled == Level.DEBUG);
            Assertion.Assert("Derived Log() message not correct.", logger.LogMessageCalled == message);
            Assertion.Assert("Derived Log() param not correct.", logger.LogParamCalled == param);
        }
    }
}
