// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class RequiredRawMaterialAgainstSaleOrders : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private DataTable dtHistory = new DataTable();

	private DataTable RawItemdt = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Button btnshow;

	private DateTimePicker datToDate;

	private Label label33;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX GridHistory;

	private Panel panel2;

	private Panel panel5;

	private Label label1;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label2;

	internal GridEX GridRawItems;

	private Label label3;

	private TextBox txtqtyUom;

	private Button txtSummPrint;

	private Button btnRegister;

	public RequiredRawMaterialAgainstSaleOrders(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void RequiredRawMaterialAgainstSaleOrders_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1240, 800);
			RawItemdt.Columns.Add("ItemId");
			RawItemdt.Columns.Add("ItemName");
			RawItemdt.Columns.Add("RequiredGrossWt", typeof(double));
			RawItemdt.Columns.Add("RequiredGrossQty", typeof(double));
			RawItemdt.Columns.Add("AvailableStockWeight", typeof(double));
			RawItemdt.Columns.Add("NetRequiredWeight", typeof(double));
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			DataSet ds = SaleOrderReports.MRPAgainstSalesOrdersRice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = datToDate.Value
			});
			if (ds.Tables[0].Rows.Count > 0)
			{
				dtHistory = ds.Tables[0];
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("ItemId");
				dt2.Columns.Add("OrderItem");
				dt2.Columns.Add("OrderUom");
				dt2.Columns.Add("OrderQty", typeof(double));
				dt2.Columns.Add("OrderWeight", typeof(double));
				dt2.Columns.Add("RawItem");
				dt2.Columns.Add("RawPackUom");
				dt2.Columns.Add("RequiredWt", typeof(double));
				dt2.Columns.Add("Equivalent", typeof(double));
				dt2.Columns.Add("RawPrct", typeof(double));
				for (int j = 0; j < dtHistory.Rows.Count; j++)
				{
					dt2.Rows.Add(dtHistory.Rows[j]["BomHId"], dtHistory.Rows[j]["BomHItemId"], dtHistory.Rows[j]["BomHItemName"], dtHistory.Rows[j]["BomHItemUomName"], dtHistory.Rows[j]["OrderQty"], dtHistory.Rows[j]["OrderWeight"], dtHistory.Rows[j]["ItemNameRm"], dtHistory.Rows[j]["UomRm"], dtHistory.Rows[j]["RequiredWeight"], dtHistory.Rows[j]["Equivalent"], dtHistory.Rows[j]["PercRm"]);
				}
				GridHistory.DataSource = dt2;
				GridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				GridHistory.ClearStructure();
			}
			if (ds.Tables[1].Rows.Count > 0)
			{
				DataTable dt = ds.Tables[1];
				RawItemdt.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					double PackUom = Conversion.ToDouble((object)txtqtyUom.Text);
					RawItemdt.Rows.Add(dt.Rows[i]["sItemId"], dt.Rows[i]["ItemNameRm"], Conversion.ToDouble(dt.Rows[i]["RequiredWeight"]), Conversion.ToDouble(dt.Rows[i]["RequiredWeight"]) / PackUom, 0, 0);
				}
				GridRawItems.DataSource = RawItemdt;
				GridRawItems.RetrieveStructure();
				UpdateBal();
				GridRawSettings();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridRawSettings()
	{
		try
		{
			GridRawItems.RootTable.Columns["ItemId"].Visible = false;
			GridRawItems.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GridRawItems.ColumnAutoResize = false;
			GridRawItems.RootTable.Columns["RequiredGrossWt"].AggregateFunction = (AggregateFunction)2;
			GridRawItems.RootTable.Columns["RequiredGrossWt"].TextAlignment = (TextAlignment)3;
			GridRawItems.RootTable.Columns["RequiredGrossWt"].FormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["RequiredGrossWt"].TotalFormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["RequiredGrossQty"].AggregateFunction = (AggregateFunction)2;
			GridRawItems.RootTable.Columns["RequiredGrossQty"].TextAlignment = (TextAlignment)3;
			GridRawItems.RootTable.Columns["RequiredGrossQty"].FormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["RequiredGrossQty"].TotalFormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["NetRequiredWeight"].AggregateFunction = (AggregateFunction)2;
			GridRawItems.RootTable.Columns["NetRequiredWeight"].TextAlignment = (TextAlignment)3;
			GridRawItems.RootTable.Columns["NetRequiredWeight"].FormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["NetRequiredWeight"].TotalFormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["AvailableStockWeight"].AggregateFunction = (AggregateFunction)2;
			GridRawItems.RootTable.Columns["AvailableStockWeight"].TextAlignment = (TextAlignment)3;
			GridRawItems.RootTable.Columns["AvailableStockWeight"].FormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["AvailableStockWeight"].TotalFormatString = "#,##0.##;(#,##0.##)";
			GridRawItems.RootTable.Columns["ItemName"].Position = 0;
			GridRawItems.RootTable.Columns["RequiredGrossQty"].Position = 1;
			GridRawItems.RootTable.Columns["RequiredGrossWt"].Position = 2;
			GridRawItems.RootTable.Columns["AvailableStockWeight"].Position = 3;
			GridRawItems.RootTable.Columns["NetRequiredWeight"].Position = 4;
			CommonServices.GridAutoAdjustment(AryColWidth, GridRawItems);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			GridHistory.RootTable.Columns["Id"].Visible = false;
			GridHistory.RootTable.Columns["ItemId"].Visible = false;
			GridHistory.RootTable.Columns["RawPackUom"].Visible = false;
			GridHistory.RootTable.Columns["Equivalent"].Visible = false;
			GridHistory.RootTable.Columns["OrderItem"].ColumnType = (ColumnType)5;
			GridHistory.RootTable.Groups.Add("OrderItem");
			GridHistory.RootTable.GroupTotals = (GroupTotals)2;
			GridHistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			GridHistory.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			GridHistory.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			GridHistory.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			GridHistory.RootTable.Columns["RequiredWt"].AggregateFunction = (AggregateFunction)2;
			GridHistory.RootTable.Columns["RequiredWt"].TextAlignment = (TextAlignment)3;
			GridHistory.RootTable.Columns["RequiredWt"].FormatString = "#,##0.##";
			GridHistory.RootTable.Columns["RequiredWt"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridHistory).Name);
			if (fs != null)
			{
				GridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtHistory, "464-RequiredRawMaterialRegisterTwo.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridRawItems).Name);
			if (fs != null)
			{
				GridRawItems.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = GridHistory.CurrentRow;
			if (e.Column.Key == "OrderItem")
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridRawItems_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = GridRawItems.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (ItemId > 0)
				{
					frmStockReport frmStockReport = new frmStockReport(UserAccount);
					frmStockReport.cmbItem.Value = ItemId;
					frmStockReport.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtqtyUom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqtyUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double packUom = Conversion.ToDouble((object)txtqtyUom.Text);
			if (packUom > 0.0 && packUom <= 100.0)
			{
				GridEXRow[] rows = GridRawItems.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double qty = Conversion.ToDouble(obj.Cells["RequiredGrossWt"].Value) / packUom;
					obj.Cells["RequiredGrossQty"].Value = qty.ToString();
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateBal()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		List<string> itemIds = new List<string>();
		GridEXRow[] rows = GridRawItems.GetRows();
		for (int i = 0; i < rows.Length; i++)
		{
			string itemId = Conversion.ToString(rows[i].Cells["ItemId"].Value);
			itemIds.Add(itemId);
		}
		DataTable updatedData = SaleOrderReports.GetAvailableStockFromTransactions(new ReportsParameters
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			ItemIds = string.Join(",", itemIds),
			DocDate = Conversion.ToDateTime((object)datToDate.Value)
		});
		rows = GridRawItems.GetRows();
		foreach (GridEXRow row in rows)
		{
			row.BeginEdit();
			DataRow[] matchingRows = updatedData.Select("ItemId = " + Conversion.ToInt(row.Cells["ItemId"].Value));
			if (matchingRows.Length != 0)
			{
				DataRow matchingRow = matchingRows[0];
				row.Cells["AvailableStockWeight"].Value = matchingRow["BalWeight"].ToString();
				double RequiredGrossWt = Convert.ToDouble(row.Cells["RequiredGrossWt"].Value);
				double balStockWt = Convert.ToDouble(row.Cells["AvailableStockWeight"].Value);
				row.Cells["NetRequiredWeight"].Value = (RequiredGrossWt - balStockWt).ToString();
			}
			row.EndEdit();
		}
	}

	private void txtqtyUom_Leave(object sender, EventArgs e)
	{
		try
		{
			double packUom = Conversion.ToDouble((object)txtqtyUom.Text);
			if (packUom > 0.0 && packUom <= 100.0)
			{
				txtqtyUom_TextChanged(null, null);
				return;
			}
			MessageBox.Show("You Can not Enter PackUom greater than 100 and Less Than Zero...", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtqtyUom.Text = "100";
			txtqtyUom_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSummPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RawItemdt == null || RawItemdt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(RawItemdt, "463-RequiredRawMaterialRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RequiredRawMaterialAgainstSaleOrders_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		GridEXLayout GridRawItems_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.txtSummPrint = new System.Windows.Forms.Button();
		this.GridRawItems = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnRegister = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridHistory = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtqtyUom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridRawItems).BeginInit();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1241, 591);
		this.panel1.TabIndex = 0;
		this.panel6.Controls.Add(this.txtSummPrint);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridRawItems);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(768, 95);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(473, 496);
		this.panel6.TabIndex = 143;
		this.txtSummPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtSummPrint.BackColor = System.Drawing.Color.Olive;
		this.txtSummPrint.FlatAppearance.BorderSize = 0;
		this.txtSummPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.txtSummPrint.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummPrint.ForeColor = System.Drawing.Color.White;
		this.txtSummPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtSummPrint.Location = new System.Drawing.Point(359, 0);
		this.txtSummPrint.Margin = new System.Windows.Forms.Padding(2);
		this.txtSummPrint.Name = "txtSummPrint";
		this.txtSummPrint.Size = new System.Drawing.Size(58, 22);
		this.txtSummPrint.TabIndex = 121;
		this.txtSummPrint.Text = "Print";
		this.txtSummPrint.UseVisualStyleBackColor = false;
		this.txtSummPrint.Click += new System.EventHandler(txtSummPrint_Click);
		this.GridRawItems.AllowEdit = (InheritableBoolean)2;
		this.GridRawItems.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridRawItems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridRawItems.DesignTimeLayout = GridRawItems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridRawItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridRawItems.DynamicFiltering = true;
		this.GridRawItems.FilterMode = (FilterMode)2;
		this.GridRawItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridRawItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridRawItems.GroupByBoxVisible = false;
		this.GridRawItems.GroupTotals = (GroupTotals)2;
		this.GridRawItems.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridRawItems).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.GridRawItems).Name = "GridRawItems";
		((ScrollableBase)this.GridRawItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridRawItems).Size = new System.Drawing.Size(473, 473);
		((System.Windows.Forms.Control)(object)this.GridRawItems).TabIndex = 144;
		this.GridRawItems.TotalRow = (InheritableBoolean)1;
		this.GridRawItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridRawItems).VisualStyle = (VisualStyle)4;
		this.GridRawItems.LinkClicked += new ColumnActionEventHandler(GridRawItems_LinkClicked);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(421, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GridRawItems;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar2.TabIndex = 143;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(0, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(473, 23);
		this.label2.TabIndex = 1;
		this.label2.Text = "This Grid Shows How Much Raw Item Required ";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.btnRegister);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 95);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(768, 496);
		this.panel2.TabIndex = 142;
		this.btnRegister.BackColor = System.Drawing.Color.Olive;
		this.btnRegister.FlatAppearance.BorderSize = 0;
		this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRegister.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.ForeColor = System.Drawing.Color.White;
		this.btnRegister.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnRegister.Location = new System.Drawing.Point(662, 1);
		this.btnRegister.Margin = new System.Windows.Forms.Padding(2);
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(58, 22);
		this.btnRegister.TabIndex = 145;
		this.btnRegister.Text = "Print";
		this.btnRegister.UseVisualStyleBackColor = false;
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 23);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(768, 473);
		this.panel5.TabIndex = 142;
		this.GridHistory.AllowEdit = (InheritableBoolean)2;
		this.GridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridHistory.DesignTimeLayout = GridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridHistory.DynamicFiltering = true;
		this.GridHistory.FilterMode = (FilterMode)2;
		this.GridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridHistory).Name = "GridHistory";
		((ScrollableBase)this.GridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridHistory).Size = new System.Drawing.Size(768, 473);
		((System.Windows.Forms.Control)(object)this.GridHistory).TabIndex = 141;
		this.GridHistory.TotalRow = (InheritableBoolean)1;
		this.GridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridHistory).VisualStyle = (VisualStyle)4;
		this.GridHistory.LinkClicked += new ColumnActionEventHandler(GridHistory_LinkClicked);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(726, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar1.TabIndex = 140;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(768, 23);
		this.label1.TabIndex = 0;
		this.label1.Text = "This Grid Shows Brand Wise Required Material";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtqtyUom);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1241, 36);
		this.panel4.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(770, 14);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(61, 15);
		this.label3.TabIndex = 120;
		this.label3.Text = "Pack Uom";
		this.txtqtyUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqtyUom.Location = new System.Drawing.Point(837, 10);
		this.txtqtyUom.Name = "txtqtyUom";
		this.txtqtyUom.Size = new System.Drawing.Size(100, 23);
		this.txtqtyUom.TabIndex = 3;
		this.txtqtyUom.Text = "100";
		this.txtqtyUom.TextChanged += new System.EventHandler(txtqtyUom_TextChanged);
		this.txtqtyUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqtyUom_KeyPress);
		this.txtqtyUom.Leave += new System.EventHandler(txtqtyUom_Leave);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(203, 5);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 2;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(67, 7);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(131, 22);
		this.datToDate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(11, 11);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 0;
		this.label33.Text = "To Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1241, 32);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(363, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Required Raw Material Against Sale Orders";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1241, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&Reset";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1241, 591);
		base.Controls.Add(this.panel1);
		base.Name = "RequiredRawMaterialAgainstSaleOrders";
		this.Text = "RequiredRawMaterialAgainstSaleOrders";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RequiredRawMaterialAgainstSaleOrders_FormClosing);
		base.Load += new System.EventHandler(RequiredRawMaterialAgainstSaleOrders_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridRawItems).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
