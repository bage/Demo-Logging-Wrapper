/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// Defines helper methods used in this component.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// All static methods are thread safe.
    /// </para>
    /// </threadsafety>
    ///
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    internal static class Helper
    {
        /// <summary>
        /// <para>
        /// Validates an object <paramref name="value"/> for null.
        /// If it is, then <see cref="ArgumentNullException"/> is thrown.
        /// </para>
        /// </summary>
        /// <param name="value">The object reference to check.</param>
        /// <param name="paramName">The name of parameter.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
        internal static void ValidateNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    paramName, "The value of parameter shouldn't be a null reference.");
            }
        }

        /// <summary>
        /// <para>
        /// Validates a string for empty value.
        /// If it is, then <see cref="ArgumentException"/> is thrown.
        /// If <paramref name="value"/> is null, verification is not performed and exception is not thrown.
        /// </para>
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="paramName">The name of parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
        internal static void ValidateNotEmptyString(string value, string paramName)
        {
            if (value != null && value == string.Empty)
            {
                throw new ArgumentException(
                    "The value of parameter shouldn't be an empty.", paramName);
            }
        }

        /// <summary>
        /// <para>
        /// Validates a string for null and empty values.
        /// If it is null, then <see cref="ArgumentNullException"/> is thrown.
        /// If it is empty string, then <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="paramName">The name of parameter.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
        internal static void ValidateNotNullNotEmptyString(string value, string paramName)
        {
            ValidateNotNull(value, paramName);
            ValidateNotEmptyString(value, paramName);
        }

        /// <summary>
        /// <para>
        /// Returns a string value of the attribute from <paramref name="config"/>.
        /// The value should not be null if <paramref name="isRequired"/> is true.
        /// The value should not be empty string.
        /// If some errors occurred, ConfigException is thrown.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="isRequired">The flag to indicate whether attribute is required or not.</param>
        /// <returns>the string value of the <paramref name="name"/>.</returns>
        /// <exception cref="ConfigException">
        /// If configuration fail, or it doesn't contain required attribute, or attribute is empty string
        /// or some type casting errors are occurred.
        /// </exception>
        internal static string GetStringAttribute(IConfiguration config, string name, bool isRequired)
        {
            try
            {
                string value = config.GetSimpleAttribute(name) as string;

                if (isRequired)
                {
                    ValidateNotNull(value, name);
                }

                ValidateNotEmptyString(value, name);

                return value;
            }
            catch (ArgumentNullException e)
            {
                throw new ConfigException(
                    string.Format("Required '{0}' attribute is not set.", e.ParamName), e);
            }
            catch (ArgumentException e)
            {
                throw new ConfigException("Invalid value of attribute " + e.ParamName, e);
            }
            catch (Exception e)
            {
                // duplicate attributes in the config or another errors
                throw new ConfigException("Something wrong with configuration.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Returns a boolean value of the attribute from <paramref name="config"/>.
        /// If value is null - method returns <paramref name="defaultValue"/>.
        /// If some errors occurred - ConfigException is thrown.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to use.</param>
        /// <param name="paramName">The name of the attribute.</param>
        /// <param name="defaultValue">The default value returned when attribute is not found.</param>
        /// <returns>the boolean value of the <paramref name="paramName"/>, or given default value if the
        /// attribute is not found.</returns>
        /// <exception cref="ConfigException">
        /// If configuration fail or some type casting errors are occurred.
        /// </exception>
        internal static bool GetBooleanAttribute(IConfiguration config, string paramName, bool defaultValue)
        {
            try
            {
                object param = config.GetSimpleAttribute(paramName);

                if (param == null)
                {
                    return defaultValue;
                }

                return Convert.ToBoolean(param);
            }
            catch (Exception e)
            {
                // duplicate parameters in the config or other errors
                throw new ConfigException("Something wrong with configuration.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Returns a Enum value of the attribute from <paramref name="config"/>.
        /// If value is null, method returns <paramref name="defaultValue"/>.
        /// If some errors occurred, ConfigException is thrown.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="defaultValue">The default value returned when attribute is not found.</param>
        /// <returns>the Enum value of the <paramref name="name"/>, or given default value if the
        /// attribute is not found.</returns>
        /// <exception cref="ConfigException">
        /// If configuration fail or some type casting errors are occurred.
        /// </exception>
        internal static Level GetLevelAttribute(IConfiguration config, string name, Level defaultValue)
        {
            try
            {
                string value = config.GetSimpleAttribute(name) as string;

                if (value == null)
                {
                    return defaultValue;
                }

                return (Level) Enum.Parse(typeof (Level), value, true);
            }
            catch (Exception e)
            {
                // duplicate attributes in the config or other errors
                throw new ConfigException("Something wrong with configuration.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Returns a list of string values of the attribute from <paramref name="config"/>.
        /// The list should not be null if <paramref name="isRequired"/> is true.
        /// The list should not contains empty strings.
        /// If some errors occurred, ConfigException is thrown.
        /// </para>
        /// </summary>
        /// <param name="config">The configuration object to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="isRequired">The flag to indicate whether attribute is required or not.</param>
        /// <returns>the list of string values of the <paramref name="name"/>.</returns>
        /// <exception cref="ConfigException">
        /// If configuration fail, or it doesn't contain required attribute, or attribute contains empty
        /// strings or some type casting errors are occurred.
        /// </exception>
        internal static IList<string> GetStringListAttribute(IConfiguration config, string name,
            bool isRequired)
        {
            try
            {
                object[] values = config.GetAttribute(name);

                if (isRequired)
                {
                    ValidateNotNull(values, name);
                }

                IList<string> list = new List<string>();

                if (values != null)
                {
                    foreach (object value in values)
                    {
                        string str = value as string;

                        ValidateNotEmptyString(str, name);

                        // add to list
                        list.Add(str);
                    }
                }

                return list;
            }
            catch (ArgumentNullException e)
            {
                throw new ConfigException(
                    string.Format("Required '{0}' attribute is not set.", e.ParamName), e);
            }
            catch (ArgumentException e)
            {
                throw new ConfigException("Invalid value of attribute " + e.ParamName, e);
            }
            catch (Exception e)
            {
                // duplicate attributes in the config or another errors
                throw new ConfigException("Something wrong with configuration.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Validates the given logger is not null and returns the logger. This method is used to perform
        /// non-null checking before passing parameters to constructor of super class.
        /// </para>
        /// </summary>
        /// <param name="logger">The logger to check.</param>
        /// <param name="name">The name of parameter.</param>
        /// <returns>the given logger.</returns>
        /// <exception cref="ArgumentNullException">If logger is null.</exception>
        internal static Logger ValidateLoggerNotNull(Logger logger, string name)
        {
            ValidateNotNull(logger, name);
            return logger;
        }
    }
}