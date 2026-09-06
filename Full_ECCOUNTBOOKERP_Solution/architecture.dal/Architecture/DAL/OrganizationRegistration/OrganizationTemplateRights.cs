using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.DAL.OrganizationRegistration;

public class OrganizationTemplateRights : DALBase
{
	public int SetData(Architecture.Model.OrganizationRegistration.OrganizationTemplateRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.OrganizationRegistration.OrganizationTemplateRights item in obj.OrganizationTemplateRightsList)
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

	public void DeleteByOrganizationTemplateId(int OrganizationTemplateId, string ScreenIds)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_OrganizationTemplateRights_GetAll");
			sqlDataBase.AddInParameter(cmd, "OrganizationTemplateId", SqlDbType.Int, OrganizationTemplateId);
			sqlDataBase.AddInParameter(cmd, "ScreenIds", SqlDbType.NVarChar, ScreenIds);
			sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "DeleteByOrganizationTemplateId");
			Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
