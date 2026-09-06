using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
namespace Architecture.WinApp.StoreManagement
{

public class LoadStockForAdjustment : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtStock = new DataTable();

	public int FlagForTrascation;

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btnSearch;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Button btnReset;

	private RadioButton rdStockTransaction;

	private RadioButton rdStockEvaludation;

	public LoadStockForAdjustment(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GetStockEvaludationDataForAdjustment()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dt = InvStockAdjustment.GetStockEvaludationDataForAdjustment(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Convert.ToDateTime(FromDate.Value),
				ToDate = Convert.ToDateTime(Todate.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetStockTransactionsDataForAdjustment()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dt = InvStockAdjustment.GetStockTransactionsDataForAdjustment(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Convert.ToDateTime(FromDate.Value),
				ToDate = Convert.ToDateTime(Todate.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtStock.Rows.Add(0);
		Hide();
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["ItemUomId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["InvPackingTypeId"].Visible = false;
			if (rdStockTransaction.Checked)
			{
				grd.RootTable.Columns["InAmount"].Visible = false;
				grd.RootTable.Columns["OutAmount"].Visible = false;
				grd.RootTable.Columns["BalAmount"].Visible = false;
				grd.RootTable.Columns["AvgRate"].Visible = false;
				grd.RootTable.Columns["AccountId"].Visible = false;
				grd.RootTable.Columns["AccountTitle"].Visible = false;
			}
			grd.RootTable.Columns["InQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OutQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OutWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OutAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["InQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OutQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OutWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OutAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InQty"].FormatString = "#,#";
			grd.RootTable.Columns["OutQty"].FormatString = "#,#";
			grd.RootTable.Columns["BalQty"].FormatString = "#,#";
			grd.RootTable.Columns["InWeight"].FormatString = "#,#";
			grd.RootTable.Columns["OutWeight"].FormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].FormatString = "#,#";
			grd.RootTable.Columns["InAmount"].FormatString = "#,#";
			grd.RootTable.Columns["OutAmount"].FormatString = "#,#";
			grd.RootTable.Columns["BalAmount"].FormatString = "#,#";
			grd.RootTable.Columns["AvgRate"].FormatString = "#,#";
			grd.RootTable.Columns["InQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["OutQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["InWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["OutWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["InAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["OutAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["AvgRate"].TotalFormatString = "#,#";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (rdStockEvaludation.Checked)
			{
				FlagForTrascation = 1;
			}
			else
			{
				FlagForTrascation = 2;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				dtStock.Rows.Add(0, Conversion.ToInt(r.Cells["WarehouseId"].Value), r.Cells["WareHouseCode"].Value.ToString(), Conversion.ToInt(r.Cells["JobLotId"].Value), r.Cells["JobLotCode"].Value.ToString(), r.Cells["CropYearId"].Value, r.Cells["CropBatch"].Value.ToString(), Conversion.ToInt(r.Cells["ItemId"].Value), r.Cells["ItemName"].Value.ToString(), Conversion.ToInt(r.Cells["InvPackingTypeId"].Value), r.Cells["PackingType"].Value.ToString(), Conversion.ToInt(r.Cells["ItemUomId"].Value), Conversion.ToString(r.Cells["PackUom"].Value), Conversion.ToDouble(r.Cells["BalQty"].Value), Conversion.ToDouble(r.Cells["BalWeight"].Value), Conversion.ToDouble(r.Cells["AvgRate"].Value), 0, 0, Conversion.ToDouble(r.Cells["BalAmount"].Value), Conversion.ToInt(r.Cells["AccountId"].Value), r.Cells["AccountTitle"].Value.ToString(), "");
			}
			Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			dtStock.Rows.Clear();
			grd.DataSource = null;
			rdStockEvaludation.Enabled = true;
			rdStockTransaction.Enabled = true;
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
			dtStock.Columns.Add("Id");
			dtStock.Columns.Add("WareHouseId");
			dtStock.Columns.Add("WareHouse");
			dtStock.Columns.Add("JobLotId");
			dtStock.Columns.Add("JobLot");
			dtStock.Columns.Add("CropYearId");
			dtStock.Columns.Add("CropYear");
			dtStock.Columns.Add("ItemId");
			dtStock.Columns.Add("Item");
			dtStock.Columns.Add("PackingTypeId");
			dtStock.Columns.Add("PackingType");
			dtStock.Columns.Add("ItemUOMId");
			dtStock.Columns.Add("ItemUOM");
			dtStock.Columns.Add("ItemQty", typeof(double));
			dtStock.Columns.Add("Weight", typeof(double));
			dtStock.Columns.Add("ItemRate", typeof(double));
			dtStock.Columns.Add("RateUomId");
			dtStock.Columns.Add("RateUom");
			dtStock.Columns.Add("ItemAmount", typeof(double));
			dtStock.Columns.Add("AccountId");
			dtStock.Columns.Add("AccountTitle");
			dtStock.Columns.Add("Comments");
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmLoadSupplySchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDepartmentRequest_FormClosed(object sender, FormClosedEventArgs e)
	{
		dtStock = null;
		Hide();
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			if (rdStockEvaludation.Checked)
			{
				GetStockEvaludationDataForAdjustment();
			}
			else
			{
				GetStockTransactionsDataForAdjustment();
			}
			if (grd.GetRows().Length != 0)
			{
				rdStockEvaludation.Enabled = false;
				rdStockTransaction.Enabled = false;
			}
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btnSearch = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.rdStockEvaludation = new System.Windows.Forms.RadioButton();
		this.rdStockTransaction = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(983, 42);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(983, 42);
		this.panelheading.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 9);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(236, 24);
		this.label23.TabIndex = 24;
		this.label23.Text = "Stock Load For Adjustment";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(934, 2);
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
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(983, 496);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.rdStockTransaction);
		this.panel2.Controls.Add(this.rdStockEvaludation);
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btnSearch);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 42);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(983, 59);
		this.panel2.TabIndex = 30;
		this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Location = new System.Drawing.Point(374, 27);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(52, 21);
		this.btnReset.TabIndex = 6;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(317, 27);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(54, 21);
		this.btnLoadOnInvoice.TabIndex = 5;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = true;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSearch.Location = new System.Drawing.Point(254, 27);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(61, 21);
		this.btnSearch.TabIndex = 4;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = true;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(134, 27);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(118, 21);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(15, 27);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(113, 21);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(137, 6);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(50, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 101);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(983, 496);
		this.panel3.TabIndex = 31;
		this.rdStockEvaludation.AutoSize = true;
		this.rdStockEvaludation.Checked = true;
		this.rdStockEvaludation.Location = new System.Drawing.Point(470, 7);
		this.rdStockEvaludation.Name = "rdStockEvaludation";
		this.rdStockEvaludation.Size = new System.Drawing.Size(137, 17);
		this.rdStockEvaludation.TabIndex = 13;
		this.rdStockEvaludation.TabStop = true;
		this.rdStockEvaludation.Text = "Stock From Evaluations";
		this.rdStockEvaludation.UseVisualStyleBackColor = true;
		this.rdStockTransaction.AutoSize = true;
		this.rdStockTransaction.Location = new System.Drawing.Point(470, 30);
		this.rdStockTransaction.Name = "rdStockTransaction";
		this.rdStockTransaction.Size = new System.Drawing.Size(143, 17);
		this.rdStockTransaction.TabIndex = 14;
		this.rdStockTransaction.Text = "Stock From Transactions";
		this.rdStockTransaction.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(983, 597);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "LoadStockForAdjustment";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(LoadDepartmentRequest_FormClosed);
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmLoadSupplySchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
