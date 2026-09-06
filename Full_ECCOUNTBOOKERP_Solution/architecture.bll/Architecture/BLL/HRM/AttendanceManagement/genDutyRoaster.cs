using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.AttendanceManagement;
using Architecture.Model;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.BLL.HRM.AttendanceManagement;

public class genDutyRoaster
{
	public static long Save(Architecture.Model.HRM.AttendanceManagement.genDutyRoaster obj)
	{
		try
		{
			if (obj.DutyRoasterId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.AttendanceManagement.genDutyRoaster().SetData(obj, "Sp_genDutyRoaster_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.AttendanceManagement.genDutyRoaster().SetData(obj, "Sp_genDutyRoaster_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.AttendanceManagement.genDutyRoaster GetByID(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DutyRoasterId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>();
			return new Architecture.DAL.HRM.AttendanceManagement.genDutyRoaster().GetAll("Sp_genDutyRoaster_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistoryHeaderData(Architecture.Model.HRM.AttendanceManagement.genDutyRoaster obj)
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
				Value = "DutyRoasterFormHistoryHeader"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("Sp_genDutyRoaster_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DutyRoaster_FormHistory(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (obj.DutyRoasterFrom != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DutyRoasterFrom",
					Value = obj.DutyRoasterFrom
				});
			}
			if (obj.DutyRoasterTo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DutyRoasterTo",
					Value = obj.DutyRoasterTo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("[dbo].[USP_DutyRoaster_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistoryDetailData(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DutyRoasterFormHistoryDetail"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("Sp_genDutyRoaster_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetDutyDatesForDutyRoaster(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.DutyRoasterId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DutyRoasterId",
					Value = obj.DutyRoasterId
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
			if (!string.IsNullOrEmpty(obj.SaleInvoiceDocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentIds",
					Value = obj.SaleInvoiceDocumentTypeIds
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
			if (obj.EmployeeCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeCategoryId",
					Value = obj.EmployeeCategoryId
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
			if (obj.PartyLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LocationId",
					Value = obj.PartyLocationId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("SP_GetDutyDatesForDutyRoaster", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetoffdaysfromDutyRoasterDetailByHeaderId(long DutyRoasterId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DutyRoasterId",
				Value = DutyRoasterId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetoffdaysfromDutyRoasterDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("Sp_genDutyRoaster_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetEmployeesByDutyRosterId(int DutyRoasterId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DutyRoasterId",
				Value = DutyRoasterId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEmployeesByDutyRosterId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>("Sp_genDutyRoaster_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
