/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>This enumeration defines values that control how long an <see cref="ObjectFactory"/>
    /// instance holds on to (and uses) the object created for a single key.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>This allows the same object instance to appear multiple times in the
    /// object graph generated through nested object instantiations.</p>
    ///
    /// <p>Thread Safety:
    /// Enumerations are thread safe.</p>
    /// </remarks>
    ///
    /// <author>aubergineanode</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal enum InstantiationLifetime
    {
        /// <summary>
        /// <p>Instantiation lifetime that specifies that the object should only be
        /// created once during the lifetime of the Object Factory (or until the
        /// factoryLifetimeObjects map is cleared).</p>
        ///
        /// <p>Future requests for the same key will return the object instance
        /// created on the first request for the key.</p>
        /// </summary>
        Factory,

        /// <summary>
        /// <p>Instantiation lifetime that specifies that the object for the key should
        /// only be created once per application call to the CreateDefinedObject method.</p>
        ///
        /// <p>If the object for the key is needed more than once in the tree of complex object
        /// instantiations (includes both constructors and methods), then the same instance of the
        /// object will be used in all instances. However, on the next application call to
        /// for the object with that key, a new instance will be instantiated.</p>
        /// </summary>
        OncePerTopLevelObject,

        /// <summary>
        /// <p>Instantiation lifetime that specifies that a new object should be created
        /// every time the object for the key is requested.</p>
        /// </summary>
        Instance
    }
}
