using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.AnalyticDashboard
{

public class AnalayticalAnalysisPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable data = new DataTable();

	private DataTable dtGrid = new DataTable();

	private int[] AryColWidth;

	private int ParentCategorId;

	private bool BranchFeature;

	private bool BranchSelected;

	private IContainer components;

	private Panel panel1;

	internal GridEX grdAnalyticalAnalysisPopUp;

	private Panel panel4;

	private Label lblFor;

	private Panel panel3;

	public AnalayticalAnalysisPopUp(UserAccount pUser, DataTable dt, int ParentId = 0, bool branchSelected = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		data = dt;
		UserAccount = pUser;
		ParentCategorId = ParentId;
		BranchSelected = branchSelected;
	}

	private void AnalayticalAnalysisPopUp_Load(object sender, EventArgs e)
	{
		try
		{
			BranchFeature = CommonServices.GetERPFeatureById(11);
			if (data.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("BranchesId", typeof(int));
					dtGrid.Columns.Add("BranchName");
					dtGrid.Columns.Add("FirstGroupName");
					dtGrid.Columns.Add("SecondGroupName");
					dtGrid.Columns.Add("Crop");
					dtGrid.Columns.Add("PrctSale", typeof(double));
					dtGrid.Columns.Add("Qty", typeof(double));
					dtGrid.Columns.Add("WeightKg", typeof(double));
					dtGrid.Columns.Add("AvgRate", typeof(double));
					dtGrid.Columns.Add("ItemAmount", typeof(double));
					dtGrid.Columns.Add("OtherExpense", typeof(double));
					dtGrid.Columns.Add("Commission", typeof(double));
					dtGrid.Columns.Add("Freight", typeof(double));
					dtGrid.Columns.Add("Exp40Kg", typeof(double));
					dtGrid.Columns.Add("AvgRateExp", typeof(double));
					dtGrid.Columns.Add("Amount", typeof(double));
					dtGrid.Columns.Add("AnalysisQty", typeof(double));
					dtGrid.Columns.Add("Moisture", typeof(double));
					dtGrid.Columns.Add("Empty Shell / Trash", typeof(double));
					dtGrid.Columns.Add("Dust/Stone", typeof(double));
					dtGrid.Columns.Add("Broken", typeof(double));
					dtGrid.Columns.Add("AGL", typeof(double));
					dtGrid.Columns.Add("Damage", typeof(double));
				}
				foreach (DataRow row in data.Rows)
				{
					double ItemAmount = ((ParentCategorId == 0) ? Conversion.ToDouble(row["ItemAmount"]) : 0.0);
					double OtherExpense = ((ParentCategorId == 0) ? Conversion.ToDouble(row["OtherExpense"]) : 0.0);
					double Commission = ((ParentCategorId == 0) ? Conversion.ToDouble(row["Commission"]) : 0.0);
					double Freight = ((ParentCategorId == 0) ? Conversion.ToDouble(row["Freight"]) : 0.0);
					dtGrid.Rows.Add(row["BranchesId"], row["BranchName"], row["FirstGroupName"], row["SecondGroupName"], row["CropBatch"], row["PrctSale"], row["Qty"], row["WeightKg"], row["AvgRate"], ItemAmount, OtherExpense, Commission, Freight, row["Exp40Kg"], row["AvgRateExp"], row["Amount"], row["AnalysisQty"], row["Moisture"], row["Empty Shell / Trash"], row["Dust/Stone"], row["Broken"], row["AGL"], row["Damage"]);
				}
				grdAnalyticalAnalysisPopUp.DataSource = dtGrid;
				grdSettingForBreaakUp();
			}
			else
			{
				grdAnalyticalAnalysisPopUp.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingForBreaakUp()
	{
		try
		{
			grdAnalyticalAnalysisPopUp.AllowDelete = (InheritableBoolean)2;
			grdAnalyticalAnalysisPopUp.AllowEdit = (InheritableBoolean)2;
			grdAnalyticalAnalysisPopUp.ColumnAutoResize = false;
			grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["FirstGroupName"].Visible = false;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["BranchesId"].Visible = false;
			if (data.Rows[0]["FirstGroupCaption"].ToString() == "Party Name")
			{
				grdAnalyticalAnalysisPopUp.RootTable.Columns["SecondGroupName"].Caption = "Item Name";
			}
			else
			{
				grdAnalyticalAnalysisPopUp.RootTable.Columns["SecondGroupName"].Caption = "Party Name";
			}
			if (ParentCategorId == 1)
			{
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Empty Shell / Trash"].Width = 90;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Dust/Stone"].Width = 80;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Moisture"].Width = 80;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Dust/Stone"].TextAlignment = (TextAlignment)3;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Dust/Stone"].FormatString = "#,#.###";
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Empty Shell / Trash"].FormatString = "#,#.###";
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Broken"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["AGL"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Damage"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["ItemAmount"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["OtherExpense"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Commission"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Freight"].Visible = false;
			}
			else if (ParentCategorId == 2)
			{
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Broken"].Width = 70;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["AGL"].Width = 70;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Damage"].Width = 70;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Broken"].FormatString = "#,#.###";
				grdAnalyticalAnalysisPopUp.RootTable.Columns["AGL"].FormatString = "#,#.###";
				grdAnalyticalAnalysisPopUp.RootTable.Columns["Damage"].FormatString = "#,#.###";
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Dust/Stone"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["ItemAmount"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["OtherExpense"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Commission"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Freight"].Visible = false;
			}
			else
			{
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Dust/Stone"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Broken"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["AGL"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Damage"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["Moisture"].Visible = false;
				grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["AnalysisQty"].Visible = false;
			}
			grdAnalyticalAnalysisPopUp.RootTable.ColumnSets["BranchName"].Visible = BranchFeature && BranchSelected;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchSelected;
			grdAnalyticalAnalysisPopUp.RootTable.Groups.Add("FirstGroupName");
			grdAnalyticalAnalysisPopUp.RootTable.Columns["BranchName"].Width = 120;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["SecondGroupName"].Width = 400;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Crop"].Width = 70;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].Width = 60;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Qty"].Width = 70;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["WeightKg"].Width = 90;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Amount"].Width = 110;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRate"].Width = 90;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Exp40Kg"].Width = 80;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRateExp"].Width = 100;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AnalysisQty"].Width = 80;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].Caption = "% Of Total";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["WeightKg"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AnalysisQty"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["OtherExpense"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["WeightKg"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Exp40Kg"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["OtherExpense"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].FormatString = "#,#0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Qty"].FormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["WeightKg"].FormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Amount"].FormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRate"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Exp40Kg"].FormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRateExp"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AnalysisQty"].FormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Moisture"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["OtherExpense"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["PrctSale"].TotalFormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["WeightKg"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Amount"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Exp40Kg"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AvgRateExp"].TotalFormatString = "#,##0.###";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["OtherExpense"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Commission"].TotalFormatString = "#,##0.####";
			grdAnalyticalAnalysisPopUp.RootTable.Columns["Freight"].TotalFormatString = "#,##0.####";
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		GridEXLayout grdAnalyticalAnalysisPopUp_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.AnalayticalAnalysisPopUp));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdAnalyticalAnalysisPopUp = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblFor = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAnalyticalAnalysisPopUp).BeginInit();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(814, 450);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(814, 425);
		this.panel3.TabIndex = 52;
		this.grdAnalyticalAnalysisPopUp.AllowEdit = (InheritableBoolean)2;
		this.grdAnalyticalAnalysisPopUp.AlternatingColors = true;
		this.grdAnalyticalAnalysisPopUp.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAnalyticalAnalysisPopUp_DesignTimeLayout.LayoutString = resources.GetString("grdAnalyticalAnalysisPopUp_DesignTimeLayout.LayoutString");
		this.grdAnalyticalAnalysisPopUp.DesignTimeLayout = grdAnalyticalAnalysisPopUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAnalyticalAnalysisPopUp.DynamicFiltering = true;
		this.grdAnalyticalAnalysisPopUp.FilterMode = (FilterMode)2;
		this.grdAnalyticalAnalysisPopUp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).Font = new System.Drawing.Font("Verdana", 9f);
		this.grdAnalyticalAnalysisPopUp.GroupTotals = (GroupTotals)2;
		this.grdAnalyticalAnalysisPopUp.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).Name = "grdAnalyticalAnalysisPopUp";
		((ScrollableBase)this.grdAnalyticalAnalysisPopUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).Size = new System.Drawing.Size(814, 425);
		((System.Windows.Forms.Control)(object)this.grdAnalyticalAnalysisPopUp).TabIndex = 48;
		this.grdAnalyticalAnalysisPopUp.TotalRow = (InheritableBoolean)1;
		this.grdAnalyticalAnalysisPopUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAnalyticalAnalysisPopUp).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.lblFor);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(814, 25);
		this.panel4.TabIndex = 51;
		this.lblFor.AutoSize = true;
		this.lblFor.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFor.ForeColor = System.Drawing.Color.White;
		this.lblFor.Location = new System.Drawing.Point(4, 4);
		this.lblFor.Name = "lblFor";
		this.lblFor.Size = new System.Drawing.Size(84, 19);
		this.lblFor.TabIndex = 5;
		this.lblFor.Text = "Break Up";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(814, 450);
		base.Controls.Add(this.panel1);
		base.Name = "AnalayticalAnalysisPopUp";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Pop Up";
		base.Load += new System.EventHandler(AnalayticalAnalysisPopUp_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAnalyticalAnalysisPopUp).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
