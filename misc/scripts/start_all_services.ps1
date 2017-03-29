$Env:ASPNETCORE_ENVIRONMENT = "Development"
$Env:RUN_LOCAL = "true"
[int]$port = 5000

Set-Location -Path ..\..\src\Services
$serviceFolders = Get-ChildItem
foreach ($folder in $serviceFolders)
{
    Set-Location $folder.FullName
    $env:ASPNETCORE_URLS="https://*:$port"
    $port = $port + 10

    Start-Process dotnet run
}