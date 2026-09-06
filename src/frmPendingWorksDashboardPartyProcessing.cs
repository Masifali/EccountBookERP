using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.PartyProcessing.Dashboard
{

public class frmPendingWorksDashboardPartyProcessing : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label label1;

	private Panel panel2;

	private Label label3;

	private Label label7;

	private Button btnshow;

	private Panel panel4;

	private Panel panel5;

	private Panel panel19;

	private Panel panel20;

	private Label label4;

	private FlowLayoutPanel SaleDashboardFlowLayout;

	private FlowLayoutPanel PurchaseDashboardFlowLayout;

	public DateTimePicker datToDate;

	public DateTimePicker datFromDate;

	public frmPendingWorksDashboardPartyProcessing(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPendingWorksDashboardPartyProcessing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1238, 847);
			datFromDate.Focus();
			AllCardsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCardsBind()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			PurchaseDashboardFlowLayout.Controls.Clear();
			SaleDashboardFlowLayout.Controls.Clear();
			DataTable dthistory = InvFoodProductionPartyProcessing.PendingWorkDashboardPartyProcessing(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ReportType = "FiguresOnly"
			});
			if (dthistory.Rows.Count <= 0)
			{
				return;
			}
			int num = dthistory.AsEnumerable().Count((DataRow r) => r.Field<string>("PendingWorkDepartment") == "Purchase Department");
			DataTable dtPurchaseDepartment = new DataTable();
			DataTable dtSaleDepartment = new DataTable();
			if (num > 0)
			{
				dtPurchaseDepartment = (from r in dthistory.AsEnumerable()
					where r.Field<string>("PendingWorkDepartment") == "Purchase Department"
					select r).CopyToDataTable();
			}
			if (dthistory.AsEnumerable().Count((DataRow r) => r.Field<string>("PendingWorkDepartment") == "Sale Department") > 0)
			{
				dtSaleDepartment = (from r in dthistory.AsEnumerable()
					where r.Field<string>("PendingWorkDepartment") == "Sale Department"
					select r).CopyToDataTable();
			}
			if (dtPurchaseDepartment.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] PurchaseCardsList = new DynamicallyGenerateCards[dtPurchaseDepartment.Rows.Count];
				for (int j = 0; j < PurchaseCardsList.Length; j++)
				{
					PurchaseCardsList[j] = new DynamicallyGenerateCards();
					PurchaseCardsList[j].Id = Conversion.ToString(dtPurchaseDepartment.Rows[j]["PendingWorkDepartment"]);
					PurchaseCardsList[j].Title = Conversion.ToString(dtPurchaseDepartment.Rows[j]["PendingWorkCaption"]);
					PurchaseCardsList[j].Count = Conversion.ToString(dtPurchaseDepartment.Rows[j]["Count"]);
					PurchaseDashboardFlowLayout.Controls.Add(PurchaseCardsList[j]);
					PurchaseCardsList[j].UserControlClicked += UserControl_Click;
				}
			}
			if (dtSaleDepartment.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] SaleCardsList = new DynamicallyGenerateCards[dtSaleDepartment.Rows.Count];
				for (int i = 0; i < SaleCardsList.Length; i++)
				{
					SaleCardsList[i] = new DynamicallyGenerateCards();
					SaleCardsList[i].Id = Conversion.ToString(dtSaleDepartment.Rows[i]["PendingWorkDepartment"]);
					SaleCardsList[i].Title = Conversion.ToString(dtSaleDepartment.Rows[i]["PendingWorkCaption"]);
					SaleCardsList[i].Count = Conversion.ToString(dtSaleDepartment.Rows[i]["Count"]);
					SaleDashboardFlowLayout.Controls.Add(SaleCardsList[i]);
					SaleCardsList[i].UserControlClicked += UserControl_Click;
				}
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
			DynamicallyGenerateCards obj = (DynamicallyGenerateCards)sender;
			string DepartmentName = obj.Id;
			string pendingWorkName = obj.Title;
			GetDetailPendingWorkForPurchase(pendingWorkName, DepartmentName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			AllCardsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailPendingWorkForPurchase(string PendingWorkName, string DepartmentName)
	{
		try
		{
			frmPendingWorksDetailPartyProcessing obj = new frmPendingWorksDetailPartyProcessing(UserAccount);
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.DepartmentName = DepartmentName;
			obj.PendingWorkName = PendingWorkName;
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPendingWorksDashboardPartyProcessing_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.SaleDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PurchaseDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1222, 808);
		this.panel1.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel19);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 67);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1222, 741);
		this.panel3.TabIndex = 1;
		this.panel19.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel19.Controls.Add(this.SaleDashboardFlowLayout);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Location = new System.Drawing.Point(597, 7);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(613, 731);
		this.panel19.TabIndex = 7;
		this.SaleDashboardFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.SaleDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.SaleDashboardFlowLayout.Name = "SaleDashboardFlowLayout";
		this.SaleDashboardFlowLayout.Size = new System.Drawing.Size(613, 685);
		this.SaleDashboardFlowLayout.TabIndex = 1;
		this.panel20.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel20.Controls.Add(this.label4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(613, 46);
		this.panel20.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(153, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(255, 32);
		this.label4.TabIndex = 0;
		this.label4.Text = "Sale Pending Works";
		this.panel4.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel4.Controls.Add(this.PurchaseDashboardFlowLayout);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Location = new System.Drawing.Point(3, 7);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(588, 731);
		this.panel4.TabIndex = 6;
		this.PurchaseDashboardFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PurchaseDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.PurchaseDashboardFlowLayout.Name = "PurchaseDashboardFlowLayout";
		this.PurchaseDashboardFlowLayout.Size = new System.Drawing.Size(588, 685);
		this.PurchaseDashboardFlowLayout.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(588, 46);
		this.panel5.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(153, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(319, 32);
		this.label1.TabIndex = 0;
		this.label1.Text = "Purchase Pending Works";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.datToDate);
		this.panel2.Controls.Add(this.datFromDate);
		this.panel2.Controls.Add(this.btnshow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1222, 67);
		this.panel2.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(218, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(47, 15);
		this.label3.TabIndex = 78;
		this.label3.Text = "To Date";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(12, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 77;
		this.label7.Text = "From Date";
		this.datToDate.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datToDate.Location = new System.Drawing.Point(218, 33);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(200, 27);
		this.datToDate.TabIndex = 75;
		this.datFromDate.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datFromDate.Location = new System.Drawing.Point(12, 33);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(203, 27);
		this.datFromDate.TabIndex = 74;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(423, 33);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 27);
		this.btnshow.TabIndex = 76;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1222, 808);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmPendingWorksDashboardPartyProcessing";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPendingWorksDashboardPartyProcessing_FormClosed);
		base.Load += new System.EventHandler(frmPendingWorksDashboardPartyProcessing_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
