using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.FeedMill.Purchase;
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.FeedMill.Purchase;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Special_Rights.Configurations;
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
namespace Architecture.WinApp.FeedMill.Purchase
{

public class PurchaseInvoiceDirect : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private WaitForm WF = new WaitForm();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCrAccountForEmptyBag = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private DataTable dtEmptyBagsItem = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtAccountlstForFreight = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtwareh = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtjoblot = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private DataTable dtcrop = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private decimal SupplierGLCreditAmount;

	private decimal SupplierGLDebitAmount;

	private decimal SupplierCommissionAmount;

	private int InwardFreightAccountId;

	private decimal BaseRate;

	private int BaseCurrency;

	private int updateDetailIndex = -1;

	private int VoucherHeadId;

	public int RecId;

	private int ScreenId;

	private bool Approved;

	private bool WagesStatus;

	private int EmptyBagsWeightCutStatus;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool OrderExist;

	private bool HasMultiCurrencyFeature;

	public bool TaxPercntEditableOrNot;

	private DataTable dtTax = new DataTable();

	private LoadPurchaseOrderFeed LoadPO;

	private bool WagesActiveOrInActive;

	private bool FreightDebitToExpenses;

	private UserAccount UserAccount = new UserAccount();

	private List<InvPurchaseInvoiceDetail> lstRemoveDetailRecord = new List<InvPurchaseInvoiceDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private Label label7;

	private TextBox txtSupplierReference;

	private Label label6;

	private Label label12;

	private TextBox txtbillno;

	private GroupBox groupBox1;

	private TextBox txtEmptyBagsTotal;

	private Label label16;

	private TextBox txtEmptybagsUnit;

	private Label label15;

	private TextBox txtAmount;

	private Label label14;

	private Label label9;

	private Label label42;

	private Label label41;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtGrossWeight;

	private Label label39;

	private TextBox txtQty;

	private Label label38;

	private Label label35;

	private TextBox txtNetBillWeight;

	private Label label36;

	private TextBox txtlabanalysisno;

	private Label label37;

	private Label label32;

	private Label label33;

	private TextBox txtStockWeight;

	private Label label34;

	private TextBox txtAddLss;

	private Label label31;

	private Label label30;

	private Label label28;

	private Label label22;

	private DateTimePicker txtgpdate;

	internal UltraCombo comsupplier;

	internal UltraCombo comPackUOM;

	internal UltraCombo comjobLot;

	internal UltraCombo CmbCropYear;

	internal UltraCombo comPackingType;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	internal UltraCombo comproject;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	internal UltraCombo combcommAgent;

	private Label label20;

	private Label label26;

	private Label label11;

	private TextBox txtCommissionRemarks;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	private Label label24;

	private TextBox txtwtcut;

	private Label label46;

	private Label label47;

	private Label label48;

	private TextBox txtvehicleno;

	private Label label50;

	private TextBox txtratecuttotal;

	private Label label49;

	private TextBox txtratecut;

	private Button btnAdd;

	private TextBox txtgatepassno;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grd;

	private TextBox txtBillAmount;

	private Label label51;

	private TextBox txtWeightCutTotal;

	private Label label52;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CheckBox ChkBok;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combCommType;

	internal UltraCombo CmbDeliveryTerm;

	internal UltraCombo combcommUOM;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnRefresh;

	internal GridEX grdFreight;

	internal GridEX grdDetail;

	private CheckBox ChkPrintSlip;

	private ToolStripButton btnPurchaseOrderLoader;

	private TabControl tabControl2;

	private TabPage tabFreight;

	private TabPage tabOtherExpense;

	private TabPage tabSupplierAddLess;

	private TabPage tabEmptyBags;

	internal GridEX grdEmptyBags;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryAc;

	private Label label23;

	private Label label53;

	private Label label54;

	private Label label55;

	private TextBox txtBrokeryRate;

	private Label label56;

	private TextBox txtBrokeryAmount;

	private ToolStripButton btn225PartySlip;

	private NotifyIcon notifyIcon1;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel12;

	private Label label5;

	private Panel panel6;

	private Label label57;

	private Panel panel11;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label58;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnSpecialRights;

	private Panel panel18;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel7;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TabPage tabDetail;

	private Panel panel5;

	private Label label64;

	private Panel panel8;

	private Label label65;

	private Panel panel13;

	private Label label66;

	private Panel panel14;

	private Label label67;

	private Panel panel15;

	private Label label68;

	private TextBox txtremarks;

	private TextBox txtInvoiceWeight;

	private TextBox txtInvoiceQty;

	private Label label1;

	private Label label13;

	internal UltraCombo cmbCurrency;

	private Label labelFcy;

	private TextBox txtExchangeRate;

	private Label labelExcha;

	private TextBox txtFcyAmount;

	private Label labelFcyAmount;

	private TextBox txtBrokeryRemarks;

	private Label label72;

	private Label label73;

	internal UltraCombo cmbtaxtype;

	private Label label74;

	private Label label75;

	private TextBox txtTaxAmount;

	private TextBox txtTaxPercent;

	private Label label76;

	private TextBox txtTotalAmount;

	internal UltraCombo txtCity;

	private Label label77;

	private Label label78;

	private TextBox txtremarksDetail;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Label lblBalance;

	private Label label2;

	private Label label8;

	internal UltraCombo CmbTaxAccount;

	private TextBox txtFactoryWeight;

	private Label label27;

	private TextBox txtSupplierWeight;

	private Label label44;

	private TextBox txtDiffWeight;

	private Label label43;

	private Label lblDiff;

	private Label label3;

	internal UltraCombo CmbFreightDeduction;

	private TextBox txtFreightDeduction;

	private Label label45;

	public PurchaseInvoiceDirect(UserAccount user)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchaseInvoiceDirect_Load(object sender, EventArgs e)
	{
		try
		{
			WF.Show(this);
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1320, 750);
			ScreenId = CommonServices.GetScreenIdByName(base.Name);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnSlip.Enabled = formright.DoHavePrintRights;
			btn225PartySlip.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			ChkBok.Checked = formright.DoHavePrintRights;
			ChkPrintSlip.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			Configurations();
			SpecialRightsImplement();
			MultiCurrencyFeature();
			GetallTaxType();
			CurrencyFill();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxType");
			dtTax.Columns.Add("TaxPrcnt");
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			dtpacktype.Columns.Add("Id", typeof(int));
			dtpacktype.Columns.Add("PackTypeDesc");
			dtAccountlstForFreight.Columns.Add("Id");
			dtAccountlstForFreight.Columns.Add("SupplierCustomerId");
			dtAccountlstForFreight.Columns.Add("AccountTitle");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			AccountsFill();
			OtherItemsBind();
			CreditAccountForEmptyBag();
			Project();
			DocumentNo();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			TaxAccountFill();
			FreightDeductionAccountFill();
			bindWareHouse();
			item();
			cropyear();
			JobLot();
			PackingType();
			BinCity();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("CropYearId", typeof(int));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("PackUOMEquivalent", typeof(decimal));
			dtGrid.Columns.Add("ItemQty", typeof(decimal));
			dtGrid.Columns.Add("GrossWeight", typeof(decimal));
			dtGrid.Columns.Add("EmptyBags", typeof(decimal));
			dtGrid.Columns.Add("EmptyBagsTotal", typeof(decimal));
			dtGrid.Columns.Add("WeightCut", typeof(decimal));
			dtGrid.Columns.Add("WeightCutTotal", typeof(decimal));
			dtGrid.Columns.Add("AddLss", typeof(decimal));
			dtGrid.Columns.Add("NetBillWeight", typeof(decimal));
			dtGrid.Columns.Add("StockWeight", typeof(decimal));
			dtGrid.Columns.Add("Rate", typeof(decimal));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("RateUOMEquivalent", typeof(decimal));
			dtGrid.Columns.Add("RateCut", typeof(decimal));
			dtGrid.Columns.Add("RateCutTotal", typeof(decimal));
			dtGrid.Columns.Add("ItemAmount", typeof(decimal));
			dtGrid.Columns.Add("TaxId", typeof(int));
			dtGrid.Columns.Add("Tax%", typeof(decimal));
			dtGrid.Columns.Add("TaxAmount", typeof(decimal));
			dtGrid.Columns.Add("LabSampleNo");
			dtGrid.Columns.Add("GpDate");
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("Remarks");
			dtGrid.Columns.Add("Expense", typeof(decimal));
			dtGrid.Columns.Add("Freights", typeof(decimal));
			dtGrid.Columns.Add("Commission", typeof(decimal));
			dtGrid.Columns.Add("LessDeduction", typeof(decimal));
			dtGrid.Columns.Add("ItemNetAmount", typeof(decimal));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("PurchaseOrderId", typeof(int));
			dtInvExp.Columns.Add("PurchaseOrderSupplierExpId", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(decimal));
			dtInvExp.Columns.Add("Rate", typeof(decimal));
			dtInvExp.Columns.Add("Amount", typeof(decimal));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(decimal));
			dtGrdGL.Columns.Add("Qty", typeof(decimal));
			dtGrdGL.Columns.Add("Rate", typeof(decimal));
			dtGrdGL.Columns.Add("Debit", typeof(decimal));
			dtGrdGL.Columns.Add("Credit", typeof(decimal));
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtFreight.Columns.Add("Id", typeof(int));
			dtFreight.Columns.Add("OrderId", typeof(int));
			dtFreight.Columns.Add("OrderExpenseId", typeof(int));
			dtFreight.Columns.Add("TransporterId", typeof(int));
			dtFreight.Columns.Add("Percentage", typeof(decimal));
			dtFreight.Columns.Add("Qty", typeof(decimal));
			dtFreight.Columns.Add("Rate", typeof(decimal));
			dtFreight.Columns.Add("Credit", typeof(decimal));
			dtFreight.Columns.Add("Debit", typeof(decimal));
			dtFreight.Columns.Add("Remarks");
			dtFreight.Columns.Add("GlAccountId", typeof(int));
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			EmptyBagsTypeFill();
			EmptyBagsItemFill();
			dtEmptyBags.Columns.Add("PurchaseOrderId");
			dtEmptyBags.Columns.Add("Type");
			dtEmptyBags.Columns.Add("ItemId");
			dtEmptyBags.Columns.Add("ReceivedQty", typeof(decimal));
			dtEmptyBags.Columns.Add("PurchaseQty", typeof(decimal));
			dtEmptyBags.Columns.Add("Rate", typeof(decimal));
			dtEmptyBags.Columns.Add("Amount", typeof(decimal));
			dtEmptyBags.Columns.Add("Remarks");
			dtEmptyBags.Columns.Add("CreditAccountId");
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocDate.Focus();
			HistoryComboFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbDeliveryTerm).Enabled = true;
			WF.Close();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurrency.Value = id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				labelFcy.Visible = true;
				labelExcha.Visible = true;
				labelFcyAmount.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				labelFcy.Visible = false;
				labelExcha.Visible = false;
				labelFcyAmount.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || !(Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m))
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1859",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDecimal(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal amount = Conversion.ToDecimal(obj.Cells["ItemAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Configurations()
	{
		try
		{
			List<ConfigrationsAllocation> listf = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightInwardAc").ToList();
			if (listf.Count > 0)
			{
				int value = Conversion.ToInt((object)listf[0].ConfigKey.ToString());
				if (value > 0)
				{
					InwardFreightAccountId = value;
				}
			}
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnPurchaseInvoiceDirect"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 1104);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			string FREIGHTDEBIT = CommonServices.GetConfigurationByOrgCompandConfigDescription("DebitAmountChargetoExpenseAcFreightGridPurchase");
			if (FREIGHTDEBIT != string.Empty)
			{
				FreightDebitToExpenses = Conversion.ToBool((object)FREIGHTDEBIT);
			}
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				comWarehouse.Value = WareHouseId;
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (Conversion.ToInt(txtCity.Value) == 0)
				{
					txtCity.Value = CityId;
				}
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				if (Conversion.ToInt(comjobLot.Value) == 0)
				{
					comjobLot.Value = JobId;
				}
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				CmbCropYear.Value = CropYearId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = (BaseRate = Conversion.ToDecimal((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SpecialRightsImplement()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.SreenRightsUserWise.SpecialRightsByScreenIdAndUserId(new global::Architecture.Model.SreenRightsUserWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ScreenId = ScreenId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 1)
				{
					ChkPrintSlip.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 2)
				{
					ChkBok.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnSpecialRights_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialSreenRightsUserWise obj = new SpecialSreenRightsUserWise(UserAccount);
			obj.ScreenId = ScreenId;
			((Control)(object)obj.cmbScreenName).Enabled = false;
			obj.UserId = Conversion.ToInt((object)UserAccount.ID);
			if (UserAccount.RoleName != "Admin")
			{
				((Control)(object)obj.cmbUserName).Enabled = false;
			}
			obj.Show();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,16,17,18,19");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PreviousPurchaseTaxAc()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int PurchaseTaxAcId = InvPurchaseInvoice.GetLastPurchaseTaxAcId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (PurchaseTaxAcId > 0)
			{
				CmbTaxAccount.Value = PurchaseTaxAcId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetallTaxType()
	{
		try
		{
			dtTaxType.Rows.Clear();
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1104"
			};
			DataTable dtSupplier = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"Supplier",
						typeof(string)
					}
				}
			};
			DataTable dt = InvPurchaseInvoice.DropDownFillFromPurchaseInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Project()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Projects> lst = Projects.GetAll(new Projects
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count > 0)
			{
				DataTable dtproject = new DataTable();
				dtproject.Columns.Add("Id");
				dtproject.Columns.Add("ProjectName");
				for (int i = 0; i < lst.Count; i++)
				{
					dtproject.Rows.Add(lst[i].Id, lst[i].ProjectName);
				}
				if (dtproject.Rows.Count > 0)
				{
					DDL.BindDDL(dtproject, comproject, "Id", "ProjectName", "Project Name", ZeroIndex: false);
					((UltraGridBase)comproject).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvPurchaseInvoice.GenerateCode(new InvPurchaseInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1104,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int Id = Conversion.ToInt(comsupplier.Value);
			int Idr = Conversion.ToInt(combcommAgent.Value);
			int Idb = Conversion.ToInt(CmbBrokeryAc.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(1);
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "Commission Agent", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, CmbBrokeryAc, "Id", "CompanyName", "Broker", ZeroIndex: true);
				}
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "Commission Agent", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, CmbBrokeryAc, "Id", "CompanyName", "Broker", ZeroIndex: true);
				}
			}
			if (Id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comsupplier.Value = Id;
				}
				else
				{
					((Control)(object)comsupplier).Text = string.Empty;
				}
			}
			if (Idr > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idr))
				{
					combcommAgent.Value = Idr;
				}
				else
				{
					((Control)(object)combcommAgent).Text = string.Empty;
				}
			}
			if (Idb > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idb))
				{
					CmbBrokeryAc.Value = Idb;
				}
				else
				{
					((Control)(object)CmbBrokeryAc).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int Id = Conversion.ToInt(combpttrm.Value);
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", true);
			((UltraGridBase)combpttrm).Rows[2].Activate();
			if (Id > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					combpttrm.Value = Id;
				}
				else
				{
					((Control)(object)combpttrm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "ExFactory");
			dt.Rows.Add(2, "Factory");
			dt.Rows.Add(3, "ExFactory FactoryWeight");
			dt.Rows.Add(4, "Factory FactoryWeight");
			dt.Rows.Add(5, "Factory PartyWeight");
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, combCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			DDL.BindDDL(dt, CmbBrokeryType, "Id", "CommissionType", "BrokeryType", ZeroIndex: false);
			((UltraGridBase)combCommType).Rows[1].Activate();
			((UltraGridBase)CmbBrokeryType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, combcommUOM, "Id", "UOM", "UOM", ZeroIndex: false);
			DDL.BindDDL(dt, CmbBrokeryRateUom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)combcommUOM).Rows[0].Activate();
			((UltraGridBase)CmbBrokeryRateUom).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BinCity()
	{
		try
		{
			int Id = Conversion.ToInt(txtCity.Value);
			dtCity.Clear();
			dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtCity, txtCity, "Id", "CityName", "City Name ", true);
			if (Id > 0)
			{
				if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					txtCity.Value = Id;
				}
				else
				{
					((Control)(object)txtCity).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindWareHouse()
	{
		try
		{
			int Id = Conversion.ToInt(comWarehouse.Value);
			dtwareh.Clear();
			dtwareh = CommonServices.getActiveWareHouse();
			if (dtwareh.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtwareh, comWarehouse, "Id", "WareHouseName", "Ware House ", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtwareh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comWarehouse.Value = Id;
				}
				else
				{
					((Control)(object)comWarehouse).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void item()
	{
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			((Control)(object)comItem).Text = string.Empty;
			dtitem.Rows.Clear();
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						comItem.Value = Id;
					}
					else
					{
						((Control)(object)comItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)comItem).Text = string.Empty;
				((UltraGridBase)comItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cropyear()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCropYear.Value);
			dtcrop.Clear();
			dtcrop = CommonServices.CropYearGetAllService();
			if (dtcrop.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtcrop, CmbCropYear, "Id", "CropYear", "Crop Year", true);
			if (Id > 0)
			{
				if (dtcrop.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbCropYear.Value = Id;
				}
				else
				{
					((Control)(object)CmbCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLot()
	{
		try
		{
			int Id = Conversion.ToInt(comjobLot.Value);
			dtjoblot.Clear();
			dtjoblot = CommonServices.JobLotGetAllService();
			if (dtjoblot.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtjoblot, comjobLot, "Id", "JobLotDescription", "Job Lot ", true);
			if (Id > 0)
			{
				if (dtjoblot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comjobLot.Value = Id;
				}
				else
				{
					((Control)(object)comjobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingType()
	{
		try
		{
			int Id = Conversion.ToInt(comPackingType.Value);
			DataTable dtpkt = InvPackingType.Getall();
			if (dtpkt.Rows.Count <= 0)
			{
				return;
			}
			dtpacktype.Clear();
			for (int i = 0; i < dtpkt.Rows.Count; i++)
			{
				dtpacktype.Rows.Add(dtpkt.Rows[i]["Id"], dtpkt.Rows[i]["PackTypeDesc"]);
			}
			if (dtpacktype.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtpacktype, comPackingType, "Id", "PackTypeDesc", "PackingType", ZeroIndex: true);
			((UltraGridBase)comPackingType).Rows[0].Activate();
			if (Id > 0)
			{
				if (dtpacktype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comPackingType.Value = Id;
				}
				else
				{
					((Control)(object)comPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackUOM()
	{
		try
		{
			string Packuom = ((Control)(object)comPackUOM).Text;
			string Rateuom = ((Control)(object)comRateUOM).Text;
			((Control)(object)comPackUOM).Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(comItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, comPackUOM, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				DDL.BindDDL(dtUom, comRateUOM, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)comPackUOM).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)comPackUOM).DisplayLayout.Bands[0].Columns[2].Width = 85;
				((UltraGridBase)comRateUOM).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)comRateUOM).DisplayLayout.Bands[0].Columns[1].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)comPackUOM).Text = Packuom;
				}
				else
				{
					((Control)(object)comPackUOM).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)comRateUOM).Text = Rateuom;
				}
				else
				{
					((Control)(object)comRateUOM).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)comPackUOM).Text = string.Empty;
				((Control)(object)comRateUOM).Text = string.Empty;
				((UltraGridBase)comPackUOM).DataSource = null;
				((UltraGridBase)comRateUOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomers(1);
				if (dt.Rows.Count > 0)
				{
					dtAccountlst.Clear();
					dtAccountlstForFreight.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
						dtAccountlst.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
					}
				}
				return;
			}
			dtAccountlst.Rows.Clear();
			dtAccountlstForFreight.Rows.Clear();
			DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
			if (dt2.Rows.Count > 0)
			{
				dtAccountlst.Clear();
				for (int l = 0; l < dt2.Rows.Count; l++)
				{
					dtAccountlst.Rows.Add(Conversion.ToInt(dt2.Rows[l]["Id"]), 0, Conversion.ToString(dt2.Rows[l]["AccountTitle"]));
				}
			}
			if (!FreightDebitToExpenses)
			{
				DataTable dtfreight = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
				if (dtfreight.Rows.Count > 0)
				{
					dtAccountlstForFreight.Clear();
					for (int j = 0; j < dtfreight.Rows.Count; j++)
					{
						dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dtfreight.Rows[j]["Id"]), 0, Conversion.ToString(dtfreight.Rows[j]["AccountTitle"]));
					}
				}
				return;
			}
			DataTable dtfreight2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
			if (dtfreight2.Rows.Count > 0)
			{
				dtAccountlstForFreight.Clear();
				for (int k = 0; k < dtfreight2.Rows.Count; k++)
				{
					dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dtfreight2.Rows[k]["Id"]), 0, Conversion.ToString(dtfreight2.Rows[k]["AccountTitle"]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FreightDeductionAccountFill()
	{
		try
		{
			DataTable dtfreight = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
			if (dtfreight.Rows.Count > 0 && dtfreight != null)
			{
				DDL.BindDDL(dtfreight, CmbFreightDeduction, "Id", "AccountTitle", "Freight Deduction Account", ZeroIndex: false);
				CmbFreightDeduction.Value = InwardFreightAccountId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 1104);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			BillWeightDeliveryTermWise();
			PackUOM();
			if (((UltraGridBase)comItem).ActiveRow != null && Conversion.ToInt(comItem.Value) > 0)
			{
				GetTaxTypeIdAndTaxPercent();
				return;
			}
			((Control)(object)cmbtaxtype).Text = string.Empty;
			((UltraGridBase)cmbtaxtype).DataSource = null;
			txtTaxPercent.Clear();
			txtTaxAmount.Clear();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void BillWeightDeliveryTermWise()
	{
		try
		{
			decimal SupplierWeight = default(decimal);
			decimal FactoryWeight = default(decimal);
			if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory" || ((Control)(object)CmbDeliveryTerm).Text == "Factory PartyWeight")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text.Trim());
				txtGrossWeight.Text = SupplierWeight.ToString();
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "Factory FactoryWeight" || ((Control)(object)CmbDeliveryTerm).Text == "ExFactory FactoryWeight")
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				txtGrossWeight.Text = FactoryWeight.ToString();
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "Factory")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text.Trim());
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				if (SupplierWeight > FactoryWeight)
				{
					txtGrossWeight.Text = FactoryWeight.ToString();
				}
				else
				{
					txtGrossWeight.Text = SupplierWeight.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTaxTypeIdAndTaxPercent()
	{
		try
		{
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(comItem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				dtTax.Clear();
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, cmbtaxtype, "Id", "TaxType", "Tax Name", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)cmbtaxtype).Text = string.Empty;
				((UltraGridBase)cmbtaxtype).DataSource = null;
				txtTaxPercent.Text = "";
				txtTaxAmount.Text = "";
			}
			if (updateDetailIndex > -1 && Conversion.ToInt(grd.CurrentRow.Cells["TaxId"].Value) > 0)
			{
				cmbtaxtype.Value = Conversion.ToInt(grd.CurrentRow.Cells["TaxId"].Value);
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void comPackingType_Leave(object sender, EventArgs e)
	{
	}

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
	}

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(comItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(comItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				comItem.Value = ItemId;
				comItem.Focus();
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(comsupplier.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(comsupplier.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToDecimal((object)txtSupplierWeight.Text) == 0m)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtSupplierWeight.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtFactoryWeight.Text) == 0m)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtFactoryWeight.Focus();
			return false;
		}
		if (((UltraGridBase)comproject).ActiveRow == null || Conversion.ToInt(comproject.Value) == 0)
		{
			MessageBox.Show("Project Field is Required");
			comproject.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null || Conversion.ToInt(CmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (((((UltraGridBase)CmbBrokeryAc).ActiveRow != null && Conversion.ToInt(CmbBrokeryAc.Value) > 0) || Conversion.ToDecimal((object)txtBrokeryAmount.Text.Trim()) > 0m) && txtCommissionRemarks.Text == string.Empty)
		{
			MessageBox.Show("Brokery Remarks Field is Required");
			txtBrokeryRemarks.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		if (txtBillAmount.Text.Trim() == null || txtBillAmount.Text.Trim() == "" || Conversion.ToDecimal((object)txtBillAmount.Text) == 0m)
		{
			MessageBox.Show("Bill Amount Field is Required");
			txtBillAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (Conversion.ToDecimal((object)txtSupplierWeight.Text) == 0m)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtSupplierWeight.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtFactoryWeight.Text) == 0m)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtFactoryWeight.Focus();
			return false;
		}
		if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)comPackingType).ActiveRow == null || Conversion.ToInt(comPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			comPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)comPackUOM).ActiveRow == null || Conversion.ToInt(comPackUOM.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			comPackUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtQty.Text.Trim()) == 0m)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtGrossWeight.Text.Trim()) <= 0m)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetBillWeight.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtNetBillWeight.Text.Trim()) <= 0m)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetBillWeight.Focus();
			return false;
		}
		if (txtStockWeight.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtStockWeight.Text.Trim()) <= 0m)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtStockWeight.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtRate.Text) == 0m)
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			comRateUOM.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
		{
			throw new Exception("Please Check Item Amount");
		}
		if (txtTotalAmount.Text == string.Empty || txtTotalAmount.Text == "0")
		{
			throw new Exception("Please Check Total Amount");
		}
		if (((UltraGridBase)txtCity).ActiveRow == null || Conversion.ToInt(txtCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			txtCity.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtTaxAmount.Text) > 0m)
		{
			if (txtTaxPercent.Text.Trim() == string.Empty || txtTaxPercent.Text.Trim() == "0" || txtTaxPercent.Text.Trim() == null)
			{
				MessageBox.Show("Tax Percent is Required");
				txtTaxPercent.Focus();
				return false;
			}
			if (Conversion.ToDecimal((object)txtTaxAmount.Text) > 0m && (((UltraGridBase)cmbtaxtype).ActiveRow == null || Conversion.ToInt(cmbtaxtype.Value) == 0))
			{
				MessageBox.Show("Tax Name is Required");
				cmbtaxtype.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void OtherItemsBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsTypeFill()
	{
		try
		{
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsItemFill()
	{
		try
		{
			dtEmptyBagsItem = CommonServices.GetItemByItemTypeId("14");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditAccountForEmptyBag()
	{
		try
		{
			dtCrAccountForEmptyBag = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsGridRefresh()
	{
		try
		{
			grdEmptyBags.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grdEmptyBags.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["ItemId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdEmptyBags.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtEmptyBagsItem.DefaultView, "Id", "ItemName");
			grdEmptyBags.RootTable.Columns["ItemId"].Width = 250;
			grdEmptyBags.RootTable.Columns["Type"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["Type"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["Type"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["Type"].Caption = "Type";
			grdEmptyBags.RootTable.Columns["Type"].ValueList.PopulateValueList((IEnumerable)dtEmpType.DefaultView, "Id", "Type");
			grdEmptyBags.RootTable.Columns["Type"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		dtEmptyBags.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdEmptyBags.DataSource = dtEmptyBags;
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			grdEmptyBags.ColumnAutoResize = false;
			EmptyBagsGridRefresh();
			if (OrderExist)
			{
				grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)0;
				grdEmptyBags.RootTable.Columns["Rate"].EditType = (EditType)0;
			}
			if (OrderExist && EmptyBagsWeightCutStatus != 1)
			{
				grdEmptyBags.RootTable.Columns["ReceivedQty"].EditType = (EditType)0;
			}
			grdEmptyBags.RootTable.Columns["PurchaseOrderId"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["Remarks"].Width = 400;
			grdEmptyBags.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].Width = 150;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].Caption = "Credit account";
			grdEmptyBags.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dtCrAccountForEmptyBag.DefaultView, "Id", "AccountTitle");
			grdEmptyBags.RootTable.Columns["PurchaseQty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["PurchaseQty"].TextAlignment = (TextAlignment)3;
			grdEmptyBags.RootTable.Columns["PurchaseQty"].FormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["PurchaseQty"].TotalFormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["ReceivedQty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["ReceivedQty"].TextAlignment = (TextAlignment)3;
			grdEmptyBags.RootTable.Columns["ReceivedQty"].FormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["ReceivedQty"].TotalFormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdEmptyBags.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdEmptyBags.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdEmptyBags.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdEmptyBags);
			grdEmptyBags.RootTable.Columns.Add("Delete");
			grdEmptyBags.RootTable.Columns["Delete"].Key = "Delete";
			grdEmptyBags.RootTable.Columns["Delete"].Caption = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonText = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Delete"].Width = 20;
			grdEmptyBags.RootTable.Columns["Delete"].Position = 0;
			grdEmptyBags.RootTable.Columns.Add("Add");
			grdEmptyBags.RootTable.Columns["Add"].Caption = "Add";
			grdEmptyBags.RootTable.Columns["Add"].Caption = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonText = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Add"].Width = 20;
			grdEmptyBags.RootTable.Columns["Add"].Position = 1;
			grdEmptyBags.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "ReceivedQty" || e.Column.Key == "PurchaseQty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdEmptyBags.CurrentRow;
				dtEmptyBags.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtEmptyBags.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Column.Key == "Add" && grdEmptyBags.GetRows().Length == 1)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmptyBags.UpdateData();
			GridEXRow item = grdEmptyBags.CurrentRow;
			if ((e.Column.Key == "Rate" || e.Column.Key == "PurchaseQty") && item != null && (int)item.RowType == 0 && item.Cells["PurchaseQty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal(item.Cells["PurchaseQty"].Value);
				decimal Rate = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount;
			}
			grdEmptyBags.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && Conversion.ToInt(grd.GetRow(0).Cells["PurchaseOrderId"].Value) > 0)
			{
				throw new Exception("You can not add manual Record beacause record against order exist in Grid");
			}
			if (FormValidationDetail())
			{
				if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
				{
					throw new Exception("Please Check ItemAmount");
				}
				dtGrid.Rows.Add(0, 0, 0, 0, Conversion.ToInt(comWarehouse.Value), Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropYear.Value), Conversion.ToInt(comjobLot.Value), Conversion.ToInt(comPackingType.Value), Conversion.ToInt(comPackUOM.Value), Conversion.ToString((object)((Control)(object)comPackUOM).Text), Conversion.ToDecimal(((UltraDropDownBase)comPackUOM).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtGrossWeight.Text), Conversion.ToDecimal((object)txtEmptybagsUnit.Text), Conversion.ToDecimal((object)txtEmptyBagsTotal.Text), Conversion.ToDecimal((object)txtwtcut.Text), Conversion.ToDecimal((object)txtWeightCutTotal.Text), Conversion.ToDecimal((object)txtAddLss.Text), Conversion.ToDecimal((object)txtNetBillWeight.Text), Conversion.ToDecimal((object)txtStockWeight.Text), Conversion.ToDecimal((object)txtRate.Text), Conversion.ToInt(comRateUOM.Value), Conversion.ToString((object)((Control)(object)comRateUOM).Text), Conversion.ToDecimal(((UltraDropDownBase)comRateUOM).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDecimal((object)txtratecut.Text), Conversion.ToDecimal((object)txtratecuttotal.Text), Conversion.ToDecimal((object)txtAmount.Text), Conversion.ToInt(cmbtaxtype.Value), Conversion.ToDecimal((object)txtTaxPercent.Text), Conversion.ToDecimal((object)txtTaxAmount.Text), Conversion.ToString((object)txtlabanalysisno.Text), Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString(), Conversion.ToInt((object)txtgatepassno.Text), Conversion.ToString((object)txtvehicleno.Text), Conversion.ToInt(txtCity.Value), Conversion.ToString((object)txtremarksDetail.Text), 0, 0, 0, 0, Conversion.ToDecimal((object)txtTotalAmount.Text), (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Math.Round(Conversion.ToDecimal((object)txtAmount.Text) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount) : 0m);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				TotalCommissionAmount();
				TotalBrokeryAmount();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				FreightDeductionProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_doubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) == 0)
			{
				updateDetailIndex = r.RowIndex;
				comWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				comItem.Value = r.Cells["ItemId"].Value.ToString();
				comItem_Leave(null, null);
				CmbCropYear.Value = r.Cells["CropYearId"].Value.ToString();
				comjobLot.Value = r.Cells["JobLotId"].Value.ToString();
				comPackingType.Value = r.Cells["PackingTypeId"].Value.ToString();
				comPackUOM.Value = r.Cells["PackUOMId"].Value.ToString();
				txtQty.Text = Conversion.ToDecimal(r.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtGrossWeight.Text = Conversion.ToDecimal(r.Cells["GrossWeight"].Value).ToString("#,##0.###");
				txtEmptybagsUnit.Text = Conversion.ToDecimal(r.Cells["EmptyBags"].Value).ToString("#,##0.###");
				txtEmptyBagsTotal.Text = Conversion.ToDecimal(r.Cells["EmptyBagsTotal"].Value).ToString("#,##0.###");
				txtwtcut.Text = Conversion.ToDecimal(r.Cells["WeightCut"].Value).ToString("#,##0.###");
				txtWeightCutTotal.Text = Conversion.ToDecimal(r.Cells["WeightCutTotal"].Value).ToString("#,##0.###");
				txtAddLss.Text = Conversion.ToDecimal(r.Cells["AddLss"].Value).ToString("#,##0.###");
				txtNetBillWeight.Text = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value).ToString("#,##0.###");
				txtStockWeight.Text = Conversion.ToDecimal(r.Cells["StockWeight"].Value).ToString("#,##0.###");
				txtRate.Text = Conversion.ToDecimal(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				comRateUOM.Value = r.Cells["RateUOMId"].Value.ToString();
				txtratecut.Text = Conversion.ToDecimal(r.Cells["RateCut"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtratecuttotal.Text = Conversion.ToDecimal(r.Cells["RateCutTotal"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtAmount.Text = Conversion.ToDecimal(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				if (Conversion.ToInt(r.Cells["TaxId"].Value) > 0)
				{
					cmbtaxtype.Value = r.Cells["TaxId"].Value.ToString();
				}
				txtTaxPercent.Text = Conversion.ToDecimal(r.Cells["Tax%"].Value).ToString("#,##0.###");
				txtTaxAmount.Text = Conversion.ToDecimal(r.Cells["TaxAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtlabanalysisno.Text = r.Cells["LabSampleNo"].Value.ToString();
				txtgpdate.Value = Conversion.ToDateTime(r.Cells["GpDate"].Value);
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				txtCity.Value = r.Cells["CityId"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
				dtGrid.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value);
				dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
				dtGrid.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(comjobLot.Value);
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(comPackingType.Value);
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
				dtGrid.Rows[updateDetailIndex]["PackUOMId"] = Conversion.ToInt(comPackUOM.Value);
				dtGrid.Rows[updateDetailIndex]["PackUOM"] = Conversion.ToString((object)((Control)(object)comPackUOM).Text);
				dtGrid.Rows[updateDetailIndex]["PackUOM"] = Conversion.ToDecimal(((UltraDropDownBase)comPackUOM).SelectedRow.Cells["Equivalent"].Value);
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDecimal((object)txtGrossWeight.Text);
				dtGrid.Rows[updateDetailIndex]["EmptyBags"] = Conversion.ToDecimal((object)txtEmptybagsUnit.Text);
				dtGrid.Rows[updateDetailIndex]["EmptyBagsTotal"] = Conversion.ToDecimal((object)txtEmptyBagsTotal.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCut"] = Conversion.ToDecimal((object)txtwtcut.Text);
				dtGrid.Rows[updateDetailIndex]["WeightCutTotal"] = Conversion.ToDecimal((object)txtWeightCutTotal.Text);
				dtGrid.Rows[updateDetailIndex]["AddLss"] = Conversion.ToDecimal((object)txtAddLss.Text);
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = Conversion.ToDecimal((object)txtNetBillWeight.Text);
				dtGrid.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDecimal((object)txtStockWeight.Text);
				dtGrid.Rows[updateDetailIndex]["Rate"] = Conversion.ToDecimal((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["RateUOMId"] = Conversion.ToInt(comRateUOM.Value);
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToString((object)((Control)(object)comRateUOM).Text);
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToDecimal(((UltraDropDownBase)comRateUOM).SelectedRow.Cells["Equivalent"].Value);
				dtGrid.Rows[updateDetailIndex]["RateCut"] = Conversion.ToDecimal((object)txtratecut.Text);
				dtGrid.Rows[updateDetailIndex]["RateCutTotal"] = Conversion.ToDecimal((object)txtratecuttotal.Text);
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDecimal((object)txtAmount.Text);
				dtGrid.Rows[updateDetailIndex]["TaxId"] = Conversion.ToInt(cmbtaxtype.Value);
				dtGrid.Rows[updateDetailIndex]["Tax%"] = Conversion.ToDecimal((object)txtTaxPercent.Text);
				dtGrid.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDecimal((object)txtTaxAmount.Text);
				dtGrid.Rows[updateDetailIndex]["LabSampleNo"] = Conversion.ToString((object)txtlabanalysisno.Text);
				dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString();
				dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
				dtGrid.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				TotalCommissionAmount();
				TotalBrokeryAmount();
				FreightProportion();
				CommissionProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = dtGrid.Rows[r.RowIndex];
				dtGrid.Rows.Add(dr.ItemArray);
				grd.DataSource = dtGrid;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
					Pd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					Pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Pd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					Pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Pd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Pd.ItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
					Pd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					Pd.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
					Pd.EBWeight = Conversion.ToDecimal(r.Cells["EmptyBags"].Value);
					Pd.EBTotalWt = Conversion.ToDecimal(r.Cells["EmptyBagsTotal"].Value);
					Pd.WeightCut = Conversion.ToDecimal(r.Cells["WeightCut"].Value);
					Pd.WeightCutTotal = Conversion.ToDecimal(r.Cells["WeightCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDecimal(r.Cells["AddLss"].Value);
					Pd.NetBillWeight = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDecimal(r.Cells["StockWeight"].Value);
					Pd.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					Pd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					Pd.RateCut = Conversion.ToDecimal(r.Cells["RateCut"].Value);
					Pd.RateCutAmount = Conversion.ToDecimal(r.Cells["RateCutTotal"].Value);
					Pd.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					Pd.LabAnalisysNo = Conversion.ToString(r.Cells["LabSampleNo"].Value);
					Pd.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
					Pd.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					Pd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					Pd.TaxNameId = Conversion.ToInt(r.Cells["TaxId"].Value);
					Pd.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
					Pd.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
					if (Pd.TaxNameId > 0 && Pd.TaxAmount > 0m)
					{
						Pd.IsTaxable = true;
					}
					Pd.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
					Pd.FreightAmount = Conversion.ToDecimal(r.Cells["Freights"].Value);
					Pd.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
					Pd.FreightDeduction = Conversion.ToDecimal(r.Cells["LessDeduction"].Value);
					Pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					Pd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					Pd.ItemNetAmount = Math.Round(Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					Pd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					Pd.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(Pd);
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			if (e.Column.Key == "Edit")
			{
				grd_doubleClick(null, null);
			}
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			FreightProportion();
			ExpProportion();
			CommissionProportion();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			TotalCommissionAmount();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
			grd.UpdateData();
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
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["PackUOMEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUOMEquivalent"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderNo"].Visible = false;
			grd.RootTable.Columns["Freights"].Caption = "ChargeToProduct";
			grd.RootTable.Columns["PurchaseOrderNo"].Caption = "OrderNo";
			grd.RootTable.Columns["ItemQty"].Caption = "Bags";
			grd.RootTable.Columns["LessDeduction"].Caption = "FreightDeduction";
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseOrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["jobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOMEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["EmptyBags"].EditType = (EditType)0;
			grd.RootTable.Columns["EmptyBagsTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["WeightCut"].EditType = (EditType)0;
			grd.RootTable.Columns["WeightCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLss"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCut"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUOMEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["TaxId"].EditType = (EditType)0;
			grd.RootTable.Columns["Tax%"].EditType = (EditType)0;
			grd.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["LabSampleNo"].EditType = (EditType)0;
			grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)1;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
			grd.RootTable.Columns["CityId"].EditType = (EditType)4;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["Freights"].EditType = (EditType)0;
			grd.RootTable.Columns["LessDeduction"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemNetAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grd.RootTable.Columns["TaxId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["TaxId"].HasValueList = true;
			grd.RootTable.Columns["TaxId"].LimitToList = true;
			grd.RootTable.Columns["TaxId"].EditType = (EditType)(TaxPercntEditableOrNot ? 4 : 0);
			grd.RootTable.Columns["TaxId"].Caption = "TaxName";
			grd.RootTable.Columns["TaxId"].ValueList.PopulateValueList((IEnumerable)dtTaxType.DefaultView, "TaxNameId", "TaxName");
			grd.RootTable.Columns["TaxId"].Width = 60;
			grd.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WarehouseId"].HasValueList = true;
			grd.RootTable.Columns["WarehouseId"].LimitToList = true;
			grd.RootTable.Columns["WarehouseId"].Caption = "Warehouse";
			grd.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtwareh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WarehouseId"].Width = 60;
			grd.RootTable.Columns["CropYearId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CropYearId"].HasValueList = true;
			grd.RootTable.Columns["CropYearId"].LimitToList = true;
			grd.RootTable.Columns["CropYearId"].Caption = "CropYear";
			grd.RootTable.Columns["CropYearId"].ValueList.PopulateValueList((IEnumerable)dtcrop.DefaultView, "Id", "CropYear");
			grd.RootTable.Columns["CropYearId"].Width = 60;
			grd.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeId"].Caption = "PackingType";
			grd.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtpacktype.DefaultView, "Id", "PackTypeDesc");
			grd.RootTable.Columns["PackingTypeId"].Width = 60;
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].Caption = "JobLot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtjoblot.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["JobLotId"].Width = 60;
			grd.RootTable.Columns["CityId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CityId"].HasValueList = true;
			grd.RootTable.Columns["CityId"].LimitToList = true;
			grd.RootTable.Columns["CityId"].Caption = "CityName";
			grd.RootTable.Columns["CityId"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["CityId"].Width = 60;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBags"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBags"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBagsTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBagsTotal"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBagsTotal"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AddLss"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLss"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLss"].FormatString = "#,##0.###";
			grd.RootTable.Columns["AddLss"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["LessDeduction"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LessDeduction"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LessDeduction"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["LessDeduction"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			if (Conversion.ToInt(grd.GetRow(0).Cells["PurchaseOrderId"].Value) > 0)
			{
				grd.RootTable.Columns["PurchaseOrderNo"].Visible = true;
				grd.RootTable.Columns["WarehouseId"].EditType = (EditType)4;
				grd.RootTable.Columns["CropYearId"].EditType = (EditType)4;
				grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
				grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
				grd.RootTable.Columns["TaxId"].EditType = (EditType)4;
				grd.RootTable.Columns["Tax%"].EditType = (EditType)(TaxPercntEditableOrNot ? 1 : 0);
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
				grd.RootTable.Columns["GrossWeight"].EditType = (EditType)1;
				grd.RootTable.Columns["EmptyBags"].EditType = (EditType)1;
				grd.RootTable.Columns["EmptyBagsTotal"].EditType = (EditType)1;
				grd.RootTable.Columns["WeightCut"].EditType = (EditType)1;
				grd.RootTable.Columns["WeightCutTotal"].EditType = (EditType)1;
				grd.RootTable.Columns["CityId"].EditType = (EditType)4;
				grd.RootTable.Columns["AddLss"].EditType = (EditType)1;
				grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
				grd.RootTable.Columns["Rate"].EditType = (EditType)0;
				grd.RootTable.Columns["RateCut"].EditType = (EditType)1;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
				grd.RootTable.Columns["GpNo"].EditType = (EditType)1;
				grd.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Add"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.RootTable.Columns.Add("Edit");
				grd.RootTable.Columns["Edit"].Key = "Edit";
				grd.RootTable.Columns["Edit"].Caption = "Edit";
				grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Edit"].ButtonText = "Edit";
				grd.RootTable.Columns["Edit"].Width = 50;
				grd.RootTable.Columns["Edit"].Position = 1;
				grd.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "EmptyBags" && item.Cells["EmptyBags"].Value.ToString() != string.Empty)
			{
				decimal Qty5 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				decimal PackUOM5 = Conversion.ToDecimal((object)item.Cells["PackUOMEquivalent"].Value.ToString());
				decimal Emptybags = Conversion.ToDecimal((object)item.Cells["EmptyBags"].Value.ToString());
				decimal EmptyBagsTotal6 = Qty5 * Emptybags;
				item.Cells["EmptyBagsTotal"].Value = EmptyBagsTotal6;
				decimal WeightCutTotal6 = Conversion.ToDecimal((object)item.Cells["WeightCutTotal"].Value.ToString());
				decimal AddLesswt6 = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal num = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				if (num == 0m)
				{
					item.Cells["GrossWeight"].Value = Qty5 * PackUOM5;
				}
				decimal NetBillWeight6 = num - WeightCutTotal6 + AddLesswt6;
				decimal StockWeight6 = num - EmptyBagsTotal6 + WeightCutTotal6;
				item.Cells["NetBillWeight"].Value = NetBillWeight6.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight6.ToString("0,00");
				decimal Rate7 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM7 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut7 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal7 = NetBillWeight6 / RateUOM7 * RateCut7;
				item.Cells["RateCutTotal"].Value = RateCutTotal7;
				decimal ItemAmount7 = NetBillWeight6 / RateUOM7 * Rate7;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount7 - RateCutTotal7, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "EmptyBagsTotal" && item.Cells["EmptyBagsTotal"].Value.ToString() != string.Empty)
			{
				decimal Qty4 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				decimal PackUOM4 = Conversion.ToDecimal((object)item.Cells["PackUOMEquivalent"].Value.ToString());
				item.Cells["EmptyBags"].Value = 0;
				decimal EmptyBagsTotal5 = Conversion.ToDecimal((object)item.Cells["EmptyBagsTotal"].Value.ToString());
				decimal WeightCutTotal5 = Conversion.ToDecimal((object)item.Cells["WeightCutTotal"].Value.ToString());
				decimal AddLesswt5 = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal num2 = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				if (num2 == 0m)
				{
					item.Cells["GrossWeight"].Value = Qty4 * PackUOM4;
				}
				decimal NetBillWeight5 = num2 - WeightCutTotal5 + AddLesswt5;
				decimal StockWeight5 = num2 - EmptyBagsTotal5 + WeightCutTotal5;
				item.Cells["NetBillWeight"].Value = NetBillWeight5.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight5.ToString("0,00");
				decimal Rate6 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM6 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut6 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal6 = NetBillWeight5 / RateUOM6 * RateCut6;
				item.Cells["RateCutTotal"].Value = RateCutTotal6;
				decimal ItemAmount6 = NetBillWeight5 / RateUOM6 * Rate6;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount6 - RateCutTotal6, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "WeightCut" && item.Cells["WeightCut"].Value.ToString() != string.Empty)
			{
				decimal Qty3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				decimal PackUOM3 = Conversion.ToDecimal((object)item.Cells["PackUOMEquivalent"].Value.ToString());
				decimal EmptyBagsTotal4 = Conversion.ToDecimal((object)item.Cells["EmptyBagsTotal"].Value.ToString());
				decimal WeightCut = Conversion.ToDecimal((object)item.Cells["WeightCut"].Value.ToString());
				decimal WeightCutTotal4 = Qty3 * WeightCut;
				item.Cells["WeightCutTotal"].Value = WeightCutTotal4;
				decimal AddLesswt4 = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal num3 = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				if (num3 == 0m)
				{
					item.Cells["GrossWeight"].Value = Qty3 * PackUOM3;
				}
				decimal NetBillWeight4 = num3 - WeightCutTotal4 + AddLesswt4;
				decimal StockWeight4 = num3 - EmptyBagsTotal4 - WeightCutTotal4;
				item.Cells["NetBillWeight"].Value = NetBillWeight4.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight4.ToString("0,00");
				decimal Rate5 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM5 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut5 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal5 = NetBillWeight4 / RateUOM5 * RateCut5;
				item.Cells["RateCutTotal"].Value = RateCutTotal5;
				decimal ItemAmount5 = NetBillWeight4 / RateUOM5 * Rate5;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount5 - RateCutTotal5, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "WeightCutTotal" && item.Cells["WeightCutTotal"].Value.ToString() != string.Empty)
			{
				decimal Qty2 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				decimal PackUOM2 = Conversion.ToDecimal((object)item.Cells["PackUOMEquivalent"].Value.ToString());
				item.Cells["WeightCut"].Value = 0;
				decimal EmptyBagsTotal3 = Conversion.ToDecimal((object)item.Cells["EmptyBagsTotal"].Value.ToString());
				decimal WeightCutTotal3 = Conversion.ToDecimal((object)item.Cells["WeightCutTotal"].Value.ToString());
				decimal AddLesswt3 = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal num4 = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				if (num4 == 0m)
				{
					item.Cells["GrossWeight"].Value = Qty2 * PackUOM2;
				}
				decimal NetBillWeight3 = num4 - WeightCutTotal3 + AddLesswt3;
				decimal StockWeight3 = num4 - EmptyBagsTotal3 - WeightCutTotal3;
				item.Cells["NetBillWeight"].Value = NetBillWeight3.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight3.ToString("0,00");
				decimal Rate4 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM4 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut4 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal4 = NetBillWeight3 / RateUOM4 * RateCut4;
				item.Cells["RateCutTotal"].Value = RateCutTotal4;
				decimal ItemAmount4 = NetBillWeight3 / RateUOM4 * Rate4;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount4 - RateCutTotal4, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "AddLss" && item.Cells["AddLss"].Value.ToString() != string.Empty)
			{
				decimal Qty = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				decimal PackUOM = Conversion.ToDecimal((object)item.Cells["PackUOMEquivalent"].Value.ToString());
				decimal EmptyBagsTotal2 = Conversion.ToDecimal((object)item.Cells["EmptyBagsTotal"].Value.ToString());
				decimal WeightCutTotal2 = Conversion.ToDecimal((object)item.Cells["WeightCutTotal"].Value.ToString());
				decimal AddLesswt2 = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal num5 = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				if (num5 == 0m)
				{
					item.Cells["GrossWeight"].Value = Qty * PackUOM;
				}
				decimal NetBillWeight2 = num5 - WeightCutTotal2 + AddLesswt2;
				decimal StockWeight2 = num5 - EmptyBagsTotal2 - WeightCutTotal2;
				item.Cells["NetBillWeight"].Value = NetBillWeight2.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight2.ToString("0,00");
				decimal Rate3 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM3 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut3 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal3 = NetBillWeight2 / RateUOM3 * RateCut3;
				item.Cells["RateCutTotal"].Value = RateCutTotal3;
				decimal ItemAmount3 = NetBillWeight2 / RateUOM3 * Rate3;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount3 - RateCutTotal3, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "GrossWeight" && item.Cells["GrossWeight"].Value.ToString() != string.Empty)
			{
				decimal num6 = Conversion.ToDecimal((object)item.Cells["GrossWeight"].Value.ToString());
				decimal EmptyBagsTotal = Conversion.ToDecimal((object)item.Cells["EmptyBagsTotal"].Value.ToString());
				decimal WeightCutTotal = Conversion.ToDecimal((object)item.Cells["WeightCutTotal"].Value.ToString());
				decimal AddLesswt = Conversion.ToDecimal((object)item.Cells["AddLss"].Value.ToString());
				decimal NetBillWeight = num6 - WeightCutTotal + AddLesswt;
				decimal StockWeight = num6 - EmptyBagsTotal - WeightCutTotal;
				item.Cells["NetBillWeight"].Value = NetBillWeight.ToString("0,00");
				item.Cells["StockWeight"].Value = StockWeight.ToString("0,00");
				decimal Rate2 = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM2 = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut2 = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal2 = NetBillWeight / RateUOM2 * RateCut2;
				item.Cells["RateCutTotal"].Value = RateCutTotal2;
				decimal ItemAmount2 = NetBillWeight / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount2 - RateCutTotal2, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "Rate" || e.Column.Key == "RateCut")
			{
				Conversion.ToDecimal(item.Cells["GrossWeight"].Value);
				Conversion.ToDecimal(item.Cells["EmptyBagsTotal"].Value);
				Conversion.ToDecimal(item.Cells["WeightCutTotal"].Value);
				Conversion.ToDecimal(item.Cells["AddLss"].Value);
				decimal num7 = Conversion.ToDecimal(item.Cells["NetBillWeight"].Value);
				decimal Rate = Conversion.ToDecimal(item.Cells["Rate"].Value);
				decimal RateUOM = Conversion.ToDecimal(item.Cells["RateUOMEquivalent"].Value);
				decimal RateCut = Conversion.ToDecimal(item.Cells["RateCut"].Value);
				decimal RateCutTotal = num7 / RateUOM * RateCut;
				item.Cells["RateCutTotal"].Value = RateCutTotal;
				decimal ItemAmount = num7 / RateUOM * Rate;
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount - RateCutTotal, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "TaxId")
			{
				int selectedTaxId = Conversion.ToInt(item.Cells["TaxId"].Value);
				if (selectedTaxId == 0)
				{
					item.Cells["Tax%"].Value = 0;
				}
				else
				{
					DataRow[] matchingRows = dtTaxType.Select("TaxNameId = " + selectedTaxId);
					if (matchingRows.Length != 0)
					{
						item.Cells["Tax%"].Value = Conversion.ToDecimal(matchingRows[0]["TaxPercent"]);
					}
				}
			}
			else if (e.Column.Key == "Tax%" && Conversion.ToDecimal(item.Cells["Tax%"].Value) == 0m)
			{
				int selectedTaxId2 = Conversion.ToInt(item.Cells["TaxId"].Value);
				DataRow[] matchingRows2 = dtTaxType.Select("TaxNameId = " + selectedTaxId2);
				if (matchingRows2.Length != 0)
				{
					item.Cells["Tax%"].Value = Conversion.ToDecimal(matchingRows2[0]["TaxPercent"]);
				}
			}
			int TaxType = Conversion.ToInt(item.Cells["TaxId"].Value);
			decimal TaxPercent = Conversion.ToDecimal(item.Cells["Tax%"].Value);
			decimal ItemAmountt = Conversion.ToDecimal(item.Cells["ItemAmount"].Value);
			decimal TaxAmount = default(decimal);
			if (TaxPercent > 100m)
			{
				MessageBox.Show("Tax % Can't be greater than 100...");
				TaxPercent = 100m;
				item.Cells["Tax%"].Value = TaxPercent;
			}
			if (ItemAmountt > 0m && TaxPercent > 0m && TaxType > 0)
			{
				TaxAmount = ItemAmountt * TaxPercent / 100m;
				item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			else
			{
				item.Cells["Tax%"].Value = 0;
				item.Cells["TaxAmount"].Value = 0;
			}
			grd.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grd);
			TotalCommissionAmount();
			CommissionProportion();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.ColumnAutoResize = false;
			grdInvExp.RootTable.Columns["Id"].Visible = false;
			grdInvExp.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grdInvExp.RootTable.Columns["PurchaseOrderSupplierExpId"].Visible = false;
			grdInvExp.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["PurchaseOrderId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["PurchaseOrderSupplierExpId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["Remarks"].Width = 400;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Other Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			CommonServices.GridAutoAdjustmentNew(grdInvExp);
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns["Delete"].Position = 0;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Add"].Position = 1;
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal(item.Cells["Qty"].Value);
					decimal Rate = Conversion.ToDecimal(item.Cells["Rate"].Value);
					decimal Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Amount"].Value = Amount;
				}
			}
			else if (e.Column.Key == "Amount" && item.Cells["Amount"].Value.ToString() != string.Empty)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				decimal Amount2 = Math.Round(Conversion.ToDecimal(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount2;
			}
			grdInvExp.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			grdInvExp.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.ColumnAutoResize = false;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["Remarks"].Width = 300;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Supplier Add/Less";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SuppliercustomerId", "AccountTitle");
			}
			else
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Percentage"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Percentage"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdGLedger);
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns["Delete"].Position = 0;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
			grdGLedger.RootTable.Columns["Add"].Position = 1;
			grdGLedger.FrozenColumns = 2;
			SupCustIdUpdateforGLGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Credit" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (e.Column.Key == "Add")
				{
					AddRowInGLGrid();
				}
				grdGLedger.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		txtExchangeRate_TextChanged(null, null);
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				decimal Qty = Conversion.ToDecimal(item.Cells["Qty"].Value);
				decimal Rate = Conversion.ToDecimal(item.Cells["Rate"].Value);
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				decimal Percentage = Conversion.ToDecimal((object)item.Cells["Percentage"].Value.ToString());
				decimal TotlaPercentage = num / 100m * Percentage;
				if (TotlaPercentage > 0m)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Round(Math.Abs(TotlaPercentage), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit")
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDecimal((object)item.Cells["Debit"].Value.ToString()) > 0m)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit")
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDecimal((object)item.Cells["Credit"].Value.ToString()) > 0m)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Supplier Account Not select");
						item.Cells["GlAccountId"].Value = 0;
						return;
					}
				}
				else if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Supplier Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid();
			}
			grdGLedger.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforGLGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow r = grdGLedger.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["AccountId"].Text);
					if (selectedAcId != "0" && selectedAcId != string.Empty)
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDecimal(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0);
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.ColumnAutoResize = false;
			grdFreight.RootTable.Columns["Id"].Visible = false;
			grdFreight.RootTable.Columns["OrderId"].Visible = false;
			grdFreight.RootTable.Columns["OrderExpenseId"].Visible = false;
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["Debit"].Visible = FreightDebitToExpenses;
			grdFreight.RootTable.Columns["Id"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["OrderExpenseId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["Percentage"].Caption = "%";
			grdFreight.RootTable.Columns["TransporterId"].Width = 150;
			grdFreight.RootTable.Columns["Percentage"].Width = 50;
			grdFreight.RootTable.Columns["Qty"].Width = 60;
			grdFreight.RootTable.Columns["Rate"].Width = 60;
			grdFreight.RootTable.Columns["Credit"].Width = 80;
			grdFreight.RootTable.Columns["Debit"].Width = 80;
			grdFreight.RootTable.Columns["Remarks"].Width = 120;
			grdFreight.RootTable.Columns["TransporterId"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["TransporterId"].HasValueList = true;
			grdFreight.RootTable.Columns["TransporterId"].LimitToList = true;
			grdFreight.RootTable.Columns["TransporterId"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["TransporterId"].Caption = "Charge To Product";
			grdFreight.RootTable.Columns["TransporterId"].Caption = "AccountTitle";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["TransporterId"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "SuppliercustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["TransporterId"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdFreight.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdFreight.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreight.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Percentage"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreight.RootTable.Columns["Percentage"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdFreight);
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns["Delete"].Position = 0;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Add"].Position = 1;
			grdFreight.FrozenColumns = 1;
			SupCustIdUpdateforFrieghtGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforFrieghtGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["TransporterId"].Text);
					if (selectedAcId != string.Empty && selectedAcId != "0")
					{
						DataRow[] matchingRows = dtAccountlstForFreight.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDecimal(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdFreight.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtFreight.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
			}
			grdFreight.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Credit"].Value = amount;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				decimal num2 = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				decimal Percentage = Conversion.ToDecimal((object)item.Cells["Percentage"].Value.ToString());
				decimal Credit = num2 / 100m * Percentage;
				item.Cells["Credit"].Value = Math.Round(Credit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "TransporterId")
			{
				SupCustIdUpdateforFrieghtGrid();
			}
			if (e.Column.Key == "Credit")
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			if (FreightDebitToExpenses)
			{
				if (e.Column.Key == "Credit")
				{
					item.Cells["Credit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDecimal((object)item.Cells["Debit"].Value.ToString()) > 0m)
					{
						item.Cells["Credit"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit")
				{
					item.Cells["Debit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDecimal((object)item.Cells["Freight"].Value.ToString()) > 0m)
					{
						item.Cells["Debit"].Value = 0;
						MessageBox.Show("Credit Side is aleady added");
					}
				}
			}
			grdFreight.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Credit" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdFreight_Enter(object sender, EventArgs e)
	{
	}

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Expected O, but got Unknown
		//IL_1525: Unknown result type (might be due to invalid IL or missing references)
		//IL_152c: Expected O, but got Unknown
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1718: Expected O, but got Unknown
		//IL_188c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1893: Expected O, but got Unknown
		//IL_1a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8d: Expected O, but got Unknown
		//IL_1cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d02: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				purchaseInvoice.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToDecimal((object)txtFreightDeduction.Text) > 0m && (Conversion.ToInt(CmbFreightDeduction.Value) == 0 || ((UltraGridBase)CmbFreightDeduction).ActiveRow == null))
			{
				MessageBox.Show("Freight Deduction Debit Account Field Is Required");
				CmbFreightDeduction.Focus();
				return;
			}
			decimal TotalFreightCredit = default(decimal);
			decimal TotalFreightDebit = default(decimal);
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				TotalFreightCredit += Conversion.ToDecimal(r9.Cells["Credit"].Value);
				TotalFreightDebit += Conversion.ToDecimal(r9.Cells["Debit"].Value);
				if (Conversion.ToDecimal(r9.Cells["Credit"].Value) > 0m || Conversion.ToDecimal(r9.Cells["Debit"].Value) > 0m)
				{
					if (Conversion.ToInt(r9.Cells["TransporterId"].Value) == 0)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception($"Account required in Grid 'Charge to Product'...  Please Check in Row No : {r9.RowIndex + 1}");
					}
					string FrRemarks = Conversion.ToString(r9.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception($"Remarks required in Grid 'Charge to Product'...  Please Check in Row No : {r9.RowIndex + 1}");
					}
				}
			}
			if (FreightDebitToExpenses && (TotalFreightCredit > 0m || TotalFreightDebit > 0m) && TotalFreightCredit != TotalFreightDebit)
			{
				MessageBox.Show("Debit Amount not equal to Credit Amount in Grid 'Charge to Product' Please Check");
				return;
			}
			if (!FreightDebitToExpenses && TotalFreightDebit > 0m)
			{
				throw new Exception("there Should not be in Row With Debit Amount in Grid 'Charge to Product' when Configuration 'FreightDebitToExpenses' is Off");
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				if ((Conversion.ToDecimal(r8.Cells["Credit"].Value) > 0m || Conversion.ToInt(r8.Cells["Debit"].Value) > 0) && Conversion.ToInt(r8.Cells["AccountId"].Value) == 0)
				{
					((Control)(object)grdGLedger).Focus();
					throw new Exception($"Account required in Grid 'Party AddLess'...  Please Check in Row No : {r8.RowIndex + 1}");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToDecimal(r7.Cells["Amount"].Value) > 0m && Conversion.ToInt(r7.Cells["ItemId"].Value) == 0)
				{
					((Control)(object)grdInvExp).Focus();
					throw new Exception($"Item required in Grid 'Expense'...  Please Check in Row No : {r7.RowIndex + 1}");
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDecimal(r6.Cells["Amount"].Value) > 0m)
				{
					if (Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
					{
						throw new Exception($"EmptyBags Item required in Grid 'EmptyBags'...  Please Check in Row No : {r6.RowIndex + 1}");
					}
					if (Conversion.ToInt(r6.Cells["Type"].Value) == 0)
					{
						throw new Exception($"EmptyBags Type required in Grid 'EmptyBags'...  Please Check in Row No : {r6.RowIndex + 1}");
					}
				}
			}
			if ((Conversion.ToDecimal((object)txtcommamount.Text) > 0m || Conversion.ToDecimal((object)txtcommrate.Text) > 0m) && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				throw new Exception("Please Select Commission Agent Account First");
			}
			if ((Conversion.ToDecimal((object)txtBrokeryAmount.Text) > 0m || Conversion.ToDecimal((object)txtBrokeryRate.Text) > 0m) && Conversion.ToInt(CmbBrokeryAc.Value) == 0)
			{
				throw new Exception("Please Select Broker Account First");
			}
			rows = grd.GetRows();
			for (int l = 0; l < rows.Length; l++)
			{
				if (Conversion.ToDouble(rows[l].Cells["TaxAmount"].Value) > 0.0 && (((Control)(object)CmbTaxAccount).Text.Trim() == string.Empty || ((Control)(object)CmbTaxAccount).Text.Trim() == "0" || Conversion.ToInt(CmbTaxAccount.Value) == 0))
				{
					MessageBox.Show("Tax Account is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbTaxAccount.Focus();
					return;
				}
			}
			TotalCommissionAmount();
			BillAmount();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			FreightDeductionProportion();
			txtExchangeRate_TextChanged(null, null);
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			purchaseInvoice.ScreenName = Conversion.ToString((object)base.Name);
			purchaseInvoice.DocumentTypeId = 1104;
			purchaseInvoice.EntryUserId = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ApprovedDate = DateTime.Now;
			purchaseInvoice.DeliveryStartDate = DateTime.Now;
			purchaseInvoice.UnApproveDate = DateTime.Now;
			purchaseInvoice.ModifyUserId = UserAccount.ID;
			purchaseInvoice.ProjectsId = Conversion.ToInt(comproject.Value);
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			purchaseInvoice.SupplierReferenceNo = Conversion.ToInt((object)txtSupplierReference.Text.Trim());
			purchaseInvoice.SupplierBillDate = DateTime.Now;
			purchaseInvoice.RemarksHeader = txtremarks.Text.Trim();
			purchaseInvoice.FreightDebitAccountId = Conversion.ToInt(CmbFreightDeduction.Value);
			purchaseInvoice.FreightDeduction = Conversion.ToDecimal((object)txtFreightDeduction.Text);
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtduedays.Text);
			purchaseInvoice.DueDate = duedate.Value;
			purchaseInvoice.DeliveryTerm = ((Control)(object)CmbDeliveryTerm).Text.Trim();
			purchaseInvoice.SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
			purchaseInvoice.FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text);
			purchaseInvoice.DiffWeight = Conversion.ToDecimal((object)txtDiffWeight.Text);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			purchaseInvoice.CommissionType = ((Control)(object)combCommType).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDecimal((object)txtcommrate.Text);
			purchaseInvoice.CommUom = Conversion.ToDecimal((object)((Control)(object)combcommUOM).Text);
			purchaseInvoice.CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			purchaseInvoice.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			purchaseInvoice.BrokerAgentId = Conversion.ToInt(CmbBrokeryAc.Value);
			purchaseInvoice.BrokeryType = ((Control)(object)CmbBrokeryType).Text.Trim();
			purchaseInvoice.BrokeryRate = Conversion.ToDecimal((object)txtBrokeryRate.Text);
			purchaseInvoice.BrokeryUom = Conversion.ToDecimal((object)((Control)(object)CmbBrokeryRateUom).Text);
			purchaseInvoice.BrokeryAmount = Conversion.ToDecimal((object)txtBrokeryAmount.Text);
			purchaseInvoice.BrokeryRemarks = txtBrokeryRemarks.Text.Trim();
			purchaseInvoice.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			purchaseInvoice.BillQty = Conversion.ToDecimal((object)txtInvoiceQty.Text.Trim());
			purchaseInvoice.BillWeight = Conversion.ToDecimal((object)txtInvoiceWeight.Text.Trim());
			purchaseInvoice.BillAmount = Conversion.ToDecimal((object)txtBillAmount.Text.Trim());
			purchaseInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			purchaseInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
			purchaseInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text.Trim());
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
			purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
			purchaseInvoice.InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
			purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
			purchaseInvoice.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			decimal GrossWt = default(decimal);
			decimal InvoiceQty = default(decimal);
			decimal EmptyBagsRecQty = default(decimal);
			decimal EmptyBagsPurQty = default(decimal);
			rows = grd.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
				if (Conversion.ToInt(r5.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["CropYearId"].Value) == 0)
				{
					MessageBox.Show("CropYear field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["JobLotId"].Value) == 0)
				{
					MessageBox.Show("JobLot field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["PackingTypeId"].Value) == 0)
				{
					MessageBox.Show("PackingTypeId field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["PackUOMId"].Value) == 0)
				{
					MessageBox.Show("PackUOMId field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["GrossWeight"].Value) == 0m)
				{
					MessageBox.Show("GrossWeight field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["NetBillWeight"].Value) == 0m)
				{
					MessageBox.Show("NetBillWeight field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["StockWeight"].Value) == 0m)
				{
					MessageBox.Show("StockWeight field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["Rate"].Value) == 0m)
				{
					MessageBox.Show("Rate field required");
					return;
				}
				if (Conversion.ToInt(r5.Cells["RateUOMId"].Value) == 0)
				{
					MessageBox.Show("RateUom field required");
					return;
				}
				if (Conversion.ToDecimal(r5.Cells["ItemAmount"].Value) == 0m)
				{
					MessageBox.Show("ItemAmount field required");
					return;
				}
				Pd.Id = Conversion.ToInt(r5.Cells["Id"].Value);
				Pd.ActionTypeId = ((Pd.Id <= 0) ? 1 : 2);
				Pd.PurchaseOrderId = Conversion.ToInt(r5.Cells["PurchaseOrderId"].Value);
				Pd.PurchaseOrderDetailId = Conversion.ToInt(r5.Cells["PurchaseOrderDetailId"].Value);
				Pd.WarehouseId = Conversion.ToInt(r5.Cells["WarehouseId"].Value);
				Pd.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
				Pd.CropYearId = Conversion.ToInt(r5.Cells["CropYearId"].Value);
				Pd.JobLotId = Conversion.ToInt(r5.Cells["JobLotId"].Value);
				Pd.PackingTypeId = Conversion.ToInt(r5.Cells["PackingTypeId"].Value);
				Pd.ItemUOMId = Conversion.ToInt(r5.Cells["PackUOMId"].Value);
				Pd.ItemQty = Conversion.ToDecimal(r5.Cells["ItemQty"].Value);
				InvoiceQty += Pd.ItemQty;
				Pd.GrossWeight = Conversion.ToDecimal(r5.Cells["GrossWeight"].Value);
				GrossWt += Pd.GrossWeight;
				Pd.EBWeight = Conversion.ToDecimal(r5.Cells["EmptyBags"].Value);
				Pd.EBTotalWt = Conversion.ToDecimal(r5.Cells["EmptyBagsTotal"].Value);
				Pd.WeightCut = Conversion.ToDecimal(r5.Cells["WeightCut"].Value);
				Pd.WeightCutTotal = Conversion.ToDecimal(r5.Cells["WeightCutTotal"].Value);
				Pd.AdLsWeight = Conversion.ToDecimal(r5.Cells["AddLss"].Value);
				Pd.NetBillWeight = Conversion.ToDecimal(r5.Cells["NetBillWeight"].Value);
				Pd.NetStockWeight = Conversion.ToDecimal(r5.Cells["StockWeight"].Value);
				Pd.ItemRate = Conversion.ToDecimal(r5.Cells["Rate"].Value);
				Pd.RateUomId = Conversion.ToInt(r5.Cells["RateUOMId"].Value);
				Pd.RateUom = Conversion.ToString(r5.Cells["RateUOM"].Value);
				Pd.RateCut = Conversion.ToDecimal(r5.Cells["RateCut"].Value);
				Pd.RateCutAmount = Conversion.ToDecimal(r5.Cells["RateCutTotal"].Value);
				Pd.ItemAmount = Conversion.ToDecimal(r5.Cells["ItemAmount"].Value);
				Pd.LabAnalisysNo = Conversion.ToString(r5.Cells["LabSampleNo"].Value);
				Pd.GpDate = Conversion.ToDateTime(r5.Cells["GpDate"].Value);
				Pd.GpNo = Conversion.ToInt(r5.Cells["GpNo"].Value);
				Pd.VehicleNo = Conversion.ToString(r5.Cells["VehicleNo"].Value);
				Pd.CityId = Conversion.ToInt(r5.Cells["CityId"].Value);
				Pd.TaxNameId = Conversion.ToInt(r5.Cells["TaxId"].Value);
				Pd.TaxPercent = Conversion.ToDecimal(r5.Cells["Tax%"].Value);
				Pd.TaxAmount = Conversion.ToDecimal(r5.Cells["TaxAmount"].Value);
				if (Pd.TaxNameId > 0 && Pd.TaxAmount > 0m)
				{
					Pd.IsTaxable = true;
					purchaseInvoice.IsTaxable = true;
				}
				Pd.ExpenseAmount = Conversion.ToDecimal(r5.Cells["Expense"].Value);
				Pd.CommissionAmount = Conversion.ToDecimal(r5.Cells["Commission"].Value);
				Pd.FreightDeduction = Conversion.ToDecimal(r5.Cells["LessDeduction"].Value);
				Pd.FreightAmount = Conversion.ToDecimal(r5.Cells["Freights"].Value);
				Pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				Pd.FcyAmount = Conversion.ToDecimal(r5.Cells["FcyAmount"].Value);
				Pd.ItemNetAmount = Math.Round(Conversion.ToDecimal(r5.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				Pd.RemarksDetail = Conversion.ToString(r5.Cells["Remarks"].Value);
				purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
			}
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(lstRemoveDetailRecord[j]);
				}
			}
			decimal SupplierWeight = default(decimal);
			decimal FactoryWeight = default(decimal);
			if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory" || ((Control)(object)CmbDeliveryTerm).Text == "Factory PartyWeight")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text.Trim());
				if (GrossWt != SupplierWeight)
				{
					throw new Exception($"Gross Weight {GrossWt}  and Supplier Weight {SupplierWeight} Not Match");
				}
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory FactoryWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Factory FactoryWeight")
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				if (GrossWt != SupplierWeight)
				{
					throw new Exception($"Gross Weight {GrossWt}  and Factory Weight {FactoryWeight} Not Match");
				}
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "Factory")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text.Trim());
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				if (SupplierWeight > FactoryWeight)
				{
					if (GrossWt != FactoryWeight)
					{
						throw new Exception($"Gross Weight {GrossWt}  and Factory Weight {FactoryWeight} Not Match");
					}
				}
				else if (GrossWt != SupplierWeight)
				{
					throw new Exception($"Gross Weight {GrossWt}  and Supplier Weight {SupplierWeight} Not Match");
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["TransporterId"].Value.ToString()) != 0)
				{
					InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
					PF.Id = Conversion.ToInt(r4.Cells["Id"].Value);
					PF.PurchaseOrderId = Conversion.ToInt(r4.Cells["OrderId"].Value);
					PF.PurchaseOrderExpensesChargeToProductId = Conversion.ToInt(r4.Cells["OrderExpenseId"].Value);
					PF.InvGrnId = 0;
					PF.FreightId = 0;
					PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
					PF.SupplierCustomerId = Conversion.ToInt(r4.Cells["TransporterId"].Value);
					PF.Percentage = Conversion.ToDecimal(r4.Cells["Percentage"].Value);
					PF.Qty = Conversion.ToDecimal(r4.Cells["Qty"].Value);
					PF.Rate = Conversion.ToDecimal(r4.Cells["Rate"].Value);
					PF.CreditAmount = Conversion.ToDecimal(r4.Cells["Credit"].Value);
					PF.DebitAmount = Conversion.ToDecimal(r4.Cells["Debit"].Value);
					PF.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
					PE.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					PE.PurchaseOrderId = Conversion.ToInt(r3.Cells["PurchaseOrderId"].Value);
					PE.PurchaseOrderSupplierExpId = Conversion.ToInt(r3.Cells["PurchaseOrderSupplierExpId"].Value);
					PE.InvRevExpItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r3.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ.SupplierCustomerId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					}
					else
					{
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ.SupplierCustomerId = Conversion.ToInt((object)0);
					}
					PJ.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					if (PJ.Remarks == string.Empty)
					{
						PJ.Remarks = Conversion.ToString((object)r2.Cells["AccountId"].Text);
					}
					PJ.Prcnt = Conversion.ToDecimal(r2.Cells["Percentage"].Value);
					PJ.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					PJ.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					PJ.Debit = Conversion.ToDecimal(r2.Cells["Debit"].Value);
					PJ.Credit = Conversion.ToDecimal(r2.Cells["Credit"].Value);
					PJ.InvGrnId = 0;
					PJ.FreightId = 0;
					purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) <= 0)
				{
					continue;
				}
				InvPurchaseInvoiceEmptyBags PEB = new InvPurchaseInvoiceEmptyBags();
				PEB.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
				PEB.TypeId = Conversion.ToInt(r.Cells["Type"].Value);
				if (PEB.TypeId > 0)
				{
					if (PEB.TypeId == 1 || PEB.TypeId == 4 || PEB.TypeId == 5)
					{
						PEB.CreditAccountId = 0;
					}
					else
					{
						PEB.CreditAccountId = Conversion.ToInt(r.Cells["CreditAccountId"].Value);
						if (PEB.CreditAccountId <= 0)
						{
							MessageBox.Show("Credit Account Required in Empty Bag Grid in Row#" + (r.RowIndex + 1), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 3;
							((Control)(object)grdEmptyBags).Focus();
							return;
						}
					}
				}
				PEB.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				PEB.ReceivedQty = Conversion.ToDecimal(r.Cells["ReceivedQty"].Value);
				PEB.PurchaseQty = Conversion.ToDecimal(r.Cells["PurchaseQty"].Value);
				EmptyBagsRecQty += PEB.ReceivedQty;
				EmptyBagsPurQty += PEB.PurchaseQty;
				PEB.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
				PEB.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				if ((PEB.TypeId == 1 || PEB.TypeId == 2 || PEB.TypeId == 3) && PEB.Amount == 0m)
				{
					throw new Exception("Packing Material Amount Required");
				}
				PEB.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				purchaseInvoice.InvPurchaseInvoiceEmptyBagslist.Add(PEB);
			}
			decimal EmpBagsQty = EmptyBagsRecQty + EmptyBagsPurQty;
			if (EmpBagsQty > 0m && EmpBagsQty > InvoiceQty)
			{
				MessageBox.Show("Empty Bags Quantity Not gratter than ItemQty");
				return;
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				purchaseInvoice.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Record Saved Successfully");
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 1104;
				frmwagesBillHeader.RefDocId = sucess;
				frmwagesBillHeader.GrossWeightTotal = Conversion.ToDouble((object)GrossWt);
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			Reset();
			if (ChkBok.Checked)
			{
				RecId = sucess;
				VoucherHeadIdGet();
				RecId = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1104);
			}
			if (ChkPrintSlip.Checked)
			{
				CommonServices.PurchaseInvoicetDirectSlip_Feed1104A(sucess);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(RecId);
			if (invPI == null || invPI.invPurchaseInvoiceDetailList.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			comproject.Value = invPI.ProjectsId;
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			comsupplier.Value = invPI.SupplierCustomerId;
			txtSupplierReference.Text = invPI.SupplierReferenceNo.ToString();
			if (Conversion.ToDouble((object)invPI.FreightDeduction) > 0.0)
			{
				CmbFreightDeduction.Value = invPI.FreightDebitAccountId;
			}
			txtFreightDeduction.Text = Conversion.ToDouble((object)invPI.FreightDeduction).ToString();
			txtbillno.Text = invPI.ManualBillNo;
			combpttrm.Value = Conversion.ToInt((object)invPI.PaymentTermsId);
			duedate.Value = invPI.DueDate;
			txtduedays.Text = Conversion.ToString((object)invPI.DueDays);
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)invPI.DeliveryTerm);
			txtSupplierWeight.Text = Conversion.ToDouble((object)invPI.SupplierWeight).ToString("#,##");
			txtFactoryWeight.Text = Conversion.ToDouble((object)invPI.FactoryWeight).ToString("#,##");
			txtDiffWeight.Text = Conversion.ToDouble((object)invPI.DiffWeight).ToString("#,##");
			if (invPI.CommissionAgentId > 0)
			{
				combcommAgent.Value = invPI.CommissionAgentId;
			}
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			((Control)(object)combcommUOM).Text = Conversion.ToDouble((object)invPI.CommUom).ToString("#,##");
			txtcommrate.Text = Conversion.ToDouble((object)invPI.CommRate).ToString();
			txtcommamount.Text = Conversion.ToDouble((object)invPI.CommAmount).ToString();
			txtCommissionRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			if (invPI.BrokerAgentId > 0)
			{
				CmbBrokeryAc.Value = invPI.BrokerAgentId;
			}
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)invPI.BrokeryType);
			((Control)(object)CmbBrokeryRateUom).Text = invPI.BrokeryUom.ToString("#,##");
			txtBrokeryRate.Text = Conversion.ToDouble((object)invPI.BrokeryRate).ToString();
			txtBrokeryAmount.Text = Conversion.ToDouble((object)invPI.BrokeryAmount).ToString();
			txtBrokeryRemarks.Text = Conversion.ToString((object)invPI.BrokeryRemarks);
			CmbTaxAccount.Value = invPI.TaxAccountId.ToString();
			cmbCurrency.Value = invPI.CurrencyId.ToString();
			txtExchangeRate.Text = invPI.ExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtInvoiceQty.Text = invPI.BillQty.ToString("#,##.##");
			txtInvoiceWeight.Text = invPI.BillWeight.ToString("#,##.##");
			txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			Approved = invPI.IsApproved;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int m = 0; m < invPI.invPurchaseInvoiceDetailList.Count; m++)
			{
				dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[m].Id, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderId, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderDetailId, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[m].WarehouseId, invPI.invPurchaseInvoiceDetailList[m].ItemId, invPI.invPurchaseInvoiceDetailList[m].ItemCode, invPI.invPurchaseInvoiceDetailList[m].ItemName, invPI.invPurchaseInvoiceDetailList[m].CropYearId, invPI.invPurchaseInvoiceDetailList[m].JobLotId, invPI.invPurchaseInvoiceDetailList[m].PackingTypeId, invPI.invPurchaseInvoiceDetailList[m].ItemUOMId, invPI.invPurchaseInvoiceDetailList[m].ItemUom, invPI.invPurchaseInvoiceDetailList[m].ItemEquivalent, invPI.invPurchaseInvoiceDetailList[m].ItemQty, invPI.invPurchaseInvoiceDetailList[m].GrossWeight, invPI.invPurchaseInvoiceDetailList[m].EBWeight, Math.Round(invPI.invPurchaseInvoiceDetailList[m].EBTotalWt), invPI.invPurchaseInvoiceDetailList[m].WeightCut, Math.Round(invPI.invPurchaseInvoiceDetailList[m].WeightCutTotal), invPI.invPurchaseInvoiceDetailList[m].AdLsWeight, invPI.invPurchaseInvoiceDetailList[m].NetBillWeight, invPI.invPurchaseInvoiceDetailList[m].NetStockWeight, invPI.invPurchaseInvoiceDetailList[m].ItemRate, invPI.invPurchaseInvoiceDetailList[m].RateUomId, invPI.invPurchaseInvoiceDetailList[m].RateUom, invPI.invPurchaseInvoiceDetailList[m].RateEquivalent, invPI.invPurchaseInvoiceDetailList[m].RateCut, invPI.invPurchaseInvoiceDetailList[m].RateCutAmount, invPI.invPurchaseInvoiceDetailList[m].ItemAmount, invPI.invPurchaseInvoiceDetailList[m].TaxNameId, invPI.invPurchaseInvoiceDetailList[m].TaxPercent, invPI.invPurchaseInvoiceDetailList[m].TaxAmount, invPI.invPurchaseInvoiceDetailList[m].LabAnalisysNo, invPI.invPurchaseInvoiceDetailList[m].GpDate, invPI.invPurchaseInvoiceDetailList[m].GpNo, invPI.invPurchaseInvoiceDetailList[m].VehicleNo, invPI.invPurchaseInvoiceDetailList[m].CityId, invPI.invPurchaseInvoiceDetailList[m].RemarksDetail, invPI.invPurchaseInvoiceDetailList[m].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[m].FreightAmount, invPI.invPurchaseInvoiceDetailList[m].CommissionAmount, invPI.invPurchaseInvoiceDetailList[m].FreightDeduction, invPI.invPurchaseInvoiceDetailList[m].ItemNetAmount, invPI.invPurchaseInvoiceDetailList[m].FcyAmount);
				if (invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderId > 0)
				{
					OrderExist = invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderId > 0;
					((Control)(object)comsupplier).Enabled = false;
					((Control)(object)CmbDeliveryTerm).Enabled = false;
					((Control)(object)combpttrm).Enabled = false;
				}
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Rows.Clear();
			for (int l = 0; l < invPI.InvPurchaseInvoiceExpList.Count; l++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[l].Id, invPI.InvPurchaseInvoiceExpList[l].PurchaseOrderId, invPI.InvPurchaseInvoiceExpList[l].PurchaseOrderSupplierExpId, invPI.InvPurchaseInvoiceExpList[l].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[l].Qty, invPI.InvPurchaseInvoiceExpList[l].Rate, invPI.InvPurchaseInvoiceExpList[l].Amount, invPI.InvPurchaseInvoiceExpList[l].Remarks);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Rows.Clear();
			for (int k = 0; k < invPI.InvPurchaseInvoicejournalList.Count; k++)
			{
				dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invPI.InvPurchaseInvoicejournalList[k].SupplierCustomerId : invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[k].Remarks, invPI.InvPurchaseInvoicejournalList[k].Prcnt, invPI.InvPurchaseInvoicejournalList[k].Qty, invPI.InvPurchaseInvoicejournalList[k].Rate, invPI.InvPurchaseInvoicejournalList[k].Debit, invPI.InvPurchaseInvoicejournalList[k].Credit, invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId);
			}
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtFreight.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoiceFreightList.Count; j++)
			{
				dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[j].Id, invPI.InvPurchaseInvoiceFreightList[j].PurchaseOrderId, invPI.InvPurchaseInvoiceFreightList[j].PurchaseOrderExpensesChargeToProductId, SubsidiaryAccountAllownOnVouchers ? invPI.InvPurchaseInvoiceFreightList[j].SupplierCustomerId : invPI.InvPurchaseInvoiceFreightList[j].TansporterId, invPI.InvPurchaseInvoiceFreightList[j].Percentage, invPI.InvPurchaseInvoiceFreightList[j].Qty, invPI.InvPurchaseInvoiceFreightList[j].Rate, invPI.InvPurchaseInvoiceFreightList[j].CreditAmount, invPI.InvPurchaseInvoiceFreightList[j].DebitAmount, invPI.InvPurchaseInvoiceFreightList[j].Remarks, invPI.InvPurchaseInvoiceFreightList[j].TansporterId);
			}
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtEmptyBags.Rows.Clear();
			for (int i = 0; i < invPI.InvPurchaseInvoiceEmptyBagslist.Count; i++)
			{
				dtEmptyBags.Rows.Add(invPI.InvPurchaseInvoiceEmptyBagslist[i].PurchaseOrderId, invPI.InvPurchaseInvoiceEmptyBagslist[i].TypeId, invPI.InvPurchaseInvoiceEmptyBagslist[i].ItemId, invPI.InvPurchaseInvoiceEmptyBagslist[i].ReceivedQty, invPI.InvPurchaseInvoiceEmptyBagslist[i].PurchaseQty, invPI.InvPurchaseInvoiceEmptyBagslist[i].Rate, invPI.InvPurchaseInvoiceEmptyBagslist[i].Amount, invPI.InvPurchaseInvoiceEmptyBagslist[i].Remarks, invPI.InvPurchaseInvoiceEmptyBagslist[i].CreditAccountId);
				EmptyBagsWeightCutStatus = invPI.InvPurchaseInvoiceEmptyBagslist[i].TypeId;
			}
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			TotalBrokeryAmount();
			ExpProportion();
			CommissionProportion();
			FreightProportion();
			BillProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId == 0)
			{
				throw new Exception("No Record Found");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvPurchaseInvoice.DeleteByID(UserAccount.ID, RecId);
				MessageBox.Show("Delete Invoice Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveDetailRecord.Clear();
			RecId = 0;
			VoucherHeadId = 0;
			Approved = false;
			txtbillno.Text = string.Empty;
			((Control)(object)comsupplier).Text = string.Empty;
			txtSupplierReference.Text = string.Empty;
			txtduedays.Text = "0";
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
			txtremarks.Text = string.Empty;
			((Control)(object)combcommAgent).Text = string.Empty;
			((Control)(object)combCommType).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			txtBrokeryRemarks.Text = string.Empty;
			txtSupplierWeight.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
			txtDiffWeight.Text = string.Empty;
			txtFreightDeduction.Text = string.Empty;
			updateDetailIndex = -1;
			((Control)(object)comWarehouse).Text = string.Empty;
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)comjobLot).Text = string.Empty;
			((Control)(object)comPackingType).Text = string.Empty;
			((Control)(object)comPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEmptybagsUnit.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtwtcut.Text = string.Empty;
			txtWeightCutTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtratecut.Text = string.Empty;
			txtratecuttotal.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			txtlabanalysisno.Text = string.Empty;
			txtgatepassno.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			dtGrid.Rows.Clear();
			dtGrdGL.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Rows.Clear();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Rows.Clear();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtEmptyBags.Rows.Clear();
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)comsupplier).Enabled = true;
			((Control)(object)combpttrm).Enabled = true;
			((Control)(object)CmbDeliveryTerm).Enabled = true;
			txtRate.Enabled = true;
			((Control)(object)CmbDeliveryTerm).Enabled = true;
			DocDate.Focus();
			Configurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)comItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEmptybagsUnit.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtwtcut.Text = string.Empty;
			txtWeightCutTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtratecut.Text = string.Empty;
			txtratecuttotal.Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			comWarehouse.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 1104);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			AccountsFill();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			bindWareHouse();
			suppliercustomer();
			item();
			Project();
			PackingType();
			cropyear();
			TaxAccountFill();
			FreightDeductionAccountFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ExpProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			decimal TotalQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Qty = Conversion.ToDecimal(obj.Cells["ItemQty"].Value);
					obj.Cells["Expense"].Value = Math.Round(TotalAmount / TotalQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			if (!FreightDebitToExpenses)
			{
				decimal NetWeight = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
				decimal CreditForFreight = Conversion.ToDecimal((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Credit"], (AggregateFunction)2).ToString());
				CreditForFreight = Math.Round(CreditForFreight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (CreditForFreight > 0m && NetWeight > 0m)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						decimal Weight = Conversion.ToDecimal((object)obj.Cells["NetBillWeight"].Value.ToString());
						obj.Cells["Freights"].Value = CreditForFreight / NetWeight * Weight;
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Freights"].Value = 0;
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["Freights"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			decimal CommPercent = Conversion.ToDecimal((object)txtcommrate.Text);
			decimal NetWeight = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString()) : 0m);
			if (TotalAmount > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)combCommType).Text == "Percent")
					{
						decimal ItemAmount = Conversion.ToDecimal((object)r.Cells["ItemAmount"].Value.ToString());
						r.Cells["Commission"].Value = Math.Round(ItemAmount * CommPercent / 100m, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						r.EndEdit();
					}
					else
					{
						decimal Weight = Conversion.ToDecimal((object)r.Cells["NetBillWeight"].Value.ToString());
						r.Cells["Commission"].Value = Math.Round(TotalAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						r.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightDeductionProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtFreightDeduction.Text);
			decimal NetWeight = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString()) : 0m);
			if (TotalAmount > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Weight = Conversion.ToDecimal((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["LessDeduction"].Value = Math.Round(TotalAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["LessDeduction"].Value = 0;
					obj2.EndEdit();
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal BillAmount = default(decimal);
				decimal ItemAmount = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString()) : 0m);
				decimal TaxAmount = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString()) : 0m);
				decimal TotalQty = ((grd.RowCount > 0) ? Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2)) : 0m);
				decimal TotalWeight = ((grd.RowCount > 0) ? Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2)) : 0m);
				decimal ExpAmount = ((grdInvExp.RowCount > 0) ? Conversion.ToDecimal((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) : 0m);
				decimal EmptyBagsAmount = ((grdEmptyBags.RowCount > 0) ? Conversion.ToDecimal((object)grdEmptyBags.GetTotal(grdEmptyBags.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) : 0m);
				decimal JournalDebit = default(decimal);
				decimal JournalCredit = default(decimal);
				decimal TransporterCredit = default(decimal);
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (int.Parse(r2.Cells["AccountId"].Value.ToString()) > 0)
					{
						if (txtSupplierGLId.Text.ToString() == r2.Cells["AccountId"].Value.ToString())
						{
							r2.BeginEdit();
							r2.Cells["AccountId"].Value = 0.ToString();
							throw new Exception("You cannot select supplier Account");
						}
						JournalDebit += Conversion.ToDecimal((object)r2.Cells["Debit"].Value.ToString());
						JournalCredit += Conversion.ToDecimal((object)r2.Cells["Credit"].Value.ToString());
					}
				}
				rows = grdFreight.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["TransporterId"].Value) > 0 && txtSupplierGLId.Text.ToString() == r.Cells["TransporterId"].Value.ToString())
					{
						TransporterCredit += Conversion.ToDecimal((object)r.Cells["Credit"].Value.ToString());
					}
				}
				ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				TaxAmount = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				ExpAmount = Math.Round(ExpAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				EmptyBagsAmount = Math.Round(EmptyBagsAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalDebit = Math.Round(JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalCredit = Math.Round(JournalCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				TransporterCredit = Math.Round(TransporterCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				BillAmount = ItemAmount + TaxAmount + ExpAmount + EmptyBagsAmount;
				BillAmount = BillAmount + JournalDebit - JournalCredit;
				BillAmount += TransporterCredit;
				if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
				{
					BillAmount += Conversion.ToDecimal((object)txtcommamount.Text.Trim());
					SupplierCommissionAmount = Conversion.ToDecimal((object)txtcommamount.Text.Trim());
				}
				else
				{
					SupplierCommissionAmount = default(decimal);
				}
				if (((UltraGridBase)CmbBrokeryAc).ActiveRow != null)
				{
					if (Conversion.ToDecimal((object)txtBrokeryAmount.Text) > 0m)
					{
						BillAmount -= Conversion.ToDecimal((object)txtBrokeryAmount.Text.Trim());
					}
					else
					{
						txtBrokeryAmount.Text = "0";
					}
				}
				else
				{
					txtBrokeryAmount.Text = "0";
				}
				txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				BillProportion();
				txtInvoiceQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtInvoiceWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m && ItemAmount > 0m)
				{
					txtFcyAmount.Text = (BillAmount / Conversion.ToDecimal((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
				else
				{
					txtFcyAmount.Text = 0.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
			else
			{
				txtInvoiceQty.Text = "0";
				txtInvoiceWeight.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				decimal ItemAmount = Math.Round(Conversion.ToDecimal(obj.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				decimal Expense = Math.Round(Conversion.ToDecimal(obj.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				decimal Freights = Math.Round(Conversion.ToDecimal(obj.Cells["Freights"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				decimal Commission = Math.Round(Conversion.ToDecimal(obj.Cells["Commission"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				decimal LessDeduction = Math.Round(Conversion.ToDecimal(obj.Cells["LessDeduction"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				decimal TaxAmount = Math.Round(Conversion.ToDecimal(obj.Cells["TaxAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.BeginEdit();
				obj.Cells["ItemNetAmount"].Value = Math.Round(ItemAmount + TaxAmount + Expense + Freights + Commission - LessDeduction, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)combCommType).Text == "Flat")
				{
					decimal CommRate = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					txtcommamount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				if (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage")
				{
					decimal CommRate3 = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						decimal CommAmt2 = Conversion.ToDecimal((object)total.ToString()) * CommRate3 / 100m;
						txtcommamount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
				if (((Control)(object)combCommType).Text == "Comm Weight")
				{
					decimal CommRate2 = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						decimal num = Conversion.ToDecimal((object)Weight.ToString());
						decimal UOM = Conversion.ToDecimal((object)((Control)(object)combcommUOM).Text.Trim());
						decimal CommAmt = num / UOM * CommRate2;
						txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		CommissionProportion();
	}

	private void combcommAgent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalBrokeryAmount()
	{
		try
		{
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Flat")
				{
					decimal CommRate = Conversion.ToDecimal((object)txtBrokeryRate.Text.Trim());
					txtBrokeryAmount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Percent")
				{
					decimal CommRate3 = Conversion.ToDecimal((object)txtBrokeryRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						decimal CommAmt2 = Conversion.ToDecimal((object)total.ToString()) * CommRate3 / 100m;
						txtBrokeryAmount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Comm Weight")
				{
					decimal CommRate2 = Conversion.ToDecimal((object)txtBrokeryRate.Text.Trim());
					double Weight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2));
					if (Weight > 0.0 && Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text) > 0.0)
					{
						decimal num = Conversion.ToDecimal((object)Weight.ToString());
						decimal UOM = Conversion.ToDecimal((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
						decimal CommAmt = num / UOM * CommRate2;
						txtBrokeryAmount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryAc_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryType_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBrokeryRate_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryRateUom_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		txtvehicleno.Text = txtvehicleno.Text.ToUpper();
	}

	private void GetEmptyBagsInformationFromOrder(int PurchaseOrderId)
	{
		try
		{
			DataTable dtPOEmptyBagsDetail = PurchaseOrder.GetPurchaseOrderEmptyBagsDetailByOrderId(PurchaseOrderId, "");
			if (dtPOEmptyBagsDetail.Rows.Count <= 0)
			{
				return;
			}
			dtEmptyBags.Rows.Clear();
			for (int i = 0; i < dtPOEmptyBagsDetail.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["Type"]) == 1 && Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["PackingTypeId"]) == Conversion.ToInt(comPackingType.Value))
				{
					txtEmptybagsUnit.Text = Conversion.ToString(dtPOEmptyBagsDetail.Rows[i]["WeightCut"]);
					EmptyBagsWeightCutStatus = 1;
				}
				if (Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["Type"]) == 2 && Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["PackingTypeId"]) == Conversion.ToInt(comPackingType.Value))
				{
					txtEmptybagsUnit.Text = Conversion.ToString(dtPOEmptyBagsDetail.Rows[i]["WeightCut"]);
					EmptyBagsWeightCutStatus = 2;
				}
				if (Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["Type"]) == 3 && Conversion.ToInt(dtPOEmptyBagsDetail.Rows[i]["PackingTypeId"]) == Conversion.ToInt(comPackingType.Value))
				{
					txtEmptybagsUnit.Text = Conversion.ToString(dtPOEmptyBagsDetail.Rows[i]["WeightCut"]);
					EmptyBagsWeightCutStatus = 3;
				}
				dtEmptyBags.Rows.Add(dtPOEmptyBagsDetail.Rows[i]["PurchaseOrderId"], dtPOEmptyBagsDetail.Rows[i]["Type"], dtPOEmptyBagsDetail.Rows[i]["ItemId"], 0, 0, dtPOEmptyBagsDetail.Rows[i]["Rate"], 0, "", 0);
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
	}

	private void Total()
	{
		try
		{
			decimal FactoryWeight = default(decimal);
			decimal SupplierWeight = default(decimal);
			decimal Qty = default(decimal);
			decimal GrossWeight = default(decimal);
			decimal EBUnit = default(decimal);
			decimal EBTotal = default(decimal);
			decimal WeightCut = Conversion.ToDecimal((object)txtwtcut.Text);
			decimal WeightCutTotal = Conversion.ToDecimal((object)txtWeightCutTotal.Text);
			decimal AddLess = default(decimal);
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text);
			}
			if (Conversion.ToDouble((object)txtSupplierWeight.Text) > 0.0)
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
			}
			if (((UltraGridBase)comPackUOM).ActiveRow != null && Conversion.ToInt(comPackUOM.Value) > 0)
			{
				Conversion.ToDecimal(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (Conversion.ToDecimal((object)txtQty.Text) != 0m)
			{
				Qty = Conversion.ToDecimal((object)txtQty.Text);
			}
			if (Conversion.ToDecimal((object)txtGrossWeight.Text) != 0m)
			{
				GrossWeight = Conversion.ToDecimal((object)txtGrossWeight.Text);
			}
			if (Conversion.ToDecimal((object)txtEmptybagsUnit.Text) != 0m)
			{
				EBUnit = Conversion.ToDecimal((object)txtEmptybagsUnit.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmUnit")
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.###");
			}
			if (Conversion.ToDecimal((object)txtEmptyBagsTotal.Text) != 0m)
			{
				EBTotal = Conversion.ToDecimal((object)txtEmptyBagsTotal.Text);
			}
			if (Conversion.ToDecimal((object)txtNetBillWeight.Text) != 0m)
			{
				Conversion.ToDecimal((object)txtNetBillWeight.Text);
			}
			if (Conversion.ToDecimal((object)txtStockWeight.Text) != 0m)
			{
				Conversion.ToDecimal((object)txtStockWeight.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "WtCut")
			{
				WeightCutTotal = Qty * WeightCut;
				txtWeightCutTotal.Text = WeightCutTotal.ToString("#,##0.###");
			}
			WeightCutTotal = Conversion.ToDecimal((object)txtWeightCutTotal.Text);
			AddLess = ((!(Conversion.ToDecimal((object)txtAddLss.Text) != 0m)) ? default(decimal) : Conversion.ToDecimal((object)txtAddLss.Text));
			decimal Weight = default(decimal);
			Weight = ((EmptyBagsWeightCutStatus != 2) ? (Math.Round(GrossWeight, 2) - Math.Round(WeightCutTotal, 2) + AddLess) : (Math.Round(GrossWeight, 2) - Math.Round(WeightCutTotal, 2) + AddLess));
			txtNetBillWeight.Text = Math.Round(Weight, 2).ToString("#,##0.###");
			if (!(SupplierWeight > 0m) || !(FactoryWeight > 0m) || !(GrossWeight > 0m))
			{
				return;
			}
			if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory" || ((Control)(object)CmbDeliveryTerm).Text == "Factory PartyWeight")
			{
				decimal StockWt = FactoryWeight * GrossWeight / SupplierWeight;
				StockWt = StockWt - EBTotal - WeightCutTotal;
				txtStockWeight.Text = Math.Round(StockWt, 2).ToString();
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory FactoryWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Factory FactoryWeight")
			{
				decimal StockWt2 = FactoryWeight * GrossWeight / FactoryWeight;
				StockWt2 = StockWt2 - EBTotal - WeightCutTotal;
				txtStockWeight.Text = Math.Round(StockWt2, 2).ToString();
			}
			else if (((Control)(object)CmbDeliveryTerm).Text == "Factory")
			{
				if (SupplierWeight > FactoryWeight)
				{
					decimal StockWt3 = FactoryWeight * GrossWeight / FactoryWeight;
					StockWt3 = StockWt3 - EBTotal - WeightCutTotal;
					txtStockWeight.Text = Math.Round(StockWt3, 2).ToString();
				}
				else
				{
					decimal StockWt4 = FactoryWeight * GrossWeight / SupplierWeight;
					StockWt4 = StockWt4 - EBTotal - WeightCutTotal;
					txtStockWeight.Text = Math.Round(StockWt4, 2).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCaluculation()
	{
		try
		{
			decimal RateUOM = default(decimal);
			decimal Rate = default(decimal);
			decimal RateCut = default(decimal);
			decimal NetWeight = default(decimal);
			decimal ItemAmount = default(decimal);
			decimal RateCutTotal = default(decimal);
			if (((UltraGridBase)comRateUOM).ActiveRow != null)
			{
				RateUOM = Conversion.ToDecimal(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
			}
			if (Conversion.ToDecimal((object)txtRate.Text) != 0m)
			{
				Rate = Conversion.ToDecimal((object)txtRate.Text);
			}
			if (Conversion.ToDecimal((object)txtratecut.Text) != 0m)
			{
				RateCut = Conversion.ToDecimal((object)txtratecut.Text);
			}
			if (Conversion.ToDecimal((object)txtNetBillWeight.Text) != 0m)
			{
				NetWeight = Conversion.ToDecimal((object)txtNetBillWeight.Text);
			}
			if (NetWeight > 0m && RateUOM > 0m && Rate > 0m)
			{
				ItemAmount = Math.Round(NetWeight / RateUOM * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				RateCutTotal = Math.Round(NetWeight / RateUOM * RateCut, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtratecuttotal.Text = RateCutTotal.ToString(clsGlobalVariables.stringFormatsingle);
				ItemAmount -= RateCutTotal;
				txtAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = "0";
				txtTotalAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateTaxAmount()
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					txtTaxPercent.Text = TaxPercent.ToString();
				}
				double num = Conversion.ToDouble((object)txtAmount.Text);
				double TaxAmt = num * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = num + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptybagsUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtStockWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtwtcut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLss_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratecut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AmountCaluculation();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbtaxtype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
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
			WF.Show(this);
			GetAll();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1104;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("CommType", typeof(string));
				dt.Columns.Add("CommRate", typeof(decimal));
				dt.Columns.Add("CommAmount", typeof(decimal));
				dt.Columns.Add("CommRemarks", typeof(string));
				dt.Columns.Add("BillAmount", typeof(decimal));
				dt.Columns.Add("ApprovedStatus", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("Remarks", typeof(string));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["DocumentTypeId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], dthistory.Rows[i]["CommAmount"], dthistory.Rows[i]["CommissionRemarks"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["ApprovedStatus"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUser"], dthistory.Rows[i]["ApprovedDate"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["CommissionAgent"].Caption = "CommAgent";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("Slip");
			grdHistory.RootTable.Columns["Slip"].Caption = "Slip";
			grdHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdHistory.RootTable.Columns["Slip"].Position = 1;
			grdHistory.RootTable.Columns["Slip"].Width = 40;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Position = 2;
			grdHistory.RootTable.Columns["PartySlip"].Width = 70;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1104);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int currid = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = currid;
					ReadById(RecId);
				}
				if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(currid, 1104), 1104);
				}
				if (e.Column.Key == "Slip")
				{
					CommonServices.PurchaseInvoicetDirectSlip_Feed1104(currid);
				}
				if (e.Column.Key == "PartySlip")
				{
					CommonServices.PurchaseInvoicetDirectSlip_Feed1104A(currid);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_doubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = Conversion.ToInt((object)id);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				GetDetailGrdByHeadId(id);
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Id);
			if (invPI != null && invPI.invPurchaseInvoiceDetailList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("PurchaseOrderNo", typeof(int));
				dt.Columns.Add("Warehouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("PackUOM");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("EmptyBags", typeof(double));
				dt.Columns.Add("EmptyBagsTotal", typeof(double));
				dt.Columns.Add("WeightCut", typeof(double));
				dt.Columns.Add("WeightCutTotal", typeof(double));
				dt.Columns.Add("AddLss", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("RateUOM");
				dt.Columns.Add("RateCut", typeof(double));
				dt.Columns.Add("RateCutTotal", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("TaxName", typeof(string));
				dt.Columns.Add("Tax%", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("LabSampleNo");
				dt.Columns.Add("GpDate");
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("Expense", typeof(double));
				dt.Columns.Add("Freights", typeof(double));
				dt.Columns.Add("Journal", typeof(double));
				dt.Columns.Add("Commission", typeof(double));
				dt.Columns.Add("ItemNetAmount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Rows.Clear();
				for (int i = 0; i < invPI.invPurchaseInvoiceDetailList.Count; i++)
				{
					dt.Rows.Add(invPI.invPurchaseInvoiceDetailList[i].Id, invPI.invPurchaseInvoiceDetailList[i].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[i].WareHouseName, invPI.invPurchaseInvoiceDetailList[i].ItemId, invPI.invPurchaseInvoiceDetailList[i].ItemCode, invPI.invPurchaseInvoiceDetailList[i].ItemName, invPI.invPurchaseInvoiceDetailList[i].CropYear, invPI.invPurchaseInvoiceDetailList[i].JobLotDescription, invPI.invPurchaseInvoiceDetailList[i].PackTypeDesc, invPI.invPurchaseInvoiceDetailList[i].ItemUom, invPI.invPurchaseInvoiceDetailList[i].ItemQty, invPI.invPurchaseInvoiceDetailList[i].GrossWeight, invPI.invPurchaseInvoiceDetailList[i].EBWeight, Math.Round(invPI.invPurchaseInvoiceDetailList[i].EBTotalWt), invPI.invPurchaseInvoiceDetailList[i].WeightCut, Math.Round(invPI.invPurchaseInvoiceDetailList[i].WeightCutTotal), invPI.invPurchaseInvoiceDetailList[i].AdLsWeight, invPI.invPurchaseInvoiceDetailList[i].NetBillWeight, invPI.invPurchaseInvoiceDetailList[i].NetStockWeight, invPI.invPurchaseInvoiceDetailList[i].ItemRate, invPI.invPurchaseInvoiceDetailList[i].RateUom, invPI.invPurchaseInvoiceDetailList[i].RateCut, invPI.invPurchaseInvoiceDetailList[i].RateCutAmount, invPI.invPurchaseInvoiceDetailList[i].ItemAmount, invPI.invPurchaseInvoiceDetailList[i].TaxName, invPI.invPurchaseInvoiceDetailList[i].TaxPercent, invPI.invPurchaseInvoiceDetailList[i].TaxAmount, invPI.invPurchaseInvoiceDetailList[i].LabAnalisysNo, invPI.invPurchaseInvoiceDetailList[i].GpDate, invPI.invPurchaseInvoiceDetailList[i].GpNo, invPI.invPurchaseInvoiceDetailList[i].VehicleNo, invPI.invPurchaseInvoiceDetailList[i].CityName, invPI.invPurchaseInvoiceDetailList[i].RemarksDetail, invPI.invPurchaseInvoiceDetailList[i].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[i].FreightAmount, invPI.invPurchaseInvoiceDetailList[i].CommissionAmount, invPI.invPurchaseInvoiceDetailList[i].ItemNetAmount, invPI.invPurchaseInvoiceDetailList[i].FcyAmount);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PurchaseOrderNo"].Visible = false;
			grdDetail.RootTable.Columns["Freights"].Caption = "ChargeToProduct";
			grdDetail.RootTable.Columns["PurchaseOrderNo"].Caption = "OrderNo";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EmptyBags"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EmptyBags"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EmptyBags"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EmptyBags"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EmptyBagsTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EmptyBagsTotal"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EmptyBagsTotal"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLss"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLss"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLss"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLss"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseOrderLoader_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow r = grd.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) == 0)
				{
					MessageBox.Show("You cannot Load PurchaseOrder because record without Order already Exists");
					return;
				}
			}
			LoadPO = new LoadPurchaseOrderFeed(UserAccount);
			LoadPO.ShowDialog();
			LoadPO.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadPO.dtLoader;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			string OrderIds = "";
			tabControl2.SelectedIndex = 0;
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]) > 0)
				{
					OrderIds = Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]).ToString();
					break;
				}
			}
			if (grd.RowCount > 0)
			{
				if (Conversion.ToInt(comsupplier.Value) > 0 && Conversion.ToInt(dtpurchaseinvoice.Rows[0]["SupplierCustomerId"]) != Conversion.ToInt(comsupplier.Value))
				{
					MessageBox.Show("Already Loaded Rows Have Different Supplier. So you Can't Load Rows Of Different Supplier!");
					return;
				}
				if (Conversion.ToInt(CmbDeliveryTerm.Value) > 0 && Conversion.ToString(dtpurchaseinvoice.Rows[0]["DeliveryTerm"]) != ((Control)(object)CmbDeliveryTerm).Text)
				{
					MessageBox.Show("Already Loaded Rows Have Different Delivery Term. So you Can't Load Rows Of Different Delivery Term!");
					return;
				}
			}
			comsupplier.Value = Conversion.ToInt((object)dtpurchaseinvoice.Rows[0]["SupplierCustomerId"].ToString());
			((Control)(object)comsupplier).Enabled = false;
			combpttrm.Value = Conversion.ToInt((object)dtpurchaseinvoice.Rows[0]["PaymentTermsId"].ToString());
			txtduedays.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["OrderDueDays"].ToString());
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["DeliveryTerm"].ToString());
			((Control)(object)CmbDeliveryTerm).Enabled = false;
			txtremarks.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["RemarksHeader"].ToString());
			if (Conversion.ToInt(dtpurchaseinvoice.Rows[0]["CommissionAgentId"]) > 0)
			{
				((Control)(object)combcommAgent).Enabled = false;
				combcommAgent.Value = Conversion.ToInt(dtpurchaseinvoice.Rows[0]["CommissionAgentId"]);
			}
			else
			{
				((Control)(object)combcommAgent).Enabled = true;
			}
			if (Conversion.ToString(dtpurchaseinvoice.Rows[0]["CommissionType"]) != null && Conversion.ToString(dtpurchaseinvoice.Rows[0]["CommissionType"]) != string.Empty)
			{
				((Control)(object)combCommType).Enabled = false;
				((Control)(object)combCommType).Text = dtpurchaseinvoice.Rows[0]["CommissionType"].ToString();
			}
			else
			{
				((Control)(object)combCommType).Enabled = true;
			}
			if (Conversion.ToDouble(dtpurchaseinvoice.Rows[0]["CommRate"]) > 0.0)
			{
				txtcommrate.Enabled = false;
				txtcommrate.Clear();
				txtcommrate.Text = dtpurchaseinvoice.Rows[0]["CommRate"].ToString();
			}
			else
			{
				txtcommrate.Enabled = true;
			}
			txtCommissionRemarks.Text = dtpurchaseinvoice.Rows[0]["CommissionRemarks"].ToString();
			txtInvoiceQty.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["OrderQty"].ToString());
			txtInvoiceWeight.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["OrderWeight"].ToString());
			cmbCurrency.Value = Conversion.ToInt((object)dtpurchaseinvoice.Rows[0]["CurrencyId"].ToString());
			LoadDataDetailfromPurchaseInvoivce(dtpurchaseinvoice);
			txtExchangeRate.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["ExchangeRate"].ToString());
			LoadExpData(OrderIds);
			LoadPurchaseOrderExpensesChargToProductData(OrderIds);
			TotalCommissionAmount();
			FreightProportion();
			BillProportion();
			BillAmount();
			OrderIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce(DataTable dtpurchaseinvoice)
	{
		try
		{
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["PurchaseOrderId"].Value) != Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Detail");
							return;
						}
						if (Conversion.ToInt(item.Cells["PurchaseOrderDetailId"].Value) == Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					double TaxAmount = 0.0;
					double ItemNetAmount = 0.0;
					double ItemAmount = Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["BalanceWeight"]) / Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["RateUomEquivalent"]) * Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemRate"]);
					if (Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["TaxPercent"]) > 0.0)
					{
						TaxAmount = ItemAmount * Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["TaxPercent"]) / 100.0;
					}
					ItemNetAmount = ItemAmount + TaxAmount;
					dtGrid.Rows.Add(0, dtpurchaseinvoice.Rows[i]["Id"], dtpurchaseinvoice.Rows[i]["DetailId"], dtpurchaseinvoice.Rows[i]["DocNo"], 0, dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemCodeNew"], dtpurchaseinvoice.Rows[i]["ItemName"], 0, dtpurchaseinvoice.Rows[i]["JobLotId"], 0, dtpurchaseinvoice.Rows[i]["ItemUomId"], dtpurchaseinvoice.Rows[i]["ItemUom"], dtpurchaseinvoice.Rows[i]["ItemUomEquivalent"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], 0, 0, 0, 0, 0, dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUomId"], dtpurchaseinvoice.Rows[i]["RateUom"], dtpurchaseinvoice.Rows[i]["RateUomEquivalent"], 0, 0, ItemAmount, dtpurchaseinvoice.Rows[i]["TaxNameId"], dtpurchaseinvoice.Rows[i]["TaxPercent"], TaxAmount, 0, DateTime.Now, 0, "", dtpurchaseinvoice.Rows[i]["CityId"], dtpurchaseinvoice.Rows[i]["RemarksDetail"], 0, 0, 0, 0, ItemNetAmount, 0);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			CommissionProportion();
			FreightProportion();
			BillProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadExpData(string IDs)
	{
		try
		{
			DataTable item = PurchaseOrder.PurchaseOrderSupplierExpenseByOrderIds(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				bool flag = false;
				if (grdInvExp.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdInvExp.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) > 0 && Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) != Conversion.ToInt(item.Rows[i]["PurchaseOrderId"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Expense Grid");
							return;
						}
						if (Conversion.ToInt(r.Cells["PurchaseOrderSupplierExpId"].Value) == Conversion.ToInt(item.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtInvExp.Rows.Add(0, item.Rows[i]["PurchaseOrderId"], item.Rows[i]["Id"], item.Rows[i]["InvRevExpItemId"], 0, Conversion.ToDecimal(item.Rows[i]["Rate"]), 0, "");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadPurchaseOrderExpensesChargToProductData(string IDs)
	{
		try
		{
			string DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
			DataTable item = PurchaseOrder.PurchaseOrderExpensesChargeToProductDetailByOrderIds(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				bool flag = false;
				if (grdFreight.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdFreight.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["OrderId"].Value) > 0 && Conversion.ToInt(r.Cells["OrderId"].Value) != Conversion.ToInt(item.Rows[i]["PurchaseOrderId"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Expense Grid");
							return;
						}
						if (Conversion.ToInt(r.Cells["OrderExpenseId"].Value) == Conversion.ToInt(item.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					continue;
				}
				if (SubsidiaryAccountAllownOnVouchers)
				{
					switch (DeliveryTerm)
					{
					case "Load":
					case "Load & PartyWeight":
					case "Load & FactoryWeight":
						dtFreight.Rows.Add(0, item.Rows[i]["PurchaseOrderId"], item.Rows[i]["Id"], item.Rows[i]["SupplierCustomerId"], item.Rows[i]["Percentage"], item.Rows[i]["Qty"], item.Rows[i]["Rate"], item.Rows[i]["Amount"], 0, item.Rows[i]["Remarks"], item.Rows[i]["AccountId"]);
						break;
					}
				}
				else
				{
					switch (DeliveryTerm)
					{
					case "Load":
					case "Load & PartyWeight":
					case "Load & FactoryWeight":
						dtFreight.Rows.Add(0, item.Rows[i]["PurchaseOrderId"], item.Rows[i]["Id"], item.Rows[i]["AccountId"], item.Rows[i]["Percentage"], item.Rows[i]["Qty"], item.Rows[i]["Rate"], item.Rows[i]["Amount"], 0, item.Rows[i]["Remarks"], 0);
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1104);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoicetDirectSlip_Feed1104(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn225PartySlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoicetDirectSlip_Feed1104A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptybagsUnit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtStockWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratecut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBrokeryRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtAddLss_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeightCutTotal_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeightCutTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptyBagsTotal_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				duedate.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDaysCalculate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void duedate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (duedate.Value.Date < DocDate.Value.Date)
			{
				duedate.Value = DocDate.Value;
			}
			int days = (int)(duedate.Value.Date - DocDate.Value.Date).TotalDays;
			txtduedays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PurchaseInvoiceDirect_KeyDown_1(object sender, KeyEventArgs e)
	{
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						BtnSave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				btnSlip_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				comItem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				tabControl2.SelectedIndex = 0;
				((Control)(object)grdFreight).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdInvExp).Focus();
					return;
				}
				if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
					((Control)(object)grdGLedger).Focus();
					return;
				}
				if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 3;
					((Control)(object)grdEmptyBags).Focus();
					return;
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					GridEXRow item4 = grdFreight.CurrentRow;
					dtFreight.Rows[item4.RowIndex].Delete();
					if (item4 != null && (int)item4.RowType == 0)
					{
						item4.Delete();
						FreightProportion();
					}
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (tabControl2.SelectedIndex == 1)
				{
					GridEXRow item3 = grdInvExp.CurrentRow;
					dtInvExp.Rows[item3.RowIndex].Delete();
					if (item3 != null && (int)item3.RowType == 0)
					{
						item3.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (tabControl2.SelectedIndex == 2)
				{
					GridEXRow item2 = grdGLedger.CurrentRow;
					dtGrdGL.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (tabControl2.SelectedIndex == 3)
				{
					GridEXRow item = grdEmptyBags.CurrentRow;
					dtEmptyBags.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtEmptyBags.Rows.Count == 0)
					{
						AddRowInvEmptyBagsGrid();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					AddRowInFreightGrid();
				}
				if (tabControl2.SelectedIndex == 1)
				{
					AddRowInvExpGrid();
				}
				if (tabControl2.SelectedIndex == 2)
				{
					AddRowInGLGrid();
				}
				if (tabControl2.SelectedIndex == 3)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex != 0 && tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreight.CurrentColumn.Key == "Delete")
				{
					dtFreight.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (grdFreight.CurrentColumn.Key == "Add")
				{
					AddRowInFreightGrid();
				}
				grdFreight.UpdateData();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtFreight.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.UpdateData();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInFreightGrid();
				grdFreight.UpdateData();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.KeyCode == Keys.F1 && grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "TransporterId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["TransporterId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlstForFreight, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["TransporterId"].Value = obj2.ReturnId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGLedger.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn.Key == "Delete")
				{
					dtGrdGL.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtGrdGL.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.KeyCode == Keys.F1 && grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["AccountId"].Value = obj2.ReturnId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("Please Reset the Detail First...");
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						InvPurchaseInvoiceDetail Pd2 = new InvPurchaseInvoiceDetail();
						Pd2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						Pd2.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
						Pd2.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
						Pd2.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						Pd2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						Pd2.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						Pd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						Pd2.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Pd2.ItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
						Pd2.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						Pd2.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
						Pd2.EBWeight = Conversion.ToDecimal(r.Cells["EmptyBags"].Value);
						Pd2.EBTotalWt = Conversion.ToDecimal(r.Cells["EmptyBagsTotal"].Value);
						Pd2.WeightCut = Conversion.ToDecimal(r.Cells["WeightCut"].Value);
						Pd2.WeightCutTotal = Conversion.ToDecimal(r.Cells["WeightCutTotal"].Value);
						Pd2.AdLsWeight = Conversion.ToDecimal(r.Cells["AddLss"].Value);
						Pd2.NetBillWeight = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value);
						Pd2.NetStockWeight = Conversion.ToDecimal(r.Cells["StockWeight"].Value);
						Pd2.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						Pd2.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						Pd2.RateCut = Conversion.ToDecimal(r.Cells["RateCut"].Value);
						Pd2.RateCutAmount = Conversion.ToDecimal(r.Cells["RateCutTotal"].Value);
						Pd2.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
						Pd2.LabAnalisysNo = Conversion.ToString(r.Cells["LabSampleNo"].Value);
						Pd2.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
						Pd2.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
						Pd2.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
						Pd2.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
						Pd2.TaxNameId = Conversion.ToInt(r.Cells["TaxId"].Value);
						Pd2.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
						Pd2.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
						if (Pd2.TaxNameId > 0 && Pd2.TaxAmount > 0m)
						{
							Pd2.IsTaxable = true;
						}
						Pd2.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
						Pd2.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
						Pd2.FreightAmount = Conversion.ToDecimal(r.Cells["Freights"].Value);
						Pd2.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
						Pd2.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
						Pd2.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
						Pd2.ItemNetAmount = Math.Round(Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						Pd2.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						Pd2.ActionTypeId = 3;
						lstRemoveDetailRecord.Add(Pd2);
						dtGrid.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
					grd.UpdateData();
					TotalCommissionAmount();
					FreightProportion();
					txtExchangeRate_TextChanged(null, null);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_doubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
					Pd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					Pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Pd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					Pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Pd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					Pd.ItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
					Pd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					Pd.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
					Pd.EBWeight = Conversion.ToDecimal(r.Cells["EmptyBags"].Value);
					Pd.EBTotalWt = Conversion.ToDecimal(r.Cells["EmptyBagsTotal"].Value);
					Pd.WeightCut = Conversion.ToDecimal(r.Cells["WeightCut"].Value);
					Pd.WeightCutTotal = Conversion.ToDecimal(r.Cells["WeightCutTotal"].Value);
					Pd.AdLsWeight = Conversion.ToDecimal(r.Cells["AddLss"].Value);
					Pd.NetBillWeight = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value);
					Pd.NetStockWeight = Conversion.ToDecimal(r.Cells["StockWeight"].Value);
					Pd.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					Pd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					Pd.RateCut = Conversion.ToDecimal(r.Cells["RateCut"].Value);
					Pd.RateCutAmount = Conversion.ToDecimal(r.Cells["RateCutTotal"].Value);
					Pd.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					Pd.LabAnalisysNo = Conversion.ToString(r.Cells["LabSampleNo"].Value);
					Pd.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
					Pd.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
					Pd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					Pd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					Pd.TaxNameId = Conversion.ToInt(r.Cells["TaxId"].Value);
					Pd.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
					Pd.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
					if (Pd.TaxNameId > 0 && Pd.TaxAmount > 0m)
					{
						Pd.IsTaxable = true;
					}
					Pd.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
					Pd.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
					Pd.FreightAmount = Conversion.ToDecimal(r.Cells["Freights"].Value);
					Pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					Pd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					Pd.ItemNetAmount = Math.Round(Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					Pd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					Pd.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(Pd);
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grd.UpdateData();
				TotalCommissionAmount();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_doubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse"))
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtwareh, "Id", "WareHouseName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CropYearId" || grd.CurrentColumn.Key == "CropYearId"))
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtcrop, "Id", "CropYear");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["CropYearId"].Value = Id;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot"))
				{
					GrdPopUp grdPopUp3 = new GrdPopUp(dtjoblot, "Id", "JobLotDescription");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					Conversion.ToString((object)grdPopUp3.ReturnName);
					r.Cells["JobLotId"].Value = Id;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CityName" || grd.CurrentColumn.Key == "CityId"))
				{
					GrdPopUp grdPopUp4 = new GrdPopUp(dtCity, "Id", "CityName");
					grdPopUp4.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp4.ShowDialog();
					grdPopUp4.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp4.ReturnId);
					Conversion.ToString((object)grdPopUp4.ReturnName);
					r.Cells["CityId"].Value = Id;
				}
				grd.UpdateData();
				CommonServices.GridAutoAdjustmentNew(grd);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (formright.DoHaveUpdateRights)
					{
						RecId = CurrId;
						ReadById(RecId);
						comsupplier.Focus();
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Slip")
				{
					if (formright.DoHavePrintRights)
					{
						CommonServices.PurchaseInvoicetDirectSlip_Feed1104(CurrId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "PartySlip")
				{
					if (formright.DoHavePrintRights)
					{
						CommonServices.PurchaseInvoicetDirectSlip_Feed1104A(CurrId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					if (formright.DoHavePrintRights)
					{
						CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(CurrId, 1104), 1104);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1104);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				RecId = CurrId;
				ReadById(RecId);
				comsupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn.Key == "Delete")
				{
					dtInvExp.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtInvExp.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.KeyCode == Keys.F1 && grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "ItemId")
			{
				GrdPopUp obj = new GrdPopUp(dtOtherItem, "Id", "OtherItemName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["ItemId"].Value = obj.ReturnId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdEmptyBags_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdEmptyBags.CurrentColumn.Key == "Delete")
				{
					dtEmptyBags.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtEmptyBags.Rows.Count == 0)
					{
						AddRowInvEmptyBagsGrid();
					}
				}
				if (grdEmptyBags.CurrentColumn.Key == "Add" && grdEmptyBags.GetRows().Length == 1)
				{
					AddRowInvEmptyBagsGrid();
				}
				grdEmptyBags.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtEmptyBags.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtEmptyBags.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D && grdEmptyBags.GetRows().Length == 1)
			{
				AddRowInvEmptyBagsGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				if (grdEmptyBags.CurrentColumn != null && grdEmptyBags.CurrentColumn.Key == "CreditAccountId")
				{
					GrdPopUp obj3 = new GrdPopUp(dtCrAccountForEmptyBag, "Id", "AccountTitle");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					r.Cells["CreditAccountId"].Value = obj3.ReturnId;
				}
				if (grdEmptyBags.CurrentColumn != null && grdEmptyBags.CurrentColumn.Key == "ItemId")
				{
					GrdPopUp obj2 = new GrdPopUp(dtEmptyBagsItem, "Id", "ItemName");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["ItemId"].Value = obj2.ReturnId;
				}
				if (grdEmptyBags.CurrentColumn != null && grdEmptyBags.CurrentColumn.Key == "Type")
				{
					GrdPopUp obj = new GrdPopUp(dtEmpType, "Id", "Type");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["Type"].Value = obj.ReturnId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtSupplierWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFactoryWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DifferenceWeightCalculations()
	{
		try
		{
			double SupplierWeight = Conversion.ToDouble((object)txtSupplierWeight.Text.Trim());
			double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			if (SupplierWeight > 0.0 && FactoryWeight > 0.0)
			{
				double Diff = SupplierWeight - FactoryWeight;
				if (Diff > 0.0)
				{
					lblDiff.Text = "Short";
				}
				else if (Diff < 0.0)
				{
					lblDiff.Text = "Excess";
				}
				else
				{
					lblDiff.Text = "Nill";
				}
				txtDiffWeight.Text = Math.Abs(Diff).ToString();
			}
			else
			{
				txtDiffWeight.Text = "0";
				lblDiff.Text = "Nill";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DifferenceWeightCalculations();
			StockWeighCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFactoryWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DifferenceWeightCalculations();
			StockWeighCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockWeighCalculations()
	{
		try
		{
			decimal GrossWeight = default(decimal);
			decimal EmptyBagsTotal = default(decimal);
			decimal WeightCutTotal = default(decimal);
			decimal FactoryWeight = default(decimal);
			decimal SupplierWeight = default(decimal);
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text);
			}
			if (Conversion.ToDouble((object)txtSupplierWeight.Text) > 0.0)
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
			}
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				GrossWeight = Conversion.ToDecimal(item.Cells["GrossWeight"].Value);
				EmptyBagsTotal = Conversion.ToDecimal(item.Cells["EmptyBagsTotal"].Value);
				WeightCutTotal = Conversion.ToDecimal(item.Cells["WeightCutTotal"].Value);
				if (SupplierWeight > 0m && FactoryWeight > 0m && GrossWeight > 0m)
				{
					if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory" || ((Control)(object)CmbDeliveryTerm).Text == "Factory PartyWeight")
					{
						decimal StockWt = FactoryWeight * GrossWeight / SupplierWeight;
						StockWt = StockWt - EmptyBagsTotal - WeightCutTotal;
						item.Cells["StockWeight"].Value = Math.Round(StockWt, 2).ToString();
					}
					else if (((Control)(object)CmbDeliveryTerm).Text == "ExFactory FactoryWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Factory FactoryWeight")
					{
						decimal StockWt2 = FactoryWeight * GrossWeight / FactoryWeight;
						StockWt2 = StockWt2 - EmptyBagsTotal - WeightCutTotal;
						item.Cells["StockWeight"].Value = Math.Round(StockWt2, 2).ToString();
					}
					else if (((Control)(object)CmbDeliveryTerm).Text == "Factory")
					{
						if (SupplierWeight > FactoryWeight)
						{
							decimal StockWt3 = FactoryWeight * GrossWeight / FactoryWeight;
							StockWt3 = StockWt3 - EmptyBagsTotal - WeightCutTotal;
							item.Cells["StockWeight"].Value = Math.Round(StockWt3, 2).ToString();
						}
						else
						{
							decimal StockWt4 = FactoryWeight * GrossWeight / SupplierWeight;
							StockWt4 = StockWt4 - EmptyBagsTotal - WeightCutTotal;
							item.Cells["StockWeight"].Value = Math.Round(StockWt4, 2).ToString();
						}
					}
				}
				else
				{
					item.Cells["StockWeight"].Value = 0.ToString();
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightDeduction_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightDeduction_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightDeductionProportion();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Expected O, but got Unknown
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Expected O, but got Unknown
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Expected O, but got Unknown
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_105d: Expected O, but got Unknown
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Expected O, but got Unknown
		//IL_1119: Unknown result type (might be due to invalid IL or missing references)
		//IL_1123: Expected O, but got Unknown
		//IL_1940: Unknown result type (might be due to invalid IL or missing references)
		//IL_194a: Expected O, but got Unknown
		//IL_196e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1978: Expected O, but got Unknown
		//IL_1c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca8: Expected O, but got Unknown
		//IL_1cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbf: Expected O, but got Unknown
		//IL_1ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd6: Expected O, but got Unknown
		//IL_1ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2001: Expected O, but got Unknown
		//IL_200e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2018: Expected O, but got Unknown
		//IL_2025: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Expected O, but got Unknown
		//IL_2325: Unknown result type (might be due to invalid IL or missing references)
		//IL_232f: Expected O, but got Unknown
		//IL_233c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2346: Expected O, but got Unknown
		//IL_2353: Unknown result type (might be due to invalid IL or missing references)
		//IL_235d: Expected O, but got Unknown
		//IL_2646: Unknown result type (might be due to invalid IL or missing references)
		//IL_2650: Expected O, but got Unknown
		//IL_265d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2667: Expected O, but got Unknown
		//IL_2674: Unknown result type (might be due to invalid IL or missing references)
		//IL_267e: Expected O, but got Unknown
		//IL_e848: Unknown result type (might be due to invalid IL or missing references)
		//IL_e852: Expected O, but got Unknown
		//IL_e876: Unknown result type (might be due to invalid IL or missing references)
		//IL_e880: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FeedMill.Purchase.PurchaseInvoiceDirect));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.tabFreight = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.tabOtherExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label66 = new System.Windows.Forms.Label();
		this.tabSupplierAddLess = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label67 = new System.Windows.Forms.Label();
		this.tabEmptyBags = new System.Windows.Forms.TabPage();
		this.grdEmptyBags = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label68 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtCity = new UltraCombo();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.cmbtaxtype = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtWeightCutTotal = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtratecuttotal = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtratecut = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.comWarehouse = new UltraCombo();
		this.comRateUOM = new UltraCombo();
		this.comPackUOM = new UltraCombo();
		this.comjobLot = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.comPackingType = new UltraCombo();
		this.comItem = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtlabanalysisno = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtremarksDetail = new System.Windows.Forms.TextBox();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEmptybagsUnit = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtFreightDeduction = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbFreightDeduction = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.txtDiffWeight = new System.Windows.Forms.TextBox();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.comsupplier = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.comproject = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.labelFcy = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.labelExcha = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.labelFcyAmount = new System.Windows.Forms.Label();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.txtInvoiceQty = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.CmbBrokeryType = new UltraCombo();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.CmbBrokeryAc = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.lblDiff = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.combCommType = new UltraCombo();
		this.combcommUOM = new UltraCombo();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.combcommAgent = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBrokeryRemarks = new System.Windows.Forms.TextBox();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btn225PartySlip = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseOrderLoader = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.label58 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.tabFreight.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel8.SuspendLayout();
		this.tabOtherExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel13.SuspendLayout();
		this.tabSupplierAddLess.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel14.SuspendLayout();
		this.tabEmptyBags.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightDeduction).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1668, 814);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabPage1.Size = new System.Drawing.Size(1660, 783);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(4, 4);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1652, 775);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabFreight);
		this.tabControl2.Controls.Add(this.tabOtherExpense);
		this.tabControl2.Controls.Add(this.tabSupplierAddLess);
		this.tabControl2.Controls.Add(this.tabEmptyBags);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 514);
		this.tabControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1652, 261);
		this.tabControl2.TabIndex = 0;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel5);
		this.tabDetail.Location = new System.Drawing.Point(4, 27);
		this.tabDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Size = new System.Drawing.Size(1644, 230);
		this.tabDetail.TabIndex = 4;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1644, 203);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_doubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label64);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.ForeColor = System.Drawing.Color.White;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1644, 27);
		this.panel5.TabIndex = 70;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(7, 6);
		this.label64.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(53, 18);
		this.label64.TabIndex = 42;
		this.label64.Text = "Detail";
		this.tabFreight.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFreight.Controls.Add(this.panel8);
		this.tabFreight.Location = new System.Drawing.Point(4, 27);
		this.tabFreight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabFreight.Name = "tabFreight";
		this.tabFreight.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabFreight.Size = new System.Drawing.Size(1644, 226);
		this.tabFreight.TabIndex = 0;
		this.tabFreight.Text = "Charge to Product";
		this.tabFreight.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.grdFreight).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1636, 193);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).Enter += new System.EventHandler(grdFreight_Enter);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label65);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.ForeColor = System.Drawing.Color.White;
		this.panel8.Location = new System.Drawing.Point(4, 4);
		this.panel8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1636, 25);
		this.panel8.TabIndex = 70;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(1, 2);
		this.label65.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(145, 18);
		this.label65.TabIndex = 42;
		this.label65.Text = "Charge To Product";
		this.tabOtherExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabOtherExpense.Controls.Add(this.panel13);
		this.tabOtherExpense.Location = new System.Drawing.Point(4, 27);
		this.tabOtherExpense.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabOtherExpense.Name = "tabOtherExpense";
		this.tabOtherExpense.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabOtherExpense.Size = new System.Drawing.Size(1644, 226);
		this.tabOtherExpense.TabIndex = 1;
		this.tabOtherExpense.Text = "Other Expenses Credit to Supplier & Charge to Product";
		this.tabOtherExpense.UseVisualStyleBackColor = true;
		this.grdInvExp.AutomaticSort = false;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1636, 193);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label66);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.ForeColor = System.Drawing.Color.White;
		this.panel13.Location = new System.Drawing.Point(4, 4);
		this.panel13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1636, 25);
		this.panel13.TabIndex = 71;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(1, 2);
		this.label66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(284, 18);
		this.label66.TabIndex = 42;
		this.label66.Text = "Credit To Supplier Charge To Product";
		this.tabSupplierAddLess.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabSupplierAddLess.Controls.Add(this.panel14);
		this.tabSupplierAddLess.Location = new System.Drawing.Point(4, 27);
		this.tabSupplierAddLess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabSupplierAddLess.Name = "tabSupplierAddLess";
		this.tabSupplierAddLess.Size = new System.Drawing.Size(1644, 226);
		this.tabSupplierAddLess.TabIndex = 2;
		this.tabSupplierAddLess.Text = "Supplier AddLess";
		this.tabSupplierAddLess.UseVisualStyleBackColor = true;
		this.grdGLedger.AutomaticSort = false;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1644, 201);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label67);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.ForeColor = System.Drawing.Color.White;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1644, 25);
		this.panel14.TabIndex = 71;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(1, 2);
		this.label67.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(124, 18);
		this.label67.TabIndex = 42;
		this.label67.Text = "Supplier Addlss";
		this.tabEmptyBags.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.tabEmptyBags.Controls.Add(this.panel15);
		this.tabEmptyBags.Location = new System.Drawing.Point(4, 27);
		this.tabEmptyBags.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabEmptyBags.Name = "tabEmptyBags";
		this.tabEmptyBags.Size = new System.Drawing.Size(1644, 226);
		this.tabEmptyBags.TabIndex = 3;
		this.tabEmptyBags.Text = "EmptyBags";
		this.tabEmptyBags.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(1644, 201);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 2;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.UpdatingCell += new UpdatingCellEventHandler(grdEmptyBags_UpdatingCell);
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).KeyDown += new System.Windows.Forms.KeyEventHandler(grdEmptyBags_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label68);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.ForeColor = System.Drawing.Color.White;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1644, 25);
		this.panel15.TabIndex = 71;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(1, 2);
		this.label68.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(94, 18);
		this.label68.TabIndex = 42;
		this.label68.Text = "Empty Bags";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 308);
		this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1652, 206);
		this.panel4.TabIndex = 30;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.label78);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label73);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtaxtype);
		this.groupBox1.Controls.Add(this.label74);
		this.groupBox1.Controls.Add(this.label75);
		this.groupBox1.Controls.Add(this.txtTaxAmount);
		this.groupBox1.Controls.Add(this.txtTaxPercent);
		this.groupBox1.Controls.Add(this.label76);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtWeightCutTotal);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtratecuttotal);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.txtratecut);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtwtcut);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.txtlabanalysisno);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtStockWeight);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtremarksDetail);
		this.groupBox1.Controls.Add(this.txtAddLss);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtEmptybagsUnit);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 4);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox1.Size = new System.Drawing.Size(1601, 197);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(327, 18);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(65, 24);
		this.rdbtnItemCode.TabIndex = 158;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(256, 18);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(71, 24);
		this.rdbtnItemName.TabIndex = 157;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(412, 21);
		this.lblBalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(17, 20);
		this.lblBalance.TabIndex = 156;
		this.lblBalance.Text = "0";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1405, 155);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(36, 31);
		this.btnAdd.TabIndex = 30;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance179).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(485, 154);
		((System.Windows.Forms.Control)(object)this.txtCity).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(305, 30);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 28;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(793, 132);
		this.label78.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(67, 20);
		this.label78.TabIndex = 155;
		this.label78.Text = "Remarks";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(485, 132);
		this.label77.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(80, 20);
		this.label77.TabIndex = 155;
		this.label77.Text = "City Name";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label73.Location = new System.Drawing.Point(1051, 78);
		this.label73.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(68, 20);
		this.label73.TabIndex = 152;
		this.label73.Text = "Tax Type";
		this.cmbtaxtype.AllowNull = (DefaultableBoolean)1;
		this.cmbtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance184;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Location = new System.Drawing.Point(1051, 96);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Name = "cmbtaxtype";
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Size = new System.Drawing.Size(127, 30);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Leave += new System.EventHandler(cmbtaxtype_Leave);
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label74.Location = new System.Drawing.Point(1179, 78);
		this.label74.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(49, 20);
		this.label74.TabIndex = 150;
		this.label74.Text = "Tax %";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label75.Location = new System.Drawing.Point(1285, 78);
		this.label75.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(91, 20);
		this.label75.TabIndex = 151;
		this.label75.Text = "Tax Amount";
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(1285, 98);
		this.txtTaxAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(125, 27);
		this.txtTaxAmount.TabIndex = 22;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(1179, 98);
		this.txtTaxPercent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(103, 27);
		this.txtTaxPercent.TabIndex = 21;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(1413, 78);
		this.label76.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(101, 20);
		this.label76.TabIndex = 153;
		this.label76.Text = "Total Amount";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(1413, 98);
		this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(155, 27);
		this.txtTotalAmount.TabIndex = 23;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1491, 155);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(79, 31);
		this.btnCancelUpdateDetial.TabIndex = 32;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1405, 155);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(83, 31);
		this.btnUpdateDetail.TabIndex = 31;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtWeightCutTotal.BackColor = System.Drawing.Color.White;
		this.txtWeightCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCutTotal.Location = new System.Drawing.Point(7, 98);
		this.txtWeightCutTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtWeightCutTotal.Name = "txtWeightCutTotal";
		this.txtWeightCutTotal.Size = new System.Drawing.Size(111, 27);
		this.txtWeightCutTotal.TabIndex = 11;
		this.txtWeightCutTotal.Tag = "WtCutTotal";
		this.txtWeightCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightCutTotal.TextChanged += new System.EventHandler(txtWeightCutTotal_TextChanged);
		this.txtWeightCutTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutTotal_KeyPress);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(7, 78);
		this.label52.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(69, 20);
		this.label52.TabIndex = 79;
		this.label52.Text = "Cut Total";
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgatepassno.Location = new System.Drawing.Point(281, 156);
		this.txtgatepassno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(62, 27);
		this.txtgatepassno.TabIndex = 26;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(772, 78);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(103, 20);
		this.label50.TabIndex = 74;
		this.label50.Text = "Rate Cut Total";
		this.txtratecuttotal.BackColor = System.Drawing.Color.White;
		this.txtratecuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecuttotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratecuttotal.Location = new System.Drawing.Point(775, 98);
		this.txtratecuttotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtratecuttotal.Name = "txtratecuttotal";
		this.txtratecuttotal.ReadOnly = true;
		this.txtratecuttotal.Size = new System.Drawing.Size(114, 27);
		this.txtratecuttotal.TabIndex = 18;
		this.txtratecuttotal.TabStop = false;
		this.txtratecuttotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(696, 78);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(66, 20);
		this.label49.TabIndex = 72;
		this.label49.Text = "Rate Cut";
		this.txtratecut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratecut.Location = new System.Drawing.Point(700, 98);
		this.txtratecut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtratecut.Name = "txtratecut";
		this.txtratecut.Size = new System.Drawing.Size(71, 27);
		this.txtratecut.TabIndex = 17;
		this.txtratecut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtratecut.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtratecut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(345, 132);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(83, 20);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(345, 156);
		this.txtvehicleno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(137, 27);
		this.txtvehicleno.TabIndex = 27;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(289, 132);
		this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(41, 20);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtcut.Location = new System.Drawing.Point(1504, 44);
		this.txtwtcut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(65, 27);
		this.txtwtcut.TabIndex = 10;
		this.txtwtcut.Tag = "WtCut";
		this.txtwtcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwtcut.TextChanged += new System.EventHandler(txtwtcut_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1504, 21);
		this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(55, 20);
		this.label46.TabIndex = 67;
		this.label46.Text = "Wt.Cut";
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(7, 42);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(208, 30);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(605, 96);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(92, 30);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(976, 42);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(91, 30);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(605, 42);
		((System.Windows.Forms.Control)(object)this.comjobLot).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(201, 30);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 3;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance225;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance230).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance231).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance233).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(485, 42);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(117, 30);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		this.comPackingType.AllowNull = (DefaultableBoolean)1;
		this.comPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.comPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance238;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance244).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackingType).Location = new System.Drawing.Point(809, 42);
		((System.Windows.Forms.Control)(object)this.comPackingType).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comPackingType).Name = "comPackingType";
		((System.Windows.Forms.Control)(object)this.comPackingType).Size = new System.Drawing.Size(163, 30);
		((System.Windows.Forms.Control)(object)this.comPackingType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.comPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(217, 42);
		((System.Windows.Forms.Control)(object)this.comItem).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(265, 30);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		this.comItem.ValueChanged += new System.EventHandler(comItem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(120, 156);
		this.txtgpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(157, 27);
		this.txtgpdate.TabIndex = 25;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(488, 78);
		this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(39, 20);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(485, 21);
		this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(76, 20);
		this.label41.TabIndex = 53;
		this.label41.Text = "Crop Year";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(485, 98);
		this.txtRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(117, 27);
		this.txtRate.TabIndex = 15;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(909, 442);
		this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(48, 24);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(1179, 44);
		this.txtGrossWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(134, 27);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1179, 21);
		this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(99, 20);
		this.label39.TabIndex = 49;
		this.label39.Text = "Gross Weight";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(1069, 44);
		this.txtQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(107, 27);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(1069, 21);
		this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(33, 20);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(120, 132);
		this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(64, 20);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(217, 98);
		this.txtNetBillWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(126, 27);
		this.txtNetBillWeight.TabIndex = 13;
		this.txtNetBillWeight.TabStop = false;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(217, 78);
		this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(112, 20);
		this.label36.TabIndex = 43;
		this.label36.Text = "Net Bill Weight";
		this.txtlabanalysisno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlabanalysisno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlabanalysisno.Location = new System.Drawing.Point(7, 156);
		this.txtlabanalysisno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtlabanalysisno.Name = "txtlabanalysisno";
		this.txtlabanalysisno.Size = new System.Drawing.Size(111, 27);
		this.txtlabanalysisno.TabIndex = 24;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(7, 132);
		this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(105, 20);
		this.label37.TabIndex = 41;
		this.label37.Text = "Lab Analysis #";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(217, 21);
		this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(39, 20);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(605, 21);
		this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(58, 20);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtStockWeight.BackColor = System.Drawing.Color.White;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(345, 98);
		this.txtStockWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(137, 27);
		this.txtStockWeight.TabIndex = 14;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockWeight.TextChanged += new System.EventHandler(txtStockWeight_TextChanged);
		this.txtStockWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockWeight_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(345, 78);
		this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(99, 20);
		this.label34.TabIndex = 35;
		this.label34.Text = "Stock Weight";
		this.txtremarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksDetail.Location = new System.Drawing.Point(793, 156);
		this.txtremarksDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtremarksDetail.Name = "txtremarksDetail";
		this.txtremarksDetail.Size = new System.Drawing.Size(605, 27);
		this.txtremarksDetail.TabIndex = 29;
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLss.Location = new System.Drawing.Point(120, 98);
		this.txtAddLss.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(94, 27);
		this.txtAddLss.TabIndex = 12;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.txtAddLss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLss_KeyPress);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(7, 21);
		this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(87, 20);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(120, 78);
		this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(51, 20);
		this.label30.TabIndex = 31;
		this.label30.Text = "AD/LS";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(976, 21);
		this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(81, 20);
		this.label28.TabIndex = 29;
		this.label28.Text = "Pack UOM";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(605, 78);
		this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(79, 20);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.txtEmptyBagsTotal.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(1401, 44);
		this.txtEmptyBagsTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(99, 27);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.Tag = "PmTotal";
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.txtEmptyBagsTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptyBagsTotal_KeyPress);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1401, 21);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(83, 20);
		this.label16.TabIndex = 25;
		this.label16.Text = "PMW Total";
		this.txtEmptybagsUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptybagsUnit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptybagsUnit.Location = new System.Drawing.Point(1315, 44);
		this.txtEmptybagsUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtEmptybagsUnit.Name = "txtEmptybagsUnit";
		this.txtEmptybagsUnit.Size = new System.Drawing.Size(85, 27);
		this.txtEmptybagsUnit.TabIndex = 8;
		this.txtEmptybagsUnit.Tag = "PmUnit";
		this.txtEmptybagsUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptybagsUnit.TextChanged += new System.EventHandler(txtEmptybagsUnit_TextChanged);
		this.txtEmptybagsUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptybagsUnit_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1315, 21);
		this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(81, 20);
		this.label15.TabIndex = 23;
		this.label15.Text = "PMW/Unit";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(892, 98);
		this.txtAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(155, 27);
		this.txtAmount.TabIndex = 19;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(895, 78);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(64, 20);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(809, 21);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(99, 20);
		this.label9.TabIndex = 19;
		this.label9.Text = "Packing Type";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtFreightDeduction);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightDeduction);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.panel3.Controls.Add(this.txtSupplierWeight);
		this.panel3.Controls.Add(this.txtDiffWeight);
		this.panel3.Controls.Add(this.txtFactoryWeight);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.panel3.Controls.Add(this.txtremarks);
		this.panel3.Controls.Add(this.txtSupplierReference);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtduedays);
		this.panel3.Controls.Add(this.txtbillno);
		this.panel3.Controls.Add(this.txtBrokeryRate);
		this.panel3.Controls.Add(this.txtBrokeryAmount);
		this.panel3.Controls.Add(this.txtcommrate);
		this.panel3.Controls.Add(this.txtcommamount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.panel3.Controls.Add(this.labelFcy);
		this.panel3.Controls.Add(this.label44);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.txtExchangeRate);
		this.panel3.Controls.Add(this.labelExcha);
		this.panel3.Controls.Add(this.txtFcyAmount);
		this.panel3.Controls.Add(this.labelFcyAmount);
		this.panel3.Controls.Add(this.txtInvoiceWeight);
		this.panel3.Controls.Add(this.txtInvoiceQty);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.label53);
		this.panel3.Controls.Add(this.label54);
		this.panel3.Controls.Add(this.label55);
		this.panel3.Controls.Add(this.lblDiff);
		this.panel3.Controls.Add(this.label56);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.txtBillAmount);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.duedate);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtBrokeryRemarks);
		this.panel3.Controls.Add(this.txtCommissionRemarks);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label72);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1652, 281);
		this.panel3.TabIndex = 0;
		this.txtFreightDeduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightDeduction.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightDeduction.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtFreightDeduction.Location = new System.Drawing.Point(481, 245);
		this.txtFreightDeduction.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtFreightDeduction.Name = "txtFreightDeduction";
		this.txtFreightDeduction.Size = new System.Drawing.Size(214, 27);
		this.txtFreightDeduction.TabIndex = 29;
		this.txtFreightDeduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightDeduction.TextChanged += new System.EventHandler(txtFreightDeduction_TextChanged);
		this.txtFreightDeduction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightDeduction_KeyPress);
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(372, 239);
		this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(100, 37);
		this.label45.TabIndex = 146;
		this.label45.Text = "Freight Deduction";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(8, 250);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 20);
		this.label3.TabIndex = 144;
		this.label3.Text = "Debit";
		this.CmbFreightDeduction.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightDeduction.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightDeduction.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightDeduction.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightDeduction).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightDeduction.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).Location = new System.Drawing.Point(84, 244);
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).Name = "CmbFreightDeduction";
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).Size = new System.Drawing.Size(281, 27);
		((System.Windows.Forms.Control)(object)this.CmbFreightDeduction).TabIndex = 28;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(481, 37);
		((System.Windows.Forms.Control)(object)this.combpttrm).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(215, 30);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 7;
		this.combpttrm.ValueChanged += new System.EventHandler(combpttrm_ValueChanged);
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWeight.Location = new System.Drawing.Point(481, 143);
		this.txtSupplierWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.ShortcutsEnabled = false;
		this.txtSupplierWeight.Size = new System.Drawing.Size(214, 27);
		this.txtSupplierWeight.TabIndex = 11;
		this.txtSupplierWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierWeight.TextChanged += new System.EventHandler(txtSupplierWeight_TextChanged);
		this.txtSupplierWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierWeight_KeyPress);
		this.txtDiffWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiffWeight.Enabled = false;
		this.txtDiffWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffWeight.Location = new System.Drawing.Point(481, 210);
		this.txtDiffWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtDiffWeight.Name = "txtDiffWeight";
		this.txtDiffWeight.ShortcutsEnabled = false;
		this.txtDiffWeight.Size = new System.Drawing.Size(114, 27);
		this.txtDiffWeight.TabIndex = 13;
		this.txtDiffWeight.TabStop = false;
		this.txtDiffWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(481, 177);
		this.txtFactoryWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ShortcutsEnabled = false;
		this.txtFactoryWeight.Size = new System.Drawing.Size(214, 27);
		this.txtFactoryWeight.TabIndex = 12;
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtFactoryWeight_TextChanged);
		this.txtFactoryWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFactoryWeight_KeyPress);
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(481, 106);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(215, 30);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(1103, 113);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 20);
		this.label8.TabIndex = 142;
		this.label8.Text = "Tax Account";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(1225, 107);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(411, 27);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 27;
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(84, 106);
		((System.Windows.Forms.Control)(object)this.comsupplier).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(281, 30);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 3;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(84, 210);
		this.txtremarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(280, 27);
		this.txtremarks.TabIndex = 6;
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(84, 143);
		this.txtSupplierReference.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(280, 27);
		this.txtSupplierReference.TabIndex = 4;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(84, 37);
		((System.Windows.Forms.Control)(object)this.comproject).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(281, 30);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(84, 74);
		this.txtdocno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(107, 27);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(195, 74);
		this.DocDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(168, 27);
		this.DocDate.TabIndex = 2;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(481, 74);
		this.txtduedays.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(59, 27);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(84, 177);
		this.txtbillno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(281, 27);
		this.txtbillno.TabIndex = 5;
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryRate.Location = new System.Drawing.Point(977, 177);
		this.txtBrokeryRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(117, 27);
		this.txtBrokeryRate.TabIndex = 22;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_Leave);
		this.txtBrokeryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryRate_KeyPress);
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(977, 210);
		this.txtBrokeryAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(117, 27);
		this.txtBrokeryAmount.TabIndex = 24;
		this.txtBrokeryAmount.TabStop = false;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(977, 74);
		this.txtcommrate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(117, 27);
		this.txtcommrate.TabIndex = 17;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(977, 108);
		this.txtcommamount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(117, 27);
		this.txtcommamount.TabIndex = 19;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(1480, 142);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(156, 27);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 29;
		this.labelFcy.AutoSize = true;
		this.labelFcy.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelFcy.Location = new System.Drawing.Point(1387, 148);
		this.labelFcy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.labelFcy.Name = "labelFcy";
		this.labelFcy.Size = new System.Drawing.Size(71, 20);
		this.labelFcy.TabIndex = 140;
		this.labelFcy.Text = "Fcy Code";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(372, 148);
		this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(89, 20);
		this.label44.TabIndex = 139;
		this.label44.Text = "Supplier Wt";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(372, 215);
		this.label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(103, 20);
		this.label43.TabIndex = 139;
		this.label43.Text = "Difference Wt";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(372, 182);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(83, 20);
		this.label27.TabIndex = 139;
		this.label27.Text = "Factory Wt";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(1480, 177);
		this.txtExchangeRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(155, 27);
		this.txtExchangeRate.TabIndex = 31;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.labelExcha.AutoSize = true;
		this.labelExcha.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelExcha.Location = new System.Drawing.Point(1387, 182);
		this.labelExcha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.labelExcha.Name = "labelExcha";
		this.labelExcha.Size = new System.Drawing.Size(75, 20);
		this.labelExcha.TabIndex = 139;
		this.labelExcha.Text = "Exch Rate";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(1480, 210);
		this.txtFcyAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(155, 27);
		this.txtFcyAmount.TabIndex = 33;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.labelFcyAmount.AutoSize = true;
		this.labelFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelFcyAmount.Location = new System.Drawing.Point(1387, 215);
		this.labelFcyAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.labelFcyAmount.Name = "labelFcyAmount";
		this.labelFcyAmount.Size = new System.Drawing.Size(91, 20);
		this.labelFcyAmount.TabIndex = 138;
		this.labelFcyAmount.Text = "Fcy Amount";
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(1225, 177);
		this.txtInvoiceWeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.ShortcutsEnabled = false;
		this.txtInvoiceWeight.Size = new System.Drawing.Size(155, 27);
		this.txtInvoiceWeight.TabIndex = 30;
		this.txtInvoiceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInvoiceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceQty.Enabled = false;
		this.txtInvoiceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceQty.Location = new System.Drawing.Point(1225, 143);
		this.txtInvoiceQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtInvoiceQty.Name = "txtInvoiceQty";
		this.txtInvoiceQty.ShortcutsEnabled = false;
		this.txtInvoiceQty.Size = new System.Drawing.Size(155, 27);
		this.txtInvoiceQty.TabIndex = 28;
		this.txtInvoiceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(1103, 182);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(111, 20);
		this.label1.TabIndex = 133;
		this.label1.Text = "Invoice Weight";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1103, 148);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(86, 20);
		this.label13.TabIndex = 134;
		this.label13.Text = "Invoice Qty";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label5);
		this.panel12.Controls.Add(this.ChkPrintSlip);
		this.panel12.Controls.Add(this.ChkBok);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.ForeColor = System.Drawing.Color.White;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1652, 31);
		this.panel12.TabIndex = 69;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 5);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(328, 24);
		this.label5.TabIndex = 42;
		this.label5.Text = "PURCHASE DIRECT INVOICE     ";
		this.ChkPrintSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintSlip.Location = new System.Drawing.Point(1352, 4);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(119, 24);
		this.ChkPrintSlip.TabIndex = 24;
		this.ChkPrintSlip.Text = "Print Slip";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1480, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(161, 24);
		this.ChkBok.TabIndex = 25;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance251;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance257).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(797, 175);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(108, 30);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 21;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TextChanged += new System.EventHandler(CmbBrokeryType_Leave);
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance217).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(797, 208);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(108, 30);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_Leave);
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance264;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(797, 140);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(297, 30);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Leave += new System.EventHandler(CmbBrokeryAc_Leave);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(701, 148);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(55, 20);
		this.label23.TabIndex = 66;
		this.label23.Text = "Broker";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(909, 215);
		this.label53.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(64, 20);
		this.label53.TabIndex = 67;
		this.label53.Text = "Amount";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(701, 182);
		this.label54.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(100, 20);
		this.label54.TabIndex = 64;
		this.label54.Text = "Brokery Type";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(909, 182);
		this.label55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(39, 20);
		this.label55.TabIndex = 68;
		this.label55.Text = "Rate";
		this.lblDiff.AutoSize = true;
		this.lblDiff.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDiff.Location = new System.Drawing.Point(605, 214);
		this.lblDiff.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDiff.Name = "lblDiff";
		this.lblDiff.Size = new System.Drawing.Size(0, 20);
		this.lblDiff.TabIndex = 14;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(701, 215);
		this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(79, 20);
		this.label56.TabIndex = 65;
		this.label56.Text = "Rate UOM";
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance172).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(797, 71);
		((System.Windows.Forms.Control)(object)this.combCommType).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(108, 30);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.combCommType).TextChanged += new System.EventHandler(combCommType_TextChanged);
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(797, 106);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(108, 30);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 18;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1801, 54);
		this.txtCommAgentId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(21, 32);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.TabStop = false;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1801, 90);
		this.txtSupplierGLId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(21, 32);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.TabStop = false;
		this.txtSupplierGLId.Visible = false;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(1103, 213);
		this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(99, 23);
		this.label51.TabIndex = 60;
		this.label51.Text = "Bill Amount";
		this.txtBillAmount.BackColor = System.Drawing.Color.White;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(1225, 208);
		this.txtBillAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(155, 32);
		this.txtBillAmount.TabIndex = 32;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(7, 215);
		this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 20);
		this.label24.TabIndex = 57;
		this.label24.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(372, 44);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(107, 20);
		this.label4.TabIndex = 55;
		this.label4.Text = "Payment Term";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(372, 79);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(103, 20);
		this.label10.TabIndex = 43;
		this.label10.Text = "Due Day,Date";
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(543, 74);
		this.duedate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(152, 27);
		this.duedate.TabIndex = 9;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(372, 113);
		this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(105, 20);
		this.label29.TabIndex = 56;
		this.label29.Text = "Delivery Term";
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(797, 37);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(297, 30);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.combcommAgent).TextChanged += new System.EventHandler(combcommAgent_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 113);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(66, 20);
		this.label7.TabIndex = 0;
		this.label7.Text = "Supplier";
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(697, 39);
		this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(97, 37);
		this.label20.TabIndex = 33;
		this.label20.Text = "Commission Agent";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(909, 113);
		this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(64, 20);
		this.label26.TabIndex = 35;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(701, 79);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(89, 20);
		this.label11.TabIndex = 31;
		this.label11.Text = "Comm Type";
		this.txtBrokeryRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryRemarks.Location = new System.Drawing.Point(1225, 74);
		this.txtBrokeryRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBrokeryRemarks.Name = "txtBrokeryRemarks";
		this.txtBrokeryRemarks.Size = new System.Drawing.Size(410, 27);
		this.txtBrokeryRemarks.TabIndex = 26;
		this.txtCommissionRemarks.BackColor = System.Drawing.Color.White;
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(1225, 39);
		this.txtCommissionRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(410, 27);
		this.txtCommissionRemarks.TabIndex = 25;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 182);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 20);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual # ";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(1103, 79);
		this.label72.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(126, 20);
		this.label72.TabIndex = 34;
		this.label72.Text = "Brokery Remarks";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1103, 44);
		this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(115, 20);
		this.label25.TabIndex = 34;
		this.label25.Text = "Comm Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 148);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 20);
		this.label12.TabIndex = 0;
		this.label12.Text = "Reference";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(909, 79);
		this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(39, 20);
		this.label21.TabIndex = 36;
		this.label21.Text = "Rate";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(701, 113);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(79, 20);
		this.label19.TabIndex = 32;
		this.label19.Text = "Rate UOM";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 44);
		this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(57, 20);
		this.label18.TabIndex = 23;
		this.label18.Text = "Project";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 79);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(81, 20);
		this.label17.TabIndex = 26;
		this.label17.Text = "No && Date";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator5, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator3, this.btnSlip,
			this.btn225PartySlip, this.btnPurchaseOrderLoader, this.btnPrint, this.toolStripSeparator2, this.BtnSpecialRights, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1652, 27);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(98, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(75, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(94, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(88, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(134, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(118, 24);
		this.btnSlip.Text = "1104-Slip";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btn225PartySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn225PartySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn225PartySlip.Name = "btn225PartySlip";
		this.btn225PartySlip.Size = new System.Drawing.Size(174, 24);
		this.btn225PartySlip.Text = "1104A-PartySlip";
		this.btn225PartySlip.Click += new System.EventHandler(btn225PartySlip_Click);
		this.btnPurchaseOrderLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPurchaseOrderLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseOrderLoader.Name = "btnPurchaseOrderLoader";
		this.btnPurchaseOrderLoader.Size = new System.Drawing.Size(121, 24);
		this.btnPurchaseOrderLoader.Text = "LoadOrder";
		this.btnPurchaseOrderLoader.Click += new System.EventHandler(btnPurchaseOrderLoader_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(143, 24);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnSpecialRights.Image = Architecture.WinApp.Properties.Resources.RightsIcon;
		this.BtnSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSpecialRights.Name = "BtnSpecialRights";
		this.BtnSpecialRights.Size = new System.Drawing.Size(156, 24);
		this.BtnSpecialRights.Text = "Special Rights";
		this.BtnSpecialRights.Click += new System.EventHandler(BtnSpecialRights_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tabPage2.Size = new System.Drawing.Size(1660, 783);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(4, 165);
		((System.Windows.Forms.Control)(object)this.grdHistory).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1652, 345);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_doubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label58);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel11.Location = new System.Drawing.Point(4, 510);
		this.panel11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1652, 30);
		this.panel11.TabIndex = 47;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(4, 540);
		((System.Windows.Forms.Control)(object)this.grdDetail).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1652, 239);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 2);
		this.label58.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(374, 21);
		this.label58.TabIndex = 0;
		this.label58.Text = "Detail Information Of Above Selected Row";
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(4, 138);
		this.panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1652, 27);
		this.panel7.TabIndex = 51;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 2);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(180, 21);
		this.label2.TabIndex = 0;
		this.label2.Text = "FILTERED RECORDS";
		this.panel18.Controls.Add(this.groupBox3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(4, 68);
		this.panel18.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1652, 70);
		this.panel18.TabIndex = 0;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 2);
		this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox3.Size = new System.Drawing.Size(1640, 64);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1472, 38);
		this.rdapproveddate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(134, 24);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1472, 14);
		this.rdmodifydate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(115, 24);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1364, 14);
		this.drdocdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(93, 24);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1364, 38);
		this.rdentrydate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(103, 24);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1269, 25);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(84, 28);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(872, 30);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(66, 20);
		this.label59.TabIndex = 142;
		this.label59.Text = "Supplier";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(707, 30);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(56, 20);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(525, 30);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(76, 20);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(271, 30);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(61, 20);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(0, 30);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(81, 20);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance243).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(941, 22);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(321, 30);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(771, 25);
		this.txtToDocNoHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 27);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(608, 25);
		this.txtFromDocNoHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(97, 27);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(339, 25);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(181, 27);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(87, 25);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(181, 27);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label57);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(4, 40);
		this.panel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1652, 28);
		this.panel6.TabIndex = 46;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(4, 2);
		this.label57.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(338, 24);
		this.label57.TabIndex = 0;
		this.label57.Text = "PURCHASE INVOICE HISTORY    ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(4, 4);
		this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1652, 36);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1652, 27);
		this.toolStrip2.TabIndex = 28;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(81, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(98, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(95, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
		this.notifyIcon1.Text = "notifyIcon1";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1578, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 26);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1600, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(51, 25);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1599, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(49, 26);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1668, 814);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "PurchaseInvoiceDirect";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(PurchaseInvoiceDirect_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseInvoiceDirect_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabFreight.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabOtherExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabSupplierAddLess.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabEmptyBags.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightDeduction).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
