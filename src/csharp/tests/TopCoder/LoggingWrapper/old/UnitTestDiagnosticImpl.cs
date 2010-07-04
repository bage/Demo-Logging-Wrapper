// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests the DiagnosticImpl class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    [TestFixture]
    [CoverageExclude]
    public class UnitTestDiagnosticImpl
    {
        /// <summary>
        /// Some test properties for the log to initialize with.
        /// </summary>
        private Hashtable properties = new Hashtable();

        /// <summary>
        /// Some test properties for the log to initialize with.
        /// </summary>
        private Hashtable properties2 = new Hashtable();

        /// <summary>
        /// A default DiagnosticImpl instance to test with.
        /// </summary>
        private DiagnosticImpl log;

        /// <summary>
        /// The name of the log file that will be used to write the test log to.
        /// </summary>
        private static string LOG_NAME = "logger";

        /// <summary>
        /// Sets up some useful variables for testing DiagnosticImpl
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            if (EventLog.Exists(LOG_NAME))
                EventLog.Delete(LOG_NAME);

            this.properties["logger_name"] = LOG_NAME;

            this.properties2["logger_name"] = LOG_NAME;
            this.properties2[DiagnosticImpl.SOURCE] = LOG_NAME;

            this.log = new DiagnosticImpl(this.properties2);
        }

        /// <summary>
        /// Removes the test log from the system.
        /// </summary>
        [TearDown]
        public void Down()
        {
            this.log.Dispose();

            if (EventLog.Exists(LOG_NAME))
                EventLog.Delete(LOG_NAME);
        }

        /// <summary>
        /// Makes sure the SOURCE property is correct.
        /// </summary>
        [Test]
        public void TestSourceStringCorrect()
        {
            Assertion.Assert(DiagnosticImpl.SOURCE == "source");
        }

        /// <summary>
        /// Attemps to create a DiagnosticImpl instance with a null properties parameter.
        /// Should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateDiagnosticImplNullParameter()
        {
            new DiagnosticImpl((IDictionary)null);
        }

        /// <summary>
        /// Attemps to create a DiagnosticImpl instance with a properties dictionary missing the source property.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateDiagnosticImplNoSource()
        {
            new DiagnosticImpl(this.properties);
        }

        /// <summary>
        /// Attemps to create a DiagnosticImpl instance with a config_file property that isn't a string.
        /// Should throw ConfigException.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateDiagnosticImplSourcePropertyInvalid()
        {
            this.properties[DiagnosticImpl.SOURCE] = new Hashtable();

            new DiagnosticImpl(this.properties);
        }

        /// <summary>
        /// Creates a DiagnosticImpl instance and checks that everything is initialized properly.
        /// </summary>
        [Test]
        public void CreateDiagnosticImpl()
        {
            Assertion.AssertEquals("Default level not correctly initialized.", this.log.DefaultLevel, Level.DEBUG);

            Assertion.AssertEquals("Name not correctly initialized.", this.log.Logname, LOG_NAME);
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
            if (EventLog.Exists("testsource"))
                EventLog.Delete("testsource");

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
            EventLog ea=new EventLog(LOG_NAME, ".", LOG_NAME);

            EventLogEntry[] entries=new EventLogEntry[ea.Entries.Count];
            ea.Entries.CopyTo(entries,0);

            Assertion.AssertEquals("String not written to log.", written, entries[0].Message);
        }

        /// <summary>
        /// Checks to see if the string is present on the first line of the file.
        /// </summary>
        /// <param name="written">The string to check for.</param>
        private void CheckStringNotExist(string written)
        {
            if (!EventLog.Exists(LOG_NAME))
                return;

            EventLog ea=new EventLog(LOG_NAME, ".", LOG_NAME);
            if (ea.Entries.Count == 0)
                return;

            EventLogEntry[] entries=new EventLogEntry[ea.Entries.Count];
            ea.Entries.CopyTo(entries, 0);

            Assertion.Assert("String written to log.", entries[0].Message.IndexOf(written) == -1);
        }
    }
}
