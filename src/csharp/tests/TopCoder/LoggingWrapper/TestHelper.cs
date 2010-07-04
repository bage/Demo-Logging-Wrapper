/*
 * Copyright (C) 2008 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Diagnostics;
using System.ServiceModel;
// using TopCoder.Services.WCF.Logging;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// Helper class for tests.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c)2008, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExclude]
    internal static class TestHelper
    {
        /// <summary>
        /// A string as name of log for testing.
        /// </summary>
        internal const string LOG_NAME = "LogTest";

        /// <summary>
        /// A string as name of machine for testing.
        /// </summary>
        internal const string MACHINE_NAME = ".";

        /// <summary>
        /// A string as url of service for testing.
        /// </summary>
        internal const string ELS_URL = "http://localhost:1234/ELS";

        /// <summary>
        /// <para>
        /// Represent the defalut logger class used to be created.
        /// </para>
        /// </summary>
        internal const string DEFAULT_LOGGER_CLASS_APP_SETTING_NAME = "DEFAULT_LOGGER_CLASS"; 

        /// <summary>
        /// <para>
        /// Create custom event logs with given source for test.
        /// </para>
        /// </summary>
        /// <param name="source">The source of logs.</param>
        internal static void CreateLogs(string source)
        {
            try
            {
                EventLog.CreateEventSource(source, LOG_NAME);
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// <para>
        /// Gets the number of event logs with given source.
        /// </para>
        /// </summary>
        /// <param name="source">The source of logs.</param>
        /// <returns>The number of event logs with given source.</returns>
        internal static int GetLogsCount(string source)
        {
            if (EventLog.Exists(LOG_NAME))
            {
                return new EventLog(LOG_NAME, MACHINE_NAME, source).Entries.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// <para>
        /// Gets the event log entry with given index in given source.
        /// </para>
        /// </summary>
        /// <param name="source">The source of logs.</param>
        /// <param name="index">The index of event log message.</param>
        /// <returns>The event log entry with given index in given source.</returns>
        internal static EventLogEntry GetLogEntry(string source, int index)
        {
            return new EventLog(LOG_NAME, MACHINE_NAME, source).Entries[index];
        }

        /// <summary>
        /// <para>
        /// Clears custom event logs for test.
        /// </para>
        /// </summary>
        internal static void ClearLogs()
        {
            if (EventLog.Exists(LOG_NAME))
            {
                EventLog.Delete(LOG_NAME);
            }
        }
    }
}