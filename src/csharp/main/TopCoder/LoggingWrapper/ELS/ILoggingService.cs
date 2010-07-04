/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System.ServiceModel;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// <para>
    /// This interface defines the contract to log the message.
    /// </para>
    /// <para>
    /// The defined log methods contain the originator indicating the originator of the log event,
    /// the context indicating the additional information that may be displayed as a part of the log
    /// entry, the logging level (the given one or as indicated by method name), the message and
    /// the message parameters. This WCF interface will work in singleton pattern.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <strong>Thread Safety:</strong>
    /// <para>
    /// Implementation should be thread-safe.
    /// </para>
    /// </remarks>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ILoggingService
    {
        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, logging level, and message parameters.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="level">
        /// The logging level.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void Log(string originator, string[] context, Level level, string message, string[] parameters);

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.DEBUG</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void LogDebug(string originator, string[] context, string message, string[] parameters);

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.INFO</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void LogInfo(string originator, string[] context, string message, string[] parameters);

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.WARN</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void LogWarning(string originator, string[] context, string message, string[] parameters);

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.ERROR</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void LogError(string originator, string[] context, string message, string[] parameters);

        /// <summary>
        /// <para>
        /// Log the message with the specified originator, context, and message parameters using
        /// <c>Level.FATAL</c> logging level.
        /// </para>
        /// </summary>
        ///
        /// <param name="originator">
        /// A string value identifying the originator of the log event. It can not be <c>null</c>,
        /// but can be empty.
        /// </param>
        /// <param name="context">
        /// The additional information that may be displayed as a part of the log entry. It can be
        /// <c>null</c>, but can not be an empty array. The element of it can not be null, but can
        /// be empty.
        /// </param>
        /// <param name="message">
        /// The message to be logged. It can not be null nor empty after trimming.
        /// </param>
        /// <param name="parameters">
        /// The message parameters. It can be <c>null</c> or an empty array. The element of it can
        /// also be empty or null.
        /// </param>
        ///
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// <para>
        /// If any exception occurs during execution. All the argument validation exceptions and
        /// business exceptions should be wrapped by the <see cref="FaultException&lt;TCFaultException&gt;"/>.
        /// </para>
        /// </exception>
        void LogFatal(string originator, string[] context, string message, string[] parameters);
    }
}