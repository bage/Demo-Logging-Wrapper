/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using NUnit.Framework;
using TopCoder.Configuration;
using TopCoder.Configuration.File;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// Unit test for ELSImpl.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class ELSImplUnitTest
    {
        /// <summary>
        /// A string as name of logger for testing.
        /// </summary>
        internal const string LOGGER_NAME = "LogTest";

        /// <summary>
        /// A string as source of log for testing.
        /// </summary>
        internal const string SOURCE = "ELSImplSource";

        /// <summary>
        /// A level as level of logger for testing.
        /// </summary>
        private const Level DEFAULT_LEVEL = Level.INFO;

        /// <summary>
        /// A level as level of message for testing.
        /// </summary>
        private const Level LEVEL = Level.WARN;

        /// <summary>
        ///
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "message1";

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// An instance of ELSImpl for testing.
        /// </summary>
        private ELSImpl els;

        /// <summary>
        /// Sets up test environment.
        /// </summary>
        ///
        [SetUp]
        protected void SetUp()
        {
            config = (new ConfigurationManager()).
                GetConfiguration("TopCoder.LoggingWrapper.ELSImpl");

            els = new ELSImpl(config);
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
        /// Tests constructor for accuracy. Ensures all the fields are set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            Assert.IsNotNull(els, "fail to create instance");

            Assert.AreEqual(LOGGER_NAME, els.Logname, "incorrect log name");
            Assert.AreEqual(DEFAULT_LEVEL, els.DefaultLevel, "incorrect default level");
            Assert.AreEqual(0, els.NamedMessages.Count, "incorrect number of named messages");
        }

        /// <summary>
        /// Tests constructor with null text.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCtorWithNull()
        {
            new ELSImpl(null);
        }

        /// <summary>
        /// Tests constructor with bad configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void TestCtorBadConfig()
        {
            config = new DefaultConfiguration("BadConfig");

            new ELSImpl(config);
        }

        /// <summary>
        /// Tests Dispose for accuracy. Ensures the service won't work after Dispose is called.
        /// </summary>
        [Test, ExpectedException(typeof(LoggingException))]
        public void TestDisposeAccuracy()
        {
            els.Dispose();

            els.Log(LEVEL, MESSAGE);
        }

        /// <summary>
        /// Tests IsLevelEnabled for accuracy. Ensures all levels are supported.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledAccuracy()
        {
            Assert.IsTrue(els.IsLevelEnabled(Level.DEBUG), "Level.DEBUG should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.ERROR), "Level.ERROR should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.FAILUREAUDIT), "Level.FAILUREAUDIT should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.FATAL), "Level.FATAL should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.INFO), "Level.INFO should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.OFF), "Level.OFF should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.SUCCESSAUDIT), "Level.SUCCESSAUDIT should be supported");
            Assert.IsTrue(els.IsLevelEnabled(Level.WARN), "Level.WARN should be supported");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures all the parameters are sent to service.
        /// </summary>
        [Test]
        public void TestLogAccuracy()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            els.Log(LEVEL, MESSAGE);

            // check the message logged by service
            string msg = TestHelper.GetLogEntry(SOURCE, index).Message;

            string[] param = msg.Split(new char[] { ':' });

            Assert.AreEqual(LOGGER_NAME, param[0], "incorrect originator");
            Assert.AreEqual("null", param[1], "incorrect context");
            Assert.AreEqual(MESSAGE, param[2], "incorrect message");
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

            els.Log(Level.OFF, msg);

            Assert.AreEqual(index, TestHelper.GetLogsCount(SOURCE), "message should not be logged");
        }

        /// <summary>
        /// Tests Log with null message.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLogWithNullMessage()
        {
            els.Log(LEVEL, null);
        }

        /// <summary>
        /// Tests Log with null parameters.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLogWithNullParameters()
        {
            els.Log(LEVEL, MESSAGE, null);
        }

        /// <summary>
        /// Tests Log with error occurs when forwarding to service.
        /// LoggingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(LoggingException))]
        public void TestLogWithServiceError()
        {
            ILoggingServiceDummyImpl.fail = true;
            try
            {
                els.Log(LEVEL, MESSAGE);
            }
            finally
            {
                ILoggingServiceDummyImpl.fail = false;
            }
        }
    }
}