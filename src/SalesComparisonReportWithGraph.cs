using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.Graph
{

public class SalesComparisonReportWithGraph : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount userAccount = new UserAccount();

	public DateTime FromDate;

	public DateTime ToDate;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Button btnshow;

	private Label label3;

	private TextBox txtTopRows;

	private Label label7;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Chart TopCustomersChart;

	private Panel panel5;

	private Panel panel7;

	private Chart TopItemsChart;

	private Panel panel8;

	private Chart TopCitiesChart;

	private Panel panel9;

	private Chart TopPackizesChart;

	private DateTimePicker dateToDate;

	private DateTimePicker datFromDate;

	private Label label27;

	private Label label33;

	private Panel panel6;

	private Label label1;

	private Panel panel10;

	private Label label2;

	private Panel panel11;

	private Label label4;

	private Panel panel12;

	private Label label5;

	internal GridEX GridTopCustomers;

	internal GridEX GridTopItems;

	internal GridEX GridTopCities;

	internal GridEX GridTopPackSizes;

	private GroupBox groupBox1;

	internal UltraCombo cmbItemtype;

	private Label label8;

	internal UltraCombo cmbIemParentcategory;

	private Label label6;

	private RadioButton btnradBottom;

	private RadioButton btnradTop;

	private TabControl tabControlReports;

	private TabPage tabOverallComparison;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	public TabControl tabControl1;

	public SalesComparisonReportWithGraph(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		userAccount = Puser;
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SalesComparisonReportWithGraph_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(990, 820);
			if (Conversion.CheckDateTimeNull(FromDate))
			{
				datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			}
			else
			{
				datFromDate.Value = FromDate;
			}
			if (Conversion.CheckDateTimeNull(ToDate))
			{
				dateToDate.Value = DateTime.Now;
			}
			else
			{
				dateToDate.Value = ToDate;
			}
			txtTopRows.Text = "10";
			CostCenterFill();
			if (userAccount.AppId == 5 && ((DataTable)((UltraGridBase)CmbCostCenter).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
				((Control)(object)CmbCostCenter).Enabled = false;
			}
			AllComboBind();
			datFromDate.Focus();
			ShowReport();
			tabControlReports.TabPages.Add("Monthly");
			MonthlySalesComparisonReport frm = new MonthlySalesComparisonReport(userAccount);
			frm.TopLevel = false;
			frm.Visible = true;
			frm.FormBorderStyle = FormBorderStyle.None;
			frm.Dock = DockStyle.Fill;
			tabControlReports.TabPages[1].Controls.Add(frm);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CostCenterFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dt = Projects.GetSubCostCenters(userAccount.OrganizationId, userAccount.CompanyId, userAccount.ID, userAccount.AppId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCostCenter.Value = Id;
					}
					else
					{
						((Control)(object)CmbCostCenter).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCostCenter).DataSource = null;
				((Control)(object)CmbCostCenter).Text = "";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			DataTable dt = InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				AppId = userAccount.AppId,
				UserId = userAccount.ID,
				ReportType = "Sale"
			});
			if (dt.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetParentCategory"
					select r;
				DataTable dtParent = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsType = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemType"
					select r;
				DataTable dtItemType = (rowsType.Any() ? rowsType.CopyToDataTable() : dt.Clone());
				DDL.BindDDLNew(dtParent, cmbIemParentcategory, "Id", "RefName", "Parent Category", false);
				((UltraGridBase)cmbIemParentcategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItemType, cmbItemtype, "Id", "RefName", "Item Type", false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowReport()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = userAccount.OrganizationId;
			obj.CompanyId = userAccount.CompanyId;
			obj.UserId = userAccount.ID;
			obj.AppId = userAccount.AppId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = dateToDate.Value;
			obj.NoOfRecords = Conversion.ToInt((object)txtTopRows.Text);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			if (btnradTop.Checked)
			{
				obj.ApprovedFilter = "Top";
			}
			else if (btnradBottom.Checked)
			{
				obj.ApprovedFilter = "Bottom";
			}
			obj.ParentCategoryId = Conversion.ToInt(cmbIemParentcategory.Value);
			if (userAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
			{
				throw new Exception("Cost Center Not Found");
			}
			obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dtHistory = InvSaleInvoiceReports.SaleComparisionsCustomerItemCityandPack_Wise(obj);
			if (dtHistory.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtTopCustomers = new DataTable();
			EnumerableRowCollection<DataRow> CustomerRows = from r in dtHistory.AsEnumerable()
				where r.Field<string>("DescriptionTitle") == "Customer"
				select r;
			dtTopCustomers = (CustomerRows.Any() ? CustomerRows.CopyToDataTable() : dtHistory.Clone());
			if (dtTopCustomers.Rows.Count > 0)
			{
				TopCustomersChart.DataSource = dtTopCustomers;
				TopCustomersChart.Series["Customers"].XValueMember = "GroupTitle";
				TopCustomersChart.Series["Customers"].YValueMembers = "SaleAmount";
				TopCustomersChart.Series["Customers"].ChartType = SeriesChartType.Column;
				TopCustomersChart.Titles[0].Text = "Top " + txtTopRows.Text + " Customers";
				TopCustomersChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe Ui", 10f, FontStyle.Bold);
				TopCustomersChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				TopCustomersChart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				TopCustomersChart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				TopCustomersChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
				TopCustomersChart.ChartAreas[0].AxisX.Interval = 1.0;
				TopCustomersChart.Series["Customers"].ToolTip = "#VALY{#,##0.##}";
				TopCustomersChart.Series["Customers"].YValueType = ChartValueType.Double;
				DataTable dt4 = new DataTable();
				dt4.Columns.Add("CustomerName", typeof(string));
				dt4.Columns.Add("SaleQty", typeof(double));
				dt4.Columns.Add("SaleWeight", typeof(double));
				dt4.Columns.Add("SaleAmount", typeof(double));
				dt4.Columns.Add("%OfTop", typeof(double));
				for (int l = 0; l < dtTopCustomers.Rows.Count; l++)
				{
					dt4.Rows.Add(dtTopCustomers.Rows[l]["GroupTitle"], dtTopCustomers.Rows[l]["SaleQty"], dtTopCustomers.Rows[l]["SaleWeight"], dtTopCustomers.Rows[l]["SaleAmount"], dtTopCustomers.Rows[l]["%OfTopBottom"]);
				}
				GridTopCustomers.DataSource = dt4;
				GridTopCustomers.RetrieveStructure();
				GridTopCustomers.RootTable.Columns["SaleQty"].AggregateFunction = (AggregateFunction)2;
				GridTopCustomers.RootTable.Columns["SaleQty"].TextAlignment = (TextAlignment)3;
				GridTopCustomers.RootTable.Columns["SaleQty"].FormatString = "#,##0.##";
				GridTopCustomers.RootTable.Columns["SaleQty"].TotalFormatString = "#,##0.##";
				GridTopCustomers.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
				GridTopCustomers.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
				GridTopCustomers.RootTable.Columns["SaleWeight"].FormatString = "#,##0.##";
				GridTopCustomers.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.##";
				GridTopCustomers.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
				GridTopCustomers.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
				GridTopCustomers.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridTopCustomers.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				GridTopCustomers.RootTable.Columns["%OfTop"].AggregateFunction = (AggregateFunction)2;
				GridTopCustomers.RootTable.Columns["%OfTop"].TextAlignment = (TextAlignment)3;
				GridTopCustomers.RootTable.Columns["%OfTop"].FormatString = "#,##0.##";
				GridTopCustomers.RootTable.Columns["%OfTop"].TotalFormatString = "#,##0.##";
				CommonServices.GridAutoAdjustment(AryColWidth, GridTopCustomers);
			}
			else
			{
				GridTopCustomers.ClearStructure();
			}
			DataTable dtTopItems = new DataTable();
			EnumerableRowCollection<DataRow> ItemRows = from r in dtHistory.AsEnumerable()
				where r.Field<string>("DescriptionTitle") == "Item"
				select r;
			dtTopItems = (ItemRows.Any() ? ItemRows.CopyToDataTable() : dtHistory.Clone());
			if (dtTopItems.Rows.Count > 0)
			{
				TopItemsChart.DataSource = dtTopItems;
				TopItemsChart.Series["Items"].XValueMember = "GroupTitle";
				TopItemsChart.Series["Items"].YValueMembers = "SaleAmount";
				TopItemsChart.Series["Items"].ChartType = SeriesChartType.Column;
				TopItemsChart.Titles[0].Text = "Top " + txtTopRows.Text + " Items";
				TopItemsChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe Ui", 10f, FontStyle.Bold);
				TopItemsChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				TopItemsChart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				TopItemsChart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				TopItemsChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
				TopItemsChart.ChartAreas[0].AxisX.Interval = 1.0;
				TopItemsChart.Series["Items"].ToolTip = "#VALY{#,##0.##}";
				TopItemsChart.Series["Items"].YValueType = ChartValueType.Double;
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("ItemName", typeof(string));
				dt3.Columns.Add("SaleQty", typeof(double));
				dt3.Columns.Add("SaleWeight", typeof(double));
				dt3.Columns.Add("SaleAmount", typeof(double));
				dt3.Columns.Add("%OfTop", typeof(double));
				for (int k = 0; k < dtTopItems.Rows.Count; k++)
				{
					dt3.Rows.Add(dtTopItems.Rows[k]["GroupTitle"], dtTopItems.Rows[k]["SaleQty"], dtTopItems.Rows[k]["SaleWeight"], dtTopItems.Rows[k]["SaleAmount"], dtTopItems.Rows[k]["%OfTopBottom"]);
				}
				GridTopItems.DataSource = dt3;
				GridTopItems.RetrieveStructure();
				GridTopItems.RootTable.Columns["SaleQty"].AggregateFunction = (AggregateFunction)2;
				GridTopItems.RootTable.Columns["SaleQty"].TextAlignment = (TextAlignment)3;
				GridTopItems.RootTable.Columns["SaleQty"].FormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["SaleQty"].TotalFormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
				GridTopItems.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
				GridTopItems.RootTable.Columns["SaleWeight"].FormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["%OfTop"].AggregateFunction = (AggregateFunction)2;
				GridTopItems.RootTable.Columns["%OfTop"].TextAlignment = (TextAlignment)3;
				GridTopItems.RootTable.Columns["%OfTop"].FormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["%OfTop"].TotalFormatString = "#,##0.##";
				GridTopItems.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
				GridTopItems.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
				GridTopItems.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridTopItems.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustment(AryColWidth, GridTopItems);
			}
			else
			{
				GridTopItems.ClearStructure();
			}
			DataTable dtTopCities = new DataTable();
			EnumerableRowCollection<DataRow> CityRows = from r in dtHistory.AsEnumerable()
				where r.Field<string>("DescriptionTitle") == "City"
				select r;
			dtTopCities = (CityRows.Any() ? CityRows.CopyToDataTable() : dtHistory.Clone());
			if (dtTopCities.Rows.Count > 0)
			{
				TopCitiesChart.DataSource = dtTopCities;
				TopCitiesChart.Series["Cities"].XValueMember = "GroupTitle";
				TopCitiesChart.Series["Cities"].YValueMembers = "SaleAmount";
				TopCitiesChart.Series["Cities"].ChartType = SeriesChartType.Column;
				TopCitiesChart.Titles[0].Text = "Top " + txtTopRows.Text + " Cities";
				TopCitiesChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe Ui", 10f, FontStyle.Bold);
				TopCitiesChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				TopCitiesChart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				TopCitiesChart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				TopCitiesChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
				TopCitiesChart.ChartAreas[0].AxisX.Interval = 1.0;
				TopCitiesChart.Series["Cities"].ToolTip = "#VALY{#,##0.##}";
				TopCitiesChart.Series["Cities"].YValueType = ChartValueType.Double;
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("CityName", typeof(string));
				dt2.Columns.Add("SaleQty", typeof(double));
				dt2.Columns.Add("SaleWeight", typeof(double));
				dt2.Columns.Add("SaleAmount", typeof(double));
				dt2.Columns.Add("%OfTop", typeof(double));
				for (int j = 0; j < dtTopCities.Rows.Count; j++)
				{
					dt2.Rows.Add(dtTopCities.Rows[j]["GroupTitle"], dtTopCities.Rows[j]["SaleQty"], dtTopCities.Rows[j]["SaleWeight"], dtTopCities.Rows[j]["SaleAmount"], dtTopCities.Rows[j]["%OfTopBottom"]);
				}
				GridTopCities.DataSource = dt2;
				GridTopCities.RetrieveStructure();
				GridTopCities.RootTable.Columns["SaleQty"].AggregateFunction = (AggregateFunction)2;
				GridTopCities.RootTable.Columns["SaleQty"].TextAlignment = (TextAlignment)3;
				GridTopCities.RootTable.Columns["SaleQty"].FormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["SaleQty"].TotalFormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
				GridTopCities.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
				GridTopCities.RootTable.Columns["SaleWeight"].FormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["%OfTop"].AggregateFunction = (AggregateFunction)2;
				GridTopCities.RootTable.Columns["%OfTop"].TextAlignment = (TextAlignment)3;
				GridTopCities.RootTable.Columns["%OfTop"].FormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["%OfTop"].TotalFormatString = "#,##0.##";
				GridTopCities.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
				GridTopCities.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
				GridTopCities.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridTopCities.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustment(AryColWidth, GridTopCities);
			}
			else
			{
				GridTopCities.ClearStructure();
			}
			DataTable dtTopPackSizes = new DataTable();
			EnumerableRowCollection<DataRow> PackSizeRows = from r in dtHistory.AsEnumerable()
				where r.Field<string>("DescriptionTitle") == "PackingSize"
				select r;
			dtTopPackSizes = (PackSizeRows.Any() ? PackSizeRows.CopyToDataTable() : dtHistory.Clone());
			if (dtTopPackSizes.Rows.Count > 0)
			{
				TopPackizesChart.DataSource = dtTopPackSizes;
				TopPackizesChart.Series["Pack Size"].XValueMember = "GroupTitle";
				TopPackizesChart.Series["Pack Size"].YValueMembers = "SaleAmount";
				TopPackizesChart.Series["Pack Size"].ChartType = SeriesChartType.Column;
				TopPackizesChart.Titles[0].Text = "Top " + txtTopRows.Text + " Pack Size";
				TopPackizesChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe Ui", 10f, FontStyle.Bold);
				TopPackizesChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				TopPackizesChart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				TopPackizesChart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				TopPackizesChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
				TopPackizesChart.ChartAreas[0].AxisX.Interval = 1.0;
				TopPackizesChart.Series["Pack Size"].ToolTip = "#VALY{#,##0.##}";
				TopPackizesChart.Series["Pack Size"].YValueType = ChartValueType.Double;
				DataTable dt = new DataTable();
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("SaleQty", typeof(double));
				dt.Columns.Add("SaleWeight", typeof(double));
				dt.Columns.Add("SaleAmount", typeof(double));
				dt.Columns.Add("%OfTop", typeof(double));
				for (int i = 0; i < dtTopPackSizes.Rows.Count; i++)
				{
					dt.Rows.Add(dtTopPackSizes.Rows[i]["GroupTitle"], dtTopPackSizes.Rows[i]["SaleQty"], dtTopPackSizes.Rows[i]["SaleWeight"], dtTopPackSizes.Rows[i]["SaleAmount"], dtTopPackSizes.Rows[i]["%OfTopBottom"]);
				}
				GridTopPackSizes.DataSource = dt;
				GridTopPackSizes.RetrieveStructure();
				GridTopPackSizes.RootTable.Columns["SaleQty"].AggregateFunction = (AggregateFunction)2;
				GridTopPackSizes.RootTable.Columns["SaleQty"].TextAlignment = (TextAlignment)3;
				GridTopPackSizes.RootTable.Columns["SaleQty"].FormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["SaleQty"].TotalFormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
				GridTopPackSizes.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
				GridTopPackSizes.RootTable.Columns["SaleWeight"].FormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["%OfTop"].AggregateFunction = (AggregateFunction)2;
				GridTopPackSizes.RootTable.Columns["%OfTop"].TextAlignment = (TextAlignment)3;
				GridTopPackSizes.RootTable.Columns["%OfTop"].FormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["%OfTop"].TotalFormatString = "#,##0.##";
				GridTopPackSizes.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
				GridTopPackSizes.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
				GridTopPackSizes.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridTopPackSizes.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustment(AryColWidth, GridTopPackSizes);
			}
			else
			{
				GridTopPackSizes.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtTopRows_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SalesComparisonReportWithGraph_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		GridEXLayout GridTopCustomers_DesignTimeLayout = new GridEXLayout();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
		GridEXLayout GridTopItems_DesignTimeLayout = new GridEXLayout();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title9 = new System.Windows.Forms.DataVisualization.Charting.Title();
		GridEXLayout GridTopCities_DesignTimeLayout = new GridEXLayout();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title10 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title11 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title12 = new System.Windows.Forms.DataVisualization.Charting.Title();
		GridEXLayout GridTopPackSizes_DesignTimeLayout = new GridEXLayout();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Graph.SalesComparisonReportWithGraph));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlReports = new System.Windows.Forms.TabControl();
		this.tabOverallComparison = new System.Windows.Forms.TabPage();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.GridTopCustomers = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.TopCustomersChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GridTopItems = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.TopItemsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.GridTopCities = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.TopCitiesChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.GridTopPackSizes = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.TopPackizesChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnradBottom = new System.Windows.Forms.RadioButton();
		this.btnradTop = new System.Windows.Forms.RadioButton();
		this.cmbItemtype = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbIemParentcategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.dateToDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtTopRows = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControlReports.SuspendLayout();
		this.tabOverallComparison.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridTopCustomers).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TopCustomersChart).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridTopItems).BeginInit();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TopItemsChart).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridTopCities).BeginInit();
		this.panel11.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TopCitiesChart).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridTopPackSizes).BeginInit();
		this.panel12.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TopPackizesChart).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbIemParentcategory).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlReports);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(972, 745);
		this.panel1.TabIndex = 0;
		this.tabControlReports.Controls.Add(this.tabOverallComparison);
		this.tabControlReports.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlReports.Location = new System.Drawing.Point(0, 0);
		this.tabControlReports.Name = "tabControlReports";
		this.tabControlReports.SelectedIndex = 0;
		this.tabControlReports.Size = new System.Drawing.Size(972, 745);
		this.tabControlReports.TabIndex = 149;
		this.tabOverallComparison.Controls.Add(this.tabControl1);
		this.tabOverallComparison.Controls.Add(this.panel4);
		this.tabOverallComparison.Controls.Add(this.panel3);
		this.tabOverallComparison.Controls.Add(this.toolStrip3);
		this.tabOverallComparison.Location = new System.Drawing.Point(4, 22);
		this.tabOverallComparison.Name = "tabOverallComparison";
		this.tabOverallComparison.Padding = new System.Windows.Forms.Padding(3);
		this.tabOverallComparison.Size = new System.Drawing.Size(964, 719);
		this.tabOverallComparison.TabIndex = 0;
		this.tabOverallComparison.Text = "Comparison ";
		this.tabOverallComparison.UseVisualStyleBackColor = true;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(3, 146);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(958, 570);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.GridTopCustomers);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(950, 541);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Top Customers";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.GridTopCustomers.AllowEdit = (InheritableBoolean)2;
		GridTopCustomers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridTopCustomers.DesignTimeLayout = GridTopCustomers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridTopCustomers.DynamicFiltering = true;
		this.GridTopCustomers.FilterMode = (FilterMode)2;
		this.GridTopCustomers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridTopCustomers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).Location = new System.Drawing.Point(3, 367);
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).Name = "GridTopCustomers";
		((ScrollableBase)this.GridTopCustomers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).Size = new System.Drawing.Size(944, 171);
		((System.Windows.Forms.Control)(object)this.GridTopCustomers).TabIndex = 0;
		this.GridTopCustomers.TotalRow = (InheritableBoolean)1;
		this.GridTopCustomers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridTopCustomers).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 337);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(944, 30);
		this.panel6.TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(9, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(244, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Tabular View Top Customers";
		this.panel5.Controls.Add(this.TopCustomersChart);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(944, 334);
		this.panel5.TabIndex = 8;
		chartArea1.Name = "ChartArea1";
		this.TopCustomersChart.ChartAreas.Add(chartArea1);
		this.TopCustomersChart.Dock = System.Windows.Forms.DockStyle.Fill;
		legend1.Name = "Legend1";
		this.TopCustomersChart.Legends.Add(legend1);
		this.TopCustomersChart.Location = new System.Drawing.Point(0, 0);
		this.TopCustomersChart.Name = "TopCustomersChart";
		series1.ChartArea = "ChartArea1";
		series1.Color = System.Drawing.Color.Green;
		series1.Legend = "Legend1";
		series1.Name = "Customers";
		series1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
		series1.YValuesPerPoint = 2;
		this.TopCustomersChart.Series.Add(series1);
		this.TopCustomersChart.Size = new System.Drawing.Size(944, 334);
		this.TopCustomersChart.TabIndex = 0;
		this.TopCustomersChart.Text = "Top Customers Chart";
		title1.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title1.Name = "Top Customers";
		title1.Text = "Top Customers";
		title5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title5.Name = "YAxisTitle";
		title5.Text = "Amount In Rs";
		title6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title6.Name = "XAxisTitle";
		title6.Text = "Customers";
		this.TopCustomersChart.Titles.Add(title1);
		this.TopCustomersChart.Titles.Add(title5);
		this.TopCustomersChart.Titles.Add(title6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridTopItems);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(950, 541);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Top Items";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GridTopItems.AllowEdit = (InheritableBoolean)2;
		GridTopItems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridTopItems.DesignTimeLayout = GridTopItems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridTopItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridTopItems.DynamicFiltering = true;
		this.GridTopItems.FilterMode = (FilterMode)2;
		this.GridTopItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridTopItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridTopItems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridTopItems).Location = new System.Drawing.Point(3, 367);
		((System.Windows.Forms.Control)(object)this.GridTopItems).Name = "GridTopItems";
		((ScrollableBase)this.GridTopItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridTopItems).Size = new System.Drawing.Size(944, 171);
		((System.Windows.Forms.Control)(object)this.GridTopItems).TabIndex = 11;
		this.GridTopItems.TotalRow = (InheritableBoolean)1;
		this.GridTopItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridTopItems).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 337);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(944, 30);
		this.panel10.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(9, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(205, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Tabular View Top Items";
		this.panel7.Controls.Add(this.TopItemsChart);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(944, 334);
		this.panel7.TabIndex = 9;
		chartArea2.Name = "ChartArea1";
		this.TopItemsChart.ChartAreas.Add(chartArea2);
		this.TopItemsChart.Dock = System.Windows.Forms.DockStyle.Fill;
		legend2.Name = "Legend1";
		this.TopItemsChart.Legends.Add(legend2);
		this.TopItemsChart.Location = new System.Drawing.Point(0, 0);
		this.TopItemsChart.Name = "TopItemsChart";
		series2.ChartArea = "ChartArea1";
		series2.Color = System.Drawing.Color.Green;
		series2.Legend = "Legend1";
		series2.Name = "Items";
		series2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
		series2.YValuesPerPoint = 2;
		this.TopItemsChart.Series.Add(series2);
		this.TopItemsChart.Size = new System.Drawing.Size(944, 334);
		this.TopItemsChart.TabIndex = 7;
		this.TopItemsChart.Text = "Top Items Chart";
		title7.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title7.Name = "Top Items";
		title7.Text = "Top Items";
		title8.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title8.Name = "YAxisTitle";
		title8.Text = "Amount In Rs";
		title9.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title9.Name = "XAxisTitle";
		title9.Text = "Items";
		this.TopItemsChart.Titles.Add(title7);
		this.TopItemsChart.Titles.Add(title8);
		this.TopItemsChart.Titles.Add(title9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.GridTopCities);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Controls.Add(this.panel8);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(950, 541);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Top Cities";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.GridTopCities.AllowEdit = (InheritableBoolean)2;
		GridTopCities_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridTopCities.DesignTimeLayout = GridTopCities_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridTopCities).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridTopCities.DynamicFiltering = true;
		this.GridTopCities.FilterMode = (FilterMode)2;
		this.GridTopCities.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridTopCities).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridTopCities.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridTopCities).Location = new System.Drawing.Point(0, 367);
		((System.Windows.Forms.Control)(object)this.GridTopCities).Name = "GridTopCities";
		((ScrollableBase)this.GridTopCities).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridTopCities).Size = new System.Drawing.Size(950, 174);
		((System.Windows.Forms.Control)(object)this.GridTopCities).TabIndex = 11;
		this.GridTopCities.TotalRow = (InheritableBoolean)1;
		this.GridTopCities.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridTopCities).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.label4);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 337);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 30);
		this.panel11.TabIndex = 10;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(9, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(204, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Tabular View Top Cities";
		this.panel8.Controls.Add(this.TopCitiesChart);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 337);
		this.panel8.TabIndex = 9;
		chartArea3.Name = "ChartArea1";
		this.TopCitiesChart.ChartAreas.Add(chartArea3);
		this.TopCitiesChart.Dock = System.Windows.Forms.DockStyle.Fill;
		legend3.Name = "Legend1";
		this.TopCitiesChart.Legends.Add(legend3);
		this.TopCitiesChart.Location = new System.Drawing.Point(0, 0);
		this.TopCitiesChart.Name = "TopCitiesChart";
		series3.ChartArea = "ChartArea1";
		series3.Color = System.Drawing.Color.Green;
		series3.Legend = "Legend1";
		series3.Name = "Cities";
		series3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
		series3.YValuesPerPoint = 2;
		this.TopCitiesChart.Series.Add(series3);
		this.TopCitiesChart.Size = new System.Drawing.Size(950, 337);
		this.TopCitiesChart.TabIndex = 7;
		this.TopCitiesChart.Text = "Top Cities Chart";
		title10.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title10.Name = "Top Cities";
		title10.Text = "Top Cities";
		title11.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title11.Name = "YAxisTitle";
		title11.Text = "Amount In Rs";
		title12.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title12.Name = "XAxisTitle";
		title12.Text = "Cities";
		this.TopCitiesChart.Titles.Add(title10);
		this.TopCitiesChart.Titles.Add(title11);
		this.TopCitiesChart.Titles.Add(title12);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.GridTopPackSizes);
		this.tabPage4.Controls.Add(this.panel12);
		this.tabPage4.Controls.Add(this.panel9);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(950, 541);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Top Pack Size";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.GridTopPackSizes.AllowEdit = (InheritableBoolean)2;
		GridTopPackSizes_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridTopPackSizes.DesignTimeLayout = GridTopPackSizes_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridTopPackSizes.DynamicFiltering = true;
		this.GridTopPackSizes.FilterMode = (FilterMode)2;
		this.GridTopPackSizes.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridTopPackSizes.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).Location = new System.Drawing.Point(0, 366);
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).Name = "GridTopPackSizes";
		((ScrollableBase)this.GridTopPackSizes).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).Size = new System.Drawing.Size(950, 175);
		((System.Windows.Forms.Control)(object)this.GridTopPackSizes).TabIndex = 11;
		this.GridTopPackSizes.TotalRow = (InheritableBoolean)1;
		this.GridTopPackSizes.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridTopPackSizes).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label5);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 336);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 30);
		this.panel12.TabIndex = 10;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(9, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(236, 19);
		this.label5.TabIndex = 1;
		this.label5.Text = "Tabular View Top Pack Size";
		this.panel9.Controls.Add(this.TopPackizesChart);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 336);
		this.panel9.TabIndex = 9;
		chartArea4.Name = "ChartArea1";
		this.TopPackizesChart.ChartAreas.Add(chartArea4);
		this.TopPackizesChart.Dock = System.Windows.Forms.DockStyle.Fill;
		legend4.Name = "Legend1";
		this.TopPackizesChart.Legends.Add(legend4);
		this.TopPackizesChart.Location = new System.Drawing.Point(0, 0);
		this.TopPackizesChart.Name = "TopPackizesChart";
		series4.ChartArea = "ChartArea1";
		series4.Color = System.Drawing.Color.Green;
		series4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		series4.Legend = "Legend1";
		series4.Name = "Pack Size";
		series4.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
		series4.YValuesPerPoint = 2;
		this.TopPackizesChart.Series.Add(series4);
		this.TopPackizesChart.Size = new System.Drawing.Size(950, 336);
		this.TopPackizesChart.TabIndex = 7;
		this.TopPackizesChart.Text = "Top Packizes Chart";
		title2.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title2.Name = "TopPackSizes";
		title2.Text = "Top Pack Sizes";
		title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title3.Name = "YAxisTitle";
		title3.Text = "Amount In Rs";
		title4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title4.Name = "XAxisTitle";
		title4.Text = "Pack Size";
		this.TopPackizesChart.Titles.Add(title2);
		this.TopPackizesChart.Titles.Add(title3);
		this.TopPackizesChart.Titles.Add(title4);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 60);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(958, 86);
		this.panel4.TabIndex = 148;
		this.groupBox1.Controls.Add(this.btnradBottom);
		this.groupBox1.Controls.Add(this.btnradTop);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbIemParentcategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.dateToDate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtTopRows);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(9, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(805, 79);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnradBottom.AutoSize = true;
		this.btnradBottom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnradBottom.Location = new System.Drawing.Point(712, 46);
		this.btnradBottom.Name = "btnradBottom";
		this.btnradBottom.Size = new System.Drawing.Size(79, 20);
		this.btnradBottom.TabIndex = 206;
		this.btnradBottom.TabStop = true;
		this.btnradBottom.Text = "Bottom";
		this.btnradBottom.UseVisualStyleBackColor = true;
		this.btnradTop.AutoSize = true;
		this.btnradTop.Checked = true;
		this.btnradTop.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnradTop.Location = new System.Drawing.Point(713, 18);
		this.btnradTop.Name = "btnradTop";
		this.btnradTop.Size = new System.Drawing.Size(51, 20);
		this.btnradTop.TabIndex = 205;
		this.btnradTop.TabStop = true;
		this.btnradTop.Text = "Top";
		this.btnradTop.UseVisualStyleBackColor = true;
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(314, 12);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(222, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 204;
		this.label8.Text = "Item Type";
		this.cmbIemParentcategory.AllowNull = (DefaultableBoolean)1;
		this.cmbIemParentcategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbIemParentcategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbIemParentcategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbIemParentcategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbIemParentcategory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbIemParentcategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbIemParentcategory).Location = new System.Drawing.Point(314, 42);
		((System.Windows.Forms.Control)(object)this.cmbIemParentcategory).Name = "cmbIemParentcategory";
		((System.Windows.Forms.Control)(object)this.cmbIemParentcategory).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbIemParentcategory).TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(222, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 202;
		this.label6.Text = "Parent Category";
		this.dateToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dateToDate.CustomFormat = "dd-MMM-yy";
		this.dateToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dateToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateToDate.Location = new System.Drawing.Point(70, 44);
		this.dateToDate.Name = "dateToDate";
		this.dateToDate.Size = new System.Drawing.Size(149, 22);
		this.dateToDate.TabIndex = 1;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(630, 42);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(77, 26);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(69, 14);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(150, 22);
		this.datFromDate.TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(590, 18);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(39, 15);
		this.label7.TabIndex = 141;
		this.label7.Text = "Count";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 149;
		this.label27.Text = "From Date";
		this.txtTopRows.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTopRows.Location = new System.Drawing.Point(630, 14);
		this.txtTopRows.Name = "txtTopRows";
		this.txtTopRows.Size = new System.Drawing.Size(77, 22);
		this.txtTopRows.TabIndex = 4;
		this.txtTopRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTopRows_KeyPress);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 48);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 148;
		this.label33.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1139, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(43, 15);
		this.label3.TabIndex = 109;
		this.label3.Text = "Month";
		this.label3.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel3.Controls.Add(this.lblCostCenter);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(958, 30);
		this.panel3.TabIndex = 6;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(323, 3);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 139;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(232, 9);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 140;
		this.lblCostCenter.Text = "Cost Center";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(214, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Sales Comparison Report";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(958, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 11f);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(74, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(972, 745);
		base.Controls.Add(this.panel1);
		base.Name = "SalesComparisonReportWithGraph";
		this.Text = "Sales Comparison Report With Graph";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SalesComparisonReportWithGraph_FormClosing);
		base.Load += new System.EventHandler(SalesComparisonReportWithGraph_Load);
		this.panel1.ResumeLayout(false);
		this.tabControlReports.ResumeLayout(false);
		this.tabOverallComparison.ResumeLayout(false);
		this.tabOverallComparison.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridTopCustomers).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TopCustomersChart).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridTopItems).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TopItemsChart).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridTopCities).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TopCitiesChart).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridTopPackSizes).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.TopPackizesChart).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbIemParentcategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
