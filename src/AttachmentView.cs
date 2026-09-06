// using Attachment = global::Architecture.Model.Attachment;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using AttachmentPreviewControl = global::Architecture.WinApp.Common.AttachmentPreviewControl;

using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.VPS;
// // using Architecture.WinApp.DynamicCards;

public class AttachmentView : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtAttachment = new DataTable();

	private bool IsVpsAttachmentsServiceOn;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label label1;

	private Panel panel2;

	private Panel panel4;

	private Label label5;

	private Label lblTotalFiles;

	private Label lblTotalSize;

	private Label label2;

	private FlowLayoutPanel FlowLayoutAttachmentPreview;

	public AttachmentView(DataTable dt)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		dtAttachment = dt;
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		try
		{
			if (dtAttachment.Rows.Count <= 0)
			{
				return;
			}
			lblTotalFiles.Text = Conversion.ToString((object)dtAttachment.Rows.Count);
			string saveDirectory = "";
			saveDirectory = clsGlobalVariables.AttachmentFolderPath + "\\";
			double TotalFileSizeInMbs = 0.0;
			foreach (Control control in FlowLayoutAttachmentPreview.Controls)
			{
				control.Click -= UserControl_Click;
				control.Dispose();
			}
			FlowLayoutAttachmentPreview.Controls.Clear();
			ServerCredentials Sc = new ServerCredentials();
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			AttachmentPreviewControl[] AttachmentCards = new AttachmentPreviewControl[dtAttachment.Rows.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				string ForPath = ((Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != null && Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != string.Empty) ? Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) : Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]));
				AttachmentCards[i] = new AttachmentPreviewControl();
				string AttachmentName = ForPath;
				string path = "";
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
					string uri = "ftp://" + Sc.ServerIp + "/" + AttachmentName;
					try
					{
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
						request.Method = "RETR";
						request.UsePassive = true;
						request.UseBinary = true;
						request.KeepAlive = false;
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
						using Stream responseStream = ftpWebResponse.GetResponseStream();
						using MemoryStream memoryStream = new MemoryStream();
						responseStream.CopyTo(memoryStream);
						double fileSizeInMb = (double)memoryStream.Length / 1048576.0;
						TotalFileSizeInMbs += fileSizeInMb;
						AttachmentCards[i].FileSize = fileSizeInMb.ToString("0.000") + " Mb";
						Path.GetExtension(Path.GetFileName(request.RequestUri.LocalPath))?.ToUpper();
						memoryStream.Position = 0L;
						byte[] streamData = memoryStream.ToArray();
						AttachmentCards[i].memoryStream = streamData;
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse { StatusCode: FtpStatusCode.ActionNotTakenFileUnavailable })
						{
							Console.WriteLine("[INFO] Skipping missing FTP file: " + AttachmentName);
						}
						else
						{
							Console.WriteLine("[FTP ERROR] " + webEx.Message);
						}
						continue;
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
					AttachmentCards[i].path = "";
				}
				else
				{
					path = saveDirectory + ForPath;
					AttachmentCards[i].path = path;
					FileInfo fileInfo = new FileInfo(path);
					if (!fileInfo.Exists)
					{
						continue;
					}
					TotalFileSizeInMbs += (double)fileInfo.Length / 1048576.0;
					AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.000") + " Mb";
				}
				AttachmentCards[i].FileName = Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]);
				AttachmentCards[i].FileDate = Conversion.ToDateTime(dtAttachment.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy hh:mm tt");
				FlowLayoutAttachmentPreview.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
			lblTotalSize.Text = TotalFileSizeInMbs.ToString("0.000");
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			AttachmentPreviewControl obj = (AttachmentPreviewControl)sender;
			string path = obj.path;
			if (!IsVpsAttachmentsServiceOn)
			{
				if (File.Exists(path))
				{
					Process.Start(path);
				}
				return;
			}
			using MemoryStream memoryStream = new MemoryStream(obj.memoryStream);
			memoryStream.Position = 0L;
			string fileExtension = Path.GetExtension(obj.FileName).ToLower();
			string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + fileExtension);
			try
			{
				using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
				{
					memoryStream.CopyTo(fileStream);
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo(tempFilePath)
				{
					UseShellExecute = true,
					Verb = "open"
				};
				try
				{
					Process.Start(processStartInfo);
				}
				catch (Exception ex3)
				{
					MessageBox.Show("Could not open the file: " + ex3.Message);
					return;
				}
				Task.Run(async delegate
				{
					await Task.Delay(10000);
					for (int i = 0; i < 3; i++)
					{
						try
						{
							if (File.Exists(tempFilePath))
							{
								File.Delete(tempFilePath);
							}
							break;
						}
						catch (IOException)
						{
							await Task.Delay(1000);
						}
					}
				});
			}
			catch (Exception ex2)
			{
				MessageBox.Show("An error occurred while trying to open the file: " + ex2.Message);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An unexpected error occurred: " + ex.Message);
		}
	}

	private void AttachmentView_FormClosing(object sender, FormClosingEventArgs e)
	{
		foreach (Control control in FlowLayoutAttachmentPreview.Controls)
		{
			control.Click -= UserControl_Click;
			control.Dispose();
		}
		FlowLayoutAttachmentPreview.Controls.Clear();
		Dispose();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.FlowLayoutAttachmentPreview = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.lblTotalFiles = new System.Windows.Forms.Label();
		this.lblTotalSize = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.FlowLayoutAttachmentPreview);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(405, 700);
		this.panel1.TabIndex = 8;
		this.FlowLayoutAttachmentPreview.AutoScroll = true;
		this.FlowLayoutAttachmentPreview.Dock = System.Windows.Forms.DockStyle.Left;
		this.FlowLayoutAttachmentPreview.Location = new System.Drawing.Point(0, 99);
		this.FlowLayoutAttachmentPreview.Name = "FlowLayoutAttachmentPreview";
		this.FlowLayoutAttachmentPreview.Size = new System.Drawing.Size(404, 601);
		this.FlowLayoutAttachmentPreview.TabIndex = 7;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 43);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(405, 56);
		this.panel2.TabIndex = 6;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.lblTotalFiles);
		this.panel4.Controls.Add(this.lblTotalSize);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(404, 55);
		this.panel4.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(228, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(152, 25);
		this.label5.TabIndex = 3;
		this.label5.Text = "Total Size (Mbs)";
		this.lblTotalFiles.AutoSize = true;
		this.lblTotalFiles.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalFiles.ForeColor = System.Drawing.Color.White;
		this.lblTotalFiles.Location = new System.Drawing.Point(45, 28);
		this.lblTotalFiles.Name = "lblTotalFiles";
		this.lblTotalFiles.Size = new System.Drawing.Size(23, 25);
		this.lblTotalFiles.TabIndex = 2;
		this.lblTotalFiles.Text = "0";
		this.lblTotalSize.AutoSize = true;
		this.lblTotalSize.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalSize.ForeColor = System.Drawing.Color.White;
		this.lblTotalSize.Location = new System.Drawing.Point(266, 28);
		this.lblTotalSize.Name = "lblTotalSize";
		this.lblTotalSize.Size = new System.Drawing.Size(23, 25);
		this.lblTotalSize.TabIndex = 1;
		this.lblTotalSize.Text = "0";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(12, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 25);
		this.label2.TabIndex = 0;
		this.label2.Text = "Total Files";
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(405, 43);
		this.panel3.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(123, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "Attachments";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(405, 700);
		base.Controls.Add(this.panel1);
		base.Name = "AttachmentView";
		this.Text = "EccountBook";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AttachmentView_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
