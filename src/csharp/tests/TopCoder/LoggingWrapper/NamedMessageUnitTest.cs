/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for NamedMessage.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class NamedMessageUnitTest
    {
        /// <summary>
        /// Represents a string as text for testing.
        /// </summary>
        private const string TEXT = "a text";

        /// <summary>
        /// Represents a string as name for testing.
        /// </summary>
        private const string NAME = "a name";

        /// <summary>
        /// Represents a level for testing.
        /// </summary>
        private const Level LEVEL = Level.INFO;

        /// <summary>
        /// Represents a list of strings for testing.
        /// </summary>
        private IList<string> list;

        /// <summary>
        /// Creates instances for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            list = new List<string>();
            list.Add("a string");
            list.Add("another string");
        }

        /// <summary>
        /// Tests constructor for accuracy. Ensures all the fields are set properly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            NamedMessage msg = new NamedMessage(TEXT, NAME, list, LEVEL);

            Assert.IsNotNull(msg, "fail to create instance");
            Assert.AreEqual(TEXT, msg.Text, "incorrect text");
            Assert.AreEqual(NAME, msg.Name, "incorrect name");
            Assert.AreEqual(LEVEL, msg.DefaultLevel, "incorrect default level");

            Assert.AreNotSame(list, msg.ParameterNames, "shallow copy expected");
            Assert.AreEqual(list.Count, msg.ParameterNames.Count, "incorrect number of parameters");
            foreach (string s in list)
            {
                Assert.IsTrue(msg.ParameterNames.Contains(s), "incorrect parameter");
            }
        }

        /// <summary>
        /// Tests constructor with null text.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullText()
        {
            new NamedMessage(null, NAME, list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with empty text.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithEmptyText()
        {
            new NamedMessage("", NAME, list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with null name.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullName()
        {
            new NamedMessage(TEXT, null, list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with empty name.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithEmptyName()
        {
            new NamedMessage(TEXT, "", list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with null list.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestCtorWithNullList()
        {
            new NamedMessage(TEXT, NAME, null, LEVEL);
        }

        /// <summary>
        /// Tests constructor with null in list.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithNullInList()
        {
            list.Add(null);

            new NamedMessage(TEXT, NAME, list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with empty string in list.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestCtorWithEmptyInList()
        {
            list.Add("");

            new NamedMessage(TEXT, NAME, list, LEVEL);
        }

        /// <summary>
        /// Tests constructor with invalid level.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCtorWithInvalidLevel()
        {
            new NamedMessage(TEXT, NAME, list, (Level) 123);
        }
    }
}