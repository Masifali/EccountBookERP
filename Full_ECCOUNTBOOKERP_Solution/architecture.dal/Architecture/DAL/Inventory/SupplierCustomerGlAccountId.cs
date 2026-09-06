using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class SupplierCustomerGlAccountId : DALBase
{
	public List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> lst = new List<Architecture.Model.Inventory.SupplierCustomerGlAccountId>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Inventory.SupplierCustomerGlAccountId>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
