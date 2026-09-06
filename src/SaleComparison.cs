using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class SaleComparison : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private Panel panel2;

	private Panel panel3;

	private Label label27;

	private DateTimePicker datFromDate;

	private Label label1;

	private DateTimePicker datToDate;

	private Panel panel4;

	private Label label2;

	private Panel panel5;

	internal GridEX grdSaleComparison;

	private Panel panel6;

	private Panel panel7;

	internal GridEX grdQuarter;

	internal GridEX grdMonth;

	private Panel panel8;

	private Label label16;

	private Label label18;

	private Panel panel9;

	private ToolStripButton btnRefresh;

	private Button btnshow;

	public SaleComparison(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void SaleComparison_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			base.WindowState = FormWindowState.Maximized;
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datToDate.Value = clsGlobalVariables.ActiveYr.End_Period;
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			DataSet ds = InvSaleInvoiceReports.SalesComparisonsQuarter(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)datFromDate.Value),
				ToDate = Conversion.ToDateTime((object)datToDate.Value)
			});
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataTable dtgrid = new DataTable();
				dtgrid.Columns.Add("ActivityDescription");
				dtgrid.Columns.Add("InvParentCateCode");
				dtgrid.Columns.Add("SaleMonth");
				dtgrid.Columns.Add("SaleWeight", typeof(double));
				dtgrid.Columns.Add("SaleAmount", typeof(double));
				dtgrid.Columns.Add("TotalSales");
				dtgrid.Clear();
				for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
				{
					dtgrid.Rows.Add(ds.Tables[0].Rows[k]["ActivityDescription"], ds.Tables[0].Rows[k]["InvParentCateCode"], ds.Tables[0].Rows[k]["SaleMonth"], ds.Tables[0].Rows[k]["SaleWeight"], ds.Tables[0].Rows[k]["SaleAmount"], ds.Tables[0].Rows[k]["TotalSales"]);
				}
				grdSaleComparison.DataSource = dtgrid;
				grdSaleComparison.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdSaleComparison.ClearStructure();
			}
			if (ds.Tables[1].Rows.Count > 0)
			{
				DataTable dtQuarter = new DataTable();
				dtQuarter.Columns.Add("ActivityDescription");
				dtQuarter.Columns.Add("YearNo");
				dtQuarter.Columns.Add("QuarterNo");
				dtQuarter.Columns.Add("QuarterStartDate");
				dtQuarter.Columns.Add("QuarterEndDate");
				dtQuarter.Columns.Add("CurrSaleAmount", typeof(double));
				dtQuarter.Columns.Add("PrcntOfTotal", typeof(double));
				dtQuarter.Columns.Add("Curr-Prv_Diff", typeof(double));
				dtQuarter.Columns.Add("PrcntOfPrvQuarter", typeof(double));
				dtQuarter.Columns.Add("TotalSales");
				dtQuarter.Clear();
				for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
				{
					dtQuarter.Rows.Add(ds.Tables[1].Rows[j]["ActivityDescription"], ds.Tables[1].Rows[j]["YearNo"], ds.Tables[1].Rows[j]["QuarterNo"], Conversion.ToDateTime(ds.Tables[1].Rows[j]["QuarterStartDate"]).ToShortDateString(), Conversion.ToDateTime(ds.Tables[1].Rows[j]["QuarterEndDate"]).ToShortDateString(), ds.Tables[1].Rows[j]["CurrSaleAmount"], ds.Tables[1].Rows[j]["PrcntOfTotal"], ds.Tables[1].Rows[j]["Curr-Prv_Diff"], ds.Tables[1].Rows[j]["PrcntOfPrvQuarter"], ds.Tables[1].Rows[j]["TotalSales"]);
				}
				grdQuarter.DataSource = dtQuarter;
				grdQuarter.RetrieveStructure();
				GridSettingQuarterWise();
			}
			else
			{
				grdQuarter.ClearStructure();
			}
			if (ds.Tables[2].Rows.Count > 0)
			{
				DataTable dtMonth = new DataTable();
				dtMonth.Columns.Add("ActivityDescription");
				dtMonth.Columns.Add("SalesMonth");
				dtMonth.Columns.Add("CurrSaleAmount", typeof(double));
				dtMonth.Columns.Add("PrcntOfTotal", typeof(double));
				dtMonth.Columns.Add("Curr-Prv_Diff", typeof(double));
				dtMonth.Columns.Add("PrcntOfPrvMonth", typeof(double));
				dtMonth.Columns.Add("TotalSales");
				dtMonth.Clear();
				for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
				{
					dtMonth.Rows.Add(ds.Tables[2].Rows[i]["ActivityDescription"], ds.Tables[2].Rows[i]["SalesMonth"], ds.Tables[2].Rows[i]["CurrSaleAmount"], ds.Tables[2].Rows[i]["PrcntOfTotal"], ds.Tables[2].Rows[i]["Curr-Prv_Diff"], ds.Tables[2].Rows[i]["PrcntOfPrvMonth"], ds.Tables[2].Rows[i]["TotalSales"]);
				}
				grdMonth.DataSource = dtMonth;
				grdMonth.RetrieveStructure();
				GridSettingMonthWise();
			}
			else
			{
				grdMonth.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		CommonServices.GridAutoAdjustment(AryColWidth, grdSaleComparison);
		grdSaleComparison.RootTable.Columns["TotalSales"].Visible = false;
		grdSaleComparison.RootTable.Columns["SaleMonth"].Visible = false;
		grdSaleComparison.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
		grdSaleComparison.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
		grdSaleComparison.RootTable.Columns["SaleWeight"].FormatString = "#,##0.##";
		grdSaleComparison.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.##";
		grdSaleComparison.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
		grdSaleComparison.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
		grdSaleComparison.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatboth;
		grdSaleComparison.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
	}

	private void GridSettingQuarterWise()
	{
		CommonServices.GridAutoAdjustment(AryColWidth, grdQuarter);
		grdQuarter.RootTable.Columns["QuarterNo"].Visible = false;
		grdQuarter.RootTable.Columns["TotalSales"].Visible = false;
		grdQuarter.RootTable.Columns["YearNo"].Visible = false;
		grdQuarter.RootTable.Columns["CurrSaleAmount"].AggregateFunction = (AggregateFunction)2;
		grdQuarter.RootTable.Columns["CurrSaleAmount"].TextAlignment = (TextAlignment)3;
		grdQuarter.RootTable.Columns["CurrSaleAmount"].FormatString = clsGlobalVariables.stringFormatboth;
		grdQuarter.RootTable.Columns["CurrSaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
		grdQuarter.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
		grdQuarter.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
		grdQuarter.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.##";
		grdQuarter.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.##";
		grdQuarter.RootTable.Columns["Curr-Prv_Diff"].AggregateFunction = (AggregateFunction)2;
		grdQuarter.RootTable.Columns["Curr-Prv_Diff"].TextAlignment = (TextAlignment)3;
		grdQuarter.RootTable.Columns["Curr-Prv_Diff"].FormatString = "#,##0.##";
		grdQuarter.RootTable.Columns["Curr-Prv_Diff"].TotalFormatString = "#,##0.##";
		grdQuarter.RootTable.Columns["PrcntOfPrvQuarter"].AggregateFunction = (AggregateFunction)2;
		grdQuarter.RootTable.Columns["PrcntOfPrvQuarter"].TextAlignment = (TextAlignment)3;
		grdQuarter.RootTable.Columns["PrcntOfPrvQuarter"].FormatString = "#,##0.##";
		grdQuarter.RootTable.Columns["PrcntOfPrvQuarter"].TotalFormatString = "#,##0.##";
	}

	private void GridSettingMonthWise()
	{
		CommonServices.GridAutoAdjustment(AryColWidth, grdMonth);
		grdMonth.RootTable.Columns["TotalSales"].Visible = false;
		grdMonth.RootTable.Columns["CurrSaleAmount"].AggregateFunction = (AggregateFunction)2;
		grdMonth.RootTable.Columns["CurrSaleAmount"].TextAlignment = (TextAlignment)3;
		grdMonth.RootTable.Columns["CurrSaleAmount"].FormatString = clsGlobalVariables.stringFormatboth;
		grdMonth.RootTable.Columns["CurrSaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
		grdMonth.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
		grdMonth.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
		grdMonth.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.##";
		grdMonth.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.##";
		grdMonth.RootTable.Columns["Curr-Prv_Diff"].AggregateFunction = (AggregateFunction)2;
		grdMonth.RootTable.Columns["Curr-Prv_Diff"].TextAlignment = (TextAlignment)3;
		grdMonth.RootTable.Columns["Curr-Prv_Diff"].FormatString = "#,##0.##";
		grdMonth.RootTable.Columns["Curr-Prv_Diff"].TotalFormatString = "#,##0.##";
		grdMonth.RootTable.Columns["PrcntOfPrvMonth"].AggregateFunction = (AggregateFunction)2;
		grdMonth.RootTable.Columns["PrcntOfPrvMonth"].TextAlignment = (TextAlignment)3;
		grdMonth.RootTable.Columns["PrcntOfPrvMonth"].FormatString = "#,##0.##";
		grdMonth.RootTable.Columns["PrcntOfPrvMonth"].TotalFormatString = "#,##0.##";
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		grdSaleComparison.ClearStructure();
		grdQuarter.ClearStructure();
		grdMonth.ClearStructure();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		GridFill();
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		GridEXLayout grdMonth_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdQuarter_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleComparison_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdMonth = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdQuarter = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdSaleComparison = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMonth).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdQuarter).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleComparison).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(996, 749);
		this.panel1.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel8);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(996, 722);
		this.panel2.TabIndex = 5;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdMonth);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 557);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(996, 165);
		this.panel9.TabIndex = 127;
		this.grdMonth.AllowEdit = (InheritableBoolean)2;
		grdMonth_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMonth.DesignTimeLayout = grdMonth_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMonth).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdMonth.DynamicFiltering = true;
		this.grdMonth.FilterMode = (FilterMode)2;
		this.grdMonth.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMonth).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMonth.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMonth).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMonth).Name = "grdMonth";
		((ScrollableBase)this.grdMonth).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMonth).Size = new System.Drawing.Size(996, 165);
		((System.Windows.Forms.Control)(object)this.grdMonth).TabIndex = 3;
		this.grdMonth.TotalRow = (InheritableBoolean)1;
		this.grdMonth.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMonth).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label18);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 528);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(996, 29);
		this.panel8.TabIndex = 126;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(2, 5);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(104, 19);
		this.label18.TabIndex = 120;
		this.label18.Text = "Month Wise";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdQuarter);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 289);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(996, 239);
		this.panel7.TabIndex = 125;
		this.grdQuarter.AllowEdit = (InheritableBoolean)2;
		grdQuarter_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdQuarter.DesignTimeLayout = grdQuarter_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdQuarter).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdQuarter.DynamicFiltering = true;
		this.grdQuarter.FilterMode = (FilterMode)2;
		this.grdQuarter.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdQuarter).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdQuarter.GroupByBoxVisible = false;
		this.grdQuarter.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdQuarter).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdQuarter).Name = "grdQuarter";
		((ScrollableBase)this.grdQuarter).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdQuarter).Size = new System.Drawing.Size(996, 239);
		((System.Windows.Forms.Control)(object)this.grdQuarter).TabIndex = 2;
		this.grdQuarter.TotalRow = (InheritableBoolean)1;
		this.grdQuarter.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdQuarter).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label16);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 256);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(996, 33);
		this.panel6.TabIndex = 124;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(2, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(116, 19);
		this.label16.TabIndex = 120;
		this.label16.Text = "Quarter Wise";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleComparison);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 89);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(996, 167);
		this.panel5.TabIndex = 0;
		this.grdSaleComparison.AllowEdit = (InheritableBoolean)2;
		grdSaleComparison_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleComparison.DesignTimeLayout = grdSaleComparison_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSaleComparison.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleComparison.GroupByBoxVisible = false;
		this.grdSaleComparison.GroupTotals = (GroupTotals)2;
		this.grdSaleComparison.IncrementalSearchMode = (IncrementalSearchMode)2;
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).Name = "grdSaleComparison";
		((ScrollableBase)this.grdSaleComparison).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).Size = new System.Drawing.Size(996, 167);
		((System.Windows.Forms.Control)(object)this.grdSaleComparison).TabIndex = 1;
		this.grdSaleComparison.TotalRow = (InheritableBoolean)1;
		this.grdSaleComparison.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleComparison).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 32);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(996, 57);
		this.panel4.TabIndex = 123;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(299, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(64, 26);
		this.btnshow.TabIndex = 123;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 17);
		this.label27.TabIndex = 120;
		this.label27.Text = "From Date";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(154, 29);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(131, 21);
		this.datToDate.TabIndex = 122;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(156, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(54, 17);
		this.label1.TabIndex = 120;
		this.label1.Text = "To Date";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(11, 28);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(131, 21);
		this.datFromDate.TabIndex = 121;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(996, 32);
		this.panel3.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(145, 19);
		this.label2.TabIndex = 120;
		this.label2.Text = "Sale Comparison";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(996, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(996, 749);
		base.Controls.Add(this.panel1);
		base.Name = "SaleComparison";
		this.Text = "SaleComparison";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SaleComparison_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMonth).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdQuarter).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleComparison).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
