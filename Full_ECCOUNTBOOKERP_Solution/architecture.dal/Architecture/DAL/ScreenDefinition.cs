using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ScreenDefinition
{
	public int SetData(Architecture.Model.ScreenDefinition obj, string ProcName)
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
			foreach (Architecture.Model.ScreenRights item2 in obj.DetailScreenRights)
			{
				item2.ScreenID = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_ScreenRights_Insert"));
			}
			foreach (ApplicationsAllocateToScreen item in obj.ApplicationsAllocateToScreenslist)
			{
				item.ScreenId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USP_ApplicationsAllocateToScreen_Insert"));
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_CompanyRightsAndtblUserRights_Insert]";
			sqlCommand.Parameters.AddWithValue("@ScreenId", obj.Id);
			sqlCommand.Parameters.AddWithValue("@ModuleId", obj.ModuleId);
			sqlCommand.Parameters.AddWithValue("@CompanyIds", obj.CompanyIds);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.ScreenDefinition> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ScreenDefinition> lst = new List<Architecture.Model.ScreenDefinition>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ScreenDefinition>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
