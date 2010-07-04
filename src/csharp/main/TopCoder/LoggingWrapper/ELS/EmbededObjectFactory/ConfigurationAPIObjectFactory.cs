/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.ELS.EmbededObjectFactory
{
    /// <summary>
    /// <para>
    /// This class provides one plugin implementation of Object Factory component.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// The Object Factory component provides a generic infrastructure for dynamic
    /// object creation at run-time. It provides a standard interface to create
    /// objects based on object definitions that can be obtained from some source.
    /// This class provides one such source using the <see cref="IConfiguration"/>
    /// interface from the Configuration API component.
    /// </para>
    ///
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
    ///
    /// <para>
    /// The <see cref="SaveDefinition"/> method is added in version 1.1. It is used to
    /// save the specified object definition in <see cref="IConfiguration"/> object
    /// with the specified key.
    /// </para>
    ///
    /// <para>
    /// The <see cref="DeleteDefinition"/> method is added in version 1.1. It is used to
    /// delete the object definition with the specified key from <see cref="IConfiguration"/>
    /// object with the specified key.
    /// </para>
    /// </remarks>
    ///
    /// <threadsafety>
    /// This class is mutable. However, the thread-safety depends on the
    /// <see cref="IConfiguration"/> instance used in this class. If the
    /// <see cref="IConfiguration"/> used is thread safe, this class is thread safe,
    /// else it is not. To be used in thread safe manner, please make sure the
    /// <see cref="IConfiguration"/> used to instantiate this class is thread safe.
    /// </threadsafety>
    ///
    /// <author>justforplay</author>
    /// <author>nebula.lam</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class ConfigurationAPIObjectFactory : ModifiableObjectFactory
    {
        /// <summary>
        /// <para>Represents a dictionary holding parameter types, initialized in the static constructor.</para>
        /// </summary>
        private static IDictionary supportedTypes = null;

        /// <summary>
        /// <para>Represents the prefix of typed array.</para>
        /// </summary>
        private const string TypedArrayPrefix = "object[];";

        /// <summary>
        /// <para>
        /// Represents the configuration name "namespace".
        /// </para>
        /// </summary>
        private const string ConfigNamespace = "namespace";

        /// <summary>
        /// <para>
        /// Represents the configuration name "property".
        /// </para>
        /// </summary>
        private const string ConfigProperty = "property";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "name".
        /// </para>
        /// </summary>
        private const string ConfigName = "name";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "value".
        /// </para>
        /// </summary>
        private const string ConfigValue = "value";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "app_domain".
        /// </para>
        /// </summary>
        private const string ConfigAppDomain = "app_domain";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "assembly".
        /// </para>
        /// </summary>
        private const string ConfigAssembly = "assembly";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "parameters".
        /// </para>
        /// </summary>
        private const string ConfigParameters = "parameters";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "type_name".
        /// </para>
        /// </summary>
        private const string ConfigTypeName = "type_name";

        /// <summary>
        /// <para>
        /// Represents the type defined for a null parameter.
        /// </para>
        /// </summary>
        private const string NullType = "null";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "node_value".
        /// </para>
        /// </summary>
        private const string ConfigNodeValue = "node_value";

        /// <summary>
        /// <para>
        /// Represents the configuration name "methods".
        /// </para>
        /// </summary>
        private const string ConfigMethods = "methods";

        /// <summary>
        /// <para>
        /// Represents the configuration name "parameter".
        /// </para>
        /// </summary>
        private const string ConfigParameter = "parameter";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "type".
        /// </para>
        /// </summary>
        private const string ConfigType = "type";

        /// <summary>
        /// <para>
        /// Represents the configuration name "method".
        /// </para>
        /// </summary>
        private const string ConfigMethod = "method";

        /// <summary>
        /// <para>
        /// Represents the configuration name "object".
        /// </para>
        /// </summary>
        private const string ConfigObject = "object";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "method_name".
        /// </para>
        /// </summary>
        private const string ConfigMethodName = "method_name";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "instantiation_lifetime".
        /// </para>
        /// </summary>
        private const string ConfigInstantiationLifetime = "instantiation_lifetime";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "ignore_case".
        /// </para>
        /// </summary>
        private const string ConfigIgnoreCase = "ignore_case";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute name "is_property".
        /// </para>
        /// </summary>
        private const string ConfigIsProperty = "is_property";

        /// <summary>
        /// <para>
        /// Represents the configuration attribute names of the attributes of object attributes.
        /// </para>
        /// </summary>
        private static readonly string[] ObjectAttributes = new string[] {
            "app_domain", "assembly", "type_name", "ignore_case", "instantiation_lifetime", "method_name" };

        /// <summary>
        /// <para>
        /// Represents the configuration attribute names of the attributes of method attributes.
        /// </para>
        /// </summary>
        private static readonly string[] MethodAttributes = new string[] {
            "ignore_case", "is_property", "method_name" };

        /// <summary>
        /// <para>
        /// Represents the <see cref="IConfiguration"/> instance that is used as the
        /// object source in this class.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Object Factory will get the object definition from this IConfiguration instance.
        /// </para>
        ///
        /// <para>
        /// This instance is initialized in the constructor. It is never null and its
        /// reference is not changed after initialization. It is used in <see cref="GetDefinition"/>,
        /// <see cref="SaveDefinition"/> and <see cref="DeleteDefinition"/> methods to maintain the
        /// object definitions. It can be get through property getter <see cref="Configuration"/>.
        /// </para>
        /// </remarks>
        private readonly IConfiguration configuration;

        /// <summary>
        /// <para>
        /// Represents the configuration hierarchical structure type of this object source.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// There are two kinds of hierarchical structure in this component - FLAT and NESTED.
        /// </para>
        ///
        /// <para>
        /// This value is initialized in the constructor. It is default to be
        /// ConfigurationType.NESTED if not provided in the constructor.
        /// </para>
        ///
        /// <para>
        /// This is enumeration value that can only be ConfigurationType.FLAT
        /// and ConfigurationType.NESTED.
        /// </para>
        ///
        /// <para>
        /// This value is used in <see cref="GetDefinition"/>, <see cref="SaveDefinition"/>
        /// and <see cref="DeleteDefinition"/> methods to determine the processing of object
        /// definitions.
        /// </para>
        /// </remarks>
        private readonly ConfigurationType configurationType;

        /// <summary>
        /// <para>
        /// Gets the <see cref="IConfiguration"/> instance that is used as the
        /// object source in this class.
        /// </para>
        /// </summary>
        ///
        /// <value>
        /// The <see cref="IConfiguration"/> instance that is used as the
        /// object source in this class.
        /// </value>
        public IConfiguration Configuration
        {
            get
            {
                return configuration;
            }
        }

        /// <summary>
        /// <para>
        /// Gets the configuration hierarchical structure type of this object source.
        /// </para>
        /// </summary>
        ///
        /// <value>
        /// The configuration hierarchical structure type of this object source.
        /// </value>
        public ConfigurationType ConfigurationType
        {
            get
            {
                return configurationType;
            }
        }

        /// <summary>
        /// <para>
        /// Static constructor, fills the types dictionary.
        /// </para>
        /// </summary>
        static ConfigurationAPIObjectFactory()
        {
            supportedTypes = new Hashtable();

            // set the single types.
            supportedTypes.Add("int", typeof(int));
            supportedTypes.Add(typeof(int).FullName, typeof(int));
            supportedTypes.Add("bool", typeof(bool));
            supportedTypes.Add(typeof(bool).FullName, typeof(bool));
            supportedTypes.Add("sbyte", typeof(sbyte));
            supportedTypes.Add(typeof(sbyte).FullName, typeof(sbyte));
            supportedTypes.Add("byte", typeof(byte));
            supportedTypes.Add(typeof(byte).FullName, typeof(byte));
            supportedTypes.Add("char", typeof(char));
            supportedTypes.Add(typeof(char).FullName, typeof(char));
            supportedTypes.Add("short", typeof(short));
            supportedTypes.Add(typeof(short).FullName, typeof(short));
            supportedTypes.Add("ushort", typeof(ushort));
            supportedTypes.Add(typeof(ushort).FullName, typeof(ushort));
            supportedTypes.Add("uint", typeof(uint));
            supportedTypes.Add(typeof(uint).FullName, typeof(uint));
            supportedTypes.Add("long", typeof(long));
            supportedTypes.Add(typeof(long).FullName, typeof(long));
            supportedTypes.Add("ulong", typeof(ulong));
            supportedTypes.Add(typeof(ulong).FullName, typeof(ulong));
            supportedTypes.Add("float", typeof(float));
            supportedTypes.Add(typeof(float).FullName, typeof(float));
            supportedTypes.Add("double", typeof(double));
            supportedTypes.Add(typeof(double).FullName, typeof(double));
            supportedTypes.Add("string", typeof(string));
            supportedTypes.Add(typeof(string).FullName, typeof(string));
            supportedTypes.Add("object", typeof(object));

            // simple array types
            supportedTypes.Add("int[]", typeof(int[]));
            supportedTypes.Add(typeof(int[]).FullName, typeof(int[]));
            supportedTypes.Add("bool[]", typeof(bool[]));
            supportedTypes.Add(typeof(bool[]).FullName, typeof(bool[]));
            supportedTypes.Add("sbyte[]", typeof(sbyte[]));
            supportedTypes.Add(typeof(sbyte[]).FullName, typeof(sbyte[]));
            supportedTypes.Add("byte[]", typeof(byte[]));
            supportedTypes.Add(typeof(byte[]).FullName, typeof(byte[]));
            supportedTypes.Add("char[]", typeof(char[]));
            supportedTypes.Add(typeof(char[]).FullName, typeof(char[]));
            supportedTypes.Add("short[]", typeof(short[]));
            supportedTypes.Add(typeof(short[]).FullName, typeof(short[]));
            supportedTypes.Add("ushort[]", typeof(ushort[]));
            supportedTypes.Add(typeof(ushort[]).FullName, typeof(ushort[]));
            supportedTypes.Add("uint[]", typeof(uint[]));
            supportedTypes.Add(typeof(uint[]).FullName, typeof(uint[]));
            supportedTypes.Add("long[]", typeof(long[]));
            supportedTypes.Add(typeof(long[]).FullName, typeof(long[]));
            supportedTypes.Add("ulong[]", typeof(ulong[]));
            supportedTypes.Add(typeof(ulong[]).FullName, typeof(ulong[]));
            supportedTypes.Add("float[]", typeof(float[]));
            supportedTypes.Add(typeof(float[]).FullName, typeof(float[]));
            supportedTypes.Add("double[]", typeof(double[]));
            supportedTypes.Add(typeof(double[]).FullName, typeof(double[]));
            supportedTypes.Add("string[]", typeof(string[]));
            supportedTypes.Add(typeof(string[]).FullName, typeof(string[]));
            supportedTypes.Add("object[]", typeof(object[]));

            supportedTypes.Add("null", typeof(void));
        }

        /// <summary>
        /// <para>
        /// Creates a new instance with the given configuration.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        ///  The configuration type is set to the default value - ConfigurationType.NESTED.
        /// </remarks>
        ///
        /// <param name="configuration">
        /// The IConfiguration instance that is used as the object source in this class. Can't be null.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the configuration is null.
        /// </exception>
        public ConfigurationAPIObjectFactory(IConfiguration configuration)
            : this(configuration, ConfigurationType.Nested)
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new instance with the given configuration and configuration type.
        /// </para>
        /// </summary>
        ///
        /// <param name="configuration">
        /// The IConfiguration instance that is used as the object source in this class. Can't be null.
        /// </param>
        /// <param name="configurationType">
        /// The configuration hierarchical structure type of this object source.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the configuration is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the value is not a pre-defined ConfigurationType value.
        /// </exception>
        public ConfigurationAPIObjectFactory(IConfiguration configuration, ConfigurationType configurationType)
        {
            ValidateNotNull(configuration, "configuration");
            if (!Enum.IsDefined(typeof(ConfigurationType), configurationType))
            {
                throw new ArgumentException("Configuration type must be Flat or Nested.", "configurationType");
            }

            this.configuration = configuration;
            this.configurationType = configurationType;
        }

        /// <summary>
        /// <para>
        /// Retrieves the object definition info with the given key from
        /// <see cref="IConfiguration"/> object source.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// The hierarchical structure of <see cref="IConfiguration"/> contains FLAT
        /// and NESTED types. The FLAT hierarchical structure is nearly the same with
        /// the configuration parameters in <b>ConfigurationObjectFactory</b>
        /// And the NESTED hierarchical structure makes full use of nested property
        /// in Configuration API component and is better structured and more readable.
        /// </para>
        /// </remarks>
        ///
        /// <param name="key">
        /// The key to use. Can't be null or empty.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the key is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the key an empty string.
        /// </exception>
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        public override ObjectDefinition GetDefinition(string key)
        {
            return configurationType == ConfigurationType.Flat ?
                GetFlatDefinition(key) :
                GetNestedDefinition(key);
        }

        /// <summary>
        /// <para>
        /// Retrieves the object definition info with the given key from
        /// FLAT IConfiguration object source.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// The FLAT hierarchical structure is nearly the same with the configuration
        /// parameters in <b>ConfigurationObjectFactory</b>.
        /// </para>
        /// </remarks>
        ///
        /// <param name="key">
        /// The key to use. Can't be null or empty.
        /// </param>
        ///
        /// <returns>
        /// The object definition info.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the key is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the key is an empty string.
        /// </exception>
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        protected virtual ObjectDefinition GetFlatDefinition(string key)
        {
            ValidateNotNullOrEmpty(key, "key");

            try
            {
                // Convert the object configuration to NESTED hierarchical structure
                IConfiguration objectConfig = ConvertObjectDefinition(key);

                return CreateObjectDefinition(objectConfig);
            }
            catch (ConfigurationAPISourceException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Errors occurred while retrieving object definition [{0}].", key), e);
            }
        }

        /// <summary>
        /// <para>
        /// Retrieves the object definition info with the given key from
        /// NESTED IConfiguration object source.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// The NESTED hierarchical structure makes full use of nested
        /// property in Configuration API component and is better structured
        /// and more readable.
        /// </para>
        /// </remarks>
        ///
        /// <param name="key">
        /// The key to use. Can't be null or empty.
        /// </param>
        ///
        /// <returns>
        /// The object definition info.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the key is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the key is an empty string.
        /// </exception>
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also
        /// indicate incomplete definition info (missing type name), parsing errors if the
        /// source uses string representations or some other errors.
        /// </exception>
        protected virtual ObjectDefinition GetNestedDefinition(string key)
        {
            ValidateNotNullOrEmpty(key, "key");

            try
            {
                IConfiguration objectConfig = GetChildByName(configuration, ConfigObject, key, true);

                return CreateObjectDefinition(objectConfig);
            }
            catch (ConfigurationAPISourceException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Errors occurred while retrieving object definition [{0}].", key), e);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the specified object definition in <see cref="IConfiguration"/>
        /// object with the specified key.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// If an object with the same key already exists, the old definition will be overwritten.
        /// </para>
        ///
        /// <para>
        /// The structure used to save the definition depends on <see cref="ConfigurationType"/>.
        /// </para>
        /// </remarks>
        ///
        /// <param name="key">
        /// The key to use. Can't be null or empty.
        /// </param>
        /// <param name="definition">
        /// The definition to be saved.
        /// </param>
        ///
        /// <returns>
        /// The object definition info.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the key or definition is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the key is an empty string.
        /// </exception>
        public override void SaveDefinition(string key, ObjectDefinition definition)
        {
            ValidateNotNullOrEmpty(key, "key");
            ValidateNotNull(definition, "definition");

            // Try to remove the object definition first
            DeleteDefinition(key);

            if (configurationType == ConfigurationType.Flat)
            {
                SaveFlatObjectDefinition(key, definition);
            }
            else
            {
                SaveNestedObjectDefinition(key, definition);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the object definition with the specified key from the
        /// <see cref="IConfiguration"/> object.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// If the specified object definition doesn't exist, does nothing.
        /// </para>
        /// </remarks>
        ///
        /// <param name="key">
        /// The key of the object definition to be removed.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If the key is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the key is an empty string.
        /// </exception>
        public override void DeleteDefinition(string key)
        {
            ValidateNotNullOrEmpty(key, "key");

            if (configurationType == ConfigurationType.Flat)
            {
                // Get the configuration node for the object
                IConfiguration objectConfig = GetChildByName(configuration, ConfigNamespace, key, false);
                if (objectConfig != null)
                {
                    // Get method definitions
                    IConfiguration methodsConfig = GetChildByName(
                        objectConfig, ConfigProperty, ConfigMethods, false);
                    if (methodsConfig != null)
                    {
                        object[] methods = GetChildrenValues(methodsConfig);
                        foreach (string methodName in methods)
                        {
                            // Get method definition
                            IConfiguration methodConfig = GetChildByName(configuration,
                                ConfigNamespace, key + "." + methodName, false);
                            if (methodConfig != null)
                            {
                                // Remove the method definition
                                configuration.RemoveChild(methodConfig.Name);

                                // Remove the parameter definitions of the method.
                                IConfiguration methodParamsConfig = GetChildByName(configuration,
                                    ConfigNamespace, key + "." + methodName + "." + ConfigParameters, false);
                                if (methodParamsConfig != null)
                                {
                                    configuration.RemoveChild(methodParamsConfig.Name);
                                }
                            }
                        }
                    }

                    // Remove the configuration for the given key
                    configuration.RemoveChild(objectConfig.Name);

                    // Remove the parameters configuration for the given key
                    IConfiguration parametersConfig = GetChildByName(
                        configuration, ConfigNamespace, key + "." + ConfigParameters, false);
                    if (parametersConfig != null)
                    {
                        configuration.RemoveChild(parametersConfig.Name);
                    }
                }
            }
            else
            {
                // Remove the configuration for the given key
                IConfiguration objectConfig = GetChildByName(configuration, ConfigObject, key, false);
                if (objectConfig != null)
                {
                    configuration.RemoveChild(objectConfig.Name);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Saves the given object definition in NESTED hierarchical structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="key">
        /// The key of the object definition.
        /// </param>
        /// <param name="definition">
        /// The object definition to be saved.
        /// </param>
        private void SaveNestedObjectDefinition(string key, ObjectDefinition definition)
        {
            // The root config of the definition
            IConfiguration objectConfig = AddRootConfig(ConfigObject, key);

            // Write all simple attributes of the definition
            AddNewNestedConfig(objectConfig, ConfigAppDomain, definition.AppDomain);
            AddNewNestedConfig(objectConfig, ConfigAssembly, definition.Assembly);
            AddNewNestedConfig(objectConfig, ConfigTypeName, definition.TypeName);
            AddNewNestedConfig(objectConfig, ConfigIgnoreCase, definition.IgnoreCase);
            AddNewNestedConfig(objectConfig, ConfigInstantiationLifetime, definition.InstantiationLifetime);
            AddNewNestedConfig(objectConfig, ConfigMethodName, definition.MethodName);

            // Get the method call definitions
            MethodCallDefinition[] methods = definition.MethodCalls;
            if (methods.Length > 0)
            {
                // The methods config
                IConfiguration methodsConfig = new DefaultConfiguration(ConfigMethods);

                // Write each method call definition
                int index = 0;
                foreach (MethodCallDefinition method in methods)
                {
                    SaveNestedMethodDefinition(methodsConfig, ConfigMethod + (++index), method);
                }

                // Add the methods config into root config
                objectConfig.AddChild(AbstractConfiguration.Synchronized(methodsConfig));
            }

            if (definition.ParamTypes.Length > 0)
            {
                // Write the parameter definitions of the object
                SaveNestedParameters(objectConfig, definition.ParamTypes, definition.ParamValues);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the given method definition in NESTED hierarchical structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The method unique name.
        /// </param>
        /// <param name="definition">
        /// The method definition to be saved.
        /// </param>
        /// <param name="parent">
        /// The methods configuration.
        /// </param>
        private void SaveNestedMethodDefinition(IConfiguration parent, string name, MethodCallDefinition definition)
        {
            // The config of the definition
            IConfiguration methodConfig = new DefaultConfiguration(name);

            // Write all simple attributes of the definition
            AddNewNestedConfig(methodConfig, ConfigIgnoreCase, definition.IgnoreCase);
            AddNewNestedConfig(methodConfig, ConfigMethodName, definition.MethodName);
            AddNewNestedConfig(methodConfig, ConfigIsProperty, definition.IsProperty);

            if (definition.ParamTypes.Length > 0)
            {
                // Write the parameter definitions of the method
                SaveNestedParameters(methodConfig, definition.ParamTypes, definition.ParamValues);
            }

            // Add the new method configuration into methods config
            parent.AddChild(AbstractConfiguration.Synchronized(methodConfig));
        }

        /// <summary>
        /// <para>
        /// Saves the given parameters in NESTED hierarchical structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="types">
        /// The parameter types.
        /// </param>
        /// <param name="values">
        /// The parameter values.
        /// </param>
        /// <param name="parent">
        /// The object configuration.
        /// </param>
        private void SaveNestedParameters(IConfiguration parent, string[] types, object[] values)
        {
            IConfiguration paramsConfig = new DefaultConfiguration(ConfigParameters);

            for (int i = 0; i < types.Length; ++i)
            {
                string childName = ConfigParameter + "_" + (i + 1);
                IConfiguration paramConfig;
                if (types[i] == NullType)
                {
                    // null parameter
                    paramConfig = AddNewNestedConfig(paramsConfig, childName, String.Empty);
                }
                else
                {
                    Array array = values[i] as Array;
                    if (array == null)
                    {
                        // Simple parameter
                        paramConfig = AddNewNestedConfig(paramsConfig, childName, values[i]);
                    }
                    else
                    {
                        // Array parameter
                        paramConfig = AddNewNestedConfig(paramsConfig, childName, ToStringArray(array));
                    }
                }

                // Add the type of the parameter
                paramConfig.SetSimpleAttribute(ConfigType, types[i]);
            }

            parent.AddChild(AbstractConfiguration.Synchronized(paramsConfig));
        }

        /// <summary>
        /// <para>
        /// Converts the given array into string array.
        /// </para>
        /// </summary>
        ///
        /// <param name="array">
        /// The array to convert.
        /// </param>
        ///
        /// <returns>
        /// The string array.
        /// </returns>
        private static string[] ToStringArray(Array array)
        {
            string[] strs = new string[array.Length];
            for (int i = 0; i < array.Length; ++i)
            {
                object value = array.GetValue(i);
                if (value != null)
                {
                    strs[i] = value.ToString();
                }
            }
            return strs;
        }

        /// <summary>
        /// <para>
        /// Adds a new nested configuration node with given name and values
        /// </para>
        /// </summary>
        ///
        /// <param name="parent">
        /// The parent configuration of the new node.
        /// </param>
        /// <param name="name">
        /// The name of the new configuration.
        /// </param>
        /// <param name="values">
        /// The values of the new configuration.
        /// </param>
        ///
        /// <returns>
        /// The configuration created.
        /// </returns>
        private static IConfiguration AddNewNestedConfig(IConfiguration parent,
            string name, params object[] values)
        {
            if (values[0] == null)
            {
                return null;
            }

            IConfiguration child = new DefaultConfiguration(name);
            child.SetAttribute(ConfigValue, ToStringArray(values));

            parent.AddChild(AbstractConfiguration.Synchronized(child));
            return child;
        }

        /// <summary>
        /// <para>
        /// Adds a new flat configuration node with given name attribute and values
        /// </para>
        /// </summary>
        ///
        /// <param name="parent">
        /// The parent configuration of the new node.
        /// </param>
        /// <param name="name">
        /// The name of the new configuration.
        /// </param>
        /// <param name="values">
        /// The values of the new configuration.
        /// </param>
        /// <param name="propertyIndex">
        /// The index of the new node.
        /// </param>
        ///
        /// <returns>
        /// The configuration created.
        /// </returns>
        private static void AddNewFlatConfig(IConfiguration parent,
            int propertyIndex, string name, params object[] values)
        {
            if (values[0] == null)
            {
                return;
            }

            // The property node
            IConfiguration child = new DefaultConfiguration(ConfigProperty + "_" + propertyIndex);
            child.SetSimpleAttribute(ConfigName, name);

            // The value nodes
            int index = 0;
            foreach (object value in values)
            {
                if (value != null)
                {
                    IConfiguration valueNode = new DefaultConfiguration(ConfigValue + "_" + (++index));
                    valueNode.SetSimpleAttribute(ConfigNodeValue, value.ToString());

                    child.AddChild(AbstractConfiguration.Synchronized(valueNode));
                }
            }

            // Add the new property node
            parent.AddChild(AbstractConfiguration.Synchronized(child));
        }

        /// <summary>
        /// <para>
        /// Add a configuration into the root configuration.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// The name is the the "prefix_number".
        /// </remarks>
        ///
        /// <param name="prefix">
        /// The prefix of the configuration name.
        /// </param>
        /// <param name="name">
        /// The name attributes of the configuration.
        /// </param>
        ///
        /// <returns>
        /// The configuration created.
        /// </returns>
        private IConfiguration AddRootConfig(string prefix, string name)
        {
            while (true)
            {
                try
                {
                    IConfiguration config = new DefaultConfiguration(GetNextConfigName(configuration, prefix));
                    config.SetSimpleAttribute(ConfigName, name);

                    // Exception may be thrown if the same name is used by other threads.
                    configuration.AddChild(AbstractConfiguration.Synchronized(config));

                    return config;
                }
                catch
                {
                    // Ignore exceptions and try other names
                }
            }
        }

        /// <summary>
        /// <para>
        /// Gets the next available configuration name.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The parent configuration of the new name.
        /// </param>
        /// <param name="prefix">
        /// The prefix of the name.
        /// </param>
        ///
        /// <returns>
        /// The next available configuration name.
        /// </returns>
        private static string GetNextConfigName(IConfiguration config, string prefix)
        {
            int maxId = 1;
            while (true)
            {
                if (!config.ContainsChild(prefix + "_" + maxId))
                {
                    return prefix + "_" + maxId;
                }
                maxId++;
            }
        }

        /// <summary>
        /// <para>
        /// Retrieves the object definition from given configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        ///
        /// <returns>
        /// The object definition created.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private static ObjectDefinition CreateObjectDefinition(IConfiguration config)
        {
            // Read the app_domain property (optional)
            string appDomain = GetSimpleValueFromChild(config, ConfigAppDomain, false);

            // Read the assembly property (optional)
            string assembly = GetSimpleValueFromChild(config, ConfigAssembly, false);

            // Read the type_name property (required)
            string typeName = GetSimpleValueFromChild(config, ConfigTypeName, true);

            // Read the method_name property (optional)
            string methodName = GetSimpleValueFromChild(config, ConfigMethodName, false);

            // Read the ignore_case property (optional), default to false
            // must be a valid boolean value if exists
            bool ignoreCase = GetBooleanFromChild(config, ConfigIgnoreCase);

            // Read the instantiation_lifetime property (optional), default to Instance
            InstantiationLifetime instantiationLifetime = InstantiationLifetime.Instance;
            string instantiationLifetimeVal = GetSimpleValueFromChild(config, ConfigInstantiationLifetime, false);
            if (instantiationLifetimeVal != null)
            {
                instantiationLifetime = (InstantiationLifetime)Enum.Parse(
                    typeof(InstantiationLifetime), instantiationLifetimeVal, true);
            }

            // Retrieve all values of the parameters property
            string[] paramTypes = null;
            object[] paramValues = null;
            GetParameters(config[ConfigParameters], ref paramTypes, ref paramValues);

            IConfiguration methodsConfig = config[ConfigMethods];

            // Retrieve method definitions
            MethodCallDefinition[] methodCalls = null;
            if (methodsConfig != null)
            {
                int index = 0;
                IConfiguration[] methodConfigs = methodsConfig.Children;
                methodCalls = new MethodCallDefinition[methodConfigs.Length];
                foreach (IConfiguration methodConfig in methodConfigs)
                {
                    methodCalls[index++] = GetMethodDefinition(methodConfig);
                }
            }

            // construct and return an ObjectDefinition instance containing the object infomation
            return methodName == null ?
                new ObjectDefinition(appDomain, assembly, typeName, ignoreCase, paramTypes,
                    paramValues, methodCalls, instantiationLifetime) :
                new ObjectDefinition(appDomain, assembly, typeName, methodName, ignoreCase, paramTypes,
                    paramValues, methodCalls, instantiationLifetime);
        }

        /// <summary>
        /// <para>
        /// Retrieves the method definition from given configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="paramsConfig">
        /// The configuration to read.
        /// </param>
        ///
        /// <returns>
        /// The method definition created.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private static MethodCallDefinition GetMethodDefinition(IConfiguration paramsConfig)
        {
            // Read the method_name property (required)
            string methodName = GetSimpleValueFromChild(paramsConfig, ConfigMethodName, true);

            // Read the ignore_case property (optional), default to false
            // must be a valid boolean value if exists
            bool ignoreCase = GetBooleanFromChild(paramsConfig, ConfigIgnoreCase);

            // Read the is_property property (optional), default to false
            // must be a valid boolean value if exists
            bool isProperty = GetBooleanFromChild(paramsConfig, ConfigIsProperty);

            // Retrieve all values of the parameters property
            string[] paramTypes = null;
            object[] paramValues = null;
            GetParameters(paramsConfig[ConfigParameters], ref paramTypes, ref paramValues);

            return new MethodCallDefinition(methodName, isProperty, ignoreCase, paramTypes, paramValues);
        }

        /// <summary>
        /// <para>
        /// Retrieves the parameters from given configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="paramsConfig">
        /// The configuration to read.
        /// </param>
        /// <param name="paramTypes">
        /// The parameter types read.
        /// </param>
        /// <param name="paramValues">
        /// The parameter values read.
        /// </param>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private static void GetParameters(IConfiguration paramsConfig,
            ref string[] paramTypes, ref object[] paramValues)
        {
            // No parameter configured
            if (paramsConfig == null)
            {
                return;
            }

            IConfiguration[] paramConfigs = paramsConfig.Children;
            Array.Sort(paramConfigs, new ConfigurationComparer());

            // Temporary array lists
            ArrayList types = new ArrayList();
            ArrayList values = new ArrayList();
            string elementType;

            foreach (IConfiguration paramConfig in paramConfigs)
            {
                string typeName = GetSimpleAttribute(paramConfig, ConfigType, true);

                Type type = RetrieveType(ref typeName, out elementType);

                // Failed to parse the property name
                if (type == null)
                {
                    throw new ConfigurationAPISourceException(
                        String.Format("Failed to parse parameter type [{0}].", typeName));
                }

                // Parse the property value
                values.Add(ConvertValue(type, paramConfig.GetAttribute(ConfigValue)));

                types.Add(typeName);
            }

            // Convert the array list to array
            paramTypes = (string[])types.ToArray(typeof(String));
            paramValues = values.ToArray();
        }

        /// <summary>
        /// <para>
        /// Converts the object configuration in FLAT structure into NESTED structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="key">
        /// The key of the object definition.
        /// </param>
        ///
        /// <returns>
        /// The object configuration in NESTED structure.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private IConfiguration ConvertObjectDefinition(string key)
        {
            IConfiguration objectConfig = GetChildByName(configuration, ConfigNamespace, key, true);

            // The nested config
            IConfiguration nestedConfig = new DefaultConfiguration(ConfigObject);
            nestedConfig.SetSimpleAttribute(ConfigName, key);

            foreach (IConfiguration child in objectConfig.Children)
            {
                string name = GetSimpleAttribute(child, ConfigName, false);

                if (name != null && Array.IndexOf(ObjectAttributes, name) != -1)
                {
                    // Simple object attributes
                    nestedConfig.AddChild(ConvertConfigNode(child, name));
                }
                else if (name == ConfigMethods)
                {
                    // Method list of the object definition
                    nestedConfig.AddChild(ConvertMethodsConfig(key, child));
                }
            }

            // Convert parameter definitions
            IConfiguration paramsConfig = ConvertParametersConfig(key);
            if (paramsConfig != null)
            {
                nestedConfig.AddChild(paramsConfig);
            }

            return nestedConfig;
        }

        /// <summary>
        /// <para>
        /// Converts the method configuration in FLAT structure into NESTED structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="objectKey">
        /// The key of the object definition.
        /// </param>
        /// <param name="methodsConfig">
        /// The method configuration in FLAT structure.
        /// </param>
        ///
        /// <returns>
        /// The method configuration in NESTED structure.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private IConfiguration ConvertMethodsConfig(string objectKey, IConfiguration methodsConfig)
        {
            IConfiguration nestedConfig = new DefaultConfiguration(ConfigMethods);

            // All method keys
            object[] methodKeys = GetChildrenValues(methodsConfig);

            int index = 0;
            foreach (object methodKeyObj in methodKeys)
            {
                string methodKey = methodKeyObj as string;

                // The NESTED config of the method
                IConfiguration newMethodConfig = new DefaultConfiguration(ConfigMethod + "_" + (++index));

                // Get the FLAT config of the method
                string fullKey = objectKey + "." + methodKey;
                IConfiguration methodConfig =
                    GetChildByName(configuration, ConfigNamespace, fullKey, true);

                // Convert the method attributes
                foreach (IConfiguration methodAttr in methodConfig.Children)
                {
                    string attrName = GetSimpleAttribute(methodAttr, ConfigName, false);
                    if (attrName != null && Array.IndexOf(MethodAttributes, attrName) != -1)
                    {
                        newMethodConfig.AddChild(ConvertConfigNode(methodAttr, attrName));
                    }
                }

                // Convert the parameters attributes
                IConfiguration paramsConfig = ConvertParametersConfig(fullKey);
                if (paramsConfig != null)
                {
                    newMethodConfig.AddChild(paramsConfig);
                }

                nestedConfig.AddChild(newMethodConfig);
            }
            return nestedConfig;
        }

        /// <summary>
        /// <para>
        /// Converts the parameters configuration in FLAT structure into NESTED structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="key">
        /// The key of the parameters definition.
        /// </param>
        ///
        /// <returns>
        /// The parameters configuration in NESTED structure.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If exceptions occur while retrieving the definition info, but may also indicate
        /// incomplete definition info (missing type name), parsing errors if the source
        /// uses string representations or some other errors.
        /// </exception>
        private IConfiguration ConvertParametersConfig(string key)
        {
            IConfiguration parametersConfig = GetChildByName(
                configuration, ConfigNamespace, key + "." + ConfigParameters, false);

            // No parameters configured
            if (parametersConfig == null)
            {
                return null;
            }

            // The new NESTED configuration
            IConfiguration nestedConfig = new DefaultConfiguration(ConfigParameters);

            IConfiguration[] parameters = parametersConfig.Children;

            // Sort the child nodes by name, since the parameters are ordered
            Array.Sort(parameters, new ConfigurationComparer());

            for (int i = 0; i < parameters.Length; ++i)
            {
                if (IsNodeMatch(parameters[i].Name, ConfigProperty))
                {
                    // The new config for the parameter
                    IConfiguration parameter = new DefaultConfiguration(ConfigParameter + "_" + (i + 1));

                    // Type name of the parameter
                    string typeWithDesc = GetSimpleAttribute(parameters[i], ConfigName, true);
                    string type = typeWithDesc.Substring(typeWithDesc.IndexOf(":") + 1).Trim();

                    // Add the type into the new config
                    parameter.SetSimpleAttribute(ConfigType, type);

                    IConfiguration[] valueConfigs = parameters[i].Children;

                    // Sort the child nodes by name, since the array values are ordered
                    Array.Sort(valueConfigs, new ConfigurationComparer());

                    // Convert parameter values
                    object[] values = new object[valueConfigs.Length];
                    for (int j = 0; j < values.Length; ++j)
                    {
                        if (IsNodeMatch(valueConfigs[j].Name, ConfigValue))
                        {
                            values[j] = GetSimpleAttribute(valueConfigs[j], ConfigNodeValue, true);
                        }
                    }

                    parameter.SetAttribute(ConfigValue, values);

                    nestedConfig.AddChild(parameter);
                }
            }

            return nestedConfig;
        }

        /// <summary>
        /// <para>
        /// Converts a property-value configuration into NESETED structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The property configuration with values child.
        /// </param>
        /// <param name="name">
        /// The name of the new node.
        /// </param>
        ///
        /// <returns>
        /// The NESTED configuration created.
        /// </returns>
        private static IConfiguration ConvertConfigNode(IConfiguration config, string name)
        {
            IConfiguration nestedConfig = new DefaultConfiguration(name);
            nestedConfig.SetAttribute(ConfigValue, GetChildrenValues(config));
            return nestedConfig;
        }

        /// <summary>
        /// <para>
        /// Reads all values in a property configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        ///
        /// <returns>
        /// The values.
        /// </returns>
        private static object[] GetChildrenValues(IConfiguration config)
        {
            IConfiguration[] children = config.Children;
            object[] values = new object[children.Length];

            for (int i = 0; i < children.Length; ++i)
            {
                if (IsNodeMatch(children[i].Name, ConfigValue))
                {
                    values[i] = GetSimpleAttribute(children[i], ConfigNodeValue, true);
                }
            }
            return values;
        }

        /// <summary>
        /// <para>
        /// Reads a bool from the child of the given configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        /// <param name="childName">
        /// The child name.
        /// </param>
        ///
        /// <returns>
        /// The bool value, false if not found.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If failed to parse the value.
        /// </exception>
        private static bool GetBooleanFromChild(IConfiguration config, string childName)
        {
            string str = GetSimpleValueFromChild(config, childName, false);
            if (str == null)
            {
                return false;
            }

            try
            {
                return Boolean.Parse(str);
            }
            catch (Exception e)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Configuration [{0}] must be true or false.", childName), e);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a value from a property configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        /// <param name="childName">
        /// The child name.
        /// </param>
        /// <param name="mandatory">
        /// Whether the value is mandatory.
        /// </param>
        ///
        /// <returns>
        /// The value read.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If the value is missing and it is mandatory.
        /// </exception>
        private static string GetSimpleValueFromChild(
            IConfiguration config, string childName, bool mandatory)
        {
            IConfiguration child = config[childName];

            if (child != null)
            {
                string value = GetSimpleAttribute(child, ConfigValue, false);
                if (value != null)
                {
                    return value;
                }
            }

            if (mandatory)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Configuration [{0}] must be a single string.", childName));
            }
            return null;
        }

        /// <summary>
        /// <para>
        /// Gets an attribute value from a configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        /// <param name="attrName">
        /// The attribute name.
        /// </param>
        /// <param name="mandatory">
        /// Whether the value is mandatory.
        /// </param>
        ///
        /// <returns>
        /// The value read.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If the value is missing and it is mandatory.
        /// </exception>
        private static string GetSimpleAttribute(IConfiguration config, string attrName, bool mandatory)
        {
            object[] values = config.GetAttribute(attrName);

            if (values != null && values.Length == 1)
            {
                return values[0] as string;
            }

            if (mandatory)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Configuration [{0}] must be a single string.", attrName));
            }
            return null;
        }

        /// <summary>
        /// <para>
        /// Gets a configuration with specified name.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to read.
        /// </param>
        /// <param name="nodePrefix">
        /// The prefix of the configuration.
        /// </param>
        /// <param name="name">
        /// The name to search.
        /// </param>
        /// <param name="mandatory">
        /// Whether the configuration is mandatory.
        /// </param>
        ///
        /// <returns>
        /// The configuration retrieved.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If multiple configuration found of the configuration is missing and it is mandatory.
        /// </exception>
        private static IConfiguration GetChildByName(
            IConfiguration config, string nodePrefix, string name, bool mandatory)
        {
            IConfiguration matched = null;
            foreach (IConfiguration child in config.Children)
            {
                if (IsNodeMatch(child.Name, nodePrefix) && IsNameMatch(child, name))
                {
                    if (matched != null)
                    {
                        // Multiple matched nodes are found
                        throw new ConfigurationAPISourceException(
                            String.Format("Multiple configuration with name [{0}] found.", name));
                    }
                    matched = child;
                }
            }

            // No matched nodes is found
            if (matched == null && mandatory)
            {
                throw new ConfigurationAPISourceException(
                    String.Format("Configuration with name [{0}] not found.", name));
            }

            return matched;
        }

        /// <summary>
        /// <para>
        /// Checks whether the configuration is matched with specified name.
        /// </para>
        /// </summary>
        ///
        /// <param name="config">
        /// The configuration to check.
        /// </param>
        /// <param name="name">
        /// The expected name attribute value.
        /// </param>
        ///
        /// <returns>
        /// Whether the configuration is matched with specified name.
        /// </returns>
        private static bool IsNameMatch(IConfiguration config, string name)
        {
            object[] attr = config.GetAttribute(ConfigName);
            return attr != null && attr.Length == 1 && (attr[0] as string) == name;
        }

        /// <summary>
        /// <para>
        /// Checks whether the configuration name is matched with specified prefix.
        /// </para>
        /// </summary>
        ///
        /// <param name="node">
        /// The configuration name
        /// </param>
        /// <param name="prefix">
        /// The expected prefix.
        /// </param>
        ///
        /// <returns>
        /// Whether the configuration name is matched with specified prefix.
        /// </returns>
        private static bool IsNodeMatch(string node, string prefix)
        {
            return node.ToLower().StartsWith(prefix);
        }

        /// <summary>
        /// <para>Trims the name of the type by removing blanks and spaces.</para>
        /// </summary>
        ///
        /// <param name="typeName">The string denotes the name of the type.</param>
        ///
        /// <returns>The name of the type after trimmed .</returns>
        private static string FormatTypeName(string typeName)
        {
            typeName = typeName.Trim();

            int leftBracket = typeName.IndexOf('[');
            int rightBracket = typeName.IndexOf(']');

            // the type is not a multiple one as it hasn't left or right bracket or it has a
            // non-right-edged right bracket.
            if (leftBracket == -1 || rightBracket == -1)
            {
                return typeName;
            }

            // judge whether it has non-blank characters between the brackets.
            for (int i = leftBracket + 1; i < rightBracket; i++)
            {
                if (!char.IsWhiteSpace(typeName[i]))
                {
                    return typeName;
                }
            }
            string suffix = typeName.Substring(rightBracket + 1).Trim();
            if (suffix.StartsWith(";"))
            {
                suffix = ";" + suffix.Substring(1).TrimStart();
            }

            return typeName.Substring(0, leftBracket).Trim() + "[]" + suffix;
        }

        /// <summary>
        /// <para>Retrieves the type from the given type name.</para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>The type name will be trimmed  by removing blanks and spaces.</para>
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
        private static Type RetrieveType(ref string typeName, out string elementTypeName)
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
        /// <para>
        /// Converts value with the given type.
        /// </para>
        /// </summary>
        ///
        /// <param name="type">
        /// The given type.
        /// </param>
        /// <param name="value">
        /// The value which is needed to parse.
        /// </param>
        /// <returns>
        /// The object after parsed.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// if any parsing error found.
        /// </exception>
        private static object ConvertValue(Type type, string value)
        {
            try
            {
                // Simply convert the string to primitive types
                if (type.IsPrimitive)
                {
                    return Convert.ChangeType(value, type);
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationAPISourceException("Error occurs while parsing the value " + value
                    + " to the type " + type, e);
            }

            // Allow empty string parameter
            if (type != typeof(string) && value.Trim().Length == 0)
            {
                throw new ConfigurationAPISourceException("Key of type " + type + " can not be empty.");
            }

            // String or key of complex object
            return value;
        }

        /// <summary>
        /// <para>
        /// Converts value with the given type.
        /// </para>
        /// </summary>
        ///
        /// <param name="type">
        /// The given type.
        /// </param>
        /// <param name="values">
        /// The value array which is needed to parse.
        /// </param>
        /// <returns>
        /// The object after parsed.
        /// </returns>
        ///
        /// <exception cref="ConfigurationAPISourceException">
        /// If any parsing error found.
        /// </exception>
        private static object ConvertValue(Type type, object[] values)
        {
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                int len = values.Length;

                // Create actual array for primitive types;
                // or create string array to store strings or object keys
                Array arrArgs = elementType.IsPrimitive ? Array.CreateInstance(elementType, len) : new string[len];

                // parse the property values and set them into array
                for (int i = 0; i < len; ++i)
                {
                    arrArgs.SetValue(ConvertValue(elementType, values[i] as string), i);
                }
                return arrArgs;
            }

            if (values.Length != 1)
            {
                throw new ConfigurationAPISourceException("Multiple values configured for non-array type " + type);
            }

            string str = values[0] as string;

            if (type == typeof(void))
            {
                return null;
            }
            return ConvertValue(type, str);
        }

        /// <summary>
        /// <para>
        /// Checks if parameter is null and throw proper exception.
        /// </para>
        /// </summary>
        ///
        /// <param name="param">
        /// The parameter.
        /// </param>
        /// <param name="name">
        /// The parameter name.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If param is null.
        /// </exception>
        private static void ValidateNotNull(object param, string name)
        {
            if (param == null)
            {
                throw new ArgumentNullException(name, name + " should not be null.");
            }
        }

        /// <summary>
        /// <para>
        /// Checks if parameter is null or an empty string and throw proper exception.
        /// </para>
        /// </summary>
        ///
        /// <param name="param">
        /// The parameter.
        /// </param>
        /// <param name="name">
        /// The parameter name.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If param is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If param is an empty string.
        /// </exception>
        private static void ValidateNotNullOrEmpty(string param, string name)
        {
            ValidateNotNull(param, name);
            if (param.Trim() == String.Empty)
            {
                throw new ArgumentException(name + " should not be empty.", name);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the parameters types and values into FLAT structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="nameSpace">
        /// The namespace of the configuration.
        /// </param>
        /// <param name="types">
        /// The parameter types.
        /// </param>
        /// <param name="values">
        /// The parameters values.
        /// </param>
        private void SaveFlatParameters(string nameSpace, string[] types, object[] values)
        {
            // The root config of the parameters
            IConfiguration paramsConfig = AddRootConfig(ConfigNamespace, nameSpace);
            int propertyIndex = 0;

            int paramsCount = types.Length;
            for (int i = 0; i < paramsCount; ++i)
            {
                if (types[i] == NullType)
                {
                    // null parameter
                    AddNewFlatConfig(paramsConfig, ++propertyIndex, types[i], String.Empty);
                }
                else
                {
                    Array array = values[i] as Array;
                    if (array == null)
                    {
                        // Simple parameter
                        AddNewFlatConfig(paramsConfig, ++propertyIndex, types[i], values[i]);
                    }
                    else
                    {
                        // Array parameter
                        AddNewFlatConfig(paramsConfig, ++propertyIndex, types[i], ToStringArray(array));
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// Saves the method definition into FLAT structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="nameSpace">
        /// The namespace of the configuration.
        /// </param>
        /// <param name="definition">
        /// The method definition to be saved.
        /// </param>
        private void SaveFlatMethodDefinition(string nameSpace, MethodCallDefinition definition)
        {
            IConfiguration methodConfig = AddRootConfig(ConfigNamespace, nameSpace);
            int propertyIndex = 0;

            // Write all attributes of the method definition
            AddNewFlatConfig(methodConfig, ++propertyIndex, ConfigIgnoreCase, definition.IgnoreCase);
            AddNewFlatConfig(methodConfig, ++propertyIndex, ConfigMethodName, definition.MethodName);
            AddNewFlatConfig(methodConfig, ++propertyIndex, ConfigIsProperty, definition.IsProperty);

            if (definition.ParamTypes.Length > 0)
            {
                // Write parameter definitions
                SaveFlatParameters(
                    nameSpace + "." + ConfigParameters, definition.ParamTypes, definition.ParamValues);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the object definition into FLAT structure.
        /// </para>
        /// </summary>
        ///
        /// <param name="nameSpace">
        /// The namespace of the configuration.
        /// </param>
        /// <param name="definition">
        /// The object definition to be saved.
        /// </param>
        private void SaveFlatObjectDefinition(string nameSpace, ObjectDefinition definition)
        {
            IConfiguration objectConfig = AddRootConfig(ConfigNamespace, nameSpace);
            int propertyIndex = 0;

            // Save simple attributes into "property" nodes
            AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigAppDomain, definition.AppDomain);
            AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigAssembly, definition.Assembly);
            AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigTypeName, definition.TypeName);
            AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigIgnoreCase, definition.IgnoreCase);
            AddNewFlatConfig(objectConfig, ++propertyIndex,
                ConfigInstantiationLifetime, definition.InstantiationLifetime);
            AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigMethodName, definition.MethodName);

            MethodCallDefinition[] methods = definition.MethodCalls;
            if (methods.Length > 0)
            {
                // The method name list
                object[] methodNames = new string[methods.Length];
                for (int i = 0; i < methodNames.Length; ++i)
                {
                    // Write method definitions
                    methodNames[i] = methods[i].MethodName;
                    SaveFlatMethodDefinition(nameSpace + "." + methods[i].MethodName, methods[i]);
                }

                AddNewFlatConfig(objectConfig, ++propertyIndex, ConfigMethods, methodNames);
            }

            if (definition.ParamTypes.Length > 0)
            {
                // Write parameter definitions
                SaveFlatParameters(
                    nameSpace + "." + ConfigParameters, definition.ParamTypes, definition.ParamValues);
            }
        }

        /// <summary>
        /// <para>
        /// An <see cref="IComparer"/> implementation used to sort configuration.
        /// </para>
        /// </summary>
        ///
        /// <threadsafety>
        /// This class is immutable and thread safe.
        /// </threadsafety>
        ///
        /// <author>nebula.lam</author>
        /// <version>1.1</version>
        /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
        private class ConfigurationComparer : IComparer
        {
            /// <summary>
            /// <para>
            /// Compare two configuration based on name.
            /// </para>
            /// </summary>
            ///
            /// <param name="x">
            /// A configuration.
            /// </param>
            /// <param name="y">
            /// The other configuration.
            /// </param>
            ///
            /// <returns>
            /// Comparison result.
            /// </returns>
            ///
            /// <exception>
            /// The exceptions will be promulgated if any parsing errors occurred.
            /// </exception>
            public int Compare(object x, object y)
            {
                IConfiguration configA = (IConfiguration)x;
                IConfiguration configB = (IConfiguration)y;
                int xId = Int32.Parse(configA.Name.Substring(configA.Name.LastIndexOf("_") + 1));
                int yId = Int32.Parse(configB.Name.Substring(configB.Name.LastIndexOf("_") + 1));
                return xId - yId;
            }
        }
    }
}
