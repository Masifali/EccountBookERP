using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Tax.Definitions;

namespace Architecture.DAL.Tax.Definitions;

public class RegularItemsAllocateToTaxItem
{
	public int SetData(Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem item in obj.RegularItemAllocateToTaxItemList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem> lst = new List<Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Tax.Definitions.RegularItemsAllocateToTaxItem>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
