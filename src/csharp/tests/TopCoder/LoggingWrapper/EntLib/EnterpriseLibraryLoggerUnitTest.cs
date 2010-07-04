/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Diagnostics;
using NUnit.Framework;
using TopCoder.Configuration;
using System.Configuration;

namespace TopCoder.LoggingWrapper.EntLib
{
    /// <summary>
    /// Unit test for EnterpriseLibraryLogger.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class EnterpriseLibraryLoggerUnitTest
    {
        /// <summary>
        /// A string as name of logger for testing.
        /// </summary>
        private const string LOGGER_NAME = "EnterpriseLibraryLogger";

        /// <summary>
        /// A string as source of log for testing.
        /// </summary>
        private const string SOURCE = "EnterpriseLibraryLogger Source";

        /// <summary>
        /// A level as level of logger for testing.
        /// </summary>
        private const Level DEFAULT_LEVEL = Level.INFO;

        /// <summary>
        /// A level as level of message for testing.
        /// </summary>
        private const Level LEVEL = Level.WARN;

        /// <summary>
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "message {0} and {1}";

        /// <summary>
        /// A string as category for testing.
        /// </summary>
        private const string CATEGORY = "custom category";

        /// <summary>
        /// An array of strings as parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETERS = new string[] {"param1", "param2"};

        /// <summary>
        /// A string as formatted message for testing.
        /// </summary>
        private static readonly string FORMATTED = string.Format(MESSAGE, PARAMETERS);

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// An instance of EnterpriseLibraryLogger for testing.
        /// </summary>
        private EnterpriseLibraryLogger logger;

        /// <summary>
        /// <para>
        /// Creats custom event log before all tests.
        /// </para>
        /// </summary>
        [TestFixtureSetUp]
        protected void RunBeforeAllTests()
        {
            TestHelper.CreateLogs(SOURCE);
        }

        /// <summary>
        /// Sets up test environment.
        /// </summary>
        ///
        [SetUp]
        protected void SetUp()
        {
            config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", LOGGER_NAME);
            config.SetSimpleAttribute("default_level", DEFAULT_LEVEL.ToString());
            config.SetSimpleAttribute("Category", CATEGORY);

            logger = new EnterpriseLibraryLogger(config);
        }

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
        /// Tests Category for accuracy. Ensures the field is returned properly.
        /// </summary>
        [Test]
        public void TestCategoryAccuracy()
        {
            Assert.AreEqual(CATEGORY, logger.Category, "incorrect category");
        }

        /// <summary>
        /// Tests constructor for accuracy. Ensures all the fields are set properly. Also tests property
        /// Category.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            Assert.IsNotNull(logger, "fail to create instance");

            Assert.AreEqual(LOGGER_NAME, logger.Logname, "incorrect log name");
            Assert.AreEqual(DEFAULT_LEVEL, logger.DefaultLevel, "incorrect default level");
            Assert.AreEqual(0, logger.NamedMessages.Count, "incorrect number of named messages");
            Assert.AreEqual(CATEGORY, logger.Category, "incorrect category");
        }

        /// <summary>
        /// Tests constructor with null text.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNull()
        {
            new EnterpriseLibraryLogger(null);
        }

        /// <summary>
        /// Tests constructor with "Category" attribute missing in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithCategoryAbsent()
        {
            config.RemoveAttribute("Category");

            new EnterpriseLibraryLogger(config);
        }

        /// <summary>
        /// Tests constructor with empty value of "Category" attribute in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithEmptyCategory()
        {
            config.SetSimpleAttribute("Category", "");

            new EnterpriseLibraryLogger(config);
        }

        /// <summary>
        /// Tests constructor with multiply values of "Category" attribute in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithMultiplyCategory()
        {
            config.SetAttribute("Category", new object[] {"category1", "category2"});

            new EnterpriseLibraryLogger(config);
        }

        /// <summary>
        /// Tests Dispose for accuracy.
        /// </summary>
        [Test]
        public void TestDisposeAccuracy()
        {
            logger.Dispose();
        }

        /// <summary>
        /// Tests IsLevelEnabled for accuracy. Ensures all levels except OFF are supported.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledAccuracy()
        {
            Assert.IsTrue(logger.IsLevelEnabled(Level.DEBUG), "Level.DEBUG should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.ERROR), "Level.ERROR should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.FAILUREAUDIT), "Level.FAILUREAUDIT should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.FATAL), "Level.FATAL should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.INFO), "Level.INFO should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.SUCCESSAUDIT), "Level.SUCCESSAUDIT should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.WARN), "Level.WARN should be supported");
            Assert.IsFalse(logger.IsLevelEnabled(Level.OFF), "Level.OFF should not be supported");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures all the parameters are sent to service.
        /// </summary>
        [Test]
        public void TestLogAccuracy()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            
            logger.Log(LEVEL, MESSAGE, PARAMETERS);

            // check the message logged by service
            string msg = TestHelper.GetLogEntry(SOURCE, index).Message;

            Assert.IsTrue(msg.IndexOf(FORMATTED) >= 0, "incorrect message");
            Assert.IsTrue(msg.IndexOf(CATEGORY) >= 0, "incorrect category");
            Assert.IsTrue(msg.IndexOf(TraceEventType.Warning.ToString()) >= 0, "incorrect level");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures message won't be logged for OFF level.
        /// </summary>
        [Test]
        public void TestLogWithOFF()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            string msg = "never be seen";

            logger.Log(Level.OFF, msg, PARAMETERS);

            Assert.AreEqual(index, TestHelper.GetLogsCount(SOURCE), "message should not be logged");
        }

        /// <summary>
        /// Tests Log with null message.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogWithNullMessage()
        {
            logger.Log(LEVEL, null, PARAMETERS);
        }

        /// <summary>
        /// Tests Log with null parameters.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogWithNullParameters()
        {
            logger.Log(LEVEL, MESSAGE, null);
        }

        /// <summary>
        /// Tests Log with message and parameters not matching.
        /// MessageFormattingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (MessageFormattingException))]
        public void TestLogWithMalformedMessage()
        {
            logger.Log(LEVEL, "malform {0}, {1}, {2}", PARAMETERS);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "Category" attribute absent.
        /// Ensures the "Category" attribute is set properly.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationUrlAbsent()
        {
            config.RemoveAttribute("Category");

            EnterpriseLibraryLogger.InitializeZeroConfiguration(ZeroConfigurationOption.Certification,
                config);

            Assert.AreEqual("TopCoder Logger", config.GetSimpleAttribute("Category"),
                "incorrect value of Category");
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "Category" attribute exist.
        /// Ensures the "Category" attribute is not changed.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationUrlExist()
        {
            EnterpriseLibraryLogger.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            Assert.AreEqual(CATEGORY, config.GetSimpleAttribute("Category"),
                "incorrect value of Category");
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with null configuration.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestInitializeZeroConfigurationWithNull()
        {
            EnterpriseLibraryLogger.InitializeZeroConfiguration(ZeroConfigurationOption.Test, null);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with error occurs when accessing the configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestInitializeZeroConfigurationWithConfigError()
        {
            // set attribute to multiply values
            config.SetAttribute("Category", new object[] {CATEGORY, "other"});

            EnterpriseLibraryLogger.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);
        }
    }
}