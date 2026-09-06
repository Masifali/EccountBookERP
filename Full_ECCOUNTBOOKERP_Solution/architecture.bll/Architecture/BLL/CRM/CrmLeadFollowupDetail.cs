using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.CRM;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CRM;

namespace Architecture.BLL.CRM;

public class CrmLeadFollowupDetail
{
	public static int Save(Architecture.Model.CRM.CrmLeadFollowupDetail obj)
	{
		try
		{
			if (obj.LeadFollowupId == 0)
			{
				return new Architecture.DAL.CRM.CrmLeadFollowupDetail().SetData(obj, "Sp_CrmLeadFollowupDetail_Insert");
			}
			return new Architecture.DAL.CRM.CrmLeadFollowupDetail().SetData(obj, "Sp_CrmLeadFollowupDetail_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistory(ReportsParameters obj)
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
			new List<Architecture.Model.CRM.CrmLeadFollowupDetail>();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadFollowupDetail>("Sp_CrmLeadFollowupDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LeadFollowupId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByLeadFollowupId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadFollowupDetail>("Sp_CrmLeadFollowupDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFollowupHistoryByLeadId(ReportsParameters obj)
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
				ParameterName = "@LeadGenHeaderId",
				Value = obj.HeaderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetFollowupHistoryByLeadId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadFollowupDetail>("Sp_CrmLeadFollowupDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable CrmLeadFollowupDetail_SlipAndRegister(ReportsParameters obj)
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
			if (obj.HeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LeadGenHeaderId",
					Value = obj.HeaderId
				});
			}
			if (Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDateFollowUP",
					Value = obj.FromDate
				});
			}
			if (Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDateFollowUP",
					Value = obj.ToDate
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeIdFollowUp",
					Value = obj.SupplierCustomerId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadFollowupDetail>("Sp_CrmLeadFollowupDetail_SlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
