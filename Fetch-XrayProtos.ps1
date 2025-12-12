param(
    [string]$Version = "v25.9.11",   # версия Xray-core
    [string]$TargetPath = ".\Xray.Api\Xray-Protos" # папка для сохранения .proto
)

# Формируем URL архива
$zipUrl = "https://github.com/XTLS/Xray-core/archive/refs/tags/$Version.zip"
$zipFile = "$env:TEMP\xray-$Version.zip"

Write-Host "⬇️ Downloading $zipUrl ..."
Invoke-WebRequest -Uri $zipUrl -OutFile $zipFile

# Временная папка для распаковки
$tempExtract = "$env:TEMP\xray-$Version"
if (Test-Path $tempExtract) { Remove-Item $tempExtract -Recurse -Force }
Expand-Archive -Path $zipFile -DestinationPath $tempExtract -Force

# Определяем корневую папку архива (обычно Xray-core-{version})
$rootFolder = Get-ChildItem $tempExtract | Where-Object { $_.PSIsContainer } | Select-Object -First 1

# Создаём целевую папку если её нет
if (!(Test-Path $TargetPath)) {
    New-Item -ItemType Directory -Path $TargetPath | Out-Null
}

# Копируем только .proto файлы, сохраняя структуру папок
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

# Очистка временных файлов
Remove-Item $zipFile -Force
Remove-Item $tempExtract -Recurse -Force

Write-Host "✅ All .proto files os version $Version saved to $TargetPath"
