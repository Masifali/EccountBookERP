using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class DMSPathMaster
{
	public int SetData(Architecture.Model.SystemUtilities.DMSPathMaster obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (DMSPathAllocationToParty item in obj.DMSPathAllocationToPartieslist)
			{
				item.DMSPathAddressId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_DMSPathAllocationToParty_Insert"));
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

	public List<Architecture.Model.SystemUtilities.DMSPathMaster> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SystemUtilities.DMSPathMaster> lst = new List<Architecture.Model.SystemUtilities.DMSPathMaster>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.SystemUtilities.DMSPathMaster>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.SystemUtilities.DMSPathMaster item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.DMSPathAllocationToPartieslist = GenericProvider.GetAllDetail<DMSPathAllocationToParty>(sqlcon, "Sp_DMSPathMaster_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
