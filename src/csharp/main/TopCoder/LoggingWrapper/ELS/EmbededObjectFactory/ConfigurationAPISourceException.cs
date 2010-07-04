/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Runtime.Serialization;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <para>
    /// This exception extends ObjectSourceException to indicate the <see cref="IConfiguration"/>
    /// object source is invalid (For example, missing required node).
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// This exception is thrown by <see cref="ConfigurationAPIObjectFactory.GetDefinition"/>
    /// method when exception occurs whiling getting the object definition.
    /// </para>
    /// </remarks>
    ///
    /// <threadsafety>
    /// This class is derived from thread unsafe class and it is not thread safe.
    /// </threadsafety>
    ///
    /// <author>justforplay</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    internal class ConfigurationAPISourceException : ObjectSourceException
    {
        /// <summary>
        /// <para>Initializes a new instance of the ConfigurationAPISourceException class.</para>
        /// </summary>
        public ConfigurationAPISourceException()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the ConfigurationAPISourceException class with
        /// a specified error message.</para>
        /// </summary>
        ///
        /// <param name="message">A message that describes the error.</param>
        public ConfigurationAPISourceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the ConfigurationAPISourceException class
        /// with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.</para>
        /// </summary>
        ///
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ConfigurationAPISourceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the ConfigurationAPISourceException class with serialized data.</para>
        /// </summary>
        ///
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ConfigurationAPISourceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
