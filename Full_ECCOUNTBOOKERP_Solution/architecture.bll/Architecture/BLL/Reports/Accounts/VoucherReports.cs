using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Api;
using Architecture.Model.Export;

namespace Architecture.BLL.Reports.Accounts;

public class VoucherReports
{
	public static DataTable VoucherReport(ReportsParameters obj)
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
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherSlipForInventoryReport(ReportsParameters obj)
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
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable NewVoucherReprot(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeF",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeT",
					Value = obj.ToDocNo
				});
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_PaymentReceiptVoucherSlipNew_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SummaryVoucherReprot(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeF",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeT",
					Value = obj.ToDocNo
				});
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_PartyPaymentAndReceiptSummary_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PaymentByInvoiceNewSlip(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_Payment&ReceipteByInvoiceVoucherSlipNew_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportInvoiceVoucher(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_ExportInvoiceVoucherSlipNew_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TrialBalanceReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			p.Add(new SqlParameter("@ZeroBalanceType", obj.ZeroBalanceType));
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.DocumentTypeIds));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.Debit != 0.0)
			{
				p.Add(new SqlParameter("@ClDebit", obj.Debit));
			}
			if (obj.Credit != 0.0)
			{
				p.Add(new SqlParameter("@ClCredit", obj.Credit));
			}
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("AcTypeIds", obj.AccountTypeIds));
			}
			if (obj.GroupAccountId != 0)
			{
				p.Add(new SqlParameter("@GroupAccountId", obj.GroupAccountId));
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomeGroupId", obj.CustomGroupId));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.SkipCgsAccounts != 0)
			{
				p.Add(new SqlParameter("@SkipCGS", obj.SkipCgsAccounts));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_TrialBalance_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TrialBalanceNewReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			p.Add(new SqlParameter("@ZeroBalanceType", obj.ZeroBalanceType));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_TrialBalances_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TrialBalancesUpTo5thLevel(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			p.Add(new SqlParameter("@ZeroBalanceType", obj.ZeroBalanceType));
			p.Add(new SqlParameter("@ClDebit", obj.Debit));
			p.Add(new SqlParameter("@ClCredit", obj.Credit));
			return GenericProvider.GetDataTableProc<ReportsParameters>("SpAccounts_TrialBalancesUpTo5thLevel", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable HararicalTrialBalance(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter("@AccountClass", obj.Activity));
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter("@SkipZero", obj.SkipZero));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("SpCoahierarchy_TrialBalance_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherValidationReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter("@VoucherCodeF", obj.FromDocNo));
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter("@VoucherCodeT", obj.ToDocNo));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter("@EntryDateFrom", obj.EntryDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter("@EntryDateTo", obj.EntryDateTo));
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomGroupId", obj.CustomerGroupId));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.SaleInvoiceDocumentTypeIds != null && obj.SaleInvoiceDocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.SaleInvoiceDocumentTypeIds));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.ManualBillNo != null && obj.ManualBillNo != string.Empty)
			{
				p.Add(new SqlParameter("@ManualBillNo", obj.ManualBillNo));
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@IsCGS", obj.ActionId));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Accounts_VouchersValidation_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SelectedTrialBalance(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter("@CityId", obj.CityId));
			}
			p.Add(new SqlParameter("@GroupAccountId", obj.GroupAccountId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AcTrialBalanceSelectedGroupAccount_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet SelectedTrialBalanceNew(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter("@CityId", obj.CityId));
			}
			if (obj.GroupAccountId != 0)
			{
				p.Add(new SqlParameter("@GroupAccountId", obj.GroupAccountId));
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomeGroupId", obj.CustomerGroupId));
			}
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter("@ZeroBalanceType", obj.ZeroBalanceType));
			}
			if (obj.SkipCgsAccounts != 0)
			{
				p.Add(new SqlParameter("@SkipCgsAccounts", obj.SkipCgsAccounts));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.DocumentTypeIds));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.Debit != 0.0)
			{
				p.Add(new SqlParameter("@ClDebit", obj.Debit));
			}
			if (obj.Credit != 0.0)
			{
				p.Add(new SqlParameter("@ClCredit", obj.Credit));
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("SpAccounts_TrialBalanceSelectedNew_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SelectedTrialBalanceOld(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter("@CityId", obj.CityId));
			}
			if (obj.GroupAccountId != 0)
			{
				p.Add(new SqlParameter("@GroupAccountId", obj.GroupAccountId));
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomeGroupId", obj.CustomerGroupId));
			}
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter("@ZeroBalanceType", obj.ZeroBalanceType));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.DocumentTypeIds));
			}
			return GenericProvider.GetDataTableProcSetTime<ReportsParameters>("SpAccounts_TrialBalanceSelected_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralLedger(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			p.Add(new SqlParameter("@AccountId", obj.AccountId));
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GeneralLedger_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralLedgerWithOffsetAccount(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@EndDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.AccountIds != null || obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountIds", obj.AccountIds));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.EntryUser != 0)
			{
				p.Add(new SqlParameter("@EntryUser", obj.EntryUser));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryAccountId", obj.SubsidiaryAccountId));
			}
			if (obj.SubsidiaryTypeId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryTypeId", obj.SubsidiaryTypeId));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			return GenericProvider.GetDataTableProcSetTime<ReportsParameters>("Sp_Accounts_GeneralLedger_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AccountsLedgerByJobOrder(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter("@JobOrderId", obj.JobOrderId));
			}
			if (obj.JobOrderIds != null || obj.JobOrderIds != string.Empty)
			{
				p.Add(new SqlParameter("@JobOrderIds", obj.JobOrderIds));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			return GenericProvider.GetDataTableProcSetTime<ReportsParameters>("usp_AccountsLedgerByJobOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Accounts_FcyGeneralLedger_Rpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@EndDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.AccountIds != null || obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountIds", obj.AccountIds));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.EntryUser != 0)
			{
				p.Add(new SqlParameter("@EntryUser", obj.EntryUser));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryAccountId", obj.SubsidiaryAccountId));
			}
			if (obj.SubsidiaryTypeId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryTypeId", obj.SubsidiaryTypeId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Usp_Accounts_FcyGeneralLedger_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet Accounts_GcyGeneralLedger_Rpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@EndDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.EntryUser != 0)
			{
				p.Add(new SqlParameter("@EntryUser", obj.EntryUser));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryAccountId", obj.SubsidiaryAccountId));
			}
			if (obj.SubsidiaryTypeId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryTypeId", obj.SubsidiaryTypeId));
			}
			if (obj.AccountIds != null || obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountIds", obj.AccountIds));
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Usp_Accounts_gcyGeneralLedger_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GuantativeLedger(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", clsGlobalVariables.ActiveYr.Id));
			p.Add(new SqlParameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			p.Add(new SqlParameter("@AccountId", obj.AccountId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Voucher_GLQuantitativeLedgerA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralLedgerSummery(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.OrderNo != string.Empty && obj.OrderNo != null)
			{
				p.Add(new SqlParameter("@ParentAccountCode", obj.OrderNo));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryAccountId", obj.SubsidiaryAccountId));
			}
			if (obj.SubsidiaryTypeId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryTypeId", obj.SubsidiaryTypeId));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_VouchersAccountsGeneralLedgerSummery", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable BankSummaryLedgersWithAgainstsAccounts(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountIds != null && obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountIds", obj.AccountIds));
			}
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountTypeIds", obj.AccountTypeIds));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_BankSummaryWithAgainstAccounts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralLedgerStatement(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", clsGlobalVariables.ActiveYr.Id));
			p.Add(new SqlParameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			p.Add(new SqlParameter("@AccountId", obj.AccountId));
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@ReportType", "Account Statment By Date"));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GeneralLedgerStatement_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ActivitySummaryReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter("@EntryDateFrom", obj.EntryDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter("@EntryDateTo", obj.EntryDateTo));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
			}
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountTypeIds", obj.AccountTypeIds));
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.Ids));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter("@ReportTypeId", obj.ReportTypeId));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_ActivityReportSummery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ActivitySummaryDocumentTypeWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter("@EntryDateFrom", obj.EntryDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter("@EntryDateTo", obj.EntryDateTo));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountTypeIds", obj.AccountTypeIds));
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.Ids));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter("@ReportTypeId", obj.ReportTypeId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_ActivityReportSummeryDocumentTypeWise_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ActivitySummaryDetail(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter("@EntryDateFrom", obj.EntryDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter("@EntryDateTo", obj.EntryDateTo));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_ActivityDetail_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReceivableAging(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter("@IntervalDays", obj.IntervalDays));
			}
			if (!Conversion.CheckDateTimeNull(obj.EndDate))
			{
				p.Add(new SqlParameter("@EndDate", obj.EndDate));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_ReceivablesAging_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesAging(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter("@IntervalDays", obj.IntervalDays));
			}
			if (!Conversion.CheckDateTimeNull(obj.EndDate))
			{
				p.Add(new SqlParameter("@EndDate", obj.EndDate));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_PayableAging_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShowOnlyTrade",
					Value = obj.ActionId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
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
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ControlAccountIds",
					Value = obj.ControlAccountIds
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
			if (obj.CustomerGroupIds != null && obj.CustomerGroupIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.CustomerGroupIds
				});
			}
			if (obj.BalanceFrom != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.BalanceFrom
				});
			}
			if (obj.BalanceTo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.BalanceTo
				});
			}
			if (obj.ShowAssetLiability != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShowAssetLiability",
					Value = obj.ShowAssetLiability
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_Payables_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet PayablesAndPaymentSchedule(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			p.Add(new SqlParameter
			{
				ParameterName = "@ParentCategoryId",
				Value = obj.InventoryParentCategories
			});
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter("@DueDateFrom", obj.DueDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter("@DueDateTo", obj.DueDateTo));
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter("@PurchaseFromDate", obj.EffectedDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDateTo))
			{
				p.Add(new SqlParameter("@PurchaseToDate", obj.EffectedDateTo));
			}
			if (obj.BalanceFrom != 0.0)
			{
				p.Add(new SqlParameter("@BalanceFrom", obj.BalanceFrom));
			}
			if (obj.BalanceTo != 0.0)
			{
				p.Add(new SqlParameter("@BalanceTo", obj.BalanceTo));
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomGroupId", obj.CustomGroupId));
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter("@PartyGroupId", obj.CustomerGroupId));
			}
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ControlAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@ActionId", obj.ActionId));
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TradeTypeId",
					Value = obj.ReportTypeId
				});
			}
			if (obj.AgingDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AgingDays",
					Value = obj.AgingDays
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
			return GenericProvider.GetDataSetProc<ReportsParameters>("usp_PayablesAndPaymentSchedule", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReceivablesAndReceiptsSchedule(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			p.Add(new SqlParameter("@ParentCategoryId", obj.InventoryParentCategories));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter("@DueDateFrom", obj.DueDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter("@DueDateTo", obj.DueDateTo));
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter("@SaleFromDate", obj.EffectedDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDateTo))
			{
				p.Add(new SqlParameter("@SaleToDate", obj.EffectedDateTo));
			}
			if (obj.BalanceFrom != 0.0)
			{
				p.Add(new SqlParameter("@BalanceFrom", obj.BalanceFrom));
			}
			if (obj.BalanceTo != 0.0)
			{
				p.Add(new SqlParameter("@BalanceTo", obj.BalanceTo));
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter("@CustomGroupId", obj.CustomGroupId));
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter("@PartyGroupId", obj.CustomerGroupId));
			}
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ControlAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@ActionId", obj.ActionId));
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter("@TradeTypeId", obj.ReportTypeId));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("usp_ReceivablesAndReceiptsSchedule", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReceivablesReport(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountType",
					Value = obj.AccountId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountCode",
					Value = obj.Status
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShowOnlyTrade",
					Value = obj.ActionId
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
			if (obj.AccountTypeIds != null && obj.AccountTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.AccountTypeIds
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			if (obj.ShowAssetLiability != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShowAssetLiability",
					Value = obj.ShowAssetLiability
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Accounts_Receivables_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LedgerAging(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.EndDate))
			{
				p.Add(new SqlParameter("@AsOnDate", obj.EndDate));
			}
			if (obj.AgingDays != 0)
			{
				p.Add(new SqlParameter("@AgingDays", obj.AgingDays));
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter("@NoOfInternal", obj.IntervalDays));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@ActionId", obj.ActionId));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter("@AccouuntClassId", obj.AccouuntClassId));
			}
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter("@SkipZero", obj.ZeroBalanceType));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("SpAccounts_LedgerAging", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FcyReceiptsSummaryRegister(ReportsParameters obj)
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
					ParameterName = "@BranchId",
					Value = obj.BranchesId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SenderAccountId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReceiverAccountId",
					Value = obj.AccountId
				});
			}
			return GenericProvider.GetDataTableProc<ExImFCBankReceipts>("USP_FcyReceiptsSummaryRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet TradeDebtorsAndCreditors_Report(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyIdCoa",
				Value = obj.CwCoaId
			});
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccouuntClassId",
					Value = obj.AccouuntClassId
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountCode",
					Value = obj.Status
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupId",
					Value = obj.CustomGroupId
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.TradeTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TradeTypeId",
					Value = obj.TradeTypeId
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReprotTypeId",
					Value = obj.ReportTypeId
				});
			}
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportType",
					Value = obj.ReportType
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CoaDetailAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SubsidoryAccountId",
					Value = obj.SubsidiaryAccountId
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			return GenericProvider.GetDataSetProc<VoucherHead>("SpAccounts_TradeDebtorsAndCreditors_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesWithLastBillAndPaidAmount(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountType",
					Value = obj.AccountId
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountCode",
					Value = obj.Status
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
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
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Accounts_PayablesWithLastBillAndPaidAmount_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ChartofAccount(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (!string.IsNullOrEmpty(obj.ApprovedFilter) && obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsActive", obj.IsApproved));
			}
			if (!string.IsNullOrEmpty(obj.AccountLevels))
			{
				p.Add(new SqlParameter("@AccountLevels", obj.AccountLevels));
			}
			if (!string.IsNullOrEmpty(obj.CustomGroupIds))
			{
				p.Add(new SqlParameter("@CustomGroupIds", obj.CustomGroupIds));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_ChartOfAccounts_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable OpeningBalanceReport(AccountsOpeningBalances obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AccountsOpeningBalance_Slip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GeneralJournalReport(ReportsParameters obj)
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
			if (Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_GeneralJournal_SummeryRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable AccountsBalanceSheetStandardRpt(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("Sp_Accounts_BalanceSheetStandard_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable AccountsBalanceSheetStandardFormatII(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.AccountNoteId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountNoteId",
					Value = obj.AccountNoteId
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
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("SpAccounts_BalanceSheetFormatA_Report", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<ChartofAccount> AccountsBalanceSheetStandardRptlst(ReportsParameters obj)
	{
		try
		{
			List<ChartofAccount> lst = new List<ChartofAccount>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = clsGlobalVariables.ActiveYr.Id
			});
			if (Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			DataTable dt = new DataTable();
			dt = GenericProvider.GetDataTableProc<COAAllocation>("Sp_Accounts_BalanceSheetandProfilLoseStandard_History", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					ChartofAccount reports = new ChartofAccount();
					reports.Account_Level = Conversion.ToInt(dt.Rows[i]["Level"]);
					reports.AccountTitleOtherLingo = Conversion.ToString(dt.Rows[i]["AccountsType"]);
					reports.AccountTitle = Conversion.ToString(dt.Rows[i]["AccountTitle"]);
					reports.AccountCode = Conversion.ToString(dt.Rows[i]["AccountCode"]);
					reports.YearObCredit = Conversion.ToDouble(dt.Rows[i]["Amount"]);
					lst.Add(reports);
				}
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable AccountsBalanceSheetStandardForCrystal(ReportsParameters obj)
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
				ParameterName = "@ClassIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.AccountNoteId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountNoteId",
					Value = obj.AccountNoteId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("SpAccounts_BalanceSheetFormatA_ForCrystal", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable Accounts_ProfitLoss_ForCrystal(ReportsParameters obj)
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
				ParameterName = "@ClassIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.AccountNoteId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountNoteId",
					Value = obj.AccountNoteId
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
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("SpAccounts_ProfitLoss_ForCrystal", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable AccountsProfitLoassStandard(ReportsParameters obj)
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
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("Sp_Accounts_ProfitLoassStandard_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ProfitandLoss(ReportsParameters obj)
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
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("SpAccounts_ProfitLoassFormatA_Report", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ProfitAndLoss_New(ReportsParameters obj)
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
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
			return GenericProvider.GetDataTableProc<PdcInventory>("usp_ProfitAndLoss_New", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProftLoss(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.AccountNoteId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountNoteId",
					Value = obj.AccountNoteId
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
			return GenericProvider.GetDataTableProc<PdcInventory>("SpAccounts_ProfitLoassFormatA_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable BankBalances(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter("@ReportType", obj.ReportType));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_BankBalances_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable Vouchers_GetLastRecord(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_Vouchers_GetLastRecord]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable BankBalancesReceipts(ReportsParameters obj)
	{
		try
		{
			DataTable dtReciptandPayment = BankBalances(obj);
			DataTable dtRCol = new DataTable();
			dtRCol.Columns.Add("Date");
			dtRCol.Columns.Add("Id");
			dtRCol.Columns.Add("DocumentTypeId");
			dtRCol.Columns.Add("DocumentTypeSrNo");
			dtRCol.Columns.Add("DocumentTypeCode");
			dtRCol.Columns.Add("VoucherCode");
			dtRCol.Columns.Add("BankName");
			dtRCol.Columns.Add("RecivedFrom");
			dtRCol.Columns.Add("ChequeNo");
			dtRCol.Columns.Add("Amount", typeof(double));
			if (dtReciptandPayment.Rows.Count > 0)
			{
				for (int i = 0; i < dtReciptandPayment.Rows.Count; i++)
				{
					if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString("Receipts"))
					{
						dtRCol.Rows.Add(Conversion.ToDateTime(dtReciptandPayment.Rows[i]["voucherdate"]).ToString("dd-MMM-yyyy"), dtReciptandPayment.Rows[i]["Id"], dtReciptandPayment.Rows[i]["DocumentTypeId"], dtReciptandPayment.Rows[i]["DocumentTypeSrNo"], dtReciptandPayment.Rows[i]["DocumentTypeCode"], dtReciptandPayment.Rows[i]["Vouchercode"], dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["OffsetAccountTitle"], dtReciptandPayment.Rows[i]["ChequeNo"], dtReciptandPayment.Rows[i]["DebitAmount"]);
					}
				}
			}
			return dtRCol;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable BankBalancesPayments(ReportsParameters obj)
	{
		try
		{
			DataTable dtReciptandPayment = BankBalances(obj);
			DataTable dtPCol = new DataTable();
			dtPCol.Columns.Add("Date");
			dtPCol.Columns.Add("Id");
			dtPCol.Columns.Add("DocumentTypeId");
			dtPCol.Columns.Add("DocumentTypeSrNo");
			dtPCol.Columns.Add("DocumentTypeCode");
			dtPCol.Columns.Add("VoucherCode");
			dtPCol.Columns.Add("BankName");
			dtPCol.Columns.Add("PaidTo");
			dtPCol.Columns.Add("ChequeNo");
			dtPCol.Columns.Add("Amount", typeof(double));
			if (dtReciptandPayment.Rows.Count > 0)
			{
				for (int i = 0; i < dtReciptandPayment.Rows.Count; i++)
				{
					if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString("Payments"))
					{
						dtPCol.Rows.Add(Conversion.ToDateTime(dtReciptandPayment.Rows[i]["voucherdate"]).ToString("dd-MMM-yyyy"), dtReciptandPayment.Rows[i]["Id"], dtReciptandPayment.Rows[i]["DocumentTypeId"], dtReciptandPayment.Rows[i]["DocumentTypeSrNo"], dtReciptandPayment.Rows[i]["DocumentTypeCode"], dtReciptandPayment.Rows[i]["Vouchercode"], dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["OffsetAccountTitle"], dtReciptandPayment.Rows[i]["ChequeNo"], dtReciptandPayment.Rows[i]["DebitAmount"]);
					}
				}
			}
			return dtPCol;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<AccountTitlesModel> BankBalancesPayments1(ReportsParameters obj)
	{
		try
		{
			List<AccountTitlesModel> MainList = new List<AccountTitlesModel>();
			DataTable dtReciptandPayment = BankBalances(obj);
			if (dtReciptandPayment.Rows.Count > 0)
			{
				foreach (var item in from row in dtReciptandPayment.AsEnumerable()
					where Conversion.ToString(row["TranType"]) == "Payments"
					group row by row.Field<string>("AccountTitle") into grp
					select new
					{
						AccountTitle = grp.Key,
						TotalAmount = grp.Sum((DataRow r) => Convert.ToDouble(r["DebitAmount"])),
						Rows = grp
					})
				{
					AccountTitlesModel Main = new AccountTitlesModel();
					Main.AccountTitle = item.AccountTitle;
					List<VoucherNoModel> VoucherNoList = new List<VoucherNoModel>();
					foreach (var itemg in from row in dtReciptandPayment.Select("AccountTitle = '" + item.AccountTitle + "' and TranType='Payments'").CopyToDataTable().AsEnumerable()
						group row by new
						{
							VoucherType = row.Field<string>("DocumentTypeCode"),
							VoucherCode = row.Field<int>("VoucherCode")
						} into grp
						select new
						{
							grp.Key.VoucherType,
							grp.Key.VoucherCode
						})
					{
						VoucherNoModel VoucherNo = new VoucherNoModel();
						DataTable VoucherTypeResult = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "'  and TranType='Payments'").CopyToDataTable();
						for (int j = 0; j < 1; j++)
						{
							VoucherNo.VoucherType = VoucherTypeResult.Rows[j]["DocumentTypeCode"].ToString();
							VoucherNo.VoucherDate = Convert.ToDateTime(VoucherTypeResult.Rows[j]["voucherdate"].ToString());
						}
						VoucherNo.VoucherCode = itemg.VoucherCode;
						DataTable voucherDetail = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "' and TranType='Payments'").CopyToDataTable();
						List<VoucherDetailModel> VoucherDetailList = new List<VoucherDetailModel>();
						for (int i = 0; i < voucherDetail.Rows.Count; i++)
						{
							VoucherDetailModel VoucherDetail = new VoucherDetailModel();
							VoucherDetail.OffsetAccount = voucherDetail.Rows[i]["OffsetAccountTitle"].ToString();
							VoucherDetail.Amount = Convert.ToDouble(voucherDetail.Rows[i]["DebitAmount"].ToString());
							VoucherDetailList.Add(VoucherDetail);
						}
						VoucherNo.Data = VoucherDetailList;
						VoucherNoList.Add(VoucherNo);
					}
					Main.Data = VoucherNoList;
					MainList.Add(Main);
				}
			}
			return MainList;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<AccountTitlesModel> BankBalancesReceipts1(ReportsParameters obj)
	{
		try
		{
			List<AccountTitlesModel> MainList = new List<AccountTitlesModel>();
			DataTable dtReciptandPayment = BankBalances(obj);
			if (dtReciptandPayment.Rows.Count > 0)
			{
				foreach (var item in from row in dtReciptandPayment.AsEnumerable()
					where Conversion.ToString(row["TranType"]) == "Receipts"
					group row by row.Field<string>("AccountTitle") into grp
					select new
					{
						AccountTitle = grp.Key,
						TotalAmount = grp.Sum((DataRow r) => Convert.ToDouble(r["DebitAmount"])),
						Rows = grp
					})
				{
					AccountTitlesModel Main = new AccountTitlesModel();
					Main.AccountTitle = item.AccountTitle;
					List<VoucherNoModel> VoucherNoList = new List<VoucherNoModel>();
					foreach (var itemg in from row in dtReciptandPayment.Select("AccountTitle = '" + item.AccountTitle + "' and TranType='Receipts'").CopyToDataTable().AsEnumerable()
						group row by new
						{
							VoucherType = row.Field<string>("DocumentTypeCode"),
							VoucherCode = row.Field<int>("VoucherCode")
						} into grp
						select new
						{
							grp.Key.VoucherType,
							grp.Key.VoucherCode
						})
					{
						VoucherNoModel VoucherNo = new VoucherNoModel();
						DataTable VoucherTypeResult = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "'  and TranType='Receipts'").CopyToDataTable();
						for (int j = 0; j < 1; j++)
						{
							VoucherNo.VoucherType = VoucherTypeResult.Rows[j]["DocumentTypeCode"].ToString();
							VoucherNo.VoucherDate = Convert.ToDateTime(VoucherTypeResult.Rows[j]["voucherdate"].ToString());
						}
						VoucherNo.VoucherCode = itemg.VoucherCode;
						DataTable voucherDetail = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "' and TranType='Receipts'").CopyToDataTable();
						List<VoucherDetailModel> VoucherDetailList = new List<VoucherDetailModel>();
						for (int i = 0; i < voucherDetail.Rows.Count; i++)
						{
							VoucherDetailModel VoucherDetail = new VoucherDetailModel();
							VoucherDetail.OffsetAccount = voucherDetail.Rows[i]["OffsetAccountTitle"].ToString();
							VoucherDetail.Amount = Convert.ToDouble(voucherDetail.Rows[i]["DebitAmount"].ToString());
							VoucherDetailList.Add(VoucherDetail);
						}
						VoucherNo.Data = VoucherDetailList;
						VoucherNoList.Add(VoucherNo);
					}
					Main.Data = VoucherNoList;
					MainList.Add(Main);
				}
			}
			return MainList;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet PartiesPaymentAndReceipts(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter("@TransType", obj.Activity));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("USP_PartiesPaymentAndReceipts", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PaymentAndReceipts_CustomersAndSupplirWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter("@TransType", obj.Activity));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_PaymentAndReceipts_CustomersAndSupplirWise", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable CashBalances(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId > 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter("@ReportType", obj.ReportType));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_CashBalances_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable CashBalancesPayments(ReportsParameters obj)
	{
		try
		{
			DataTable dtReciptandPayment = CashBalances(obj);
			DataTable dt = new DataTable();
			dt.Columns.Add("TranType");
			dt.Columns.Add("DocumentTypeCode");
			dt.Columns.Add("VoucherCode");
			dt.Columns.Add("Id");
			dt.Columns.Add("DocumentTypeId");
			dt.Columns.Add("DocumentTypeSrNo");
			dt.Columns.Add("voucherdate");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("OffsetAccountTitle");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("NoOfAttachments");
			if (dtReciptandPayment.Rows.Count > 0)
			{
				for (int i = 0; i < dtReciptandPayment.Rows.Count; i++)
				{
					if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString("Payments"))
					{
						dt.Rows.Add(dtReciptandPayment.Rows[i]["TranType"], dtReciptandPayment.Rows[i]["DocumentTypeCode"], dtReciptandPayment.Rows[i]["VoucherCode"], dtReciptandPayment.Rows[i]["Id"], dtReciptandPayment.Rows[i]["DocumentTypeId"], dtReciptandPayment.Rows[i]["DocumentTypeSrNo"], Conversion.ToDateTime(dtReciptandPayment.Rows[i]["voucherdate"]).ToString("dd-MMM-yyyy"), dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["OffsetAccountTitle"], dtReciptandPayment.Rows[i]["DebitAmount"], dtReciptandPayment.Rows[i]["NoOfAttachments"]);
					}
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable CashBalancesReceipts(ReportsParameters obj)
	{
		try
		{
			DataTable dtReciptandPayment = CashBalances(obj);
			DataTable dt1 = new DataTable();
			dt1.Columns.Add("TranType");
			dt1.Columns.Add("DocumentTypeCode");
			dt1.Columns.Add("VoucherCode");
			dt1.Columns.Add("Id");
			dt1.Columns.Add("DocumentTypeId");
			dt1.Columns.Add("DocumentTypeSrNo");
			dt1.Columns.Add("voucherdate");
			dt1.Columns.Add("AccountTitle");
			dt1.Columns.Add("OffsetAccountTitle");
			dt1.Columns.Add("Amount", typeof(double));
			dt1.Columns.Add("NoOfAttachments");
			if (dtReciptandPayment.Rows.Count > 0)
			{
				for (int i = 0; i < dtReciptandPayment.Rows.Count; i++)
				{
					if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString("Receipts"))
					{
						dt1.Rows.Add(dtReciptandPayment.Rows[i]["TranType"], dtReciptandPayment.Rows[i]["DocumentTypeCode"], dtReciptandPayment.Rows[i]["VoucherCode"], dtReciptandPayment.Rows[i]["Id"], dtReciptandPayment.Rows[i]["DocumentTypeId"], dtReciptandPayment.Rows[i]["DocumentTypeSrNo"], Conversion.ToDateTime(dtReciptandPayment.Rows[i]["voucherdate"]).ToString("dd-MMM-yyyy"), dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["OffsetAccountTitle"], dtReciptandPayment.Rows[i]["DebitAmount"], dtReciptandPayment.Rows[i]["NoOfAttachments"]);
					}
				}
			}
			return dt1;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<AccountTitlesModel> CashBalancesPayments1(ReportsParameters obj)
	{
		try
		{
			List<AccountTitlesModel> MainList = new List<AccountTitlesModel>();
			DataTable dtReciptandPayment = CashBalances(obj);
			if (dtReciptandPayment.Rows.Count > 0)
			{
				foreach (var item in from row in dtReciptandPayment.AsEnumerable()
					where Conversion.ToString(row["TranType"]) == "Payments"
					group row by row.Field<string>("AccountTitle") into grp
					select new
					{
						AccountTitle = grp.Key,
						TotalAmount = grp.Sum((DataRow r) => Convert.ToDouble(r["DebitAmount"])),
						Rows = grp
					})
				{
					AccountTitlesModel Main = new AccountTitlesModel();
					Main.AccountTitle = item.AccountTitle;
					List<VoucherNoModel> VoucherNoList = new List<VoucherNoModel>();
					foreach (var itemg in from row in dtReciptandPayment.Select("AccountTitle = '" + item.AccountTitle + "' and TranType='Payments'").CopyToDataTable().AsEnumerable()
						group row by new
						{
							VoucherType = row.Field<string>("DocumentTypeCode"),
							VoucherCode = row.Field<int>("VoucherCode")
						} into grp
						select new
						{
							grp.Key.VoucherType,
							grp.Key.VoucherCode
						})
					{
						VoucherNoModel VoucherNo = new VoucherNoModel();
						DataTable VoucherTypeResult = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "'  and TranType='Payments'").CopyToDataTable();
						for (int j = 0; j < 1; j++)
						{
							VoucherNo.VoucherType = VoucherTypeResult.Rows[j]["DocumentTypeCode"].ToString();
							VoucherNo.VoucherDate = Convert.ToDateTime(VoucherTypeResult.Rows[j]["voucherdate"].ToString());
						}
						VoucherNo.VoucherCode = itemg.VoucherCode;
						DataTable voucherDetail = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "' and TranType='Payments'").CopyToDataTable();
						List<VoucherDetailModel> VoucherDetailList = new List<VoucherDetailModel>();
						for (int i = 0; i < voucherDetail.Rows.Count; i++)
						{
							VoucherDetailModel VoucherDetail = new VoucherDetailModel();
							VoucherDetail.OffsetAccount = voucherDetail.Rows[i]["OffsetAccountTitle"].ToString();
							VoucherDetail.Amount = Convert.ToDouble(voucherDetail.Rows[i]["DebitAmount"].ToString());
							VoucherDetailList.Add(VoucherDetail);
						}
						VoucherNo.Data = VoucherDetailList;
						VoucherNoList.Add(VoucherNo);
					}
					Main.Data = VoucherNoList;
					MainList.Add(Main);
				}
			}
			return MainList;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<AccountTitlesModel> CashBalancesReceipts1(ReportsParameters obj)
	{
		try
		{
			List<AccountTitlesModel> MainList = new List<AccountTitlesModel>();
			DataTable dtReciptandPayment = CashBalances(obj);
			if (dtReciptandPayment.Rows.Count > 0)
			{
				foreach (var item in from row in dtReciptandPayment.AsEnumerable()
					where Conversion.ToString(row["TranType"]) == "Receipts"
					group row by row.Field<string>("AccountTitle") into grp
					select new
					{
						AccountTitle = grp.Key,
						TotalAmount = grp.Sum((DataRow r) => Convert.ToDouble(r["DebitAmount"])),
						Rows = grp
					})
				{
					AccountTitlesModel Main = new AccountTitlesModel();
					Main.AccountTitle = item.AccountTitle;
					List<VoucherNoModel> VoucherNoList = new List<VoucherNoModel>();
					foreach (var itemg in from row in dtReciptandPayment.Select("AccountTitle = '" + item.AccountTitle + "' and TranType='Receipts'").CopyToDataTable().AsEnumerable()
						group row by new
						{
							VoucherType = row.Field<string>("DocumentTypeCode"),
							VoucherCode = row.Field<int>("VoucherCode")
						} into grp
						select new
						{
							grp.Key.VoucherType,
							grp.Key.VoucherCode
						})
					{
						VoucherNoModel VoucherNo = new VoucherNoModel();
						DataTable VoucherTypeResult = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "'  and TranType='Receipts'").CopyToDataTable();
						for (int j = 0; j < 1; j++)
						{
							VoucherNo.VoucherType = VoucherTypeResult.Rows[j]["DocumentTypeCode"].ToString();
							VoucherNo.VoucherDate = Convert.ToDateTime(VoucherTypeResult.Rows[j]["voucherdate"].ToString());
						}
						VoucherNo.VoucherCode = itemg.VoucherCode;
						DataTable voucherDetail = dtReciptandPayment.Select("VoucherCode = '" + itemg.VoucherCode + "' and DocumentTypeCode='" + itemg.VoucherType + "' and TranType='Receipts'").CopyToDataTable();
						List<VoucherDetailModel> VoucherDetailList = new List<VoucherDetailModel>();
						for (int i = 0; i < voucherDetail.Rows.Count; i++)
						{
							VoucherDetailModel VoucherDetail = new VoucherDetailModel();
							VoucherDetail.OffsetAccount = voucherDetail.Rows[i]["OffsetAccountTitle"].ToString();
							VoucherDetail.Amount = Convert.ToDouble(voucherDetail.Rows[i]["DebitAmount"].ToString());
							VoucherDetailList.Add(VoucherDetail);
						}
						VoucherNo.Data = VoucherDetailList;
						VoucherNoList.Add(VoucherNo);
					}
					Main.Data = VoucherNoList;
					MainList.Add(Main);
				}
			}
			return MainList;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet CashandBankBalancesSummery(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectsId", obj.ProjectsId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataSetProc<ReportsParameters>("Sp_Accounts_CashBankBalancesSummery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DialyCashAndBankBalances_Rpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter("@IncludeAllJvId", obj.SkipZero));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_DialyCashAndBankBalances_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable CashAndBank_InflowOutFlow_Rpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter("@BranchesIds", obj.BranchesIds));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Accounts_CashAndBank_InflowOutFlow_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FcBankExportReportGetAll(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.ExImInvoiceNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankAccountId",
					Value = obj.ExImInvoiceNoId
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
			if (obj.ExImLcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.ExImLcOrderId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_ExImFCBankReceipts_Register_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TrialBalanceAllLevels(ReportsParameters obj)
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter("@UserId", obj.UserId));
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
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("Sp_Accounts_TrialBalancesAllLevels_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FCYPayablesAndReceivables_Rpt(ReportsParameters obj)
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
			if (obj?.CompanyIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyIds",
					Value = obj.CompanyIds
				});
			}
			if (obj.BranchesIds != null && obj?.BranchesIds != string.Empty)
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
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeId",
					Value = obj.AccountTypeId
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportTypeId",
					Value = obj.ReportTypeId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ResultTypId",
					Value = obj.ActionId
				});
			}
			if (obj.ActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransTypeId",
					Value = obj.ActivityId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ExchangeRateUSD",
				Value = obj.ExchangeRateUSD
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ExchangeRateEURO",
				Value = obj.ExchangeRateEURO
			});
			return GenericProvider.GetDataTableProc<PdcInventory>("SPU_Accounts_FCYPayablesAndReceivables_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LedgerByJobLotDropDownAndLists(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityType ",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("Sp_Vouchers_LedgerByJobLot_DropDownAndLists", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherLedgerByJobLot(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
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
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityDescription",
					Value = obj.ReportType
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<COAAllocation>("SpVouchers_LedgerByJobLot_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GeneralLedger2Format_Rpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter("@LanguageId", obj.LanguageId));
			}
			if (!obj.IsApproved && obj.ApprovedFilter == null)
			{
				obj.ApprovedFilter = "All";
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.SubsidiaryAccountId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryAccountId", obj.SubsidiaryAccountId));
			}
			if (obj.SubsidiaryTypeId != 0)
			{
				p.Add(new SqlParameter("@SubsidiaryTypeId", obj.SubsidiaryTypeId));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("SpAccounts_GeneralLedger2Format_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PaymentByInvoiceSummery_Detail(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter("@ProjectId", obj.ProjectsId));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@RefAccountId", obj.AccountId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_PaymentByInvoiceSummery_Register", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FCY_LedgerAgainstAccountId(ReportsParameters obj)
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
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountTypeId",
					Value = obj.AccountTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@AccountId",
				Value = obj.AccountId
			});
			return GenericProvider.GetDataTableProc<PdcInventory>("SPU_Accounts_FCYCustomerLedger_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DueByDatePayablesAndReceivables(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (!string.IsNullOrEmpty(obj.Ids))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.Ids
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueDateTo",
					Value = obj.DueDateTo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_DueByDatePayablesAndReceivables", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable USp_AccountsPayablesByDuedateBetweenPeriod(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectId",
					Value = obj.ProjectsId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueDateFrom",
					Value = obj.DueDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueDateTo",
					Value = obj.DueDateTo
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.Ids
				});
			}
			if (obj.AccountIds != null && obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountIds",
					Value = obj.AccountIds
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("USp_AccountsPayablesByDuedateBetweenPeriod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchsaeSaleVoucherReport(ReportsParameters obj)
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
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_PurchaseSalesSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherSlipPartyWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			if (obj.VoucherType != null && obj.VoucherType != string.Empty)
			{
				p.Add(new SqlParameter("@SlipFlag", obj.VoucherType));
			}
			if (obj.CashVoucher != null && obj.CashVoucher != string.Empty)
			{
				p.Add(new SqlParameter("@CashVouchers", obj.CashVoucher));
			}
			if (obj.BankVoucher != null && obj.BankVoucher != string.Empty)
			{
				p.Add(new SqlParameter("@BankVouchers", obj.BankVoucher));
			}
			if (obj.AccountIds != null && obj.AccountIds != string.Empty)
			{
				p.Add(new SqlParameter("@AccountIds", obj.AccountIds));
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("Usp_VouchersPaymentReceiptSlip_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InlandFreightAggreement(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.TransactionTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TranspoterId",
					Value = obj.TransactionTypeId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyNameId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.DistrictId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DisctrictId",
					Value = obj.DistrictId
				});
			}
			if (obj.LoadingLocationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LoadingLocationId",
					Value = obj.LoadingLocationId
				});
			}
			if (obj.DesignationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DestinationLocationId",
					Value = obj.DesignationId
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
			return GenericProvider.GetDataTableProc<InvInLandFreightAgreement>("SP_InLandFreightAgreementSlipAndRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet PostDatedChecqueReport(ReportsParameters obj)
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
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqDateTo",
					Value = obj.ToDate
				});
			}
			if (obj.BankGlDrAc != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountIdBank",
					Value = obj.BankGlDrAc
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqStatus",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataSetProc<InvInLandFreightAgreement>("USP_PostDatedChecque_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DueByDateReceivables(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueDateTo",
					Value = obj.DueDateTo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.Ids
				});
			}
			if (obj.ParentAccountCode != null && obj.ParentAccountCode != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountCode",
					Value = obj.ParentAccountCode
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
			return GenericProvider.GetDataTableProc<VoucherHead>("[dbo].[USP_DueByDateReceivables]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMultiCurrencyAndLastRate(ReportsParameters obj)
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
				Value = "GetMultiCurrencyAndLastRate"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[Sp_Vouchers_GetMethods]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet ReceivablesAging_New(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AsOnDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AgingDays",
				Value = obj.AgingDays
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccouuntClassId",
				Value = obj.AccouuntClassId
			});
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportId",
					Value = obj.ReportTypeId
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Interval",
					Value = obj.IntervalDays
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.TopCustomers != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TopCustomers",
					Value = obj.TopCustomers
				});
			}
			if (!string.IsNullOrEmpty(obj.AccountCode))
			{
				p.Add(new SqlParameter("@AccountCode", obj.AccountCode));
			}
			if (!string.IsNullOrEmpty(obj.AccountCodeOperator))
			{
				p.Add(new SqlParameter("@AccountCodeOperator", obj.AccountCodeOperator));
			}
			if (!string.IsNullOrEmpty(obj.AccountTitle))
			{
				p.Add(new SqlParameter("@AccountTitle", obj.AccountTitle));
			}
			if (!string.IsNullOrEmpty(obj.AccountTitleFT))
			{
				p.Add(new SqlParameter("@AcTitleFT", obj.AccountTitleFT));
			}
			if (!string.IsNullOrEmpty(obj.ParentAccount))
			{
				p.Add(new SqlParameter("@ParentAccountTitle", obj.ParentAccount));
			}
			if (!string.IsNullOrEmpty(obj.ParentAccountFT))
			{
				p.Add(new SqlParameter("@ParentAcTitleFT", obj.ParentAccountFT));
			}
			if (!string.IsNullOrEmpty(obj.AccountType))
			{
				p.Add(new SqlParameter("@AccountType", obj.AccountType));
			}
			if (!string.IsNullOrEmpty(obj.AccountTypeFT))
			{
				p.Add(new SqlParameter("@AcTypeFT", obj.AccountTypeFT));
			}
			if (obj.Opening != 0.0)
			{
				p.Add(new SqlParameter("@Opening", obj.Opening));
			}
			if (!string.IsNullOrEmpty(obj.OpeningOperator))
			{
				p.Add(new SqlParameter("@OpeningOperator", obj.OpeningOperator));
			}
			if (obj.IstInterval != 0.0)
			{
				p.Add(new SqlParameter("@IstInterval", obj.IstInterval));
			}
			if (!string.IsNullOrEmpty(obj.IstIntervalOperator))
			{
				p.Add(new SqlParameter("@IstIntervalOperator", obj.IstIntervalOperator));
			}
			if (obj.ScnInterval != 0.0)
			{
				p.Add(new SqlParameter("@ScnInterval", obj.ScnInterval));
			}
			if (!string.IsNullOrEmpty(obj.ScnIntervalOperator))
			{
				p.Add(new SqlParameter("@ScnIntervalOperator", obj.ScnIntervalOperator));
			}
			if (obj.ThirdInterval != 0.0)
			{
				p.Add(new SqlParameter("@TrdInterval", obj.ThirdInterval));
			}
			if (!string.IsNullOrEmpty(obj.ThirdIntervalOperator))
			{
				p.Add(new SqlParameter("@TrdIntervalOperator", obj.ThirdIntervalOperator));
			}
			if (obj.Above != 0.0)
			{
				p.Add(new SqlParameter("@Above", obj.Above));
			}
			if (!string.IsNullOrEmpty(obj.AboveOperator))
			{
				p.Add(new SqlParameter("@AboveOperator", obj.AboveOperator));
			}
			if (obj.Closing != 0.0)
			{
				p.Add(new SqlParameter("@Closing", obj.Closing));
			}
			if (!string.IsNullOrEmpty(obj.ClosingOperator))
			{
				p.Add(new SqlParameter("@ClosingOperator", obj.ClosingOperator));
			}
			if (obj.CurrentBalance != 0.0)
			{
				p.Add(new SqlParameter("@CurrentBalance", obj.CurrentBalance));
			}
			if (!string.IsNullOrEmpty(obj.CurrentBalanceOperator))
			{
				p.Add(new SqlParameter("@CurrentBalanceOperator", obj.CurrentBalanceOperator));
			}
			if (obj.OverDue != 0.0)
			{
				p.Add(new SqlParameter("@OverDue", obj.OverDue));
			}
			if (!string.IsNullOrEmpty(obj.OverDueOperator))
			{
				p.Add(new SqlParameter("@OverDueOperator", obj.OverDueOperator));
			}
			if (obj.NotYetDue != 0.0)
			{
				p.Add(new SqlParameter("@NotYetDue", obj.NotYetDue));
			}
			if (!string.IsNullOrEmpty(obj.NotYetDueOperator))
			{
				p.Add(new SqlParameter("@NotYetDueOperator", obj.NotYetDueOperator));
			}
			if (!string.IsNullOrEmpty(obj.SortType))
			{
				p.Add(new SqlParameter("@SortType", obj.SortType));
			}
			if (!string.IsNullOrEmpty(obj.SortingCol))
			{
				p.Add(new SqlParameter("@SortingCol", obj.SortingCol));
			}
			return GenericProvider.GetDataSetProc<VoucherDetail>("UPS_ReceivablesAging_New", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet ReceivablesNotYetDueAging_New(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AsOnDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AgingDays",
				Value = obj.AgingDays
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccouuntClassId",
				Value = obj.AccouuntClassId
			});
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Interval",
					Value = obj.IntervalDays
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportId",
					Value = obj.ReportTypeId
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.TopCustomers != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TopCustomers",
					Value = obj.TopCustomers
				});
			}
			return GenericProvider.GetDataSetProc<VoucherDetail>("UPS_ReceivablesNotYetDueAging_New", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet PayablesAging_New(ReportsParameters obj)
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
				ParameterName = "@AsOnDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AgingDays",
				Value = obj.AgingDays
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccouuntClassId",
				Value = obj.AccouuntClassId
			});
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
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
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ControlAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Interval",
					Value = obj.IntervalDays
				});
			}
			if (obj.TopCustomers != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TopSuppliers",
					Value = obj.TopCustomers
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportId",
					Value = obj.ReportTypeId
				});
			}
			if (!string.IsNullOrEmpty(obj.AccountCode))
			{
				p.Add(new SqlParameter("@AccountCode", obj.AccountCode));
			}
			if (!string.IsNullOrEmpty(obj.AccountCodeOperator))
			{
				p.Add(new SqlParameter("@AccountCodeOperator", obj.AccountCodeOperator));
			}
			if (!string.IsNullOrEmpty(obj.AccountTitle))
			{
				p.Add(new SqlParameter("@AccountTitle", obj.AccountTitle));
			}
			if (!string.IsNullOrEmpty(obj.AccountTitleFT))
			{
				p.Add(new SqlParameter("@AcTitleFT", obj.AccountTitleFT));
			}
			if (!string.IsNullOrEmpty(obj.ParentAccount))
			{
				p.Add(new SqlParameter("@ParentAccountTitle", obj.ParentAccount));
			}
			if (!string.IsNullOrEmpty(obj.ParentAccountFT))
			{
				p.Add(new SqlParameter("@ParentAcTitleFT", obj.ParentAccountFT));
			}
			if (!string.IsNullOrEmpty(obj.AccountType))
			{
				p.Add(new SqlParameter("@AccountType", obj.AccountType));
			}
			if (!string.IsNullOrEmpty(obj.AccountTypeFT))
			{
				p.Add(new SqlParameter("@AcTypeFT", obj.AccountTypeFT));
			}
			if (obj.Opening != 0.0)
			{
				p.Add(new SqlParameter("@Opening", obj.Opening));
			}
			if (!string.IsNullOrEmpty(obj.OpeningOperator))
			{
				p.Add(new SqlParameter("@OpeningOperator", obj.OpeningOperator));
			}
			if (obj.IstInterval != 0.0)
			{
				p.Add(new SqlParameter("@IstInterval", obj.IstInterval));
			}
			if (!string.IsNullOrEmpty(obj.IstIntervalOperator))
			{
				p.Add(new SqlParameter("@IstIntervalOperator", obj.IstIntervalOperator));
			}
			if (obj.ScnInterval != 0.0)
			{
				p.Add(new SqlParameter("@ScnInterval", obj.ScnInterval));
			}
			if (!string.IsNullOrEmpty(obj.ScnIntervalOperator))
			{
				p.Add(new SqlParameter("@ScnIntervalOperator", obj.ScnIntervalOperator));
			}
			if (obj.ThirdInterval != 0.0)
			{
				p.Add(new SqlParameter("@TrdInterval", obj.ThirdInterval));
			}
			if (!string.IsNullOrEmpty(obj.ThirdIntervalOperator))
			{
				p.Add(new SqlParameter("@TrdIntervalOperator", obj.ThirdIntervalOperator));
			}
			if (obj.Above != 0.0)
			{
				p.Add(new SqlParameter("@Above", obj.Above));
			}
			if (!string.IsNullOrEmpty(obj.AboveOperator))
			{
				p.Add(new SqlParameter("@AboveOperator", obj.AboveOperator));
			}
			if (obj.Closing != 0.0)
			{
				p.Add(new SqlParameter("@Closing", obj.Closing));
			}
			if (!string.IsNullOrEmpty(obj.ClosingOperator))
			{
				p.Add(new SqlParameter("@ClosingOperator", obj.ClosingOperator));
			}
			if (obj.CurrentBalance != 0.0)
			{
				p.Add(new SqlParameter("@CurrentBalance", obj.CurrentBalance));
			}
			if (!string.IsNullOrEmpty(obj.CurrentBalanceOperator))
			{
				p.Add(new SqlParameter("@CurrentBalanceOperator", obj.CurrentBalanceOperator));
			}
			if (obj.OverDue != 0.0)
			{
				p.Add(new SqlParameter("@OverDue", obj.OverDue));
			}
			if (!string.IsNullOrEmpty(obj.OverDueOperator))
			{
				p.Add(new SqlParameter("@OverDueOperator", obj.OverDueOperator));
			}
			if (obj.NotYetDue != 0.0)
			{
				p.Add(new SqlParameter("@NotYetDue", obj.NotYetDue));
			}
			if (!string.IsNullOrEmpty(obj.NotYetDueOperator))
			{
				p.Add(new SqlParameter("@NotYetDueOperator", obj.NotYetDueOperator));
			}
			if (!string.IsNullOrEmpty(obj.SortType))
			{
				p.Add(new SqlParameter("@SortType", obj.SortType));
			}
			if (!string.IsNullOrEmpty(obj.SortingCol))
			{
				p.Add(new SqlParameter("@SortingCol", obj.SortingCol));
			}
			return GenericProvider.GetDataSetProc<VoucherDetail>("UPS_PayablesAging_New", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet PayablesNotYetDueAging_New(ReportsParameters obj)
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
				ParameterName = "@AsOnDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AgingDays",
				Value = obj.AgingDays
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccouuntClassId",
				Value = obj.AccouuntClassId
			});
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
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
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Interval",
					Value = obj.IntervalDays
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportId",
					Value = obj.ReportTypeId
				});
			}
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ControlAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.TopSuppliers != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TopSuppliers",
					Value = obj.TopSuppliers
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			return GenericProvider.GetDataSetProc<VoucherDetail>("UPS_PayablesNotYetDueAging_New", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable HararicalTrialBalanceApi(int OrganizationId, int CompanyId, int AccountLevel, DateTime? FromDate, DateTime? ToDate, string Activity = "")
	{
		DataTable dtMain = null;
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(FromDate)))
			{
				p.Add(new SqlParameter("@FromDate", FromDate));
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(ToDate)))
			{
				p.Add(new SqlParameter("@ToDate", ToDate));
			}
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter("@AccountClass", Activity));
			}
			dtMain = GenericProvider.GetDataTableProc<ReportsParameters>("SpCoahierarchy_TrialBalance_Rpt", p);
			DataTable dtFinal = new DataTable();
			dtFinal.Columns.Add("RowId", typeof(int));
			dtFinal.Columns.Add("ParentId", typeof(int));
			dtFinal.Columns.Add("AccountId", typeof(int));
			dtFinal.Columns.Add("AccountTitle", typeof(string));
			dtFinal.Columns.Add("AccountCode", typeof(string));
			dtFinal.Columns.Add("AcLevel", typeof(int));
			dtFinal.Columns.Add("AccountClass", typeof(int));
			dtFinal.Columns.Add("IsGroupDetail", typeof(string));
			dtFinal.Columns.Add("Opening", typeof(double));
			dtFinal.Columns.Add("OpeningDr", typeof(double));
			dtFinal.Columns.Add("OpeningCr", typeof(double));
			dtFinal.Columns.Add("Debit", typeof(double));
			dtFinal.Columns.Add("Credit", typeof(double));
			dtFinal.Columns.Add("Closing", typeof(double));
			dtFinal.Columns.Add("ClosingDr", typeof(double));
			dtFinal.Columns.Add("ClosingCr", typeof(double));
			dtFinal.Columns.Add("CurDbGt", typeof(double));
			dtFinal.Columns.Add("CurCrGt", typeof(double));
			dtFinal.Columns.Add("ObGt", typeof(double));
			dtFinal.Columns.Add("ClGt", typeof(double));
			dtFinal.Columns.Add("ObDrGt", typeof(double));
			dtFinal.Columns.Add("ObCrGt", typeof(double));
			dtFinal.Columns.Add("ClDrGt", typeof(double));
			dtFinal.Columns.Add("ClCrGt", typeof(double));
			int RowId = 0;
			object ParentId2ndLevel = 0;
			object ParentId3rdLevel = 0;
			object ParentId4thLevel = 0;
			object ParentId5thLevel = 0;
			for (int i = 0; i < dtMain.Rows.Count; i++)
			{
				RowId++;
				switch (AccountLevel)
				{
				case 1:
					if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
					{
						ParentId2ndLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					break;
				case 2:
					if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
					{
						ParentId2ndLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
					{
						ParentId3rdLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId2ndLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					break;
				case 3:
					if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
					{
						ParentId3rdLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
					{
						ParentId4thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId3rdLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					break;
				case 4:
					if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
					{
						ParentId4thLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 4)
					{
						ParentId5thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId4thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 5)
					{
						dtFinal.Rows.Add(RowId, ParentId5thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					break;
				case 5:
					if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 1)
					{
						ParentId2ndLevel = RowId;
						dtFinal.Rows.Add(RowId, DBNull.Value, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 2)
					{
						ParentId3rdLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId2ndLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 3)
					{
						ParentId4thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId3rdLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 4)
					{
						ParentId5thLevel = RowId;
						dtFinal.Rows.Add(RowId, ParentId4thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					else if (Conversion.ToInt(dtMain.Rows[i]["AcLevel"]) == 5)
					{
						dtFinal.Rows.Add(RowId, ParentId5thLevel, dtMain.Rows[i]["AccountId"], dtMain.Rows[i]["AccountTitle"], dtMain.Rows[i]["AccountCode"], dtMain.Rows[i]["AcLevel"], dtMain.Rows[i]["AccountClass"], dtMain.Rows[i]["IsGroupDetail"], dtMain.Rows[i]["Opening"], dtMain.Rows[i]["OpeningDr"], dtMain.Rows[i]["OpeningCr"], dtMain.Rows[i]["Debit"], dtMain.Rows[i]["Credit"], dtMain.Rows[i]["Closing"], dtMain.Rows[i]["ClosingDr"], dtMain.Rows[i]["ClosingCr"], dtMain.Rows[i]["CurDbGt"], dtMain.Rows[i]["CurCrGt"], dtMain.Rows[i]["ObGt"], dtMain.Rows[i]["ClGt"], dtMain.Rows[i]["ObDrGt"], dtMain.Rows[i]["ObCrGt"], dtMain.Rows[i]["ClDrGt"], dtMain.Rows[i]["ClCrGt"]);
					}
					break;
				}
			}
			return dtFinal;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReceivablesByDueDates(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccouuntClassId ",
				Value = obj.AccouuntClassId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
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
			return GenericProvider.GetDataTableProc<VoucherDetail>("[dbo].[Usp_ReceivablesByDueDates]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ChequeBookRegister(ReportsParameters obj)
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
			if (obj.PaidToAccount != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaidToAccount",
					Value = obj.PaidToAccount
				});
			}
			if (obj.CheqId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqBookId",
					Value = obj.CheqId
				});
			}
			if (obj.cheqStatus != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqStatus",
					Value = obj.cheqStatus
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankAccountId",
					Value = obj.AccountId
				});
			}
			return GenericProvider.GetDataTableProc<InvInLandFreightAgreement>("usp_ChequeBook_Register", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ChequeBookRegisterDropDown(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityType",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("usp_GetDataForDropDownFromCheqBook", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportReturnInvoice(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_ExportReturnInvoiceVoucherSlipNew_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerAgingDocumentWise(ReportsParameters obj)
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
				ParameterName = "@AsOnDate",
				Value = obj.AsOnDate
			});
			if (obj.AgingDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AgingDays",
					Value = obj.AgingDays
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyGroupId",
					Value = obj.CustomerGroupId
				});
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGruopId",
					Value = obj.CustomGroupId
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			return GenericProvider.GetDataTableProc<VoucherDetail>("usp_CustomerAgingReport_DocumentWise", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierAgingDocumentWise(ReportsParameters obj)
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
				ParameterName = "@AsOnDate",
				Value = obj.AsOnDate
			});
			if (obj.AgingDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AgingDays",
					Value = obj.AgingDays
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyGroupId",
					Value = obj.CustomerGroupId
				});
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGruopId",
					Value = obj.CustomGroupId
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			return GenericProvider.GetDataTableProc<VoucherDetail>("usp_SupplierAgingReport_DocumentWise", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet PayablesReportInvoiceWise(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.ParentAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountId",
					Value = obj.ParentAccountId
				});
			}
			if (obj.CustomGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGruopId",
					Value = obj.CustomGroupId
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
			return GenericProvider.GetDataSetProc<VoucherHead>("usp_PayablesReportInvoiceWise", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Accounts_getReceivablesAndPayables_Export(ReportsParameters obj)
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
			if (obj.CurrencyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceCurrencyId",
					Value = obj.CurrencyId
				});
			}
			return GenericProvider.GetDataTableProc<FcyBankReceiptBreakUp>("uspAccounts_getReceivablesAndPayables_Export", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetMonthWiseTransactions(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassId",
					Value = obj.AccouuntClassId
				});
			}
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			return GenericProvider.GetDataSetProc<FcyBankReceiptBreakUp>("usp_getMonthwiseTransaction", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetMonthWisePaymentReceiptTransactions(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			if (obj.ControlAccountIds != null && obj.ControlAccountIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentAccountIds",
					Value = obj.ControlAccountIds
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@ActionId", obj.ActionId));
			}
			return GenericProvider.GetDataSetProc<FcyBankReceiptBreakUp>("usp_getMonthwisePaymentAndReceipts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetMonthWiseProfitLoss(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataSetProc<FcyBankReceiptBreakUp>("usp_getMonthwiseProfitLoss", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getPostDatedChequePaymentAndReceipts(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<InvInLandFreightAgreement>("usp_getPostDatedChequePaymentAndReceipts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Accounts_COGS_BreakUp(ReportsParameters obj)
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
			if (obj.ParentAccountId != 0)
			{
				p.Add(new SqlParameter("@ParentAccountId", obj.ParentAccountId));
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter("@ActionId", obj.ActionId));
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("usp_Accounts_COGS_BreakUp", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
