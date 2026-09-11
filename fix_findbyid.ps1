Get-ChildItem -Path "d:\tradingsoftwarerepo\src\main\java" -Recurse -Filter "*.java" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $newContent = [regex]::Replace($content, '\.findById\((.*?)\.orElse\(null\)(.*?)\)', '.findById($1$2).orElse(null)')
    if ($content -cne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent
        Write-Host "Updated $($_.FullName)"
    }
}
