// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests the ConfigException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    [TestFixture]
    [CoverageExclude]
    public class UnitTestConfigException
    {
        /// <summary>
        /// Creates an ConfigException object.
        /// </summary>
        [Test]
        public void CreateConfigExceptionString()
        {
            new ConfigException("failed");
        }

        /// <summary>
        /// Creates an ConfigException object.
        /// </summary>
        [Test]
        public void CreateConfigExceptionStringException()
        {
            new ConfigException("failed", new Exception());
        }
    }
}