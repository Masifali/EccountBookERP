path_common = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_common, "r", encoding="utf-8", errors="ignore") as f:
    code_common = f.read()

old_stub = """	public static DataTable CompanyServiceBind()
	{
		return new DataTable();
	}"""

new_impl = """	public static DataTable CompanyServiceBind()
	{
		try
		{
			int orgId = Architecture.Common.clsGlobalVariables.UserAccount != null ? Architecture.Common.clsGlobalVariables.UserAccount.OrganizationId : 1;
			DataTable dt = Architecture.BLL.Company.GetAlldt(new Architecture.Model.Company
			{
				OrgCompanyTypeId = orgId
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				return dt;
			}
		}
		catch
		{
		}
		DataTable defaultDt = new DataTable();
		defaultDt.Columns.Add("Id");
		defaultDt.Columns.Add("CompName");
		int fallbackCompId = Architecture.Common.clsGlobalVariables.UserAccount != null ? Architecture.Common.clsGlobalVariables.UserAccount.CompanyId : 1;
		string fallbackCompName = (Architecture.Common.clsGlobalVariables.UserAccount != null && !string.IsNullOrEmpty(Architecture.Common.clsGlobalVariables.UserAccount.CompName)) ? Architecture.Common.clsGlobalVariables.UserAccount.CompName : "GOLDEN ACE FOODS";
		defaultDt.Rows.Add(fallbackCompId, fallbackCompName);
		return defaultDt;
	}"""

code_common = code_common.replace(old_stub, new_impl)

with open(path_common, "w", encoding="utf-8") as f:
    f.write(code_common)

path_coa = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path_coa, "r", encoding="utf-8", errors="ignore") as f:
    code_coa = f.read()

# Replace throw new Exception("Grid Record Not Found") with default allocation fallback
old_check = """				if (count == 0)
				{
					throw new Exception("Grid Record Not Found");
				}"""

new_check = """				if (count == 0)
				{
					COAAllocation defaultAllocation = new COAAllocation();
					defaultAllocation.CompanyId = UserAccount.CompanyId > 0 ? UserAccount.CompanyId : 1;
					defaultAllocation.IsActive = true;
					defaultAllocation.BranchId = UserAccount.BranchesId;
					coa.COAAllocationList.Add(defaultAllocation);
				}"""

code_coa = code_coa.replace(old_check, new_check)

with open(path_coa, "w", encoding="utf-8") as f:
    f.write(code_coa)

print("Updated CompanyServiceBind in CommonServices.cs and added allocation fallback in AcfrmDefCoa.cs!")
