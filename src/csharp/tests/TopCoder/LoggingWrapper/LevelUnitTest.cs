/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Unit test for Level.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExclude]
    public class LevelUnitTest
    {
        /// <summary>
        /// Tests the accuracy of definition Level.
        /// </summary>
        [Test]
        public void TestLevel()
        {
            Type type = typeof (Level);

            Assert.IsTrue(type.IsEnum, "Level should be enum.");

            Assert.AreEqual(8, Enum.GetNames(type).Length, "incorrect const values in Level.");

            Assert.IsTrue(Enum.IsDefined(type, "FATAL"), "Level.FATAL should be defined.");
            Assert.AreEqual(80000, (int) Level.FATAL, "incorrect value of Level.FATAL");

            Assert.IsTrue(Enum.IsDefined(type, "ERROR"), "Level.ERROR should be defined.");
            Assert.AreEqual(70000, (int) Level.ERROR, "incorrect value of Level.ERROR");

            Assert.IsTrue(Enum.IsDefined(type, "FAILUREAUDIT"), "Level.FAILUREAUDIT should be defined.");
            Assert.AreEqual(60000, (int) Level.FAILUREAUDIT, "incorrect value of Level.FAILUREAUDIT");

            Assert.IsTrue(Enum.IsDefined(type, "SUCCESSAUDIT"), "Level.SUCCESSAUDIT should be defined.");
            Assert.AreEqual(50000, (int) Level.SUCCESSAUDIT, "incorrect value of Level.SUCCESSAUDIT");

            Assert.IsTrue(Enum.IsDefined(type, "WARN"), "Level.WARN should be defined.");
            Assert.AreEqual(40000, (int) Level.WARN, "incorrect value of Level.WARN");

            Assert.IsTrue(Enum.IsDefined(type, "INFO"), "Level.INFO should be defined.");
            Assert.AreEqual(30000, (int) Level.INFO, "incorrect value of Level.INFO");

            Assert.IsTrue(Enum.IsDefined(type, "DEBUG"), "Level.DEBUG should be defined.");
            Assert.AreEqual(20000, (int) Level.DEBUG, "incorrect value of Level.DEBUG");

            Assert.IsTrue(Enum.IsDefined(type, "OFF"), "Level.OFF should be defined.");
            Assert.AreEqual(1, (int) Level.OFF, "incorrect value of Level.OFF");
        }
    }
}