using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.OverTimeManagement;
using Architecture.Model;
using Architecture.Model.HRM.ApprovalManagement;
using Architecture.Model.HRM.OverTimeManagement;
using Architecture.Model.Inventory;

namespace Architecture.BLL.HRM.OverTimeManagement;

public class OverTimeRequest
{
	public static long Save(Architecture.Model.HRM.OverTimeManagement.OverTimeRequest obj)
	{
		try
		{
			if (obj.OverTimeRequestId == 0L)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.OverTimeManagement.OverTimeRequest().SetData(obj, "hrm.Sp_OverTimeRequest_Insert");
			}
			obj.ActionTypeId = 2;
			return new Architecture.DAL.HRM.OverTimeManagement.OverTimeRequest().SetData(obj, "hrm.Sp_OverTimeRequest_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.OverTimeManagement.OverTimeRequest GetByID(int Id)
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
				ParameterName = "@OverTimeRequestId",
				Value = Id
			});
			new List<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>();
			return new Architecture.DAL.HRM.OverTimeManagement.OverTimeRequest().GetData("hrm.Sp_OverTimeRequest_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestToDate",
					Value = obj.ToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestFromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateT))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OverTimeToDate",
					Value = obj.GpDateT
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateF))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OverTimeFromDate",
					Value = obj.GpDateF
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
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
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
			if (obj.RequestedById != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestById",
					Value = obj.RequestedById
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>("hrm.Sp_OverTimeRequest_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromOverTimeRequest(int OrganizationId, int CompanyId, string Activity = "")
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
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>("[dbo].[USP_GetDataForDropDownFromOverTimeRequest]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable OTSlipandRegister(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
			if (obj.RequestedById != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestById",
					Value = obj.RequestedById
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestToDate",
					Value = obj.ToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestFromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateT))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OverTimeToDate",
					Value = obj.GpDateT
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateF))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OverTimeFromDate",
					Value = obj.GpDateF
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
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<InvGrn>("[hrm].[USP_OverTimeRequest_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void OvertimeRequestApproval(Architecture.Model.HRM.ApprovalManagement.ApprovalList obj)
	{
		try
		{
			foreach (Architecture.Model.HRM.ApprovalManagement.ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@DetailId",
					Value = item.DetailId
				});
				GenericProvider.GetDataTableProc<Architecture.Model.HRM.OverTimeManagement.OverTimeRequest>("hrm.usp_OvertimeRequestApproval", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
