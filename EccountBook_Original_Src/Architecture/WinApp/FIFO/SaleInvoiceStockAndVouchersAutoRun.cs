using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Sale;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FIFO;

public class SaleInvoiceStockAndVouchersAutoRun : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataSet ds = new DataSet();

	private DataTable invoiceTable = new DataTable();

	private DataTable detailTable = new DataTable();

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

	private DateTimePicker ToDate;

	private Label label3;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	internal GridEX grdSaleOrderDetail;

	private Panel panel20;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label4;

	private Panel panel7;

	private ToolStripButton btnInsertVoucherAndStock;

	public SaleInvoiceStockAndVouchersAutoRun(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		InitializeLoaderLabel();
		UserAccount = user;
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

	private void reset()
	{
		try
		{
			ToDate.Focus();
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
			ds = new DataSet();
			invoiceTable.Rows.Clear();
			detailTable.Rows.Clear();
			ds = InvSaleInvoice.getSaleDataForVoucherAndStockEffects(UserAccount.OrganizationId, UserAccount.CompanyId, 99, clsGlobalVariables.ActiveYr.Id, Conversion.ToDateTime((object)ToDate.Value));
			if (ds.Tables.Count > 0)
			{
				invoiceTable = ds.Tables[0];
				detailTable = ds.Tables[1];
				if (invoiceTable.Rows.Count > 0)
				{
					DataTable dtHeader = new DataTable();
					dtHeader.Columns.Add("Id", typeof(int));
					dtHeader.Columns.Add("DocNo", typeof(int));
					dtHeader.Columns.Add("DocDate", typeof(DateTime));
					dtHeader.Columns.Add("CustomerName", typeof(string));
					dtHeader.Columns.Add("ManualBillNo", typeof(string));
					dtHeader.Columns.Add("PaymentTerm", typeof(string));
					dtHeader.Columns.Add("DueDays", typeof(int));
					dtHeader.Columns.Add("DueDate", typeof(DateTime));
					dtHeader.Columns.Add("Qty", typeof(double));
					dtHeader.Columns.Add("Weight", typeof(double));
					dtHeader.Columns.Add("ItemAmount", typeof(double));
					dtHeader.Columns.Add("BillAmount", typeof(double));
					dtHeader.Columns.Add("RemarksHeader", typeof(string));
					for (int i = 0; i < invoiceTable.Rows.Count; i++)
					{
						dtHeader.Rows.Add(Conversion.ToInt(invoiceTable.Rows[i]["Id"]), Conversion.ToInt(invoiceTable.Rows[i]["DocNo"]), Conversion.ToDateTime(invoiceTable.Rows[i]["DocDate"]), Conversion.ToString(invoiceTable.Rows[i]["CustomerName"]), Conversion.ToString(invoiceTable.Rows[i]["ManualBillNo"]), Conversion.ToString(invoiceTable.Rows[i]["TermsDescription"]), Conversion.ToInt(invoiceTable.Rows[i]["DueDays"]), Conversion.ToDateTime(invoiceTable.Rows[i]["DueDate"]), Conversion.ToDouble(invoiceTable.Rows[i]["Qty"]), Conversion.ToDouble(invoiceTable.Rows[i]["Weight"]), Conversion.ToDouble(invoiceTable.Rows[i]["Amount"]), Conversion.ToDouble(invoiceTable.Rows[i]["BillAmount"]), Conversion.ToString(invoiceTable.Rows[i]["RemarksHeader"]));
					}
					grdsaleorder.DataSource = dtHeader;
					grdsaleorder.RetrieveStructure();
					GridSetting();
				}
				else
				{
					grdsaleorder.DataSource = null;
					grdSaleOrderDetail.DataSource = null;
				}
			}
			else
			{
				grdsaleorder.DataSource = null;
				grdSaleOrderDetail.DataSource = null;
			}
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
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdsaleorder.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yy";
			grdsaleorder.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdsaleorder.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdsaleorder.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdsaleorder.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdsaleorder.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdsaleorder.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["Qty"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Qty"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["Weight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Weight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetDetailGrd(DataTable dt2)
	{
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("InvSaleInvoiceId", typeof(int));
			dtdetail.Columns.Add("ItemName", typeof(string));
			dtdetail.Columns.Add("PackUom", typeof(string));
			dtdetail.Columns.Add("JobLot", typeof(string));
			dtdetail.Columns.Add("WareHouse", typeof(string));
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("AdLsWeight", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUom", typeof(string));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("Freight", typeof(double));
			dtdetail.Columns.Add("Expense", typeof(double));
			dtdetail.Columns.Add("Commission", typeof(double));
			dtdetail.Columns.Add("NetAmount", typeof(double));
			for (int i = 0; i < dt2.Rows.Count; i++)
			{
				dtdetail.Rows.Add(dt2.Rows[i]["Id"], dt2.Rows[i]["InvSaleInvoiceId"], dt2.Rows[i]["ItemName"], dt2.Rows[i]["PackUom"], dt2.Rows[i]["JobLotCode"], dt2.Rows[i]["WareHouseName"], dt2.Rows[i]["ItemQty"], dt2.Rows[i]["GrossWeight"], dt2.Rows[i]["AdLsWeight"], dt2.Rows[i]["NetBillWeight"], dt2.Rows[i]["ItemRate"], dt2.Rows[i]["RateUom"], dt2.Rows[i]["ItemAmount"], dt2.Rows[i]["FreightAmount"], dt2.Rows[i]["ExpenseAmount"], dt2.Rows[i]["CommissionAmount"], dt2.Rows[i]["BillAmount"]);
			}
			grdSaleOrderDetail.DataSource = dtdetail;
			grdSaleOrderDetail.RetrieveStructure();
			GridSettingDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable DetailRecordsFilterByInvoiceId(int Id)
	{
		try
		{
			DataTable detailTable = ds.Tables[1];
			EnumerableRowCollection<DataRow> filteredRows = from row in detailTable.AsEnumerable()
				where row.Field<int>("InvSaleInvoiceId") == Id
				select row;
			return filteredRows.Any() ? filteredRows.CopyToDataTable() : detailTable.Clone();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingDetail()
	{
		try
		{
			grdSaleOrderDetail.RootTable.Columns["Id"].Visible = false;
			grdSaleOrderDetail.RootTable.Columns["InvSaleInvoiceId"].Visible = false;
			grdSaleOrderDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.StringConstant;
			grdSaleOrderDetail.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.StringConstant;
			grdSaleOrderDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdSaleOrderDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdSaleOrderDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderDetail.RootTable.Columns["AdLsWeight"].Width = Constants.InventoryConstants.AddLessConstant;
			grdSaleOrderDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdSaleOrderDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdSaleOrderDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdSaleOrderDetail.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdSaleOrderDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdSaleOrderDetail.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.StringConstant;
			grdSaleOrderDetail.RootTable.Columns["NetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdSaleOrderDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["ItemQty"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["ItemQty"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["GrossWeight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["GrossWeight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSaleOrderDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["NetBillWeight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdSaleOrderDetail.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdSaleOrderDetail.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSaleOrderDetail.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdsaleorder_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdsaleorder.CurrentRow != null && (int)grdsaleorder.CurrentRow.RowType == 0 && grdsaleorder.CurrentRow.Cells.Count > 0)
			{
				DataTable dt = DetailRecordsFilterByInvoiceId(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
				GetDetailGrd(dt);
			}
			else
			{
				grdSaleOrderDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSaleOrderDetail).Name);
			if (fs != null)
			{
				grdSaleOrderDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SaleInvoiceStockAndVouchersAutoRun_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar3.mGridChooseFielder.Visible = false;
			ctrlGrdBar3.mGridSaveLayouts.Visible = false;
			ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void btnInsertVoucherAndStock_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Are you sure to Insert Vouchers and Stocks?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			base.Enabled = false;
			CenterLoaderLabel();
			lblLoader.Visible = true;
			lblLoader.BringToFront();
			if (grdsaleorder.GetRows().Count() == 0)
			{
				base.Enabled = true;
				lblLoader.Visible = false;
				MessageBox.Show("Record not found for Vouchers and Stocks", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			await Task.Run(delegate
			{
				VouchesAndStockDataInsert(ds);
			});
			base.Enabled = true;
			lblLoader.Visible = false;
			MessageBox.Show("Vouchers and Stocks have been successfully inserted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			base.Enabled = true;
			lblLoader.Visible = false;
			MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		getSaleDataForVoucherAndStockEffects();
	}

	private void VouchesAndStockDataInsert(DataSet fullDataSet)
	{
		try
		{
			Dictionary<int, DataRow[]> detailData = GetFilteredDataByInvoiceId(detailTable, "InvSaleInvoiceId");
			List<InvSaleInvoice> invoiceList = Conversion.ConvertDataTableToList<InvSaleInvoice>(invoiceTable);
			int batchSize = 100;
			int batchCount = (int)Math.Ceiling((double)invoiceList.Count / (double)batchSize);
			for (int batchIndex = 0; batchIndex < batchCount; batchIndex++)
			{
				foreach (InvSaleInvoice obj in invoiceList.Skip(batchIndex * batchSize).Take(batchSize).ToList())
				{
					try
					{
						int invoiceId = obj.Id;
						if (detailData.TryGetValue(invoiceId, out var details))
						{
							List<InvSaleInvoiceDetail> detailList = Conversion.ConvertDataTableToList<InvSaleInvoiceDetail>(details.CopyToDataTable());
							obj.InvSaleInvoiceDetaillist = detailList;
						}
						InvSaleInvoice.AutoVoucherAndStockEffects(obj);
					}
					catch (Exception ex)
					{
						throw new Exception($"Error processing InvoiceNo {obj.DocNo}: {ex.Message}");
					}
				}
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	private Dictionary<int, DataRow[]> GetFilteredDataByInvoiceId(DataTable table, string columnName)
	{
		return (from row in table.AsEnumerable()
			group row by row.Field<int>(columnName)).ToDictionary((IGrouping<int, DataRow> group) => group.Key, (IGrouping<int, DataRow> group) => group.ToArray());
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
			frmSaleInvoiceDirect obj = new frmSaleInvoiceDirect(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmSaleInvoiceDirect").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(Id);
				}
				else
				{
					MessageBox.Show("You don't have right");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		GridEXLayout grdSaleOrderDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FIFO.SaleInvoiceStockAndVouchersAutoRun));
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdSaleOrderDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.grdsaleorder = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnInsertVoucherAndStock = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderDetail).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
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
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 111);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1174, 615);
		this.panel5.TabIndex = 3;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel20);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 280);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1174, 335);
		this.panel6.TabIndex = 1;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 28);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1174, 307);
		this.panel7.TabIndex = 3;
		this.grdSaleOrderDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdSaleOrderDetail).BackColor = System.Drawing.Color.White;
		this.grdSaleOrderDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdSaleOrderDetail_DesignTimeLayout.LayoutString = resources.GetString("grdSaleOrderDetail_DesignTimeLayout.LayoutString");
		this.grdSaleOrderDetail.DesignTimeLayout = grdSaleOrderDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSaleOrderDetail.FilterMode = (FilterMode)2;
		this.grdSaleOrderDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOrderDetail.GroupByBoxVisible = false;
		this.grdSaleOrderDetail.GroupTotals = (GroupTotals)2;
		this.grdSaleOrderDetail.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).Name = "grdSaleOrderDetail";
		((ScrollableBase)this.grdSaleOrderDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).Size = new System.Drawing.Size(1174, 307);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderDetail).TabIndex = 1;
		this.grdSaleOrderDetail.TotalRow = (InheritableBoolean)1;
		this.grdSaleOrderDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOrderDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar3);
		this.panel20.Controls.Add(this.label4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1174, 28);
		this.panel20.TabIndex = 2;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1133, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdSaleOrderDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 22);
		this.ctrlGrdBar3.TabIndex = 65;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(122, 19);
		this.label4.TabIndex = 64;
		this.label4.Text = "Invoice Detail";
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		this.grdsaleorder.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdsaleorder_DesignTimeLayout.LayoutString = resources.GetString("grdsaleorder_DesignTimeLayout.LayoutString");
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		this.grdsaleorder.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(1174, 280);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.SelectionChanged += new System.EventHandler(grdsaleorder_SelectionChanged);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.ToDate);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1174, 56);
		this.panel4.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(148, 27);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 21);
		this.btnshow.TabIndex = 11;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(10, 27);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(133, 21);
		this.ToDate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "AsOn Date";
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
		this.ctrlGrdBar2.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 4;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(384, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Sale Invoice Pending For Vouchers And Stocks";
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
		this.btnInsertVoucherAndStock.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnInsertVoucherAndStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnInsertVoucherAndStock.Name = "btnInsertVoucherAndStock";
		this.btnInsertVoucherAndStock.Size = new System.Drawing.Size(185, 27);
		this.btnInsertVoucherAndStock.Text = "Insert Voucher && Stock";
		this.btnInsertVoucherAndStock.Click += new System.EventHandler(btnInsertVoucherAndStock_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1174, 726);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "SaleInvoiceStockAndVouchersAutoRun";
		this.Text = "Sale Invoice Pending For Vouchers And Stocks";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSaleOrderRegisterRetail_FormClosed);
		base.Load += new System.EventHandler(SaleInvoiceStockAndVouchersAutoRun_Load);
		base.Resize += new System.EventHandler(SaleInvoiceStockAndVouchersAutoRun_Resize);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
