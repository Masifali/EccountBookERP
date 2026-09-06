using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genEmployee
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genEmployee obj)
	{
		try
		{
			if (obj.EmployeeId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.genEmployee().SetData(obj, "Sp_genEmployee_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.genEmployee().SetData(obj, "Sp_genEmployee_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genEmployee GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.genEmployee>();
			return new Architecture.DAL.HRM.EmployeeManagement.genEmployee().GetAll("Sp_genEmployee_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genEmployee GetByNo(string EmployeeNo)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeNo",
				Value = EmployeeNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByEmployeeNo"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.genEmployee>();
			return new Architecture.DAL.HRM.EmployeeManagement.genEmployee().GetAll("Sp_genEmployee_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(ReportsParameters obj)
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
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployee_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateEmployeeNo(Architecture.Model.HRM.EmployeeManagement.genEmployee obj)
	{
		try
		{
			int Code = 0;
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
				Value = "GenerateEmployeeNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployee_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["EmployeeNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetEmployeeIdByEmployeeNo(Architecture.Model.HRM.EmployeeManagement.genEmployee obj)
	{
		try
		{
			int EmployeeId = 0;
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
				ParameterName = "@EmployeeNo",
				Value = obj.EmployeeNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEmployeeIdByEmployeeNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployee_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				EmployeeId = Conversion.ToInt(dt.Rows[0]["EmployeeId"]);
			}
			return EmployeeId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCaoByDepartmentId(ReportsParameters obj)
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
				ParameterName = "@DepartmentId",
				Value = obj.DepartmentId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCaoByDepartmentId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployee_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
