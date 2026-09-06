using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FIFO;

public class SaleInvoiceStockRateUpdate : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataSet ds = new DataSet();

	private DataTable dtgrdMain = new DataTable();

	private DataTable dtgrdDetail = new DataTable();

	private Label lblLoader;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Label label23;

	private DateTimePicker datFromDate;

	private Label label3;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	internal GridEX grdDetail;

	private Panel panel20;

	private Label label4;

	private Panel panel7;

	private ToolStripButton btnInsertVoucherAndStock;

	private DateTimePicker datToDate;

	private Label label1;

	private RadioButton RadExport;

	private RadioButton RadSale;

	private Panel pnlsettled;

	private RadioButton rdUnSettled;

	private RadioButton rdSettled;

	public SaleInvoiceStockRateUpdate(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		InitializeLoaderLabel();
		UserAccount = user;
	}

	private void SaleInvoiceStockAndVouchersAutoRun_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			dtgrdMain.Columns.Add("Id", typeof(int));
			dtgrdMain.Columns.Add("DocumentTypeId", typeof(int));
			dtgrdMain.Columns.Add("DocumentType", typeof(string));
			dtgrdMain.Columns.Add("DocNo", typeof(int));
			dtgrdMain.Columns.Add("DocDate", typeof(DateTime));
			dtgrdMain.Columns.Add("CustomerName", typeof(string));
			dtgrdMain.Columns.Add("ManualBillNo", typeof(string));
			dtgrdMain.Columns.Add("Qty", typeof(double));
			dtgrdMain.Columns.Add("Weight", typeof(double));
			dtgrdMain.Columns.Add("ItemAmount", typeof(double));
			dtgrdMain.Columns.Add("FcyCode", typeof(string));
			dtgrdMain.Columns.Add("ExchangeRate", typeof(double));
			dtgrdMain.Columns.Add("FcyAmount", typeof(double));
			dtgrdMain.Columns.Add("BillAmount", typeof(double));
			dtgrdMain.Columns.Add("ApprovedStatus", typeof(string));
			dtgrdMain.Columns.Add("RemarksHeader", typeof(string));
			dtgrdMain.Columns.Add("EntryDate", typeof(DateTime));
			dtgrdMain.Columns.Add("EntryUser", typeof(string));
			dtgrdDetail.Columns.Add("Id", typeof(int));
			dtgrdDetail.Columns.Add("MainId", typeof(int));
			dtgrdDetail.Columns.Add("ItemName", typeof(string));
			dtgrdDetail.Columns.Add("PackUom", typeof(string));
			dtgrdDetail.Columns.Add("JobLot", typeof(string));
			dtgrdDetail.Columns.Add("WareHouse", typeof(string));
			dtgrdDetail.Columns.Add("ItemQty", typeof(double));
			dtgrdDetail.Columns.Add("BillWeight", typeof(double));
			dtgrdDetail.Columns.Add("StockWeight", typeof(double));
			dtgrdDetail.Columns.Add("ItemRate", typeof(double));
			dtgrdDetail.Columns.Add("RateUom", typeof(string));
			dtgrdDetail.Columns.Add("Amount", typeof(double));
			dtgrdDetail.Columns.Add("CgsRate", typeof(double));
			dtgrdDetail.Columns.Add("CgsRatePerKg", typeof(double));
			dtgrdDetail.Columns.Add("CgsAmount", typeof(double));
			dtgrdDetail.Columns.Add("Freight", typeof(double));
			dtgrdDetail.Columns.Add("Expense", typeof(double));
			dtgrdDetail.Columns.Add("Commission", typeof(double));
			dtgrdDetail.Columns.Add("NetAmount", typeof(double));
			dtgrdDetail.Columns.Add("JobStatus", typeof(string));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeLoaderLabel()
	{
		lblLoader = new Label();
		lblLoader.Text = "Loading...";
		lblLoader.AutoSize = true;
		lblLoader.Font = new Font("Arial", 14f, FontStyle.Bold);
		lblLoader.ForeColor = Color.Red;
		lblLoader.Visible = false;
		base.Controls.Add(lblLoader);
	}

	private void CenterLoaderLabel()
	{
		lblLoader.Left = (base.ClientSize.Width - lblLoader.Width) / 2;
		lblLoader.Top = (base.ClientSize.Height - lblLoader.Height) / 2;
	}

	private void SaleInvoiceStockAndVouchersAutoRun_Resize(object sender, EventArgs e)
	{
		if (lblLoader.Visible)
		{
			CenterLoaderLabel();
		}
	}

	private async void btnInsertVoucherAndStock_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Are you sure to update stock values?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			base.Enabled = false;
			CenterLoaderLabel();
			lblLoader.Visible = true;
			lblLoader.BringToFront();
			if (grd.GetCheckedRows().Count() == 0)
			{
				base.Enabled = true;
				lblLoader.Visible = false;
				MessageBox.Show("Check the row first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			List<InvoiceInfo> selectedInvoices = new List<InvoiceInfo>();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (!rdSettled.Checked)
				{
					string ColumnName = (RadSale.Checked ? "InvSaleInvoiceId" : "ExportVoucherId");
					if ((from x in DetailRecordsFilterByInvoiceId(id, ColumnName).AsEnumerable()
						where x.Field<string>("JobStatus") == "UnSettled"
						select x).Count() > 0)
					{
						base.Enabled = true;
						lblLoader.Visible = false;
						MessageBox.Show("Some job order settlement is pending please check...");
						return;
					}
				}
				selectedInvoices.Add(new InvoiceInfo
				{
					Id = id,
					DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ActionId = (rdSettled.Checked ? 1 : 0)
				});
			}
			InvSaleInvoice.StockRateUpdateFromJobOrder(selectedInvoices);
			base.Enabled = true;
			lblLoader.Visible = false;
			MessageBox.Show("Stocks rate have been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			base.Enabled = true;
			lblLoader.Visible = false;
			MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		getSaleDataForVoucherAndStockEffects();
	}

	private void reset()
	{
		try
		{
			datFromDate.Focus();
			grd.DataSource = null;
			grdDetail.DataSource = null;
			pnlsettled.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void frmSaleOrderRegisterRetail_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			getSaleDataForVoucherAndStockEffects();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getSaleDataForVoucherAndStockEffects()
	{
		try
		{
			pnlsettled.Enabled = true;
			ds = new DataSet();
			if (RadSale.Checked)
			{
				ds = InvSaleInvoice.getSaleDataForCgsUpdate(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, Conversion.ToDateTime((object)datFromDate.Value), Conversion.ToDateTime((object)datToDate.Value), rdSettled.Checked ? 1 : 0);
			}
			else
			{
				ds = InvSaleInvoice.getExportDataForCgsUpdate(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, Conversion.ToDateTime((object)datFromDate.Value), Conversion.ToDateTime((object)datToDate.Value), rdSettled.Checked ? 1 : 0);
			}
			if (ds.Tables.Count == 0)
			{
				grd.DataSource = null;
				grdDetail.DataSource = null;
				return;
			}
			DataTable invoiceTable = ds.Tables[0];
			if (invoiceTable.Rows.Count == 0)
			{
				grd.DataSource = null;
				grdDetail.DataSource = null;
				return;
			}
			dtgrdMain.Clear();
			foreach (DataRow dr in invoiceTable.Rows)
			{
				string ManualBillNo = (RadSale.Checked ? Conversion.ToString(dr["ManualBillNo"]) : "");
				double billAmount = (RadSale.Checked ? Conversion.ToDouble(dr["BillAmount"]) : 0.0);
				dtgrdMain.Rows.Add(Conversion.ToInt(dr["Id"]), Conversion.ToInt(dr["DocumentTypeId"]), Conversion.ToString(dr["DocumentTypeDescription"]), Conversion.ToInt(dr["DocNo"]), Conversion.ToDateTime(dr["DocDate"]), Conversion.ToString(dr["CustomerName"]), ManualBillNo, Conversion.ToDouble(dr["Qty"]), Conversion.ToDouble(dr["Weight"]), Conversion.ToDouble(dr["Amount"]), Conversion.ToString(dr["FcyCode"]), Conversion.ToDouble(dr["ExchangeRate"]), Conversion.ToDouble(dr["FcyAmount"]), billAmount, Conversion.ToString(dr["ApprovedStatus"]), Conversion.ToString(dr["RemarksHeader"]), Conversion.ToDateTime(dr["EntryDate"]), Conversion.ToString(dr["EntryUser"]));
			}
			_ = RadSale.Checked;
			grd.DataSource = dtgrdMain;
			grd.RetrieveStructure();
			GridSetting();
			pnlsettled.Enabled = false;
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 2);
			grd.RootTable.Columns["BillAmount"].Visible = RadSale.Checked;
			grd.RootTable.Columns["ManualBillNo"].Visible = RadSale.Checked;
			grd.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant + 10;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable DetailRecordsFilterByInvoiceId(int Id, string ColumnName)
	{
		try
		{
			DataTable detailTable = ds.Tables[1];
			EnumerableRowCollection<DataRow> filteredRows = from row in detailTable.AsEnumerable()
				where row.Field<int>(ColumnName) == Id
				select row;
			return filteredRows.Any() ? filteredRows.CopyToDataTable() : detailTable.Clone();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdsaleorder_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				string ColumnName = (RadSale.Checked ? "InvSaleInvoiceId" : "ExportVoucherId");
				DataTable dt = DetailRecordsFilterByInvoiceId(Conversion.ToInt(r.Cells["Id"].Value), ColumnName);
				GetDetailGrd(dt);
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrd(DataTable dt2)
	{
		try
		{
			dtgrdDetail.Clear();
			foreach (DataRow dr in dt2.Rows)
			{
				int MainId = (RadSale.Checked ? Conversion.ToInt(dr["InvSaleInvoiceId"]) : Conversion.ToInt(dr["ExportVoucherId"]));
				double FreightAmount = (RadSale.Checked ? Conversion.ToDouble(dr["FreightAmount"]) : 0.0);
				double ExpenseAmount = (RadSale.Checked ? Conversion.ToDouble(dr["ExpenseAmount"]) : 0.0);
				double CommissionAmount = (RadSale.Checked ? Conversion.ToDouble(dr["CommissionAmount"]) : 0.0);
				double ItemNetAmount = (RadSale.Checked ? Conversion.ToDouble(dr["ItemNetAmount"]) : 0.0);
				dtgrdDetail.Rows.Add(dr["Id"], MainId, dr["ItemName"], dr["PackUom"], dr["JobLotCode"], dr["WareHouseName"], dr["ItemQty"], dr["NetBillWeight"], dr["NetStockWeight"], dr["ItemRate"], dr["RateUom"], dr["ItemAmount"], dr["CgsRate"], dr["CgsRatePerKg"], dr["CgsAmount"], FreightAmount, ExpenseAmount, CommissionAmount, ItemNetAmount, dr["JobStatus"]);
			}
			grdDetail.DataSource = dtgrdDetail;
			grdDetail.RetrieveStructure();
			GridSettingDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingDetail()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["MainId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 2);
			grdDetail.RootTable.Columns["Freight"].Visible = RadSale.Checked;
			grdDetail.RootTable.Columns["Expense"].Visible = RadSale.Checked;
			grdDetail.RootTable.Columns["Commission"].Visible = RadSale.Checked;
			grdDetail.RootTable.Columns["NetAmount"].Visible = RadSale.Checked;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["CgsRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["CgsRatePerKg"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["CgsAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["NetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["JobStatus"].Width = Constants.InventoryConstants.StatusConstant;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			throw new Exception(ex.Message);
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FIFO.SaleInvoiceStockRateUpdate));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.RadExport = new System.Windows.Forms.RadioButton();
		this.RadSale = new System.Windows.Forms.RadioButton();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnInsertVoucherAndStock = new System.Windows.Forms.ToolStripButton();
		this.pnlsettled = new System.Windows.Forms.Panel();
		this.rdUnSettled = new System.Windows.Forms.RadioButton();
		this.rdSettled = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.pnlsettled.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1174, 726);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 93);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1174, 633);
		this.panel5.TabIndex = 3;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel20);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 280);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1174, 353);
		this.panel6.TabIndex = 1;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 28);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1174, 325);
		this.panel7.TabIndex = 3;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		this.grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		this.grdDetail.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1174, 325);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1174, 28);
		this.panel20.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(122, 19);
		this.label4.TabIndex = 64;
		this.label4.Text = "Invoice Detail";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1174, 280);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grdsaleorder_SelectionChanged);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.pnlsettled);
		this.panel4.Controls.Add(this.RadExport);
		this.panel4.Controls.Add(this.RadSale);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1174, 38);
		this.panel4.TabIndex = 0;
		this.RadExport.AutoSize = true;
		this.RadExport.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.RadExport.Location = new System.Drawing.Point(587, 10);
		this.RadExport.Name = "RadExport";
		this.RadExport.Size = new System.Drawing.Size(131, 20);
		this.RadExport.TabIndex = 4;
		this.RadExport.Text = "Export Invoice";
		this.RadExport.UseVisualStyleBackColor = true;
		this.RadSale.AutoSize = true;
		this.RadSale.Checked = true;
		this.RadSale.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.RadSale.Location = new System.Drawing.Point(467, 10);
		this.RadSale.Name = "RadSale";
		this.RadSale.Size = new System.Drawing.Size(114, 20);
		this.RadSale.TabIndex = 3;
		this.RadSale.TabStop = true;
		this.RadSale.Text = "Sale Invoice";
		this.RadSale.UseVisualStyleBackColor = true;
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(257, 9);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(133, 23);
		this.datToDate.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(214, 13);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "ToDate";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(393, 10);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 21);
		this.btnshow.TabIndex = 2;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(75, 9);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(133, 23);
		this.datFromDate.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 13);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "FromDate";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1174, 25);
		this.panel3.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1134, 1);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 4;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(164, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Invoice Settlement";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1174, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnInsertVoucherAndStock });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1174, 30);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 27);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnInsertVoucherAndStock.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnInsertVoucherAndStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnInsertVoucherAndStock.Name = "btnInsertVoucherAndStock";
		this.btnInsertVoucherAndStock.Size = new System.Drawing.Size(78, 27);
		this.btnInsertVoucherAndStock.Text = "Update";
		this.btnInsertVoucherAndStock.Click += new System.EventHandler(btnInsertVoucherAndStock_Click);
		this.pnlsettled.Controls.Add(this.rdUnSettled);
		this.pnlsettled.Controls.Add(this.rdSettled);
		this.pnlsettled.Location = new System.Drawing.Point(730, 1);
		this.pnlsettled.Name = "pnlsettled";
		this.pnlsettled.Size = new System.Drawing.Size(217, 33);
		this.pnlsettled.TabIndex = 132;
		this.rdUnSettled.AutoSize = true;
		this.rdUnSettled.Checked = true;
		this.rdUnSettled.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.rdUnSettled.Location = new System.Drawing.Point(104, 10);
		this.rdUnSettled.Name = "rdUnSettled";
		this.rdUnSettled.Size = new System.Drawing.Size(100, 20);
		this.rdUnSettled.TabIndex = 134;
		this.rdUnSettled.TabStop = true;
		this.rdUnSettled.Text = "Un Settled";
		this.rdUnSettled.UseVisualStyleBackColor = true;
		this.rdSettled.AutoSize = true;
		this.rdSettled.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.rdSettled.Location = new System.Drawing.Point(9, 10);
		this.rdSettled.Name = "rdSettled";
		this.rdSettled.Size = new System.Drawing.Size(77, 20);
		this.rdSettled.TabIndex = 133;
		this.rdSettled.Text = "Settled";
		this.rdSettled.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1174, 726);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "SaleInvoiceStockRateUpdate";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSaleOrderRegisterRetail_FormClosed);
		base.Load += new System.EventHandler(SaleInvoiceStockAndVouchersAutoRun_Load);
		base.Resize += new System.EventHandler(SaleInvoiceStockAndVouchersAutoRun_Resize);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.pnlsettled.ResumeLayout(false);
		this.pnlsettled.PerformLayout();
		base.ResumeLayout(false);
	}
}
