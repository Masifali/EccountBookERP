using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Inventory_Reports;

public class frmSaleInvoiceRegisterTrading : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInvRegister = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	internal GridEX DataGridHistory;

	private Label label5;

	private DateTimePicker txtTodate;

	private Label label4;

	private DateTimePicker txtFromDate;

	private Label label2;

	private Button btnshow;

	internal UltraCombo cmbSupplier;

	internal UltraCombo cmbItem;

	private Label label7;

	private TextBox txtDocNoFrom;

	private TextBox txtDocNoTo;

	private Label label1;

	private Label label3;

	public frmSaleInvoiceRegisterTrading(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceRegister_Load(object sender, EventArgs e)
	{
		suppliercustomer();
		ItemFill();
		txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		SaleInvoiceRegisterBind();
	}

	private void suppliercustomer()
	{
		try
		{
			DataTable dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbSupplier, "Id", "CompanyName", "Suplier ", ZeroIndex: false);
				((UltraGridBase)cmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 201;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		SaleInvoiceRegisterBind();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		RefreHistory();
		SaleInvoiceRegisterBind();
	}

	public void RefreHistory()
	{
		try
		{
			txtDocNoFrom.Text = string.Empty;
			txtDocNoTo.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			txtDocNoFrom.Focus();
			txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			txtTodate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void SaleInvoiceRegisterBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters invoice = new ReportsParameters();
			invoice.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			invoice.ItemId = Conversion.ToInt(cmbItem.Value);
			invoice.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			invoice.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			invoice.FromDate = txtFromDate.Value;
			invoice.ToDate = txtTodate.Value;
			dtInvRegister = SalesTrading.GetInvoiceRegister(invoice);
			if (dtInvRegister.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderNo");
				dt.Columns.Add("OrderDate");
				dt.Columns.Add("DocNo");
				dt.Columns.Add("DocDate");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemDesc");
				dt.Columns.Add("UOM");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Tax %", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("TotalAmount", typeof(double));
				for (int i = 0; i < dtInvRegister.Rows.Count; i++)
				{
					dt.Rows.Add(dtInvRegister.Rows[i]["OrderNo"], Conversion.ToDateTime(dtInvRegister.Rows[i]["OrderDate"]).ToString("dd-MMM-yyyy"), dtInvRegister.Rows[i]["DocNo"], Conversion.ToDateTime(dtInvRegister.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtInvRegister.Rows[i]["CompanyName"], dtInvRegister.Rows[i]["ItemCode"], dtInvRegister.Rows[i]["ItemName"], dtInvRegister.Rows[i]["UOMCode"], dtInvRegister.Rows[i]["ItemQty"], dtInvRegister.Rows[i]["ItemRate"], Math.Round(Conversion.ToDouble(dtInvRegister.Rows[i]["ItemAmount"]), 2), dtInvRegister.Rows[i]["TaxPercent"], Math.Round(Conversion.ToDouble(dtInvRegister.Rows[i]["TaxAmount"]), 2), Math.Round(Conversion.ToDouble(dtInvRegister.Rows[i]["ItemAmount"]) + Conversion.ToDouble(dtInvRegister.Rows[i]["TaxAmount"]), 2));
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["OrderDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ItemDesc"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["UOM"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Qty"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Rate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Amount"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Tax %"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["OrderDate"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["SupplierCustomer"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ItemDesc"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Tax %"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["TaxAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["OrderNo"].Width = 60;
			DataGridHistory.RootTable.Columns["OrderDate"].Width = 100;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 60;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 100;
			DataGridHistory.RootTable.Columns["SupplierCustomer"].Width = 300;
			DataGridHistory.RootTable.Columns["ItemCode"].Width = 60;
			DataGridHistory.RootTable.Columns["ItemDesc"].Width = 300;
			DataGridHistory.RootTable.Columns["UOM"].Width = 100;
			DataGridHistory.RootTable.Columns["Qty"].Width = 60;
			DataGridHistory.RootTable.Columns["Rate"].Width = 70;
			DataGridHistory.RootTable.Columns["Amount"].Width = 80;
			DataGridHistory.RootTable.Columns["Tax %"].Width = 50;
			DataGridHistory.RootTable.Columns["TaxAmount"].Width = 80;
			DataGridHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Tax %"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Qty"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Rate"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Tax %"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TaxAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalAmount"].FormatString = "0,0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ReportsParameters
			{
				SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text),
				ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text),
				FromDate = txtFromDate.Value,
				ToDate = txtTodate.Value
			};
			if (dtInvRegister == null || dtInvRegister.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtInvRegister, "226-RptInvSaleInvoiceRegister.rpt");
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtTodate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbSupplier = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1053, 604);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 105);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1053, 499);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1053, 499);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 48;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtTodate);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtFromDate);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.txtDocNoFrom);
		this.panel3.Controls.Add(this.txtDocNoTo);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1053, 76);
		this.panel3.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(304, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(66, 15);
		this.label5.TabIndex = 203;
		this.label5.Text = "From Date ";
		this.txtTodate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtTodate.Location = new System.Drawing.Point(370, 47);
		this.txtTodate.Name = "txtTodate";
		this.txtTodate.Size = new System.Drawing.Size(201, 23);
		this.txtTodate.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(14, 51);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(66, 15);
		this.label4.TabIndex = 201;
		this.label4.Text = "From Date ";
		this.txtFromDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(97, 47);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(201, 23);
		this.txtFromDate.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(576, 23);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 199;
		this.label2.Text = "Customer";
		this.btnshow.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.Location = new System.Drawing.Point(576, 47);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 23);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = true;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(640, 17);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 3;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(370, 17);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(304, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(31, 15);
		this.label7.TabIndex = 198;
		this.label7.Text = "Item";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(97, 19);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(62, 23);
		this.txtDocNoFrom.TabIndex = 0;
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(236, 19);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(62, 23);
		this.txtDocNoTo.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(14, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(77, 15);
		this.label1.TabIndex = 197;
		this.label1.Text = "Doc No From";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(165, 23);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(61, 15);
		this.label3.TabIndex = 196;
		this.label3.Text = "Doc No To";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1053, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton6, this.toolStripSeparator1, this.btnnew, this.toolStripSeparator, this.Print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1053, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Times New Roman", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(205, 24);
		this.toolStripButton6.Text = "Sale Invoice Register            ";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(79, 24);
		this.Print.Text = "226-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1053, 604);
		base.Controls.Add(this.panel1);
		base.Name = "frmSaleInvoiceRegisterTrading";
		this.Text = "Sale Invoice Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceRegister_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
