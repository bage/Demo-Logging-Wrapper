/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Text;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper.EntLib
{
    /// <summary>
    /// Unit test for LoggingWrapperTraceListener.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LoggingWrapperTraceListenerUnitTest
    {
        /// <summary>
        /// A string as log name for testing.
        /// </summary>
        private const string LOGGER_NAME = "LoggingWrapperTraceListener";

        /// <summary>
        /// A string as message for testing.
        /// </summary>
        private const string MESSAGE = "message3";

        /// <summary>
        /// A string as category for testing.
        /// </summary>
        private const string CATEGORY = "category3";

        /// <summary>
        /// An logger as underlying logger for testing.
        /// </summary>
        private SimpleLogger logger;

        /// <summary>
        /// An instance of LoggingWrapperTraceListener for testing.
        /// </summary>
        private LoggingWrapperTraceListener listener;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            logger = new SimpleLogger(LOGGER_NAME);

            listener = new LoggingWrapperTraceListener(logger);
        }

        /// <summary>
        /// Tests constructor for accuracy.
        /// </summary>
        [Test]
        public void TestCtorAccuracy1()
        {
            Assert.IsNotNull(listener, "fail to create instance");
        }

        /// <summary>
        /// Tests constructor for accuracy.
        /// </summary>
        [Test]
        public void TestCtorAccuracy2()
        {
            Assert.IsNotNull(new LoggingWrapperTraceListener(), "fail to create instance");
        }

        /// <summary>
        /// Tests constructor with null logger.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullLogger()
        {
            new LoggingWrapperTraceListener(null);
        }

        /// <summary>
        /// Tests Close for accuracy. Ensures the underlying logger is disposed.
        /// </summary>
        [Test]
        public void TestCloseAccuracy()
        {
            Assert.IsFalse(SimpleLogger.LastDispose, "underlying logger should be available");

            listener.Close();

            Assert.IsTrue(SimpleLogger.LastDispose, "underlying logger should be disposed");
        }

        /// <summary>
        /// Tests Write(object) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteObjectAccuracy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.Write(sb);

            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests Write(object) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteObjectWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.Write(sb);
        }

        /// <summary>
        /// Tests Write(string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteStringAccuracy()
        {
            listener.Write(MESSAGE);

            Assert.AreEqual(MESSAGE, SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests Write(string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteStringWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            listener.Write(MESSAGE);
        }

        /// <summary>
        /// Tests Write(object, string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteObjectCategoryAccuracy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.Write(sb, CATEGORY);

            Assert.AreEqual("[" + CATEGORY + "]" + MESSAGE, SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests Write(object, string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteObjectCategoryWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.Write(sb, CATEGORY);
        }

        /// <summary>
        /// Tests Write(string, string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteStringCategoryAccuracy()
        {
            listener.Write(MESSAGE, CATEGORY);

            Assert.AreEqual("[" + CATEGORY + "]" + MESSAGE, SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests Write(string, string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteStringCategoryWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            listener.Write(MESSAGE, CATEGORY);
        }

        /// <summary>
        /// Tests WriteLine(object) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteLineObjectAccuracy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.WriteLine(sb);

            Assert.AreEqual(MESSAGE + "\n", SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests WriteLine(object) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteLineObjectWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.WriteLine(sb);
        }

        /// <summary>
        /// Tests WriteLine(string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteLineStringAccuracy()
        {
            listener.WriteLine(MESSAGE);

            Assert.AreEqual(MESSAGE + "\n", SimpleLogger.LastMessage, "incorrect message");
        }

        /// <summary>
        /// Tests WriteLine(string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteLineStringWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            listener.WriteLine(MESSAGE);
        }

        /// <summary>
        /// Tests WriteLine(object, string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteLineObjectCategoryAccuracy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.WriteLine(sb, CATEGORY);

            Assert.AreEqual("[" + CATEGORY + "]" + MESSAGE + "\n", SimpleLogger.LastMessage,
                "incorrect message");
        }

        /// <summary>
        /// Tests WriteLine(object, string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteLineObjectCategoryWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            StringBuilder sb = new StringBuilder();
            sb.Append(MESSAGE);

            listener.WriteLine(sb, CATEGORY);
        }

        /// <summary>
        /// Tests WriteLine(string, string) for accuracy. Ensures the message is written properly.
        /// </summary>
        [Test]
        public void TestWriteLineStringCategoryAccuracy()
        {
            listener.WriteLine(MESSAGE, CATEGORY);

            Assert.AreEqual("[" + CATEGORY + "]" + MESSAGE + "\n", SimpleLogger.LastMessage,
                "incorrect message");
        }

        /// <summary>
        /// Tests WriteLine(string, string) with error occurs when logging.
        /// Eusures exception will be ignored.
        /// </summary>
        [Test]
        public void TestWriteLineStringCategoryWithErrorIgnore()
        {
            SimpleLogger.Ex = new Exception();

            listener.WriteLine(MESSAGE, CATEGORY);
        }
    }
}