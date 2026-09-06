using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.BLL.HRM.EmployeeManagement;

public class HRM_Reports
{
	public static DataSet DailyAttendanceRpt(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Date",
					Value = obj.DocDate
				});
			}
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
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
			if (obj.EmployeeCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeCategoryId",
					Value = obj.EmployeeCategoryId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsMissing",
					Value = obj.ItemStatus
				});
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_genDailyAttandance_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable MonthlyAttendanceSummary(ReportsParameters obj)
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
				ParameterName = "@Month",
				Value = obj.GetMonth
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.GetYear
			});
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_genEmployeeAttendanceSummery_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet DailyLateandEarlyDeparture(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyLocationId",
					Value = obj.PartyLocationId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Date",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_hrmDailyLED_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable genDSStrengthAttendence(ReportsParameters obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@date",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_genDSStrengthAttendence_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet genDutyRosterEmployeeWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Date",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_genDutyRosterEmployeeWise_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet genEmployeeAttendence(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_genEmployeeAttendence_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EmployeeOverTime(ReportsParameters obj)
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
			if (obj.LoadingLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.LoadingLocationId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_EmployeeOverTime_Register", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EmployeeMonthlyAttendenceRpt(ReportsParameters obj)
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
				ParameterName = "@Month",
				Value = obj.GetMonth
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.GetYear
			});
			if (Conversion.ToInt(obj.EmployeeId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (Conversion.ToInt(obj.DepartmentId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (Conversion.ToInt(obj.SectionId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (Conversion.ToInt(obj.PartyLocationId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyLocationId",
					Value = obj.PartyLocationId
				});
			}
			if (Conversion.ToInt(obj.ShiftId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_genEmployeeMonthlyAttendence_rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable MonthlyAttendanceRegisterRpt(ReportsParameters obj)
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
				ParameterName = "@Month",
				Value = obj.GetMonth
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.GetYear
			});
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
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
			if (obj.SectionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SectionId",
					Value = obj.SectionId
				});
			}
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyLocationId",
					Value = obj.PartyLocationId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AttendanceRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable MonthlyAttendanceRegisterNew(ReportsParameters obj)
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
				ParameterName = "@Month",
				Value = obj.GetMonth
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = obj.GetYear
			});
			if (Conversion.ToInt(obj.EmployeeId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (Conversion.ToInt(obj.DepartmentId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_MonthlyAttendanceRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet EmployeeHistoryRpt(ReportsParameters obj)
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
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchId",
					Value = obj.BranchesId
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
			if (obj.EmployeeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeId",
					Value = obj.EmployeeId
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Active",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_genEmployeeHistory_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EmployeeRegistrationSlipAndRegister(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>("Sp_genEmployee_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
