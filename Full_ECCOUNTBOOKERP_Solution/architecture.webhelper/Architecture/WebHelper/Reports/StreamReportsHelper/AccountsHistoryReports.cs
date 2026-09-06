using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Model;
using Architecture.WebHelper.Entity;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports.StreamReportsHelper;

public class AccountsHistoryReports
{
	public static Stream AcRptActivitySummery_110(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ActivitySummaryReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptActivitySummery_110A(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ActivitySummaryReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("ClosingDr") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("ClosingCr") > 0.0
				select r;
			DataTable dtSubReport = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportDebitObject = new SubReportObject(dtDebit, "ActivitySummeryDebitSubReport.rpt");
			SubReportObject subReportCreditObject = new SubReportObject(dtSubReport, "ActivitySummeryCreditSubReport.rpt");
			lstSubReports.Add(subReportDebitObject);
			lstSubReports.Add(subReportCreditObject);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, lstSubReports, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptActivitySummery_110B(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ActivitySummaryDocumentTypeWise(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("Debit") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("Credit") > 0.0
				select r;
			DataTable dtSubReport = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportDebitObject = new SubReportObject(dtDebit, "ActivitySummeryDebitSubReportForB.rpt");
			SubReportObject subReportCreditObject = new SubReportObject(dtSubReport, "ActivitySummeryCreditSubReportForB.rpt");
			lstSubReports.Add(subReportDebitObject);
			lstSubReports.Add(subReportCreditObject);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, lstSubReports, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptAccountsActivityDetail_116(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ActivitySummaryDetail(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptGeneralLedger_105(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedgerWithOffsetAccount(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptGeneralLedgerB_106(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedgerWithOffsetAccount(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptQuantativeLedger_107(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedgerWithOffsetAccount(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptGeneralLedgerSummery_108(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedgerSummery(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptGeneralLedgerSummar2_105A(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedger2Format_Rpt(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream DueByDatePayablesAndReceivables_142(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.DueByDatePayablesAndReceivables(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream USp_AccountsPayablesByDuedateBetweenPeriod_159(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.USp_AccountsPayablesByDuedateBetweenPeriod(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream TradeDebtorsReport_122And122A(ReportsParameters obj)
	{
		try
		{
			DataSet ds = VoucherReports.TradeDebtorsAndCreditors_Report(obj);
			DataTable dt = ds.Tables[0].Copy();
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			if (dt.Columns.Contains("CompLogoImage"))
			{
				dt.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
			}
			dt.Columns.Add("CompLogoImage", typeof(byte[]));
			foreach (DataRow row in dt.Rows)
			{
				row["CompLogoImage"] = ((ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0][0] : "");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream PayablesWithAmountAndDate_141(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.PayablesWithLastBillAndPaidAmount(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptAccounts_ReceivablesWithStatus_122(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ReceivablesReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptTrialBalances_111A(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.TrialBalanceReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("ClosingDr") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("ClosingCr") > 0.0
				select r;
			DataTable dtSubReport = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportDebitObject = new SubReportObject(dtDebit, "TrialBalanceDebitSubReport.rpt");
			SubReportObject subReportCreditObject = new SubReportObject(dtSubReport, "TrialBalanceCreditSubReport.rpt");
			lstSubReports.Add(subReportDebitObject);
			lstSubReports.Add(subReportCreditObject);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, lstSubReports, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream SelectedTrialBalance_113A(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.SelectedTrialBalanceOld(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptVoucherSlip_118(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherValidationReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream AcRptVoucherSlip_118A(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherValidationReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			EnumerableRowCollection<DataRow> dt2 = from r in dt.AsEnumerable()
				where r.Field<double>("DebitAmount") > 0.0
				select r;
			DataTable dtDebit = (dt2.Any() ? dt2.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> dt3 = from r in dt.AsEnumerable()
				where r.Field<double>("CreditAmount") > 0.0
				select r;
			DataTable dtSubReport = (dt3.Any() ? dt3.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DebitTransactions = new SubReportObject(dtDebit, "VoucherReportDebitSubReport.rpt");
			lstSubReports.Add(DebitTransactions);
			SubReportObject CreditTransactions = new SubReportObject(dtSubReport, "VoucherReportCreditSubReport.rpt");
			lstSubReports.Add(CreditTransactions);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, lstSubReports, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream InventoryPayablesandReceivables_134(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GeneralReprots.InventoryPayablesandReceivables(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream FCYPayablesAndReceivablesRpt_153(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.FCYPayablesAndReceivables_Rpt(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream DueByDateReceivables_380(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.DueByDateReceivables(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream GetSubsidiaryPayablesReceivables_375(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherHead.GetSubsidiaryPayablesReceivables(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream BankBalances_590(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.BankBalances(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream CashBalances_592(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.CashBalances(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream CashAndBankSummary_593(ReportsParameters obj)
	{
		try
		{
			DataSet ds = VoucherReports.CashandBankBalancesSummery(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			ReportHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream CashAndBank_InflowOutFlow_Rpt_594(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.CashAndBank_InflowOutFlow_Rpt(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream DialyCashAndBankBalances_Rpt_595(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.DialyCashAndBankBalances_Rpt(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream PartiesPaymentAndReceipts_596(ReportsParameters obj)
	{
		try
		{
			DataSet ds = VoucherReports.PartiesPaymentAndReceipts(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			ReportHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				return ReportHelper.GetStreamForFile(dt, rptPath);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream PaymentAndReceipts_CustomersAndSupplirWise_598(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.PaymentAndReceipts_CustomersAndSupplirWise(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				return ReportHelper.GetStreamForFile(dt, rptPath);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream BankSummaryLedger_105_01_02(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.BankSummaryLedgersWithAgainstsAccounts(obj);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream TrialBalanceAllLevelReports(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.HararicalTrialBalanceApi(obj.OrganizationId, obj.CompanyId, obj.Account_Level, obj.FromDate, obj.ToDate, obj.Activity);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream ReceivablesByDueDate_121_01(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.ReceivablesByDueDates(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream ReceivablesAgingNew_123(ReportsParameters obj)
	{
		try
		{
			DataSet ds = new DataSet();
			ds = VoucherReports.ReceivablesAging_New(obj);
			if (ds.Tables.Count == 0 || ds == null)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dt = ds.Tables[0].Copy();
			ReportHelper.ReplaceCompLogoColumn(dt, (ds.Tables[2].Rows.Count > 0) ? ds.Tables[2] : null);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream PayablesAgingNew_124(ReportsParameters obj)
	{
		try
		{
			DataSet ds = VoucherReports.PayablesAging_New(obj);
			if (ds.Tables.Count == 0 || ds == null)
			{
				throw new Exception("Record Not Found");
			}
			ReportHelper.ReplaceCompLogoColumn(ds.Tables[0], (ds.Tables[2].Rows.Count > 0) ? ds.Tables[2] : null);
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
				}
				return ReportHelper.GetStreamForFile(ds.Tables[0], rptPath, null, null, rptdt);
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
