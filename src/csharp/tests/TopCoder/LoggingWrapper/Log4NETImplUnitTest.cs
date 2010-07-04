/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using log4net;
using NUnit.Framework;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for Log4NETImpl.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class Log4NETImplUnitTest
    {
        /// <summary>
        /// A string as name of log for testing.
        /// </summary>
        private const string LOGNAME = "Log4NETLogger";

        /// <summary>
        /// A string as name of application log for testing.
        /// </summary>
        private const string APPLICATION = "Application";

        /// <summary>
        /// A string as source of log for testing.
        /// </summary>
        private const string SOURCE = "SourceLog4NET";

        /// <summary>
        /// A string as machine name of log for testing.
        /// </summary>
        private const string MACHINE_NAME = ".";

        /// <summary>
        /// A level as level of logger for testing.
        /// </summary>
        private const Level DEFAULT_LEVEL = Level.INFO;

        /// <summary>
        /// A level as level of message for testing.
        /// </summary>
        private const Level LEVEL = Level.WARN;

        /// <summary>
        /// A string as config file for testing.
        /// </summary>
        private const string CONFIG_FILE = @"..\..\test_files\log4net.config";

        /// <summary>
        /// A string as default configuration file for testing.
        /// </summary>
        private const string DEFAULT_FILE = "log4net.config";

        /// <summary>
        /// A string as name of named message for testing.
        /// </summary>
        private const string NAMEDMESSAGE = "namedMessage1";

        /// <summary>
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "{1} greater than {0}";

        /// <summary>
        /// An array of strings as names of parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETER_NAMES = new string[] {"name1", "name2"};

        /// <summary>
        /// An array of strings as parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETERS = new string[] {"value1", "value2"};

        /// <summary>
        /// A string as formatted message for testing.
        /// </summary>
        private static readonly string FORMATTED = "value2 greater than value1";

        /// <summary>
        /// A string as formatted message for testing.
        /// </summary>
        private static readonly string NAMED_FORMATTED =
            "value2 greater than value1 means value1 less than value2";

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// An instance of NamedMessage for testing.
        /// </summary>
        private NamedMessage namedMsg;

        /// <summary>
        /// An instance of Log4NETImpl for testing.
        /// </summary>
        private Log4NETImpl logger;

        /// <summary>
        /// <para>
        /// Creates custom event log for testing.
        /// </para>
        /// </summary>
        [TestFixtureSetUp]
        protected void RunBeforeAllTests()
        {
            TestHelper.CreateLogs(SOURCE);
        }

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            namedMsg = new NamedMessage(MESSAGE, NAMEDMESSAGE, new List<string>(PARAMETER_NAMES),
                Level.ERROR);

            config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", LOGNAME);
            config.SetSimpleAttribute("default_level", DEFAULT_LEVEL.ToString());
            config.SetSimpleAttribute("config_file", CONFIG_FILE);

            IConfiguration msgsConfig = new DefaultConfiguration("NamedMessages");

            IConfiguration msgConfig = new DefaultConfiguration(NAMEDMESSAGE);
            msgConfig.SetSimpleAttribute("text", MESSAGE);
            msgConfig.SetSimpleAttribute("default_level", Level.DEBUG);
            msgConfig.SetAttribute("parameters", new object[] {PARAMETER_NAMES[0], PARAMETER_NAMES[1]});
            msgsConfig.AddChild(msgConfig);

            config.AddChild(msgsConfig);

            logger = new Log4NETImpl(config);
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

            // clear log entries in Application log
            EventLog log = new EventLog(APPLICATION, MACHINE_NAME, SOURCE);
            log.Clear();
        }

        /// <summary>
        /// Tests Log4NETImpl(IConfiguration) for accuracy. Ensures all the fields are set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            Assert.IsNotNull(logger, "fail to create instance");

            Assert.AreEqual(LOGNAME, logger.Logname, "incorrect log name");
            Assert.AreEqual(DEFAULT_LEVEL, logger.DefaultLevel, "incorrect default level");
            Assert.AreEqual(1, logger.NamedMessages.Count, "incorrect number of named messages");
            Assert.AreEqual(NAMEDMESSAGE, logger.NamedMessages[NAMEDMESSAGE].Name,
                "incorrect name of named message");
            Assert.AreEqual(MESSAGE, logger.NamedMessages[NAMEDMESSAGE].Text,
                "incorrect text of named message");
            Assert.AreEqual(PARAMETER_NAMES.Length, logger.NamedMessages[NAMEDMESSAGE].ParameterNames.Count,
                "incorrect number of parameters of named message");
            foreach (string name in PARAMETER_NAMES)
            {
                Assert.IsTrue(logger.NamedMessages[NAMEDMESSAGE].ParameterNames.Contains(name),
                    "incorrect parameter of named message");
            }
        }

        /// <summary>
        /// Tests Log4NETImpl(IConfiguration) with null text.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNull()
        {
            new Log4NETImpl((IConfiguration) null);
        }

        /// <summary>
        /// Tests constructor with "config_file" attribute missing in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithUrlAbsent()
        {
            config.RemoveAttribute("config_file");

            new Log4NETImpl(config);
        }

        /// <summary>
        /// Tests constructor with invalid value of "config_file" attribute in configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidUrl()
        {
            config.SetSimpleAttribute("config_file", "invalid");

            new Log4NETImpl(config);
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
            // remember the id of last message
            EventLog log = new EventLog(APPLICATION, MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            logger.Log(LEVEL, MESSAGE, PARAMETERS);

            // check the message logged by service
            string msg = log.Entries[index].Message;

            Assert.IsTrue(msg.IndexOf(FORMATTED) >= 0, "incorrect message");
            Assert.IsTrue(msg.IndexOf("warn", StringComparison.OrdinalIgnoreCase) >= 0,
                "incorrect level");
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures all levels are mapped correctly.
        /// </summary>
        [Test]
        public void TestLogLevelMapping()
        {
            // remember the id of last message
            EventLog log = new EventLog("", MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            IDictionary<Level, string> dict =
                new Dictionary<Level, string>();
            dict[Level.FATAL] = "fatal";
            dict[Level.ERROR] = "error";
            dict[Level.FAILUREAUDIT] = "debug";
            dict[Level.SUCCESSAUDIT] = "debug";
            dict[Level.WARN] = "warn";
            dict[Level.INFO] = "info";
            dict[Level.DEBUG] = "debug";

            foreach (Level level in dict.Keys)
            {
                logger.Log(level, MESSAGE, PARAMETERS);

                // check the type of message logged by service
                string msg = log.Entries[index++].Message;

                Assert.IsTrue(msg.IndexOf(dict[level], StringComparison.OrdinalIgnoreCase) >= 0,
                    "incorrect level");
            }
        }

        /// <summary>
        /// Tests Log for accuracy. Ensures message won't be logged for OFF level.
        /// </summary>
        [Test]
        public void TestLogWithOFF()
        {
            // remember the id of last message
            EventLog log = new EventLog("", MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            string msg = "never be seen";

            logger.Log(Level.OFF, msg, PARAMETERS);

            Assert.AreEqual(index, log.Entries.Count, "message should not be logged");
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
        /// Tests LogNamedMessage for accuracy. Ensures message are logged properly
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy()
        {
            // remember the id of last message
            EventLog log = new EventLog("", MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            namedMsg = new NamedMessage(MESSAGE, NAMEDMESSAGE, new List<string>(PARAMETER_NAMES),
                Level.ERROR);

            logger.LogNamedMessage(LEVEL, namedMsg, PARAMETERS);

            // check the message logged by service
            string msg = log.Entries[index].Message;

            Assert.IsTrue(msg.IndexOf(NAMED_FORMATTED) >= 0, "incorrect message");
            Assert.IsTrue(msg.IndexOf("warn", StringComparison.OrdinalIgnoreCase) >= 0,
                "incorrect level");

            // check the parameters are removed
            for (int i = 0; i < PARAMETER_NAMES.Length; i++)
            {
                Assert.IsNull(LogicalThreadContext.Properties[PARAMETER_NAMES[i]],
                    "paramters in logical thread context should be removed");
            }
        }

        /// <summary>
        /// Tests LogNamedMessage for accuracy. Ensures all levels are mapped correctly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageLevelMapping()
        {
            // remember the id of last message
            EventLog log = new EventLog("", MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            IDictionary<Level, string> dict =
                new Dictionary<Level, string>();
            dict[Level.FATAL] = "fatal";
            dict[Level.ERROR] = "error";
            dict[Level.FAILUREAUDIT] = "debug";
            dict[Level.SUCCESSAUDIT] = "debug";
            dict[Level.WARN] = "warn";
            dict[Level.INFO] = "info";
            dict[Level.DEBUG] = "debug";

            foreach (Level level in dict.Keys)
            {
                logger.LogNamedMessage(level, namedMsg, PARAMETERS);

                // check the message logged by service
                string msg = log.Entries[index++].Message;

                Assert.IsTrue(msg.IndexOf(dict[level], StringComparison.OrdinalIgnoreCase) >= 0,
                    "incorrect level");
            }
        }

        /// <summary>
        /// Tests LogNamedMessage for accuracy. Ensures message won't be logged for OFF level.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithOFF()
        {
            // remember the id of last message
            EventLog log = new EventLog("", MACHINE_NAME, SOURCE);
            int index = log.Entries.Count;

            logger.LogNamedMessage(Level.OFF, namedMsg, PARAMETERS);

            Assert.AreEqual(index, log.Entries.Count, "message should not be logged");
        }

        /// <summary>
        /// Tests LogNamedMessage with null named message.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogNamedMessageWithNullNamedMessage()
        {
            logger.LogNamedMessage(LEVEL, (NamedMessage) null, PARAMETERS);
        }

        /// <summary>
        /// Tests LogNamedMessage with null parameters.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogNamedMessageWithNullParameters()
        {
            logger.LogNamedMessage(LEVEL, namedMsg, null);
        }

        /// <summary>
        /// Tests LogNamedMessage with number of parameters and number of parameter in message not matching.
        /// MessageFormattingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (MessageFormattingException))]
        public void TestLogNamedMessageWithParametersNotMatch()
        {
            logger.LogNamedMessage(LEVEL, namedMsg, new object[] {"param1", "param2", "param3"});
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "config_file" attributes exist and the file exist.
        /// Ensures the attribute and file won't change.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationBothExist()
        {
            // record the time
            long time = DateTime.Now.Ticks;

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            // check attribute
            Assert.AreEqual(CONFIG_FILE, config.GetSimpleAttribute("config_file"),
                "incorrect value of attribute");

            // check file is not modified
            Assert.IsTrue(new FileInfo(CONFIG_FILE).LastWriteTime.Ticks < time,
                "the file should not be modified");
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "config_file" attributes not exist but the file exist.
        /// Ensures the attribute is set and file won't change.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationFileExist()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // create default file
            File.Copy(CONFIG_FILE, DEFAULT_FILE);

            // record the time
            long time = DateTime.Now.Ticks;

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            // check attribute
            Assert.AreEqual(DEFAULT_FILE, config.GetSimpleAttribute("config_file"),
                "incorrect value of attribute");

            // check file is not modified
            Assert.IsTrue(new FileInfo(DEFAULT_FILE).LastWriteTime.Ticks < time,
                "the file should not be modified");

            // remove default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with "config_file" attributes exist but the file not exist.
        /// Ensures the attribute won't change and file is created.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationAttributeExist()
        {
            string tmpFile = CONFIG_FILE + "~";

            // move the file
            if (File.Exists(tmpFile))
            {
                File.Delete(tmpFile);
            }
            File.Move(CONFIG_FILE, tmpFile);

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            // check attribute
            Assert.AreEqual(CONFIG_FILE, config.GetSimpleAttribute("config_file"),
                "incorrect value of attribute");

            // check file is created
            Assert.IsTrue(File.Exists(CONFIG_FILE), "the file should be created");

            // move back the file
            File.Delete(CONFIG_FILE);
            File.Move(tmpFile, CONFIG_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with neither "config_file" attributes or the file exist.
        /// Ensures the attribute is set and file is created properly.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationNeitherExist()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            // check attribute
            Assert.AreEqual(DEFAULT_FILE, config.GetSimpleAttribute("config_file"),
                "incorrect value of attribute");

            // check file is not modified
            Assert.IsTrue(File.Exists(DEFAULT_FILE), "the file should be created");

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with null configuration.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestInitializeZeroConfigurationWithNullConfiguration()
        {
            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, null);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with error occurs when accessing the configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestInitializeZeroConfigurationWithConfigurationError()
        {
            // change attribute to multiply values
            config.SetAttribute("config_file", new object[] {CONFIG_FILE, "log4net.config"});

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Test option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithTest()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Test, config);

            // check the file
            CheckDefaultFile("log4net.Appender.FileAppender", new string[] {@"..\..\test_files\log.txt"},
                null);

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Component option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithComponent()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Component, config);

            // check the file
            CheckDefaultFile("log4net.Appender.FileAppender", new string[] {@"log.txt"}, "INFO");

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Certification option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithCertification()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Certification, config);

            // check the file
            CheckDefaultFile("log4net.Appender.RollingFileAppender",
                new string[] {@"logs\log", "Date", "-1"}, null);

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Client Debug option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithClientDebug()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.ClientDebug, config);

            // check the file
            CheckDefaultFile("log4net.Appender.RollingFileAppender",
                new string[] {@"logs\log", "Composite", "30"}, null);

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Client Stress option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithClientStress()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.ClientStress, config);

            // check the file
            CheckDefaultFile("log4net.Appender.RollingFileAppender",
                new string[] {@"logs\log", "Composite", "30"}, "ERROR");

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Tests InitializeZeroConfiguration with Release option.
        /// Ensures configuration file is created with proper content.
        /// </summary>
        [Test]
        public void TestInitializeZeroConfigurationWithRelease()
        {
            if (File.Exists(DEFAULT_FILE))
            {
                File.Delete(DEFAULT_FILE);
            }

            // remove attribute
            config.RemoveAttribute("config_file");

            Log4NETImpl.InitializeZeroConfiguration(ZeroConfigurationOption.Release, config);

            // check the file
            CheckDefaultFile("log4net.Appender.RollingFileAppender",
                new string[] {@"logs\log", "Composite", "30"}, "WARN");

            // remove the default file
            File.Delete(DEFAULT_FILE);
        }

        /// <summary>
        /// Checks whether the context of the default configuration file is correct.
        /// </summary>
        /// <param name="appenderType">The expected type of appender.</param>
        /// <param name="appendArgs">The expected arguments of appender.</param>
        /// <param name="level">The expected level of root.</param>
        private static void CheckDefaultFile(string appenderType, string[] appendArgs, string level)
        {
            // load default configuration file
            XmlDocument doc = new XmlDocument();
            doc.Load(DEFAULT_FILE);

            // check root element of document
            XmlNode root = doc.FirstChild;
            Assert.AreEqual("log4net", root.Name, "incorrect root element");

            // check appender element
            string appenderName = CheckAppenderElement(root.FirstChild, appenderType, appendArgs);

            // check root element
            CheckRootElement(root.FirstChild.NextSibling, appenderName, level);
        }

        /// <summary>
        /// Checks whether the context of the appender elemnt in configuration file is correct.
        /// </summary>
        /// <param name="appender">The appender element to check.</param>
        /// <param name="appenderType">The expected type of appender.</param>
        /// <param name="appendArgs">The expected arguments of appender.</param>
        /// <returns>the name of appender.</returns>
        private static string CheckAppenderElement(XmlNode appender, string appenderType,
            string[] appendArgs)
        {
            Assert.AreEqual("appender", appender.Name, "incorrect appender element");

            // check type
            Assert.AreEqual(appenderType, appender.Attributes["type"].Value, "incorrect appender type");

            // check file element
            XmlNode next = appender.FirstChild;
            CheckElement(next, "file", appendArgs[0]);

            if (appendArgs.Length > 1)
            {
                // check datePattern element
                next = next.NextSibling;
                CheckElement(next, "datePattern", @".yyyy-MM-dd.\tx\t");

                // check staticLogFileName element
                next = next.NextSibling;
                CheckElement(next, "staticLogFileName", "false");

                // check rollingStyle element
                next = next.NextSibling;
                CheckElement(next, "rollingStyle", appendArgs[1]);

                // check maxSizeRollBackups element
                next = next.NextSibling;
                CheckElement(next, "maxSizeRollBackups", appendArgs[2]);
            }

            // check layout element
            next = next.NextSibling;
            Assert.AreEqual("layout", next.Name, "incorrect layout element");
            Assert.AreEqual("log4net.Layout.PatternLayout", next.Attributes["type"].Value,
                "incorrect layout element");
            CheckElement(next.FirstChild, "conversionPattern",
                @"%date [%thread] %-5level %logger [%ndc] - %message%newline");

            // return name of appender
            return appender.Attributes["name"].Value;
        }

        /// <summary>
        /// Checks whether the context of the appender elemnt in configuration file is correct.
        /// </summary>
        /// <param name="root">The root element.</param>
        /// <param name="appenderName">The expected name of appender.</param>
        /// <param name="level">The expected level.</param>
        private static void CheckRootElement(XmlNode root, string appenderName, string level)
        {
            // check appender reference
            XmlNode appenderRef = root.FirstChild;
            Assert.AreEqual("appender-ref", appenderRef.Name, "incorrect appender reference");
            Assert.AreEqual(appenderName, appenderRef.Attributes["ref"].Value,
                "incorrect appender reference");

            // check level
            if (level != null)
            {
                CheckElement(appenderRef.NextSibling, "level", level);
            }
            else
            {
                Assert.AreEqual(1, root.ChildNodes.Count, "no level should be provided");
            }
        }

        /// <summary>
        /// Checks whether the given node is of given name and has an attribute named "value" of given value.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <param name="name">The expected name of node.</param>
        /// <param name="value">The expected value of attribute "value".</param>
        private static void CheckElement(XmlNode node, string name, string value)
        {
            Assert.AreEqual(name, node.Name, "incorrect " + name + " element");
            Assert.AreEqual(value, node.Attributes["value"].Value,
                "incorrect value of " + name + " element");
        }

        /// <summary>
        /// Tests the accuracy of definition Log4NETImpl.Log4NETLevel.
        /// </summary>
        [Test]
        public void TestLog4NETLevel()
        {
            Type type = typeof (Log4NETImpl.Log4NETLevel);

            Assert.IsTrue(type.IsEnum, "Log4NETLevel should be enum.");

            Assert.AreEqual(6, Enum.GetNames(type).Length, "incorrect const values in Log4NETLevel.");

            Assert.IsTrue(Enum.IsDefined(type, "OFF"), "Log4NETLevel.OFF should be defined.");
            Assert.IsTrue(Enum.IsDefined(type, "DEBUG"), "Log4NETLevel.DEBUG should be defined.");
            Assert.IsTrue(Enum.IsDefined(type, "INFO"), "Log4NETLevel.INFO should be defined.");
            Assert.IsTrue(Enum.IsDefined(type, "WARN"), "Log4NETLevel.WARN should be defined.");
            Assert.IsTrue(Enum.IsDefined(type, "ERROR"), "Log4NETLevel.ERROR should be defined.");
            Assert.IsTrue(Enum.IsDefined(type, "FATAL"), "Log4NETLevel.FATAL should be defined.");
        }
    }
}