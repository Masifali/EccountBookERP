using System;
using System.Data;
using System.IO;
using Architecture.BLL.ContractorWages;
using Architecture.Model;
using Architecture.WebHelper.Helper;

namespace Architecture.WebHelper.Reports.StreamReportsHelper;

public class ContractorWagesReport
{
	public static string ContractorWagesBill_SlipandRegister_002(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(obj);
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

	public static string ContractorWagesBillDirect_SlipandRegister_003(ReportsParameters obj)
	{
		try
		{
			DataTable dt = InvContractorWagesBillDirect.InvContractorWagesBillDirect_SlipAndRegister(obj);
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
}
