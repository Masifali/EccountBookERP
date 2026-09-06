using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Templates;

namespace Architecture.DAL.Templates;

public class OrganizationTemplates
{
	public int SetData(Architecture.Model.Templates.OrganizationTemplates obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Templates.OrganizationTemplates> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Templates.OrganizationTemplates> lst = new List<Architecture.Model.Templates.OrganizationTemplates>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.OrganizationTemplates>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Templates.OrganizationTemplates> GetById(string ProcName, int Id)
	{
		List<Architecture.Model.Templates.OrganizationTemplates> lst = new List<Architecture.Model.Templates.OrganizationTemplates>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.OrganizationTemplates>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
