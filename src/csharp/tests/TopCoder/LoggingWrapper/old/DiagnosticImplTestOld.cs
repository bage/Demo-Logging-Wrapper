// @author TCSDESIGNER
// @copyright (c) TopCoder Software

using System;
using System.Diagnostics;
using NUnit.Framework;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Test cases for LoggingWrapper
    /// </summary>
    [TestFixture]
    [CoverageExclude]
    public class DiagnosticImplTest
    {
        /// <summary>
        /// Set ups configuration of the LogManager
        /// </summary>
        [SetUp]
        public void StartUp()
        {
            if(EventLog.Exists("Test Log"))
                EventLog.Delete("Test Log");
            LogManager.Configuration = new System.Collections.Specialized.NameValueCollection();
            LogManager.Configuration.Add(LogManager.CLASS_PARAMETER, "TopCoder.LoggingWrapper.DiagnosticImpl");
            LogManager.Configuration.Add("Source", "Test Log");
            LogManager.Configuration.Add("LogName", "Test Log");
            LogManager.LoadConfiguration();
        }
        /// <summary>
        /// Test if a log is of expected type.
        /// </summary>
        [Test]
        public void LogInitializedCorectly()
        {
            Assertion.AssertEquals("TopCoder.LoggingWrapper.DiagnosticImpl",LogManager.GetLogger().GetType().FullName);
        }
        /// <summary>
        /// Tests if log entry is actualy writen to the system event log.
        /// </summary>
        [Test]
        public void isSimpleEntryWriten()
        {
            EventLog ea=new EventLog("Test Log",".","Test Log");
            LogManager.Log("Hello world!");
            EventLogEntry[] entries=new EventLogEntry[ea.Entries.Count];
            ea.Entries.CopyTo(entries,0);
            Assertion.AssertEquals("Hello world!",entries[0].Message);
            ea.Clear();
        }
        /// <summary>
        /// Tests if log entry is actualy writen to the system event log.
        /// </summary>
        [Test]
        public void isFormatedEntryWriten()
        {
            EventLog ea=new EventLog("Test Log",".","Test Log");
            LogManager.Log("Hello {0}!", "World");
            EventLogEntry[] entries=new EventLogEntry[ea.Entries.Count];
            ea.Entries.CopyTo(entries,0);
            Assertion.AssertEquals("Hello World!",entries[0].Message);
            ea.Clear();
        }
        /// <summary>
        /// Removes test log from the system.
        /// </summary>
        [TearDown]
        public void Down()
        {
            if(EventLog.Exists("Test Log"))
                EventLog.Delete("Test Log");
        }
    }
}
