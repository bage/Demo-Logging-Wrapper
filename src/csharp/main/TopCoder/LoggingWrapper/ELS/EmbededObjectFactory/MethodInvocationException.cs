/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>Exception used to signal object creation related problems due to not
    /// being able to invoke a method on a created object.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>It is used to wrap any exceptions thrown while working with method/property
    /// invocations (reflection related). Primarily this will include exceptions related
    /// to the inability to find a compatible method or invoke it.</p>
    ///
    /// <p>Thread Safety:
    /// This class is derived from thread unsafe class and it is not thread safe.</p>
    /// </remarks>
    ///
    /// <author>aubergineanode</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    internal class MethodInvocationException : ObjectCreationException
    {
        /// <summary>
        /// <p>Initializes a new instance of the MethodInvocationException class.</p>
        /// </summary>
        public MethodInvocationException()
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the MethodInvocationException class with
        /// a specified error message.</p>
        /// </summary>
        ///
        /// <param name="message">A message that describes the error.</param>
        public MethodInvocationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the MethodInvocationException class
        /// with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.</p>
        /// </summary>
        ///
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MethodInvocationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the MethodInvocationException class with serialized data.</p>
        /// </summary>
        ///
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MethodInvocationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
