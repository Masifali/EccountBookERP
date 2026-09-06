using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Authentication;
using Architecture.WebHelper.Entity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Architecture.WebHelper.Helper;

public static class ReportHelper
{
	public static Stream GetStreamForFile(DataTable dt, string RptURL, List<SubReportObject> lstSubReports = null, List<ReportSectionObject> rptSection = null, DataTable dtParamenter = null)
	{
		try
		{
			ReportDocument crystalReport = new ReportDocument();
			LogHelper.Reportlogwriteondrive("Method intilize");
			LogHelper.Reportlogwriteondrive("Report Path is : " + RptURL);
			LogHelper.Reportlogwriteondrive("Total Record found : " + dt.Rows.Count);
			crystalReport.Dispose();
			crystalReport.Close();
			crystalReport = new ReportDocument();
			crystalReport.Load(RptURL);
			LogHelper.Reportlogwriteondrive("Report successfully loaded");
			if (dt != null)
			{
				crystalReport.SetDataSource(dt);
			}
			if (lstSubReports != null && lstSubReports.Count > 0)
			{
				LogHelper.Reportlogwriteondrive("Sub Report Intilize");
				foreach (SubReportObject item in lstSubReports)
				{
					crystalReport.OpenSubreport(item.SubReportName).SetDataSource(item.dtSubReport);
				}
			}
			if (dtParamenter.Rows.Count > 0 && dtParamenter != null)
			{
				LogHelper.Reportlogwriteondrive("Report Parameters Initilize");
				for (int i = 0; i < dtParamenter.Rows.Count; i++)
				{
					crystalReport.SetParameterValue(Conversion.ToString(dtParamenter.Rows[i]["Perameter"]), Conversion.ToString(dtParamenter.Rows[i]["value"]));
				}
			}
			Stream stream = crystalReport.ExportToStream(ExportFormatType.PortableDocFormat);
			stream.Seek(0L, SeekOrigin.Begin);
			crystalReport.Dispose();
			crystalReport.Close();
			LogHelper.Reportlogwriteondrive("Method Completed");
			return stream;
		}
		catch (Exception ex)
		{
			LogHelper.Reportlogwriteondrive("error has occured " + ex.Message);
			throw ex;
		}
	}

	public static void SavePdf(Stream getStream, string UploadPath)
	{
		try
		{
			FileStream fileStream = new FileStream(UploadPath, FileMode.Create, FileAccess.Write);
			getStream.CopyTo(fileStream);
			fileStream.Dispose();
			File.Exists(UploadPath);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static string GetConfigurationFromAllocation(int OrganizationId, int CompanyId, string ConfigDescription)
	{
		try
		{
			string ConfigKey = "";
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ConfigDescription",
				Value = ConfigDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetConfigurationByOrgCompandConfigDescription"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				ConfigKey = Conversion.ToString(dt.Rows[0]["ConfigKey"]);
			}
			return ConfigKey;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ReplaceCompLogoColumn(DataTable dtToChange, DataTable dtData)
	{
		try
		{
			if (dtToChange == null || dtData == null || dtData.Rows.Count == 0)
			{
				return;
			}
			if (dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
			}
			if (!dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns.Add("CompLogoImage", typeof(byte[]));
			}
			object logoValue = dtData.Rows[0][0];
			foreach (DataRow row in dtToChange.Rows)
			{
				row["CompLogoImage"] = ((logoValue != DBNull.Value) ? logoValue : null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message + " Exception from adding comp logo from dataset");
		}
	}
}
