/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Runtime.Serialization;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>Exception used to signal object definition retrieval related problems. </p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>It is used to wrap any exceptions thrown while a specific <see cref="ObjectFactory"/>
    /// implementation encounters while retrieving object definitions given the key.</p>
    ///
    /// <p>A Configuration Manager implementation will wrap configuration exceptions,
    /// a database implementation will wrap SQL exceptions and so on.</p>
    ///
    /// <p>This class extends ObjectCreationException because as the user is concerned,
    /// an object definition retrieval exception is still an object creation problem.</p>
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
    internal class ObjectSourceException : ObjectCreationException
    {
        /// <summary>
        /// <p>Initializes a new instance of the ObjectSourceException class.</p>
        /// </summary>
        public ObjectSourceException()
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectSourceException class with
        /// a specified error message.</p>
        /// </summary>
        ///
        /// <param name="message">A message that describes the error.</param>
        public ObjectSourceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectSourceException class
        /// with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.</p>
        /// </summary>
        ///
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ObjectSourceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of the ObjectSourceException class with serialized data.</p>
        /// </summary>
        ///
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ObjectSourceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
