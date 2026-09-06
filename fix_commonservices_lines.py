path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    c = f.read()

# Fix string format in ColorNameToHexCode
c = c.replace('string.Format("#{0}{1}{2}", color.R:X2, color.G:X2, color.B:X2)', 'string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B)')

# Fix VoucherHead calls in GetSubsiadiaryBalance and GetAccountBalance
c = c.replace('VoucherHead.GetGLAndSubsidiaryCurrentBalance(', 'Architecture.BLL.Accounts.VoucherHead.GetGLAndSubsidiaryCurrentBalance(')
c = c.replace('VoucherHead.ReadByCurrentBalanceByDateAndAccountId(', 'Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(')
c = c.replace('new VoucherHead', 'new Architecture.Model.Accounts.VoucherHead')

with open(path_cs, "w", encoding="utf-8") as f:
    f.write(c)

print("Fixed CommonServices.cs lines!")
