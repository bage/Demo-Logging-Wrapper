/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using log4net.Appender;
using log4net.Core;
using TopCoder.LoggingWrapper.ELS.EmbededObjectFactory;
using TopCoder.Configuration;

namespace TopCoder.LoggingWrapper.ELS
{
    /// <summary>
    /// <para>
    /// The ELSAppender is a custom log4NET appender that forwards logging events to a TopCoder Enterprise
    /// Logging Service instance. It follows the basic pattern for an ELSAppender, inheriting from a
    /// skeleton appender provided by the framework. This class can be instantiated based on settings in
    /// the log4NET config file, as shown in the component spec.
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>
    /// This class is not immutable, as the Url property can be changed, which then changes the
    /// loggingService. In normal use, this property should only be called by the log4NET framework when
    /// instantiating the appender. It should not be called otherwise. For all practical purposes, this
    /// class should be considered immutable. This class is thread-safe as long as the Url property is not
    /// used except by the log4NET framework.
    /// </para>
    /// </threadsafety>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public class ELSAppender : AppenderSkeleton
    {
        /// <summary>
        /// <para>
        /// The enterprise logging service that this appender forwards logging events to, changed in
        /// bug fix.
        /// </para>
        /// <para>
        /// This field is initialized in the <see cref="ELSAppender(IConfiguration)"/>
        /// constructor.
        /// </para>
        /// </summary>
        private ILoggingService loggingService;

        /// <summary>
        /// <para>Key used to create the <see cref="ILoggingService"/> instance using
        /// object factory.</para>
        /// </summary>
        private const string SERVICECONFIGURATIONKEY = "loggingService";

        /// <summary>
        /// <para>
        /// Represents the mapping from the Level enumeration in log4NET to that in TC Logging Wrapper.
        /// </para>
        /// <para>
        /// The key is the Level value in Log4NET, and the value is the corresponding level in TC Logging
        /// Wrapper. It is populated in static constructor.
        /// </para>
        /// </summary>
        private static readonly IDictionary<log4net.Core.Level, Level> levelMapping
            = new Dictionary<log4net.Core.Level, Level>();

        /// <summary>
        /// <para>
        /// Constructor for creating an appender programmatically with given logging service.
        /// </para>
        /// </summary>
        /// <param name="serviceConfiguration">The configuration that the appender is created from.</param>
        /// <exception cref="ArgumentNullException">If service is null.</exception>
        public ELSAppender(IConfiguration serviceConfiguration)
            : base()
        {
            Helper.ValidateNotNull(serviceConfiguration, "serviceConfiguration");

            try
            {
                ConfigurationAPIObjectFactory of =
                    new ConfigurationAPIObjectFactory(serviceConfiguration);
                loggingService = (ILoggingService)of.CreateDefinedObject(SERVICECONFIGURATIONKEY);
            }
            catch (Exception e)
            {
                throw new ConfigException("Failed to create the service client.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Appends the logging event to the backend logging service.
        /// </para>
        /// </summary>
        /// <param name="loggingEvent">The logging event to append.</param>
        /// <exception cref="ArgumentNullException">If loggingEvent is null</exception>
        /// <exception cref="LoggingException">If the call to the logging service fails.</exception>
        protected override void Append(LoggingEvent loggingEvent)
        {
            Helper.ValidateNotNull(loggingEvent, "loggingEvent");

            Level level = levelMapping[loggingEvent.Level];

            // forward to the logging service
            try
            {
                loggingService.Log(loggingEvent.LoggerName, null, level, loggingEvent.RenderedMessage, null);
            }
            catch (Exception e)
            {
                throw new LoggingException("Error occurs when forwarding the message to service.", e);
            }
        }

        /// <summary>
        /// <para>
        /// Releases the resource used by this appender.
        /// </para>
        /// </summary>
        protected override void OnClose()
        {
            if (loggingService is IDisposable)
            {
                (loggingService as IDisposable).Dispose();
            }
            base.OnClose();
        }

        /// <summary>
        /// <para>
        /// This static constructor initializes the level mapping from Level in Log4NET to Level in TC
        /// Logging Wrapper.
        /// </para>
        /// </summary>
        static ELSAppender()
        {
            levelMapping[log4net.Core.Level.Alert] = Level.ERROR;
            levelMapping[log4net.Core.Level.All] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Critical] = Level.ERROR;
            levelMapping[log4net.Core.Level.Debug] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Emergency] = Level.ERROR;
            levelMapping[log4net.Core.Level.Error] = Level.ERROR;
            levelMapping[log4net.Core.Level.Fatal] = Level.FATAL;
            levelMapping[log4net.Core.Level.Fine] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Finer] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Finest] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Info] = Level.INFO;
            levelMapping[log4net.Core.Level.Notice] = Level.INFO;
            levelMapping[log4net.Core.Level.Off] = Level.OFF;
            levelMapping[log4net.Core.Level.Severe] = Level.ERROR;
            levelMapping[log4net.Core.Level.Trace] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Verbose] = Level.DEBUG;
            levelMapping[log4net.Core.Level.Warn] = Level.WARN;
        }
    }
}
