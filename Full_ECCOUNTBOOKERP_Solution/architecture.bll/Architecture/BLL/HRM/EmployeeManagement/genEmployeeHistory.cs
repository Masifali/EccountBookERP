using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.EmployeeManagement;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class genEmployeeHistory
{
	public static int Save(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
	{
		try
		{
			if (obj.EmployeeHistoryId == 0)
			{
				return new Architecture.DAL.HRM.EmployeeManagement.genEmployeeHistory().SetData(obj, "Sp_genEmployeeHistory_Insert");
			}
			return new Architecture.DAL.HRM.EmployeeManagement.genEmployeeHistory().SetData(obj, "Sp_genEmployeeHistory_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeHistoryId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>();
			return new Architecture.DAL.HRM.EmployeeManagement.genEmployeeHistory().GetAll("Sp_genEmployeeHistory_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllActiveEmployee(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllActiveEmployee"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllEmployeesActive(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.SaleInvoiceDocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentIds",
					Value = obj.SaleInvoiceDocumentTypeIds
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.DesignationId
				});
			}
			if (obj.SectionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (obj.EmployeeGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeGroupId",
					Value = obj.EmployeeGroupId
				});
			}
			if (obj.StoreId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StoreId",
					Value = obj.StoreId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.PartyLocationId
				});
			}
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllActiveEmployee"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable EmployeeLoadForDutyRoaster(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (obj.SectionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (obj.DepartmentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.LocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.LocationId
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeCategoryId",
					Value = obj.DesignationId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "EmployeeLoadForDutyRoaster"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetEmployeesFromEmployeeHistory(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (obj.DepartmentId != 0)
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
				Value = "GetEmployeesFromEmployeeHistory"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllEmployeesActiveFroEIOBPolicy(Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory obj)
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
			if (obj.DepartmentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.DesignationId
				});
			}
			if (obj.SectionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (obj.EmployeeGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeGroupId",
					Value = obj.EmployeeGroupId
				});
			}
			if (obj.StoreId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StoreId",
					Value = obj.StoreId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.PartyLocationId
				});
			}
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllActiveEmployeeForEIOBPolicy"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountsFromEmployee(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("USP_GetAccountsFromEmployee", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
