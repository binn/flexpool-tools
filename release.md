# Release

`dotnet publish -c Release -r <RID> --self-contained=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true`

RID is release identifier, could be `win-x64` or `linux-x64` (or more depending on other platforms)  


WIN:

`dotnet publish -c Release -r win-x64 --self-contained=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true`

LINUX:

`dotnet publish -c Release -r linux-x64 --self-contained=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true`