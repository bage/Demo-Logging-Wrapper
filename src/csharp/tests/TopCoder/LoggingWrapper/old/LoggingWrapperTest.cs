// @author TCSDESIGNER
// @copyright (c) TopCoder Software

using System;
using NUnit.Framework;
using System.IO;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Test cases for LoggingWrapper
    /// </summary>
    [TestFixture]
    [CoverageExclude]
    public class LoggingWrapperTest
    {
        private const string logfile = @"..\..\test_files\error-log.txt";

        /// <summary>
        /// <para>Prepares the environment for testing.</para>
        /// </summary>
        [SetUp]
        protected void StartUp()
        {
            LogManager.Configuration = new System.Collections.Specialized.NameValueCollection();
            LogManager.Configuration.Add(LogManager.CLASS_PARAMETER, "TopCoder.LoggingWrapper.Log4NETImpl");
            LogManager.Configuration.Add("Log4NetConfiguration", @"..\..\conf\log4net.config");
            LogManager.Configuration.Add("LogName", "Test Log");
            LogManager.LoadConfiguration();
            LogManager.LoadPlugin("TopCoder.LoggingWrapper.Log4NETImpl");
        }

        /// <summary>
        /// <para>Tests the LoadConfiguration, if it can complete successfully.</para>
        /// </summary>
        [Test]
        public void LoadConfiguration()
        {
            LogManager.LoadConfiguration();
        }

        /// <summary>
        /// <para>Tests the LoadPlugin when it is passed an invalid class name. It
        /// should throw <b>InvalidPluginException</b>.</para>
        /// </summary>
        [Test]
        [ExpectedException(typeof(TopCoder.LoggingWrapper.InvalidPluginException))]
        public void InvalidImplementationException()
        {
            LogManager.LoadPlugin(typeof(TopCoder.LoggingWrapper.InvalidPluginException).FullName);
        }

        /// <summary>
        /// <para>Tests the LoadPlugin when it is passed an nonexistant class name. It
        /// should throw <b>ConfigException</b>.</para>
        /// </summary>
        [Test]
        [ExpectedException(typeof(TopCoder.LoggingWrapper.ConfigException))]
        public void ConfigException()
        {
            LogManager.LoadPlugin("TopCoder.LoggingWrapper.NotExistedSolution");
        }

        /// <summary>
        /// <para>Tests the <b>LogManager.Log</b> method if it can log successfully.</para>
        /// </summary>
        [Test]
        public void SimpleLog()
        {
            LogManager.Log("Hello world!");
        }

        /// <summary>
        /// <para>Tests the <b>LogManager.Log</b> method if it can log with formatted string.</para>
        /// </summary>
        [Test]
        public void FormatLog()
        {
            LogManager.Log("Hello {0}! ", "World!");
        }

        /// <summary>
        /// <para>Tests the <b>LogManager.LoadConfiguration</b> method if it can
        /// work between two Log calls.</para>
        /// </summary>
        [Test]
        public void ReloadConfigurationDuringExecution()
        {
            LogManager.Log("Hello {0}! ", "World!");
            LogManager.LoadConfiguration();
            LogManager.Log("Hello {0}!! ", 10);
        }

        /// <summary>
        /// <para>Tests if the the logger is equal to what is configured.</para>
        /// </summary>
        [Test]
        public void RetrievingLoggingSolution()
        {
            LogManager.Log("Hello {0}! ", "World!");
            Logger log = LogManager.GetLogger();
            Assertion.Equals(log.ToString(), "TopCoder.LoggingWrapper.DiagnosticImpl");
        }

        /// <summary>
        /// <para>Tests whether the OFF log level is enable.</para>
        /// </summary>
        [Test]
        public void IsEnableLoggingLevel()
        {
            Assertion.Equals(LogManager.IsLevelEnabled(Level.OFF), true);
        }

        /// <summary>
        /// <para>Tests whether setting of the log level works correctly.</para>
        /// </summary>
        [Test]
        public void SetupDefaultLoggingLevel()
        {
            LogManager.GetLogger().Level = Level.ERROR;
            LogManager.Log("DANGEROUS ERROR!");
            LogManager.GetLogger().Level = Level.DEBUG;
            LogManager.Log("JUST DEBUG");
        }

        /// <summary>
        /// Removes test log from the system.
        /// </summary>
        [TearDown]
        public void Down()
        {
            log4net.LogManager.Shutdown();
            if (File.Exists(logfile))
                File.Delete(logfile);
        }

    }
}
