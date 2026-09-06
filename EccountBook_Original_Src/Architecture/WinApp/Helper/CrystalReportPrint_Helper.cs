using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.CommissionAgent;
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.Reports.Party_Processing;
using Architecture.BLL.StoreManagement;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Reports;

namespace Architecture.WinApp.Helper;

public static class CrystalReportPrint_Helper
{
	public static void LoadReportsToDropdown(string folderName, ToolStripDropDownButton dropdown)
	{
		try
		{
			string[] filesSelected = null;
			string connStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"]?.ConnectionString;
			if (!string.IsNullOrEmpty(connStringReportPath))
			{
				string reportPath = Path.Combine(connStringReportPath, folderName);
				if (Directory.Exists(reportPath))
				{
					filesSelected = Directory.GetFiles(reportPath, "*.rpt");
				}
				else
				{
					Directory.CreateDirectory(reportPath);
				}
			}
			else
			{
				string reportPath = Path.Combine(Application.StartupPath, "..\\Reports\\", folderName);
				if (Directory.Exists(reportPath))
				{
					filesSelected = Directory.GetFiles(reportPath, "*.rpt");
				}
			}
			dropdown.DropDownItems.Clear();
			if (filesSelected != null && filesSelected.Length != 0)
			{
				string[] array = filesSelected;
				foreach (string file in array)
				{
					dropdown.DropDownItems.Add(Path.GetFileNameWithoutExtension(file));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void DeliveryOrderPrintByInvoicesId(int InvoiceId, int DocumentTypeId)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (InvoiceId == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			DataTable dt = InvDeliveryOrder.DeliveryOrderExortSlip_ByInvoiceId(organizationId, CompanyId, DocumentTypeId, InvoiceId);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "527-ExportDeliveryOrderByInvoiceIdSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void OutwardGatePassPrintByInvoicesIds_295(string InvoiceIds, int InvoiceId)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (string.IsNullOrWhiteSpace(InvoiceIds) && InvoiceId == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				InvoiceId = InvoiceId,
				InvoiceNo = InvoiceIds
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "295-InvRptOutwardGatePassSlipWithItems.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GdnSlipPartyProcessing339(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 89,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("Not Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "339-InvRptGoodsReceiptsNotesRiceSlipGDN.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnSlipPartyProcessing333(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 49,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("Not Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "333-InvRptGoodsReceiptsNotesRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockConversionPartyProcessing_SummeryReport600(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = StockConversionPartyProcessing.StockConversionPartyProcessingSummeryRpt(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = Id
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "600-StockConversionPartyProcessing_Summery_Rpt.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionPartyProcessingBillReport_611(int JobOrderId, int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.JobOrderId = JobOrderId;
			DataTable dt = InvProductionProcessingBill.InvProductionProcessingBill_PartyProcessingModule_Report_611(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvProductionProcessingBill.InvProductionProcessingBill_SubReport_611(obj), "PartyProcessingBillSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subBPReportObject = new SubReportObject(InvProductionProcessingBill.InvProductionProcessingBillByProduct_SubReport_611(obj), "PartyProcessingBillByProductSubReport.rpt");
			lstSubReports.Add(subBPReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "611-ProductionPartyProcessingBill.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionPartyProcessingSlip601_01(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionPartyProcessing.FoodProductionSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = RecId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "601_01-FoodProductionTransactionSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void LogisticRateNegotiationSlip_1300(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = PrintId;
				DataTable dtMain = logisticRateNegotiationHeader.logisticRateNegotiationHeader_Slip(obj);
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(logisticRateNegotiationHeader.logisticRateNegotiation_SourceDocumentSubReport(obj.Id), "logisticRateNegotiation_SourceDocumentSubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtMain, "1300_LogisticRateNegotiationSlip_Slip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void AgreementHeader_Slip_1301(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dtMain = AgreementHeader.AgreementHeader_Slip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtMain, "1301_AgreementHeader_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void LogisticRateNegotiationTransporterSlip_1302(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = PrintId;
				DataTable dtMain = logisticRateNegotiationHeader.logisticRateNegotiationHeader_Slip(obj);
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(logisticRateNegotiationHeader.logisticRateNegotiation_SourceDocumentSubReport(obj.Id), "logisticRateNegotiationTransporter_SourceDocumentSubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtMain, "1302_LogisticRateNegotiationTransporter_Slip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseOrderHeader_Slip_1303(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dtMain = PurchaseOrderHeader.PurchaseOrderHeader_Slip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtMain, "1303_PurchaseOrderHeader_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ServicesBillHeader_Slip_1304(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dtMain = ServicesBillHeader.ServicesBillHeader_Slip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtMain, "1304_ServicesBillHeader_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FreightVoucherOutward_Slip_1305(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dtMain = FreightVoucherOutward.FreightVoucherOutward_Slip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtMain, "1305_FreightVoucherOutward_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void FreightVoucherOutward_Slip_1305_01(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dtMain = FreightVoucherOutward.FreightVoucherOutward_Slip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					HeaderId = PrintId
				});
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtMain, "1305_FreightVoucherOutward_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void BuyerInquiryBookingSlip_1050A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InquiryBookingMaster.BuyerInquiryBooking_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1050A_BuyerInquiryBookingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void BuyerInquiryBookingSlip_1050(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InquiryBookingMaster.BuyerInquiryBooking_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1050_BuyerInquiryBookingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierOfferSlip_1051(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = purchaseOrderMaster.purchaseOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1051_SupplierOfferSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlip_1052(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = purchaseOrderMaster.purchaseOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1052_PurchaseOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void saleOrderSlip_1053(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = saleOrderMaster.saleOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1053_saleOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnSupplierLoadingChallanSlip_1054_01(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = grnSupplierLoadingMaster.grnSupplierLoading_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1054_01_GrnSupplierLoadingChallanSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnSupplierLoadingSlip_1054(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = grnSupplierLoadingMaster.grnSupplierLoading_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1054_GrnSupplierLoadingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GdnBuyerDispatchSlip_1055(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = gdnBuyerDispatchMaster.gdnBuyerDispatchMaster_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1055_GdnBuyerDispatchSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommissionAgentBillSlip_1056(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvCommAgentTradeBill.TradeBillSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1056_CommissionAgentTradeBillSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommissionAgentBillIncomeSlip_1056A(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvCommAgentTradeBill.CommissionBillSupplierCustomerAndIncomeTransactionsSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1056A_CommissionBillIncomeSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommissionAgentBillPurchaseSlip_1056_01(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvCommAgentTradeBill.TradeBillSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1056_01_CommissionAgentTradeBillPurchaseSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommissionAgentBillSaleSlip_1056_02(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvCommAgentTradeBill.TradeBillSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1056_02_CommissionAgentTradeBillSaleSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommissionAgentBillSaleSlip_1056_03(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvCommAgentTradeBill.TradeBillSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1056_03_CommissionAgentTradeBillSaleSlipII.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StorePurchasePreBillSlip147(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dt = PurchasePreBillHeader.PurchasePreBillHeader_Slip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchasePreBillHeader.PurchasePreBillHeader_ExpenseSubReport(obj.Id), "PurchasePreBillHeader_ExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "147_PurchasePreBillSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreDeliveryChallanSlip148(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dt = DeliveryChallanHeader.DeliveryChallanHeader_Slip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(DeliveryChallanHeader.DeliveryChallanHeader_ExpenseSubReport(obj.Id), "DeliveryChallanHeader_ExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "148_DeliveryChallanSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
