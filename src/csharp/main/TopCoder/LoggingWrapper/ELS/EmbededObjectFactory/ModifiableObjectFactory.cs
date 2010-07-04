/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// To support modification and saving of object definitions, the abstract subclass of ObjectFactory is added.
    /// The class provides the API to modify the existing (or create a new) object definition and save it in the object
    /// definition source. It also provides the API to delete the object definitions given their keys.
    /// </summary>
    ///
    /// <remarks>
    /// <p>Thread Safety:
    /// The class is extended from <code>ObjectFactory</code> which is thread safe, and it doesn't add
    /// mutable data member. Thus, this class is thread safe.</p>
    /// </remarks>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.2</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal abstract class ModifiableObjectFactory : ObjectFactory
    {
        /// <summary>
        /// The default constructor for <code>ModifiableObjectFactory</code> class.
        /// </summary>
        protected ModifiableObjectFactory()
        {
            // do nothing
        }

        /// <summary>
        /// Saves the ObjectDefinition instance using the specific key.
        /// </summary>
        /// <param name="key">The specific key to use</param>
        /// <param name="definition">The ObjectDefinition instance to save</param>
        /// <exception cref="ObjectSourceException">
        /// Wraps any implementation specific exceptions that may occur while saving the definition
        /// info but may also indicate incomplete definition info (missing type name), parsing errors
        /// if the source uses string representations or some other implementation specific error.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key is null or definition is null.</exception>
        /// <exception cref="ArgumentException">If the key is empty.</exception>
        public abstract void SaveDefinition(string key, ObjectDefinition definition);

        /// <summary>
        /// Deletes the ObjectDefinition instance with the specific key.
        /// </summary>
        /// <param name="key">The specific key to use</param>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps any implementation specific exceptions that may occur while deleting the definition
        /// info but may also indicate incomplete definition info (missing type name), parsing errors
        /// if the source uses string representations or some other implementation specific error.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key is null.</exception>
        /// <exception cref="ArgumentException">If the key is empty.</exception>
        public abstract void DeleteDefinition(string key);
    }
}
