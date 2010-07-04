/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// This exception will be thrown by all errors caused by incorrect configuration.  This will happen
    /// because the Logger can not be succesfully created from the configuration values.
    /// <para>
    /// Changes in 3.0: this class is changed to inherit from BaseException. Two constructors are added.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable thus thread safe.
    /// </para>
    /// </threadsafety>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <since>2.0</since>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class ConfigException : Exception
    {
        /// <summary>
        /// Creates a new instance of ConfigException.
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        public ConfigException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of ConfigException with a descriptive message about why
        /// the exception was thrown.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        public ConfigException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of ConfigException with a descriptive message about why
        /// the exception was thrown, and a wrapped exception which was the actual cause.
        /// </summary>
        /// <param name="message">A descriptive message of why the exception was thrown.</param>
        /// <param name="cause">The wrapped exception which was the actual cause.</param>
        public ConfigException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// Creates a new instance of ConfigException from the serialization info and streaming context
        /// given.
        /// <para>
        /// New in 3.0.
        /// </para>
        /// </summary>
        ///
        /// <param name="info">Serialization info for the exception.</param>
        /// <param name="context">Streaming context of the exception.</param>
        protected ConfigException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
