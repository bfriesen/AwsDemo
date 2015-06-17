AWS Demo
========

This repository contains a code samples showing how a System.MissingMethodException is thrown when using the AWSSDK NuGet package via an internal .NET 4.0 library.

In order to recreate the problem, begin by adding a local NuGet package source to Visual Studio. Go to Tools > Options > NuGet Package Manager > Package Sources. From there, add a new package source - its Source should be the `build` directory under the root of this repository. That directory contains a .nupkg file, which will be available to Visual Studio when building projects that have a dependency on the DotNet40Library package.

Open AwsDemo.sln and notice there three projects in the solution - one class library and two console applications. DotNet40Application references .NET Framework 4.0, while DotNet45Application references .NET Framework 4.5.

In order to run either application, you'll need to provide credentials. When I ran them, I modified my App.config files to include my AWS credentials (but didn't commit them to git).

Each console application takes a single parameter, which is the url to the queue that it should send to. If you're running the applications from Visual Studio, you'll need to supply this from each project's properties (Properties > Debug > Command line arguments).

When you run DotNet40Application, it successfully sends the message to SQS (if you supplied a valid queue url - otherwise it displays a timeout message). However, when DotNet45Application is run, this exception is thrown:

```
System.MissingMethodException was unhandled
  _HResult=-2146233069
  _message=Method not found: 'System.IAsyncResult Amazon.SQS.IAmazonSQS.BeginSendMessage(Amazon.SQS.Model.SendMessageRequest, System.AsyncCallback, System.Object)'.
  HResult=-2146233069
  IsTransient=false
  Message=Method not found: 'System.IAsyncResult Amazon.SQS.IAmazonSQS.BeginSendMessage(Amazon.SQS.Model.SendMessageRequest, System.AsyncCallback, System.Object)'.
  Source=DotNet40Library
  StackTrace:
       at DotNet40Library.MessageSender.SendAsync(String message)
       at DotNet45Application.Program.Main(String[] args) in c:\dev\AwsDemo\DotNet45Application\Program.cs:line 22
       at System.AppDomain._nExecuteAssembly(RuntimeAssembly assembly, String[] args)
       at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
       at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
       at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
       at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
       at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
       at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
       at System.Threading.ThreadHelper.ThreadStart()
  InnerException: 
```

The reason why this exception is thrown is because the .NET 4.5 DLL supplied with the AWSSDK NuGet package does not contain these methods. And the DotNet45Application references the .NET 4.5 DLL because it is itself a .NET 4.5 application.

Workaround
----------

Switch to the `workaround` branch to the workaround that we will probably go with.
Switch to the `ugly-workaround` branch to see an ugly, reflection-based workaround.

Solution
--------

From the perspective of a client of Amazon's, either of these solutions will work:
- Provide a Task-based .NET 4.0 version of the DLL alongside the existing .NET 3.5 and .NET 4.5 versions in the AWSSDK NuGet package.
- Include the old IAsyncResult-based, Begin/End-style asynchronous API in the .NET 4.5 DLL that the 3.5 DLL provides.