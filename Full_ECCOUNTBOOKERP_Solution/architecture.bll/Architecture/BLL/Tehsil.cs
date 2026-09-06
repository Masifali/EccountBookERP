using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class Tehsil
{
	public static int save(Architecture.Model.Tehsil obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Tehsil().SetData(obj, "Sp_Tehsil_Insert");
			}
			return new Architecture.DAL.Tehsil().SetData(obj, "Sp_Tehsil_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.Tehsil obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "ReadAll"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Tehsil>("Sp_Tehsil_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByDistrictId(Architecture.Model.Tehsil obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@DistrictId", obj.DistrictId));
			p.Add(new SqlParameter("@Activity", "ReadByDistrictId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Tehsil>("Sp_Tehsil_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
