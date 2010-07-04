/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// This enum represents the logging level of a message that will be logged through a Logger interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Enum is thread safe.
    /// </para>
    /// </remarks>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public enum Level
    {
        /// <summary>
        /// Logging level FATAL designates very severe error events that will presumably lead the
        /// application to abort.
        /// </summary>
        FATAL = 80000,

        /// <summary>
        /// Logging level ERROR designates error events that might still allow the application to continue
        /// running.
        /// </summary>
        ERROR = 70000,

        /// <summary>
        /// Logging level FAILUREAUDIT indicates a security event that occurs when an audited access
        /// attempt fails; for example, a failed attempt to open a file.
        /// </summary>
        FAILUREAUDIT = 60000,

        /// <summary>
        /// Logging level SUCCESSAUDIT indicates a significant, successful operation.
        /// </summary>
        SUCCESSAUDIT = 50000,

        /// <summary>
        /// Logging level WARN designates potentially harmful situations.
        /// </summary>
        WARN = 40000,

        /// <summary>
        /// Logging level INFO  designates informational messages that highlight the progress of the
        /// application at coarse-grained level.
        /// </summary>
        INFO = 30000,

        /// <summary>
        /// Logging level DEBUG designates fine-grained informational events that are most useful to debug
        /// an application.
        /// </summary>
        DEBUG = 20000,

        /// <summary>
        /// Logging level OFF designates a lower level priority than all the rest.
        /// </summary>
        OFF = 1
    }
}