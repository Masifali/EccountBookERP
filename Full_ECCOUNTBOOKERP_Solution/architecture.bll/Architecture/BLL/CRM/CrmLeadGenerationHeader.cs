using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.CRM;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CRM;

namespace Architecture.BLL.CRM;

public class CrmLeadGenerationHeader
{
	public static int Save(Architecture.Model.CRM.CrmLeadGenerationHeader obj)
	{
		try
		{
			if (obj.LeadGenHeaderId == 0L)
			{
				return new Architecture.DAL.CRM.CrmLeadGenerationHeader().SetData(obj, "Sp_CrmLeadGenerationHeader_Insert");
			}
			return new Architecture.DAL.CRM.CrmLeadGenerationHeader().SetData(obj, "Sp_CrmLeadGenerationHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenrateCode(Architecture.Model.CRM.CrmLeadGenerationHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "GenerateCode"));
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadGenerationHeader>("Sp_CrmLeadGenerationHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CRM.CrmLeadGenerationHeader GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LeadGenHeaderId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.CRM.CrmLeadGenerationHeader>();
			return new Architecture.DAL.CRM.CrmLeadGenerationHeader().GetDate("Sp_CrmLeadGenerationHeader_GetAllMethod", p)[0];
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
				ParameterName = "@FilterType",
				Value = obj.ReqType
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LeadGenHeaderId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			new List<Architecture.Model.CRM.CrmLeadGenerationHeader>();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmContactHeader>("Sp_CrmLeadGenerationHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getContactNoByLeadGenerateID(Architecture.Model.CRM.CrmLeadGenerationHeader obj)
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
				Value = obj.LeadGenHeaderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByContactId"
			});
			new List<Architecture.Model.CRM.CrmLeadGenerationHeader>();
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CrmLeadGenerationHeader>("Sp_CrmLeadGenerationHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
