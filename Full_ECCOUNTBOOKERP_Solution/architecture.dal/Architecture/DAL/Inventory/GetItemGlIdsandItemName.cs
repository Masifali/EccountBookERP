using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class GetItemGlIdsandItemName : DALBase
{
	public List<Architecture.Model.Inventory.GetItemGlIdsandItemName> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.GetItemGlIdsandItemName> lst = new List<Architecture.Model.Inventory.GetItemGlIdsandItemName>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Inventory.GetItemGlIdsandItemName>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<GetAccountsByEmployeeId> getdataForEmployee(string ProcName, List<SqlParameter> p = null)
	{
		List<GetAccountsByEmployeeId> lst = new List<GetAccountsByEmployeeId>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<GetAccountsByEmployeeId>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
