// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;
    using System.Collections;
    using System.IO;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests the Log4NETImpl class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    [TestFixture]
    [CoverageExclude]
    public class UnitTestLog4NETImpl
    {
        /// <summary>
        /// The name of the log file that will be used to write the test log to.
        /// </summary>
        private const string logfile = "..\\..\\test_files\\error-log.txt";

        /// <summary>
        /// Some test properties for the log to initialize with.
        /// </summary>
        private Hashtable properties = new Hashtable();

        /// <summary>
        /// Some test properties for the log to initialize with.
        /// </summary>
        private Hashtable properties2 = new Hashtable();

        /// <summary>
        /// A default Log4NETImpl instance to test with.
        /// </summary>
        private Log4NETImpl log;

        /// <summary>
        /// Sets up some useful variables for use later on.
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            this.properties["logger_name"] = "test log #1";

            this.properties2["logger_name"] = "test log #1";
            this.properties2[Log4NETImpl.CONFIG_FILE] = "../../conf/log4net.config";

            this.log = new Log4NETImpl(this.properties2);
        }

        /// <summary>
        /// Cleans up any files that were left on the system.
        /// </summary>
        [TearDown]
        public void Destroy()
        {
            log4net.LogManager.Shutdown();
            File.Delete(logfile);
        }

        /// <summary>
        /// Makes sure the CONFIG_FILE property is correct.
        /// </summary>
        [Test]
        public void TestConfigFileStringCorrect()
        {
            Assertion.Assert(Log4NETImpl.CONFIG_FILE == "config_file");
        }

        /// <summary>
        /// Attemps to create a Log4NETImpl instance with a null properties parameter.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateLog4NETImplNullParameter()
        {
            new Log4NETImpl((IDictionary)null);
        }

        /// <summary>
        /// Attemps to create a Log4NETImpl instance with a properties dictionary missing the config_file property.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLog4NETImplNoConfigFile()
        {
            new Log4NETImpl(this.properties);
        }

        /// <summary>
        /// Attemps to create a Log4NETImpl instance with a config_file property pointing to a non-existant file.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLog4NETImplInvalidConfigFile()
        {
            this.properties[Log4NETImpl.CONFIG_FILE] = "doesntexist";

            new Log4NETImpl(this.properties);
        }

        /// <summary>
        /// Attemps to create a Log4NETImpl instance with a config_file property that isn't a string.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateLog4NETImplConfigFilePropertyInvalid()
        {
            this.properties[Log4NETImpl.CONFIG_FILE] = new Hashtable();

            new Log4NETImpl(this.properties);
        }

        /// <summary>
        /// Creates a Log4NETImpl instance and checks that everything is initialized properly.
        /// </summary>
        [Test]
        public void CreateLog4NET()
        {
            Assertion.AssertEquals("Default level not correctly initialized.", this.log.DefaultLevel, Level.DEBUG);
            Assertion.AssertEquals("Name not correctly initialized.", this.log.Logname, "test log #1");
        }

        /// <summary>
        /// Checks if a simple message is correctly written to the log.
        /// </summary>
        [Test]
        public void CheckSimpleEntryWritten()
        {
            this.log.Log("Hello world!");

            this.CheckForString("Hello world!");
        }

        /// <summary>
        /// Checks if a formatted message is correctly written to the log.
        /// </summary>
        [Test]
        public void CheckFormattedEntryWritten()
        {
            this.log.Log("Hello world! {0} {1}", 15, "test");

            this.CheckForString("Hello world! 15 test");
        }

        /// <summary>
        /// Checks if a simple message is correctly written to the log.
        /// </summary>
        [Test]
        public void CheckSimpleEntryWrittenLevelInfo()
        {
            this.log.Log(Level.INFO, "Hello world!");

            this.CheckForString("Hello world!");
        }

        /// <summary>
        /// Checks if a formatted message is correctly written to the log.
        /// </summary>
        [Test]
        public void CheckFormattedEntryWrittenLevelInfo()
        {
            this.log.Log(Level.INFO, "Hello world! {0} {1}", 15, "test");

            this.CheckForString("Hello world! 15 test");
        }

        /// <summary>
        /// Checks if the level is obeyed when writing to the file.
        /// </summary>
        [Test]
        public void CheckEntryNotWrittenLevelTooLow()
        {
            this.log.Log(Level.OFF, "Hello world! {0} {1}", 15, "test");

            this.CheckStringNotExist("Hello world! 15 test");
        }

        /// <summary>
        /// Ensures that any exceptions thrown by Log() are ignored.
        /// </summary>
        [Test]
        public void LogExceptionIgnore()
        {
            log4net.LogManager.Shutdown();

            this.log.Log("blah");
        }

        /// <summary>
        /// Checks to ensure all Levels are enabled in IsLevelEnabled.
        /// </summary>
        [Test]
        public void TestLevelEnabled()
        {
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.DEBUG));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.ERROR));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.INFO));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.WARN));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.OFF));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.SUCCESSAUDIT));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.FAILUREAUDIT));
            Assertion.Assert("Level should be enabled", this.log.IsLevelEnabled(Level.FATAL));
        }

        /// <summary>
        /// Checks to see if the string is present on the first line of the file.
        /// </summary>
        /// <param name="written">The string to check for.</param>
        private void CheckForString(string written)
        {
            log4net.LogManager.Shutdown();

            using (FileStream fs=File.OpenRead(logfile))
            {
                StreamReader sr=new StreamReader(fs);
                string str=sr.ReadLine();

                Assertion.Assert("Logged message not found", str.IndexOf(written) > -1);
            }
        }

        /// <summary>
        /// Checks to see if the string is present on the first line of the file.
        /// </summary>
        /// <param name="written">The string to check for.</param>
        private void CheckStringNotExist(string written)
        {
            log4net.LogManager.Shutdown();

            using (FileStream fs=File.OpenRead(logfile))
            {
                StreamReader sr=new StreamReader(fs);
                string str=sr.ReadLine();
                if (str == null) return;

                Assertion.Assert("Logged message found", str.IndexOf(written) == -1);
            }
        }
    }
}
