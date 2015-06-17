using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DotNet40Library")]
[assembly: AssemblyDescription("Sample .NET 4.0 library that has a nuget dependency on AWSSDK. Contains a class that makes calls to IAmazonSQS.BeginSendMessage and IAmazonSQS.EndSendMessage. These two methods exist in the .NET 3.5 version of AWSSDK but not in the .NET 4.5 version.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Brian Friesen")]
[assembly: AssemblyProduct("DotNet40Library")]
[assembly: AssemblyCopyright("Copyright © Brian Friesen 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9dfe8e83-91e3-4c05-93ec-491c0681381c")]

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
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
