/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for LoggingException.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LoggingExceptionUnitTest
    {
        /// <summary>
        /// <para>
        /// The error message used for testing.
        /// </para>
        /// </summary>
        private static readonly string MESSAGE = "Error message.";

        /// <summary>
        /// <para>
        /// The inner cause used for testing.
        /// </para>
        /// </summary>
        private static readonly Exception CAUSE = new Exception("Inner exception error message.");

        /// <summary>
        /// <para>
        /// An instance of LoggingException used for testing.
        /// </para>
        /// </summary>
        private LoggingException exception;

        /// <summary>
        /// <para>
        /// Accuracy test for the zero parameter constructor.
        /// </para>
        /// </summary>
        [Test]
        public void Test0ParamCtor()
        {
            exception = new LoggingException();
            Assert.IsNotNull(exception, "Fail to create instance.");
        }

        /// <summary>
        /// <para>
        /// Accuracy test for the one parameter constructor. The message should be set properly.
        /// </para>
        /// </summary>
        [Test]
        public void Test1ParamCtor()
        {
            exception = new LoggingException(MESSAGE);
            Assert.AreEqual(MESSAGE, exception.Message, "The message was not set properly.");
        }

        /// <summary>
        /// <para>
        /// Accuracy test for the two parameter constructor. The message and inner cause should be set properly.
        /// </para>
        /// </summary>
        [Test]
        public void Test2ParamCtor()
        {
            exception = new LoggingException(MESSAGE, CAUSE);
            Assert.AreEqual(MESSAGE, exception.Message, "The message was not set properly.");
            Assert.AreSame(CAUSE, exception.InnerException, "The inner cause was not set properly.");
        }

        /// <summary>
        /// <para>
        /// Ensures that the serialization constructor was given and set the message and inner cause properly.
        /// </para>
        /// </summary>
        [Test]
        public void TestSerialization()
        {
            LoggingException serialized;
            exception = new LoggingException(MESSAGE, CAUSE);

            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // serialize the exception
                formatter.Serialize(stream, exception);
                stream.Position = 0;
                // deserialize
                serialized = (LoggingException) formatter.Deserialize(stream);
            }

            Assert.AreEqual(MESSAGE, serialized.Message, "The message was not set properly.");
            // Just check the message because it is impossible to compare two different Exception
            // instances for equality.
            Assert.AreEqual(CAUSE.Message, serialized.InnerException.Message,
                "The inner cause was not set properly.");
        }
    }
}