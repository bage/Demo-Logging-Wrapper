/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>This is a data class that is used to store object definition data.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p> It is defined because the definition data contains a large set of information.
    /// That way the exchange of information between the <see cref="ObjectFactory"/>  methods and its
    /// implementations are simplified.</p>
    ///
    /// <p>The contained information is: application domain name, assembly, type name,
    /// whether creation is done through a static factory method, name of the static method
    /// that creates the object, whether the static method name is case sensitive,
    /// methods will apply to the created object, instantiation lifetime of the object,
    /// parameter types and values.
    /// Some of these data is stored in the base class.</p>
    ///
    /// <p>Version 1.1: This class has been refactored so that many of the properties that were
    /// previously in it are now in the base <see cref="ObjectPartDefinition"/> class.</p>
    ///
    /// <p>Version 1.2: The setter for the each property is added except <code>MethodCalls</code>.
    /// For <code>MethodCalls</code> property, RemoveMethodCall/AddMethodCall/InsertMethodCall/RemoveAtMethodCall
    /// operations are added to support modification the content of methodCalls List.
    /// </p>
    ///
    /// <p>Thread Safety:
    /// The class is mutable so it is not thread safe.</p>
    /// </remarks>
    ///
    /// <author>adic</author>
    /// <author>LittleBlack</author>
    /// <author>aubergineanode</author>
    /// <author>nebula.lam</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.2</version>
    /// <since>1.1</since>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class ObjectDefinition : ObjectPartDefinition
    {
        /// <summary>
        /// <p>Represents the assembly name. Can be null (means no assembly will be loaded).</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private string assembly;

        /// <summary>
        /// <p>Represents the type name.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Cannot be null.</p>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private string typeName;

        /// <summary>
        /// <p>Represents a flag indicates whether creation is done through a static factory method.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private bool isStatic;

        /// <summary>
        /// <p>Represents the method calls that should be invoked on the created object.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: Change the underlying implementation to <code>IList</code>.
        /// </p>
        ///
        /// <remarks>
        /// <p>No item in the array can be null. The field itself can be null (considered the same
        /// as a 0-length array, i.e. no methods should be invoked).</p>
        /// </remarks>
        /// <since>1.1</since>
        private IList methodCalls;

        /// <summary>
        /// <p>Represents the instantiation lifetime of the object, which can alter the object creation graph.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private InstantiationLifetime instantiationLifetime;

        /// <summary>
        /// <p>The application domain name.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Can be null (means the current application domain is used).
        /// Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private string appDomain = null;

        /// <summary>
        /// <p>Property for the application domain name. Can be null (means the current application domain is used).</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for the property is added.
        /// </p>
        ///
        /// <value>The application domain name. Can be null  (means the current application domain is used).</value>
        /// <since>1.1</since>
        public string AppDomain
        {
            get
            {
                return appDomain;
            }
            set
            {
                appDomain = value;
            }
        }

        /// <summary>
        /// <p>Property for the assembly name. Can be null if appDomain is null or empty(means no assembly
        /// will be loaded).
        /// </p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for the property is added.
        /// </p>
        ///
        /// <value>The assembly name. Can be null if appDomain is null or empty(means no assembly will be loaded).
        /// </value>
        /// <exception cref="ArgumentNullException">If we set it to null when appDomain is not null or empty string
        /// </exception>
        /// <since>1.1</since>
        public string Assembly
        {
            get
            {
                return assembly;
            }
            set
            {
                if (appDomain != null && appDomain.Trim().Length > 0)
                {
                    Helper.ValidateNotNull(value, "Assembly");
                }
                assembly = value;
            }
        }

        /// <summary>
        /// <p>Property for the type name.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for the property is added.
        /// </p>
        ///
        /// <value>The type name.</value>
        /// <exception cref="ArgumentNullException">
        /// If we set it to null
        /// </exception>
        /// <since>1.1</since>
        public string TypeName
        {
            get
            {
                return typeName;
            }
            set
            {
                Helper.ValidateNotNull(value, "TypeName");
                typeName = value;
            }
        }

        /// <summary>
        /// <p>Property for isStatic field.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>If isStatic is set to false, the <code>MethodName</code> property will be set to null.</p>
        ///
        /// </remarks>
        /// <p>
        /// Version 1.2: The setter for the property is added.
        /// </p>
        ///
        /// <value>A flag indicates whether creation is done through a static factory method.</value>
        /// <since>1.1</since>
        public bool IsStatic
        {
            get
            {
                return isStatic;
            }
            set
            {
                isStatic = value;
                if(isStatic == false)
                {
                    MethodName = null;
                }
            }
        }

        /// <summary>
        /// <p>Property for the method calls that should be invoked on the created object.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The method to construct MethodCallDefinition array changed.
        /// </p>
        ///
        /// <remarks>
        /// <p>No item in the array can be null. The field itself can be null (considered the same
        /// as a 0-length array, i.e. no methods should be invoked).</p>
        ///
        /// </remarks>
        /// <value>The method calls that should be invoked on the created object.</value>
        /// <exception cref="ArgumentException">Method calls array contains null element</exception>
        /// <since>1.1</since>
        public MethodCallDefinition[] MethodCalls
        {
            get
            {
                return (MethodCallDefinition[])((ArrayList)methodCalls).ToArray(typeof(MethodCallDefinition));
            }
            set
            {
                if (value != null)
                {
                    foreach (MethodCallDefinition methodCall in value)
                    {
                        if (methodCall == null)
                        {
                            throw new ArgumentException(
                                "Method calls array contains null element.", "methodCalls");
                        }
                    }
                }
                this.methodCalls = value == null ?
                new ArrayList() : new ArrayList(value);
            }
        }

        /// <summary>
        /// <p>Property for instantiationLifetime field.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for the property is added.
        /// </p>
        ///
        /// <value>The instantiation lifetime of the object, which can alter the object creation graph.</value>
        /// <exception cref="ArgumentException">
        /// If instantiationLifetime is invalid.
        /// </exception>
        /// <since>1.1</since>
        public InstantiationLifetime InstantiationLifetime
        {
            get
            {
                return instantiationLifetime;
            }
            set
            {
                if (!Enum.IsDefined(typeof(InstantiationLifetime), value))
                {
                    throw new ArgumentException(
                        "Given instantiation lifetime is not a valid value.", "InstantiationLifetime");
                }
                instantiationLifetime = value;
            }
        }

        /// <summary>
        /// <p>Creates a new instance of ObjectDefinition.</p>
        /// </summary>
        ///
        /// <param name="appDomain">The application domain (can be null or empty).</param>
        /// <param name="assembly">The assembly name (can be null if appDomain is null or empty).</param>
        /// <param name="typeName">The type name (cannot be null).</param>
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the static method name.
        /// </param>
        /// <param name="paramTypes">
        /// The types of the parameters (can be null or empty).
        /// </param>
        /// <param name="paramValues">
        /// The values of the parameters (can be null or empty).
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If typeName is null or an array element (besides an element in paramValues
        /// corresponding to a paramType of "null") is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or assembly is the empty string.
        /// </exception>
        public ObjectDefinition(string appDomain, string assembly, string typeName,
            bool ignoreCase, string[] paramTypes, object[] paramValues)
            : this(appDomain, assembly, typeName, ignoreCase,
            paramTypes, paramValues, null, InstantiationLifetime.Factory)
        {
        }

        /// <summary>
        /// <p>Creates a new instance of ObjectDefinition.</p>
        /// </summary>
        ///
        /// <param name="appDomain">The application domain (can be null or empty).</param>
        /// <param name="assembly">The assembly name (can be null if appDomain is null or empty).</param>
        /// <param name="typeName">The type name (cannot be null).</param>
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the static method name.
        /// </param>
        /// <param name="paramTypes">
        /// The types of the parameters (can be null or empty).
        /// </param>
        /// <param name="paramValues">
        /// The values of the parameters (can be null or empty).
        /// </param>
        /// <param name="methodCalls">
        /// The method calls definitions to be made on the object after it is create (can be null).
        /// </param>
        /// <param name="instantiationLifetime">
        /// InstantiationLifetime value stating the length of the lifetime of this instance.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If typeName is null or an array element (besides an element in paramValues corresponding
        /// to a paramType of "null") is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or assembly is the empty string
        /// or instantiationLifetime is invalid.
        /// </exception>
        public ObjectDefinition(string appDomain, string assembly, string typeName,
            bool ignoreCase, string[] paramTypes, object[] paramValues, MethodCallDefinition[] methodCalls,
            InstantiationLifetime instantiationLifetime)
            : this(appDomain, assembly, typeName, null, ignoreCase,
            instantiationLifetime, paramTypes, paramValues, methodCalls)
        {
            isStatic = false;
        }

        /// <summary>
        /// <p>Creates a new instance of ObjectDefinition.</p>
        /// </summary>
        ///
        /// <param name="appDomain">The application domain (can be null or empty).</param>
        /// <param name="assembly">The assembly name (can be null if appDomain is null or empty).</param>
        /// <param name="typeName">The type name (cannot be null).</param>
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the static method name.
        /// </param>
        /// <param name="paramTypes">
        /// The types of the parameters (can be null or empty).
        /// </param>
        /// <param name="paramValues">
        /// The values of the parameters (can be null or empty).
        /// </param>
        /// <param name="methodCalls">
        /// The method calls definitions to be made on the object after it is create (can be null).
        /// </param>
        /// <param name="instantiationLifetime">
        /// InstantiationLifetime value stating the length of the lifetime of this instance.
        /// </param>
        /// <param name="methodName">The name of the static method to use to create the object.</param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If typeName or methodName is null or an array element (besides an element in paramValues
        /// corresponding to a paramType of "null") is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or assembly is the empty string
        /// or instantiationLifetime is invalid or methodName is empty.
        /// </exception>
        public ObjectDefinition(string appDomain, string assembly, string typeName, string methodName,
            bool ignoreCase, string[] paramTypes, object[] paramValues, MethodCallDefinition[] methodCalls,
            InstantiationLifetime instantiationLifetime)
            : this(appDomain, assembly, typeName, methodName, ignoreCase, instantiationLifetime,
            paramTypes, paramValues, methodCalls)
        {
            Helper.ValidateNotNull(methodName, "methodName");
            isStatic = true;
        }

        /// <summary>
        /// <p>Creates a new instance of ObjectDefinition.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: Move part of parameter verification to property setter.
        /// </p>
        ///
        /// <param name="appDomain">The application domain (can be null or empty).</param>
        /// <param name="assembly">The assembly name (can be null if appDomain is null or empty).</param>
        /// <param name="typeName">The type name (cannot be null).</param>
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the static method name.
        /// </param>
        /// <param name="paramTypes">
        /// The types of the parameters (can be null or empty).
        /// </param>
        /// <param name="paramValues">
        /// The values of the parameters (can be null or empty).
        /// </param>
        /// <param name="methodCalls">
        /// The method calls definitions to be made on the object after it is create (can be null).
        /// </param>
        /// <param name="instantiationLifetime">
        /// InstantiationLifetime value stating the length of the lifetime of this instance.
        /// </param>
        /// <param name="methodName">The name of the static method to use to create the object.</param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If typeName or methodName is null or an array element (besides an element in paramValues
        /// corresponding to a paramType of "null") is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or assembly is the empty string
        /// or instantiationLifetime is invalid or methodName is empty.
        /// </exception>
        /// <since>1.1</since>
        private ObjectDefinition(string appDomain, string assembly, string typeName,
            string methodName, bool ignoreCase, InstantiationLifetime instantiationLifetime,
            string[] paramTypes, object[] paramValues, MethodCallDefinition[] methodCalls)
            : base(ignoreCase, paramTypes, paramValues, methodName)
        {


            this.AppDomain = appDomain;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.InstantiationLifetime = instantiationLifetime;
            this.MethodCalls = methodCalls;
        }

        /// <summary>
        /// Adds a MethodCallDefinition instance to method calls list.
        /// </summary>
        /// <returns>The position into which the new MethodCallDefinition instance was inserted.</returns>
        ///
        /// <param name="methodCallDefinition">The MethodCallDefinition instance to add</param>
        /// <exception cref="ArgumentNullException">If the MethodCallDefinition instance to add is null</exception>
        public int AddMethodCallDefinition(MethodCallDefinition methodCallDefinition)
        {
            Helper.ValidateNotNull(methodCallDefinition, "methodCallDefinition");
            return methodCalls.Add(methodCallDefinition);

        }

        /// <summary>
        /// Removes a MethodCallDefinition instance from the method calls list.
        /// </summary>
        /// <param name="methodCallDefinition">The MethodCallDefinition instance to remove</param>
        /// <exception cref="ArgumentNullException">If the MethodCallDefinition instance to remove is null</exception>
        public void RemoveMethodCallDefinition(MethodCallDefinition methodCallDefinition)
        {
            Helper.ValidateNotNull(methodCallDefinition, "methodCallDefinition");
            methodCalls.Remove(methodCallDefinition);
        }

        /// <summary>
        /// Inserts a MethodCallDefinition instance into the method calls list at specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which MethodCallDefinition instance should be inserted.</param>
        /// <param name="methodCallDefinition">The MethodCallDefinition instance to insert</param>
        /// <exception cref="ArgumentNullException">If the MethodCallDefinition instance to insert is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If index is less than zero or index is greater than Count
        /// of method calls list</exception>
        public void InsertMethodCallDefinition(int index, MethodCallDefinition methodCallDefinition)
        {
            Helper.ValidateNotNull(methodCallDefinition, "methodCallDefinition");
            methodCalls.Insert(index, methodCallDefinition);
        }

        /// <summary>
        /// Removes a MethodCallDefinition instance from the method calls list at specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which MethodCallDefinition instance should be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">If index is less than zero or index is greater than or
        /// equal to Count of method calls list</exception>
        public void RemoveAtMethodCallDefinition(int index)
        {
            methodCalls.RemoveAt(index);
        }
    }
}
