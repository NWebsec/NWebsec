using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using NWebsec.Mvc;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NWebsec.Mvc")]
[assembly: AssemblyDescription("The NWebsec security library for MVC applications")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Andre N. Klingsheim")]
[assembly: AssemblyProduct("NWebsec.Mvc")]
[assembly: AssemblyCopyright("Copyright © 2012 - 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8d6189e8-2d19-488a-8f73-eb52bebb617d")]

[assembly: InternalsVisibleTo("NWebsec.Mvc.Tests.Unit, PublicKey=0024000004800000940000000602000000240000525341310004000001000100C1B3E708C81A67A13FD2A4C80CA7D89E90DEF20F75FCE8F9E9F32270329D7889D8701054DF961C3A8C99C3CB381AF5427DAFE0810D243BBD59CE8D9BC93971649CFC00526D3238FC4F817EF80523F5C5E2FE12F7E97EA4969F6A8FCC553B173D9DFB563213880BF5B310BD96E302BDCE94C40D448FCD532F77032EC5D9A732C4")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.1.0")]
[assembly: AssemblyFileVersion("3.0.1.0")]

[assembly: PreApplicationStartMethod(typeof(MvcStart), "DisableMvcVersionHeader")]