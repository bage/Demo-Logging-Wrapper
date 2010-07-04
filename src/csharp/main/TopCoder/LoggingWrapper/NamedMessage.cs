/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// The NamedMessage class is a tiny data storage class that stores all the information for one named message. It
    /// does not have any behavior, only a few getters.  This class is used by the Logger class (and implementations)
    /// to translate between a message name and all the information that is needed to actually log a messaage.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable and hence thread-safe.
    /// </para>
    /// </threadsafety>
    ///
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class NamedMessage
    {
        /// <summary>
        /// <para>
        /// Represents the name of the named message. This field is set in the constructor, is immutable, and can not
        /// be null or an empty string.
        /// </para>
        /// </summary>
        private readonly string name;

        /// <summary>
        /// <para>
        /// Represents the actual text that will be logged for this named message. The text should include any
        /// backend specific way of referencing parameters. This field is set in the constructor, is immutable, and
        /// can not be null or an empty string.
        /// </para>
        /// </summary>
        private readonly string text;

        /// <summary>
        /// <para>
        /// Represents the names of the parameters to be used when logging the message. Logging implementations
        /// associate the parameters passed to LogNamedMessage by these values by matching index. This field is set
        /// in the constructor, is immutable, and can be empty. Can not be null and can not contain null or empty
        /// items.
        /// </para>
        /// </summary>
        private readonly IList<string> parameterNames;

        /// <summary>
        /// <para>
        /// Represents the default level at which this named message should be logged, if a specific level is not
        /// provided to the LogNamedMessage call. This field is set in the constructor, and is immutable.
        /// </para>
        /// </summary>
        private readonly Level defaultLevel;

        /// <summary>
        /// <para>
        /// Represents the property to get the name of the named message.
        /// </para>
        /// </summary>
        /// <value>The name of the named message.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// <para>
        /// Represents the property to get the actual text that will be logged for this named message.
        /// </para>
        /// </summary>
        /// <value>The actual text that will be logged for this named message.</value>
        public string Text
        {
            get
            {
                return text;
            }
        }

        /// <summary>
        /// <para>
        /// Represents the property to get a copy of the names of the parameters to be used when logging the message.
        /// </para>
        /// </summary>
        /// <value>The names of the parameters to be used when logging the message.</value>
        public IList<string> ParameterNames
        {
            get
            {
                return new List<string>(parameterNames);
            }
        }

        /// <summary>
        /// <para>
        /// Represents the property to get default level at which this named message should be logged.
        /// </para>
        /// </summary>
        /// <value>The default level at which this named message should be logged.</value>
        public Level DefaultLevel
        {
            get
            {
                return defaultLevel;
            }
        }

        /// <summary>
        /// Creates a new instance of a NamedMessage.
        /// </summary>
        /// <param name="text">The actual text that will be logged for this named message.</param>
        /// <param name="name">The name of the named message.</param>
        /// <param name="parameterNames">The names of the parameters to be used when logging the message.</param>
        /// <param name="defaultLevel">The default level at which this named message should be logged.</param>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        /// <exception cref="ArgumentException">If text or name is empty, or if parameterNames contains null or empty
        /// items, or level is invalid.</exception>
        public NamedMessage(string text, string name, IList<string> parameterNames, Level defaultLevel)
        {
            Helper.ValidateNotNullNotEmptyString(text, "text");
            Helper.ValidateNotNullNotEmptyString(name, "name");
            Helper.ValidateNotNull(parameterNames, "parameterNames");
            if (!Enum.IsDefined(typeof(Level), defaultLevel))
            {
                throw new ArgumentException("Invaild argument of default level");
            }
            if (parameterNames.Contains(null))
            {
                throw new ArgumentException("item in parameterNames cannot be null.");
            }
            if (parameterNames.Contains(""))
            {
                throw new ArgumentException("item in parameterNames cannot be empty string.");
            }

            this.name = name;
            this.text = text;
            this.parameterNames = new List<string>(parameterNames);
            this.defaultLevel = defaultLevel;
        }
    }
}
