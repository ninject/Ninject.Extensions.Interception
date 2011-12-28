using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Interception extesnion for Ninject")]
[assembly: AssemblyDescription("Adds support for interception to Ninject")]
[assembly : AssemblyConfiguration( "" )]
[assembly : AssemblyTrademark( "" )]
[assembly : AssemblyCulture( "" )]
[assembly : Guid( "9c3f8b57-2bbf-4bb8-bdc2-cd0989fef52d" )]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif