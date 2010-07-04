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
    /// Unit test for Helper.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class HelperUnitTest
    {
        /// <summary>
        /// Represents a string as name for testing.
        /// </summary>
        private const string NAME = "name";

        /// <summary>
        /// Represents IConfiguration instance for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// Creates configuration for testing.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            config = new DefaultConfiguration("default");
        }

        /// <summary>
        /// Tests method ValidateNotNull with null parameter.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestValidateNotNullWithNull()
        {
            Helper.ValidateNotNull(null, NAME);
        }

        /// <summary>
        /// Tests method ValidateNotNull with valid parameter.
        /// </summary>
        [Test]
        public void TestValidateNotNullWithNotNull()
        {
            Helper.ValidateNotNull(new object(), NAME);
        }

        /// <summary>
        /// Tests method ValidateNotEmptyString with empty string parameter.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestValidateNotEmptyStringWithEmpty()
        {
            Helper.ValidateNotEmptyString("", NAME);
        }

        /// <summary>
        /// Tests method ValidateNotEmptyString with valid parameter.
        /// </summary>
        [Test]
        public void TestValidateNotEmptyStringWithNotEmpty()
        {
            Helper.ValidateNotEmptyString("a", NAME);
        }

        /// <summary>
        /// Tests method ValidateNotEmptyString with null parameter. Null should be considered valid.
        /// </summary>
        [Test]
        public void TestValidateNotEmptyStringWithNull()
        {
            Helper.ValidateNotEmptyString(null, NAME);
        }

        /// <summary>
        /// Tests method ValidateNotNullNotEmptyString with null parameter.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestValidateNotNullNotEmptyStringWithNull()
        {
            Helper.ValidateNotNullNotEmptyString(null, NAME);
        }

        /// <summary>
        /// Tests method ValidateNotNullNotEmptyString with empty string parameter.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestValidateNotNullNotEmptyStringWithEmpty()
        {
            Helper.ValidateNotNullNotEmptyString("", NAME);
        }

        /// <summary>
        /// Tests method ValidateNotNullNotEmptyString with valid parameter.
        /// </summary>
        [Test]
        public void TestValidateNotNullNotEmptyStringWithValid()
        {
            Helper.ValidateNotNullNotEmptyString("a", NAME);
        }

        /// <summary>
        /// Tests method GetStringAttribute, when required attiribute absent.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetStringAttributeWithRequiredAbsent()
        {
            Helper.GetStringAttribute(config, NAME, true);
        }

        /// <summary>
        /// Tests method GetStringAttribute, when attiribute is empty string.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetStringAttributeWithEmpty()
        {
            config.SetSimpleAttribute(NAME, "");
            Helper.GetStringAttribute(config, NAME, false);
        }

        /// <summary>
        /// Tests method GetStringAttribute, when attiribute is of multiply values.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetStringAttributeWithMultiply()
        {
            config.SetAttribute(NAME, new object[] {"name1", "name2"});
            Helper.GetStringAttribute(config, NAME, false);
        }

        /// <summary>
        /// Tests method GetStringAttribute for accuracy with required attiribute.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetStringAttributeWithRequiredExist()
        {
            string expected = "value of required";
            config.SetSimpleAttribute(NAME, expected);

            Assert.AreEqual(expected, Helper.GetStringAttribute(config, NAME, true),
                "The returned value is wrong.");
        }

        /// <summary>
        /// Tests method GetStringAttribute for accuracy with optional attiribute.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetStringAttributeWithOptionalExist()
        {
            string expected = "value of required";
            config.SetSimpleAttribute(NAME, expected);

            Assert.AreEqual(expected, Helper.GetStringAttribute(config, NAME, false),
                "The returned value is wrong.");
        }

        /// <summary>
        /// Tests method GetStringAttribute for accuracy with optional attiribute absent.
        /// The returned value should be null.
        /// </summary>
        [Test]
        public void TestGetStringAttributeWithOptionalAbsent()
        {
            Assert.IsNull(Helper.GetStringAttribute(config, NAME, false),
                "The returned value should be a null reference.");
        }

        /// <summary>
        /// Tests GetBooleanAttribute method, when attiribute is null.
        /// Method should returns a default value.
        /// </summary>
        [Test]
        public void TestGetBooleanAttributeWithDefault()
        {
            Assert.IsTrue(Helper.GetBooleanAttribute(config, NAME, true),
                "The returned value is not default.");
            Assert.IsFalse(Helper.GetBooleanAttribute(config, NAME, false),
                "The returned value is not default.");
        }

        /// <summary>
        /// Tests GetBooleanAttribute method for accuracy.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetBooleanAttributeWithValid()
        {
            string expected = "tRUe";
            config.SetSimpleAttribute(NAME, expected);

            Assert.IsTrue(Helper.GetBooleanAttribute(config, NAME, false),
                "The returned value is wrong.");
        }

        /// <summary>
        /// Tests GetBooleanAttribute method with boolean type value.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetBooleanAttributeWithBoolean()
        {
            config.SetSimpleAttribute(NAME, true);

            Assert.IsTrue(Helper.GetBooleanAttribute(config, NAME, true),
                "The returned value is wrong.");
        }

        /// <summary>
        /// Tests GetBooleanAttribute method when attiribute is not boolean.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetBooleanAttributeWithMalformedAttribute()
        {
            config.SetSimpleAttribute(NAME, "not true or false");
            Helper.GetBooleanAttribute(config, NAME, false);
        }

        /// <summary>
        /// Tests method GetBooleanAttribute, when attiribute is of multiply values.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetBooleanAttributeWithMultiply()
        {
            config.SetAttribute(NAME, new object[] {"true", "false"});
            Helper.GetBooleanAttribute(config, NAME, false);
        }

        /// <summary>
        /// Tests GetLevelAttribute method, when attiribute is null.
        /// Method should returns a default value.
        /// </summary>
        [Test]
        public void TestGetLevelAttributeWithDefault()
        {
            Assert.AreEqual(Level.OFF, Helper.GetLevelAttribute(config, NAME, Level.OFF),
                "The returned value is not default.");
            Assert.AreEqual(Level.DEBUG, Helper.GetLevelAttribute(config, NAME, Level.DEBUG),
                "The returned value is not default.");
        }

        /// <summary>
        /// Tests GetLevelAttribute method for accuracy.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetLevelAttributeWithValid()
        {
            string expected = "FaTaL";
            config.SetSimpleAttribute(NAME, expected);

            Assert.AreEqual(Level.FATAL, Helper.GetLevelAttribute(config, NAME, Level.OFF),
                "The returned value is wrong.");
        }

        /// <summary>
        /// Tests GetLevelAttribute method when attiribute is not level.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetLevelAttributeWithMalformedAttribute()
        {
            config.SetSimpleAttribute(NAME, "not level");
            Helper.GetLevelAttribute(config, NAME, Level.OFF);
        }

        /// <summary>
        /// Tests method GetLevelAttribute, when attiribute is of multiply values.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetLevelAttributeWithMultiply()
        {
            config.SetAttribute(NAME, new object[] {"WARN", "ERROR"});
            Helper.GetLevelAttribute(config, NAME, Level.OFF);
        }

        /// <summary>
        /// Tests method GetStringListAttribute, when required attiribute absent.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetStringListAttributeWithRequiredAbsent()
        {
            Helper.GetStringListAttribute(config, NAME, true);
        }

        /// <summary>
        /// Tests method GetStringListAttribute, when attiribute contains empty string.
        /// ConfigException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ConfigException))]
        public void TestGetStringListAttributeContainEmpty()
        {
            config.SetAttribute(NAME, new string[] {"a", ""});
            Helper.GetStringListAttribute(config, NAME, true);
        }

        /// <summary>
        /// Tests method GetStringListAttribute for accuracy with required attiribute.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetStringListAttributeWithRequiredExist()
        {
            string[] expected = new string[] {"value of required", "annother value"};
            config.SetAttribute(NAME, expected);

            IList<string> values = Helper.GetStringListAttribute(config, NAME, true);
            Assert.AreEqual(expected[0], values[0], "The returned value is wrong.");
            Assert.AreEqual(expected[1], values[1], "The returned value is wrong.");
        }

        /// <summary>
        /// Tests method GetStringListAttribute for accuracy with optional attiribute.
        /// The established value should be equal to received.
        /// </summary>
        [Test]
        public void TestGetStringListAttributeWithOptionalExist()
        {
            string[] expected = new string[] {"value of required", "annother value"};
            config.SetAttribute(NAME, expected);

            IList<string> values = Helper.GetStringListAttribute(config, NAME, false);
            Assert.AreEqual(expected[0], values[0], "The returned value is wrong.");
            Assert.AreEqual(expected[1], values[1], "The returned value is wrong.");
        }

        /// <summary>
        /// Tests method GetStringListAttribute for accuracy with optional attiribute absent.
        /// The returned value should be empty list.
        /// </summary>
        [Test]
        public void TestGetStringListAttributeWithOptionalAbsent()
        {
            Assert.AreEqual(0, Helper.GetStringListAttribute(config, NAME, false).Count,
                "The returned value should be empty list.");
        }

        /// <summary>
        /// Tests method ValidateLoggerNotNull with null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void TestValidateLoggerNotNullWithNull()
        {
            Helper.ValidateLoggerNotNull(null, NAME);
        }

        /// <summary>
        /// Tests method ValidateLoggerNotNull with valid parameter.
        /// The logger should be returned.
        /// </summary>
        [Test]
        public void TestValidateLoggerNotNullWithValid()
        {
            Logger logger = new SimpleLogger(NAME);
            Assert.AreSame(logger, Helper.ValidateLoggerNotNull(logger, NAME), "incorrect logger returned");
        }
    }
}