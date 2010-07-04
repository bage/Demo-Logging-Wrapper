/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>Exception used to signal object creation related problems.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>It is used to wrap any exceptions thrown while working with assemblies,
    /// types and invocations (reflection related). This may include I/O exceptions
    /// while loading assemblies.</p>
    ///
    /// <p>Thread Safety:
    /// This class is derived from thread unsafe class and it is not thread safe.</p>
    /// </remarks>
    ///
    /// <author>adic</author>
    /// <author>LittleBlack</author>
    /// <author>aubergineanode</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    internal class ObjectCreationException : ApplicationException
    {
        /// <summary>
        /// <p>Initializes a new instance of the ObjectCreationException class.</p>
        /// </summary>
        public ObjectCreationException()
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectCreationException class with
        /// a specified error message.</p>
        /// </summary>
        ///
        /// <param name="message">A message that describes the error.</param>
        public ObjectCreationException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectCreationException class
        /// with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.</p>
        /// </summary>
        ///
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ObjectCreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectCreationException class with serialized data.</p>
        /// </summary>
        ///
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ObjectCreationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
