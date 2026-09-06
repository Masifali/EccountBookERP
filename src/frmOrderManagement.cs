using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Dashboard
{

public class frmOrderManagement : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabPage Purchase;

	private TabPage Sale;

	private Panel panel2;

	internal GridEX grdPurchaseOrders;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private Panel panel3;

	private Label label1;

	internal GridEX grdSaleOrders;

	private CtrlGrdBar ctrlGrdBar2;

	private TabControl tabControl;

	public frmOrderManagement(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		ctrlGrdBar1.mGridChooseFielder.Visible = false;
		ctrlGrdBar1.mGridSaveLayouts.Visible = false;
		ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
		ctrlGrdBar2.mGridChooseFielder.Visible = false;
		ctrlGrdBar2.mGridSaveLayouts.Visible = false;
		ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
	}

	private void frmOrderManagement_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = Dashboard.SPU_Invetnory_OrderManagementDashBoards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = DateTime.Now
			});
			if (dt.Rows.Count > 0)
			{
				if (!dt.Columns.Contains("SortNo"))
				{
					dt.Columns.Add("SortNo");
					dt.Columns.Add("CardCount");
					dt.Columns.Add("TranType");
					dt.Columns.Add("ActivityDescription", typeof(double));
					dt.Columns.Add("CategoryDescription", typeof(double));
					dt.Columns.Add("ItemName", typeof(double));
					dt.Columns.Add("OrderBalanceQty", typeof(double));
					dt.Columns.Add("OrderBalanceWeight", typeof(double));
					dt.Columns.Add("StockBalanceQty", typeof(double));
					dt.Columns.Add("StockBalanceWeight", typeof(double));
					dt.Columns.Add("AvgRate40Kg", typeof(double));
					dt.Columns.Add("OrderCount", typeof(double));
					dt.Columns.Add("OrderAmount", typeof(double));
				}
				EnumerableRowCollection<DataRow> purchaseRows = from r in dt.AsEnumerable()
					where r.Field<string>("TranType") == "Purchase"
					select r;
				if (purchaseRows.Any())
				{
					grdPurchaseOrders.DataSource = purchaseRows.CopyToDataTable();
					GridSettingPurchase();
				}
				else
				{
					grdPurchaseOrders.DataSource = null;
				}
				EnumerableRowCollection<DataRow> saleRows = from r in dt.AsEnumerable()
					where r.Field<string>("TranType") == "Sales"
					select r;
				if (saleRows.Any())
				{
					grdSaleOrders.DataSource = saleRows.CopyToDataTable();
					GridSettingSale();
				}
				else
				{
					grdSaleOrders.DataSource = null;
				}
			}
			else
			{
				grdPurchaseOrders.DataSource = null;
				grdSaleOrders.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPurchase()
	{
		try
		{
			grdPurchaseOrders.RootTable.ColumnSets["TranType"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["CardCount"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["SortNo"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["ActivityDescription"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["CategoryDescription"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["OrderCount"].Visible = false;
			grdPurchaseOrders.RootTable.ColumnSets["AvgRate40Kg"].Visible = false;
			grdPurchaseOrders.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grdPurchaseOrders.RootTable.Columns["OrderBalanceQty"].Width = Constants.ExportConstants.QtyConstant;
			grdPurchaseOrders.RootTable.Columns["OrderBalalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdPurchaseOrders.RootTable.Columns["StockBalanceQty"].Width = Constants.ExportConstants.QtyConstant;
			grdPurchaseOrders.RootTable.Columns["StockBalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdPurchaseOrders.RootTable.Columns["OrderAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdPurchaseOrders.RootTable.Columns["OrderBalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseOrders.RootTable.Columns["OrderBalanceQty"].TextAlignment = (TextAlignment)3;
			grdPurchaseOrders.RootTable.Columns["OrderBalanceQty"].FormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["OrderBalanceQty"].TotalFormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["StockBalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseOrders.RootTable.Columns["StockBalanceQty"].TextAlignment = (TextAlignment)3;
			grdPurchaseOrders.RootTable.Columns["StockBalanceQty"].FormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["StockBalanceQty"].TotalFormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["OrderBalalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseOrders.RootTable.Columns["OrderBalalanceWeight"].TextAlignment = (TextAlignment)3;
			grdPurchaseOrders.RootTable.Columns["OrderBalalanceWeight"].FormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["OrderBalalanceWeight"].TotalFormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["StockBalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseOrders.RootTable.Columns["StockBalanceWeight"].TextAlignment = (TextAlignment)3;
			grdPurchaseOrders.RootTable.Columns["StockBalanceWeight"].FormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["StockBalanceWeight"].TotalFormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseOrders.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdPurchaseOrders.RootTable.Columns["OrderAmount"].FormatString = "#,##0.###";
			grdPurchaseOrders.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingSale()
	{
		try
		{
			grdSaleOrders.RootTable.ColumnSets["TranType"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["CardCount"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["SortNo"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["ActivityDescription"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["CategoryDescription"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["OrderCount"].Visible = false;
			grdSaleOrders.RootTable.ColumnSets["AvgRate40Kg"].Visible = false;
			grdSaleOrders.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grdSaleOrders.RootTable.Columns["OrderBalanceQty"].Width = Constants.ExportConstants.QtyConstant;
			grdSaleOrders.RootTable.Columns["OrderBalalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdSaleOrders.RootTable.Columns["StockBalanceQty"].Width = Constants.ExportConstants.QtyConstant;
			grdSaleOrders.RootTable.Columns["StockBalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdSaleOrders.RootTable.Columns["OrderAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdSaleOrders.RootTable.Columns["OrderBalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrders.RootTable.Columns["OrderBalanceQty"].TextAlignment = (TextAlignment)3;
			grdSaleOrders.RootTable.Columns["OrderBalanceQty"].FormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["OrderBalanceQty"].TotalFormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["StockBalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrders.RootTable.Columns["StockBalanceQty"].TextAlignment = (TextAlignment)3;
			grdSaleOrders.RootTable.Columns["StockBalanceQty"].FormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["StockBalanceQty"].TotalFormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["OrderBalalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrders.RootTable.Columns["OrderBalalanceWeight"].TextAlignment = (TextAlignment)3;
			grdSaleOrders.RootTable.Columns["OrderBalalanceWeight"].FormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["OrderBalalanceWeight"].TotalFormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["StockBalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrders.RootTable.Columns["StockBalanceWeight"].TextAlignment = (TextAlignment)3;
			grdSaleOrders.RootTable.Columns["StockBalanceWeight"].FormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["StockBalanceWeight"].TotalFormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrders.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdSaleOrders.RootTable.Columns["OrderAmount"].FormatString = "#,##0.###";
			grdSaleOrders.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmOrderManagement_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		GridEXLayout grdPurchaseOrders_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Dashboard.frmOrderManagement));
		GridEXLayout grdSaleOrders_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl = new System.Windows.Forms.TabControl();
		this.Purchase = new System.Windows.Forms.TabPage();
		this.grdPurchaseOrders = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.Sale = new System.Windows.Forms.TabPage();
		this.grdSaleOrders = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl.SuspendLayout();
		this.Purchase.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOrders).BeginInit();
		this.panel2.SuspendLayout();
		this.Sale.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrders).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.tabControl);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1370, 661);
		this.panel1.TabIndex = 1;
		this.tabControl.Controls.Add(this.Purchase);
		this.tabControl.Controls.Add(this.Sale);
		this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl.Location = new System.Drawing.Point(0, 0);
		this.tabControl.Name = "tabControl";
		this.tabControl.SelectedIndex = 0;
		this.tabControl.Size = new System.Drawing.Size(1370, 661);
		this.tabControl.TabIndex = 0;
		this.Purchase.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseOrders);
		this.Purchase.Controls.Add(this.panel2);
		this.Purchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Purchase.Location = new System.Drawing.Point(4, 25);
		this.Purchase.Name = "Purchase";
		this.Purchase.Padding = new System.Windows.Forms.Padding(3);
		this.Purchase.Size = new System.Drawing.Size(1362, 632);
		this.Purchase.TabIndex = 0;
		this.Purchase.Text = "Purchase";
		this.Purchase.UseVisualStyleBackColor = true;
		this.grdPurchaseOrders.AllowEdit = (InheritableBoolean)2;
		this.grdPurchaseOrders.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPurchaseOrders_DesignTimeLayout.LayoutString = resources.GetString("grdPurchaseOrders_DesignTimeLayout.LayoutString");
		this.grdPurchaseOrders.DesignTimeLayout = grdPurchaseOrders_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPurchaseOrders.DynamicFiltering = true;
		this.grdPurchaseOrders.FilterMode = (FilterMode)2;
		this.grdPurchaseOrders.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseOrders.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).Name = "grdPurchaseOrders";
		((ScrollableBase)this.grdPurchaseOrders).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).Size = new System.Drawing.Size(1356, 599);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrders).TabIndex = 27;
		this.grdPurchaseOrders.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseOrders.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseOrders).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.ForeColor = System.Drawing.Color.Teal;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1356, 27);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Purchase Orders";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.BackColor = System.Drawing.Color.Transparent;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1317, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPurchaseOrders;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 24);
		this.ctrlGrdBar1.TabIndex = 0;
		this.Sale.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOrders);
		this.Sale.Controls.Add(this.panel3);
		this.Sale.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Sale.Location = new System.Drawing.Point(4, 25);
		this.Sale.Name = "Sale";
		this.Sale.Padding = new System.Windows.Forms.Padding(3);
		this.Sale.Size = new System.Drawing.Size(1376, 632);
		this.Sale.TabIndex = 1;
		this.Sale.Text = "Sale";
		this.Sale.UseVisualStyleBackColor = true;
		this.grdSaleOrders.AllowEdit = (InheritableBoolean)2;
		this.grdSaleOrders.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdSaleOrders_DesignTimeLayout.LayoutString = resources.GetString("grdSaleOrders_DesignTimeLayout.LayoutString");
		this.grdSaleOrders.DesignTimeLayout = grdSaleOrders_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSaleOrders.DynamicFiltering = true;
		this.grdSaleOrders.FilterMode = (FilterMode)2;
		this.grdSaleOrders.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOrders.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).Name = "grdSaleOrders";
		((ScrollableBase)this.grdSaleOrders).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).Size = new System.Drawing.Size(1370, 600);
		((System.Windows.Forms.Control)(object)this.grdSaleOrders).TabIndex = 28;
		this.grdSaleOrders.TotalRow = (InheritableBoolean)1;
		this.grdSaleOrders.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOrders).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1370, 26);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1331, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdSaleOrders;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 24);
		this.ctrlGrdBar2.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(103, 19);
		this.label1.TabIndex = 3;
		this.label1.Text = "Sale Orders";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 661);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmOrderManagement";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmOrderManagement_FormClosed);
		base.Load += new System.EventHandler(frmOrderManagement_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl.ResumeLayout(false);
		this.Purchase.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOrders).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.Sale.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrders).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
