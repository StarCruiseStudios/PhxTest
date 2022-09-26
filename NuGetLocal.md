## Install the NuGet CLI
https://www.nuget.org/downloads
Add nuget.exe to PATH.

## Configure Nuget Local Repository
`$env:NUGET_LOCAL_REPO = "D:\Source\Nuget\"`
`nuget sources Add -Name "Local" -Source $env:NUGET_LOCAL_REPO`

## Publish to NuGet local repo
Execute this command at a solution level  
`dotnet msbuild -target:NugetLocal -maxcpucount:1`

## Publish to public nuget
`nuget SetApiKey <Your-API-Key>`

This will push both `nupkg` and `snupkg` files.
`nuget push MyPackage.nupkg`
