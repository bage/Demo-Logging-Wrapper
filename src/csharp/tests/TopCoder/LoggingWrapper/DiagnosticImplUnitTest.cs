/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for DiagnosticImpl.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class DiagnosticImplUnitTest
    {
        /// <summary>
        /// A string as source of log for testing.
        /// </summary>
        private const string SOURCE = "DiagnosticImpl Source";

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
        /// An array of strings as parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETERS = new string[] {"param3", "param4"};

        /// <summary>
        /// A string as formatted message for testing.
        /// </summary>
        private static readonly string FORMATTED = string.Format(MESSAGE, PARAMETERS);

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// An instance of DiagnosticImpl for testing.
        /// </summary>
        private DiagnosticImpl logger;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", TestHelper.LOG_NAME);
            config.SetSimpleAttribute("default_level", DEFAULT_LEVEL.ToString());
            config.SetSimpleAttribute("source", SOURCE);

            logger = new DiagnosticImpl(config);
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
        /// Tests DiagnosticImpl(IConfiguration) for accuracy. Ensures all the fields are set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            Assert.IsNotNull(logger, "fail to create instance");

            Assert.AreEqual(TestHelper.LOG_NAME, logger.Logname, "incorrect log name");
            Assert.AreEqual(DEFAULT_LEVEL, logger.DefaultLevel, "incorrect default level");
            Assert.AreEqual(0, logger.NamedMessages.Count, "incorrect number of named messages");
        }

        /// <summary>
        /// Tests Log4NETImpl(IConfiguration) with null text.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNull()
        {
            new DiagnosticImpl((IConfiguration) null);
        }

        /// <summary>
        /// Tests constructor with "source" attribute missing in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithUrlAbsent()
        {
            config.RemoveAttribute("source");

            new DiagnosticImpl(config);
        }

        /// <summary>
        /// Tests constructor with empty value of "source" attribute in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithEmptyUrl()
        {
            config.SetSimpleAttribute("source", "");

            new DiagnosticImpl(config);
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
        /// Tests IsLevelEnabled for accuracy. Ensures all levels are supported.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledAccuracy()
        {
            Assert.IsTrue(logger.IsLevelEnabled(Level.DEBUG), "Level.DEBUG should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.ERROR), "Level.ERROR should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.FAILUREAUDIT), "Level.FAILUREAUDIT should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.FATAL), "Level.FATAL should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.INFO), "Level.INFO should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.OFF), "Level.OFF should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.SUCCESSAUDIT), "Level.SUCCESSAUDIT should be supported");
            Assert.IsTrue(logger.IsLevelEnabled(Level.WARN), "Level.WARN should be supported");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures message are logged properly.
        /// </summary>
        [Test]
        public void TestLogAccuracy()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            logger.Log(LEVEL, MESSAGE, PARAMETERS);

            // check the message logged by service
            Assert.IsTrue(TestHelper.GetLogEntry(SOURCE, index).Message.IndexOf(FORMATTED) >= 0,
                "incorrect message");
            Assert.AreEqual(EventLogEntryType.Warning, TestHelper.GetLogEntry(SOURCE, index).EntryType,
                "incorrect level");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures all levels are mapped correctly.
        /// </summary>
        [Test]
        public void TestLogLevelMapping()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            IDictionary<Level, EventLogEntryType> dict =
                new Dictionary<Level, EventLogEntryType>();
            dict[Level.FATAL] = EventLogEntryType.Error;
            dict[Level.ERROR] = EventLogEntryType.Error;
            dict[Level.FAILUREAUDIT] = EventLogEntryType.FailureAudit;
            dict[Level.SUCCESSAUDIT] = EventLogEntryType.SuccessAudit;
            dict[Level.WARN] = EventLogEntryType.Warning;
            dict[Level.INFO] = EventLogEntryType.Information;
            dict[Level.DEBUG] = EventLogEntryType.Information;

            foreach (Level level in dict.Keys)
            {
                logger.Log(level, MESSAGE, PARAMETERS);

                // check the type of message logged by service
                Assert.AreEqual(dict[level], TestHelper.GetLogEntry(SOURCE, index++).EntryType,
                    "incorrect level");
            }
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
        /// Tests InitializeZeroConfiguration with "source" attribute absent.
        /// Ensures the "source" attribute is set properly.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationUrlAbsent()
        {
            config.RemoveAttribute("source");

            DiagnosticImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Certification,
                config);

            Assert.AreEqual("TopCoder Logger", config.GetSimpleAttribute("source"),
                "incorrect value of source");
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "source" attribute exist.
        /// Ensures the "source" attribute is not changed.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationUrlExist()
        {
            DiagnosticImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            Assert.AreEqual(SOURCE, config.GetSimpleAttribute("source"),
                "incorrect value of source");
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with null configuration.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestInitializeZeroConfigurationWithNull()
        {
            DiagnosticImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, null);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with error occurs when accessing the configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestInitializeZeroConfigurationWithConfigError()
        {
            // set attribute to multiply values
            config.SetAttribute("source", new object[] {SOURCE, "other"});

            DiagnosticImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);
        }
    }
}
