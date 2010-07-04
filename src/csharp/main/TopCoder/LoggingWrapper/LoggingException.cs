/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// The base class for all exceptions thrown from the Log and LogNamedMessage methods of the Logger
    /// class. Generally, a LoggingException indicates that the backend logging solution encountered an
    /// error. For the specific condition that message formatting failed, a MessageFormattingException is
    /// thrown instead. Future versions of the component may add more specific exceptions.
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable thus thread safe.
    /// </para>
    /// </threadsafety>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class LoggingException : Exception
    {
        /// <summary>
        /// Creates a new instance of LoggingException.
        /// </summary>
        public LoggingException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of LoggingException with a descriptive message about why
        /// the exception was thrown.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        public LoggingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of LoggingException with a descriptive message about why
        /// the exception was thrown, and a wrapped exception which was the actual cause.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        /// <param name="cause">The wrapped exception which was the actual cause.</param>
        public LoggingException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// Creates a new instance of LoggingException from the serialization info and streaming context
        /// given.
        /// </summary>
        ///
        /// <param name="info">Serialization info for the exception.</param>
        /// <param name="context">Streaming context of the exception.</param>
        protected LoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}