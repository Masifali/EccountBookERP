[System.Reflection.Assembly]::LoadFrom("d:\CShapEccorErp\src\bin\Debug\Architecture.BLL.dll").GetTypes() | % { $_.FullName } | Out-File -FilePath "d:\CShapEccorErp\src\bll_types.txt"
[System.Reflection.Assembly]::LoadFrom("d:\CShapEccorErp\src\bin\Debug\Architecture.Model.dll").GetTypes() | % { $_.FullName } | Out-File -FilePath "d:\CShapEccorErp\src\model_types.txt"
