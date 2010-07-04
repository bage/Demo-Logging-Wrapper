/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>Defines helper methods and constants used in this component.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>Thread Safety:
    /// All static methods are thread safe.</p>
    /// </remarks>
    ///
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class Helper
    {
        /// <summary>
        /// <p>Represents a dictionary holding parameter types, initialized in the static constructor.</p>
        /// </summary>
        private static IDictionary supportedTypes = null;

        /// <summary>
        /// <p>Represents the prefix of typed array.</p>
        /// </summary>
        private const string TypedArrayPrefix = "object[];";

        /// <summary>
        /// <p>Private constructor.</p>
        /// </summary>
        private Helper()
        {
        }

        /// <summary>
        /// <p>Static constructor, fills the types dictionary.</p>
        /// </summary>
        static Helper()
        {
            supportedTypes = new Hashtable();

            // set the single types.
            supportedTypes.Add("int", typeof(int));
            supportedTypes.Add("bool", typeof(bool));
            supportedTypes.Add("sbyte", typeof(sbyte));
            supportedTypes.Add("byte", typeof(byte));
            supportedTypes.Add("char", typeof(char));
            supportedTypes.Add("short", typeof(short));
            supportedTypes.Add("ushort", typeof(ushort));
            supportedTypes.Add("uint", typeof(uint));
            supportedTypes.Add("long", typeof(long));
            supportedTypes.Add("ulong", typeof(ulong));
            supportedTypes.Add("float", typeof(float));
            supportedTypes.Add("double", typeof(double));
            supportedTypes.Add("string", typeof(string));
            supportedTypes.Add("object", typeof(object));

            // simple array types
            supportedTypes.Add("int[]", typeof(int[]));
            supportedTypes.Add("bool[]", typeof(bool[]));
            supportedTypes.Add("sbyte[]", typeof(sbyte[]));
            supportedTypes.Add("byte[]", typeof(byte[]));
            supportedTypes.Add("char[]", typeof(char[]));
            supportedTypes.Add("short[]", typeof(short[]));
            supportedTypes.Add("ushort[]", typeof(ushort[]));
            supportedTypes.Add("uint[]", typeof(uint[]));
            supportedTypes.Add("long[]", typeof(long[]));
            supportedTypes.Add("ulong[]", typeof(ulong[]));
            supportedTypes.Add("float[]", typeof(float[]));
            supportedTypes.Add("double[]", typeof(double[]));
            supportedTypes.Add("string[]", typeof(string[]));
            supportedTypes.Add("object[]", typeof(object[]));

            supportedTypes.Add("null", typeof(void));
        }

        /// <summary>
        /// <p>Check if the given type is primitive data type or string.</p>
        /// </summary>
        ///
        /// <param name="type">The type to check.</param>
        ///
        /// <returns>True if the given type is primitive data type or string, otherwise false.</returns>
        public static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }

        /// <summary>
        /// <p>Check if the given type is array of primitive data type or string.</p>
        /// </summary>
        ///
        /// <param name="type">The type to check.</param>
        ///
        /// <returns>True if the given type is array of primitive data type or string, otherwise false.</returns>
        public static bool IsPrimitiveOrStringArray(Type type)
        {
            return type.IsArray && IsPrimitiveOrString(type.GetElementType());
        }

        /// <summary>
        /// <p>Retrieves the type from the given type name.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The type name will be trimmed  by removing blanks and spaces.</p>
        /// </remarks>
        ///
        /// <param name="typeName">The string denotes the name of the type.</param>
        ///
        /// <param name="elementTypeName">
        /// The type name of the element if the given type represents typed array,
        /// otherwise, will be set to null.
        /// </param>
        ///
        /// <returns>The real type parsed from the type name.</returns>
        public static Type RetrieveType(ref string typeName, out string elementTypeName)
        {
            typeName = FormatTypeName(typeName);

            elementTypeName = null;
            if (typeName.StartsWith(TypedArrayPrefix))
            {
                // typed array, "object[];<ElementType>"
                elementTypeName = typeName.Substring(TypedArrayPrefix.Length);

                return typeof(object[]);
            }
            else
            {
                // simple type, null or array
                return (Type)supportedTypes[typeName];
            }
        }

        /// <summary>
        /// <p>Check if parameter is null and throw proper Exception.</p>
        /// </summary>
        ///
        /// <param name="param">Parameter.</param>
        /// <param name="name">Parameter name.</param>
        ///
        /// <exception cref="ArgumentNullException">If param is null.</exception>
        public static void ValidateNotNull(object param, string name)
        {
            if (param == null)
            {
                throw new ArgumentNullException(name, name + " should not be null.");
            }
        }

        /// <summary>
        /// <p>Check if parameter is null or an empty string and throw proper Exception.</p>
        /// </summary>
        ///
        /// <param name="param">Parameter.</param>
        /// <param name="name">Parameter name.</param>
        ///
        /// <exception cref="ArgumentNullException">If param is null.</exception>
        /// <exception cref="ArgumentException">If param is an empty string.</exception>
        public static void ValidateNotNullOrEmpty(string param, string name)
        {
            ValidateNotNull(param, name);
            if (param.Trim() == String.Empty)
            {
                throw new ArgumentException(name + " should not be empty.", name);
            }
        }

        /// <summary>
        /// <p>Trims the name of the type by removing blanks and spaces.</p>
        /// </summary>
        ///
        /// <param name="typeName">The string denotes the name of the type.</param>
        ///
        /// <returns>The name of the type after trimmed .</returns>
        private static string FormatTypeName(string typeName)
        {
            string name = typeName.Substring(typeName.LastIndexOf(':') + 1).Trim();

            int leftBracket = name.IndexOf('[');
            int rightBracket = name.IndexOf(']');

            // the type is not a multiple one as it hasn't left or right bracket or it has a
            // non-right-edged right bracket.
            if (leftBracket == -1 || rightBracket == -1)
            {
                return name;
            }

            // judge whether it has non-blank characters between the brackets.
            for (int i = leftBracket + 1; i < rightBracket; i++)
            {
                if (!char.IsWhiteSpace(name[i]))
                {
                    return name;
                }
            }
            string suffix = name.Substring(rightBracket + 1).Trim();
            if (suffix.StartsWith(";"))
            {
                suffix = ";" + suffix.Substring(1).TrimStart();
            }

            return name.Substring(0, leftBracket).Trim() + "[]" + suffix;
        }
    }
}
