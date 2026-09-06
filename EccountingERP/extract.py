import openpyxl
import re

file_path = r"C:\Users\muhammad.asif\Desktop\zzzz\asia\Petition Phase 5 and SIX.xlsx"
wb = openpyxl.load_workbook(file_path, data_only=True)
sheet = wb.active

numbers = []
# Iterate over all rows starting from row 4 (since rows 1-3 are headers/blank)
for row in range(4, sheet.max_row + 1):
    val = sheet.cell(row=row, column=5).value
    if val is None:
        continue
    val_str = str(val).strip()
    if val_str:
        # Extract digits
        digits = re.sub(r'\D', '', val_str)
        if len(digits) >= 10:
            # Format to international format (e.g., 0300... -> +92300...)
            if digits.startswith('0'):
                digits = '92' + digits[1:]
            elif digits.startswith('3'):
                digits = '92' + digits
            numbers.append(digits)

with open(r"d:\tradingsoftwarerepo\numbers.txt", "w") as f:
    for n in numbers:
        f.write(n + "\n")
