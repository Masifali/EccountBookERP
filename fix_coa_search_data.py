path_common = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_common, "r", encoding="utf-8", errors="ignore") as f:
    code_common = f.read()

old_stub = """	public static DataTable CoaAllocationGetForComboServiceBind()
	{
		return new DataTable();
	}"""

new_impl = """	public static DataTable CoaAllocationGetForComboServiceBind()
	{
		try
		{
			UserAccount user = Architecture.Common.clsGlobalVariables.UserAccount;
			int orgId = user != null ? user.OrganizationId : 1;
			int compId = user != null ? user.CompanyId : 1;
			int userId = user != null ? user.ID : 1;

			return Architecture.BLL.Accounts.COAAllocation.GetForComboBind(new ReportsParameters
			{
				OrganizationId = orgId,
				CompanyId = compId,
				UserId = userId
			});
		}
		catch
		{
			return new DataTable();
		}
	}"""

code_common = code_common.replace(old_stub, new_impl)

with open(path_common, "w", encoding="utf-8") as f:
    f.write(code_common)

path_coa = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path_coa, "r", encoding="utf-8", errors="ignore") as f:
    code_coa = f.read()

# Add ValueChanged event bindings for CmbAccountTitle and CmbAccountTypeFilter
old_leave_title = '((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);'
new_leave_title = '((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);\nthis.CmbAccountTitle.ValueChanged += new System.EventHandler(CmbAccountTitle_Leave);'

old_leave_filter = '((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Leave += new System.EventHandler(CmbAccountTypeFilter_Leave);'
new_leave_filter = '((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Leave += new System.EventHandler(CmbAccountTypeFilter_Leave);\nthis.CmbAccountTypeFilter.ValueChanged += new System.EventHandler(CmbAccountTypeFilter_Leave);'

code_coa = code_coa.replace(old_leave_title, new_leave_title)
code_coa = code_coa.replace(old_leave_filter, new_leave_filter)

with open(path_coa, "w", encoding="utf-8") as f:
    f.write(code_coa)

print("Fixed CoaAllocationGetForComboServiceBind in CommonServices.cs and added ValueChanged event listeners in AcfrmDefCoa.cs!")
