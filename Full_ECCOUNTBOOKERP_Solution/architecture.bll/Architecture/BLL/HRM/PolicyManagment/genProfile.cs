using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PolicyManagment;
using Architecture.Model.HRM.PolicyManagment;

namespace Architecture.BLL.HRM.PolicyManagment;

public class genProfile
{
	public static int Save(Architecture.Model.HRM.PolicyManagment.genProfile obj)
	{
		try
		{
			if (obj.ProfileId == 0)
			{
				return new Architecture.DAL.HRM.PolicyManagment.genProfile().SetData(obj, "Sp_genProfile_Insert");
			}
			return new Architecture.DAL.HRM.PolicyManagment.genProfile().SetData(obj, "Sp_genProfile_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PolicyManagment.genProfile GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProfileId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.PolicyManagment.genProfile>();
			return new Architecture.DAL.HRM.PolicyManagment.genProfile().GetAll("Sp_genProfile_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.PolicyManagment.genProfile obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.genProfile>("Sp_genProfile_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByProfileTypeId(Architecture.Model.HRM.PolicyManagment.genProfile obj)
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
			if (obj.ProfileTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProfileTypeId",
					Value = obj.ProfileTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByProfileTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PolicyManagment.genProfile>("Sp_genProfile_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
