// Copyright (c)2005, TopCoder, Inc. All rights reserved

namespace TopCoder.LoggingWrapper
{
    using System;
    using System.Collections;

    /// <summary>
    /// A helper class to test the interface methods without going through the other derived methods.  It is also
    /// used as a test of creating a logger from another assembly, which is why it is in it's own file.  This allows
    /// the custom dll "CustomDll.dll" to be built using the command
    /// "csc /target:library /out:customdll.dll /r:..\..\..\..\..\build\classes\TopCoder.LoggingWrapper.Test.dll LoggerTester.cs".
    /// This library will need to be rebuilt if the Logger interface ever changes, simply use the above command when having
    /// built TopCoder.LoggingWrapper.Test.dll and then move customdll.dll to the test_files directory.
    ///
    /// This is neccesary because otherwise the Logger class will be loaded twice, for example, if we attempt to
    /// use the default TopCoder.LoggingWrapper.dll or even the test assembly, it does not matter.  The .NET system
    /// will load the assembly into memory again, and then it will not be able to recognize the Logger class that
    /// the dervied Logger is using, and we will get an InvalidCastException.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    [CoverageExclude]
    public class LoggerTester : Logger
    {
        /// <summary>
        /// Whether the Log method was called or not.
        /// </summary>
        public bool LogCalled = false;

        /// <summary>
        /// The Level the Log method was called with.
        /// </summary>
        public Level LogLevelCalled = Level.OFF;

        /// <summary>
        /// The message the Log method was called with.
        /// </summary>
        public string LogMessageCalled = null;

        /// <summary>
        /// The parameters the Log method was called with.
        /// </summary>
        public object[] LogParamCalled = null;

        /// <summary>
        /// Needed constructor
        /// </summary>
        /// <param name="logname"></param>
        public LoggerTester(string logname) : base(logname)
        {
        }

        /// <summary>
        /// Needed constructor
        /// </summary>
        /// <param name="logname"></param>
        /// <param name="defaultLevel"></param>
        public LoggerTester(string logname, Level defaultLevel) : base(logname, defaultLevel)
        {
        }

        /// <summary>
        /// Needed constructor
        /// </summary>
        /// <param name="properties"></param>
        public LoggerTester(IDictionary properties) : base(properties)
        {
        }

        /// <summary>
        /// Dispose is also required.
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// Tests the Log() functionality.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="param"></param>
        public override void Log(Level level, string message, params object[] param)
        {
            this.LogCalled = true;

            this.LogLevelCalled = level;
            this.LogMessageCalled = message;
            this.LogParamCalled = param;
        }

        /// <summary>
        /// IsLevelEnabled is also required to be overridden.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public override bool IsLevelEnabled(Level level)
        {
            return false;
        }
    }
}
