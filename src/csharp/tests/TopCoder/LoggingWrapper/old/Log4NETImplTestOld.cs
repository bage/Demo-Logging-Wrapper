// @author TCSDESIGNER
// @copyright (c) TopCoder Software

using System;
using System.IO;
using NUnit.Framework;
using log4net.Config;
using log4net;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Test cases for LoggingWrapper
    /// </summary>
    [TestFixture]
    [CoverageExclude]
    public class Log4NETImplTest
    {
        /// <summary>
        /// <para>Private attribute holding the path to error log file.</para>
        /// </summary>
        private const string logfile = @"../../test_files/error-log.txt";

        /// <summary>
        /// <para>Prepares the environment for testing.</para>
        /// </summary>
        [SetUp]
        public void StartUp()
        {
            LogManager.Configuration = new System.Collections.Specialized.NameValueCollection();
            LogManager.Configuration.Add(LogManager.CLASS_PARAMETER, "TopCoder.LoggingWrapper.Log4NETImpl");
            LogManager.Configuration.Add("Log4NetConfiguration", @"..\..\conf\log4net.config");
            LogManager.Configuration.Add("LogName", "Test Log");
            LogManager.LoadConfiguration();
            LogManager.LoadPlugin("TopCoder.LoggingWrapper.Log4NETImpl");

        }
        /// <summary>
        /// Test if a log is of expected type.
        /// </summary>
        [Test]
        public void LogInitializedCorectly()
        {
            Assertion.AssertEquals("TopCoder.LoggingWrapper.Log4NETImpl", LogManager.GetLogger().GetType().FullName);
        }
        /// <summary>
        /// Tests if log entry is actualy writen to the system event log.
        /// </summary>
        [Test]
        public void isSimpleEntryWriten()
        {
            LogManager.Log("Hello World!");
            log4net.LogManager.Shutdown();
            FileStream fs = File.OpenRead(logfile);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            fs.Close();
            int i = str.IndexOf("Hello World!");

            Assertion.Assert(i > -1);

        }
        /// <summary>
        /// Tests if log entry is actualy writen to the system event log.
        /// </summary>
        [Test]
        public void isFormatedEntryWriten()
        {
            LogManager.Log("Hello formated {0}!", "World");
            log4net.LogManager.Shutdown();
            FileStream fs = File.OpenRead(logfile);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            fs.Close();
            int i = str.IndexOf("Hello formated World!");
            Assertion.Assert(i > -1);
        }
        /// <summary>
        /// Removes test log from the system.
        /// </summary>
        [TearDown]
        public void Down()
        {
            log4net.LogManager.Shutdown();
            File.Delete(logfile);
        }
    }
}
