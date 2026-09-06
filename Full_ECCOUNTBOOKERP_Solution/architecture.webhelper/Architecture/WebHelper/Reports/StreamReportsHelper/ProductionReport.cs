using System;
using System.Data;
using System.IO;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Model;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports.StreamReportsHelper;

public class ProductionReport
{
	public static string InvFoodProductionSlip_601(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionSlip(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string InvFoodProductionRecoverySummeryReport_602(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionRecoverySummeryReport(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string PendingInvoicesForProduction_603(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.PendingInvoicesForProduction(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string FoodProductionComparisons_Rpt_604(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProduction.FoodProductionComparisons_Rpt(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string StockConversionSummeryReport_605(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.StockConversionSummeryReport(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string InvFoodProductionPackingAndOverHeadReportByJobOrder_606(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string FoodProductionIssuanceGrnWiseByJobOrderId_607(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static string Production_GainLossSummary_Rpt_610(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.Production_GainLossSummary_Rpt(obj);
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
				Stream streamForFile = ReportHelper.GetStreamForFile(dt, rptPath, null, null, rptdt);
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

	public static Stream FoodProductionSummery3_Rpt_615(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProduction_Summery3_Rpt(obj);
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
