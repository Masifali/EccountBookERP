using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.Model.Steel.Production;

namespace Architecture.BLL.Steel.Production;

public class ProductionReports
{
	public static DataTable FoodProductionSlip(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Steel.Production.InvFoodProduction>("[ST].[USP_FoodProduction_Slip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvFoodProductionOverHeadReportByJobOrder(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionJobOrderId",
				Value = obj.JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProductionOverHeadJobOrderWise_Slip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvFoodProduction_Summery_WithOutValue_Rpt(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionJobOrderId",
				Value = obj.JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProductionSummery_WithOutValue]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProductionSettlement(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionJobOrderId",
				Value = obj.JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = obj.Activity
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProductionSettlement]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
