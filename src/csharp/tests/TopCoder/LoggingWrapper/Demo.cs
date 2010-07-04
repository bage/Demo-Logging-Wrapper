/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System.Diagnostics;
using System.IO;
using log4net.Config;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using NUnit.Framework;
using TopCoder.Configuration;
using CM = System.Configuration.ConfigurationManager;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// Demo of Logging Wrapper 3.0.
    /// </para>
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class Demo
    {
        /// <summary>
        /// <para>
        /// Disposes WCF objects and clears the custom event logs after all tests.
        /// </para>
        /// </summary>
        [TestFixtureTearDown]
        protected void RunAfterAllTests()
        {
            // TestHelper.StopWCFService();

            TestHelper.ClearLogs();

            // clear log entries in Application log
            EventLog log = new EventLog("Application", ".", "SourceLog4NET");
            log.Clear();
        }

        /// <summary>
        /// <para>
        /// Demo 1 : Create Logger from the LogManage.
        /// </para>
        /// </summary>
        [Test]
        public void TestDemoLogManager()
        {
            // create the logger from the specified namespaces, one for each of the
            // configured namespaces.
            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.Log4NETImpl";
            // logger1 is a Log4NETImpl instance
            Logger logger1 = LogManager.CreateLogger();

            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.DiagnosticImpl";
            // logger2 is a DiagnosticImpl instance
            Logger logger2 = LogManager.CreateLogger();

            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                = "TopCoder.LoggingWrapper.ELSImpl";
            // logger3 is an ELSImpl instance
            Logger logger3 = LogManager.CreateLogger();

            CM.AppSettings[TestHelper.DEFAULT_LOGGER_CLASS_APP_SETTING_NAME]
                   = "TopCoder.LoggingWrapper.EnterpriseLibraryLogger";
            // logger4 is an EnterpriseLibraryLogger instance
            Logger logger4 = LogManager.CreateLogger();

            // create the logger from the Default Namespace
            // If the namespace is not specified, the namespace defaults to
            // TopCoder.LoggingWrapper.LogManager.
            Logger logger = LogManager.CreateLogger();

            // It is also possible to create a logger from a non-file based
            // configuration
            IConfiguration config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_class", "TopCoder.LoggingWrapper.DiagnosticImpl");
            config.SetSimpleAttribute("logger_name", "LogTest");
            config.SetSimpleAttribute("default_level", "INFO");
            config.SetSimpleAttribute("source", "source of demo");
            logger = LogManager.CreateLogger(config);
        }

        /// <summary>
        /// <para>
        /// Demo 2 : Log message with the Logger directly.
        /// </para>
        /// </summary>
        [Test]
        public void TestDemoLogger()
        {
            // create the logger using default configuration
            Logger logger = LogManager.CreateLogger();

            // log simple message with default level
            logger.Log("Hello World");

            // log formatted message with default level
            logger.Log("Hello {0}", "World");

            // log formatted message with specified logging level
            logger.Log(Level.WARN, "Hello {0}", "World");

            // This call will not log anything at all because the logger has been
            // configured to filter out the ERROR level.
            logger.Log(Level.ERROR, "error message");
            // On the other hand, a call at a non-filtered level will log the message
            logger.Log(Level.INFO, "info message");


            // If we has exception propagation turned on, we¡¯ll get a MessageFormattingException
            // because it has not enough params are specified.
            logger = LogManager.CreateLogger();
            try
            {
                logger.Log("String to be formatted {0}, {1}, {2}", "only 1 arg");
            }
            catch (MessageFormattingException e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// <para>
        /// Demo 3 : Use zero-configuration option.
        /// </para>
        /// </summary>
        [Test]
        public void TestDemoZeroConfiguration()
        {
            // The zero-configuration can be as simple as specifying the logging
            // class and the zero-configuration type that should be used.

            // The configuration varies for different loggers. For DiagnosticImpl logger,
            // The source will be set to "TopCoder Logger" if it doesn't exist before.
            Logger logger = LogManager.CreateLogger();
        }

        /// <summary>
        /// <para>
        /// Demo 4 : Use ELS custom appender for log4net.
        /// </para>
        /// </summary>
        [Test]
        public void TestDemoELSAppender()
        {
            // The appender should be configured in the log4net config file like so
            // <appender name="ELSAppender" type="TopCoder.LoggingWrapper.ELS.ELSAppender" >
            //   <param name="url" value="http://localhost:1234/ELS" />
            // </appender>
            // <logger name="Some Logger">
            //   <appender-ref ref="ELSAppender" />
            // </logger>

            // load the config file
            XmlConfigurator.Configure(new FileInfo(@"..\..\test_files\log4net.config"));

            // get the logger
            log4net.ILog log = log4net.LogManager.GetLogger("Some Logger");

            // log to ELS
            log.Debug("Message to go to ELS");
        }

        /// <summary>
        /// <para>
        /// Demo 4 : Use ELS custom appender for log4net.
        /// </para>
        /// </summary>
        [Test]
        public void TestDemoLoggingWrapperTraceListener()
        {
            LogEntry entry = new LogEntry();

            // set the entry
            entry.Message = "This is my message";
            entry.Severity = TraceEventType.Error;
            entry.Priority = 5;

            // write the entry
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
        }
    }
}
