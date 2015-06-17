AWS Demo - Ugly Workaround
==========================

This workaround relies on reflection in order to choose the right methods to use. It inspects the Assembly of the AWSSDK DLL and uses the IAsyncResult-based, Begin/End asynchronous API of the .NET 3.5 version when that version is detected. Otherwise, it uses the Task-based asynchronous API of the .NET 4.5 version.

There are ways of almost eliminating the reflection performance penality, but, for simplicity's sake, they are not included here.