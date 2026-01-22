param(
    [string]$Version = "v26.1.13",   # Xray-core version
    [string]$TargetPath = ".\Xray.Api\Xray-Protos" # .proto source folder
)

$zipUrl = "https://github.com/XTLS/Xray-core/archive/refs/tags/$Version.zip"
$zipFile = "$env:TEMP\xray-$Version.zip"

Write-Host "⬇️ Downloading $zipUrl ..."
Invoke-WebRequest -Uri $zipUrl -OutFile $zipFile

$tempExtract = "$env:TEMP\xray-$Version"
if (Test-Path $tempExtract) { Remove-Item $tempExtract -Recurse -Force }
Expand-Archive -Path $zipFile -DestinationPath $tempExtract -Force

$rootFolder = Get-ChildItem $tempExtract | Where-Object { $_.PSIsContainer } | Select-Object -First 1

if (!(Test-Path $TargetPath)) {
    New-Item -ItemType Directory -Path $TargetPath | Out-Null
}

Get-ChildItem -Path $rootFolder.FullName -Recurse -Filter *.proto | ForEach-Object {
    $relativePath = $_.FullName.Substring($rootFolder.FullName.Length).TrimStart('\')
    $dest = Join-Path $TargetPath $relativePath

    $destDir = Split-Path $dest
    if (!(Test-Path $destDir)) {
        New-Item -ItemType Directory -Path $destDir -Force | Out-Null
    }

    Copy-Item $_.FullName -Destination $dest -Force
    Write-Host "📄 Saved: $relativePath"
}

Remove-Item $zipFile -Force
Remove-Item $tempExtract -Recurse -Force

Write-Host "✅ All .proto files os version $Version saved to $TargetPath"
