[System.Reflection.Assembly]::LoadFrom("d:\CShapEccorErp\src\bin\Debug\Architecture.Common.dll").GetTypes() | % { $_.FullName }
