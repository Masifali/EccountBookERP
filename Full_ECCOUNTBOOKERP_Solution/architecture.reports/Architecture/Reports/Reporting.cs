using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Common;
using Architecture.Model;
using Architecture.Model.Common;
using CrystalDecisions.CrystalReports.Engine;

namespace Architecture.Reports;

public class Reporting
{
	private ReportViewer reportViewer = new ReportViewer();

	public bool ReportExport = true;

	public bool ReportPrint = true;

	public void RptPerameter(string parameter, string value)
	{
		ReportPerameter rp = new ReportPerameter();
		if (value != null)
		{
			if (value == string.Empty)
			{
				rp.value = null;
			}
			else
			{
				rp.value = value.ToString();
			}
		}
		rp.Perameter = parameter.ToString();
		reportViewer.lst.Add(rp);
	}

	public void ShowReport(string Reportname)
	{
		reportViewer.Reportname = Reportname;
		reportViewer.ReportExport = ReportExport;
		reportViewer.ReportPrint = ReportPrint;
		reportViewer.Show();
	}

	public void ShowReportWithDataTable(DataTable dt, string ReportName)
	{
		reportViewer.ftpserver = false;
		reportViewer.ReportDataTable = dt;
		reportViewer.Reportname = ReportName;
		reportViewer.Show();
	}

	public void ShowReportWithDataTableSubReprt(DataTable dt, string ReportName, List<SubReportObject> lstSubrpt = null)
	{
		reportViewer.ftpserver = false;
		reportViewer.ReportDataTable = dt;
		reportViewer.Reportname = ReportName;
		reportViewer.lstSubReports = lstSubrpt;
		reportViewer.Show();
	}

	public void ReportGenerateWithDataTableSubReportFromFTP(DataTable dt, string ReportPath, List<SubReportObject> lstSubrpt = null)
	{
		reportViewer.ReportDataTable = dt;
		reportViewer.reportPath = ReportPath;
		reportViewer.ftpserver = true;
		reportViewer.lstSubReports = lstSubrpt;
		reportViewer.Show();
	}

	public void ReportDirectPrint(DataTable dt, string ReportName, List<SubReportObject> lstSubrpt = null, List<ReportPerameter> lst = null)
	{
		ReportDocument report = new ReportDocument();
		try
		{
			string reportPath = string.Empty;
			string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
			reportPath = ((!(ConnStringReportPath != string.Empty)) ? (Application.ExecutablePath + "\\..\\Reports\\") : (ConnStringReportPath + "\\"));
			reportPath += ReportName;
			if (!File.Exists(reportPath))
			{
				throw new Exception("Report Or Configuration Path Not Found");
			}
			report.Dispose();
			report.Close();
			report.Load(reportPath);
			report.SetDataSource(dt);
			if (lstSubrpt != null && lstSubrpt.Count > 0)
			{
				foreach (SubReportObject item in lstSubrpt)
				{
					report.OpenSubreport(item.SubReportName).SetDataSource(item.dtSubReport);
				}
			}
			if (lst != null && lst != null && lst.Count > 0)
			{
				foreach (ReportPerameter items in lst)
				{
					report.SetParameterValue(items.Perameter, items.value);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		report.PrintToPrinter(1, collated: false, 0, 0);
	}

	public void LoadReport()
	{
		string reportPath = string.Empty;
		string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
		reportPath = ((!(ConnStringReportPath != string.Empty)) ? (Application.ExecutablePath + "\\..\\Reports\\") : (ConnStringReportPath + "\\"));
		ReportDocument crystalReport = new ReportDocument();
		reportPath += "CrystalLoadDLL.rpt";
		crystalReport.Load(reportPath);
	}

	public void ShowReportWithHeader(int ReportId, DataTable dt, string ReportName, List<SubReportObject> lstSubrpt = null)
	{
		List<SubReportObject> lstSub = new List<SubReportObject>();
		ReportsParameters obj = new ReportsParameters();
		lstSub.Add(SubReportRepository.GetCompanyHeader(ReportId));
		if (lstSubrpt != null)
		{
			foreach (SubReportObject item in lstSubrpt)
			{
				lstSub.Add(item);
			}
		}
		reportViewer.ReportDataTable = dt;
		reportViewer.Reportname = ReportName;
		reportViewer.lstSubReports = lstSub;
		reportViewer.Show();
	}

	public void CrystalLoaddDll(DataTable dt, string ReportName)
	{
		ReportDocument report = new ReportDocument();
		try
		{
			string reportPath = string.Empty;
			reportPath = Application.ExecutablePath + "\\..\\Reports\\";
			reportPath += ReportName;
			if (File.Exists(reportPath))
			{
				report.Dispose();
				report.Close();
				report.Load(reportPath);
			}
		}
		catch (Exception)
		{
		}
	}
}
