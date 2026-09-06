using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Architecture.BLL;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WebHelper.Entity;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports;

public class AccountsReports
{
	public static Stream GeneralLedgerPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.GeneralLedger(obj);
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

	public static Stream PaymentVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				LogHelper.Reportlogwriteondrive("Record Not Found" + dt.Rows.Count);
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

	public static Stream ReceiptVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream ExpenseVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream ContraVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream BillsPayableVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream BillsReceivablesVoucherSlipPdfReprot(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream ExportInvoiceVoucherSlip102(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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
				if (obj.CompanyAddress != null && obj.CompanyAddress != string.Empty && obj.CompanyName != null && obj.CompanyName != string.Empty && obj.UserName != null && obj.UserName != string.Empty)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", obj.CompanyAddress);
					rptdt.Rows.Add("@CompanyName", obj.CompanyName);
					rptdt.Rows.Add("@PrintedBy", obj.UserName);
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

	public static Stream AccountsVoucherSlip102(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.NewVoucherReprot(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			string rptPath = "";
			string list = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Report Folder Path");
			if (list != string.Empty)
			{
				rptPath = list.ToString() + "\\";
				obj.ReportName = "102-ANewAcRptPaymentReceiptsVoucherSlip";
				rptPath = rptPath + obj.ReportName + ".rpt";
				DataTable rptdt = new DataTable();
				DataTable dtcomp = CommonServices.GetCompanyData(obj.OrganizationId, obj.CompanyId);
				if (dtcomp.Rows.Count > 0)
				{
					rptdt.Columns.Add("Perameter");
					rptdt.Columns.Add("value");
					rptdt.Rows.Add("@CompanyAddress", Conversion.ToString(dtcomp.Rows[0]["CompAddress"]));
					rptdt.Rows.Add("@CompanyName", Conversion.ToString(dtcomp.Rows[0]["CompName"]));
					if (obj.UserId != 0)
					{
						Architecture.Model.UserAccount UsrObj = Architecture.BLL.UserAccount.GetByID(obj.UserId);
						rptdt.Rows.Add("@PrintedBy", UsrObj.UserName);
					}
					else
					{
						rptdt.Rows.Add("@PrintedBy", obj.UserName);
					}
					return ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
				}
				throw new Exception("Company Record Not Found");
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream PaymentByInvoiceSlip132(ReportsParameters obj)
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
			DataTable dt = GenericProvider.GetDataTableProc<VoucherHead>("SpVouchers_Payment&ReceipteByInvoiceVoucherSlipNew_Rpt", p);
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

	public static Stream FcyGeneralLedger_156(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.FCY_LedgerAgainstAccountId(obj);
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

	public static Stream DayBookSlip(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GeneralReprots.DayBookSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dt, "DayBookDebitSubReport.rpt");
			SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(dt, "DayBookCreditSubReport.rpt");
			lstSubReports.Add(DayBookSubReport_DebitTransactions);
			lstSubReports.Add(DayBookSubReport_CreditTransactions);
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

	public static Stream AcRptVoucherSlip_102(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.VoucherReport(obj);
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

	public static Stream FcyGeneralLedger_105C(ReportsParameters obj)
	{
		try
		{
			DataTable dt = VoucherReports.Accounts_FcyGeneralLedger_Rpt(obj);
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
}
