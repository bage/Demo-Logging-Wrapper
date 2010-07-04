Installation Requirements
  -------------------------
  Microsoft .NET Framework 2.0  <http://msdn.microsoft.com/netframework/productinfo>
  NUnit 2.4  <http://sourceforge.net/projects/nunit>
  NCover 1.5.8 <http://www.ncover.com/download/discontinued>
  FxCop 1.35 <http://code.msdn.microsoft.com/Release/ProjectReleases.aspx?ProjectName=codeanalysis&ReleaseId=553>
  Code Documenter 1.0.1 (http://www.topcoder.com/wiki/display/projects/Code+Documenter/)


  TopCoder Software Environment Configuration
  -------------------------------------------
  TopCoder Software has defined a directory structure to promote component reuse
  and facilitate Component Based Development.

  Steps to setting up your environment:
  1- Designate a directory on your file system to be used as your TCS_HOME.
     (i.e. c:\tcs or /etc/home/user/tcs)
  2- All TopCoder Software components are distributed with MSBuild
     based build scripts and NUnit (http://nunit.org) based test cases.
     Please properly install and configure these tools before working with TopCoder
     Software components.

  Component directory Structure
  ------------------------------------------
  The directory layout for each component is set up as follows:
  /build               - binary related files required to run the components
  /conf                - configuration data required by the component
  /docs                - component documentation
  /docs/coverage       - test coverage reports in html format
  /log                 - log files generated from test suite execution
  /src                 - source code for the component
  /test_files          - source code for the component test cases


  TopCoder Software MSBuild Targets
  ------------------------------------------
  Each component comes with 2 .csproj (one including only the sources and one including the sources&tests  build scripts of the component) with the following targets:
  Build
       - compiles .cs sources into the /build directory
  Test (to be runned from .csproj for the tests of the component)
       - runs test cases and outputs nunit results into /log,
         depends on 'Build'
  NCoverReport (to be runned from .csproj for the tests of the component)
       - generates test coverage reports for the component's tests into /log.
         This executable can be found in the NCover directory.
  FxCop (to be runned from .csproj for the sources of the component)
       - generates reports information about the assemblies, such as possible design,
         localization, performance, and security improvements.
  Doc (to be runned from .csproj for the sources of the component)
       - generates API documentation for the component in a javadoc manner.
  Dist            - generates a binary distribution for deployment, depends on
                    'Build'
  CleanSolution           - deletes all compiled code in /build and the content of /log directory.


  Steps to run through the tests:
  ------------------------------------------
   No special steps are needed to be taken in order to run the tests.
   
  ---------------------------------------------------------------------------------------------------------------
  Thanks for using TopCoder Software components!

  The TopCoder Software Team
  service@topcodersoftware.com
