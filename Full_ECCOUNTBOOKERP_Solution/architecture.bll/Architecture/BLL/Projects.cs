using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class Projects
{
	public static int Save(Architecture.Model.Projects obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Projects().SetData(obj, "Proc_Projects_Insert");
			}
			return new Architecture.DAL.Projects().SetData(obj, "Proc_Projects_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Projects> GetAll(Architecture.Model.Projects pro)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = pro.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = pro.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "GetAll"
			});
			new List<Architecture.Model.Projects>();
			return new Architecture.DAL.Projects().GetAll("Sp_Projects_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAlldt(Architecture.Model.Projects pro)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = pro.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = pro.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "GetAll"
			});
			new List<Architecture.Model.Projects>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Projects>("Sp_Projects_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Projects GetByID(int Id)
	{
		try
		{
			new List<SqlParameter>
			{
				new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				},
				new SqlParameter
				{
					ParameterName = "@MethodType",
					Value = "GetById"
				}
			};
			new List<Architecture.Model.Projects>();
			return new Architecture.DAL.Projects().GetById("Sp_Projects_GetAllMethod", Id)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParentCostCenter()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Projects>("usp_getParentCostCenter", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSubCostCenters(int OrganizationId, int CompanyId, int UserId, int AppId = 0, int ParentCostCenterId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			if (AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = AppId
				});
			}
			if (ParentCostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCostCenterId",
					Value = ParentCostCenterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Projects>("usp_getCostCenters", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
