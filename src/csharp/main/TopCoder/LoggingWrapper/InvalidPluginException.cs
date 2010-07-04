// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;

    /// <summary>
    /// <para>
    /// Exception thrown when a pluggable logging implementation is invalid.  Generally this means that the logging
    /// implementation was missing an implementation of the Logger class, or does not have the correct public
    /// constructor.
    /// </para>
    /// This class is now obsolete.
    /// </summary>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <version>2.0</version>
    [Obsolete]
    [Serializable]
    public class InvalidPluginException : System.Exception
    {
        /// <summary>
        /// Creates a new instance of InvalidPluginException with the reason for the exception.
        /// </summary>
        /// <param name="message">The reason the exception happened</param>
        public InvalidPluginException(string message) : base(message)
        {
        }
    }
}
