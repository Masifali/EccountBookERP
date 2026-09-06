using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class genProfileType
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.genProfileType obj)
	{
		try
		{
			if (obj.ProfileTypeId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.genProfileType().SetData(obj, "Sp_genProfileType_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.genProfileType().SetData(obj, "Sp_genProfileType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.genProfileType GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProfileTypeId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.genProfileType>();
			return new Architecture.DAL.HRM.PolicyManagment.genProfileType().GetAll("Sp_genProfileType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.genProfileType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.genProfileType>("Sp_genProfileType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
