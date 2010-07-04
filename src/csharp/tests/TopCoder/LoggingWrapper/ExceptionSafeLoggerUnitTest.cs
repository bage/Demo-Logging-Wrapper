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
    /// Unit test for ExceptionSafeLogger.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class ExceptionSafeLoggerUnitTest
    {
        /// <summary>
        /// A string as name of logger for ExceptionSafeLogger.
        /// </summary>
        private const string LOGGER_NAME = "UnderlyingLogger";

        /// <summary>
        /// A string as name of exception log for testing.
        /// </summary>
        private const string EXCEPTION_LOGGER_NAME = "ExceptionLogger";

        /// <summary>
        /// A level as default level of logger for testing.
        /// </summary>
        private const Level DEFAULT_LEVEL = Level.INFO;

        /// <summary>
        /// A level as level of logger for testing.
        /// </summary>
        private const Level LEVEL = Level.WARN;

        /// <summary>
        /// A string as name of named message for testing.
        /// </summary>
        private const string NAMEDMESSAGE = "namedMessage1";

        /// <summary>
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "{0} message {1}";

        /// <summary>
        /// An array of strings as names of parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETER_NAMES = new string[] {"paramName11", "paramName22"};

        /// <summary>
        /// An array of strings as parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETERS = new string[] {"param11", "param22"};

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// A logger used as underlying logger for testing.
        /// </summary>
        private SimpleLogger simpleLogger;

        /// <summary>
        /// A logger used as exception logger for testing.
        /// </summary>
        private AnotherSimpleLogger anotherLogger;

        /// <summary>
        /// An instance of NamedMessage for testing.
        /// </summary>
        private NamedMessage namedMsg;

        /// <summary>
        /// An instance of ExceptionSafeLogger for testing.
        /// </summary>
        private ExceptionSafeLogger logger;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            namedMsg = new NamedMessage(MESSAGE, NAMEDMESSAGE, new List<string>(PARAMETER_NAMES),
                Level.ERROR);

            config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("logger_name", LOGGER_NAME);
            config.SetSimpleAttribute("default_level", DEFAULT_LEVEL.ToString());

            IConfiguration msgsConfig = new DefaultConfiguration("NamedMessages");

            IConfiguration msgConfig = new DefaultConfiguration(NAMEDMESSAGE);
            msgConfig.SetSimpleAttribute("text", MESSAGE);
            msgConfig.SetSimpleAttribute("default_level", Level.DEBUG.ToString());
            msgConfig.SetAttribute("parameters", new object[] {PARAMETER_NAMES[0], PARAMETER_NAMES[1]});
            msgsConfig.AddChild(msgConfig);

            config.AddChild(msgsConfig);

            simpleLogger = new SimpleLogger(config);

            anotherLogger = new AnotherSimpleLogger(EXCEPTION_LOGGER_NAME);

            logger = new ExceptionSafeLogger(simpleLogger, anotherLogger);
        }

        /// <summary>
        /// Tests constructor for accuracy. Ensures all fields are set properly. Also tests the
        /// properties.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            Assert.AreSame(LOGGER_NAME, logger.Logname, "incorrect log name");
            Assert.AreEqual(DEFAULT_LEVEL, logger.DefaultLevel, "incorrect default level");
            Assert.AreEqual(simpleLogger.NamedMessages.Count, logger.NamedMessages.Count,
                "incorrect named messages");
            foreach (string key in simpleLogger.NamedMessages.Keys)
            {
                Assert.AreEqual(simpleLogger.NamedMessages[key], logger.NamedMessages[key],
                    "incorrect named message");
            }
        }

        /// <summary>
        /// Tests constructor with null underlying logger.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullUnderlyingLogger()
        {
            new ExceptionSafeLogger(null, simpleLogger);
        }

        /// <summary>
        /// Tests constructor with null exception logger.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullExceptionLogger()
        {
            new ExceptionSafeLogger(simpleLogger, null);
        }

        /// <summary>
        /// Tests Dispose for accuracy, Ensure the underlying logger and exception logger are disposed.
        /// </summary>
        [Test]
        public void TestDisposeAccuracy()
        {
            logger.Dispose();

            Assert.IsTrue(SimpleLogger.LastDispose, "underlying logger should be disposed");

            Assert.IsTrue(AnotherSimpleLogger.LastDispose, "exception logger should be disposed");
        }

        /// <summary>
        /// Tests Dispose with exception thrown by underlying exception.
        /// Eusures the exception will be ignored.
        /// </summary>
        [Test]
        public void TestDisposeWitheUnderlyingLoggerError()
        {
            SimpleLogger.Ex = new Exception();

            logger.Dispose();
        }

        /// <summary>
        /// Tests Dispose with exception thrown by exception exception.
        /// Eusures the exception will be ignored.
        /// </summary>
        [Test]
        public void TestDisposeWitheExceptionLoggerError()
        {
            AnotherSimpleLogger.Ex = new Exception();

            logger.Dispose();
        }

        /// <summary>
        /// Tests IsLevelEnabled for accuracy. Ensures behavior of the method is the same as that of the
        /// underlying logger.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledAccuracy()
        {
            SimpleLogger.IsEnable = true;

            Assert.IsTrue(logger.IsLevelEnabled(Level.INFO), "incorrect value of IsLevelEnabled");

            SimpleLogger.IsEnable = false;

            Assert.IsFalse(logger.IsLevelEnabled(Level.INFO), "incorrect value of IsLevelEnabled");
        }

        /// <summary>
        /// Tests IsLevelEnabled with exception thrown by underlying logger. Ensures exception will
        /// be logged by exception logger.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledWithUnderlyingLoggerError()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.IsLevelEnabled(Level.INFO);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests IsLevelEnabled with exception thrown by underlying logger. Ensures if error occurs
        /// when exception logger logging the message, the exception will be ignored.
        /// </summary>
        [Test]
        public void TestIsLevelEnabledWithExceptionLoggerError()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.IsLevelEnabled(Level.INFO);
        }

        /// <summary>
        /// Tests Log(Level, string, params object[]) for accuracy. Ensures behavior of the method is the
        /// same as that of the underlying logger.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogAccuracy1()
        {
            logger.Log(LEVEL, MESSAGE, PARAMETERS);

            // check the arugment
            Assert.AreEqual(LEVEL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
            object[] param = SimpleLogger.LastParam;
            Assert.AreEqual(PARAMETERS.Length, param.Length, "incorrect parameters");
            for (int i = 0; i < PARAMETERS.Length; i++)
            {
                Assert.AreEqual(PARAMETERS[i], param[i], "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests Log(Level, string, params object[]) with exception thrown by underlying logger. Ensures
        /// exception will be logged by exception logger.
        /// </summary>
        [Test]
        public void TestLogWithUnderlyingLoggerError1()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.Log(LEVEL, MESSAGE, PARAMETERS);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests Log(Level, string, params object[]) with exception thrown by underlying logger. Ensures
        /// if error occurs when exception logger logging the message, the exception will be ignored.
        /// </summary>
        [Test]
        public void TestLogWithExceptionLoggerError1()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.Log(LEVEL, MESSAGE, PARAMETERS);
        }

        /// <summary>
        /// Tests Log(string, params object[]) for accuracy. Ensures behavior of the method is the
        /// same as that of the underlying logger.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogAccuracy2()
        {
            logger.Log(MESSAGE, PARAMETERS);

            // check the arugment
            Assert.AreEqual(DEFAULT_LEVEL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
            object[] param = SimpleLogger.LastParam;
            Assert.AreEqual(PARAMETERS.Length, param.Length, "incorrect parameters");
            for (int i = 0; i < PARAMETERS.Length; i++)
            {
                Assert.AreEqual(PARAMETERS[i], param[i], "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests Log(string, params object[]) with exception thrown by underlying logger. Ensures
        /// exception will be logged by exception logger.
        /// </summary>
        [Test]
        public void TestLogWithUnderlyingLoggerError2()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.Log(MESSAGE, PARAMETERS);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests Log(string, params object[]) with exception thrown by underlying logger. Ensures
        /// if error occurs when exception logger logging the message, the exception will be ignored.
        /// </summary>
        [Test]
        public void TestLogWithExceptionLoggerError2()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.Log(MESSAGE, PARAMETERS);
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) for accuracy. Ensures behavior of
        /// the method is the same as that of the underlying logger.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy1()
        {
            logger.LogNamedMessage(LEVEL, namedMsg, PARAMETERS);

            // check the arugment
            Assert.AreEqual(LEVEL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
            object[] param = SimpleLogger.LastParam;
            Assert.AreEqual(PARAMETERS.Length, param.Length, "incorrect parameters");
            for (int i = 0; i < PARAMETERS.Length; i++)
            {
                Assert.AreEqual(PARAMETERS[i], param[i], "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) with exception thrown by underlying
        /// logger. Ensures exception will be logged by exception logger.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithUnderlyingLoggerError1()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.LogNamedMessage(LEVEL, namedMsg, PARAMETERS);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) with exception thrown by underlying
        /// logger. Ensures if error occurs when exception logger logging the message, the exception will
        /// be ignored.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithExceptionLoggerError1()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.LogNamedMessage(LEVEL, namedMsg, PARAMETERS);
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) for accuracy. Ensures behavior of
        /// the method is the same as that of the underlying logger.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy2()
        {
            logger.LogNamedMessage(LEVEL, NAMEDMESSAGE, PARAMETERS);

            // check the arugment
            Assert.AreEqual(LEVEL, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
            object[] param = SimpleLogger.LastParam;
            Assert.AreEqual(PARAMETERS.Length, param.Length, "incorrect parameters");
            for (int i = 0; i < PARAMETERS.Length; i++)
            {
                Assert.AreEqual(PARAMETERS[i], param[i], "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with exception thrown by underlying
        /// logger. Ensures exception will be logged by exception logger.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithUnderlyingLoggerError2()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.LogNamedMessage(LEVEL, NAMEDMESSAGE, PARAMETERS);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with exception thrown by underlying
        /// logger. Ensures if error occurs when exception logger logging the message, the exception will
        /// be ignored.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithExceptionLoggerError2()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.LogNamedMessage(LEVEL, NAMEDMESSAGE, PARAMETERS);
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) for accuracy. Ensures behavior of
        /// the method is the same as that of the underlying logger.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageAccuracy3()
        {
            logger.LogNamedMessage(NAMEDMESSAGE, PARAMETERS);

            // check the arugment
            Assert.AreEqual(Level.DEBUG, SimpleLogger.LastLevel, "incorrect level");
            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
            object[] param = SimpleLogger.LastParam;
            Assert.AreEqual(PARAMETERS.Length, param.Length, "incorrect parameters");
            for (int i = 0; i < PARAMETERS.Length; i++)
            {
                Assert.AreEqual(PARAMETERS[i], param[i], "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) with exception thrown by underlying
        /// logger. Ensures exception will be logged by exception logger.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithUnderlyingLoggerError3()
        {
            Exception ex = new Exception(MESSAGE);
            SimpleLogger.Ex = ex;

            logger.LogNamedMessage(NAMEDMESSAGE, PARAMETERS);

            // check the message logged
            Assert.AreEqual(ex.ToString(), AnotherSimpleLogger.LastMessage, "incorrect exception logged");
        }

        /// <summary>
        /// Tests LogNamedMessage(string, params object[]) with exception thrown by underlying
        /// logger. Ensures if error occurs when exception logger logging the message, the exception will
        /// be ignored.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithExceptionLoggerError3()
        {
            SimpleLogger.Ex = new Exception();

            AnotherSimpleLogger.Ex = new Exception();

            logger.LogNamedMessage(NAMEDMESSAGE, PARAMETERS);
        }
    }
}
