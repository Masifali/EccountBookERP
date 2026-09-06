using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
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
namespace Architecture.WinApp.PackingMaterial_Store
{

public class LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts : Form
{
	private DropDownBind DDL = new DropDownBind();

	private int[] AryColWidth;

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtHistory = new DataTable();

	public DataTable DtLoader = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Button btnReset;

	private Label label10;

	private TextBox txtSelectedQty;

	public LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PendingInventoryTransactionsForIssuanceLoad()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			DataTable dtlst = InvStoreSendReceipt.GetPackingMaterialFilledToEmpty(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Convert.ToDateTime(FromDate.Value),
				ToDate = Convert.ToDateTime(Todate.Value)
			});
			if (dtlst.Rows.Count > 0)
			{
				if (dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("RefDocumentTypeId", typeof(int));
					dtHistory.Columns.Add("RefDocIdNo", typeof(int));
					dtHistory.Columns.Add("RefDocSubIdNo", typeof(int));
					dtHistory.Columns.Add("DocDate", typeof(DateTime));
					dtHistory.Columns.Add("DocNo", typeof(int));
					dtHistory.Columns.Add("SenderWareHouseId", typeof(int));
					dtHistory.Columns.Add("SenderWareHouse", typeof(string));
					dtHistory.Columns.Add("PackingTypeId", typeof(int));
					dtHistory.Columns.Add("PackingType");
					dtHistory.Columns.Add("PackUom");
					dtHistory.Columns.Add("IssueQty", typeof(double));
					dtHistory.Columns.Add("ReceivedQty", typeof(double));
					dtHistory.Columns.Add("BalQty", typeof(double));
				}
				for (int i = 0; i < dtlst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dtlst.Rows[i]["DocumentTypeId"], dtlst.Rows[i]["PrdHeaderId"], dtlst.Rows[i]["PrdDetailId"], Conversion.ToDateTime(dtlst.Rows[i]["DocDate"]).ToShortDateString(), dtlst.Rows[i]["DocCode"], dtlst.Rows[i]["SenderWareHouseId"], dtlst.Rows[i]["SenderWareHouse"], dtlst.Rows[i]["PackingtypeId"], dtlst.Rows[i]["PackTypeCode"], dtlst.Rows[i]["UOMCode"], dtlst.Rows[i]["IssueQty"], dtlst.Rows[i]["ReceiptQty"], dtlst.Rows[i]["BalanceQty"]);
				}
				grd.DataSource = dtHistory;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["SenderWareHouseId"].Visible = false;
			grd.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["IssueQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["IssueQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["IssueQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ReceivedQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ReceivedQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ReceivedQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ReceivedQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnHeaderClick(object sender, ColumnActionEventArgs e)
	{
		SelectedWeightCalculation();
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		SelectedWeightCalculation();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		grd.ClearStructure();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		DtLoader = null;
		Hide();
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingInventoryTransactionsForIssuanceLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			int getCount = grd.GetCheckedRows().Count();
			if (getCount > 0)
			{
				if (getCount > 0)
				{
					int WareHouseId = 0;
					int WID = 0;
					DtLoader = dtHistory.Clone();
					DtLoader.Rows.Clear();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						WareHouseId = Conversion.ToInt(r.Cells["SenderWarehouseId"].Value);
						if (WareHouseId != 0)
						{
							if (WID == 0)
							{
								WID = Conversion.ToInt(r.Cells["SenderWarehouseId"].Value);
							}
							if (WID != WareHouseId)
							{
								DtLoader.Rows.Clear();
								MessageBox.Show("Sorry Check Rows Which Have Same Warehouse");
								return;
							}
							DataRow dr = dtHistory.Rows[r.RowIndex];
							DtLoader.Rows.Add(dr.ItemArray);
							WID = Conversion.ToInt(r.Cells["SenderWarehouseId"].Value);
						}
					}
					Hide();
					Hide();
				}
				else
				{
					MessageBox.Show("Chek the row first");
				}
			}
			else
			{
				MessageBox.Show("Please Select Row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			PendingInventoryTransactionsForIssuanceLoad();
			FromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadavailableTransactionsForIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SelectedWeightCalculation()
	{
		try
		{
			double SelectedQty = 0.0;
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				SelectedQty += Conversion.ToDouble(item.Cells["BalQty"].Value);
			}
			txtSelectedQty.Text = Math.Round(SelectedQty).ToString("#,##0.###");
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.txtSelectedQty = new System.Windows.Forms.TextBox();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1117, 32);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1068, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1117, 584);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.ColumnHeaderClick += new ColumnActionEventHandler(grd_ColumnHeaderClick);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(602, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Loader For Packing Material And Consumeable Store Item Send Receipts";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.txtSelectedQty);
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 32);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1117, 58);
		this.panel2.TabIndex = 30;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(478, 32);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(103, 15);
		this.label10.TabIndex = 42;
		this.label10.Text = "Total Selected Qty";
		this.txtSelectedQty.BackColor = System.Drawing.Color.White;
		this.txtSelectedQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSelectedQty.Location = new System.Drawing.Point(606, 28);
		this.txtSelectedQty.Name = "txtSelectedQty";
		this.txtSelectedQty.ReadOnly = true;
		this.txtSelectedQty.Size = new System.Drawing.Size(120, 23);
		this.txtSelectedQty.TabIndex = 13;
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(374, 28);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(71, 24);
		this.btnReset.TabIndex = 11;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(313, 28);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 24);
		this.btnLoadOnInvoice.TabIndex = 10;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(239, 28);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(74, 24);
		this.btngrnlod.TabIndex = 9;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(124, 28);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(111, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(10, 28);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(111, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(125, 9);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 90);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1117, 584);
		this.panel3.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1117, 674);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts";
		this.Text = "Loader For Packing Material And Consumeable Store Item Send Receipts";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadavailableTransactionsForIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
