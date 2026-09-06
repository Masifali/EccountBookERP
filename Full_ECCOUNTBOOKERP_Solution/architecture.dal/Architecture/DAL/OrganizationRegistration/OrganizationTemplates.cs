using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.DAL.OrganizationRegistration;

public class OrganizationTemplates
{
	public int SetData(Architecture.Model.OrganizationRegistration.OrganizationTemplates obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.OrganizationRegistration.OrganizationTemplates> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.OrganizationRegistration.OrganizationTemplates> lst = new List<Architecture.Model.OrganizationRegistration.OrganizationTemplates>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.OrganizationRegistration.OrganizationTemplates>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
