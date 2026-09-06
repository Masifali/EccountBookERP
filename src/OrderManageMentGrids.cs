using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.InventoryReports;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Inventory_Stocks_Report;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DynamicCards
{

public class OrderManageMentGrids : UserControl
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private string GridCFormName = "";

	private string GridCName = "";

	private DataTable _dt;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label lblTitle;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar3;

	public DataTable dataTable
	{
		get
		{
			return _dt;
		}
		set
		{
			_dt = value;
		}
	}

	public OrderManageMentGrids(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void OrderManageMentCardHeader_Load(object sender, EventArgs e)
	{
		try
		{
			if (dataTable.Rows.Count > 0)
			{
				lblTitle.Text = Conversion.ToString(dataTable.Rows[0]["ActivityDescription"]);
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("OrderCount", typeof(double));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("AvgRate40Kg", typeof(double));
				dt.Columns.Add("OrderAmount", typeof(double));
				dt.Columns.Add("StockQty", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dt.Rows.Add(dataTable.Rows[i]["ItemId"], dataTable.Rows[i]["ItemName"], dataTable.Rows[i]["OrderCount"], dataTable.Rows[i]["OrderBalanceQty"], dataTable.Rows[i]["OrderBalalanceWeight"], dataTable.Rows[i]["AvgRate40Kg"], dataTable.Rows[i]["OrderAmount"], dataTable.Rows[i]["StockBalanceQty"], dataTable.Rows[i]["StockBalanceWeight"]);
				}
				((Control)(object)grd).Name = Conversion.ToString(dataTable.Rows[0]["ActivityDescription"]);
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSetting()
	{
		try
		{
			grd.ColumnAutoResize = true;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["OrderCount"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["OrderCount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderCount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderCount"].FormatString = "#,#";
			grd.RootTable.Columns["OrderCount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AvgRate40Kg"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["AvgRate40Kg"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate40Kg"].FormatString = "#,##0.###";
			grd.RootTable.Columns["StockQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.###";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "ItemName")
			{
				int ItemId3 = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
				if (ItemId3 > 0)
				{
					frmStockReport frmStockReport = new frmStockReport(UserAccount);
					frmStockReport.cmbItem.Value = ItemId3;
					frmStockReport.ShowDialog();
				}
			}
			if (!(e.Column.Key == "OrderCount"))
			{
				return;
			}
			if (Conversion.ToString(dataTable.Rows[0]["TranType"]) == "Sales")
			{
				int ItemId = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
				if (ItemId > 0)
				{
					frmSaleOrderHistory obj = new frmSaleOrderHistory(UserAccount)
					{
						FromDate = clsGlobalVariables.ActiveYr.Start_Period.Date,
						To_Date = DateTime.Now
					};
					obj.CmbItem.Value = ItemId;
					((Control)(object)obj.CmbOrderStatus).Text = "Open";
					((Control)(object)obj.CmbIsApproved).Text = "All";
					obj.ShowDialog();
				}
			}
			else if (Conversion.ToString(dataTable.Rows[0]["TranType"]) == "Purchase")
			{
				int ItemId2 = Conversion.ToInt(grd.CurrentRow.Cells["ItemId"].Value);
				if (ItemId2 > 0)
				{
					PurchaseOrderHistory obj2 = new PurchaseOrderHistory(UserAccount)
					{
						FromDate = clsGlobalVariables.ActiveYr.Start_Period.Date,
						ToDate = DateTime.Now
					};
					obj2.CmbItem.Value = ItemId2;
					obj2.RadOpenDetail.Checked = true;
					obj2.RadApprovedOnlyDetail.Checked = true;
					obj2.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout("OrderManagementDashboardForPurchase", ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblTitle = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(920, 349);
		this.panel1.TabIndex = 1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(918, 316);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 6;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.lblTitle);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(918, 31);
		this.panel2.TabIndex = 0;
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(918, 31);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Title";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(871, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Visible = false;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.panel1);
		base.Name = "OrderManageMentGrids";
		base.Size = new System.Drawing.Size(920, 349);
		base.Load += new System.EventHandler(OrderManageMentCardHeader_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
