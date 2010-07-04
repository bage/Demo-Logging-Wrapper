/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Security.Policy;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <p>The ObjectFactory class provides a dynamic object instantiation functionality.</p>
    /// </summary>
    ///
    /// <remarks>
    /// <p>This class provides a very large number of method overloads that allow all kinds of
    /// object instantiations to be done. Here are the possible instantiation options:</p>
    /// <p>an optional application domain (AppDomain) can be specified to create the object into;
    /// an optional new application domain name can be created for the object;
    /// the type can be passed as a Type or as a type name;
    /// optionally the assembly to load it from can be given;
    /// a list of parameters can be given;
    /// all the above info can be taken from an external definition source based on a key
    /// (for example a property name for a configuration file based definition source);
    /// there are also overloads for rarely used reflection options allowing access to the full
    /// potential of reflection.</p>
    /// <p>All this capabilities are implemented using 15 static and 9 non-static methods overloads
    /// for different combinations between the above options.</p>
    ///
    /// <p>This class uses an abstract factory pattern. It defines an abstract method for object
    /// definition retrieval. This isolates the reflection logic from the definition info retrieval.
    /// The concrete implementations will have to implement only ONE method. There are 4 static
    /// factory methods that make the access to the object factories easier and pluggable through
    /// the use of a configuration file. This configuration file allows to associate a name with
    /// a definition sources. By changing the configuration file a totally different factory can
    /// be used with no code changes.</p>
    ///
    /// <p>Version 1.1: This class now supports invocation of methods on the created objects
    /// (see the CreateDefinedObject overloads and ApplyMethod). It also supports various
    /// instantiation lifetimes of objects</p>
    ///
    /// <p>
    /// <i>Version 1.2 : </i>
    /// Visibility of GetDefinition(string) is changed to public from protected.
    /// </p>
    /// <p>Thread Safety:
    /// The factoryLifetimesObjects map is synchronized. Thus, this class is thread safe.</p>
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
    internal abstract class ObjectFactory
    {
        /// <summary>
        /// <p>Represents the default string.</p>
        /// </summary>
        private const string DefaultString = "default";

        /// <summary>
        /// <p>Represents the default namespace.</p>
        /// </summary>
        private const string DefaultNamespace = "TopCoder.LoggingWrapper.ELS.EmbededObjectFactory";

        /// <summary>
        /// <p>>Represents the const default ignore case.</p>
        /// </summary>
        private const bool DefaultIgnoreCase = false;

        /// <summary>
        /// <p>Represents the map (from string keys to object instances) of objects that have been loaded
        /// and have the 'Factory' instantiation lifetime.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>All keys in the dictionary are non-null, non-empty strings. A value in the map can be
        /// of any type, but can not be null.</p>
        ///
        /// <p>This field is added to and accessed in the GetDefinedObject method (and perhaps also
        /// in developer added methods), and the contents can be cleared through the ClearFactoryLifetimeObjects
        /// method. Otherwise, entries can only be added to the dictionary.</p>
        ///
        /// <p>The dictionary field itself will not change or even be null.</p>
        /// </remarks>
        private readonly IDictionary factoryLifetimeObjects = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// <p>Creates a new instance of ObjectFactory.</p>
        /// </summary>
        protected ObjectFactory()
        {
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and an assembly file to use and creates
        /// the object into a new application domain with a given name.</p>
        /// </remarks>
        ///
        /// <param name="appDomain">The application domain name to create.</param>
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If any argument is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If any argument is empty.
        /// </exception>
        public static object CreateObject(string appDomain, string assembly, string type)
        {
            return CreateObject(appDomain, assembly, type, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and an assembly file to use and creates
        /// the object into a new application domain with a given name.</p>
        /// </remarks>
        ///
        /// <param name="appDomain">The application domain name to create.</param>
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If appDomain, assembly or type is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If appDomain, assembly or type is empty.
        /// </exception>
        public static object CreateObject(string appDomain, string assembly, string type, object[] parameters)
        {
            return CreateObject(appDomain, assembly, type,
                parameters, DefaultIgnoreCase, GetBindingFlags(DefaultIgnoreCase), null, null,
                null, null, null, null, false);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and a parameter list to use. It creates the object
        /// into a new application domain (name is given). It also specifies a lot of rarely used
        /// reflection parameters. This overload is defined so that all reflection options are accessible.</p>
        /// </remarks>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appBasePath">
        /// The base directory that the assembly resolver uses to probe for assemblies (see .NET documentation).
        /// </param>
        /// <param name="appDomain">The new application domain name to create.</param>
        /// <param name="appRelativeSearchPath">
        /// The path relative to the base directory where the assembly resolver should probe for
        /// private assemblies (see .NET documentation).
        /// </param>
        /// <param name="assembly">The assembly to load.</param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of
        /// members, and retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for
        /// the type constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <param name="shadowCopyFiles">
        /// If true, a shadow copy of an assembly is loaded into this application domain (see .NET documentation).
        /// </param>
        /// <param name="type">The type name of the object to create.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If the appDomain, type or assembly arguments is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the appDomain, type or assembly arguments is empty.
        /// </exception>
        public static object CreateObject(string appDomain, string assembly, string type,
            object[] parameters, bool ignoreCase, BindingFlags bindingAttr, Binder binder, CultureInfo culture,
            object[] activationAttributes, Evidence securityAttributes, string appBasePath,
            string appRelativeSearchPath, bool shadowCopyFiles)
        {
            Helper.ValidateNotNullOrEmpty(appDomain, "appDomain");

            AppDomain domain = CreateDomain(
                appDomain, securityAttributes, appBasePath, appRelativeSearchPath, shadowCopyFiles);

            return CreateObject(domain, assembly, type, parameters, ignoreCase, bindingAttr, binder,
                culture, activationAttributes, securityAttributes);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and an assembly file to use and creates
        /// the object into a given application domain.</p>
        /// </remarks>
        ///
        /// <param name="appDomain">The application domain.</param>
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        /// <exception cref="ArgumentException">If assembly or type is empty.</exception>
        public static object CreateObject(AppDomain appDomain, string assembly, string type)
        {
            return CreateObject(appDomain, assembly, type, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name, a parameter list and an assembly file to use and
        /// creates the object into a given application domain.</p>
        /// </remarks>
        ///
        /// <param name="appDomain">The application domain.</param>
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain, assembly or type is null.</exception>
        /// <exception cref="ArgumentException">If assembly or type is empty.</exception>
        public static object CreateObject(AppDomain appDomain, string assembly, string type, object[] parameters)
        {
            return CreateObject(appDomain, assembly, type, parameters, DefaultIgnoreCase,
                GetBindingFlags(DefaultIgnoreCase), null, null, null, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and a parameter list to use. It creates the
        /// object into a new application domain (name is given). It also specifies a lot of
        /// rarely used reflection parameters. This overload is defined so that all reflection
        /// options are accessible.</p>
        /// </remarks>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appDomain">The new application domain name to create.</param>
        /// <param name="assembly">The assembly to load.</param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and retrieval
        /// of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the type
        /// constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization.</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the appDomain, type or assembly is null.</exception>
        /// <exception cref="ArgumentException">If the type or assembly is empty.</exception>
        public static object CreateObject(AppDomain appDomain, string assembly, string type, object[] parameters,
            bool ignoreCase, BindingFlags bindingAttr, Binder binder, CultureInfo culture,
            object[] activationAttributes, Evidence securityAttributes)
        {
            Helper.ValidateNotNull(appDomain, "appDomain");
            Helper.ValidateNotNullOrEmpty(assembly, "assembly");
            Helper.ValidateNotNullOrEmpty(type, "type");

            try
            {
                return appDomain.CreateInstanceFromAndUnwrap(assembly, type, ignoreCase, bindingAttr, binder,
                    parameters, culture, activationAttributes, securityAttributes);
            }
            catch (Exception inner)
            {
                throw new ObjectCreationException(
                          String.Format("Failed to create object type [{0}].", type), inner);
            }
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition. It also specifies a new
        /// application domain to create the object into.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        /// <param name="appDomain">The application domain.</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If appDomain or key is empty.</exception>
        public object CreateDefinedObject(string appDomain, string key)
        {
            return CreateDefinedObject(appDomain, key, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition and also a parameter list which
        /// OVERRIDES the parameter list from the object definition for the given key.
        /// It also specifies a new application domain to create the object into.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        /// <param name="appDomain">The application domain.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If appDomain or key is empty.</exception>
        public object CreateDefinedObject(string appDomain, string key, object[] parameters)
        {
            return CreateDefinedObject(appDomain, key, parameters,
                DefaultIgnoreCase, GetBindingFlags(DefaultIgnoreCase),
                null, null, null, null, null, null, false);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition, the parameter list which OVERRIDES
        /// the parameter list from the object definition for the given key and a lot of rarely used reflection
        /// options. It also specifies a new application domain to create the object into.</p>
        /// </remarks>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appBasePath">
        /// The base directory that the assembly resolver uses to probe for assemblies (see .NET documentation).
        /// </param>
        /// <param name="appDomain">The new application domain name.</param>
        /// <param name="appRelativeSearchPath">
        /// The path relative to the base directory where the assembly resolver should probe for
        /// private assemblies (see .NET documentation).
        /// </param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and retrieval
        /// of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="key">The key for the object definition.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <param name="shadowCopyFiles">
        /// If true, a shadow copy of an assembly is loaded into this application domain (see .NET documentation).
        /// </param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If appDomain or key is empty.</exception>
        public object CreateDefinedObject(string appDomain, string key, object[] parameters, bool ignoreCase,
            BindingFlags bindingAttr, Binder binder, CultureInfo culture, object[] activationAttributes,
            Evidence securityAttributes, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
        {
            // create app domain with given parameters
            AppDomain appDomainCreated = CreateDomain(
                appDomain, securityAttributes, appBasePath, appRelativeSearchPath, shadowCopyFiles);

            return CreateDefinedObject(appDomainCreated, key, parameters, ignoreCase, bindingAttr,
                binder, culture, activationAttributes, securityAttributes);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition. It also specifies
        /// the application domain to create the object into.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        /// <param name="appDomain">The application domain.</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If key is empty.</exception>
        public object CreateDefinedObject(AppDomain appDomain, string key)
        {
            return CreateDefinedObject(appDomain, key, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition and also a parameter list which
        /// OVERRIDES the parameter list from the object definition for the given key. It also specifies
        /// the application domain to create the object into.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        /// <param name="appDomain">The application domain.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If key is empty.</exception>
        public object CreateDefinedObject(AppDomain appDomain, string key, object[] parameters)
        {
            return CreateDefinedObject(appDomain, key, parameters, DefaultIgnoreCase,
                GetBindingFlags(DefaultIgnoreCase), null, null, null, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection. The definition information is taken from an
        /// implementation specific source (could be a configuration file, could be a database, etc).</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition but also a parameter list.
        /// This parameter list OVERRIDES the parameter list that may be specified in the object definition
        /// for the given key. There are also a lot of rarely used reflection options that can be specified.
        /// The object is created into the given application domain.</p>
        /// </remarks>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appDomain">The application domain to create into.</param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of/ members, and
        /// retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="key">The key for the object definition.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or could
        /// signal an implementation specific problem (missing property, parse error, etc)
        /// </exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If appDomain or key is null.</exception>
        /// <exception cref="ArgumentException">If key is empty.</exception>
        public object CreateDefinedObject(AppDomain appDomain, string key, object[] parameters, bool ignoreCase,
            BindingFlags bindingAttr, Binder binder, CultureInfo culture, object[] activationAttributes,
            Evidence securityAttributes)
        {
            return CreateDefinedObject(appDomain, key, parameters, ignoreCase, bindingAttr, binder,
                culture, activationAttributes, securityAttributes, new Hashtable(), new Hashtable());
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name.</p>
        /// </remarks>
        ///
        /// <param name="type">The type of the object to create.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the type is null.</exception>
        /// <exception cref="ArgumentException">If the type is empty.</exception>
        public static object CreateObject(string type)
        {
            return CreateObject(type, (object[])null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and a parameter list to use.</p>
        /// </remarks>
        ///
        /// <param name="type">The type name of the object to create.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the type is null.</exception>
        /// <exception cref="ArgumentException">If the type is empty.</exception>
        public static object CreateObject(string type, object[] parameters)
        {
            return CreateObject(GenerateType(type, null, false, DefaultIgnoreCase), parameters);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and a parameter list to use. It also specifies
        /// a lot of rarely used reflection parameters. This overload is defined so that all reflection
        /// options are accessible.</p>
        /// </remarks>
        ///
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and
        /// retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the type constructor
        /// (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the corresponding
        /// element in the types array (see .NET documentation).
        /// </param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="callConvention">
        /// The CallingConventions object that specifies the set of rules to use regarding the order and layout
        /// of arguments, how the return value is passed, what registers are used for arguments, and the stack
        /// is cleaned up (see .NET documentation).
        /// </param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the type is null.</exception>
        /// <exception cref="ArgumentException">If the type is empty.</exception>
        public static object CreateObject(string type, object[] parameters, bool ignoreCase,
            BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
            ParameterModifier[] modifiers, CultureInfo culture)
        {
            return CreateObject(GenerateType(type, null, false, ignoreCase),
                parameters, bindingAttr, binder, callConvention, modifiers, culture);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name and an assembly file to use.</p>
        /// </remarks>
        ///
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        /// <exception cref="ArgumentException">If any argument is empty string.</exception>
        public static object CreateObject(string assembly, string type)
        {
            return CreateObject(assembly, type, (object[])null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a type name, a parameter list and an assembly file to use.</p>
        /// </remarks>
        ///
        /// <param name="assembly">The assembly file to use.</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If assembly or type is null.</exception>
        /// <exception cref="ArgumentException">If assembly or type is empty string.</exception>
        public static object CreateObject(string assembly, string type, object[] parameters)
        {
            return CreateObject(GenerateType(type, assembly, true, DefaultIgnoreCase), parameters);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection. This overload specifies a type name and a parameter
        /// list to use. It also specifies a lot of rarely used reflection parameters. This overload is defined
        /// so that all reflection options are accessible.</p>
        /// </summary>
        ///
        /// <param name="assembly">The assembly to load.</param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and retrieval
        /// of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="callConvention">
        /// The CallingConventions object that specifies the set of rules to use regarding the order and layout of
        /// arguments, how the return value is passed, what registers are used for arguments, and the stack
        /// is cleaned up (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the corresponding
        /// element in the types array (see .NET documentation).
        /// </param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="type">The type name of the object to create.</param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If assembly or type is null.</exception>
        /// <exception cref="ArgumentException">If assembly or type is empty string.</exception>
        public static object CreateObject(string assembly, string type, object[] parameters,
            bool ignoreCase, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
            ParameterModifier[] modifiers, CultureInfo culture)
        {
            return CreateObject(GenerateType(type, assembly, true, ignoreCase),
                parameters, bindingAttr, binder, callConvention, modifiers, culture);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies only a Type.</p>
        /// </remarks>
        ///
        /// <param name="type">The type of the object to create.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the argument is null.</exception>
        public static object CreateObject(Type type)
        {
            return CreateObject(type, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a Type and a parameter list to use.</p>
        /// </remarks>
        ///
        /// <param name="type">The type of the object to create.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the type argument is null.</exception>
        public static object CreateObject(Type type, object[] parameters)
        {
            return CreateObject(type, parameters, GetBindingFlags(false), null, CallingConventions.Any, null, null);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This overload specifies a Type and a parameter list to use. It also specifies
        /// a lot of rarely used reflection parameters. This overload is defined so that all
        /// reflection options are accessible.</p>
        /// </remarks>
        ///
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members,
        /// and retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the type
        /// constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the
        /// corresponding element in the types array (see .NET documentation).
        /// </param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="type">The type of the object to create.</param>
        /// <param name="callConvention">
        /// The CallingConventions object that specifies the set of rules to use regarding the order and layout
        /// of arguments, how the return value is passed, what registers are used for arguments, and the stack
        /// is cleaned up (see .NET documentation).
        /// </param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the type argument is null.</exception>
        public static object CreateObject(Type type, object[] parameters, BindingFlags bindingAttr,
            Binder binder, CallingConventions callConvention, ParameterModifier[] modifiers, CultureInfo culture)
        {
            Helper.ValidateNotNull(type, "type");

            // set the default value of the binder and parameters.
            if (binder == null)
            {
                binder = Type.DefaultBinder;
            }
            if (parameters == null)
            {
                parameters = new object[0];
            }

            try
            {
                // get constructors match with the given CallingConventions
                MethodBase[] candidates = FilterMethods(type.GetConstructors(bindingAttr), callConvention);

                object state = null;
                object[] args = parameters;
                ConstructorInfo ctor = binder.BindToMethod(bindingAttr, candidates,
                    ref args, modifiers, culture, null, out state) as ConstructorInfo;

                // invoke by the given array to insure ref/out parameters work correctly
                return ctor.Invoke(parameters);
            }
            catch (Exception e)
            {
                throw new ObjectCreationException(
                    String.Format("Failed to create object of type [{0}].", type), e);
            }
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies only the key for the object definition.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or
        /// could signal an implementation specific problem (missing property, parse error, etc).
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key argument is null.</exception>
        /// <exception cref="ArgumentException">If the key argument is empty string.</exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        public object CreateDefinedObject(string key)
        {
            return CreateDefinedObject(key, null, null, null, null, CallingConventions.Any,
                null, null, null, null, null, null, false, new Hashtable(), new Hashtable(), false);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition but also a parameter list.
        /// This parameter list OVERRIDES the parameter list that may be specified in the object definition
        /// for the given key.</p>
        /// </remarks>
        ///
        /// <param name="key">The key for the object definition.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or
        /// could signal an implementation specific problem (missing property, parse error, etc).
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key argument is null.</exception>
        /// <exception cref="ArgumentException">If the key argument is empty string.</exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        public object CreateDefinedObject(string key, object[] parameters)
        {
            return CreateDefinedObject(key, parameters, null, null, null, CallingConventions.Any,
                null, null, null, null, null, null, false, new Hashtable(), new Hashtable(), true);
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>The definition information is taken from an implementation specific source
        /// (could be a configuration file, could be a database, etc).</p>
        ///
        /// <p>This overload specifies the key for the object definition but also a parameter list.
        /// This parameter list OVERRIDES the parameter list that may be specified in the object definition
        /// for the given key. There are also a lot of rarely used reflection options that can be specified.</p>
        /// </remarks>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appBasePath">
        /// The base directory that the assembly resolver uses to probe for assemblies (see .NET documentation).
        /// </param>
        /// <param name="appRelativeSearchPath">
        /// The path relative to the base directory where the assembly resolver should probe
        /// for private assemblies (see .NET documentation).
        /// </param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and
        /// retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="callConvention">
        /// The CallingConventions flags that specifies the set of rules to use regarding the order
        /// and layout of arguments, how the return value is passed, what registers are used for arguments,
        /// and the stack is cleaned up (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="key">The key for the object definition.</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the
        /// corresponding element in the types array (see .NET documentation).
        /// </param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <param name="shadowCopyFiles">
        /// If true, a shadow copy of an assembly is loaded into this application domain (see .NET documentation).
        /// </param>
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or
        /// could signal an implementation specific problem (missing property, parse error, etc).
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key argument is null.</exception>
        /// <exception cref="ArgumentException">If the key argument is empty string.</exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        public object CreateDefinedObject(string key, object[] parameters, bool ignoreCase, BindingFlags bindingAttr,
            Binder binder, CallingConventions callConvention, ParameterModifier[] modifiers, CultureInfo culture,
            object[] activationAttributes, Evidence securityAttributes, string appBasePath,
            string appRelativeSearchPath, bool shadowCopyFiles)
        {
            return CreateDefinedObject(key, parameters, ignoreCase, bindingAttr, binder, callConvention,
                modifiers, culture, activationAttributes, securityAttributes, appBasePath, appRelativeSearchPath,
                shadowCopyFiles, new Hashtable(), new Hashtable(), true);
        }

        /// <summary>
        /// <p>Retrieves the object definition info with the given key from a definition source.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This method is abstract and each subclass will implement it differently.
        /// A configuration file based implementation will look into configuration files.
        /// A database implementation will look into database tables and so on.
        /// The definition info will be returned into an ObjectDefinition.</p>
        /// </remarks>
        ///
        /// <p>
        /// <i>Version 1.2 : </i> Visibility of GetDefinition(string) is changed to public from protected.
        /// </p>
        ///
        /// <param name="key">The key to use.</param>
        ///
        /// <returns>The object definition info.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps any implementation specific exceptions that may occur while retrieving the definition
        /// info but may also indicate incomplete definition info (missing type name), parsing errors
        /// if the source uses string representations or some other implementation specific errors.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key is null.</exception>
        /// <exception cref="ArgumentException">If the key is empty.</exception>
        /// <since>1.1</since>
        public abstract ObjectDefinition GetDefinition(string key);

        /// <summary>
        /// <p>Applies the given method definition to the given object.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This method will invoke either the method specified by the definition,
        /// or set the property specified by the definition.</p>
        /// </remarks>
        ///
        /// <param name="applyMethodsTo">The object to invoke the method on.</param>
        /// <param name="methodCallDefinition">The definition of the method to invoke.</param>
        ///
        /// <exception cref="MethodInvocationException">If there is an error in invoking the method.</exception>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        public void ApplyMethodCall(object applyMethodsTo, MethodCallDefinition methodCallDefinition)
        {
            Helper.ValidateNotNull(applyMethodsTo, "applyMethodsTo");
            Helper.ValidateNotNull(methodCallDefinition, "methodCallDefinition");

            try
            {
                ApplyMethodCall(applyMethodsTo, methodCallDefinition, new Hashtable(), new Hashtable());
            }
            catch (Exception e)
            {
                throw new MethodInvocationException("Error occurred while invoking the method.", e);
            }
        }

        /// <summary>
        /// <p>Clears all 'Factory' instantiation lifetime objects that have been loaded.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>When an object key with the 'Factory' instantiation lifetime (which previously had
        /// an entry in the dictionary) is next requested, a new instance will be created.</p>
        /// </remarks>
        public void ClearFactoryLifetimeObjects()
        {
            factoryLifetimeObjects.Clear();
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection.</p>
        /// </summary>
        ///
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="appBasePath">
        /// The base directory that the assembly resolver uses to probe for assemblies (see .NET documentation).
        /// </param>
        /// <param name="appRelativeSearchPath">
        /// The path relative to the base directory where the assembly resolver should probe
        /// for private assemblies (see .NET documentation).
        /// </param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and
        /// retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="callConvention">
        /// The CallingConventions flags that specifies the set of rules to use regarding the order
        /// and layout of arguments, how the return value is passed, what registers are used for arguments,
        /// and the stack is cleaned up (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="key">The key for the object definition.</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the
        /// corresponding element in the types array (see .NET documentation).
        /// </param>
        /// <param name="actualParams">The parameter list to use (can be null or have null elements).</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <param name="shadowCopyFiles">
        /// If true, a shadow copy of an assembly is loaded into this application domain (see .NET documentation).
        /// </param>
        /// <param name="definitionOverwrited">
        /// A flag indicating whether the parameter/options is overwritten by user given values.
        /// </param>
        /// <param name="objectKeysSeen">
        /// A set store all the keys that are waiting to be solved in the chain of recursive method calls.
        /// </param>
        /// <param name="oncePerTopLevelInstantiationObjects">
        /// A dictionary store the loaded objects that are configured with OncePerTopLevelObject
        /// instantiation lifetime.
        /// </param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or
        /// could signal an implementation specific problem (missing property, parse error, etc).
        /// </exception>
        /// <exception cref="ArgumentNullException">If the key argument is null.</exception>
        /// <exception cref="ArgumentException">If the key argument is empty string.</exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        private object CreateDefinedObject(string key, object[] actualParams, object ignoreCase, object bindingAttr,
            Binder binder, CallingConventions callConvention, ParameterModifier[] modifiers, CultureInfo culture,
            object[] activationAttributes, Evidence securityAttributes, string appBasePath,
            string appRelativeSearchPath, bool shadowCopyFiles, IDictionary objectKeysSeen,
            IDictionary oncePerTopLevelInstantiationObjects, bool definitionOverwrited)
        {
            Helper.ValidateNotNullOrEmpty(key, "key");

            try
            {
                // apply instantiation lifetime scheme if the object definition doesn't change
                if (!definitionOverwrited)
                {
                    // try to get from dictionaries
                    object cachedObj = oncePerTopLevelInstantiationObjects[key];
                    if (cachedObj == null)
                    {
                        cachedObj = factoryLifetimeObjects[key];
                    }
                    if (cachedObj != null)
                    {
                        return cachedObj;
                    }

                    // detect infinite cycle
                    if (objectKeysSeen.Contains(key))
                    {
                        throw new ObjectCreationException("Infinite instantiation cycle detected. key: " + key);
                    }

                    // add key to objectKeysSeen to predict cycles
                    objectKeysSeen.Add(key, null);
                }

                ObjectDefinition definition = GetDefinition(key);

                object objCreated = null;

                // create the object into a specified app domain
                if (definition.AppDomain != null)
                {
                    // create the domain
                    AppDomain appDomain = CreateDomain(definition.AppDomain, securityAttributes,
                        appBasePath, appRelativeSearchPath, shadowCopyFiles);

                    objCreated = CreateDefinedObject(appDomain, key, actualParams, ignoreCase,
                        bindingAttr, binder, culture, activationAttributes, securityAttributes,
                        objectKeysSeen, oncePerTopLevelInstantiationObjects);
                }
                else
                {
                    // create the parameters using config values
                    if (actualParams == null)
                    {
                        actualParams = GenerateActualParams(
                            definition, objectKeysSeen, oncePerTopLevelInstantiationObjects);
                    }

                    bool caseInsensitive = ignoreCase == null ? definition.IgnoreCase : (bool)ignoreCase;

                    Type objType = GenerateType(definition.TypeName, definition.Assembly, false,
                        caseInsensitive);

                    // create object through a static method
                    if (definition.IsStatic)
                    {
                        BindingFlags flags;
                        if (bindingAttr == null)
                        {
                            // default binding flags for static factory method
                            flags = BindingFlags.Public | BindingFlags.Static;
                            if (caseInsensitive)
                            {
                                flags |= BindingFlags.IgnoreCase;
                            }
                        }
                        else
                        {
                            // the binding flags is specified by user
                            flags = (BindingFlags)bindingAttr;
                        }

                        objCreated = CreateObject(objType, definition.MethodName,
                            actualParams, flags, binder, callConvention, modifiers, culture);
                    }
                    else
                    {
                        BindingFlags flags = bindingAttr == null ?
                            GetBindingFlags(caseInsensitive) : (BindingFlags)bindingAttr;

                        // create object by constructor
                        objCreated = CreateObject(objType,
                            actualParams, flags, binder, callConvention, modifiers, culture);
                    }
                }

                if (!definitionOverwrited)
                {
                    // maintain dictionaries
                    objectKeysSeen.Remove(key);

                    switch (definition.InstantiationLifetime)
                    {
                        case InstantiationLifetime.Factory:
                            factoryLifetimeObjects.Add(key, objCreated);
                            break;
                        case InstantiationLifetime.OncePerTopLevelObject:
                            oncePerTopLevelInstantiationObjects.Add(key, objCreated);
                            break;
                    }
                }

                // invoke all methods specified by the ObjectDefinition using the ApplyMethod method
                foreach (MethodCallDefinition methodCall in definition.MethodCalls)
                {
                    ApplyMethodCall(objCreated, methodCall, objectKeysSeen, oncePerTopLevelInstantiationObjects);
                }

                return objCreated;
            }
            catch (ObjectCreationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ObjectCreationException(
                    String.Format("Failed to create object of key [{0}].", key), e);
            }
        }

        /// <summary>
        /// <p>Dynamically creates an object using reflection into the specified application domain.</p>
        /// </summary>
        ///
        /// <param name="appDomain">The application domain.</param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="activationAttributes">Activation attributes (see .NET documentation).</param>
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members, and
        /// retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the
        /// type constructor (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="ignoreCase">Should case be ignored while looking the Type.</param>
        /// <param name="key">The key for the object definition.</param>
        /// <param name="securityAttributes">Security attributes for creation authorization (see .NET doc).</param>
        /// <param name="objectKeysSeen">
        /// A set store all the keys that are waiting to be solved in the chain of recursive method calls.
        /// </param>
        /// <param name="oncePerTopLevelInstantiationObjects">
        /// A dictionary store the loaded objects that are configured with OncePerTopLevelObject
        /// instantiation lifetime.
        /// </param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectSourceException">
        /// Wraps an implementation specific exception (SQL exception for a database implementation,
        /// Configuration Manager exception for a configuration file implementation, etc) or
        /// could signal an implementation specific problem (missing property, parse error, etc).
        /// </exception>
        /// <exception cref="ArgumentNullException">If the appDomain or key argument is null.</exception>
        /// <exception cref="ArgumentException">If the key argument is empty string.</exception>
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        private object CreateDefinedObject(AppDomain appDomain, string key, object[] parameters, object ignoreCase,
            object bindingAttr, Binder binder, CultureInfo culture, object[] activationAttributes,
            Evidence securityAttributes, IDictionary objectKeysSeen, IDictionary oncePerTopLevelInstantiationObjects)
        {
            Helper.ValidateNotNullOrEmpty(key, "key");
            Helper.ValidateNotNull(appDomain, "appDomain");

            try
            {
                // get the definition of the object by given key.
                ObjectDefinition definition = GetDefinition(key);

                if (definition.IsStatic)
                {
                    throw new ObjectCreationException(
                        "Creation through a static factory method is not supported " +
                        "when creating an object in an alternate application domain.");
                }

                bool caseInsensitive = ignoreCase == null ? DefaultIgnoreCase : (bool)ignoreCase;
                BindingFlags bindingFlags = bindingAttr == null ?
                    GetBindingFlags(caseInsensitive) : (BindingFlags)bindingAttr;

                // set the parameters if the given one is null.
                if (parameters == null)
                {
                    parameters = GenerateActualParams(
                        definition, objectKeysSeen, oncePerTopLevelInstantiationObjects);
                }

                // create and return object.
                if (definition.Assembly == null)
                {
                    // get the type of the object.
                    Type objectType = GenerateType(definition.TypeName, null, false, caseInsensitive);

                    return appDomain.CreateInstanceAndUnwrap(
                        objectType.Assembly.FullName, objectType.FullName,
                        caseInsensitive, bindingFlags, binder,
                        parameters, culture, activationAttributes, securityAttributes);
                }
                else
                {
                    return appDomain.CreateInstanceFromAndUnwrap(
                        definition.Assembly, definition.TypeName, caseInsensitive,
                        bindingFlags, binder, parameters, culture, activationAttributes, securityAttributes);
                }
            }
            catch (ObjectCreationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ObjectCreationException("Error occurs while creating the object.", e);
            }
        }

        /// <summary>
        /// <p>Generates the actual parameters using the definition.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Exceptions will be promulgated.</p>
        /// </remarks>
        ///
        /// <param name="definition">
        /// The definition contains information that use to generate parameter objects.
        /// </param>
        /// <param name="objectKeysSeen">
        /// A set store all the keys that are waiting to be solved in the chain of recursive method calls.
        /// </param>
        /// <param name="oncePerTopLevelInstantiationObjects">
        /// A dictionary store the loaded objects that are configured with OncePerTopLevelObject
        /// instantiation lifetime.
        /// </param>
        ///
        /// <returns>The actual parameters array.</returns>
        private object[] GenerateActualParams(ObjectPartDefinition definition,
            IDictionary objectKeysSeen, IDictionary oncePerTopLevelInstantiationObjects)
        {
            object[] paramValues = definition.ParamValues;
            string[] paramTypes = definition.ParamTypes;
            int paramsLen = paramValues.Length;

            object[] actualParams = new object[paramsLen];

            for (int i = 0; i < paramsLen; ++i)
            {
                // the element type name of array parameters
                string elementTypeName;

                // parse the parameter type
                Type type = Helper.RetrieveType(ref paramTypes[i], out elementTypeName);

                if (Helper.IsPrimitiveOrString(type) || Helper.IsPrimitiveOrStringArray(type))
                {
                    // primitive types or string
                    actualParams[i] = paramValues[i];
                }
                else if (type == typeof(void))
                {
                    // null parameter value
                    actualParams[i] = null;
                }
                else if (type.IsArray)
                {
                    // get the keys of objects
                    string[] arrayKeys = paramValues[i] as string[];

                    // create an array with the element type
                    int arrayLen = arrayKeys.Length;
                    Type elementType = elementTypeName == null ?
                        typeof(object) : Type.GetType(elementTypeName, true);
                    Array value = Array.CreateInstance(elementType, arrayLen);

                    for (int j = 0; j < arrayLen; ++j)
                    {
                        // create array elements according to the key
                        object element = CreateDefinedObject(arrayKeys[j], null, null, null, null,
                            CallingConventions.Any, null, null, null, null, null, null, false,
                            objectKeysSeen, oncePerTopLevelInstantiationObjects, false);

                        // validate the created object is valid for the array type
                        if (!elementType.IsAssignableFrom(element.GetType()))
                        {
                            throw new ObjectCreationException(
                                String.Format("Instance of key [{0}] can not be assigned to array [{1}].",
                                arrayKeys[j], value.GetType()));
                        }

                        // set the created instance into the array
                        value.SetValue(element, j);
                    }
                    actualParams[i] = value;
                }
                else
                {
                    // create single complex object
                    string paramKey = paramValues[i] as string;
                    actualParams[i] = CreateDefinedObject(paramKey, null, null, null, null,
                        CallingConventions.Any, null, null, null, null, null, null, false,
                        objectKeysSeen, oncePerTopLevelInstantiationObjects, false);
                }

                // Failed to create parameter object
                if (actualParams[i] == null && type != typeof(void))
                {
                    throw new ObjectCreationException(
                        String.Format("Failed to create {0}-th parameter.", i));
                }
            }
            return actualParams;
        }

        /// <summary>
        /// <p>Applies the given method definition to the given object.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This method will invoke either the method specified by the definition,
        /// or set the property specified by the definition.</p>
        ///
        /// <p>Exceptions will be promulgated.</p>
        /// </remarks>
        ///
        /// <param name="applyMethodsTo">The object to invoke the method on.</param>
        /// <param name="methodCallDefinition">The definition of the method to invoke.</param>
        /// <param name="objectKeysSeen">
        /// A set store all the keys that are waiting to be solved in the chain of recursive method calls.
        /// </param>
        /// <param name="oncePerTopLevelInstantiationObjects">
        /// A dictionary store the loaded objects that are configured with OncePerTopLevelObject
        /// instantiation lifetime.
        /// </param>
        private void ApplyMethodCall(object applyMethodsTo, MethodCallDefinition methodCallDefinition,
            IDictionary objectKeysSeen, IDictionary oncePerTopLevelInstantiationObjects)
        {
            object[] parameters = GenerateActualParams(
                methodCallDefinition, objectKeysSeen, oncePerTopLevelInstantiationObjects);

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            if (methodCallDefinition.IgnoreCase)
            {
                flags |= BindingFlags.IgnoreCase;
            }

            MethodBase[] methods;
            if (methodCallDefinition.IsProperty)
            {
                // since CLS allow indexed property, more than one properties may have the same name,
                PropertyInfo[] properties = applyMethodsTo.GetType().GetProperties(flags);

                // filter properties by name
                ArrayList candidates = new ArrayList();
                foreach (PropertyInfo candidate in properties)
                {
                    if (String.Compare(candidate.Name, methodCallDefinition.MethodName,
                        methodCallDefinition.IgnoreCase) == 0)
                    {
                        MethodBase setter = candidate.GetSetMethod();

                        if (setter != null)
                        {
                            candidates.Add(setter);
                        }
                    }
                }
                methods = (MethodBase[])candidates.ToArray(typeof(MethodBase));
            }
            else
            {
                // Get a list of methods matched the MethodName
                methods = FilterMethods(applyMethodsTo.GetType(), methodCallDefinition.MethodName, flags);
            }

            // select a method match the given parameters
            object state = null;
            object[] args = parameters;
            MethodBase method = Type.DefaultBinder.BindToMethod(
                flags, methods, ref args, null, null, null, out state);

            // invoke by the given array to insure ref/out parameters work correctly
            method.Invoke(applyMethodsTo, parameters);
        }

        /// <summary>
        /// <p>Generate the type of an object using the given parameters.</p>
        /// </summary>
        ///
        /// <param name="type">The name of the type.</param>
        /// <param name="assembly">The assembly using for load type.</param>
        /// <param name="assemblyNeeded">If the assembly is needed.</param>
        /// <param name="ignoreCase">Should case be considered.</param>
        ///
        /// <returns>The type after generating.</returns>
        ///
        /// <exception cref="ObjectCreationException">Error occurs while getting type from assembly.</exception>
        private static Type GenerateType(string type, string assembly, bool assemblyNeeded, bool ignoreCase)
        {
            Helper.ValidateNotNullOrEmpty(type, "type");
            if (assemblyNeeded)
            {
                Helper.ValidateNotNullOrEmpty(assembly, "assembly");
            }

            try
            {
                return assembly == null ?
                    Type.GetType(type, true, ignoreCase) :
                    Assembly.LoadFrom(assembly).GetType(type, true, ignoreCase);
            }
            catch (Exception e)
            {
                throw new ObjectCreationException("Error occurs while getting type from assembly.", e);
            }
        }

        /// <summary>
        /// <p>Creates a new application domain with the given name, using evidence, application base path,
        /// relative search path, and a parameter that specifies whether a shadow copy of an assembly
        /// is to be loaded into the application domain. </p>
        /// </summary>
        ///
        /// <param name="appDomain">
        /// The friendly name of the domain. This friendly name can be displayed in user
        /// interfaces to identify the domain.
        /// </param>
        /// <param name="securityAttributes">
        /// Evidence mapped through the security policy to establish a top-of-stack permission set.
        /// </param>
        /// <param name="appBasePath">
        /// The base directory that the assembly resolver uses to probe for assemblies.
        /// </param>
        /// <param name="appRelativeSearchPath">
        /// The path relative to the base directory where the assembly resolver should probe for private assemblies.
        /// </param>
        /// <param name="shadowCopyFiles">
        /// If true, a shadow copy of an assembly is loaded into this application domain.
        /// </param>
        ///
        /// <returns>The newly created application domain.</returns>
        ///
        /// <exception cref="ObjectCreationException">If failed to create application domain.</exception>
        /// <exception cref="ArgumentNullException">If appDomain is null.</exception>
        /// <exception cref="ArgumentException">If appDomain is empty string.</exception>
        private static AppDomain CreateDomain(string appDomain, Evidence securityAttributes, string appBasePath,
            string appRelativeSearchPath, bool shadowCopyFiles)
        {
            Helper.ValidateNotNullOrEmpty(appDomain, "appDomain");
            try
            {
                return AppDomain.CreateDomain(appDomain, securityAttributes, appBasePath,
                    appRelativeSearchPath, shadowCopyFiles);
            }
            catch (Exception inner)
            {
                throw new ObjectCreationException(
                          String.Format("Failed to create application domain [{0}].", appDomain), inner);
            }
        }

        /// <summary>
        /// <p>Gets default binding flags for method searching.</p>
        /// </summary>
        ///
        /// <param name="ignoreCase">Combine the flag of ignore case if true.</param>
        ///
        /// <returns>The binding flags.</returns>
        private static BindingFlags GetBindingFlags(bool ignoreCase)
        {
            BindingFlags ret = BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance;
            if (ignoreCase)
            {
                ret |= BindingFlags.IgnoreCase;
            }
            return ret;
        }

        /// <summary>
        /// <p>Dynamically creates an object by a static method using reflection.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Exceptions will be promulgated.</p>
        /// </remarks>
        ///
        /// <param name="binder">
        /// An object that enables the binding, coercion of argument types, invocation of members,
        /// and retrieval of MemberInfo objects through reflection (see .NET documentation).
        /// </param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the type
        /// constructor/method (see .NET documentation).
        /// </param>
        /// <param name="culture">Culture specific information (see .NET documentation).</param>
        /// <param name="modifiers">
        /// An array of ParameterModifier objects representing the attributes associated with the
        /// corresponding element in the types array (see .NET documentation).
        /// </param>
        /// <param name="parameters">The parameter list to use (can be null or have null elements).</param>
        /// <param name="type">The type of the object to create.</param>
        /// <param name="methodName">The static method name used to create the object.</param>
        /// <param name="callConvention">
        /// The CallingConventions flags that specifies the set of rules to use regarding the order
        /// and layout of arguments, how the return value is passed, what registers are used for arguments,
        /// and the stack is cleaned up (see .NET documentation).
        /// </param>
        ///
        /// <returns>The dynamically created object.</returns>
        ///
        /// <exception cref="ObjectCreationException">
        /// Wraps an object creation exception (reflection exception) or signals an object creation problem.
        /// </exception>
        private static object CreateObject(
            Type type, string methodName, object[] parameters, BindingFlags bindingAttr,
            Binder binder, CallingConventions callConvention, ParameterModifier[] modifiers, CultureInfo culture)
        {
            // set the default value of the binder and parameters.
            if (binder == null)
            {
                binder = Type.DefaultBinder;
            }

            // get methods matched methodName and callConvention
            MethodBase[] methods = FilterMethods(type, methodName, bindingAttr);
            methods = FilterMethods(methods, callConvention);

            object state = null;
            object[] args = parameters;
            MethodBase method = binder.BindToMethod(
                bindingAttr, methods, ref args, modifiers, culture, null, out state);

            // invoke by the given array to insure ref/out parameters work correctly
            object objCreated = method.Invoke(null, parameters);

            if (objCreated == null)
            {
                throw new ObjectCreationException(
                    String.Format("Failed to create instance of {0} by method {1}.", type, methodName));
            }

            return objCreated;
        }

        /// <summary>
        /// <p>Gets a list of methods that match the methodName from the given type.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Exceptions will be promulgated.</p>
        /// </remarks>
        ///
        /// <param name="type">The type.</param>
        /// <param name="methodName">The name of the method to search.</param>
        /// <param name="bindingAttr">
        /// A combination of zero or more bit flags that affect the search for the method.
        /// </param>
        ///
        /// <returns>A list of methods has the given name.</returns>
        private static MethodBase[] FilterMethods(Type type, string methodName, BindingFlags bindingAttr)
        {
            bool ignoreCase = (bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default;

            ArrayList candidates = new ArrayList();
            MethodBase[] methods = type.GetMethods(bindingAttr);
            foreach (MethodBase candidate in methods)
            {
                if (String.Compare(methodName, candidate.Name, ignoreCase) == 0)
                {
                    candidates.Add(candidate);
                }
            }
            return (MethodBase[])candidates.ToArray(typeof(MethodBase));
        }

        /// <summary>
        /// <p>Filters a list of methods with the CallingConventions flags.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>Exceptions will be promulgated.</p>
        /// </remarks>
        ///
        /// <param name="candidates">A list of methods to filter.</param>
        /// <param name="callCnv">The CallingConventions flags.</param>
        ///
        /// <returns>A list of methods match the CallingConventions flags.</returns>
        private static MethodBase[] FilterMethods(MethodBase[] candidates, CallingConventions callCnv)
        {
            ArrayList result = new ArrayList();
            foreach (MethodBase method in candidates)
            {
                if ((method.CallingConvention & callCnv) != 0)
                {
                    result.Add(method);
                }
            }

            return (MethodBase[])result.ToArray(typeof(MethodBase));
        }
    }
}
