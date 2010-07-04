/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>This is a data class that is used to store object definition data for
    /// method/property invocations.</p>
    /// </summary>
    ///
    /// <p>
    /// <i>Version 1.2 : </i>
    /// The setter for the <code>IsProperty</code> property is added.
    /// The getter/setter for <code>MethodName</code> property is added.
    /// </p>
    ///
    /// <remarks>
    /// <p> It is defined because the definition data contains a large set of information.
    /// That way the exchange of information between the <see cref="ObjectFactory"/> methods and its
    /// implementations are simplified.</p>
    ///
    /// <p>This class represents the invocation of a method/property.</p>
    ///
    /// <p>The contained information is: method name, whether the method name is case sensitive,
    /// whether the item to invoke is a property or a method, parameter types and values.
    /// Some of these data is stored in the base class.</p>
    ///
    /// <p>Thread Safety:
    /// The class is mutable so it is not thread safe.</p>
    /// </remarks>
    ///
    /// <author>aubergineanode</author>
    /// <author>nebula.lam</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.2</version>
    /// <since>1.1</since>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class MethodCallDefinition : ObjectPartDefinition
    {
        /// <summary>
        /// <p>Represents a flag indicating whether the "method" to be invoked
        /// is a method or a property. True for properties, and false for methods.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private bool isProperty;

        /// <summary>
        /// <p>Property for isProperty field. True for properties, and false for methods.</p>
        /// </summary>
        ///
        /// <p>
        /// <i>Version 1.2 : </i> The setter for this property is added.
        /// </p>
        ///
        /// <value>
        /// A flag indicating whether the "method" to be invoked is a method or
        /// a property. True for properties, and false for methods.
        /// </value>
        ///
        /// <since>1.1</since>
        public bool IsProperty
        {
            get
            {
                return isProperty;
            }
            set
            {
                isProperty = value;
            }
        }

        /// <summary>
        /// <p>Property for methodName field.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>It is the name of the method/property to invoke (can not be null).</p>
        /// </remarks>
        ///
        /// <value>The name of the method to invoke.</value>
        /// <exception cref="ArgumentException">if empty string is set to it</exception>
        /// <exception cref="ArgumentNullException">if null string is set to it</exception>
        public new string MethodName
        {
            get
            {
                return base.MethodName;
            }
            set
            {
                Helper.ValidateNotNullOrEmpty(value, "methodName");
                base.MethodName = value;
            }
        }

        /// <summary>
        /// <p>Creates a new instance of MethodCallDefinition.</p>
        /// </summary>
        ///
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the method name.
        /// </param>
        /// <param name="paramTypes">The types of the parameters for the method (can be null).</param>
        /// <param name="paramValues">The values of the parameters for the method (can be null).</param>
        /// <param name="methodName">
        /// The name of the method to invoke (for ObjectDefinition, this is the name of the static method to use).
        /// </param>
        /// <param name="isProperty">Whether the method is a normal method or a property.</param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If methodName is null or an array element (besides an element in paramValues
        /// corresponding to a paramType of "null") is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or if a null entry in paramValues does not
        /// correspond to a "null" entry in paramTypes or methodName is the empty string.
        /// </exception>
        public MethodCallDefinition(string methodName, bool isProperty,
            bool ignoreCase, string[] paramTypes, object[] paramValues) :
            base(ignoreCase, paramTypes, paramValues, methodName)
        {
            Helper.ValidateNotNull(methodName, "methodName");
            this.isProperty = isProperty;
        }
    }
}
