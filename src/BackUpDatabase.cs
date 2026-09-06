using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Authentication;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

public class BackUpDatabase : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label lblOperation;

	private Button btnBackup;

	private ProgressBar progressBar1;

	private Label lblPercent;

	private BackgroundWorker backgroundWorker1;

	private Button btnclose;

	public BackUpDatabase(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		progressBar1.Visible = false;
		lblPercent.Visible = false;
		btnclose.Visible = false;
	}

	private void btnBackup_Click(object sender, EventArgs e)
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		try
		{
			string Folderpath = string.Empty;
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Backup Folder Path").ToList();
			Folderpath = ((list.Count <= 0) ? Application.ExecutablePath : list[0].ConfigKey.ToString());
			Cursor.Current = Cursors.Default;
			SqlConnection con = new SqlConnection(Services.GetConnectionString());
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(con.ConnectionString);
			sqlConnectionStringBuilder.InitialCatalog = "master";
			sqlConnectionStringBuilder.Password.ToString();
			SqlConnection con2 = new SqlConnection(sqlConnectionStringBuilder.ConnectionString.ToString());
			if (con2.State == ConnectionState.Closed)
			{
				con2.Open();
			}
			progressBar1.Value = 0;
			lblPercent.Visible = true;
			string dbbackup = con.Database.ToString();
			if (con.State.ToString() == "Open")
			{
				con.Close();
			}
			con.Open();
			string dbmappath = Folderpath + "\\" + dbbackup + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
			SqlCommand sqlCommand = new SqlCommand("BACKUP DATABASE [" + dbbackup + "] TO DISK='" + dbmappath + " .bak '", con2);
			sqlCommand.CommandTimeout = 200;
			sqlCommand.ExecuteNonQuery();
			con.Close();
			string zipPath = dbmappath + ".Zip";
			string FilePath = dbmappath + " .bak";
			string password = "Ali@Ayyan@Usman_Amir1421";
			using (FileStream backupFileStream = File.OpenRead(FilePath))
			{
				using FileStream zipFileStream = new FileStream(zipPath, FileMode.Create);
				ZipOutputStream zipOutputStream = new ZipOutputStream((Stream)zipFileStream);
				try
				{
					zipOutputStream.Password = password;
					ZipEntry zipEntry = new ZipEntry(Path.GetFileName(FilePath));
					zipOutputStream.PutNextEntry(zipEntry);
					byte[] buffer = new byte[4096];
					StreamUtils.Copy((Stream)backupFileStream, (Stream)(object)zipOutputStream, buffer);
					zipOutputStream.CloseEntry();
				}
				finally
				{
					((IDisposable)zipOutputStream)?.Dispose();
				}
			}
			File.Delete(FilePath);
			backgroundWorker1.RunWorkerAsync();
			progressBar1.Show();
			btnclose.Visible = true;
			btnBackup.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			for (int i = 1; i <= 100; i++)
			{
				Thread.Sleep(50);
				backgroundWorker1.WorkerReportsProgress = true;
				backgroundWorker1.ReportProgress(i);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
		lblPercent.Text = e.ProgressPercentage + "%";
		if (lblPercent.Text == "100%")
		{
			MessageBox.Show("Backup successfully", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Close();
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void BackUpDatabase_Load(object sender, EventArgs e)
	{
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
		this.btnclose = new System.Windows.Forms.Button();
		this.lblPercent = new System.Windows.Forms.Label();
		this.lblOperation = new System.Windows.Forms.Label();
		this.btnBackup = new System.Windows.Forms.Button();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnclose);
		this.panel1.Controls.Add(this.lblPercent);
		this.panel1.Controls.Add(this.lblOperation);
		this.panel1.Controls.Add(this.btnBackup);
		this.panel1.Controls.Add(this.progressBar1);
		this.panel1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1151, 548);
		this.panel1.TabIndex = 0;
		this.panel1.UseWaitCursor = true;
		this.btnclose.Location = new System.Drawing.Point(987, 107);
		this.btnclose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(92, 28);
		this.btnclose.TabIndex = 4;
		this.btnclose.Text = "Close";
		this.btnclose.UseVisualStyleBackColor = true;
		this.btnclose.UseWaitCursor = true;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.lblPercent.AutoSize = true;
		this.lblPercent.Location = new System.Drawing.Point(497, 51);
		this.lblPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(32, 17);
		this.lblPercent.TabIndex = 3;
		this.lblPercent.Text = "0 %";
		this.lblPercent.UseWaitCursor = true;
		this.lblOperation.AutoSize = true;
		this.lblOperation.Location = new System.Drawing.Point(36, 107);
		this.lblOperation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblOperation.Name = "lblOperation";
		this.lblOperation.Size = new System.Drawing.Size(0, 17);
		this.lblOperation.TabIndex = 2;
		this.lblOperation.UseWaitCursor = true;
		this.btnBackup.Location = new System.Drawing.Point(874, 107);
		this.btnBackup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnBackup.Name = "btnBackup";
		this.btnBackup.Size = new System.Drawing.Size(105, 28);
		this.btnBackup.TabIndex = 1;
		this.btnBackup.Text = "Start";
		this.btnBackup.UseVisualStyleBackColor = true;
		this.btnBackup.UseWaitCursor = true;
		this.btnBackup.Click += new System.EventHandler(btnBackup_Click);
		this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.progressBar1.ForeColor = System.Drawing.Color.Green;
		this.progressBar1.Location = new System.Drawing.Point(40, 13);
		this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(1047, 34);
		this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.progressBar1.TabIndex = 0;
		this.progressBar1.UseWaitCursor = true;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1151, 548);
		base.Controls.Add(this.panel1);
		this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "BackUpDatabase";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "BackUpDatabase";
		base.UseWaitCursor = true;
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(BackUpDatabase_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
