/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>This is a data class that is used to store object definition data that
    /// is common to constructor and method/property invocations.</p>
    /// </summary>
    ///
    /// <p>Version 1.2: The setter for the each property is added except <code>ParamTypes</code> and <code>
    /// ParamValues</code>.
    /// For <code>ParamTypes</code> and <code>ParamValues</code> properties, a set of operations
    /// AddParam/RemoveParam/InsertParam/RemoveAtParam are added to set them at the same time.
    /// </p>
    ///
    /// <remarks>
    /// <p> It is defined because the definition data contains a large set of information.
    /// That way the exchange of information between the <see cref="ObjectFactory"/> methods
    /// and its implementations are simplified.</p>
    ///
    /// <p>The contained information is: method name, whether the method name is case sensitive,
    /// parameter types and values.</p>
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
    internal abstract class ObjectPartDefinition
    {
        /// <summary>
        /// <p>Represents the name of the method to invoke.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>In the case of ObjectDefinition, this will be the name of the static method
        /// that creates the object (may be null if a constructor is used).</p>
        ///
        /// <p>In the case of MethodCallDefinition, this will be the name of the
        /// method/property to invoke (can not be null).</p>
        ///
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private string methodName = null;

        /// <summary>
        /// <p>Represents a flag indicates whether to ignore the case sensitivity when
        /// looking up the method name (and static method name, for ObjectDefinition).</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Initialized in the constructor. Used by the property getter/setter.</p>
        /// </remarks>
        private bool ignoreCase;

        /// <summary>
        /// <p>Represents the parameter types for the creation of the type.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: Change the underlying implementation to <code>IList</code>.
        /// </p>
        ///
        /// <remarks>
        /// <p>Can be empty (means no arguments are used) but can't be null. Must have the same number
        /// of elements as paramValues. Cannot have null elements.</p>
        ///
        /// <p>Elements can be bool, sbyte, byte, short, ushort, int, uint, long, ulong, char,
        /// float, double, string, object with [] as optional suffix (used for arrays) but can
        /// be any type if specified.</p>
        ///
        /// <p>In the case of object[], the type of the array can follow.</p>
        ///
        /// </remarks>
        /// <since>1.1</since>
        private IList paramTypes;

        /// <summary>
        /// <p>Represents the parameter values for the creation of the type.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: Change the underlying implementation to <code>IList</code>.
        /// </p>
        /// <remarks>
        /// <p>Can be empty (means no arguments are used) but can't be null. Must have the same number of
        /// elements as paramTypes. Can have null elements, but they must correspond to "null" entries
        /// in the paramTypes array.</p>
        ///
        /// <p>For the simple types and strings the elements represent the actual values. For objects,
        /// the elements are the key (string) that can be used to create that object (see sample config
        /// file for an example). For array types, the elements are arrays with elements following the
        /// two rules above. In the case of object[], the type of the array can follow.</p>
        ///
        /// </remarks>
        /// <since>1.1</since>
        private IList paramValues;

        /// <summary>
        /// <p>Property for methodName field.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for this property is added.
        /// </p>
        ///
        /// <remarks>
        /// <p>In the case of ObjectDefinition, this will be the name of the static method
        /// that creates the object (may be null if a constructor is used).</p>
        ///
        /// <p>In the case of MethodCallDefinition, this will be the name of the
        /// method/property to invoke (can not be null).</p>
        /// </remarks>
        ///
        /// <value>The name of the method to invoke.</value>
        /// <exception cref="ArgumentException">if empty string is set to it</exception>
        /// <since>1.1</since>
        public string MethodName
        {
            get
            {
                return methodName;
            }
            set
            {
                if (value != null)
                {
                    Helper.ValidateNotNullOrEmpty(value, "methodName");
                }
                methodName = value;
            }
        }

        /// <summary>
        /// <p>Represents a flag indicates whether to ignore the case sensitivity when
        /// looking up the method name (and static method name, for ObjectDefinition).</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The setter for this property is added.
        /// </p>
        ///
        /// <value>A flag indicates whether to ignore the case sensitivity when looking up the method name.</value>
        /// <since>1.1</since>
        public bool IgnoreCase
        {
            get
            {
                return ignoreCase;
            }
            set
            {
                ignoreCase = value;
            }
        }

        /// <summary>
        /// <p>Gets the parameter values for the creation of the type.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The method to construct string array changed.
        /// </p>
        ///
        /// <remarks>
        /// <p>Can be empty (means no arguments are used) but can't be null. Must have the same number of
        /// elements as paramTypes. Can have null elements, but they must correspond to "null" entries
        /// in the paramTypes array.</p>
        ///
        /// <p>For the simple types and strings the elements represent the actual values. For objects,
        /// the elements are the key (string) that can be used to create that object (see sample config
        /// file for an example). For array types, the elements are arrays with elements following the
        /// two rules above. In the case of object[], the type of the array can follow.</p>
        /// </remarks>
        ///
        /// <value>The parameter values for the creation of the type.</value>
        /// <since>1.1</since>
        public string[] ParamTypes
        {
            get
            {
                return (string[])((ArrayList)paramTypes).ToArray(typeof(string));
            }
        }

        /// <summary>
        /// <p>Gets the parameter values for the creation of the type.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: The method to construct object array changed.
        /// </p>
        ///
        /// <remarks>
        /// <p>Can be empty (means no arguments are used) but can't be null. Must have the same number of
        /// elements as paramTypes. Can have null elements, but they must correspond to "null" entries
        /// in the paramTypes array.</p>
        ///
        /// <p>For the simple types and strings the elements represent the actual values. For objects,
        /// the elements are the key (string) that can be used to create that object (see sample config
        /// file for an example). For array types, the elements are arrays with elements following the
        /// two rules above. In the case of object[], the type of the array can follow.</p>
        /// </remarks>
        ///
        /// <value>The parameter values for the creation of the type.</value>
        /// <since>1.1</since>
        public object[] ParamValues
        {
            get
            {
                return ((ArrayList)paramValues).ToArray();
            }
        }

        /// <summary>
        /// The helper method to set the <code>ParamTypes</code> and <code>ParamValues</code> properties.
        /// </summary>
        /// <param name="paramTypes">The types of the parameters for the constructor/method (can be null).</param>
        /// <param name="paramValues">The values of the parameters for the constructor/method (can be null).</param>
        ///
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or if a null entry in paramValues does not
        /// correspond to a "null" entry in paramTypes.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If an array element (besides an element in paramValues corresponding to a paramType of "null") is null.
        /// </exception>
        private void SetParamTypesAndParamValues(string[] paramTypes, object[] paramValues)
        {
            int typesLength = paramTypes == null ? -1 : paramTypes.Length;
            int valuesLength = paramValues == null ? -1 : paramValues.Length;

            // if the two arrays don't have the same size, throw ArgumentException.
            if (typesLength != valuesLength)
            {
                throw new ArgumentException("The arrays paramTypes and paramValues don't have the same size.");
            }
            for (int i = 0; i < typesLength; ++i)
            {
                try
                {
                    CheckParamTypeValue(ref paramTypes[i], paramValues[i]);
                }
                catch (ArgumentNullException e)
                {
                    throw new ArgumentException("Null reference found in types array.",
                      String.Format("paramTypes[{0}]", i), e);
                }

            }
            this.paramTypes = paramTypes == null ? new ArrayList() : new ArrayList(paramTypes);
            this.paramValues = paramValues == null ? new ArrayList() : new ArrayList(paramValues);

        }

        /// <summary>
        /// <p>Creates a new instance of ObjectPartDefinition.</p>
        /// </summary>
        ///
        /// <p>
        /// Version 1.2: Move parameter verification to property setter and
        /// <code>SetParamTypesAndParamValues(string[], object[])</code>.
        /// </p>
        ///
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the method name.
        /// </param>
        /// <param name="paramTypes">The types of the parameters for the constructor/method (can be null).</param>
        /// <param name="paramValues">
        /// The values of the parameters for the constructor/method (can be null).
        /// </param>
        /// <param name="methodName">
        /// The name of the method to invoke (for ObjectDefinition, this is the name of the static
        /// method to use) (can be null, but not the empty string).
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or if a null entry in paramValues does not
        /// correspond to a "null" entry in paramTypes or methodName is the empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If an array element (besides an element in paramValues corresponding to a paramType of "null") is null.
        /// </exception>
        /// <since>1.1</since>
        protected ObjectPartDefinition(
            bool ignoreCase, string[] paramTypes, object[] paramValues, string methodName)
        {
            this.MethodName = methodName;
            this.ignoreCase = ignoreCase;
            SetParamTypesAndParamValues(paramTypes, paramValues);
        }

        /// <summary>
        /// <p>Creates a new instance of ObjectPartDefinition.</p>
        /// </summary>
        ///
        /// <param name="ignoreCase">
        /// False for case sensitive lookup, true for case insensitive lookup of the method name.
        /// </param>
        /// <param name="paramTypes">The types of the parameters for the constructor/method (can be null).</param>
        /// <param name="paramValues">
        /// The values of the parameters for the constructor/method (can be null).
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// If the paramTypes and paramValues arrays don't have the same size and aren't both null or
        /// if paramTypes has invalid values (see ParamTypes), or if a null entry in paramValues does not
        /// correspond to a "null" entry in paramTypes.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If an array element (besides an element in paramValues corresponding to a paramType of "null") is null.
        /// </exception>
        protected ObjectPartDefinition(bool ignoreCase, string[] paramTypes, object[] paramValues)
            : this(ignoreCase, paramTypes, paramValues, null)
        {
        }

        /// <summary>
        /// Adds parameter type and value pair to each list.
        /// </summary>
        /// <param name="paramType">The type of the parameter for the constructor/method (can be null).</param>
        /// <param name="paramValue">The value of the parameter for the constructor/method (can be null).</param>
        /// <exception cref="ArgumentException">
        /// If paramType has invalid value or if a null paramValue does not correspond to a "null" paramType.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If paramType is null.
        /// </exception>
        /// <returns>
        /// The position into which the new param type and value were inserted.
        /// </returns>
        public int AddParam(string paramType, object paramValue)
        {
            CheckParamTypeValue(ref paramType, paramValue);
            paramTypes.Add(paramType);
            return paramValues.Add(paramValue);
        }

        /// <summary>
        /// Removes parameter type and value pair from each list. The method will remove the first pair
        /// of param type and value if they exist.
        /// </summary>
        /// <param name="paramType">The type of the parameter for the constructor/method (can be null).</param>
        /// <param name="paramValue">The value of the parameter for the constructor/method (can be null).</param>
        ///
        /// <exception cref="ArgumentException">
        /// If paramType has invalid value or if a null paramValue does not correspond to a "null" paramType.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If paramType is null.
        /// </exception>
        public void RemoveParam(string paramType, object paramValue)
        {
            CheckParamTypeValue(ref paramType, paramValue);

            for (int i = 0; i < paramTypes.Count; i++)
            {
                if (paramTypes[i].Equals(paramType) && paramValues[i].Equals(paramValue))
                {
                    paramTypes.RemoveAt(i);
                    paramValues.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Inserts parameter type and value pair into each list at specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which MethodCallDefinition instance should be inserted.</param>
        /// <param name="paramType">The type of the parameter for the constructor/method (can be null).</param>
        /// <param name="paramValue">The value of the parameter for the constructor/method (can be null).</param>
        ///
        /// <exception cref="ArgumentException">
        /// If paramType has invalid value or if a null paramValue does not correspond to a "null" paramType.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If paramType is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If index is less than zero or index is greater than Count
        /// of parameter type/value list</exception>
        public void InsertParam(int index, string paramType, object paramValue)
        {
            CheckParamTypeValue(ref paramType, paramValue);
            paramTypes.Insert(index, paramType);
            paramValues.Insert(index, paramValue);
        }

        /// <summary>
        /// Removes parameter type and value pair from each list at specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which MethodCallDefinition instance should be inserted.</param>
        ///
        /// <exception cref="ArgumentOutOfRangeException">If index is less than zero or index is greater than or
        /// equal to Count of parameter type/value list</exception>
        public void RemoveAtParam(int index)
        {
            paramTypes.RemoveAt(index);
            paramValues.RemoveAt(index);
        }

        /// <summary>
        /// The helper method to verify the paramType and paramValue pair.
        /// </summary>
        /// <param name="paramType">The type of the parameter for the constructor/method (can be null).</param>
        /// <param name="paramValue">The value of the parameter for the constructor/method (can be null).</param>
        /// <exception cref="ArgumentException">
        /// If paramType has invalid value or if a null paramValue does not correspond to a "null" paramType.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If paramType is null.
        /// </exception>
        private void CheckParamTypeValue(ref string paramType, object paramValue)
        {
            Helper.ValidateNotNull(paramType, "paramType");

            string elementType;
            Type type = Helper.RetrieveType(ref paramType, out elementType);
            if (type == null)
            {
                throw new ArgumentException("The type " + paramType + " is invalid.");
            }

            // "null" type must corresponding to null value
            if (paramValue == null && type != typeof(void))
            {
                throw new ArgumentException(
                    "Only 'null' type can specify a null value.");
            }

            if (type == typeof(void) && paramValue != null)
            {
                throw new ArgumentException("Value of 'null' parameter is not null.");
            }
        }
    }
}
