using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class tblUserRights : DALBase
{
	public int SetData(Architecture.Model.tblUserRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			ScreenRightstoUser rightstoUser = new ScreenRightstoUser();
			rightstoUser.UserId = obj.UserId;
			rightstoUser.CompanyId = obj.CompanyId;
			rightstoUser.Activity = "DeleteByUserIdCompanyId";
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, rightstoUser, "Sp_tblUserRights_GetAllMethod"));
			foreach (Architecture.Model.tblUserRights item in obj.TblUserRightslst)
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

	public int RightsUpdateNew(Architecture.Model.tblUserRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.tblUserRights item in obj.TblUserRightslst)
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

	public List<Architecture.Model.tblUserRights> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.tblUserRights> lst = new List<Architecture.Model.tblUserRights>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.tblUserRights>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.tblUserRights> GetViewRightsByUserId(ReportsParameters obj)
	{
		List<Architecture.Model.tblUserRights> tblUserRightslist = new List<Architecture.Model.tblUserRights>();
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_GetUserRightsForView_UserId");
			sqlDataBase.AddInParameter(cmd, "UserId", SqlDbType.Int, obj.EntryUser);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, obj.CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				tblUserRightslist.Add(FillDTO(reader));
			}
			return tblUserRightslist;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	private Architecture.Model.tblUserRights FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.tblUserRights
			{
				ModuleDescription = Conversion.ToString(reader["ModuleDescription"]),
				ModuleID = Conversion.ToInt(reader["ModuleID"]),
				RightName = Conversion.ToString(reader["RightName"]),
				RightsID = Conversion.ToInt(reader["RightsID"]),
				ScreenID = Conversion.ToInt(reader["ScreenID"]),
				ScreenName = Conversion.ToString(reader["ScreenName"]),
				ScreenAlias = Conversion.ToString(reader["ScreenAlias"]),
				Value = Conversion.ToBool(reader["Value"]),
				MenuControllName = Conversion.ToString(reader["MenuControllName"]),
				ModelMenuControllName = Conversion.ToString(reader["ModelMenuControllName"]),
				IsFavorite = Conversion.ToBool(reader["IsFavorite"]),
				TargetUrl = Conversion.ToString(reader["TargetUrl"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
