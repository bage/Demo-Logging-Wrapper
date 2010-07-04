/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for ZeroConfigurationOption.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class ZeroConfigurationOptionUnitTest
    {
        /// <summary>
        /// Tests the accuracy of definition ZeroConfigurationOption.
        /// </summary>
        [Test]
        public void TestZeroConfigurationOption()
        {
            Type type = typeof (ZeroConfigurationOption);

            Assert.IsTrue(type.IsEnum, "ZeroConfigurationOption should be enum.");

            Assert.AreEqual(6, Enum.GetNames(type).Length,
                "incorrect const values in ZeroConfigurationOption.");

            Assert.IsTrue(Enum.IsDefined(type, "Test"),
                "ZeroConfigurationOption.Test should be defined.");

            Assert.IsTrue(Enum.IsDefined(type, "Component"),
                "ZeroConfigurationOption.Component should be defined.");

            Assert.IsTrue(Enum.IsDefined(type, "Certification"),
                "ZeroConfigurationOption.Certification should be defined.");

            Assert.IsTrue(Enum.IsDefined(type, "ClientDebug"),
                "ZeroConfigurationOption.ClientDebug should be defined.");

            Assert.IsTrue(Enum.IsDefined(type, "ClientStress"),
                "ZeroConfigurationOption.ClientStress should be defined.");

            Assert.IsTrue(Enum.IsDefined(type, "Release"),
                "ZeroConfigurationOption.Release should be defined.");
        }
    }
}