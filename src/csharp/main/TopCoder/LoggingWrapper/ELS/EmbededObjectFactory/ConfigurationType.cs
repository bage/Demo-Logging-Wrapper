/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

// For xml tags
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <para>
    /// This is enumeration of configuration hierarchical structure type.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// There are two kinds of hierarchical structure in <see cref="IConfiguration"/>
    /// that are supported by this class.
    /// </para>
    ///
    /// <para>
    /// The first one is FLAT hierarchical structure, which is nearly the same with
    /// the structure of configuration file in <b>ConfigurationObjectFactory</b>
    /// which uses ConfigManager to load the configuration source from file, except
    /// that the node text is changed to be attribute value of "node_value" attribute
    /// (I.e. <c>&lt;value&gt;[node text]&lt;/value&gt;</c> is changed to be
    /// <c>&lt;value_n node_value = "[node text]" /&gt;</c>, because the <see cref="IConfiguration"/>
    /// doesn't have the "node text" concept).
    /// </para>
    ///
    /// <para>
    /// Another is NESTED hierarchical structure, which is newly defined in this component
    /// to fully use the nested property of Configuration API component. NESTED hierarchical
    /// structure is encouraged for newly designed/developed component in that it is better
    /// structured and more readable.
    /// </para>
    /// </remarks>
    ///
    /// <threadsafety>
    /// Enumeration is thread safe.
    /// </threadsafety>
    ///
    /// <author>justforplay</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal enum ConfigurationType
    {
        /// <summary>
        /// <para>Represents the FLAT hierarchical structure, which is nearly the same
        /// with the structure of configuration file in <b>ConfigurationObjectFactory</b>
        /// which uses ConfigManager to load the configuration source from file.</para>
        /// </summary>
        Flat,

        /// <summary>
        /// <para>Represents the NESTED hierarchical structure, which is newly defined in this
        /// component to fully use the nested property of Configuration API component.</para>
        /// </summary>
        ///
        /// <remarks>
        /// This NESTED structure is encouraged to be used if not for the reuse of
        /// pre-existed configuration file that is designed to be loaded using.
        /// </remarks>
        Nested
    }
}
