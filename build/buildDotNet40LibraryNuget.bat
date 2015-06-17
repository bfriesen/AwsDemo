msbuild /p:Configuration=Release ..\DotNet40Library\DotNet40Library.csproj
nuget pack ..\DotNet40Library\DotNet40Library.csproj -Properties Configuration=Release