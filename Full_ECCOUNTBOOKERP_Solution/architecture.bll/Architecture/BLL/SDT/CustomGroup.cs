using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SDT;
using Architecture.Model.SDT;

namespace Architecture.BLL.SDT;

public class CustomGroup
{
	public static int save(Architecture.Model.SDT.CustomGroup obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.SDT.CustomGroup().SetData(obj, "[sdt].[USP_CustomGroup_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory(Architecture.Model.SDT.CustomGroup obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<CustomGroup>("[sdt].[USP_CustomGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCustomGroupForCombo(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@Activity", "GetCustomGroupForCombo"));
			return GenericProvider.GetDataTableProc<CustomGroup>("[sdt].[USP_CustomGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SDT.CustomGroup ReadById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@customGroupId",
				Value = id
			});
			new List<Architecture.Model.SDT.CustomGroup>();
			return new Architecture.DAL.SDT.CustomGroup().GetAll("[sdt].[USP_CustomGroup_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
