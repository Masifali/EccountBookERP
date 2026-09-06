using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.VPS;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;

namespace Architecture.Reports;

public class ReportViewer : Form
{
	public List<ReportPerameter> lst = new List<ReportPerameter>();

	public bool ftpserver = false;

	public string reportPath = string.Empty;

	public string Reportname = string.Empty;

	public bool ReportExport;

	public bool ReportPrint;

	private bool CrystalFTPAllow = false;

	private int ZoomLevel = 0;

	public DataTable ReportDataTable = null;

	public DataTable SubReportDataTable = null;

	public List<SubReportObject> lstSubReports = null;

	private ReportDocument cryRpt = new ReportDocument();

	private IContainer components = null;

	private CrystalReportViewer crystalReportViewer1;

	public ReportViewer()
	{
		InitializeComponent();
	}

	public void ReportGenerateWithParam()
	{
		string getencriptedstring = Services.GetConnectionString();
		SqlConnection con = new SqlConnection(getencriptedstring);
		SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(con.ConnectionString);
		string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
		if (ConnStringReportPath != string.Empty)
		{
			reportPath = ConnStringReportPath + "\\";
		}
		else
		{
			reportPath = Application.ExecutablePath + "\\..\\Reports\\";
		}
		string spath = reportPath + Reportname;
		ResetViewer();
		ResetObject();
		cryRpt = new ReportDocument();
		cryRpt.Load(spath);
		cryRpt.DataSourceConnections[0].IntegratedSecurity = false;
		cryRpt.DataSourceConnections[0].SetConnection(con.DataSource, con.Database, builder.UserID, builder.Password.ToString());
		crystalReportViewer1.Refresh();
		if (lst.Count > 0)
		{
			foreach (ReportPerameter items in lst)
			{
				cryRpt.SetParameterValue(items.Perameter, items.value);
			}
		}
		crystalReportViewer1.ReportSource = cryRpt;
		if (ZoomLevel > 0)
		{
			crystalReportViewer1.Zoom(ZoomLevel);
		}
		crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
		lst.Clear();
	}

	public void ReportGenerateWithDataTable()
	{
		try
		{
			if (CrystalFTPAllow)
			{
				string ftpUri = "";
				ServerCredentials Sc = new ServerCredentials();
				Sc = GetServerCredentials();
				string clientname = clsGlobalVariables.UserAccount.ClientsNameForReports.Trim();
				ftpUri = ((!string.IsNullOrWhiteSpace(clientname)) ? ("ftp://" + Sc.ServerIp + "/Reports/" + Uri.EscapeDataString(clientname) + "/" + Reportname) : ("ftp://" + Sc.ServerIp + "/Reports/" + Reportname));
				reportPath = Path.Combine(Path.GetTempPath(), Reportname);
				try
				{
					bool shouldDownload = false;
					if (File.Exists(reportPath))
					{
						DateTime localFileLastModified = File.GetLastWriteTime(reportPath);
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUri);
						request.Method = "MDTM";
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
						DateTime ftpFileLastModified = response.LastModified;
						if (ftpFileLastModified > localFileLastModified)
						{
							shouldDownload = true;
						}
					}
					else
					{
						shouldDownload = true;
					}
					if (shouldDownload)
					{
						FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
						downloadRequest.Method = "RETR";
						downloadRequest.UsePassive = true;
						downloadRequest.UseBinary = true;
						downloadRequest.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
						{
							using Stream responseStream = downloadResponse.GetResponseStream();
							using FileStream fileStream = new FileStream(reportPath, FileMode.Create);
							responseStream.CopyTo(fileStream);
						}
						File.SetLastWriteTime(reportPath, DateTime.Now);
					}
					if (File.Exists(reportPath))
					{
						ResetViewer();
						ResetObject();
						cryRpt = new ReportDocument();
						cryRpt.Load(reportPath);
						cryRpt.SetDataSource(ReportDataTable);
						if (lst.Count > 0)
						{
							foreach (ReportPerameter items2 in lst)
							{
								cryRpt.SetParameterValue(items2.Perameter, items2.value);
							}
						}
						crystalReportViewer1.ReportSource = cryRpt;
						if (ZoomLevel > 0)
						{
							crystalReportViewer1.Zoom(ZoomLevel);
						}
						crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
						return;
					}
					throw new Exception("Report Configuration not found");
				}
				catch (UriFormatException)
				{
					MessageBox.Show("The FTP server address is not correctly formatted. Please check the FTP URL.", "Invalid FTP URI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				catch (WebException webEx)
				{
					if (webEx.Response is FtpWebResponse ftpResponse)
					{
						if (ftpResponse.StatusCode == FtpStatusCode.NotLoggedIn)
						{
							MessageBox.Show("Unable to connect to the FTP server. Please verify the FTP server IP, username, and password.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else if (ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
						{
							MessageBox.Show("The requested report is currently unavailable on the server. Please contact support or try again later.", "Report Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else
						{
							MessageBox.Show("FTP Error: " + ftpResponse.StatusDescription, "FTP Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else if (webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.NameResolutionFailure)
					{
						MessageBox.Show("Unable to connect to the internet. Please check your connection and try again.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						MessageBox.Show("Error downloading report from FTP: " + webEx.Message, "FTP Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					return;
				}
				catch (Exception ex2)
				{
					MessageBox.Show("An error occurred: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
			if (ConnStringReportPath != string.Empty)
			{
				reportPath = ConnStringReportPath + "\\";
			}
			else
			{
				reportPath = Application.ExecutablePath + "\\..\\Reports\\";
			}
			reportPath += Reportname;
			if (File.Exists(reportPath))
			{
				ResetViewer();
				ResetObject();
				cryRpt = new ReportDocument();
				cryRpt.Load(reportPath);
				cryRpt.SetDataSource(ReportDataTable);
				if (lst.Count > 0)
				{
					foreach (ReportPerameter items in lst)
					{
						cryRpt.SetParameterValue(items.Perameter, items.value);
					}
				}
				crystalReportViewer1.ReportSource = cryRpt;
				if (ZoomLevel > 0)
				{
					crystalReportViewer1.Zoom(ZoomLevel);
				}
				crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
				return;
			}
			throw new Exception("Report Or Configuration Path Not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void ReportGenerateWithDataTableSubReport()
	{
		try
		{
			if (CrystalFTPAllow)
			{
				string ftpUri = "";
				ServerCredentials Sc = new ServerCredentials();
				Sc = GetServerCredentials();
				string clientname = clsGlobalVariables.UserAccount.ClientsNameForReports.Trim();
				ftpUri = ((!string.IsNullOrWhiteSpace(clientname)) ? ("ftp://" + Sc.ServerIp + "/Reports/" + Uri.EscapeDataString(clientname) + "/" + Reportname) : ("ftp://" + Sc.ServerIp + "/Reports/" + Reportname));
				reportPath = Path.Combine(Path.GetTempPath(), Reportname);
				try
				{
					bool shouldDownload = false;
					if (File.Exists(reportPath))
					{
						DateTime localFileLastModified = File.GetLastWriteTime(reportPath);
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUri);
						request.Method = "MDTM";
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
						DateTime ftpFileLastModified = response.LastModified;
						if (ftpFileLastModified > localFileLastModified)
						{
							shouldDownload = true;
						}
					}
					else
					{
						shouldDownload = true;
					}
					if (shouldDownload)
					{
						FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
						downloadRequest.Method = "RETR";
						downloadRequest.UsePassive = true;
						downloadRequest.UseBinary = true;
						downloadRequest.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
						{
							using Stream responseStream = downloadResponse.GetResponseStream();
							using FileStream fileStream = new FileStream(reportPath, FileMode.Create);
							responseStream.CopyTo(fileStream);
						}
						File.SetLastWriteTime(reportPath, DateTime.Now);
					}
					if (File.Exists(reportPath))
					{
						ResetViewer();
						ResetObject();
						cryRpt = new ReportDocument();
						cryRpt.Load(reportPath);
						cryRpt.SetDataSource(ReportDataTable);
						if (lstSubReports != null && lstSubReports.Count > 0)
						{
							List<string> subreportNames2 = (from ReportDocument r in cryRpt.Subreports
								select r.Name).ToList();
							foreach (SubReportObject item in lstSubReports)
							{
								if (item.dtSubReport != null && subreportNames2.Contains(item.SubReportName))
								{
									cryRpt.OpenSubreport(item.SubReportName).SetDataSource(item.dtSubReport);
								}
							}
						}
						if (lst.Count > 0)
						{
							foreach (ReportPerameter items in lst)
							{
								cryRpt.SetParameterValue(items.Perameter, items.value);
							}
						}
						crystalReportViewer1.ReportSource = cryRpt;
						if (ZoomLevel > 0)
						{
							crystalReportViewer1.Zoom(ZoomLevel);
						}
						crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
						return;
					}
					throw new Exception("Report Configuration not found");
				}
				catch (UriFormatException)
				{
					MessageBox.Show("The FTP server address is not correctly formatted. Please check the FTP URL.", "Invalid FTP URI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				catch (WebException webEx)
				{
					if (webEx.Response is FtpWebResponse ftpResponse)
					{
						if (ftpResponse.StatusCode == FtpStatusCode.NotLoggedIn)
						{
							MessageBox.Show("Unable to connect to the FTP server. Please verify the FTP server IP, username, and password.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else if (ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
						{
							MessageBox.Show("The requested report is currently unavailable on the server. Please contact support or try again later.", "Report Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else
						{
							MessageBox.Show("FTP Error: " + ftpResponse.StatusDescription, "FTP Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else if (webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.NameResolutionFailure)
					{
						MessageBox.Show("Unable to connect to the internet. Please check your connection and try again.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						MessageBox.Show("Error downloading report from FTP: " + webEx.Message, "FTP Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					return;
				}
				catch (Exception ex2)
				{
					MessageBox.Show("An error occurred: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
			if (ConnStringReportPath != string.Empty)
			{
				reportPath = ConnStringReportPath + "\\";
			}
			else
			{
				reportPath = Application.ExecutablePath + "\\..\\Reports\\";
			}
			reportPath += Reportname;
			if (File.Exists(reportPath))
			{
				ResetViewer();
				ResetObject();
				cryRpt = new ReportDocument();
				cryRpt.Load(reportPath);
				cryRpt.SetDataSource(ReportDataTable);
				if (lstSubReports != null && lstSubReports.Count > 0)
				{
					List<string> subreportNames = (from ReportDocument r in cryRpt.Subreports
						select r.Name).ToList();
					foreach (SubReportObject item2 in lstSubReports)
					{
						if (item2.dtSubReport != null && subreportNames.Contains(item2.SubReportName))
						{
							cryRpt.OpenSubreport(item2.SubReportName).SetDataSource(item2.dtSubReport);
						}
					}
				}
				if (lst.Count > 0)
				{
					foreach (ReportPerameter items2 in lst)
					{
						cryRpt.SetParameterValue(items2.Perameter, items2.value);
					}
				}
				crystalReportViewer1.ReportSource = cryRpt;
				if (ZoomLevel > 0)
				{
					crystalReportViewer1.Zoom(ZoomLevel);
				}
				crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
				return;
			}
			throw new Exception("Report Configuration not found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetViewer()
	{
		crystalReportViewer1.ReportSource = null;
		crystalReportViewer1.Refresh();
	}

	private void ResetObject()
	{
		try
		{
			if (cryRpt != null)
			{
				cryRpt.Close();
				cryRpt.Dispose();
				cryRpt = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReportViewer_FormClosed(object sender, FormClosedEventArgs e)
	{
		ResetViewer();
		ResetObject();
		DeleteCrystalTempFiles();
	}

	private void ReportViewer_Load(object sender, EventArgs e)
	{
		try
		{
			ZoomLevel = Conversion.ToInt(clsGlobalVariables.CrystalPrintPreviewZoomDefault);
			CrystalFTPAllow = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, "CrystalAllowFromFTPServer"));
			if (lstSubReports != null)
			{
				ReportGenerateWithDataTableSubReport();
			}
			if (ReportDataTable != null && lstSubReports == null)
			{
				ReportGenerateWithDataTable();
			}
			if (lstSubReports == null && ReportDataTable == null)
			{
				ReportGenerateWithParam();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportViewer_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			crystalReportViewer1.PrintReport();
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	public static void DeleteCrystalTempFiles()
	{
		try
		{
			string tempPath = Path.GetTempPath();
			string[] rptFiles = Directory.GetFiles(tempPath, "*.rpt", SearchOption.TopDirectoryOnly);
			string[] array = rptFiles;
			foreach (string file in array)
			{
				if (!IsFileLocked(file))
				{
					File.Delete(file);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool TryDeleteFile(string filePath, int maxRetries = 3)
	{
		for (int i = 0; i < maxRetries; i++)
		{
			try
			{
				if (!IsFileLocked(filePath))
				{
					File.Delete(filePath);
					return true;
				}
			}
			catch
			{
			}
			Thread.Sleep(200);
		}
		return false;
	}

	private static bool IsFileLocked(string filePath)
	{
		FileStream stream = null;
		try
		{
			stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}
		catch (IOException)
		{
			return true;
		}
		finally
		{
			stream?.Close();
		}
		return false;
	}

	public static ServerCredentials GetServerCredentials()
	{
		ServerCredentials obj = new ServerCredentials();
		try
		{
			string ServerIp = ConfigurationManager.ConnectionStrings["VPSName"].ConnectionString;
			string VPSUser = ConfigurationManager.ConnectionStrings["VPSUser"].ConnectionString;
			string VPSPwd = ConfigurationManager.ConnectionStrings["VPSPwd"].ConnectionString;
			if (string.IsNullOrEmpty(ServerIp))
			{
				throw new InvalidOperationException("The server IP configuration is not set.");
			}
			obj.ServerIp = Encryption.DecryptString(ServerIp);
			if (string.IsNullOrEmpty(VPSUser))
			{
				throw new InvalidOperationException("The VPS user configuration is not set.");
			}
			obj.UserName = Encryption.DecryptString(VPSUser);
			if (string.IsNullOrEmpty(VPSPwd))
			{
				throw new InvalidOperationException("The VPS password configuration is not set.");
			}
			obj.Password = Encryption.DecryptString(VPSPwd);
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Error while getting server credentials", ex);
		}
		return obj;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
		base.SuspendLayout();
		this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
		base.Controls.Add(this.crystalReportViewer1);
		this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.crystalReportViewer1.Name = "crystalReportViewer1";
		this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
		this.crystalReportViewer1.ActiveViewIndex = -1;
		this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
		this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
		this.crystalReportViewer1.Name = "crystalReportViewer1";
		this.crystalReportViewer1.Size = new System.Drawing.Size(800, 450);
		this.crystalReportViewer1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.crystalReportViewer1);
		base.KeyPreview = true;
		base.Name = "ReportViewer";
		this.Text = "Report Viewer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ReportViewer_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ReportViewer_KeyDown);
		base.ResumeLayout(false);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ReportViewer_FormClosed);
	}
}
