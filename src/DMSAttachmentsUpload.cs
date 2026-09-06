using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL.DMS;
using Architecture.Model;
using Architecture.Model.DMS;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DMS_FolderHierarchy
{

public class DMSAttachmentsUpload : Form
{
	public class CustomFile
	{
		public string FileName { get; set; }

		public string FileNewPath { get; set; }

		public string FilePreviousPath { get; set; }

		public string FileExtension { get; set; }
	}

	private List<CustomFile> listOfFile = new List<CustomFile>();

	public string FolderPath;

	public long FolderHierarchyId;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel ParentPanel;

	private Panel panel1;

	private Button btnBrowse;

	private FlowLayoutPanel AttachmentsUploadedFlowLayoutPanel;

	private Button btnUpload;

	private Label lblPercent;

	private ProgressBar progressBar1;

	private BackgroundWorker backgroundWorker;

	public DMSAttachmentsUpload(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
		progressBar1.Visible = false;
		lblPercent.Visible = false;
	}

	private void DMSAttachmentsUpload_Load(object sender, EventArgs e)
	{
		try
		{
			ParentPanel.HorizontalScroll.Visible = true;
			ParentPanel.HorizontalScroll.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBrowse_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = true;
			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = fileDialog.FileNames;
				foreach (string file in fileNames)
				{
					FileInfo fi = new FileInfo(file);
					CustomFile obj = new CustomFile();
					obj.FileName = Path.GetFileName(file);
					obj.FileNewPath = FolderPath + "\\" + obj.FileName;
					obj.FileExtension = fi.Extension;
					obj.FilePreviousPath = file;
					listOfFile.Add(obj);
				}
				GenerateAttachmentsPreviewCards();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateAttachmentsPreviewCards()
	{
		try
		{
			AttachmentsUploadedFlowLayoutPanel.Controls.Clear();
			DMSAttachmentsUploadControl[] AttachmentCards = new DMSAttachmentsUploadControl[listOfFile.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				AttachmentCards[i] = new DMSAttachmentsUploadControl();
				string path = listOfFile[i].FilePreviousPath;
				AttachmentCards[i].path = path;
				FileInfo fileInfo = new FileInfo(path);
				AttachmentCards[i].FileName = fileInfo.Name;
				AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.00") + " Mb";
				AttachmentsUploadedFlowLayoutPanel.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			DMSAttachmentsUploadControl obj = (DMSAttachmentsUploadControl)sender;
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				AttachmentsUploadedFlowLayoutPanel.Controls.Remove(obj);
				listOfFile.RemoveAll((CustomFile x) => x.FileName == obj.FileName);
				return;
			}
			string path = obj.path;
			if (File.Exists(path))
			{
				Process.Start(path);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpload_Click(object sender, EventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		try
		{
			if (listOfFile.Count > 0)
			{
				DocFilePath docFile = new DocFilePath();
				docFile.docFilePathsList = new List<DocFilePath>();
				for (int i = 0; i < listOfFile.Count; i++)
				{
					DocFilePath Info = new DocFilePath();
					Info.OrganizationId = UserAccount.OrganizationId;
					Info.CompanyId = UserAccount.CompanyId;
					Info.DocFilePathId = 0L;
					Info.FolderHierarchyId = FolderHierarchyId;
					Info.FileName = listOfFile[i].FileName;
					Info.FilePath = listOfFile[i].FileNewPath;
					Info.FileExtension = listOfFile[i].FileExtension;
					Info.IsApproved = false;
					Info.CreatedById = UserAccount.ID;
					Info.CreatedDate = DateTime.Now;
					Info.ModifyById = UserAccount.ID;
					Info.ModifyDate = DateTime.Now;
					File.Copy(listOfFile[i].FilePreviousPath, FolderPath + "\\" + listOfFile[i].FileName);
					docFile.docFilePathsList.Add(Info);
				}
				if (docFile.docFilePathsList.Count > 0)
				{
					progressBar1.Value = 0;
					lblPercent.Visible = true;
					DocFilePath.Save(docFile);
					backgroundWorker.RunWorkerAsync();
					progressBar1.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DMSAttachmentsUpload_FormClosing(object sender, FormClosingEventArgs e)
	{
		listOfFile.Clear();
	}

	private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			for (int i = 1; i <= 100; i++)
			{
				Thread.Sleep(50);
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.ReportProgress(i);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
		lblPercent.Text = e.ProgressPercentage + "%";
		if (lblPercent.Text == "100%")
		{
			MessageBox.Show("File's Uploaded successfully", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			lblPercent.Visible = false;
			progressBar1.Visible = false;
			listOfFile.Clear();
			AttachmentsUploadedFlowLayoutPanel.Controls.Clear();
		}
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
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.AttachmentsUploadedFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblPercent = new System.Windows.Forms.Label();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.btnUpload = new System.Windows.Forms.Button();
		this.btnBrowse = new System.Windows.Forms.Button();
		this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
		this.ParentPanel.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.Controls.Add(this.AttachmentsUploadedFlowLayoutPanel);
		this.ParentPanel.Controls.Add(this.panel1);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(0, 0);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(870, 616);
		this.ParentPanel.TabIndex = 0;
		this.AttachmentsUploadedFlowLayoutPanel.BackColor = System.Drawing.Color.White;
		this.AttachmentsUploadedFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.AttachmentsUploadedFlowLayoutPanel.Location = new System.Drawing.Point(0, 56);
		this.AttachmentsUploadedFlowLayoutPanel.Name = "AttachmentsUploadedFlowLayoutPanel";
		this.AttachmentsUploadedFlowLayoutPanel.Size = new System.Drawing.Size(870, 560);
		this.AttachmentsUploadedFlowLayoutPanel.TabIndex = 1;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.lblPercent);
		this.panel1.Controls.Add(this.progressBar1);
		this.panel1.Controls.Add(this.btnUpload);
		this.panel1.Controls.Add(this.btnBrowse);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(870, 56);
		this.panel1.TabIndex = 0;
		this.lblPercent.AutoSize = true;
		this.lblPercent.Location = new System.Drawing.Point(182, 19);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(24, 13);
		this.lblPercent.TabIndex = 14;
		this.lblPercent.Text = "0 %";
		this.lblPercent.UseWaitCursor = true;
		this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.progressBar1.ForeColor = System.Drawing.Color.Green;
		this.progressBar1.Location = new System.Drawing.Point(219, 12);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(501, 28);
		this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.progressBar1.TabIndex = 13;
		this.progressBar1.UseWaitCursor = true;
		this.btnUpload.Location = new System.Drawing.Point(787, 18);
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.Size = new System.Drawing.Size(75, 23);
		this.btnUpload.TabIndex = 1;
		this.btnUpload.Text = "Upload";
		this.btnUpload.UseVisualStyleBackColor = true;
		this.btnUpload.Click += new System.EventHandler(btnUpload_Click);
		this.btnBrowse.Location = new System.Drawing.Point(22, 12);
		this.btnBrowse.Name = "btnBrowse";
		this.btnBrowse.Size = new System.Drawing.Size(75, 23);
		this.btnBrowse.TabIndex = 0;
		this.btnBrowse.Text = "Browse";
		this.btnBrowse.UseVisualStyleBackColor = true;
		this.btnBrowse.Click += new System.EventHandler(btnBrowse_Click);
		this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
		this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(870, 616);
		base.Controls.Add(this.ParentPanel);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "DMSAttachmentsUpload";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "DMS Attachments Upload";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DMSAttachmentsUpload_FormClosing);
		base.Load += new System.EventHandler(DMSAttachmentsUpload_Load);
		this.ParentPanel.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
