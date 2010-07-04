// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;

    /// <summary>
    /// Exception class for all pluggable implementation exceptions
    /// </summary>
    /// <author>TCSDEVELOPER, Mikhail_T</author>
    /// <version>2.0</version>
    [Obsolete]
    [Serializable]
    public class PluginException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the PluginException class
        /// </summary>
        public PluginException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PluginException class with defined message
        /// </summary>
        public PluginException(string message ) : base(message)
        {
        }
    }
}
