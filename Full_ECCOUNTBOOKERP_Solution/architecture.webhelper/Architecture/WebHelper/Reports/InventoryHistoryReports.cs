using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Reports.General;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WebHelper.Entity;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports;

public class InventoryHistoryReports
{
	public static Stream InvRptPurchaseOrderRiceSlip_203(ReportsParameters obj)
	{
		try
		{
			DataTable dt = PurchaseOrderReports.PurchaseOrderSlipReport203(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrderReports.PurchaseOrderSlipReport203SubReport(obj.OrderId), "PurchaseOrderSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(PurchaseOrderReports.PurchaseOrderSupplierExpense_SubReport(obj.OrderId), "PurchaseOrderSupplierExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject2);
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

	public static Stream RptPurchaseOrderRegisterRice_206(ReportsParameters obj)
	{
		try
		{
			DataTable dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
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

	public static Stream InvRptPurchaseOrderDetailRegisterRice_205(ReportsParameters obj)
	{
		try
		{
			DataTable dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
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

	public static Stream InwardGatePassWithWbAndLabSlip_257(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.LabAnalysisPurchaseByGP_SubReport(obj), "LabAnalysisPurchaseByGPSubReport.rpt");
			SubReportObject subReportObject2 = new SubReportObject(WbTransactions.WbTransationByGPID_SubReport(obj.Id), "WbTransationByGPIDSubReport.rpt");
			lstSubReports.Add(subReportObject);
			lstSubReports.Add(subReportObject2);
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

	public static Stream InvRptGatePassInwardRegisterA_253(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(obj);
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

	public static Stream InvRptGoodsReceiptsNotesRiceSlip_211(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvGrnandGdnReports.InvGrnSlip211(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dtSubReport = InvGrnandGdnReports.GrnDetailEmptyBagsSubReport(obj.Id);
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dtSubReport, "GrnDetailEmptyBagsSubReport.rpt");
			lstSubReports.Add(subReportObject);
			ReportsParameters objwages = new ReportsParameters();
			objwages.OrganizationId = obj.OrganizationId;
			objwages.CompanyId = obj.CompanyId;
			objwages.DocumentTypeId = 101;
			objwages.RefRefDocIdNo = obj.Id;
			if (obj.DocumentTypeId == 0)
			{
				objwages.RefDocumentTypeId = 46;
			}
			else
			{
				objwages.RefDocumentTypeId = obj.DocumentTypeId;
			}
			SubReportObject subReportObject2 = new SubReportObject(InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(objwages), "InvContractorWagesSubReport.rpt");
			lstSubReports.Add(subReportObject2);
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

	public static Stream InvRptPurchaseInvoicePurchaseAvgRateByItem_221(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateByItem221(obj);
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

	public static Stream InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier_222(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateBySupplier222(obj);
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

	public static Stream InvRptPurchaseRegisterDetail_223(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDetailRegister(obj);
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

	public static Stream InvRptPurchaseBillRegister_224(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDetailRegister(obj);
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

	public static Stream InvRptSaleOderRegister_270(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.SaleOrder.SaleOrderHistory(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
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

	public static Stream InvRptSalesOrderRegister_271(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.SaleOrder.SaleOrderHistory(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
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

	public static Stream InvRptSaleOrderSlipWithDespatchDetail_272(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.SaleOrder.SaleOrderHistory(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			ReportHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			DataTable dtSubReport = RptSaleOrderDispatchSubRep272SupReprt(obj.Id);
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dtSubReport, "RptSaleOrderSlipDispatchSubRep");
			lstSubReports.Add(subReportObject);
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

	public static DataTable RptSaleOrderDispatchSubRep272SupReprt(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvGdn_DispatchBySalesOrderId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream InvRptGatePassOutwardRegister_291(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(obj);
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

	public static Stream InvRptGdnRiceSlip_260(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(obj);
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

	public static Stream InvRptGdnRegister_261(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvGrnandGdnReports.InvGdnRegister261(obj);
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

	public static Stream InvRptWeighBridgeRegister_281(ReportsParameters obj)
	{
		try
		{
			DataTable dt = WbTransactionsReports.WbTransactionHistory(obj);
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

	public static Stream RptInvPurchaseInvoiceRegister_227(ReportsParameters obj)
	{
		try
		{
			DataTable dt = new DataTable();
			dt = PurchaseTrading.GetInvoiceRegister(obj);
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

	public static Stream RptInvSaleInvoiceRegister_226(ReportsParameters obj)
	{
		try
		{
			DataTable dt = new DataTable();
			dt = SalesTrading.GetInvoiceRegister(obj);
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

	public static Stream RptInvSaleInvoiceRegister_296(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceRegister(obj);
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

	public static Stream InvRptItemsList_200(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.Item.RptItemList(obj);
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

	public static Stream StockEvalautionDetail_GenerateStocks_Register_401(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InventoryStockEvalautionDetail.InventoryStockGenrate(obj);
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

	public static Stream InvStockRptInventoryTransactionsA_402(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InventoryStockEvalautionDetail.InventoryTransactions_GenerateTransactionsLedgerStocks(obj);
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

	public static Stream InvStockRptInventoryTransactionsStocks_403(ReportsParameters obj)
	{
		try
		{
			DataTable dt = StocksReport.stockGeneralSummaryByWeight(obj);
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

	public static Stream InvSaleInvoice_302(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectRegister(obj);
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

	public static Stream SalesRegisterSummary_312(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InventoryStockEvalautionDetail.EvaulationDetailSalesReports(obj);
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

	public static Stream RptItemLedgerTradeProRegister(ReportsParameters obj)
	{
		try
		{
			DataTable dt = StocksReport.itemLedgerTradePro(obj);
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

	public static Stream DeliveryOrderRegister263(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.DeliverySchedule.GetDeliveryOrderForApproval(obj);
			DataTable dt = ds?.Tables[0];
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
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

	public static Stream InvWeighBridgeSlip_280(ReportsParameters obj)
	{
		try
		{
			DataTable dt = WbTransactionsReports.WbTransactionSlip280(obj);
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

	public static Stream InvOutstandingPurchaseOrderRptByAvgRate_207(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.PurchaseOrder.ReportsAgainstOutstandingPurchaseOrders(obj);
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

	public static Stream RiceAndPaddyPurchaseReportPdf_231(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.RicePaddyPuchaseReport(obj);
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

	public static string PurchaseInvoiceSlip_PM231(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoice_PM231(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, lstSubReports, null, rptdt);
				string ServerFilePathUrl = "";
				string lst = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ServerFileURL");
				if (lst != string.Empty)
				{
					ServerFilePathUrl = lst.ToString() + "\\";
					string ShareLocalDrivePath = "";
					string lstShareDrive = ReportHelper.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ShareLocalDrivePath");
					if (lstShareDrive != string.Empty)
					{
						ShareLocalDrivePath = lstShareDrive.ToString() + "\\";
						string fileName = obj.ReportName + ".pdf";
						string UploadPath = ShareLocalDrivePath + "\\" + fileName;
						ReportHelper.SavePdf(streamForFile, UploadPath);
						return ServerFilePathUrl + "\\" + fileName;
					}
					throw new Exception("Share Local Drive Path not Found");
				}
				throw new Exception("ServerFileURL not Found");
			}
			throw new Exception("Report Folder Path not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Stream SupplierCustomerRegister(ReportsParameters obj)
	{
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(obj);
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

	public static Stream InvGrn_Register_334_335(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.InvGrn.InvGrn_History(obj);
			DataTable dt = ds?.Tables[0].Copy();
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
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

	public static Stream PurchasesingRegister_234(ReportsParameters obj)
	{
		try
		{
			DataSet ds = Architecture.BLL.Inventory.InventoryStockEvalautionDetail.PurchasesingRegister(obj);
			DataTable dt = ds.Tables?[0].Copy();
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
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

	public static Stream PurchaseRegisterWithActivities_Reports(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations(obj);
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

	public static Stream StockAdjustmentSlipAndRegister409And410(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InvStockAdjustment.StockAdjustmentSlipAndRegister409(obj);
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

	public static Stream InvPurchaseDemondSlip_454(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.InvPurchaseDemandHeader.InvPurchaseDemondSlip(obj);
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

	public static Stream GrnSlipStore212(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvGrnandGdnReports.GrnSlipStore(obj);
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

	public static Stream OrderHistoryRegister_271_01(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Inventory.SaleOrder.SaleOrderHistoryForCustomer(obj);
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
