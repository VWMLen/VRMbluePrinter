$files = Get-ChildItem -Path "c:\Workspace\VRMblueprinter\Assets\Script" -Filter *.cs -Recurse
foreach ($file in $files) {
    try {
        $path = $file.FullName
        # Read file. UTF8 without BOM can be tricky, but -Encoding Default is common for Shift-JIS.
        $content = Get-Content $path -Encoding Default
        # Write back as UTF8 with BOM
        [System.IO.File]::WriteAllLines($path, $content, (New-Object System.Text.UTF8Encoding $true))
        Write-Host "Converted: $path"
    }
    catch {
        Write-Error "Failed: $path"
    }
}
