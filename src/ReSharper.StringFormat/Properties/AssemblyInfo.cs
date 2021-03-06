using System.Reflection;
using JetBrains.ActionManagement;

#if !RESHARPER9
using JetBrains.Application.PluginSupport;

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("ReSharper.StringFormat")]
[assembly: PluginDescription("Generate string.Format from PHP-like string with embedded variables.")]
[assembly: PluginVendor("Hazzik")]

#endif
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ReSharper.StringFormat")]
[assembly: AssemblyDescription("Generate string.Format from PHP-like string with embedded variables.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Alexander Zaytsev")]
[assembly: AssemblyProduct("ReSharper.StringFormat")]
[assembly: AssemblyCopyright("Copyright © Alexander Zaytsev, 2013-2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.4.2.0")]
[assembly: AssemblyFileVersion("0.4.2.0")]

