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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Authentication;
using CircularProgressBar;
using WinFormAnimation;

public class BackUpDatabaseNew : Form
{
	private BackgroundWorker backupWorker;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label lblOperation;

	private Button btnBackup;

	private Label lblPercent;

	private Label label3;

	private Label label4;

	private Label label2;

	private Label label1;

	private CircularProgressBar ProgBarDbBack;

	public BackUpDatabaseNew(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		backupWorker = new BackgroundWorker();
		backupWorker.WorkerReportsProgress = true;
		backupWorker.DoWork += BackupWorker_DoWork;
		backupWorker.ProgressChanged += BackupWorker_ProgressChanged;
		backupWorker.RunWorkerCompleted += BackupWorker_RunWorkerCompleted;
	}

	private void BackUpDatabase_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(880, 490);
			((Control)(object)ProgBarDbBack).Visible = false;
			lblPercent.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BackupWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			string Folderpath = string.Empty;
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Backup Folder Path").ToList();
			Folderpath = ((list.Count <= 0) ? Application.ExecutablePath : list[0].ConfigKey.ToString());
			BackUpLogs.RunDatabaseBackup(Services.GetConnectionString(), Folderpath, backupWorker, (string)null, (string)null);
			e.Result = "Database backup completed!";
			ProgBarDbBack.ProgressColor = Color.DarkCyan;
		}
		catch (Exception ex)
		{
			e.Result = "An error occurred during database backup:\n" + ex.Message;
		}
	}

	private void BackupWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		try
		{
			((ProgressBar)(object)ProgBarDbBack).Value = e.ProgressPercentage;
			lblPercent.Text = e.ProgressPercentage + "%";
			if (e.ProgressPercentage <= 25)
			{
				ProgBarDbBack.ProgressColor = Color.DarkTurquoise;
			}
			else if (e.ProgressPercentage <= 50)
			{
				ProgBarDbBack.ProgressColor = Color.DarkTurquoise;
			}
			else if (e.ProgressPercentage <= 75)
			{
				ProgBarDbBack.ProgressColor = Color.DarkTurquoise;
			}
			else
			{
				ProgBarDbBack.ProgressColor = Color.DarkTurquoise;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BackupWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		try
		{
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
			}
			else
			{
				MessageBox.Show(e.Result.ToString());
			}
			btnBackup.Enabled = true;
			((Control)(object)ProgBarDbBack).Hide();
			ProgBarDbBack.ProgressColor = Color.DarkCyan;
			lblPercent.Visible = false;
			lblPercent.Text = "0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public void btnBackup_Click(object sender, EventArgs e)
	{
		try
		{
			btnBackup.Enabled = false;
			((Control)(object)ProgBarDbBack).Show();
			lblPercent.Visible = true;
			lblPercent.Text = "0";
			backupWorker.RunWorkerAsync();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BackUpDatabaseNew_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			if (backupWorker.IsBusy)
			{
				backupWorker.CancelAsync();
			}
			backupWorker.Dispose();
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lblOperation = new System.Windows.Forms.Label();
		this.lblPercent = new System.Windows.Forms.Label();
		this.btnBackup = new System.Windows.Forms.Button();
		this.ProgBarDbBack = new CircularProgressBar();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.lblOperation);
		this.panel1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(182, 451);
		this.panel1.TabIndex = 0;
		this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(0, 383);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(182, 22);
		this.label3.TabIndex = 4;
		this.label3.Text = "EccounBookERP Backup";
		this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(0, 405);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(182, 46);
		this.label4.TabIndex = 4;
		this.label4.Text = "Your Shield in the Digital Realm.";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(0, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(182, 49);
		this.label2.TabIndex = 4;
		this.label2.Text = "Secure Your Data, Ensure Success";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 15f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(182, 51);
		this.label1.TabIndex = 3;
		this.label1.Text = "EccountBook ERP";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblOperation.AutoSize = true;
		this.lblOperation.Location = new System.Drawing.Point(27, 87);
		this.lblOperation.Name = "lblOperation";
		this.lblOperation.Size = new System.Drawing.Size(0, 13);
		this.lblOperation.TabIndex = 2;
		this.lblPercent.AutoSize = true;
		this.lblPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPercent.Location = new System.Drawing.Point(675, 19);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(37, 16);
		this.lblPercent.TabIndex = 3;
		this.lblPercent.Text = "0 %";
		this.btnBackup.BackColor = System.Drawing.Color.Teal;
		this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBackup.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBackup.ForeColor = System.Drawing.Color.White;
		this.btnBackup.Location = new System.Drawing.Point(727, 5);
		this.btnBackup.Name = "btnBackup";
		this.btnBackup.Size = new System.Drawing.Size(126, 37);
		this.btnBackup.TabIndex = 1;
		this.btnBackup.Text = "Start BackUp";
		this.btnBackup.UseVisualStyleBackColor = false;
		this.btnBackup.Click += new System.EventHandler(btnBackup_Click);
		this.ProgBarDbBack.AnimationFunction = (KnownAnimationFunctions)4;
		this.ProgBarDbBack.AnimationSpeed = 500;
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).BackColor = System.Drawing.Color.Transparent;
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).Font = new System.Drawing.Font("Microsoft Sans Serif", 72f, System.Drawing.FontStyle.Bold);
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.ProgBarDbBack.InnerColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.ProgBarDbBack.InnerMargin = 2;
		this.ProgBarDbBack.InnerWidth = -1;
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).Location = new System.Drawing.Point(182, 0);
		((System.Windows.Forms.ProgressBar)(object)this.ProgBarDbBack).MarqueeAnimationSpeed = 2000;
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).Name = "ProgBarDbBack";
		this.ProgBarDbBack.OuterColor = System.Drawing.Color.DarkCyan;
		this.ProgBarDbBack.OuterMargin = -25;
		this.ProgBarDbBack.OuterWidth = 26;
		this.ProgBarDbBack.ProgressColor = System.Drawing.Color.DarkTurquoise;
		this.ProgBarDbBack.ProgressWidth = 25;
		this.ProgBarDbBack.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36f);
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).Size = new System.Drawing.Size(486, 445);
		this.ProgBarDbBack.StartAngle = 270;
		this.ProgBarDbBack.SubscriptColor = System.Drawing.Color.FromArgb(166, 166, 166);
		this.ProgBarDbBack.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
		this.ProgBarDbBack.SubscriptText = ".23";
		this.ProgBarDbBack.SuperscriptColor = System.Drawing.Color.FromArgb(166, 166, 166);
		this.ProgBarDbBack.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
		this.ProgBarDbBack.SuperscriptText = "°C";
		((System.Windows.Forms.Control)(object)this.ProgBarDbBack).TabIndex = 5;
		this.ProgBarDbBack.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
		((System.Windows.Forms.ProgressBar)(object)this.ProgBarDbBack).Value = 68;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(864, 451);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.ProgBarDbBack);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.lblPercent);
		base.Controls.Add(this.btnBackup);
		this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		base.Name = "BackUpDatabaseNew";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "BackUpDatabase";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(BackUpDatabaseNew_FormClosed);
		base.Load += new System.EventHandler(BackUpDatabase_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
