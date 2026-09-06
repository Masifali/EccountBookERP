using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.LoanManagement;
using Architecture.Model;
using Architecture.Model.HRM.LoanManagement;

namespace Architecture.BLL.HRM.LoanManagement;

public class EmployeeAdvance
{
	public static int Save(Architecture.Model.HRM.LoanManagement.EmployeeAdvance obj)
	{
		try
		{
			if (obj.EmployeeAdvanceId == 0)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.LoanManagement.EmployeeAdvance().SetData(obj, "hrm.Sp_EmployeeAdvance_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.LoanManagement.EmployeeAdvance().SetData(obj, "hrm.Sp_EmployeeAdvance_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.LoanManagement.EmployeeAdvance GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeAdvanceId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>();
			return new Architecture.DAL.HRM.LoanManagement.EmployeeAdvance().GetAll("hrm.Sp_EmployeeAdvance_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(ReportsParameters obj)
	{
		try
		{
			int DocNo = 0;
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
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("hrm.Sp_EmployeeAdvance_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.PartyLocationId
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("hrm.Sp_EmployeeAdvance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateEmployeeAdvanceForApproval(Architecture.Model.HRM.LoanManagement.EmployeeAdvance obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeAdvanceId",
				Value = obj.EmployeeAdvanceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ApprovedUserId",
				Value = obj.ApprovedById
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ApprovedAmount",
				Value = obj.ApprovedAmount
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateForEmployeeAdvanceApproval"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("hrm.Sp_EmployeeAdvance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryForApproval(ReportsParameters obj)
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
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DesignationId",
					Value = obj.PartyLocationId
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistoryForApproval"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("hrm.Sp_EmployeeAdvance_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPreviousAdvanceAmount(ReportsParameters obj)
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
				ParameterName = "@EmployeeId",
				Value = obj.EmployeeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = obj.MonthValue
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.GetYear
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("dbo.USP_GetPreviousAdvanceAmount", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable EmployeeAdvanceSlip(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeAdvance>("dbo.USP_EmployeeAdvanceSlip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
