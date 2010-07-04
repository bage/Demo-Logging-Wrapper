/*
 * Copyright (c) 2008, TopCoder, Inc. All rights reserved.
 */

using System;

namespace TopCoder.LoggingWrapper
{
    /// <summary>
    /// This a is custom attribute controls the action of the NCover.
    /// <para>We use this attribute to exclude the coverage of mock classes from the coverage report.</para>
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>3.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    internal class CoverageExcludeAttribute : Attribute
    {
    }
}