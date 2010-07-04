/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for LevelFilteredLogger.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LevelFilteredLoggerUnitTest
    {
        /// <summary>
        /// A string as name of logger for testing.
        /// </summary>
        private const string LOGGER_NAME = "LevelFilteredLogger";

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
        private const string MESSAGE = "{0} {1} message";

        /// <summary>
        /// An array of strings as names of parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETER_NAMES = new string[] {"paramName11", "paramName22"};

        /// <summary>
        /// An array of strings as parameters for testing.
        /// </summary>
        private static readonly string[] PARAMETERS = new string[] {"param11", "param22"};

        /// <summary>
        /// A logger used as underlying logger for testing.
        /// </summary>
        private SimpleLogger simpleLogger;

        /// <summary>
        /// An instance of NamedMessage for testing.
        /// </summary>
        private NamedMessage namedMsg;

        /// <summary>
        /// A list of levels to be filtered for testing.
        /// </summary>
        private IList<Level> filteredLevels;

        /// <summary>
        /// An instance of LevelFilteredLogger for testing.
        /// </summary>
        private LevelFilteredLogger logger;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            namedMsg = new NamedMessage(MESSAGE, NAMEDMESSAGE, new List<string>(PARAMETER_NAMES),
                Level.ERROR);

            IDictionary<string, NamedMessage> msgs = new Dictionary<string, NamedMessage>();
            msgs.Add(NAMEDMESSAGE, new NamedMessage(MESSAGE, NAMEDMESSAGE,
                new List<string>(PARAMETER_NAMES), Level.ERROR));

            simpleLogger = new SimpleLogger(LOGGER_NAME, DEFAULT_LEVEL, msgs);

            filteredLevels = new List<Level>();
            filteredLevels.Add(Level.FATAL);

            logger = new LevelFilteredLogger(simpleLogger, filteredLevels);
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
        /// Tests constructor with null logger.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullLogger()
        {
            new LevelFilteredLogger(null, filteredLevels);
        }

        /// <summary>
        /// Tests constructor with null filteredLevels.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullFilteredLevels()
        {
            new LevelFilteredLogger(simpleLogger, null);
        }

        /// <summary>
        /// Tests Dispose for accuracy, Ensure the underlying logger is disposed.
        /// </summary>
        [Test]
        public void TestDisposeAccuracy()
        {
            logger.Dispose();

            Assert.IsTrue(SimpleLogger.LastDispose, "underlying logger should be disposed");
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
        /// Tests Log(Level, string, params object[]) with level outside the filteredLevels list.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogWithoutFiltered()
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
        /// Tests Log(Level, string, params object[]) with level inside the filteredLevels list.
        /// Ensures the message won't be logged.
        /// </summary>
        [Test]
        public void TestLogWithFiltered()
        {
            logger.Log(filteredLevels[0], MESSAGE, PARAMETERS);

            Assert.AreEqual(Level.OFF, SimpleLogger.LastLevel, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastMessage, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastParam, "no message should be logged");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) with level outside the
        /// filteredLevels list.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithoutFiltered()
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
        /// Tests LogNamedMessage(Level, NamedMessage, params object[]) with level inside the
        /// filteredLevels list.
        /// Ensures the message won't be logged.
        /// </summary>
        [Test]
        public void TestLogNamedMessageWithFiltered()
        {
            logger.LogNamedMessage(filteredLevels[0], namedMsg, PARAMETERS);

            Assert.AreEqual(Level.OFF, SimpleLogger.LastLevel, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastMessage, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastParam, "no message should be logged");
        }

        /// <summary>
        /// Tests LogNamedMessage(Level, string, params object[]) with level outside the
        /// filteredLevels list.
        /// Ensures the message will be logged properly.
        /// </summary>
        [Test]
        public void TestLogNamedMessageIdentifierWithoutFiltered()
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
        /// Tests LogNamedMessage(Level, string, params object[]) with level inside the
        /// filteredLevels list.
        /// Ensures the message won't be logged.
        /// </summary>
        [Test]
        public void TestLogNamedMessageIdentifierWithFiltered()
        {
            logger.LogNamedMessage(filteredLevels[0], NAMEDMESSAGE, PARAMETERS);

            Assert.AreEqual(Level.OFF, SimpleLogger.LastLevel, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastMessage, "no message should be logged");
            Assert.IsNull(SimpleLogger.LastParam, "no message should be logged");
        }
    }
}
