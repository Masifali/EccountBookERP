$excel = New-Object -ComObject Excel.Application
$excel.Visible = $false
$workbook = $excel.Workbooks.Open("C:\Users\muhammad.asif\Desktop\zzzz\asia\Petition Phase 5 and SIX.xlsx")
$sheet = $workbook.Sheets.Item(1)

$row = 2
$numbers = @()
while ($true) {
    $cellValue = $sheet.Cells.Item($row, 4).Text
    if (-not $cellValue) {
        break
    }
    $numbers += $cellValue
    $row++
}

$workbook.Close($false)
$excel.Quit()
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($excel) | Out-Null

$numbers | Out-File "numbers.txt"
