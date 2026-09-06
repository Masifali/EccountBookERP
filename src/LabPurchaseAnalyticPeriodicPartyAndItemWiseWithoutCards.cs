using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lab
{

public class LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards : Form
{
	public UserAccount UserAccount = new UserAccount();

	public DateTime FromDate;

	public DateTime ToDate;

	public DateTime SeasonStartDate;

	public DateTime SeasonEndDate;

	public int SortNo;

	public int ItemId;

	private IContainer components;

	private ToolStrip tsItemType;

	private Panel panel5;

	private Label label3;

	private Panel panel1;

	internal GridEX grd;

	private Label lblItemName;

	public LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void PurchaseAnalyticPeriodic_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 625);
			GetData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.LabPurchaseAnalyticsPartyWiseByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				StartDate = SeasonStartDate,
				EndDate = SeasonEndDate,
				ItemId = ItemId,
				SortNo = SortNo
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Description");
			dataTable.Columns.Add("Supplier");
			dataTable.Columns.Add("NoofVehicles", typeof(int));
			dataTable.Columns.Add("TotalQty", typeof(double));
			dataTable.Columns.Add("Moisture", typeof(double));
			dataTable.Columns.Add("GreenGrain", typeof(double));
			dataTable.Columns.Add("EmptyShell", typeof(double));
			dataTable.Columns.Add("Damage", typeof(double));
			dataTable.Columns.Add("Sherivalled", typeof(double));
			dataTable.Columns.Add("Fungus", typeof(double));
			dataTable.Columns.Add("AGL", typeof(double));
			dataTable.Columns.Add("Broken", typeof(double));
			dataTable.Columns.Add("AvgRate", typeof(double));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				lblItemName.Text = "( " + dt.Rows[i]["ItemName"].ToString() + " " + dt.Rows[i]["CropYear"].ToString() + " )";
				dataTable.Rows.Add(dt.Rows[i]["Description"], dt.Rows[i]["SupplierName"], dt.Rows[i]["NoofVehicles"], dt.Rows[i]["TotalQty"], dt.Rows[i]["Moisture"], dt.Rows[i]["GreenGrain"], dt.Rows[i]["EmptyShell"], dt.Rows[i]["Damage"], dt.Rows[i]["Sherivalled"], dt.Rows[i]["Fungus"], dt.Rows[i]["AGL"], dt.Rows[i]["Broken"], dt.Rows[i]["AvgRate"]);
			}
			grd.DataSource = dataTable;
			grd.RetrieveStructure();
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["Description"].Width = 100;
			grd.RootTable.Columns["Supplier"].Width = 200;
			grd.RootTable.Columns["NoofVehicles"].Width = 80;
			grd.RootTable.Columns["TotalQty"].Width = 80;
			grd.RootTable.Columns["Moisture"].Width = 60;
			grd.RootTable.Columns["GreenGrain"].Width = 55;
			grd.RootTable.Columns["EmptyShell"].Width = 55;
			grd.RootTable.Columns["Damage"].Width = 60;
			grd.RootTable.Columns["Sherivalled"].Width = 75;
			grd.RootTable.Columns["Fungus"].Width = 55;
			grd.RootTable.Columns["AGL"].Width = 55;
			grd.RootTable.Columns["Broken"].Width = 55;
			grd.RootTable.Columns["AvgRate"].Width = 110;
			grd.RootTable.Groups.Add("Description");
			grd.RootTable.Columns["NoofVehicles"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NoofVehicles"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NoofVehicles"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NoofVehicles"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Moisture"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Moisture"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Moisture"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["GreenGrain"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["GreenGrain"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GreenGrain"].FormatString = "#,##0.###";
			grd.RootTable.Columns["GreenGrain"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyShell"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["EmptyShell"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyShell"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyShell"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Damage"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Damage"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Damage"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Sherivalled"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Sherivalled"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Sherivalled"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Sherivalled"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Fungus"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Fungus"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Fungus"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Fungus"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AGL"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AGL"].FormatString = "#,##0.###";
			grd.RootTable.Columns["AGL"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Broken"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Broken"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Broken"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblItemName = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(909, 25);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblItemName);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(909, 27);
		this.panel5.TabIndex = 20;
		this.lblItemName.AutoSize = true;
		this.lblItemName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblItemName.ForeColor = System.Drawing.Color.White;
		this.lblItemName.Location = new System.Drawing.Point(429, 6);
		this.lblItemName.Name = "lblItemName";
		this.lblItemName.Size = new System.Drawing.Size(45, 16);
		this.lblItemName.TabIndex = 1;
		this.lblItemName.Text = "label1";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(415, 16);
		this.label3.TabIndex = 0;
		this.label3.Text = "Lab Purchase Analytic Periodic Product And Party Wise Report";
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 52);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(909, 503);
		this.panel1.TabIndex = 21;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.IncrementalSearchMode = (IncrementalSearchMode)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(909, 503);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(909, 555);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards";
		this.Text = "PurchaseAnalyticPeriodic";
		base.Load += new System.EventHandler(PurchaseAnalyticPeriodic_Load);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
