using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.WebHelper.Entity;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports.StreamReportsHelper;

public class LabHistoryReports
{
	public static string SampleAnalysisRegister_650(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			DataTable dt = GenericProvider.GetDataTableProc<GatePassInward>("Sp_InvLabSampleAnalysisHeader_SlipandRegister", p);
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

	public static string RptInvLabSampleLogRegister_651(ReportsParameters obj)
	{
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
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
					ParameterName = "@FromSampleNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToSampleNo",
					Value = obj.ToDocNo
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_RiceSlipAndRegister_Rep", p);
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

	public static string RptInvLabAnalysisPurchaseRegister_653(ReportsParameters obj)
	{
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PurchaseOrderId",
					Value = obj.OrderId
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Datefrom",
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabAnalysisPurchase_RegisterReport", p);
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

	public static string RptInvLabPurchaseAnalysisSlip653(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Lab.InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dataForLabPurchaseAnalysisSubReport = Architecture.BLL.Lab.InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport(obj.Id);
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dataForLabPurchaseAnalysisSubReport, "LabPurchaseAnalysisSubParameterReport.rpt");
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

	public static string GetPrintSlipAndReport_657(ReportsParameters obj)
	{
		try
		{
			DataTable dt = Architecture.BLL.Lab.InvLabSampleAnalysisHeader.GetPrintSlipAndReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dataForLabSampleAnalysisSubReport = Architecture.BLL.Lab.InvLabSampleAnalysisHeader.GetDataForLabSampleAnalysisSubReport(obj.Id);
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dataForLabSampleAnalysisSubReport, "LabSampleAnalysisSubParameterReport.rpt");
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
}
