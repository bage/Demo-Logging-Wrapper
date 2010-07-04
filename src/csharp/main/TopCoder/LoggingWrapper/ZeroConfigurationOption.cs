/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// <para>
    /// The ZeroConfigurationOption enum defines the valid values that can be specified in the
    /// default_config configuration setting to make use of the zero-configuration setup of this component.
    /// This value is read by the LogManager and then one of these enum values is passed to the
    /// InitializeZeroConfiguration method of the configuration specified logger.
    /// </para>
    /// <para>
    /// Each value of this enum describes an expected setup for the backend logging solution. However,
    /// backend solutions may not implement exactly the recommended functionality, so it is ultimately up
    /// to each backend solution to define what the values mean for that backend.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Enum is thread safe.
    /// </para>
    /// </remarks>
    /// <author>aubergineanode, TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public enum ZeroConfigurationOption
    {
        /// <summary>
        /// <para>
        /// Enum value for Test configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// All messages should be logged to the file "../../test_files/log.txt".
        /// </para>
        /// </summary>
        Test,

        /// <summary>
        /// <para>
        /// Enum value for Component configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// Messages at level INFO and above will be logged to the file "./log.txt".
        /// </para>
        /// </summary>
        Component,

        /// <summary>
        /// <para>
        /// Enum value for Certification configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// Messages are logged to a dated folder (format "yyyy-mm-dd") in the logs subfolder.
        /// Only Info or higher messages are logged.
        /// Logs are rolled over on a regular basis (daily or 1MB size limit recommended).
        /// </para>
        /// </summary>
        Certification,

        /// <summary>
        /// <para>
        /// Enum value for ClientDebug configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// Messages are logged to a dated folder (format "yyyy-mm-dd") in the logs subfolder.
        /// Only Info or higher messages are logged.
        /// Logs are rolled over on a regular basis (daily or 1MB size limit recommended).
        /// The maximum number of logs to be kept is limited (recommended 30 days of logs).
        /// </para>
        /// </summary>
        ClientDebug,

        /// <summary>
        /// <para>
        /// Enum value for ClientStress configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// Messages are logged to a dated folder (format "yyyy-mm-dd") in the logs subfolder.
        /// Only Error or Failure messages are logged.
        /// Logs are rolled over on a regular basis (daily or 1MB size limit recommended).
        /// The maximum number of logs to be kept is limited (recommended 30 days of
        /// logs).
        /// </para>
        /// </summary>
        ClientStress,

        /// <summary>
        /// <para>
        /// Enum value for Release configuration.
        /// </para>
        /// <para>
        /// Recommended backend configuration:
        /// Messages are logged to a dated folder (format "yyyy-mm-dd") in the logs subfolder.
        /// Only Info or higher messages are logged.
        /// Logs are rolled over on a regular basis (daily or 1MB size limit recommended).
        /// The maximum number of logs to be kept is limited (recommended 30 days of logs).
        /// </para>
        /// </summary>
        Release
    }
}