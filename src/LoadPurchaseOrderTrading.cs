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
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.PurchaseTrading
{

public class LoadPurchaseOrderTrading : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtPurchaseOrder = new DataTable();

	private PurchaseInvoiceTradePro TradePro;

	private List<InvWareHouse> lstwh = new List<InvWareHouse>();

	private DataTable dtloadPO = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private Label label4;

	private TextBox txtDocNo;

	internal GridEX grd;

	public LoadPurchaseOrderTrading(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void SupplierNameFilll()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingPOLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters purchaseOrder = new ReportsParameters();
			purchaseOrder.FromDate = Convert.ToDateTime(FromDate.Value);
			purchaseOrder.ToDate = Convert.ToDateTime(Todate.Value);
			if (Conversion.ToInt((object)PurchaseInvoiceTradePro.SupplierId) > 0)
			{
				purchaseOrder.SupplierCustomerId = Conversion.ToInt((object)PurchaseInvoiceTradePro.SupplierId);
			}
			if (txtDocNo.Text != string.Empty)
			{
				purchaseOrder.DocNo = Conversion.ToInt((object)txtDocNo.Text.Trim());
			}
			purchaseOrder.DocumentTypeId = 42;
			dtloadPO = InvPurchaseInvoice.PurchaseOrderLoadForPurchaseInvoice(purchaseOrder);
			if (dtloadPO.Rows.Count > 0)
			{
				grd.DataSource = dtloadPO;
				grd.RetrieveStructure();
				grdSettings();
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void WareHousefill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvWareHouse invWare = new InvWareHouse();
			lstwh = InvWareHouse.GetList(invWare);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtPurchaseOrder = null;
		Hide();
	}

	public void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["OrderItemUOMId"].Visible = false;
			grd.RootTable.Columns["OrderItemRateUOMId"].Visible = false;
			grd.RootTable.Columns["BrokerAgentName"].Visible = false;
			grd.RootTable.Columns["CommissionType"].Visible = false;
			grd.RootTable.Columns["CommRate"].Visible = false;
			grd.RootTable.Columns["UomScheduleIdCmRate"].Visible = false;
			grd.RootTable.Columns["CommAmount"].Visible = false;
			grd.RootTable.Columns["CommissionRemarks"].Visible = false;
			grd.RootTable.Columns["BrokerAgentSupCustId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["Crop"].Visible = false;
			grd.RootTable.Columns["RemarksHeader"].Visible = false;
			grd.RootTable.Columns["DeliveryTerm"].Visible = false;
			grd.RootTable.Columns["LabSampleNo"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grd.RootTable.Columns["ItemQty"].Width = 80;
			grd.RootTable.Columns["SupplierName"].Width = 200;
			grd.RootTable.Columns["ItemName"].Width = 250;
			grd.RootTable.Columns["OrderNo"].Width = 80;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingPOLoad();
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
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int SupplierCustomer = 0;
				int i = 0;
				dtPurchaseOrder.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					SupplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					if (SupplierCustomer != 0)
					{
						if (i == 0)
						{
							i = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString());
						}
						if (i != SupplierCustomer)
						{
							dtPurchaseOrder.Rows.Clear();
							MessageBox.Show("Sorry the OrderNo should be of same Supplier");
							break;
						}
						dtPurchaseOrder.Rows.Add(r.Cells["Id"].Value, r.Cells["OrderNo"].Value.ToString(), i = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString()), r.Cells["ItemId"].Value.ToString(), r.Cells["ItemName"].Value.ToString(), r.Cells["OrderItemUOMId"].Value.ToString(), r.Cells["UOMCodeItem"].Value.ToString(), r.Cells["ItemQty"].Value.ToString(), r.Cells["ItemRate"].Value.ToString(), r.Cells["OrderItemRateUOMId"].Value.ToString(), r.Cells["RateUOM"].Value.ToString(), r.Cells["Amount"].Value.ToString(), r.Cells["TaxPercent"].Value.ToString(), r.Cells["TaxAmount"].Value.ToString(), r.Cells["CommissionType"].Value.ToString(), r.Cells["CommRate"].Value.ToString(), r.Cells["UomScheduleIdCmRate"].Value.ToString(), r.Cells["CommAmount"].Value.ToString(), r.Cells["CommissionRemarks"].Value.ToString(), r.Cells["BrokerAgentSupCustId"].Value.ToString(), r.Cells["JobLotId"].Value.ToString(), r.Cells["JobLotDescription"].Value.ToString(), r.Cells["RemarksHeader"].Value.ToString(), r.Cells["DeliveryTerm"].Value.ToString());
					}
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Chek the row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		PendingPOLoad();
		FromDate.Focus();
		((Control)(object)CmbSupplier).Text = string.Empty;
		txtDocNo.Text = string.Empty;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			WareHousefill();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtPurchaseOrder.Columns.Add("Id");
			dtPurchaseOrder.Columns.Add("OrderNo");
			dtPurchaseOrder.Columns.Add("SupplierCustomerId");
			dtPurchaseOrder.Columns.Add("ItemId");
			dtPurchaseOrder.Columns.Add("ItemName");
			dtPurchaseOrder.Columns.Add("OrderItemUOMId");
			dtPurchaseOrder.Columns.Add("UOMCodeItem");
			dtPurchaseOrder.Columns.Add("ItemQty", typeof(double));
			dtPurchaseOrder.Columns.Add("ItemRate");
			dtPurchaseOrder.Columns.Add("OrderItemRateUOMId");
			dtPurchaseOrder.Columns.Add("RateUOM", typeof(double));
			dtPurchaseOrder.Columns.Add("Amount", typeof(double));
			dtPurchaseOrder.Columns.Add("TaxPercent");
			dtPurchaseOrder.Columns.Add("TaxAmount");
			dtPurchaseOrder.Columns.Add("CommissionType");
			dtPurchaseOrder.Columns.Add("CommRate");
			dtPurchaseOrder.Columns.Add("UomScheduleIdCmRate");
			dtPurchaseOrder.Columns.Add("CommAmount");
			dtPurchaseOrder.Columns.Add("CommissionRemarks");
			dtPurchaseOrder.Columns.Add("BrokerAgentSupCustId");
			dtPurchaseOrder.Columns.Add("JobLotId");
			dtPurchaseOrder.Columns.Add("JobLotDescription");
			dtPurchaseOrder.Columns.Add("RemarksHeader");
			dtPurchaseOrder.Columns.Add("DeliveryTerm");
			SupplierNameFilll();
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
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.btnReset = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1164, 48);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(1164, 46);
		this.panelheading.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 9);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(191, 24);
		this.label23.TabIndex = 24;
		this.label23.Text = "Purchase Order Load";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.txtDocNo);
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 48);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1164, 88);
		this.panel2.TabIndex = 30;
		this.txtDocNo.Location = new System.Drawing.Point(373, 45);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(188, 20);
		this.txtDocNo.TabIndex = 3;
		this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Location = new System.Drawing.Point(705, 45);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 24);
		this.btnReset.TabIndex = 6;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(305, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "Doc No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 46);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Supplier Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(109, 42);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(187, 24);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(639, 45);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(64, 24);
		this.btnLoadOnInvoice.TabIndex = 5;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = true;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1106, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.btngrnlod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.Location = new System.Drawing.Point(572, 45);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(65, 24);
		this.btngrnlod.TabIndex = 4;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = true;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(373, 18);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(188, 21);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(109, 18);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(187, 21);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(305, 21);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(50, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(16, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 136);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1164, 461);
		this.panel3.TabIndex = 31;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1164, 461);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1164, 597);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "LoadPurchaseOrderTrading";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}

}
