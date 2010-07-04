/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for Logger. SimpleLogger inherited from Logger is used for testing.
    /// Note that only the new or changed functions in 3.0 are tested.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LoggerUnitTest
    {
        /// <summary>
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "a message";

        /// <summary>
        /// A string as name of log for testing.
        /// </summary>
        private const string LOGNAME = "name of log";

        /// <summary>
        /// A level as default level of logger for testing.
        /// </summary>
        private const Level LEVEL = Level.INFO;

        /// <summary>
        /// A string as name of message for testing.
        /// </summary>
        private const string NAME1 = "message name1";

        /// <summary>
        /// A string as name of message for testing.
        /// </summary>
        private const string NAME2 = "message name2";

        /// <summary>
        /// A dictionary of named messages for testing.
        /// </summary>
        private IDictionary<string, NamedMessage> msgs;

        /// <summary>
        /// An instance of NamedMessage for testing.
        /// </summary>
        private NamedMessage namedMsg;

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// An instance of SimpleLogger for testing.
        /// </summary>
        private SimpleLogger logger;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            IList<string> param = new List<string>();
            param.Add("param1");
            param.Add("param2");

            namedMsg = new NamedMessage("text1", NAME1, param, Level.ERROR);

            msgs = new Dictionary<string, NamedMessage>();
            msgs.Add(NAME1, namedMsg);
            msgs.Add(NAME2, new NamedMessage("text2", NAME2, new List<string>(), Level.WARN));

            config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", LOGNAME);
            config.SetSimpleAttribute("default_level", LEVEL.ToString());

            IConfiguration msgsConfig = new DefaultConfiguration("NamedMessages");

            IConfiguration msgConfig = new DefaultConfiguration(NAME1);
            msgConfig.SetSimpleAttribute("text", "text1");
            msgConfig.SetSimpleAttribute("default_level", Level.ERROR);
            msgConfig.SetAttribute("parameters", new object[] {"param1", "param2"});
            msgsConfig.AddChild(msgConfig);

            msgConfig = new DefaultConfiguration(NAME2);
            msgConfig.SetSimpleAttribute("text", "text2");
            msgConfig.SetSimpleAttribute("default_level", Level.WARN);
            msgsConfig.AddChild(msgConfig);

            config.AddChild(msgsConfig);

            logger = new SimpleLogger(LOGNAME, LEVEL, msgs);
        }

        /// <summary>
        /// Tests Logger(string, Level, IDictionary) for accuracy. Ensures field namedMessages is set
        /// properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy1()
        {
            Assert.AreNotSame(msgs, logger.NamedMessages, "shallow copy expected");
            Assert.AreEqual(msgs.Count, logger.NamedMessages.Count, "incorrect number of named messages");
            foreach (string s in msgs.Keys)
            {
                Assert.AreEqual(msgs[s], logger.NamedMessages[s], "incorrect named message");
            }
        }

        /// <summary>
        /// Tests Logger(string, Level) for accuracy. Ensures field namedMessages is set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy2()
        {
            logger = new SimpleLogger(LOGNAME, LEVEL);

            Assert.AreEqual(0, logger.NamedMessages.Count, "incorrect number of named messages");
        }

        /// <summary>
        /// Tests Logger(string) for accuracy. Ensures field namedMessages is set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy3()
        {
            logger = new SimpleLogger(LOGNAME);

            Assert.AreEqual(0, logger.NamedMessages.Count, "incorrect number of named messages");
        }

        /// <summary>
        /// Tests Logger(string, Level, IDictionary) with null namedMessages.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullDictionary()
        {
            new SimpleLogger(LOGNAME, LEVEL, null);
        }

        /// <summary>
        /// Tests Logger(string, Level, IDictionary) with empty key in namedMessages.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithEmptyKeyInDictionary()
        {
            msgs.Add("", new NamedMessage("text", "name", new List<string>(), Level.INFO));

            new SimpleLogger(LOGNAME, LEVEL, msgs);
        }

        /// <summary>
        /// Tests Logger(string, Level, IDictionary) with null value in namedMessages.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithNullValueInDictionary()
        {
            msgs.Add("name3", null);

            new SimpleLogger(LOGNAME, LEVEL, msgs);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) for accuracy. Ensures all fields are set properly.
        /// </summary>
        [Test]
        public void TestCtorConfigAccuracy()
        {
            logger = new SimpleLogger(config);

            Assert.AreEqual(LOGNAME, logger.Logname, "incrrect log name");
            Assert.AreEqual(LEVEL, logger.DefaultLevel, "incorrect default level");
            Assert.AreEqual(msgs.Count, logger.NamedMessages.Count, "incorrect number of named messages");
            foreach (string s in new string[] {NAME1, NAME2})
            {
                Assert.AreEqual(msgs[s].Name, logger.NamedMessages[s].Name,
                    "incorrect name of named message");
                Assert.AreEqual(msgs[s].Text, logger.NamedMessages[s].Text,
                    "incorrect text of named message");
                Assert.AreEqual(msgs[s].ParameterNames.Count, logger.NamedMessages[s].ParameterNames.Count,
                    "incorrect number of parameters of named message");
                foreach (string name in msgs[s].ParameterNames)
                {
                    Assert.IsTrue(logger.NamedMessages[s].ParameterNames.Contains(name),
                        "incorrect parameter of named message");
                }
            }
        }

        /// <summary>
        /// Tests Logger(IConfiguration) without "default_level" in configuration. Ensures the default
        /// level is Level.DEBUG.
        /// </summary>
        [Test]
        public void TestCtorConfigNoDefaultLevel()
        {
            config.RemoveAttribute("default_level");

            logger = new SimpleLogger(config);

            Assert.AreEqual(Level.DEBUG, logger.DefaultLevel, "incorrect default level");
        }

        /// <summary>
        /// Tests Logger(IConfiguration) without "NamedMessages" in configuration. Ensures the
        /// namedMessages dictionary is empty.
        /// </summary>
        [Test]
        public void TestCtorConfigNoNamedMessages()
        {
            config.RemoveChild("NamedMessages");

            logger = new SimpleLogger(config);

            Assert.AreEqual(0, logger.NamedMessages.Count, "incorrect number of named messages");
        }

        /// <summary>
        /// Tests Logger(IConfiguration) without "default_level" in configuration of the first named
        /// message. Ensures the default level is the default level of logger.
        /// </summary>
        [Test]
        public void TestCtorConfigNoDefaultLevelInNamedMessage()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).RemoveAttribute("default_level");

            logger = new SimpleLogger(config);

            Assert.AreEqual(Level.INFO, logger.NamedMessages[NAME1].DefaultLevel,
                "incorrect default level of named message");
        }

        /// <summary>
        /// Tests Logger(IConfiguration) without "parameters" in configuration of the first named
        /// message. Ensures the parameters of named message is empty.
        /// </summary>
        [Test]
        public void TestCtorConfigNoParametersInNamedMessage()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).RemoveAttribute("parameters");

            logger = new SimpleLogger(config);

            Assert.AreEqual(0, logger.NamedMessages[NAME1].ParameterNames.Count,
                "incorrect number of parmeters of named message");
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with null configuration.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullConfiguration()
        {
            new SimpleLogger((IConfiguration) null);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with "logger_name" attribute absent.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithoutLoggerName()
        {
            config.RemoveAttribute("logger_name");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with invalid value of "logger_name" attribute.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidLoggerName()
        {
            config.SetSimpleAttribute("logger_name", "");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with multiply values of "logger_name" attribute.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithMutiplyLoggerName()
        {
            config.SetAttribute("logger_name", new object[] {"name1", "name2"});

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with invalid value of "default_level" attribute.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidDefaultLevel()
        {
            config.SetSimpleAttribute("default_level", "not level");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with multiply values of "default_level" attribute.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithMultiplyDefaultLevel()
        {
            config.SetAttribute("default_level", new object[] {"FATAL", "INFO"});

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) without "text" attribute in named message configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithoutTextInNamedMessage()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).RemoveAttribute("text");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with invalid value of "text" attribute in named message
        /// configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidTextInNamedMessage()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).SetSimpleAttribute("text", "");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with mutiply values of "text" attribute in named message
        /// configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithMutiplyTextInNamedMessage()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).SetAttribute("text",
                new object[] {"text1", "text2"});

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with invalid value of "default_level" attribute in named message
        /// configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidDefaultLevelInNamedMessge()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).SetSimpleAttribute("default_level",
                "not level");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with multiply values of "default_level" attribute in named message
        /// configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithMultiplyDefaultLevelInNamedMessge()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).SetAttribute("default_level",
                new object[] {"INFO", "FATAL"});

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Logger(IConfiguration) with invalid value of "parameters" attribute in named message
        /// configuration.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestCtorWithInvalidParametersInNamedMessge()
        {
            config.GetChild("NamedMessages").GetChild(NAME1).SetSimpleAttribute("parameters", "");

            new SimpleLogger(config);
        }

        /// <summary>
        /// Tests Log(string, params object[]) for accuracy .
        /// Ensures the default level is used.
        /// </summary>
        [Test]
        public void TestLogAccuracy()
        {
            logger.Log(MESSAGE);

            Assert.AreEqual(LEVEL, SimpleLogger.LastLevel, "default level should be used");
        }

        /// <summary>
        /// Tests Log(string, params object[]) with ArgumentNullException.
        /// Ensures the exception will be thrown.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogWithArgumentNullException()
        {
            // preset the exception
            SimpleLogger.Ex = new ArgumentNullException();

            logger.Log(MESSAGE);
        }

        /// <summary>
        /// Tests Log(string, params object[]) with MessageFormattingException.
        /// Ensures the exception will be thrown.
        /// </summary>
        [Test, ExpectedException(typeof (MessageFormattingException))]
        public void TestLogWithMessageFormattingException()
        {
            // preset the exception
            SimpleLogger.Ex = new MessageFormattingException();

            logger.Log(MESSAGE);
        }

        /// <summary>
        /// Tests Log(string, params object[]) with LoggingException.
        /// Ensures the exception will be thrown.
        /// </summary>
        [Test, ExpectedException(typeof (LoggingException))]
        public void TestLogWithLoggingException()
        {
            // preset the exception
            SimpleLogger.Ex = new LoggingException();

            logger.Log(MESSAGE);
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) for accuracy.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy1()
        {
            object param = new object();

            logger.LogNamedMessage(Level.FATAL, namedMsg, param);

            // check the argument
            Assert.AreEqual(Level.FATAL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreSame(namedMsg.Text, SimpleLogger.LastMessage, "incorrect message");
            object[] lastParam = SimpleLogger.LastParam;
            Assert.AreEqual(1, lastParam.Length, "incorrect number of parameters");
            Assert.AreSame(param, lastParam[0], "incorrect parameter");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) with exception (such as
        /// MessageFormattingException. Ensures the exception will be thrown.
        /// </summary>
        [Test, ExpectedException(typeof (MessageFormattingException))]
        public void TestLogNamedMessageWithException()
        {
            // preset the exception
            SimpleLogger.Ex = new MessageFormattingException();

            logger.LogNamedMessage(Level.FATAL, namedMsg);
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) for accuracy.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy2()
        {
            object param = new object();

            logger.LogNamedMessage(Level.FATAL, NAME1, param);

            // check the argument
            Assert.AreEqual(Level.FATAL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreSame(namedMsg.Text, SimpleLogger.LastMessage, "incorrect message");
            object[] lastParam = SimpleLogger.LastParam;
            Assert.AreEqual(1, lastParam.Length, "incorrect number of parameters");
            Assert.AreSame(param, lastParam[0], "incorrect parameter");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with null string.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogNamedMessageWithNullString1()
        {
            logger.LogNamedMessage(Level.FATAL, (string) null);
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestLogNamedMessageWithEmptyString1()
        {
            logger.LogNamedMessage(Level.FATAL, "");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with message name no found.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestLogNamedMessageWithNameNoFound1()
        {
            logger.LogNamedMessage(Level.FATAL, "nonexist");
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) for accuracy. Ensures the default level of
        /// named message is used.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy3()
        {
            object param = new object();

            logger.LogNamedMessage(NAME1, param);

            // check the argument
            Assert.AreEqual(Level.ERROR, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreSame(namedMsg.Text, SimpleLogger.LastMessage, "incorrect message");
            object[] lastParam = SimpleLogger.LastParam;
            Assert.AreEqual(1, lastParam.Length, "incorrect number of parameters");
            Assert.AreSame(param, lastParam[0], "incorrect parameter");
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) with null string.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestLogNamedMessageWithNullString2()
        {
            logger.LogNamedMessage(null);
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) with empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestLogNamedMessageWithEmptyString2()
        {
            logger.LogNamedMessage("");
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) with message name no found.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestLogNamedMessageWithNameNoFound2()
        {
            logger.LogNamedMessage("nonexist");
        }
    }
}