/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.ServiceModel;
using TopCoder.Configuration;
using TopCoder.LoggingWrapper.ELS.EmbededObjectFactory;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// <para>
    /// The ELSImpl class is a simple Logger implementation that forwards all Log and LogNamedMessage calls
    /// to an instance of the EnterpriseLoggingService.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is immutable and hence thread-safe. Use of the WCF service is thread-safe.
    /// </para>
    /// </threadsafety>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class ELSImpl : Logger
    {
        /// <summary>
        /// <para>
        /// The enterprise logging service that this logger forwards log messages to.
        /// </para>
        /// <para>
        /// This field is immutable, set in the constructor, and can not be null. Used in the Log method.
        /// </para>
        /// </summary>
        private readonly ILoggingService loggingService;

        /// <summary>
        /// <para>Key used to create the <see cref="ILoggingService"/> instance using
        /// object factory.</para>
        /// </summary>
        private const string SERVICECONFIGURATIONKEY = "loggingService";

        /// <summary>
        /// <para>
        /// Creates a new ElSImpl instance with setting loaded from given configuration.
        /// </para>
        /// </summary>
        /// <param name="configuration">The configuration object to load settings from.</param>
        /// <exception cref="ArgumentNullException">If the configuration is null.</exception>
        /// <exception cref="ConfigException">If any of the configuration settings are missing or are
        /// invalid values, or if there is an exception when instantiating the logging service.</exception>
        public ELSImpl(IConfiguration configuration)
            : base(configuration)
        {
            try
            {
                // instantiate the logging service with basic http binding and given url as endpoint
                // loggingService = new LoggingServiceClient(new BasicHttpBinding(), new EndpointAddress(url));
                ConfigurationAPIObjectFactory of =
                    new ConfigurationAPIObjectFactory(configuration);
                loggingService = (ILoggingService)of.CreateDefinedObject(SERVICECONFIGURATIONKEY);
            }
            catch (Exception e)
            {
                throw new ConfigException("Fail to instantiate the logging service.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Disposes the logging service used by this logger.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            if (loggingService is IDisposable)
            {
                (loggingService as IDisposable).Dispose();
            }
        }

        /// <summary>
        /// Returns true if the level is supported by the logger. All levels are supported.
        /// </summary>
        /// <param name="level">The logging level to check.</param>
        /// <returns>true for all levels.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return true;
        }

        /// <summary>
        /// <para>
        /// Logs a message by forwarding to the logging service.
        /// </para>
        /// <para>
        /// If the level is not supported by this class, or the level is off, the message will not be logged.
        /// </para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.
        /// </param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        /// <exception cref="ArgumentNullException">If message or param is null.</exception>
        /// <exception cref="LoggingException">If any error occurs when accessing the configuration.
        /// </exception>
        public override void Log(Level level, string message, params object[] param)
        {
            Helper.ValidateNotNull(message, "message");
            Helper.ValidateNotNull(param, "param");

            if (level == LoggingWrapper.Level.OFF || !IsLevelEnabled(level))
            {
                return;
            }

            string[] strParam = new string[param.Length];
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] == null)
                {
                    throw new LoggingException("params should not contain null");
                }
                strParam[i] = param[i].ToString();
            }

            // forward to the logging service
            try
            {
                loggingService.Log(Logname, null, level, message, strParam);
            }
            catch (Exception e)
            {
                throw new LoggingException("Error occurs when forwarding the message to service.", e);
            }
        }        
    }
}