using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.Model.Production;

namespace Architecture.BLL.Reports.Party_Processing;

public class ProductionReports
{
	public static DataTable InvFoodProductionPartyProcessingRecoverySummeryReport(ReportsParameters obj)
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
				ParameterName = "@JobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			return GenericProvider.GetDataTableProc<InvFoodProductionPartyProcessing>("Sp_InvFoodProductionPartyProcessing_Summery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_609(ReportsParameters obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.JobOrderId
			});
			return GenericProvider.GetDataTableProc<InvFoodProduction>("Sp_InvFoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
