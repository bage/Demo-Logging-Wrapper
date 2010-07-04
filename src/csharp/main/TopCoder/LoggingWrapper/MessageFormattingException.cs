/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// The MessageFormattingException is thrown in from the Log and LogNamedMessage methods of the Logger
    /// class. It indicates that the parameters to these methods could not correctly be combined with the
    /// message string. Commonly, this will indicate a failure of a string.Format call. For some logging
    /// implementations, it may indicate a similar failure in the backend logging solution.
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
    public class MessageFormattingException : LoggingException
    {
        /// <summary>
        /// Creates a new instance of MessageFormattingException.
        /// </summary>
        public MessageFormattingException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of MessageFormattingException with a descriptive message about why
        /// the exception was thrown.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        public MessageFormattingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of MessageFormattingException with a descriptive message about why
        /// the exception was thrown, and a wrapped exception which was the actual cause.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        /// <param name="cause">The wrapped exception which was the actual cause.</param>
        public MessageFormattingException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// Creates a new instance of MessageFormattingException from the serialization info and streaming
        /// context given.
        /// </summary>
        ///
        /// <param name="info">Serialization info for the exception.</param>
        /// <param name="context">Streaming context of the exception.</param>
        protected MessageFormattingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}