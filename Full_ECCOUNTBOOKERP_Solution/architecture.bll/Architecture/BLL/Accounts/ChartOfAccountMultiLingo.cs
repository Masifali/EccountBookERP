using System;
using Architecture.DAL.Accounts;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class ChartOfAccountMultiLingo
{
	public static int SaveList(Architecture.Model.Accounts.ChartOfAccountMultiLingo obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.ChartOfAccountMultiLingo().SetDataList(obj, "Sp_ChartOfAccountMultiLingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
