using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Reports.Accounts;

public class GeneralReprots
{
	public static DataSet DueDateAnalysisPayablesAndReceivablesForcast(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DaysInterval",
					Value = obj.IntervalDays
				});
			}
			if (obj.SortNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SortNo",
					Value = obj.SortNo
				});
			}
			return GenericProvider.GetDataSetProc<VoucherHead>("[dbo].[usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PdcInventorySlip(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_PdcInventoryHeader_rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PdcReceiptsRegister(ReportsParameters obj)
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GLCrAccountId",
					Value = obj.AccountId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.CheqDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFromCheq",
					Value = obj.CheqDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.CheqDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateToCheq",
					Value = obj.CheqDateTo
				});
			}
			if (obj.ChartOfAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GLDrAccountId",
					Value = obj.ChartOfAccountId
				});
			}
			if (obj.Status != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqStatus",
					Value = obj.Status
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("Sp_PdcInventory_SlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PdcReceiptsRegisterSummery(ReportsParameters obj)
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GLCrAccountId",
					Value = obj.AccountId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.CheqDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFromCheq",
					Value = obj.CheqDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.CheqDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateToCheq",
					Value = obj.CheqDateTo
				});
			}
			if (obj.Status != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqStatus",
					Value = obj.Status
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("Sp_PdcInventory_ReceiptsSummery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LatestCurrencyRates(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<PdcInventory>("SpVouchers_GetLatestCurrencyRates", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InventoryPayablesandReceivables(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@TranTypes",
				Value = obj.TranTypes
			});
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
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.AccountTypeIds
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.Ids
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportType",
					Value = obj.ReportTypeId
				});
			}
			if (obj.Debit != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ClDebit",
					Value = obj.Debit
				});
			}
			if (obj.Credit != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ClCredit",
					Value = obj.Credit
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomeGroupId",
					Value = obj.CustomerGroupId
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("SpAccounts_InventoryPayablesandReceivables_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesandReceivablesMenagment(ReportsParameters obj)
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
					ParameterName = "@EndDate",
					Value = obj.ToDate
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IntervalDays",
					Value = obj.IntervalDays
				});
			}
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CBalNotEqualTo",
					Value = obj.DocNo
				});
			}
			if (obj.ItemClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportClassification",
					Value = obj.ItemClassId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportType",
					Value = obj.ItemTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("SpAccounts_PayablesReceivablesAging_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralLedgerCommissionAndBrokery(ReportsParameters obj)
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
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_CommissionAndBrokery_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DayBookSlip(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipCashTrans",
					Value = obj.ZeroBalanceType
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDate",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_DayBookSlip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesAndReceivablesWithPaymentAndReceipts(ReportsParameters obj)
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
			if (!string.IsNullOrWhiteSpace(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ClassId",
					Value = obj.AccouuntClassId
				});
			}
			if (obj.ParentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountId",
					Value = obj.ParentId
				});
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupId",
					Value = obj.CustomGroupId
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.AreaCity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityName",
					Value = obj.AreaCity
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("usp_getPayablesAndReceivablesWithPaymentAndReceipts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
