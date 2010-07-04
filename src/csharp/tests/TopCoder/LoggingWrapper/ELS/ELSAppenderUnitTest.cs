/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using log4net.Core;
using NUnit.Framework;
using TopCoder.Configuration;
using TopCoder.Configuration.File;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// Unit test for ELSAppender.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class ELSAppenderUnitTest
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
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "message1";

        /// <summary>
        /// Instance of LoggingEvent for testing.
        /// </summary>
        private LoggingEvent loggingEvent;

        /// <summary>
        /// A log4net.Core.Level for testing.
        /// </summary>
        private log4net.Core.Level level = log4net.Core.Level.Info;

        /// <summary>
        /// Instance of ELSAppender for testing.
        /// </summary>
        private ELSAppender appender;

        /// <summary>
        /// <para>Configuration used to create object.</para>
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// Sets up test environment.
        /// </summary>
        ///
        [SetUp]
        protected void SetUp()
        {
            LoggingEventData data = new LoggingEventData();
            data.LoggerName = LOGGER_NAME;
            data.Message = MESSAGE;
            data.Level = level;
            loggingEvent = new LoggingEvent(data);

            config = (new ConfigurationManager()).
                GetConfiguration("TopCoder.LoggingWrapper.ELS.ELSAppender");
            appender = new ELSAppender(config);
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
        /// Tests constructor for accuracy.
        /// </summary>
        [Test]
        public void TestCtorAccuracy2()
        {
            Assert.IsNotNull(appender, "fail to create instance");
        }

        /// <summary>
        /// Tests constructor with null argument.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCtorWithNull()
        {
            new ELSAppender(null);
        }

        /// <summary>
        /// Tests Append for accuracy. Ensures the message are sent to service properly.
        /// </summary>
        [Test]
        public void TestAppendAccuracy()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            appender = new MockELSAppender(config);

            (appender as MockELSAppender).Append(loggingEvent);

            // check the message logged by service
            string msg = TestHelper.GetLogEntry(SOURCE, index).Message;

            string[] param = msg.Split(new char[] { ':' });

            Assert.AreEqual(LOGGER_NAME, param[0], "incorrect originator");
            Assert.AreEqual("null", param[1], "incorrect context");
            Assert.AreEqual(MESSAGE, param[2], "incorrect message");
        }

        /// <summary>
        /// Tests Append for accuracy. Ensures the levels are mapped properly.
        /// </summary>
        [Test]
        public void TestAppendLevelMapping()
        {
            // remember the id for the next log message
            int index = TestHelper.GetLogsCount(SOURCE);

            appender = new MockELSAppender(config);

            IDictionary<log4net.Core.Level, Level> dict = new Dictionary<log4net.Core.Level, Level>();
            dict[log4net.Core.Level.Alert] = Level.ERROR;
            dict[log4net.Core.Level.All] = Level.DEBUG;
            dict[log4net.Core.Level.Critical] = Level.ERROR;
            dict[log4net.Core.Level.Debug] = Level.DEBUG;
            dict[log4net.Core.Level.Emergency] = Level.ERROR;
            dict[log4net.Core.Level.Error] = Level.ERROR;
            dict[log4net.Core.Level.Fatal] = Level.FATAL;
            dict[log4net.Core.Level.Fine] = Level.DEBUG;
            dict[log4net.Core.Level.Finer] = Level.DEBUG;
            dict[log4net.Core.Level.Finest] = Level.DEBUG;
            dict[log4net.Core.Level.Info] = Level.INFO;
            dict[log4net.Core.Level.Notice] = Level.INFO;
            dict[log4net.Core.Level.Off] = Level.OFF;
            dict[log4net.Core.Level.Severe] = Level.ERROR;
            dict[log4net.Core.Level.Trace] = Level.DEBUG;
            dict[log4net.Core.Level.Verbose] = Level.DEBUG;
            dict[log4net.Core.Level.Warn] = Level.WARN;

            foreach (log4net.Core.Level key in dict.Keys)
            {
                // change level of event
                LoggingEventData data = new LoggingEventData();
                data.LoggerName = LOGGER_NAME;
                data.Message = MESSAGE;
                data.Level = key;
                loggingEvent = new LoggingEvent(data);

                (appender as MockELSAppender).Append(loggingEvent);

                // check the message logged by service
                string msg = TestHelper.GetLogEntry(SOURCE, index++).Message;

                string[] param = msg.Split(new char[] { ':' });

                Assert.AreEqual(dict[key].ToString(), param[3], "incorrect level");
            }
        }

        /// <summary>
        /// Tests Append with null argument.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAppendWithNull()
        {
            appender = new MockELSAppender(config);
            (appender as MockELSAppender).Append(null);
        }

        /// <summary>
        /// Tests Append with error occurs when forwarding to service.
        /// LoggingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(LoggingException))]
        public void TestAppendWithServiceError()
        {
            // Specify for service to return error
            ILoggingServiceDummyImpl.fail = true;
            try
            {
                appender = new MockELSAppender(config);
                (appender as MockELSAppender).Append(loggingEvent);
            }
            finally
            {
                ILoggingServiceDummyImpl.fail = false;
            }
        }

        /// <summary>
        /// Tests OnClose for accuracy. Ensures the service is disposed.
        /// </summary>
        [Test]
        public void TestOnCloseAccuracy()
        {
            appender = new MockELSAppender(config);
            (appender as MockELSAppender).OnClose();

            try
            {
                (appender as MockELSAppender).Append(loggingEvent);
                Assert.Fail("service should be disposed");
            }
            catch (LoggingException)
            {
                // expect
            }

        }

        /// <summary>
        /// A class extends ELSAppender for testing the protect method.
        /// </summary>
        ///
        /// <author>TCSDEVELOPER</author>
        /// <version>3.0</version>
        /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
        [CoverageExclude]
        class MockELSAppender : ELSAppender
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            public MockELSAppender(IConfiguration config)
                : base(config)
            {
            }

            /// <summary>
            /// <para>
            /// Appends the logging event to the backend logging service.
            /// Delegate to the same method in super class.
            /// </para>
            /// </summary>
            /// <param name="loggingEvent">The logging event to append.</param>
            /// <exception cref="ArgumentNullException">If loggingEvent is null</exception>
            /// <exception cref="LoggingException">If the call to the logging service fails.</exception>
            public new void Append(LoggingEvent loggingEvent)
            {
                base.Append(loggingEvent);
            }

            /// <summary>
            /// <para>
            /// Releases the resource used by this appender.
            /// </para>
            /// </summary>
            public new void OnClose()
            {
                base.OnClose();
            }
        }
    }
}
