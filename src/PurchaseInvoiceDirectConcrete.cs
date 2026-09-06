// using Architecture.Reports;
using System;
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
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.pcc.purchase
{

public class PurchaseInvoiceDirectConcrete : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable dtGrid = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtDiscType = new DataTable();

	private DataTable dtjoblot = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtwareh = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int Id;

	private long RowVersionId;

	private int BaseCurrency;

	private int OutwardFreightAccountId;

	private bool Approved;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool OrderExist;

	private bool HasMultiCurrencyFeature;

	public bool TaxPercntEditableOrNot;

	private decimal BaseRate;

	private int DocumentType;

	private List<InvPurchaseInvoiceDetail> lstRemoveDetailRecord = new List<InvPurchaseInvoiceDetail>();

	private UserAccount UserAccount = new UserAccount();

	private LoadPurchaseOrderConcrete LoadTransactions;

	private LoadGrnForInvoice GrnLoad;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox txtAmount;

	private Label label14;

	private Label label42;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label35;

	private Label label32;

	private Label label33;

	private Label label31;

	private DateTimePicker txtgpdate;

	internal UltraCombo comjobLot;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	private Label label47;

	private Label label48;

	private TextBox txtvehicleno;

	private Button btnAdd;

	private TextBox txtgatepassno;

	internal GridEX grdChargeToProduct;

	internal GridEX grdPartyAddLs;

	internal GridEX grd;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private CheckBox ChkBok;

	private TextBox txtBillAmountHeader;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	internal UltraCombo comsupplier;

	internal UltraCombo combcommAgent;

	private Label label7;

	private Label label20;

	private Label label26;

	private Label label11;

	private TextBox txtCommissionRemarks;

	private Label label6;

	private Label label25;

	private Label label21;

	private TextBox txtbillno;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label5;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combCommType;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combcommUOM;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdDetailHistory;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	internal UltraCombo CmbTransporterAc;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	private ToolStripButton btnPrintA;

	internal UltraCombo txtCity;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Panel panel17;

	private Label label54;

	private Panel panel18;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label55;

	private Panel panel19;

	private GroupBox groupBox3;

	private TextBox txtInvoiceWeight;

	private TextBox txtInvoiceQty;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbCurrency;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private Label label59;

	private Label label60;

	private Label label61;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel12;

	private Label label37;

	private TabPage tabPage5;

	private Panel panel15;

	private Label label53;

	internal UltraCombo CmbAttributeVarient;

	private Label label1;

	private Label label2;

	private Label label9;

	private TextBox txtItemWeight;

	private TextBox txtVarientEquivalent;

	internal UltraCombo CmbDiscountType;

	private Label label15;

	private Label label69;

	private Label label70;

	private TextBox txtDiscountRate;

	private TextBox txtDiscountAmount;

	private Label label71;

	private TextBox txtTotalAmount;

	private Panel panel5;

	private Panel panel7;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

	internal UltraCombo CmbCommissionAgentHistory;

	private Label label72;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private Panel panel8;

	private Label label43;

	private TextBox txtNetWeight;

	private Label label79;

	private Label label89;

	private Label label3;

	private ToolStripButton BtnRefreshHistory;

	private Label label51;

	private Label label57;

	private TextBox txtNetRate;

	private Label label56;

	private TextBox txtRateCut;

	private Label label49;

	internal UltraCombo cmbtaxtype;

	private Label label50;

	private Label label64;

	private TextBox txtTaxAmount;

	private TextBox txtTaxPercent;

	private TextBox txtremarks;

	private Label label24;

	private Label label12;

	internal UltraCombo CmbTaxAccount;

	private GroupBox CurrencyInfoBox;

	private TextBox txtFreightAmountHeader;

	private TextBox txtFreightRemarks;

	private Label label22;

	private Panel panel6;

	private ToolStripButton btnLoadGrn;

	private TextBox txtItemAmountWithOutDiscount;

	private Label label16;

	private Label label52;

	private Label label80;

	private Label label85;

	private Label label86;

	private TextBox txtDiscountHeader;

	private TextBox txtItemAmountHeader;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private Label label68;

	private TextBox txtManualTotalBillAmount;

	private Label label27;

	private TextBox txtItemAmountWithoutDiscHeader;

	private Label label28;

	private TextBox txtTaxAmountHeader;

	private Label label34;

	private Label label30;

	private TextBox txtPartyAddLessGridAmountHeader;

	private TextBox txtOtherItemGridAmountHeader;

	private Label label36;

	private Label label45;

	private Label labelFreightPlusIcon;

	private Label label39;

	private Label labelCommAmtPlusIcon;

	private Label labelCommAmtHeader;

	private TextBox txtCommissionAmountHeader;

	public PurchaseInvoiceDirectConcrete(UserAccount user)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 700);
			TaxPercntEditableOrNot = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("TaxPercentEditable"));
			if (TaxPercntEditableOrNot)
			{
				txtTaxPercent.Enabled = true;
			}
			else
			{
				txtTaxPercent.Enabled = false;
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnPrintA.Enabled = formright.DoHavePrintRights;
			btnSlip.Enabled = formright.DoHavePrintRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Checked = formright.DoHavePrintRights;
			ChkBok.Enabled = formright.DoHavePrintRights;
			ChkBok.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			CurrencyFill();
			MultiCurrencyFeature();
			GetallTaxType();
			dtDiscType.Columns.Add("Id", typeof(int));
			dtDiscType.Columns.Add("DiscountType");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("ItemWeight", typeof(double));
			bindWareHouse();
			JobLotBind();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("GrnId", typeof(int));
			dtGrid.Columns.Add("GrnDetailId", typeof(int));
			dtGrid.Columns.Add("GrnNo", typeof(int));
			dtGrid.Columns.Add("OrderId", typeof(int));
			dtGrid.Columns.Add("OrderDetailId", typeof(int));
			dtGrid.Columns.Add("OrderNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName", typeof(string));
			dtGrid.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("AttributeVarientId", typeof(int));
			dtGrid.Columns.Add("AttributeVarient", typeof(string));
			dtGrid.Columns.Add("VarientUnit", typeof(decimal));
			dtGrid.Columns.Add("ItemQty", typeof(decimal));
			dtGrid.Columns.Add("ItemWeight", typeof(decimal));
			dtGrid.Columns.Add("NetWeight", typeof(decimal));
			dtGrid.Columns.Add("Rate", typeof(decimal));
			dtGrid.Columns.Add("RateCut", typeof(decimal));
			dtGrid.Columns.Add("NetRate", typeof(decimal));
			dtGrid.Columns.Add("ItemAmountWithoutDisc", typeof(decimal));
			dtGrid.Columns.Add("DiscountTypeId", typeof(int));
			dtGrid.Columns.Add("DiscountType", typeof(string));
			dtGrid.Columns.Add("DiscRate", typeof(decimal));
			dtGrid.Columns.Add("DiscAmount", typeof(decimal));
			dtGrid.Columns.Add("ItemAmount", typeof(decimal));
			dtGrid.Columns.Add("TaxId", typeof(int));
			dtGrid.Columns.Add("Tax%", typeof(decimal));
			dtGrid.Columns.Add("TaxAmount", typeof(decimal));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("Expense", typeof(decimal));
			dtGrid.Columns.Add("Commission", typeof(decimal));
			dtGrid.Columns.Add("Freight", typeof(decimal));
			dtGrid.Columns.Add("ItemNetAmount", typeof(decimal));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("ItemName", typeof(string));
			dtInvExp.Columns.Add("Qty", typeof(decimal));
			dtInvExp.Columns.Add("Rate", typeof(decimal));
			dtInvExp.Columns.Add("Amount", typeof(decimal));
			dtInvExp.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("GlAccountId", typeof(string));
			dtGrdGL.Columns.Add("AccountTitle", typeof(string));
			dtGrdGL.Columns.Add("Remarks", typeof(string));
			dtGrdGL.Columns.Add("Percentage", typeof(decimal));
			dtGrdGL.Columns.Add("Qty", typeof(decimal));
			dtGrdGL.Columns.Add("Rate", typeof(decimal));
			dtGrdGL.Columns.Add("Debit", typeof(decimal));
			dtGrdGL.Columns.Add("Credit", typeof(decimal));
			BindGrids();
			DocumentNo();
			Project();
			braches();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			TransporterAcFill();
			TaxAccountFill();
			item();
			DiscountTypefill();
			BinCity();
			dtForGridComboFill();
			ConfigurationDefault();
			PreviousPurchaseTaxAc();
			HistoryComboBind();
			ParameterFill();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocDate.Focus();
		}
		catch (Exception ex)
		{
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void GetallTaxType()
	{
		try
		{
			dtTaxType.Rows.Clear();
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
			if (dtTaxType.Rows.Count > 0)
			{
				DDL.BindDDL(dtTaxType, cmbtaxtype, "TaxNameId", "TaxName", "Tax Type", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				DocumentTypeId = 1859,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int Id = Conversion.ToInt(comsupplier.Value);
			int Idr = Conversion.ToInt(combcommAgent.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers();
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
				}
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			DDL.BindDDL(dt, combCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)combCommType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			((UltraGridBase)combcommUOM).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbTransporterAc.Value);
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbTransporterAc, "Id", "AccountTitle", "Account Title", true);
			if (ItId1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					CmbTransporterAc.Value = ItId1;
				}
				else
				{
					((Control)(object)CmbTransporterAc).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
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
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["WeightKgs"]);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId", typeof(int));
				dt.Columns.Add("ItemAttribute");
				dt.Columns.Add("VarientEquivalent", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["ItemAttribute"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dt, CmbAttributeVarient, "ItemAttributeVarientId", "ItemAttribute", "Varient", ZeroIndex: true);
				if (ItId != null && ItId != string.Empty)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("ItemAttribute") == ItId))
					{
						((Control)(object)CmbAttributeVarient).Text = ItId;
					}
					else
					{
						((Control)(object)CmbAttributeVarient).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAttributeVarient).Text = string.Empty;
				txtVarientEquivalent.Text = string.Empty;
				((UltraGridBase)CmbAttributeVarient).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotBind()
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DiscountTypefill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbDiscountType.Value);
			dtDiscType.Clear();
			dtDiscType.Rows.Add(1, "Flat");
			dtDiscType.Rows.Add(2, "Percent");
			DDL.BindDDL(dtDiscType, CmbDiscountType, "Id", "DiscountType", "DiscountType", ZeroIndex: true);
			((UltraGridBase)CmbDiscountType).Rows[0].Activate();
			((UltraGridBase)CmbDiscountType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dtDiscType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbDiscountType.Value = ItId;
				}
				else
				{
					((Control)(object)CmbDiscountType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		catch (Exception)
		{
			MessageBox.Show("City Not Found");
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 1859);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtForGridComboFill()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			dtAccountlst.Clear();
			dtOtherItem.Clear();
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
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

	private void ConfigurationDefault()
	{
		try
		{
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
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(comItem.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemWeight"].Value);
				bindvarientunit(Conversion.ToInt(comItem.Value));
			}
			else
			{
				txtItemWeight.Text = 0.ToString();
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int ItemId = Conversion.ToInt(comItem.Value);
			int wareId = Conversion.ToInt(comWarehouse.Value);
			int AttributeVarientId = Conversion.ToInt(CmbAttributeVarient.Value);
			int jobLotId = Conversion.ToInt(comjobLot.Value);
			if (ItemId > 0)
			{
				DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, jobLotId, AttributeVarientId);
				if (DtStock.Rows.Count > 0)
				{
					lblBalance.Text = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
				}
				else
				{
					lblBalance.Text = 0.ToString();
				}
			}
			else
			{
				lblBalance.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comWarehouse_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
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
				CurrencyInfoBox.Visible = true;
			}
			else
			{
				CurrencyInfoBox.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings(grd);
			}
			DataTable dataTable2 = dtInvExp;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				DataTable dataTable3 = dtInvExp;
				if (dataTable3 != null && dataTable3.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdChargeToProduct.DataSource = dtInvExp;
				grdChargeToProduct.RetrieveStructure();
				grdInvExpSettings();
			}
			DataTable dataTable4 = dtGrdGL;
			if (dataTable4 != null && dataTable4.Columns.Count > 0)
			{
				DataTable dataTable5 = dtGrdGL;
				if (dataTable5 != null && dataTable5.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdPartyAddLs.DataSource = dtGrdGL;
				grdPartyAddLs.RetrieveStructure();
				gridGLSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, "", 0, 0, 0, "");
		grdChargeToProduct.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdChargeToProduct.ColumnAutoResize = false;
			grdChargeToProduct.RootTable.Columns["Id"].Visible = false;
			grdChargeToProduct.RootTable.Columns["ItemId"].Visible = false;
			grdChargeToProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdChargeToProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdChargeToProduct.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdChargeToProduct.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdChargeToProduct.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdChargeToProduct.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdChargeToProduct.RootTable.Columns["Amount"].EditType = (EditType)1;
			grdChargeToProduct.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdChargeToProduct.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdChargeToProduct.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdChargeToProduct.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdChargeToProduct.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdChargeToProduct.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdChargeToProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdChargeToProduct.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdChargeToProduct.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdChargeToProduct.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdChargeToProduct);
			grdChargeToProduct.RootTable.Columns.Add("Delete");
			grdChargeToProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdChargeToProduct.RootTable.Columns["Delete"].Caption = "X";
			grdChargeToProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdChargeToProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdChargeToProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdChargeToProduct.RootTable.Columns["Delete"].Width = 30;
			grdChargeToProduct.RootTable.Columns["Delete"].Position = 0;
			grdChargeToProduct.RootTable.Columns.Add("Add");
			grdChargeToProduct.RootTable.Columns["Add"].Key = "Add";
			grdChargeToProduct.RootTable.Columns["Add"].Caption = "+";
			grdChargeToProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdChargeToProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdChargeToProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdChargeToProduct.RootTable.Columns["Add"].Width = 30;
			grdChargeToProduct.RootTable.Columns["Add"].Position = 1;
			grdChargeToProduct.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdChargeToProduct.CurrentRow;
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
			grdChargeToProduct.UpdateData();
			OtherItemsProportion();
			FreightHeaderProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdChargeToProduct.UpdateData();
			GridEXRow item = grdChargeToProduct.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
					decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Amount = num * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else if (e.Column.Key == "Amount")
			{
				item.Cells["Amount"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			grdChargeToProduct.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdChargeToProduct);
			OtherItemsProportion();
			FreightHeaderProportion();
			txtExchangeRate_TextChanged(null, null);
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

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, 0, "", "", 0, 0, 0, 0, 0);
		grdPartyAddLs.DataSource = dtGrdGL;
	}

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Debit" || e.Column.Key == "Credit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void gridGLSettings()
	{
		try
		{
			grdPartyAddLs.ColumnAutoResize = false;
			grdPartyAddLs.RootTable.Columns["AccountId"].Visible = false;
			grdPartyAddLs.RootTable.Columns["GlAccountId"].Visible = false;
			grdPartyAddLs.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdPartyAddLs.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdPartyAddLs.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdPartyAddLs.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Percentage"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Debit"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Credit"].EditType = (EditType)1;
			grdPartyAddLs.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			grdPartyAddLs.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdPartyAddLs.RootTable.Columns["Percentage"].FormatString = "#,##0.##";
			grdPartyAddLs.RootTable.Columns["Percentage"].TotalFormatString = "#,##0.##";
			grdPartyAddLs.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPartyAddLs.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPartyAddLs.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdPartyAddLs.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdPartyAddLs.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPartyAddLs.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPartyAddLs.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdPartyAddLs.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdPartyAddLs.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyAddLs.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyAddLs.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdPartyAddLs.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdPartyAddLs.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyAddLs.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPartyAddLs.RootTable.Columns.Add("Delete");
			grdPartyAddLs.RootTable.Columns["Delete"].Key = "Delete";
			grdPartyAddLs.RootTable.Columns["Delete"].Caption = "X";
			grdPartyAddLs.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPartyAddLs.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPartyAddLs.RootTable.Columns["Delete"].ButtonText = "X";
			grdPartyAddLs.RootTable.Columns["Delete"].Width = 30;
			grdPartyAddLs.RootTable.Columns["Delete"].Position = 0;
			grdPartyAddLs.RootTable.Columns.Add("Add");
			grdPartyAddLs.RootTable.Columns["Add"].Key = "Add";
			grdPartyAddLs.RootTable.Columns["Add"].Caption = "+";
			grdPartyAddLs.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPartyAddLs.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPartyAddLs.RootTable.Columns["Add"].ButtonText = "+";
			grdPartyAddLs.RootTable.Columns["Add"].Width = 30;
			grdPartyAddLs.RootTable.Columns["Add"].Position = 1;
			grdPartyAddLs.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdPartyAddLs.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdPartyAddLs.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPartyAddLs.UpdateData();
			GridEXRow item = grdPartyAddLs.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				decimal Qty = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
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
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDecimal((object)item.Cells["Debit"].Value.ToString()) > 0m)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDecimal((object)item.Cells["Credit"].Value.ToString()) > 0m)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "AccountId" && (Conversion.ToString(item.Cells["Remarks"].Value) == string.Empty || item.Cells["Remarks"].Value == null))
			{
				item.Cells["Remarks"].Value = Conversion.ToString((object)item.Cells["AccountId"].Text);
			}
			grdPartyAddLs.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsDuplicateItem(int excludeRowIndex = -1)
	{
		GridEXRow[] rows = grd.GetRows();
		foreach (GridEXRow row in rows)
		{
			if (row.RowIndex != excludeRowIndex && Conversion.ToInt(comWarehouse.Value) == Conversion.ToInt(row.Cells["WarehouseId"].Value) && Conversion.ToInt(comItem.Value) == Conversion.ToInt(row.Cells["ItemId"].Value) && Conversion.ToInt(CmbAttributeVarient.Value) == Conversion.ToInt(row.Cells["AttributeVarientId"].Value) && Conversion.ToInt(comjobLot.Value) == Conversion.ToInt(row.Cells["JobLotId"].Value))
			{
				MessageBox.Show("Duplicate items are not allowed. An item with the same Warehouse, Job Lot and Attribute Variant already exists.");
				return true;
			}
		}
		return false;
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0)
			{
				GridEXRow r = grd.GetRow(0);
				if (Conversion.ToInt(r.Cells["GrnId"].Value) > 0 || Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
				{
					throw new Exception("You cannot add manual records because order/GRN exists.");
				}
			}
			if (FormValidationDetail() && !IsDuplicateItem())
			{
				DataRow dr = dtGrid.NewRow();
				dr["Id"] = 0;
				dr["GrnId"] = 0;
				dr["GrnDetailId"] = 0;
				dr["GrnNo"] = 0;
				dr["OrderId"] = 0;
				dr["OrderDetailId"] = 0;
				dr["OrderNo"] = 0;
				FillCustomerDetailRow(dr);
				dtGrid.Rows.Add(dr);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings(grd);
				TotalCommissionAmount();
				CommissionProportion();
				WagesProportion();
				OtherItemsProportion();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
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
			if (FormValidationDetail() && !IsDuplicateItem(updateDetailIndex))
			{
				DataRow dr = dtGrid.Rows[updateDetailIndex];
				FillCustomerDetailRow(dr);
				TotalCommissionAmount();
				CommissionProportion();
				WagesProportion();
				OtherItemsProportion();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerDetailRow(DataRow dr)
	{
		dr["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
		dr["ItemId"] = Conversion.ToInt(comItem.Value);
		dr["ItemName"] = ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value.ToString();
		dr["ItemCode"] = ((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value.ToString();
		dr["AttributeVarientId"] = Conversion.ToInt(CmbAttributeVarient.Value);
		dr["AttributeVarient"] = ((Control)(object)CmbAttributeVarient).Text;
		dr["VarientUnit"] = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
		dr["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
		dr["ItemWeight"] = Conversion.ToDecimal((object)txtItemWeight.Text);
		dr["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
		decimal rate = Conversion.ToDecimal((object)txtRate.Text);
		decimal rateCut = Conversion.ToDecimal((object)txtRateCut.Text);
		dr["Rate"] = rate;
		dr["RateCut"] = rateCut;
		dr["NetRate"] = rate - rateCut;
		decimal grossAmount = Conversion.ToDecimal((object)txtItemAmountWithOutDiscount.Text);
		dr["ItemAmountWithoutDisc"] = grossAmount;
		dr["DiscountTypeId"] = Conversion.ToInt(CmbDiscountType.Value);
		dr["DiscountType"] = ((Control)(object)CmbDiscountType).Text;
		dr["DiscRate"] = Conversion.ToDecimal((object)txtDiscountRate.Text);
		decimal discAmount = Conversion.ToDecimal((object)txtDiscountAmount.Text);
		dr["DiscAmount"] = discAmount;
		dr["ItemAmount"] = grossAmount - discAmount;
		dr["TaxId"] = Conversion.ToInt(cmbtaxtype.Value);
		dr["Tax%"] = Conversion.ToDecimal((object)txtTaxPercent.Text);
		dr["TaxAmount"] = Conversion.ToDecimal((object)txtTaxAmount.Text);
		dr["JobLotId"] = Conversion.ToInt(comjobLot.Value);
		dr["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value);
		dr["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
		dr["VehicleNo"] = txtvehicleno.Text;
		dr["CityId"] = Conversion.ToInt(txtCity.Value);
		dr["CityName"] = ((Control)(object)txtCity).Text;
		dr["Expense"] = 0m;
		dr["Commission"] = 0m;
		dr["Freight"] = 0m;
		dr["ItemNetAmount"] = Conversion.ToDecimal((object)txtTotalAmount.Text);
		dr["FcyAmount"] = ((Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Math.Round((grossAmount - discAmount) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount) : 0m);
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && Conversion.ToInt(r.Cells["OrderId"].Value) <= 0 && Conversion.ToInt(r.Cells["GrnId"].Value) <= 0)
			{
				updateDetailIndex = r.RowIndex;
				comWarehouse.Value = r.Cells["WarehouseId"].Value;
				comItem.Value = r.Cells["ItemId"].Value;
				bindvarientunit(Conversion.ToInt(comItem.Value));
				CmbAttributeVarient.Value = r.Cells["AttributeVarientId"].Value;
				txtVarientEquivalent.Text = r.Cells["VarientUnit"].Value.ToString();
				txtQty.Text = Conversion.ToDecimal(r.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtItemWeight.Text = Conversion.ToDecimal(r.Cells["ItemWeight"].Value).ToString("#,##0.####");
				txtNetWeight.Text = Conversion.ToDecimal(r.Cells["NetWeight"].Value).ToString("#,##0.####");
				txtRate.Text = Conversion.ToDecimal(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtRateCut.Text = Conversion.ToDecimal(r.Cells["RateCut"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtNetRate.Text = Conversion.ToDecimal(r.Cells["NetRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtItemAmountWithOutDiscount.Text = Conversion.ToDecimal(r.Cells["ItemAmountWithoutDisc"].Value).ToString("#,##0.####");
				txtDiscountRate.Text = Conversion.ToDecimal(r.Cells["DiscRate"].Value).ToString("#,##0.###");
				txtDiscountAmount.Text = Conversion.ToDecimal(r.Cells["DiscAmount"].Value).ToString("#,##0.####");
				txtAmount.Text = Conversion.ToDecimal(r.Cells["ItemAmount"].Value).ToString("#,##0.####");
				if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) > 0)
				{
					CmbDiscountType.Value = r.Cells["DiscountTypeId"].Value;
				}
				if (Conversion.ToInt(r.Cells["TaxId"].Value) > 0)
				{
					cmbtaxtype.Value = r.Cells["TaxId"].Value;
				}
				txtTaxPercent.Text = Conversion.ToDecimal(r.Cells["Tax%"].Value).ToString("#,##0.###");
				txtTaxAmount.Text = Conversion.ToDecimal(r.Cells["TaxAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				comjobLot.Value = r.Cells["JobLotId"].Value;
				txtgpdate.Value = Conversion.ToDateTime(r.Cells["GpDate"].Value);
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				txtCity.Value = r.Cells["CityId"].Value;
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				comItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WarehouseId"], dtwareh, "Id", "WarehouseName", "Warehouse");
			GridEX_Helper.ComboBind(grd.RootTable.Columns["TaxId"], dtTaxType, "TaxNameId", "TaxName", "Tax Name", 60);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobLotId"], dtjoblot, "Id", "JobLotDescription", "Job Lot", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CityId"], dtCity, "Id", "CityName", "City");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "OrderId", "OrderDetailId", "GrnId", "GrnDetailId", "AttributeVarientId", "DiscountTypeId", "ItemId", "CityId" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"ItemQty",
						(EditType)1
					},
					{
						"Rate",
						(EditType)1
					},
					{
						"RateCut",
						(EditType)1
					},
					{
						"DiscRate",
						(EditType)1
					},
					{
						"DiscAmount",
						(EditType)1
					},
					{
						"Tax%",
						(EditType)(TaxPercntEditableOrNot ? 1 : 0)
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			if (!(((Control)(object)gridEX).Name != "grd"))
			{
				DetailGridComboBind();
				GridEXRow row = gridEX.GetRow(0);
				int grnId = Conversion.ToInt(row.Cells["GrnId"].Value);
				int orderId = Conversion.ToInt(row.Cells["OrderId"].Value);
				gridEX.RootTable.Columns["OrderNo"].Visible = false;
				gridEX.RootTable.Columns["GrnNo"].Visible = false;
				gridEX.FrozenColumns = 1;
				if (grnId > 0 && orderId > 0)
				{
					gridEX.RootTable.Columns["OrderNo"].Visible = true;
					gridEX.RootTable.Columns["GrnNo"].Visible = true;
					gridEX.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["JobLotId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["Rate"].EditType = (EditType)0;
					gridEX.RootTable.Columns["RateCut"].EditType = (EditType)1;
				}
				else if (grnId > 0 && orderId == 0)
				{
					gridEX.RootTable.Columns["GrnNo"].Visible = true;
					gridEX.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["JobLotId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["Rate"].EditType = (EditType)1;
					gridEX.RootTable.Columns["RateCut"].EditType = (EditType)1;
				}
				else if (grnId == 0 && orderId > 0)
				{
					gridEX.RootTable.Columns["OrderNo"].Visible = true;
					gridEX.RootTable.Columns["ItemQty"].EditType = (EditType)1;
					gridEX.RootTable.Columns["Rate"].EditType = (EditType)0;
					gridEX.RootTable.Columns["RateCut"].EditType = (EditType)1;
					gridEX.RootTable.Columns["WarehouseId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["JobLotId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["GpDate"].EditType = (EditType)2;
					gridEX.RootTable.Columns["GpNo"].EditType = (EditType)1;
					gridEX.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 1);
					gridEX.FrozenColumns = 2;
				}
				else
				{
					gridEX.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["ItemQty"].EditType = (EditType)0;
					gridEX.RootTable.Columns["Rate"].EditType = (EditType)0;
					gridEX.RootTable.Columns["RateCut"].EditType = (EditType)0;
					gridEX.RootTable.Columns["JobLotId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["GpDate"].EditType = (EditType)0;
					gridEX.RootTable.Columns["GpNo"].EditType = (EditType)0;
					gridEX.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				}
				ctrlGrdBar1_Load(null, null);
			}
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Rate" || e.Column.Key == "RateCut")
				{
					double Qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					double ItemWt = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
					double num = Conversion.ToDouble(item.Cells["VarientUnit"].Value);
					double num2 = Conversion.ToDouble(item.Cells["Rate"].Value);
					double Ratecut = Conversion.ToDouble(item.Cells["RateCut"].Value);
					double NetRate = 0.0;
					double NetWeight = 0.0;
					double amount = 0.0;
					NetRate = num2 - Ratecut;
					item.Cells["NetRate"].Value = NetRate;
					NetWeight = Conversion.ToDouble((object)(num * Qty * ItemWt));
					item.Cells["NetWeight"].Value = NetWeight.ToString("##,#.##");
					amount = Conversion.ToDouble((object)(num * Qty * NetRate));
					item.Cells["ItemAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else if (e.Column.Key == "TaxId")
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
							item.BeginEdit();
							item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows[0]["TaxPercent"]);
							item.EndEdit();
						}
					}
				}
				else if (e.Column.Key == "Tax%" && Conversion.ToDouble(item.Cells["Tax%"].Value) == 0.0)
				{
					int selectedTaxId2 = Conversion.ToInt(item.Cells["TaxId"].Value);
					DataRow[] matchingRows2 = dtTaxType.Select("TaxNameId = " + selectedTaxId2);
					if (matchingRows2.Length != 0)
					{
						item.BeginEdit();
						item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows2[0]["TaxPercent"]);
						item.EndEdit();
					}
				}
				int TaxType = Conversion.ToInt(item.Cells["TaxId"].Value);
				double TaxPercent = Conversion.ToDouble(item.Cells["Tax%"].Value);
				double ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				double TaxAmount = 0.0;
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					item.Cells["Tax%"].Value = TaxPercent;
				}
				if (ItemAmount > 0.0 && TaxPercent > 0.0 && TaxType > 0)
				{
					TaxAmount = ItemAmount * TaxPercent / 100.0;
					item.BeginEdit();
					item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.EndEdit();
				}
				else
				{
					item.Cells["Tax%"].Value = 0;
					item.Cells["TaxAmount"].Value = 0;
				}
			}
			grd.UpdateData();
			OtherItemsProportion();
			TotalCommissionAmount();
			FreightHeaderProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (e.Column.Key == "Add")
				{
					AddDetailRow(r);
				}
			}
			TotalCommissionAmount();
			CommissionProportion();
			WagesProportion();
			OtherItemsProportion();
			FreightHeaderProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				_ = item.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DataRow dr = dtGrid.Rows[r.RowIndex];
				dtGrid.Rows.Add(dr.ItemArray);
				dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
				grd.DataSource = dtGrid;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvPurchaseInvoiceDetail d = new InvPurchaseInvoiceDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(d);
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				grd.UpdateData();
				TotalCommissionAmount();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("CustomerName Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null || Conversion.ToInt(combpttrm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
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
		if (Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit" && Conversion.ToInt((object)txtduedays.Text.Trim()) <= 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbTransporterAc.Value) > 0 && !(Conversion.ToDouble((object)txtFreightAmountHeader.Text) > 0.0))
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtFreightAmountHeader.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmountHeader.Text) > 0.0 && txtFreightRemarks.Text.Trim() == "")
		{
			MessageBox.Show("Freight Remarks Field is Required");
			txtFreightRemarks.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmountHeader.Text) > 0.0 && Conversion.ToInt(CmbTransporterAc.Value) <= 0)
		{
			MessageBox.Show("Carriage Ac Field is Required");
			CmbTransporterAc.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
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
		if (((UltraGridBase)CmbAttributeVarient).ActiveRow == null || Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Varient Field is Required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtItemWeight.Text.Trim() == "" || txtItemWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Item Weight Field is Required");
			txtItemWeight.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
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
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)txtCity).ActiveRow == null || Conversion.ToInt(txtCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			txtCity.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0)
		{
			if (txtTaxPercent.Text.Trim() == string.Empty || txtTaxPercent.Text.Trim() == "0" || txtTaxPercent.Text.Trim() == null)
			{
				MessageBox.Show("Tax Percent is Required");
				txtTaxPercent.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0 && ((UltraGridBase)cmbtaxtype).ActiveRow == null)
			{
				MessageBox.Show("Tax Name is Required");
				cmbtaxtype.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = false;
			((Control)(object)comItem).Enabled = false;
			((Control)(object)comjobLot).Enabled = false;
			txtQty.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnablesFields()
	{
		try
		{
			((Control)(object)combpttrm).Enabled = true;
			((Control)(object)combcommAgent).Enabled = true;
			((Control)(object)combCommType).Enabled = true;
			txtcommrate.Enabled = true;
			((Control)(object)comWarehouse).Enabled = true;
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comjobLot).Enabled = true;
			txtQty.Enabled = true;
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
			EnablesFields();
			DocumentType = 0;
			Id = 0;
			VoucherHeadId = 0;
			RowVersionId = 0L;
			Approved = false;
			((Control)(object)comsupplier).Enabled = true;
			((Control)(object)combdeliverytrm).Enabled = true;
			((Control)(object)CmbTransporterAc).Enabled = true;
			((Control)(object)comsupplier).Text = string.Empty;
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)combcommAgent).Text = string.Empty;
			((Control)(object)combCommType).Text = string.Empty;
			txtInvoiceQty.Text = string.Empty;
			txtInvoiceWeight.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtFreightAmountHeader.Text = string.Empty;
			txtFreightRemarks.Text = string.Empty;
			txtduedays.Text = "0";
			lstRemoveDetailRecord.Clear();
			OrderExist = false;
			((Control)(object)comItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRateCut.Text = string.Empty;
			txtNetRate.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			DocDate.Focus();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			AddRowInGLGrid();
			grdPartyAddLs.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdChargeToProduct.RetrieveStructure();
			grdInvExpSettings();
			DocumentNo();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtBillAmountHeader.Text = string.Empty;
			txtManualTotalBillAmount.Text = "0";
			ConfigurationDefault();
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
			txtQty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtRateCut.Text = string.Empty;
			txtNetRate.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)cmbtaxtype).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			txtgatepassno.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			updateDetailIndex = -1;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comItem).Text = string.Empty;
		((Control)(object)comjobLot).Text = string.Empty;
		((Control)(object)comWarehouse).Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		comItem.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllSerivesItems", "MultiCurrency", "AccountsWithCustomGroupId", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CurrencyFill();
			Project();
			braches();
			suppliercustomer();
			PaymentTerms();
			CommissionTypeFill();
			CommissionUOMFill();
			TransporterAcFill();
			GetallTaxType();
			bindWareHouse();
			item();
			JobLotBind();
			DiscountTypefill();
			BinCity();
			dtForGridComboFill();
			ConfigurationDefault();
			TaxAccountFill();
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

	private void InvfrmInvSaleInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Insert()
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_0a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Expected O, but got Unknown
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Expected O, but got Unknown
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f03: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToDecimal((object)txtFreightAmountHeader.Text.Trim()) > 0m)
			{
				int num = Conversion.ToInt(CmbTransporterAc.Value);
				if (num == 0)
				{
					CmbTransporterAc.Focus();
					throw new Exception("Transporter Account field Required");
				}
				int SupGlId = Conversion.ToInt((object)txtSupplierGLId.Text);
				if (num == SupGlId)
				{
					throw new Exception("Transporter Account can not be same as Customer Please check");
				}
			}
			if (Conversion.ToDecimal((object)txtcommamount.Text.Trim()) > 0m && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				combcommAgent.Focus();
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			for (int l = 0; l < rows.Length; l++)
			{
				if (Conversion.ToDouble(rows[l].Cells["TaxAmount"].Value) > 0.0 && (((Control)(object)CmbTaxAccount).Text.Trim() == string.Empty || ((Control)(object)CmbTaxAccount).Text.Trim() == "0"))
				{
					MessageBox.Show("Tax Account is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbTaxAccount.Focus();
					return;
				}
			}
			InvPurchaseInvoice obj = new InvPurchaseInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = Id;
				obj.RowVersion = RowVersionId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDecimal(r5.Cells["Credit"].Value) > 0m || Conversion.ToInt(r5.Cells["Debit"].Value) > 0)
				{
					int num2 = Conversion.ToInt(r5.Cells["AccountId"].Value);
					int GlAccountId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
					if (num2 == 0 && GlAccountId == 0)
					{
						MessageBox.Show("Please Select an Account Party Addless Grid in Row no: " + (r5.RowIndex + 1));
						return;
					}
				}
			}
			rows = grdChargeToProduct.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToDecimal(r4.Cells["Amount"].Value) > 0m && Conversion.ToInt(r4.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			OtherItemsProportion();
			FreightHeaderProportion();
			GridItemNetAmountCalculate();
			txtExchangeRate_TextChanged(null, null);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1859;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.DeliveryStartDate = DateTime.Now;
			obj.ExpiryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			obj.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			obj.CommissionType = ((Control)(object)combCommType).Text.Trim();
			obj.CommissionRate = Conversion.ToDecimal((object)txtcommrate.Text);
			obj.CommissionUom = Conversion.ToDecimal((object)((Control)(object)combcommUOM).Text);
			obj.CommissionAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			obj.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			obj.InvoiceQty = Conversion.ToDecimal((object)txtInvoiceQty.Text);
			obj.InvoiceWeight = Conversion.ToDecimal((object)txtInvoiceWeight.Text);
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			obj.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.TransporterId = Conversion.ToInt(CmbTransporterAc.Value);
			obj.FrieghtAmount = Conversion.ToDecimal((object)txtFreightAmountHeader.Text);
			obj.TransporterRemarks = txtFreightRemarks.Text;
			obj.BillAmount = Conversion.ToDecimal((object)txtBillAmountHeader.Text.Trim());
			obj.InvPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			obj.InvPurchaseInvoiceJournalList = new List<InvPurchaseInvoiceJournal>();
			obj.InvPurchaseInvoiceExpenseList = new List<InvPurchaseInvoiceExpense>();
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			decimal GrossWt = default(decimal);
			if (lstRemoveDetailRecord.Count > 0 && Id > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					obj.InvPurchaseInvoiceDetailList.Add(lstRemoveDetailRecord[j]);
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvPurchaseInvoiceDetail vd = new InvPurchaseInvoiceDetail();
				vd.Id = ((Id != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r3);
				FormHelper.ValidateField(vd.WarehouseId, "Warehouse", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemId, "Item", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemAttributeVarientId, "AttributeVarient", r3.RowIndex);
				FormHelper.ValidateField(vd.VarientEquivalent, "VarientUnit", r3.RowIndex);
				FormHelper.ValidateField(vd.JobLotId, "JobLot", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemQty, "ItemQty", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemWeight, "ItemWeight", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemNetWeight, "NetWeight", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemRate, "ItemRate", r3.RowIndex);
				FormHelper.ValidateField(vd.ItemAmountWithOutDiscount, "ItemAmount WithOut Discount", r3.RowIndex);
				if (vd.ItemDiscountTypeId > 0 || vd.ItemDiscountRate > 0.0 || vd.ItemDiscountAmount > 0.0)
				{
					FormHelper.ValidateField(vd.ItemDiscountTypeId, "Discount Type", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemDiscountRate, "Discount Rate", r3.RowIndex);
					FormHelper.ValidateField(vd.ItemDiscountAmount, "Discount Amount", r3.RowIndex);
				}
				FormHelper.ValidateField(vd.ItemAmount, "ItemAmount", r3.RowIndex);
				if (vd.TaxTypeId > 0 || vd.TaxPercent > 0m || vd.TaxAmount > 0m)
				{
					FormHelper.ValidateField(vd.TaxTypeId, "Tax Name", r3.RowIndex);
					FormHelper.ValidateField(vd.TaxPercent, "Tax Percent", r3.RowIndex);
					FormHelper.ValidateField(vd.TaxAmount, "Tax Amount", r3.RowIndex);
				}
				GrossWt += vd.ItemNetWeight;
				obj.InvPurchaseInvoiceDetailList.Add(vd);
			}
			if (obj.InvPurchaseInvoiceDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			rows = grdChargeToProduct.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0 || Conversion.ToDecimal((object)r2.Cells["Amount"].Value.ToString()) != 0m)
				{
					InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
					PE.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("Item Required in Charge to Product Grid And row No: " + (r2.RowIndex + 1));
					}
					PE.OtherItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					if (Conversion.ToDecimal(r2.Cells["Amount"].Value) == 0m)
					{
						throw new Exception("Amount Required in Charge to Product Grid And row No: " + (r2.RowIndex + 1));
					}
					PE.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					obj.InvPurchaseInvoiceExpenseList.Add(PE);
				}
			}
			rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["AccountId"].Value) != 0 || Conversion.ToInt(r.Cells["GlAccountId"].Value) != 0 || Conversion.ToInt(r.Cells["Credit"].Value) != 0 || Conversion.ToInt(r.Cells["Debit"].Value) != 0)
				{
					InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
					if (Conversion.ToInt(r.Cells["AccountId"].Value) == 0 && Conversion.ToInt(r.Cells["GlAccountId"].Value) == 0)
					{
						throw new Exception("Account Required in Party AddLess Grid And row No: " + (r.RowIndex + 1));
					}
					PJ.ChartofAccountId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
					PJ.SupplierCustomerId = Conversion.ToInt(r.Cells["AccountId"].Value);
					PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDecimal(r.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDecimal(r.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDecimal(r.Cells["Credit"].Value);
					if (PJ.JvDebit == 0m && PJ.JvCredit == 0m)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPartyAddLs).Focus();
						throw new Exception("Credit or Debit is Required in Party AddLess Grid And row No: " + (r.RowIndex + 1));
					}
					if ((PJ.JvDebit > 0m || PJ.JvCredit > 0m) && PJ.JvRemarks == string.Empty)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPartyAddLs).Focus();
						throw new Exception("Remarks Field is Required in Party AddLess Grid And row No: " + (r.RowIndex + 1));
					}
					obj.InvPurchaseInvoiceJournalList.Add(PJ);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
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
			int sucess = InvPurchaseInvoice.Save(obj);
			if (Id == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
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
					}
				}
			}
			Reset();
			if (ChkBok.Checked)
			{
				Id = sucess;
				VoucherHeadIdGet();
				Id = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1859);
			}
			if (ChkPrintslip.Checked)
			{
				GenerateItemSlip(sucess);
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
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			Id = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Id);
			if (invPI == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			RowVersionId = invPI.RowVersion;
			DocDate.Value = invPI.DocDate;
			txtdocno.Text = invPI.DocNo.ToString();
			txtbillno.Text = invPI.ManualBillNo;
			comsupplier.Value = invPI.SupplierCustomerId;
			if (invPI.TaxAccountId > 0)
			{
				CmbTaxAccount.Value = invPI.TaxAccountId;
			}
			txtremarks.Text = invPI.RemarksHeader;
			combpttrm.Value = invPI.PaymentTermsId;
			txtduedays.Text = invPI.DueDays.ToString();
			duedate.Value = invPI.DueDate;
			((Control)(object)combdeliverytrm).Text = invPI.DeliveryTerm;
			combcommAgent.Value = invPI.CommissionAgentId;
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = invPI.CommissionRate.ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)combcommUOM).Text = invPI.CommissionUom.ToString();
			txtcommamount.Text = invPI.CommissionAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtCommissionRemarks.Text = invPI.CommissionRemarks;
			txtInvoiceQty.Text = invPI.InvoiceQty.ToString();
			txtInvoiceWeight.Text = invPI.InvoiceWeight.ToString();
			txtExchangeRate.Text = invPI.ExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			cmbCurrency.Value = invPI.CurrencyId;
			txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			if (invPI.TransporterId > 0)
			{
				CmbTransporterAc.Value = invPI.TransporterId;
			}
			txtFreightAmountHeader.Text = invPI.FrieghtAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtFreightRemarks.Text = invPI.TransporterRemarks;
			txtBillAmountHeader.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtManualTotalBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			combranches.Value = invPI.BranchesId;
			comproject.Value = invPI.ProjectsId;
			Approved = invPI.IsApproved;
			if (invPI.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
			}
			else if (invPI.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			FillDetailFromListCommonForReadById(invPI, dtGrid);
			dtInvExp.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoiceExpenseList.Count; j++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpenseList[j].Id, invPI.InvPurchaseInvoiceExpenseList[j].OtherItemId, invPI.InvPurchaseInvoiceExpenseList[j].OtherItemName, invPI.InvPurchaseInvoiceExpenseList[j].Qty, invPI.InvPurchaseInvoiceExpenseList[j].Rate, invPI.InvPurchaseInvoiceExpenseList[j].Amount, invPI.InvPurchaseInvoiceExpenseList[j].Remarks);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			dtGrdGL.Rows.Clear();
			for (int i = 0; i < invPI.InvPurchaseInvoiceJournalList.Count; i++)
			{
				dtGrdGL.Rows.Add(invPI.InvPurchaseInvoiceJournalList[i].SupplierCustomerId, invPI.InvPurchaseInvoiceJournalList[i].ChartofAccountId, (invPI.InvPurchaseInvoiceJournalList[i].SupplierCustomerId > 0) ? invPI.InvPurchaseInvoiceJournalList[i].SupplierName : ((invPI.InvPurchaseInvoiceJournalList[i].ChartofAccountId > 0) ? invPI.InvPurchaseInvoiceJournalList[i].AccountTitle : ""), invPI.InvPurchaseInvoiceJournalList[i].JvRemarks, invPI.InvPurchaseInvoiceJournalList[i].JvPrcnt, invPI.InvPurchaseInvoiceJournalList[i].JvQty, invPI.InvPurchaseInvoiceJournalList[i].JvRate, invPI.InvPurchaseInvoiceJournalList[i].JvDebit, invPI.InvPurchaseInvoiceJournalList[i].JvCredit);
			}
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			BindGrids();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			OtherItemsProportion();
			FreightHeaderProportion();
			WagesProportion();
			CommissionProportion();
			BillAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete beacause Record has approved");
			}
			if (Id == 0)
			{
				throw new Exception("No Record Found");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvPurchaseInvoice.DeleteByID(UserAccount.ID, Id);
				MessageBox.Show("Delete Voucher Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvPurchaseInvoiceDetail detail, GridEXRow r)
	{
		detail.LineId = r.RowIndex + 1;
		detail.IsApproved = false;
		detail.InvGrnId = Conversion.ToInt(r.Cells["GrnId"].Value);
		detail.InvGrnDetailId = Conversion.ToInt(r.Cells["GrnDetailId"].Value);
		detail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
		detail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
		detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		detail.WareHouseName = Conversion.ToString((object)r.Cells["WarehouseId"].Text);
		detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		detail.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		detail.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
		detail.ItemAttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
		detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
		detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		detail.JobLotDescription = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
		detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
		detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
		detail.ItemRateWithoutRateCut = Conversion.ToDecimal(r.Cells["Rate"].Value);
		detail.RateCut = Conversion.ToDecimal(r.Cells["RateCut"].Value);
		detail.ItemRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
		detail.ItemAmountWithOutDiscount = Conversion.ToDouble(r.Cells["ItemAmountWithoutDisc"].Value);
		detail.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountTypeId"].Value);
		detail.ItemDiscountType = Conversion.ToString(r.Cells["DiscountType"].Value);
		detail.ItemDiscountRate = Conversion.ToDouble(r.Cells["DiscRate"].Value);
		detail.ItemDiscountAmount = Conversion.ToDouble(r.Cells["DiscAmount"].Value);
		detail.ItemAmount = Math.Round(Conversion.ToDecimal(r.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		detail.TaxTypeId = Conversion.ToInt(r.Cells["TaxId"].Value);
		detail.TaxName = Conversion.ToString((object)r.Cells["TaxId"].Text);
		detail.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
		detail.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
		detail.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
		detail.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
		detail.FreightAmount = Conversion.ToDecimal(r.Cells["Freight"].Value);
		detail.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
		detail.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
		detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
		detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
		detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
		detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
		detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
		detail.ItemNetAmount = Math.Round(Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		detail.RemarksDetail = "";
	}

	private void FillDetailFromListCommonForReadById(InvPurchaseInvoice invGrn, DataTable dtGrid, bool AddExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.InvPurchaseInvoiceDetailList : null) == null || invGrn.InvPurchaseInvoiceDetailList.Count == 0)
		{
			return;
		}
		foreach (InvPurchaseInvoiceDetail detail in invGrn.InvPurchaseInvoiceDetailList)
		{
			DataRow dr = dtGrid.NewRow();
			dr["Id"] = detail.Id;
			dr["GrnId"] = detail.InvGrnId;
			dr["GrnDetailId"] = detail.InvGrnDetailId;
			dr["GrnNo"] = detail.GrnNo;
			dr["OrderId"] = detail.PurchaseOrderId;
			dr["OrderDetailId"] = detail.PurchaseOrderDetailId;
			dr["OrderNo"] = detail.OrderNo;
			dr["WarehouseId"] = detail.WarehouseId;
			dr["ItemId"] = detail.ItemId;
			dr["ItemName"] = detail.ItemName;
			dr["ItemCode"] = detail.ItemCode;
			dr["AttributeVarientId"] = detail.ItemAttributeVarientId;
			dr["AttributeVarient"] = detail.ItemAttributeVarient;
			dr["VarientUnit"] = detail.VarientEquivalent;
			dr["ItemQty"] = detail.ItemQty;
			dr["ItemWeight"] = detail.ItemWeight;
			dr["NetWeight"] = detail.ItemNetWeight;
			dr["Rate"] = detail.ItemRateWithoutRateCut;
			dr["RateCut"] = detail.RateCut;
			dr["NetRate"] = detail.ItemRate;
			dr["ItemAmountWithoutDisc"] = ((Conversion.ToDecimal((object)detail.ItemAmountWithOutDiscount) == 0m) ? detail.ItemAmount : Conversion.ToDecimal((object)detail.ItemAmountWithOutDiscount));
			dr["DiscountTypeId"] = detail.ItemDiscountTypeId;
			dr["DiscountType"] = detail.ItemDiscountType;
			dr["DiscRate"] = detail.ItemDiscountRate;
			dr["DiscAmount"] = detail.ItemDiscountAmount;
			dr["ItemAmount"] = detail.ItemAmount;
			dr["TaxId"] = detail.TaxTypeId;
			dr["Tax%"] = detail.TaxPercent;
			dr["TaxAmount"] = detail.TaxAmount;
			dr["JobLotId"] = detail.JobLotId;
			dr["GpDate"] = detail.GpDate;
			dr["GpNo"] = detail.GpNo;
			dr["VehicleNo"] = detail.VehicleNo;
			dr["CityId"] = detail.CityId;
			dr["CityName"] = detail.CityName;
			dr["Expense"] = detail.ExpenseAmount;
			dr["Commission"] = detail.CommissionAmount;
			dr["Freight"] = detail.FreightAmount;
			dr["ItemNetAmount"] = detail.ItemNetAmount;
			dr["FcyAmount"] = detail.FcyAmount;
			if (AddExtraColumns)
			{
				dr["Warehouse"] = detail.WareHouseName;
				dr["TaxName"] = detail.TaxName;
				dr["JobLot"] = detail.JobLotDescription;
			}
			dtGrid.Rows.Add(dr);
		}
	}

	private void HistoryComboBind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			int IdC = Conversion.ToInt(CmbCommissionAgentHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1859"
			};
			DataTable dtCustomer = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"CustomerName"
				}
			};
			DataTable dtCommissionAgent = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"CommissionAgent"
				}
			};
			DataTable dt = InvPurchaseInvoice.DropDownFillFromPurchaseInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CommissionAgent")
					{
						dtCommissionAgent.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "CustomerName", "Supplier Name", ZeroIndex: true);
					if (Id > 0)
					{
						if (dtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbCustomerHistory.Value = Id;
						}
						else
						{
							((Control)(object)CmbCustomerHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbCustomerHistory).DataSource = null;
				}
				if (dtCommissionAgent.Rows.Count > 0)
				{
					DDL.BindDDL(dtCommissionAgent, CmbCommissionAgentHistory, "Id", "CommissionAgent", "CommissionAgent", ZeroIndex: true);
					if (IdC > 0)
					{
						if (dtCommissionAgent.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdC))
						{
							CmbCommissionAgentHistory.Value = IdC;
						}
						else
						{
							((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbCommissionAgentHistory).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
				((UltraGridBase)CmbCommissionAgentHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 1859;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			PI.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			PI.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			PI.CommissionAgentId = Conversion.ToInt(CmbCommissionAgentHistory.Value);
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			DataTable dthistory = InvPurchaseInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtSales = new DataTable();
				dtSales.Columns.Add("Id", typeof(int));
				dtSales.Columns.Add("DocNo", typeof(string));
				dtSales.Columns.Add("DocDate", typeof(DateTime));
				dtSales.Columns.Add("SupplierName", typeof(string));
				dtSales.Columns.Add("TaxAccount", typeof(string));
				dtSales.Columns.Add("ManualBillNo", typeof(string));
				dtSales.Columns.Add("PaymentTerm", typeof(string));
				dtSales.Columns.Add("DueDays", typeof(int));
				dtSales.Columns.Add("DueDate", typeof(DateTime));
				dtSales.Columns.Add("DeliveryTerm", typeof(string));
				dtSales.Columns.Add("InvoiceQty", typeof(int));
				dtSales.Columns.Add("InvoiceWeight", typeof(decimal));
				dtSales.Columns.Add("CommissionAgent", typeof(string));
				dtSales.Columns.Add("CommRate", typeof(decimal));
				dtSales.Columns.Add("CommAmount", typeof(decimal));
				dtSales.Columns.Add("Transporter", typeof(string));
				dtSales.Columns.Add("Frieght", typeof(decimal));
				dtSales.Columns.Add("BillAmount", typeof(decimal));
				dtSales.Columns.Add("FcyCode", typeof(string));
				dtSales.Columns.Add("ExchangeRate", typeof(decimal));
				dtSales.Columns.Add("FcyAmount", typeof(decimal));
				dtSales.Columns.Add("IsApproved", typeof(bool));
				dtSales.Columns.Add("EntryDate", typeof(DateTime));
				dtSales.Columns.Add("EntryUser", typeof(string));
				dtSales.Columns.Add("NoOfAttachments", typeof(int));
				dtSales.Rows.Clear();
				foreach (DataRow row in dthistory.Rows)
				{
					dtSales.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["SupplierName"], row["TaxAccount"], row["ManualBillNo"], row["TermsDescription"], row["DueDays"], Conversion.ToDateTime(row["DueDate"]).ToShortDateString(), row["DeliveryTerm"], row["InvoiceQty"], row["InvoiceWeight"], row["CommissionAgent"], row["CommissionRate"], row["CommissionAmount"], row["Transporter"], row["FrieghtAmount"], row["BillAmount"], row["CurrencyName"], row["ExchangeRate"], row["FcyAmount"], row["IsApproved"], row["EntryDate"], row["UserName"], row["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtSales;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
				grdDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "IsApproved" };
			if (!HasMultiCurrencyFeature)
			{
				hiddenColumns.AddRange(new string[3] { "FcyCode", "ExchangeRate", "FcyAmount" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.SetColumnWidth(grdHistory, "BillAmount", 100);
			GridEX_Helper.SetColumnWidth(grdHistory, "CommRate", 80);
			GridEX_Helper.SetColumnWidth(grdHistory, "CommAmount", 100);
			GridEX_Helper.SetColumnWidth(grdHistory, "InvoiceQty", 80);
			GridEX_Helper.SetColumnWidth(grdHistory, "InvoiceWeight", 90);
			GridEX_Helper.SetColumnWidth(grdHistory, "Frieght", 80);
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print-1859", 80, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "PrintA", "Print-1859A", 90, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Voucher", "Voucher", 70, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
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
			cmbDateTypeHistory.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1856);
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
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int RId = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					Id = RId;
					ReadById(Id);
				}
				else if (e.Column.Key == "Print")
				{
					GenerateItemSlip(RId);
				}
				else if (e.Column.Key == "PrintA")
				{
					GenerateCustomerSlip(RId);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(RId, 1859)), 1859);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			InvPurchaseInvoice Obj = InvPurchaseInvoice.GetByID(Id);
			if (Obj != null && Obj.InvPurchaseInvoiceDetailList.Count == 0)
			{
				grdDetailHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtGrid.Clone();
			dtDetailHistory.Columns.Add("Warehouse", typeof(string));
			dtDetailHistory.Columns.Add("TaxName", typeof(string));
			dtDetailHistory.Columns.Add("JobLot", typeof(string));
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			grdDetailHistory.DataSource = dtDetailHistory;
			grdDetailHistory.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			grdSettings(grdDetailHistory);
			grdDetailHistory.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetailHistory.RootTable.Columns["TaxId"].Visible = false;
			grdDetailHistory.RootTable.Columns["JobLotId"].Visible = false;
			grdDetailHistory.RootTable.Columns["Warehouse"].Position = grdDetailHistory.RootTable.Columns["WarehouseId"].Position + 1;
			grdDetailHistory.RootTable.Columns["TaxName"].Position = grdDetailHistory.RootTable.Columns["TaxId"].Position + 1;
			grdDetailHistory.RootTable.Columns["JobLot"].Position = grdDetailHistory.RootTable.Columns["JobLotId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1859);
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
			if (Id > 0)
			{
				GenerateItemSlip(Id);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateItemSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseExpenseSubReport(obj.Id), "InvPurchaseInvoice_ItemExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1859_InvPurchaseInvoice_DirectSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCustomerSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseSupplierBillSubReport(obj.Id), "InvPurchaseInvoice_SupplierBillExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1859A_InvPurchaseInvoice_DirectSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn294APrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateCustomerSlip(Id);
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailHistory).Name);
			if (fs != null)
			{
				grdDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillAmount()
	{
		try
		{
			txtCommissionAmountHeader.Visible = false;
			labelCommAmtHeader.Visible = false;
			labelCommAmtPlusIcon.Visible = false;
			labelFreightPlusIcon.Visible = false;
			if (grd.GetRows().Count() > 0)
			{
				decimal BillAmount = default(decimal);
				decimal TotalQty = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemQty"));
				decimal TotalWeight = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "NetWeight"));
				decimal ItemAmountWithoutDisc = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmountWithoutDisc"));
				txtItemAmountWithoutDiscHeader.Text = ItemAmountWithoutDisc.ToString(clsGlobalVariables.stringFormatsingle);
				decimal DiscountAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "DiscAmount"));
				txtDiscountHeader.Text = DiscountAmount.ToString(clsGlobalVariables.stringFormatsingle);
				decimal ItemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
				txtItemAmountHeader.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
				Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "FcyAmount"));
				decimal TaxAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "TaxAmount"));
				txtTaxAmountHeader.Text = TaxAmount.ToString(clsGlobalVariables.stringFormatsingle);
				decimal ExpAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdChargeToProduct, "Amount"));
				txtOtherItemGridAmountHeader.Text = ExpAmount.ToString(clsGlobalVariables.stringFormatsingle);
				BillAmount = ItemAmount + ExpAmount + TaxAmount;
				decimal JournalDebit = default(decimal);
				decimal JournalCredit = default(decimal);
				GridEXRow[] rows = grdPartyAddLs.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0 || Conversion.ToInt((object)r.Cells["GlAccountId"].Value.ToString()) > 0)
					{
						JournalDebit += Conversion.ToDecimal((object)r.Cells["Debit"].Value.ToString());
						JournalCredit += Conversion.ToDecimal((object)r.Cells["Credit"].Value.ToString());
					}
				}
				txtPartyAddLessGridAmountHeader.Text = (JournalDebit - JournalCredit).ToString(clsGlobalVariables.stringFormatsingle);
				BillAmount = BillAmount + JournalDebit - JournalCredit;
				decimal TotalNetAmount = BillAmount;
				if (Conversion.ToInt(CmbTransporterAc.Value) > 0 && Conversion.ToInt(comsupplier.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)comsupplier).SelectedRow.Cells[3].Value) == Conversion.ToInt(CmbTransporterAc.Value))
				{
					decimal FrieghtHeader = Conversion.ToDecimal((object)txtFreightAmountHeader.Text.Trim());
					BillAmount += FrieghtHeader;
					labelFreightPlusIcon.Visible = true;
				}
				if (((UltraGridBase)combcommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m && Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
				{
					decimal CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
					BillAmount += CommAmount;
					txtCommissionAmountHeader.Text = CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtCommissionAmountHeader.Visible = true;
					labelCommAmtHeader.Visible = true;
					labelCommAmtPlusIcon.Visible = true;
				}
				txtBillAmountHeader.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtInvoiceQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtInvoiceWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m && TotalNetAmount > 0m)
				{
					txtFcyAmount.Text = (TotalNetAmount / Conversion.ToDecimal((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
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
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridItemNetAmountCalculate()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				CalculateRowNetAmount(r);
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateRowNetAmount(GridEXRow r)
	{
		r.Cells["ItemNetAmount"].Value = Conversion.ToDouble(r.Cells["ItemAmount"].Value) + Conversion.ToDouble(r.Cells["TaxAmount"].Value) + Conversion.ToDouble(r.Cells["Expense"].Value) + Conversion.ToDouble(r.Cells["Freight"].Value) + Conversion.ToDouble(r.Cells["Commission"].Value);
	}

	private void OtherItemsProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)grdChargeToProduct.GetTotal(grdChargeToProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			decimal ItemQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				decimal Qty = Conversion.ToDecimal((object)obj.Cells["ItemQty"].Value.ToString());
				obj.Cells["Expense"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesProportion()
	{
	}

	private void CommissionProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			decimal CommPercent = Conversion.ToDecimal((object)txtcommrate.Text);
			decimal ItemQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (((Control)(object)combCommType).Text == "Percent")
				{
					decimal ItemAmount = Conversion.ToDecimal((object)r.Cells["ItemAmount"].Value.ToString());
					r.Cells["Commission"].Value = ItemAmount * CommPercent / 100m;
					r.EndEdit();
				}
				else
				{
					decimal Qty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
					r.Cells["Commission"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					r.EndEdit();
				}
			}
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightHeaderProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtFreightAmountHeader.Text.Trim());
			if (TotalAmount > 0m)
			{
				decimal ItemQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Qty = Conversion.ToDecimal(obj.Cells["ItemQty"].Value);
					obj.Cells["Freight"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freight"].Value = 0;
					obj2.EndEdit();
				}
			}
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				FreightHeaderProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransporterAc_Leave(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			GetallTaxType();
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
			decimal CommRate = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
			if (((Control)(object)combCommType).Text == "Flat")
			{
				txtcommamount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage")
			{
				object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
				if (total != null)
				{
					decimal CommAmt = Conversion.ToDecimal((object)total.ToString()) * CommRate / 100m;
					txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			_ = ((Control)(object)combCommType).Text == "Comm Weight";
			CommissionProportion();
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

	private void combcommUOM_Leave_1(object sender, EventArgs e)
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

	private void combCommType_Leave_1(object sender, EventArgs e)
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

	private void combcommAgent_Leave(object sender, EventArgs e)
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

	private void RecalculateAllDetailValues()
	{
		CalculateItemAmountWithoutDiscount();
		CalculateDiscountAndItemAmount();
		CalculateTaxAmountAndTotalAmount();
	}

	private void CalculateItemAmountWithoutDiscount()
	{
		try
		{
			decimal qty = Conversion.ToDecimal((object)txtQty.Text);
			decimal unit = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			decimal itemWeight = Conversion.ToDecimal((object)txtItemWeight.Text);
			decimal rate = Math.Max(Conversion.ToDecimal((object)txtNetRate.Text), 0m);
			decimal netWeight = default(decimal);
			decimal amount = default(decimal);
			if (qty > 0m && unit > 0m)
			{
				netWeight = unit * qty * itemWeight;
				amount = Math.Round(unit * qty * rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			txtNetWeight.Text = netWeight.ToString("##,#.##");
			txtNetRate.Text = rate.ToString();
			txtItemAmountWithOutDiscount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDiscountAndItemAmount()
	{
		try
		{
			decimal itemAmount = Conversion.ToDecimal((object)txtItemAmountWithOutDiscount.Text);
			decimal discountRate = Conversion.ToDecimal((object)txtDiscountRate.Text);
			decimal discountAmount = Conversion.ToDecimal((object)txtDiscountAmount.Text);
			bool num = ((Control)(object)CmbDiscountType).Text == "Percent";
			string activeTag = Conversion.ToString(base.ActiveControl?.Tag);
			if (num)
			{
				if (activeTag == "DiscPercent")
				{
					discountAmount = Math.Round(itemAmount * discountRate / 100m, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else
				{
					discountRate = ((itemAmount > 0m) ? Math.Round(discountAmount * 100m / itemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForRate, MidpointRounding.AwayFromZero) : 0m);
				}
				if (discountRate > 99m)
				{
					MessageBox.Show("Discount Percentage Cannot greater than 99");
					discountRate = 99m;
					discountAmount = Math.Round(itemAmount * discountRate / 100m, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else if (activeTag == "DiscPercent")
			{
				discountAmount = discountRate;
			}
			else
			{
				discountRate = discountAmount;
			}
			txtDiscountRate.Text = discountRate.ToString("#,##0.###");
			txtDiscountAmount.Text = discountAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtAmount.Text = (itemAmount - discountAmount).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmountAndTotalAmount()
	{
		try
		{
			double itemAmount = Conversion.ToDouble((object)txtAmount.Text);
			double taxPercent = 0.0;
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				taxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				if (taxPercent <= 0.0)
				{
					taxPercent = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value);
					txtTaxPercent.Text = taxPercent.ToString("#,##0.###");
				}
				taxPercent = Math.Min(taxPercent, 100.0);
				double taxAmount = itemAmount * taxPercent / 100.0;
				txtTaxAmount.Text = taxAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = (itemAmount + taxAmount).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtTotalAmount.Text = itemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetRateCalculation()
	{
		try
		{
			if (Conversion.ToDouble((object)txtRate.Text) > 0.0)
			{
				double rate = Conversion.ToDouble((object)txtRate.Text);
				double rateCut = Conversion.ToDouble((object)txtRateCut.Text);
				txtNetRate.Text = (rate - rateCut).ToString("#,##0.####");
			}
			RecalculateAllDetailValues();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		RecalculateAllDetailValues();
	}

	private void txtNetRate_TextChanged(object sender, EventArgs e)
	{
		RecalculateAllDetailValues();
	}

	private void txtDiscountRate_TextChanged(object sender, EventArgs e)
	{
		RecalculateAllDetailValues();
	}

	private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
	{
		RecalculateAllDetailValues();
	}

	private void CmbDiscountType_Leave(object sender, EventArgs e)
	{
		RecalculateAllDetailValues();
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		NetRateCalculation();
	}

	private void txtratecut_TextChanged(object sender, EventArgs e)
	{
		NetRateCalculation();
	}

	private void CmbAttributeVarient_Leave(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttributeVarient).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			CalculateItemAmountWithoutDiscount();
			CalculateDiscountAndItemAmount();
			CalculateTaxAmountAndTotalAmount();
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
				CalculateTaxAmountAndTotalAmount();
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
				CalculateTaxAmountAndTotalAmount();
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

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combdeliverytrm).Text == "Ponch")
			{
				CmbTransporterAc.Value = OutwardFreightAccountId;
				return;
			}
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtFreightAmountHeader.Text = "0";
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
			DueDateGenerate();
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
				throw new Exception("Due Date Can't less Than DocDate");
			}
			int days = (duedate.Value.Date - DocDate.Value.Date).Days;
			txtduedays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_TextChanged(object sender, EventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtgatepassno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSettlementDiscountHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightAmountHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnLoadGdn_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null)
				{
					if (Conversion.ToInt(item.Cells["OrderId"].Value) == 0 && Conversion.ToInt(item.Cells["GrnId"].Value) == 0)
					{
						MessageBox.Show("You cannot Load Order because Manual record exist");
						return;
					}
					if (Conversion.ToInt(item.Cells["GrnId"].Value) > 0)
					{
						MessageBox.Show("You cannot Load Order because record against Grn exist");
						return;
					}
				}
			}
			LoadTransactions = new LoadPurchaseOrderConcrete(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
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
			DataTable dtpurchaseinvoice = LoadTransactions.dtLoader;
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
				if (Conversion.ToInt(CmbTransporterAc.Value) > 0 && Conversion.ToInt(dtpurchaseinvoice.Rows[0]["TransporterId"]) != Conversion.ToInt(CmbTransporterAc.Value))
				{
					MessageBox.Show("Already Loaded Rows Have Different Transporter. So you Can't Load Rows Of Different Transporter!");
					return;
				}
				if (Conversion.ToInt(combdeliverytrm.Value) > 0 && Conversion.ToString(dtpurchaseinvoice.Rows[0]["DeliveryTerm"]) != ((Control)(object)combdeliverytrm).Text)
				{
					MessageBox.Show("Already Loaded Rows Have Different Delivery Term. So you Can't Load Rows Of Different Delivery Term!");
					return;
				}
			}
			comsupplier.Value = Conversion.ToInt((object)dtpurchaseinvoice.Rows[0]["SupplierCustomerId"].ToString());
			((Control)(object)comsupplier).Enabled = false;
			combpttrm.Value = Conversion.ToInt((object)dtpurchaseinvoice.Rows[0]["PaymentTermsId"].ToString());
			txtduedays.Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["DueDays"].ToString());
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)dtpurchaseinvoice.Rows[0]["DeliveryTerm"].ToString());
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
			if (Conversion.ToDouble(dtpurchaseinvoice.Rows[0]["CommissionRate"]) > 0.0)
			{
				txtcommrate.Enabled = false;
				txtcommrate.Clear();
				txtcommrate.Text = dtpurchaseinvoice.Rows[0]["CommissionRate"].ToString();
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
			TotalCommissionAmount();
			OtherItemsProportion();
			FreightHeaderProportion();
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
						if (Conversion.ToInt(item.Cells["OrderId"].Value) != Conversion.ToInt(dtpurchaseinvoice.Rows[i]["Id"]))
						{
							MessageBox.Show("Data against another OrderNo Already Exist in Detail");
							return;
						}
						if (Conversion.ToInt(item.Cells["OrderDetailId"].Value) == Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, 0, 0, 0, dtpurchaseinvoice.Rows[i]["Id"], dtpurchaseinvoice.Rows[i]["DetailId"], dtpurchaseinvoice.Rows[i]["DocNo"], 0, dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemCodeNew"], dtpurchaseinvoice.Rows[i]["ItemAttributeVarientId"], dtpurchaseinvoice.Rows[i]["ItemAttributeVarient"], dtpurchaseinvoice.Rows[i]["VarientEquivalent"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["ItemWeight"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["ItemRate"], 0, dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["BalanceAmount"], dtpurchaseinvoice.Rows[i]["TaxTypeId"], dtpurchaseinvoice.Rows[i]["TaxPercent"], dtpurchaseinvoice.Rows[i]["TaxAmount"], dtpurchaseinvoice.Rows[i]["JobLotId"], DateTime.Now, 0, "", dtpurchaseinvoice.Rows[i]["CityId"], dtpurchaseinvoice.Rows[i]["CityName"], 0, 0, 0, 0, 0);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings(grd);
			CommissionProportion();
			OtherItemsProportion();
			FreightHeaderProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string ID)
	{
	}

	private void btnLoadGrn_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.GetRow(0);
				if (item != null)
				{
					if (Conversion.ToInt(item.Cells["GrnId"].Value) == 0 && Conversion.ToInt(item.Cells["OrderId"].Value) == 0)
					{
						MessageBox.Show("You cannot Load Grn because Manual record exist");
						return;
					}
					if (Conversion.ToInt(item.Cells["GrnId"].Value) == 0 && Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
					{
						MessageBox.Show("You cannot Load Grn because record against Order exist");
						return;
					}
				}
			}
			GrnLoad = new LoadGrnForInvoice(UserAccount);
			GrnLoad.ShowDialog();
			GrnLoad.BringToFront();
			LoadInGridDetailForGrn();
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void LoadInGridDetailForGrn()
	{
		try
		{
			DataTable dtLoader = GrnLoad.dtLoader;
			if (dtLoader.Rows.Count <= 0)
			{
				return;
			}
			tabControl2.SelectedIndex = 0;
			if (DocumentType == 0)
			{
				DocumentType = ((Conversion.ToInt(dtLoader.Rows[0]["GrnTypeId"]) == 0) ? 1 : 2);
			}
			int id = ((Conversion.ToInt(dtLoader.Rows[0]["GrnTypeId"]) == 0) ? 1 : 2);
			if (DocumentType != id)
			{
				MessageBox.Show("Already Loaded Rows are of different type. so you cant load more rows", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			((Control)(object)comsupplier).Enabled = false;
			((Control)(object)combdeliverytrm).Enabled = false;
			if (grd.RowCount > 0)
			{
				if (Conversion.ToInt(comsupplier.Value) > 0 && Conversion.ToInt(dtLoader.Rows[0]["SupplierCustomerId"]) != Conversion.ToInt(comsupplier.Value))
				{
					MessageBox.Show("Already Loaded Rows Have Different Supplier. So you Can't Load Rows Of Different Supplier!");
					return;
				}
				if (Conversion.ToInt(CmbTransporterAc.Value) > 0 && Conversion.ToInt(dtLoader.Rows[0]["TransporterId"]) != Conversion.ToInt(CmbTransporterAc.Value))
				{
					MessageBox.Show("Already Loaded Rows Have Different Transporter. So you Can't Load Rows Of Different Transporter!");
					return;
				}
				if (Conversion.ToInt(combdeliverytrm.Value) > 0 && Conversion.ToString(dtLoader.Rows[0]["DeliveryTerm"]) != ((Control)(object)combdeliverytrm).Text)
				{
					MessageBox.Show("Already Loaded Rows Have Different Delivery Term. So you Can't Load Rows Of Different Delivery Term!");
					return;
				}
			}
			comsupplier.Value = Conversion.ToInt(dtLoader.Rows[0]["SupplierCustomerId"]);
			((Control)(object)combdeliverytrm).Text = dtLoader.Rows[0]["DeliveryTerm"].ToString();
			if (Conversion.ToInt(dtLoader.Rows[0]["OrderId"]) > 0)
			{
				((Control)(object)combpttrm).Enabled = false;
				combpttrm.Value = Conversion.ToInt(dtLoader.Rows[0]["PaymentTermsId"]);
				txtduedays.Text = dtLoader.Rows[0]["DueDays"].ToString();
			}
			double TotalFreightOfDifferentGrn = 0.0;
			foreach (var group2 in from row in dtLoader.AsEnumerable()
				group row by row.Field<int>("Id") into @group
				select new
				{
					Id = @group.Key,
					TotalFreight = @group.Select((DataRow row) => row.Field<decimal>("FrieghtAmountHeader")).FirstOrDefault()
				})
			{
				TotalFreightOfDifferentGrn += Conversion.ToDouble((object)group2.TotalFreight);
			}
			txtFreightAmountHeader.Text = TotalFreightOfDifferentGrn.ToString("#,##0.###");
			if (TotalFreightOfDifferentGrn > 0.0)
			{
				CmbTransporterAc.Value = Conversion.ToInt(dtLoader.Rows[0]["TransporterId"]);
			}
			txtremarks.Text = dtLoader.Rows[0]["RemarksHeader"].ToString();
			if (Conversion.ToInt(dtLoader.Rows[0]["CommissionAgentId"]) > 0)
			{
				((Control)(object)combcommAgent).Enabled = false;
				combcommAgent.Value = Conversion.ToInt(dtLoader.Rows[0]["CommissionAgentId"]);
			}
			else
			{
				((Control)(object)combcommAgent).Enabled = true;
			}
			if (Conversion.ToString(dtLoader.Rows[0]["CommissionType"]) != null && Conversion.ToString(dtLoader.Rows[0]["CommissionType"]) != string.Empty)
			{
				((Control)(object)combCommType).Enabled = false;
				((Control)(object)combCommType).Text = dtLoader.Rows[0]["CommissionType"].ToString();
			}
			else
			{
				((Control)(object)combCommType).Enabled = true;
			}
			if (Conversion.ToDouble(dtLoader.Rows[0]["CommissionRate"]) > 0.0)
			{
				txtcommrate.Enabled = false;
				txtcommrate.Clear();
				txtcommrate.Text = dtLoader.Rows[0]["CommissionRate"].ToString();
			}
			else
			{
				txtcommrate.Enabled = true;
			}
			txtCommissionRemarks.Text = dtLoader.Rows[0]["CommissionRemarks"].ToString();
			bool flag = false;
			for (int i = 0; i < dtLoader.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["GrnDetailId"].Value) == Conversion.ToInt(dtLoader.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, dtLoader.Rows[i]["Id"], dtLoader.Rows[i]["DetailId"], dtLoader.Rows[i]["DocNo"], dtLoader.Rows[i]["OrderId"], dtLoader.Rows[i]["OrderDetailId"], dtLoader.Rows[i]["OrderNo"], dtLoader.Rows[i]["WarehouseId"], dtLoader.Rows[i]["ItemId"], dtLoader.Rows[i]["ItemName"], dtLoader.Rows[i]["ItemCode"], dtLoader.Rows[i]["ItemAttributeVarientId"], dtLoader.Rows[i]["ItemAttributeVarient"], dtLoader.Rows[i]["VarientEquivalent"], dtLoader.Rows[i]["ItemQty"], dtLoader.Rows[i]["ItemWeight"], dtLoader.Rows[i]["NetWeight"], dtLoader.Rows[i]["ItemRate"], 0, dtLoader.Rows[i]["ItemRate"], dtLoader.Rows[i]["ItemAmount"], dtLoader.Rows[i]["TaxTypeId"], dtLoader.Rows[i]["TaxPercent"], dtLoader.Rows[i]["TaxAmount"], dtLoader.Rows[i]["JoblotId"], Conversion.ToDateTime(dtLoader.Rows[i]["GpDate"]).ToShortDateString(), dtLoader.Rows[i]["GpNo"], dtLoader.Rows[i]["VehicleNo"], dtLoader.Rows[i]["CityId"], dtLoader.Rows[i]["CityName"], 0, 0, 0, 0, 0);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings(grd);
			if (Conversion.ToInt(dtLoader.Rows[0]["CurrencyIdHeader"]) > 0)
			{
				cmbCurrency.Value = Conversion.ToInt((object)dtLoader.Rows[0]["CurrencyIdHeader"].ToString());
			}
			if (Conversion.ToDouble(dtLoader.Rows[0]["ExchangeRateHeader"]) > 0.0)
			{
				txtExchangeRate.Text = Conversion.ToDouble(dtLoader.Rows[0]["ExchangeRateHeader"]).ToString("#,##0.####");
			}
			OtherItemsProportion();
			FreightHeaderProportion();
			TotalCommissionAmount();
			CommissionProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			((Control)(object)comsupplier).Enabled = true;
			((Control)(object)combdeliverytrm).Enabled = true;
			((Control)(object)CmbTransporterAc).Enabled = true;
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdChargeToProduct).Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grdPartyAddLs).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdChargeToProduct).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPartyAddLs).Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					comWarehouse.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btn294APrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnLoadAll_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbDateTypeHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
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
					cmbDateTypeHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 1856");
			dt.Rows.Add("Alt+2", "For Print Slip 1856A");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(item);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(item);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse"))
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtwareh, "Id", "WareHouseName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					item.Cells["WarehouseId"].Value = Id;
					item.Cells["Warehouse"].Value = Name;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot"))
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtjoblot, "Id", "JobLotDescription");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					Name = Conversion.ToString((object)grdPopUp2.ReturnName);
					item.Cells["JobLotId"].Value = Id;
					item.Cells["JobLot"].Value = Name;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CityName" || grd.CurrentColumn.Key == "CityId"))
				{
					GrdPopUp grdPopUp3 = new GrdPopUp(dtCity, "Id", "CityName");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					Name = Conversion.ToString((object)grdPopUp3.ReturnName);
					item.Cells["CityId"].Value = Id;
					item.Cells["CityName"].Value = Name;
				}
				grd.UpdateData();
				TotalCommissionAmount();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPartyAddLs.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPartyAddLs.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPartyAddLs.CurrentColumn.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						item.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (grdPartyAddLs.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				BillAmount();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grdPartyAddLs.CurrentColumn != null && (grdPartyAddLs.CurrentColumn.Key == "AccountId" || grdPartyAddLs.CurrentColumn.Key == "GlAccountId" || grdPartyAddLs.CurrentColumn.Key == "AccountTitle"))
				{
					if (SubsidiaryAccountAllownOnVouchers)
					{
						DataTable dt = CommonServices.GetVendorsAndCustomers();
						GrdPopUp obj = new GrdPopUp(dt, "Id", "CompanyName");
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						obj.BringToFront();
						Id = Conversion.ToInt((object)obj.ReturnId);
						Name = Conversion.ToString((object)obj.ReturnName);
						if (Id > 0)
						{
							item.Cells["AccountId"].Value = Id;
							item.Cells["AccountTitle"].Value = Name;
							DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
							int GlAccountId = ((foundRow != null) ? Conversion.ToInt(foundRow["GlAccountId"]) : 0);
							item.Cells["GlAccountId"].Value = GlAccountId;
							if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(item.Cells["AccountId"].Value))
							{
								MessageBox.Show("Selected Account Can not be Same As Supplier Account");
								item.Cells["AccountId"].Value = 0;
								item.Cells["GlAccountId"].Value = 0;
								item.Cells["AccountTitle"].Value = "";
								return;
							}
						}
						else
						{
							item.Cells["AccountId"].Value = 0;
							item.Cells["GlAccountId"].Value = 0;
							item.Cells["AccountTitle"].Value = "";
						}
					}
					else
					{
						GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						Id = Conversion.ToInt((object)obj2.ReturnId);
						Name = Conversion.ToString((object)obj2.ReturnName);
						if (Id > 0)
						{
							item.Cells["GlAccountId"].Value = Id;
							item.Cells["AccountId"].Value = 0;
							item.Cells["AccountTitle"].Value = Name;
							if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
							{
								MessageBox.Show("Selected Account Can not be Same As Supplier Account");
								item.Cells["GlAccountId"].Value = 0;
								item.Cells["AccountId"].Value = 0;
								item.Cells["AccountTitle"].Value = "";
								return;
							}
						}
						else
						{
							item.Cells["GlAccountId"].Value = 0;
							item.Cells["AccountId"].Value = 0;
							item.Cells["AccountTitle"].Value = "";
						}
					}
				}
				grdPartyAddLs.UpdateData();
				CommonServices.GridAutoAdjustmentNew(grdPartyAddLs);
				TotalCommissionAmount();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			((Control)(object)grdPartyAddLs).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdChargeToProduct.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdChargeToProduct.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdChargeToProduct.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtInvExp.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
					OtherItemsProportion();
					FreightHeaderProportion();
					txtExchangeRate_TextChanged(null, null);
				}
				if (grdChargeToProduct.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
					grdChargeToProduct.UpdateData();
					OtherItemsProportion();
					FreightHeaderProportion();
					txtExchangeRate_TextChanged(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdChargeToProduct.UpdateData();
				OtherItemsProportion();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdChargeToProduct.UpdateData();
				OtherItemsProportion();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grdChargeToProduct.CurrentColumn != null && (grdChargeToProduct.CurrentColumn.Key == "ItemId" || grdChargeToProduct.CurrentColumn.Key == "ItemName"))
				{
					Conversion.ToInt(item.Cells["ItemId"].Value);
					GrdPopUp grdPopUp = new GrdPopUp(dtOtherItem, "Id", "OtherItemName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					item.Cells["ItemId"].Value = Id;
					item.Cells["ItemName"].Value = Name;
				}
				grdChargeToProduct.UpdateData();
				CommonServices.GridAutoAdjustmentNew(grdChargeToProduct);
			}
			((Control)(object)grdChargeToProduct).Focus();
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
						Id = CurrId;
						ReadById(Id);
						comsupplier.Focus();
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (formright.DoHavePrintRights)
					{
						GenerateItemSlip(CurrId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "PrintA")
				{
					if (formright.DoHavePrintRights)
					{
						GenerateCustomerSlip(CurrId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					if (formright.DoHavePrintRights)
					{
						CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(CurrId, 1859)), 1859);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1859);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Id = CurrId;
				ReadById(Id);
				comsupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtManualTotalBillAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtManualTotalBillAmount.Text) > 0.0)
			{
				CalculateWithManualOrderAmountTotal();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtManualTotalBillAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWithManualOrderAmountTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWithManualOrderAmountTotal()
	{
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			decimal ManualBillAmount = Conversion.ToDecimal((object)txtManualTotalBillAmount.Text);
			decimal ItemAmountWithoutDisc = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmountWithoutDisc"));
			txtItemAmountWithoutDiscHeader.Text = ItemAmountWithoutDisc.ToString(clsGlobalVariables.stringFormatsingle);
			decimal TaxAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "TaxAmount"));
			decimal OtherItemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdChargeToProduct, "Amount"));
			decimal ActualBillAmount = ItemAmountWithoutDisc + TaxAmount + OtherItemAmount;
			decimal JournalDebit = default(decimal);
			decimal JournalCredit = default(decimal);
			GridEXRow[] rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 || Conversion.ToInt(r.Cells["GlAccountId"].Value) > 0)
				{
					JournalDebit += Conversion.ToDecimal(r.Cells["Debit"].Value);
					JournalCredit += Conversion.ToDecimal(r.Cells["Credit"].Value);
				}
			}
			ActualBillAmount = ActualBillAmount + JournalDebit - JournalCredit;
			if (Conversion.ToInt(CmbTransporterAc.Value) > 0 && Conversion.ToInt(comsupplier.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)comsupplier).SelectedRow.Cells[3].Value) == Conversion.ToInt(CmbTransporterAc.Value))
			{
				decimal FreightAmount = Conversion.ToDecimal((object)txtFreightAmountHeader.Text);
				ActualBillAmount += FreightAmount;
			}
			if (((UltraGridBase)combcommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m && Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
			{
				decimal CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
				ActualBillAmount += CommAmount;
			}
			if (ManualBillAmount <= 0m)
			{
				txtDiscountHeader.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = Math.Round(ActualBillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = txtBillAmountHeader.Text;
			}
			else if (ManualBillAmount > ActualBillAmount)
			{
				MessageBox.Show($"Manual Bill Amount {ManualBillAmount} Can't be Greater Than Expected Bill Amount {ActualBillAmount}");
				txtDiscountHeader.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = Math.Round(ActualBillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = txtBillAmountHeader.Text;
			}
			else
			{
				txtDiscountHeader.Text = (ActualBillAmount - ManualBillAmount).ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = Math.Round(ManualBillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			PropotionateDiscountByManualOrderAmountInGrid();
			Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "DiscAmount"));
			txtItemAmountHeader.Text = GridEX_Helper.GetColumnSum(grd, "ItemAmount").ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateDiscountByManualOrderAmountInGrid()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			decimal FooterDisAmt = Conversion.ToDecimal((object)txtDiscountHeader.Text);
			decimal totalGridAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmountWithoutDisc"));
			decimal totalCalculatedDiscount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				decimal RowItemAmount2 = Conversion.ToDecimal((object)r2.Cells["ItemAmountWithoutDisc"].Value.ToString());
				decimal RowDiscountAmount = ((totalGridAmount > 0m) ? (RowItemAmount2 / totalGridAmount * 100m) : 0m) * FooterDisAmt / 100m;
				decimal DisRate2 = default(decimal);
				r2.BeginEdit();
				decimal ItemAmount2 = default(decimal);
				r2.Cells["DiscAmount"].Value = Math.Round(RowDiscountAmount, 2);
				ItemAmount2 = RowItemAmount2 - RowDiscountAmount;
				r2.Cells["ItemAmount"].Value = ItemAmount2;
				if (Conversion.ToInt(r2.Cells["DiscountTypeId"].Value) == 0)
				{
					r2.Cells["DiscountTypeId"].Value = 2;
					r2.Cells["DiscountType"].Value = "Percent";
				}
				if (Conversion.ToInt(r2.Cells["DiscountTypeId"].Value) == 1)
				{
					DisRate2 = RowDiscountAmount;
					r2.Cells["DisRate"].Value = Math.Round(DisRate2, 2);
				}
				else if (Conversion.ToInt(r2.Cells["DiscountTypeId"].Value) == 2)
				{
					if (FooterDisAmt > 0m && RowDiscountAmount > 0m)
					{
						DisRate2 = ((RowItemAmount2 > 0m) ? (RowDiscountAmount / RowItemAmount2 * 100m) : 0m);
						r2.Cells["DiscRate"].Value = Math.Round(DisRate2, 2);
					}
					else
					{
						DisRate2 = default(decimal);
						r2.Cells["DiscRate"].Value = 0;
					}
				}
				else
				{
					DisRate2 = default(decimal);
					r2.Cells["DiscRate"].Value = 0;
				}
				if (RowItemAmount2 > 0m && RowDiscountAmount > 0m)
				{
					ItemAmount2 = RowItemAmount2 - RowDiscountAmount;
					r2.Cells["ItemAmount"].Value = ItemAmount2;
				}
				else if (RowItemAmount2 > 0m)
				{
					ItemAmount2 = RowItemAmount2;
					r2.Cells["ItemAmount"].Value = ItemAmount2;
				}
				else
				{
					r2.Cells["ItemAmount"].Value = 0;
				}
				CalculateRowNetAmount(r2);
				r2.EndEdit();
				totalCalculatedDiscount += RowDiscountAmount;
			}
			if (!(Math.Abs(totalCalculatedDiscount - FooterDisAmt) > 0.01m))
			{
				return;
			}
			decimal adjustmentFactor = ((totalCalculatedDiscount > 0m) ? (FooterDisAmt / totalCalculatedDiscount) : 0m);
			if (adjustmentFactor > 0m)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					decimal RowItemAmount = Conversion.ToDecimal((object)r.Cells["ItemAmount"].Value.ToString());
					decimal UpdatedDiscountAmount = Math.Round(Conversion.ToDecimal((object)r.Cells["DiscAmount"].Value.ToString()) * adjustmentFactor, 2);
					r.BeginEdit();
					r.Cells["DiscAmount"].Value = UpdatedDiscountAmount;
					r.Cells["ItemAmount"].Value = RowItemAmount - UpdatedDiscountAmount;
					decimal DisRate = default(decimal);
					decimal ItemAmount = default(decimal);
					if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 1)
					{
						DisRate = UpdatedDiscountAmount;
						r.Cells["DisRate"].Value = Math.Round(DisRate, 2);
					}
					else if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 2)
					{
						if (RowItemAmount > 0m && UpdatedDiscountAmount > 0m)
						{
							DisRate = ((RowItemAmount > 0m) ? (UpdatedDiscountAmount / RowItemAmount * 100m) : 0m);
							r.Cells["DiscRate"].Value = Math.Round(DisRate, 2);
						}
						else
						{
							DisRate = default(decimal);
							r.Cells["DiscRate"].Value = 0;
						}
					}
					else
					{
						DisRate = default(decimal);
						r.Cells["DiscRate"].Value = 0;
					}
					ItemAmount = ((RowItemAmount > 0m && UpdatedDiscountAmount > 0m) ? (RowItemAmount - UpdatedDiscountAmount) : ((!(RowItemAmount > 0m)) ? default(decimal) : RowItemAmount));
					r.Cells["ItemAmount"].Value = ItemAmount;
					CalculateRowNetAmount(r);
					r.EndEdit();
				}
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtManualTotalBillAmount_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Expected O, but got Unknown
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Expected O, but got Unknown
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Expected O, but got Unknown
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Expected O, but got Unknown
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Expected O, but got Unknown
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Expected O, but got Unknown
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Expected O, but got Unknown
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Expected O, but got Unknown
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Expected O, but got Unknown
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c33: Expected O, but got Unknown
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Expected O, but got Unknown
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Expected O, but got Unknown
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db4: Expected O, but got Unknown
		//IL_0de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0deb: Expected O, but got Unknown
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Expected O, but got Unknown
		//IL_0e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Expected O, but got Unknown
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e64: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_1708: Unknown result type (might be due to invalid IL or missing references)
		//IL_1712: Expected O, but got Unknown
		//IL_19f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Expected O, but got Unknown
		//IL_1a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a11: Expected O, but got Unknown
		//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a28: Expected O, but got Unknown
		//IL_1cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbb: Expected O, but got Unknown
		//IL_1cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd2: Expected O, but got Unknown
		//IL_1cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce9: Expected O, but got Unknown
		//IL_bada: Unknown result type (might be due to invalid IL or missing references)
		//IL_bae4: Expected O, but got Unknown
		//IL_bb08: Unknown result type (might be due to invalid IL or missing references)
		//IL_bb12: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdChargeToProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPartyAddLs_DesignTimeLayout = new GridEXLayout();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
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
		Appearance appearance107 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdChargeToProduct = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdPartyAddLs = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.txtBillAmountHeader = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.txtFreightRemarks = new System.Windows.Forms.TextBox();
		this.label79 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbTransporterAc = new UltraCombo();
		this.txtFreightAmountHeader = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CurrencyInfoBox = new System.Windows.Forms.GroupBox();
		this.cmbCurrency = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label20 = new System.Windows.Forms.Label();
		this.combcommAgent = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.combCommType = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label49 = new System.Windows.Forms.Label();
		this.cmbtaxtype = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtDiscountRate = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetRate = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.CmbDiscountType = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.CmbAttributeVarient = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtRateCut = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.comjobLot = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtCity = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.comWarehouse = new UltraCombo();
		this.comItem = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtItemAmountWithOutDiscount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.txtInvoiceQty = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.comsupplier = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.combcommUOM = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnPrintA = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnLoadGrn = new System.Windows.Forms.ToolStripButton();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbCommissionAgentHistory = new UltraCombo();
		this.label72 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label52 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.txtDiscountHeader = new System.Windows.Forms.TextBox();
		this.txtItemAmountHeader = new System.Windows.Forms.TextBox();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.label68 = new System.Windows.Forms.Label();
		this.txtManualTotalBillAmount = new System.Windows.Forms.TextBox();
		this.txtItemAmountWithoutDiscHeader = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtTaxAmountHeader = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtOtherItemGridAmountHeader = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtPartyAddLessGridAmountHeader = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.labelFreightPlusIcon = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtCommissionAmountHeader = new System.Windows.Forms.TextBox();
		this.labelCommAmtHeader = new System.Windows.Forms.Label();
		this.labelCommAmtPlusIcon = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdChargeToProduct).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPartyAddLs).BeginInit();
		this.panel15.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).BeginInit();
		this.panel3.SuspendLayout();
		this.CurrencyInfoBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 761);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1276, 732);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1270, 726);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 295);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1270, 201);
		this.tabControl2.TabIndex = 33;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1262, 172);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1256, 145);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1256, 21);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdChargeToProduct);
		this.tabPage4.Controls.Add(this.panel12);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1262, 172);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Debit To Party (Other Item)";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdChargeToProduct).BackColor = System.Drawing.Color.White;
		grdChargeToProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdChargeToProduct.DesignTimeLayout = grdChargeToProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdChargeToProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Name = "grdChargeToProduct";
		((ScrollableBase)this.grdChargeToProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Size = new System.Drawing.Size(1256, 148);
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).TabIndex = 0;
		this.grdChargeToProduct.TotalRow = (InheritableBoolean)1;
		this.grdChargeToProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdChargeToProduct).VisualStyle = (VisualStyle)4;
		this.grdChargeToProduct.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdChargeToProduct.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdChargeToProduct.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label37);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1256, 21);
		this.panel12.TabIndex = 32;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(5, 1);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 17);
		this.label37.TabIndex = 36;
		this.label37.Text = "Expense Grid";
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdPartyAddLs);
		this.tabPage5.Controls.Add(this.panel15);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1262, 172);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Party Add/Less";
		this.tabPage5.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPartyAddLs).BackColor = System.Drawing.Color.White;
		grdPartyAddLs_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPartyAddLs.DesignTimeLayout = grdPartyAddLs_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPartyAddLs.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Name = "grdPartyAddLs";
		((ScrollableBase)this.grdPartyAddLs).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Size = new System.Drawing.Size(1262, 153);
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).TabIndex = 0;
		this.grdPartyAddLs.TotalRow = (InheritableBoolean)1;
		this.grdPartyAddLs.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPartyAddLs).VisualStyle = (VisualStyle)4;
		this.grdPartyAddLs.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdPartyAddLs.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdPartyAddLs.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label53);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1262, 22);
		this.panel15.TabIndex = 34;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(6, 2);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(147, 17);
		this.label53.TabIndex = 36;
		this.label53.Text = "Party Add/Less Grid";
		this.panel6.Controls.Add(this.PanelMessage);
		this.panel6.Controls.Add(this.label68);
		this.panel6.Controls.Add(this.txtManualTotalBillAmount);
		this.panel6.Controls.Add(this.label52);
		this.panel6.Controls.Add(this.labelCommAmtPlusIcon);
		this.panel6.Controls.Add(this.label45);
		this.panel6.Controls.Add(this.labelFreightPlusIcon);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label36);
		this.panel6.Controls.Add(this.label80);
		this.panel6.Controls.Add(this.label85);
		this.panel6.Controls.Add(this.label86);
		this.panel6.Controls.Add(this.label27);
		this.panel6.Controls.Add(this.labelCommAmtHeader);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.label30);
		this.panel6.Controls.Add(this.label28);
		this.panel6.Controls.Add(this.txtDiscountHeader);
		this.panel6.Controls.Add(this.txtItemAmountWithoutDiscHeader);
		this.panel6.Controls.Add(this.txtItemAmountHeader);
		this.panel6.Controls.Add(this.txtCommissionAmountHeader);
		this.panel6.Controls.Add(this.txtPartyAddLessGridAmountHeader);
		this.panel6.Controls.Add(this.txtOtherItemGridAmountHeader);
		this.panel6.Controls.Add(this.txtTaxAmountHeader);
		this.panel6.Controls.Add(this.txtBillAmountHeader);
		this.panel6.Controls.Add(this.label51);
		this.panel6.Controls.Add(this.label89);
		this.panel6.Controls.Add(this.txtFreightRemarks);
		this.panel6.Controls.Add(this.label79);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterAc);
		this.panel6.Controls.Add(this.txtFreightAmountHeader);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(0, 496);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1270, 230);
		this.panel6.TabIndex = 0;
		this.txtBillAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmountHeader.Location = new System.Drawing.Point(1104, 202);
		this.txtBillAmountHeader.Name = "txtBillAmountHeader";
		this.txtBillAmountHeader.ReadOnly = true;
		this.txtBillAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtBillAmountHeader.TabIndex = 10;
		this.txtBillAmountHeader.TabStop = false;
		this.txtBillAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.BackColor = System.Drawing.Color.Gainsboro;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.Black;
		this.label51.Location = new System.Drawing.Point(959, 202);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(120, 23);
		this.label51.TabIndex = 125;
		this.label51.Text = "Net Bill Amount";
		this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label89.BackColor = System.Drawing.Color.Gainsboro;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label89.Location = new System.Drawing.Point(575, 148);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(238, 26);
		this.label89.TabIndex = 128;
		this.label89.Text = "Carriage Ac &&  Amt (Cr)";
		this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFreightRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarks.Location = new System.Drawing.Point(815, 177);
		this.txtFreightRemarks.Name = "txtFreightRemarks";
		this.txtFreightRemarks.Size = new System.Drawing.Size(451, 23);
		this.txtFreightRemarks.TabIndex = 9;
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(1082, 204);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(21, 18);
		this.label79.TabIndex = 137;
		this.label79.Text = "=";
		this.label24.BackColor = System.Drawing.Color.LightGray;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(575, 177);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(238, 23);
		this.label24.TabIndex = 102;
		this.label24.Text = "Freight Remarks";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbTransporterAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporterAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Location = new System.Drawing.Point(815, 148);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Name = "CmbTransporterAc";
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Leave += new System.EventHandler(CmbTransporterAc_Leave);
		this.txtFreightAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmountHeader.Location = new System.Drawing.Point(1104, 150);
		this.txtFreightAmountHeader.Name = "txtFreightAmountHeader";
		this.txtFreightAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtFreightAmountHeader.TabIndex = 8;
		this.txtFreightAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmountHeader.TextChanged += new System.EventHandler(txtFreightAmountHeader_TextChanged);
		this.txtFreightAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmountHeader_KeyPress);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.CurrencyInfoBox);
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1270, 270);
		this.panel3.TabIndex = 29;
		this.CurrencyInfoBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.CurrencyInfoBox.Controls.Add(this.label61);
		this.CurrencyInfoBox.Controls.Add(this.txtExchangeRate);
		this.CurrencyInfoBox.Controls.Add(this.label60);
		this.CurrencyInfoBox.Controls.Add(this.txtFcyAmount);
		this.CurrencyInfoBox.Controls.Add(this.label59);
		this.CurrencyInfoBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CurrencyInfoBox.Location = new System.Drawing.Point(1039, 26);
		this.CurrencyInfoBox.Name = "CurrencyInfoBox";
		this.CurrencyInfoBox.Size = new System.Drawing.Size(230, 128);
		this.CurrencyInfoBox.TabIndex = 2;
		this.CurrencyInfoBox.TabStop = false;
		this.CurrencyInfoBox.Text = "Currency Information";
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
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(77, 18);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(148, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(5, 23);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(55, 15);
		this.label61.TabIndex = 128;
		this.label61.Text = "Fcy Code";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(77, 45);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(148, 23);
		this.txtExchangeRate.TabIndex = 1;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(5, 49);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(58, 15);
		this.label60.TabIndex = 127;
		this.label60.Text = "Exch Rate";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(77, 72);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(148, 23);
		this.txtFcyAmount.TabIndex = 2;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(5, 76);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(72, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Fcy Amount";
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.txtCommissionRemarks);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.txtcommrate);
		this.groupBox3.Controls.Add(this.txtcommamount);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(722, 27);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(317, 127);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Commission";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(3, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Agent (Cr)";
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(67, 14);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(4, 73);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(3, 47);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(32, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Type";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(67, 93);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(247, 23);
		this.txtCommissionRemarks.TabIndex = 4;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(3, 97);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 95;
		this.label25.Text = "Remarks";
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(67, 41);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(155, 47);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Rate";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(223, 43);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(91, 23);
		this.txtcommrate.TabIndex = 2;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(67, 69);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(247, 23);
		this.txtcommamount.TabIndex = 3;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtaxtype);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label69);
		this.groupBox1.Controls.Add(this.label64);
		this.groupBox1.Controls.Add(this.txtTaxAmount);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add(this.txtTaxPercent);
		this.groupBox1.Controls.Add(this.txtDiscountRate);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.txtDiscountAmount);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtNetRate);
		this.groupBox1.Controls.Add(this.txtNetWeight);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtRateCut);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtItemWeight);
		this.groupBox1.Controls.Add(this.txtVarientEquivalent);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtItemAmountWithOutDiscount);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 155);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1264, 114);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(195, 65);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(53, 15);
		this.label49.TabIndex = 116;
		this.label49.Text = "Tax Type";
		this.cmbtaxtype.AllowNull = (DefaultableBoolean)1;
		this.cmbtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Location = new System.Drawing.Point(195, 83);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Name = "cmbtaxtype";
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Leave += new System.EventHandler(cmbtaxtype_Leave);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(309, 65);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(38, 15);
		this.label50.TabIndex = 114;
		this.label50.Text = "Tax %";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(1191, 16);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(56, 15);
		this.label69.TabIndex = 146;
		this.label69.Text = "Disc Rate";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(365, 65);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(72, 15);
		this.label64.TabIndex = 115;
		this.label64.Text = "Tax Amount";
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(365, 85);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmount.TabIndex = 17;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(3, 65);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(77, 15);
		this.label70.TabIndex = 147;
		this.label70.Text = "Disc Amount";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(932, 16);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(52, 15);
		this.label57.TabIndex = 153;
		this.label57.Text = "Net Rate";
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(309, 85);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(55, 23);
		this.txtTaxPercent.TabIndex = 16;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.txtDiscountRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountRate.Location = new System.Drawing.Point(1191, 37);
		this.txtDiscountRate.Name = "txtDiscountRate";
		this.txtDiscountRate.Size = new System.Drawing.Size(70, 23);
		this.txtDiscountRate.TabIndex = 12;
		this.txtDiscountRate.Tag = "DiscPercent";
		this.txtDiscountRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountRate.TextChanged += new System.EventHandler(txtDiscountRate_TextChanged);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1191, 84);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(40, 24);
		this.btnAdd.TabIndex = 24;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountAmount.Location = new System.Drawing.Point(3, 85);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(90, 23);
		this.txtDiscountAmount.TabIndex = 13;
		this.txtDiscountAmount.TabStop = false;
		this.txtDiscountAmount.Tag = "DiscAmount";
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmount.TextChanged += new System.EventHandler(txtDiscountAmount_TextChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(695, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(68, 15);
		this.label43.TabIndex = 149;
		this.label43.Text = "Net Weight";
		this.txtNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetRate.Enabled = false;
		this.txtNetRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetRate.Location = new System.Drawing.Point(933, 37);
		this.txtNetRate.Name = "txtNetRate";
		this.txtNetRate.Size = new System.Drawing.Size(75, 23);
		this.txtNetRate.TabIndex = 9;
		this.txtNetRate.Tag = "AddLess";
		this.txtNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetRate.TextChanged += new System.EventHandler(txtNetRate_TextChanged);
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(695, 37);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(85, 23);
		this.txtNetWeight.TabIndex = 6;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(456, 65);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(80, 15);
		this.label71.TabIndex = 145;
		this.label71.Text = "Total Amount";
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(1100, 35);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Leave += new System.EventHandler(CmbDiscountType_Leave);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(857, 16);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(51, 15);
		this.label56.TabIndex = 151;
		this.label56.Text = "Rate Cut";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1100, 16);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(83, 15);
		this.label15.TabIndex = 141;
		this.label15.Text = "Discount Type";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(456, 85);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(100, 23);
		this.txtTotalAmount.TabIndex = 18;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(365, 35);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(619, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Item Weight";
		this.txtRateCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateCut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateCut.Location = new System.Drawing.Point(857, 37);
		this.txtRateCut.Name = "txtRateCut";
		this.txtRateCut.Size = new System.Drawing.Size(75, 23);
		this.txtRateCut.TabIndex = 8;
		this.txtRateCut.Tag = "RateCut";
		this.txtRateCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateCut.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtRateCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(467, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 132;
		this.label2.Text = "Varient Unit";
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(558, 83);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(558, 65);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(538, 37);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(80, 23);
		this.txtQty.TabIndex = 4;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(365, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(94, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Attribute Varient";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(538, 16);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(619, 37);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(75, 23);
		this.txtItemWeight.TabIndex = 7;
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(467, 37);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(70, 23);
		this.txtVarientEquivalent.TabIndex = 3;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(254, 14);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(201, 14);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(978, 83);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 23;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(978, 65);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(309, 16);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1191, 61);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(70, 23);
		this.btnCancelUpdateDetial.TabIndex = 26;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1191, 84);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 24);
		this.btnUpdateDetail.TabIndex = 25;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(823, 85);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(51, 23);
		this.txtgatepassno.TabIndex = 21;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(876, 65);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(876, 85);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(100, 23);
		this.txtvehicleno.TabIndex = 22;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(823, 65);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(164, 35);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(720, 85);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(101, 23);
		this.txtgpdate.TabIndex = 20;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(781, 16);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Item Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(781, 37);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(75, 23);
		this.txtRate.TabIndex = 7;
		this.txtRate.Tag = "ItemRate";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(722, 65);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(166, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(5, 16);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.txtItemAmountWithOutDiscount.BackColor = System.Drawing.Color.White;
		this.txtItemAmountWithOutDiscount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountWithOutDiscount.Enabled = false;
		this.txtItemAmountWithOutDiscount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmountWithOutDiscount.Location = new System.Drawing.Point(1009, 37);
		this.txtItemAmountWithOutDiscount.Name = "txtItemAmountWithOutDiscount";
		this.txtItemAmountWithOutDiscount.ReadOnly = true;
		this.txtItemAmountWithOutDiscount.Size = new System.Drawing.Size(90, 23);
		this.txtItemAmountWithOutDiscount.TabIndex = 10;
		this.txtItemAmountWithOutDiscount.TabStop = false;
		this.txtItemAmountWithOutDiscount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1003, 8);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(98, 30);
		this.label16.TabIndex = 21;
		this.label16.Text = "Amount Without\r\nDiscount";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(94, 85);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 14;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(94, 65);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.txtInvoiceWeight);
		this.groupBox2.Controls.Add(this.txtInvoiceQty);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(7, 27);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(714, 127);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(3, 95);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 140;
		this.label22.Text = "Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(2, 71);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(73, 15);
		this.label12.TabIndex = 135;
		this.label12.Text = "Tax Account";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(88, 66);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(205, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(88, 93);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(624, 23);
		this.txtremarks.TabIndex = 11;
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(603, 67);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.ShortcutsEnabled = false;
		this.txtInvoiceWeight.Size = new System.Drawing.Size(109, 23);
		this.txtInvoiceWeight.TabIndex = 10;
		this.txtInvoiceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInvoiceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceQty.Enabled = false;
		this.txtInvoiceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceQty.Location = new System.Drawing.Point(603, 40);
		this.txtInvoiceQty.Name = "txtInvoiceQty";
		this.txtInvoiceQty.ShortcutsEnabled = false;
		this.txtInvoiceQty.Size = new System.Drawing.Size(109, 23);
		this.txtInvoiceQty.TabIndex = 9;
		this.txtInvoiceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(530, 71);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(65, 15);
		this.label62.TabIndex = 129;
		this.label62.Text = "Invoice Wt";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(88, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(83, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(173, 13);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(120, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(2, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(530, 44);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(68, 15);
		this.label63.TabIndex = 130;
		this.label63.Text = "Invoice Qty";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(376, 13);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(151, 23);
		this.txtbillno.TabIndex = 4;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(376, 38);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(603, 11);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(293, 17);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(2, 44);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(73, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Supplier (Cr)";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(293, 44);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Term";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(376, 67);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(40, 23);
		this.txtduedays.TabIndex = 6;
		this.txtduedays.Text = "0";
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance156).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(88, 38);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(293, 63);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 30);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days\r\n&& Date";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(417, 67);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(110, 22);
		this.duedate.TabIndex = 7;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(530, 17);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(74, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Deliver Term";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkBok);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1270, 26);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 2);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(147, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Purchase Invoice";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(1070, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(76, 19);
		this.ChkPrintslip.TabIndex = 7;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1166, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(102, 19);
		this.ChkBok.TabIndex = 8;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(871, 4);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(158, 19);
		this.ChkResetOnSave.TabIndex = 6;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1392, 41);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1392, 73);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1302, 79);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(1333, 106);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Visible = false;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1302, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1372, 135);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "UOM";
		this.label19.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(450, 224);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(469, 224);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.btnAttachment, this.toolStripSeparator4, this.btnSlip, this.btnPrintA, this.btnPrint,
			this.btnLoadGrn, this.btnLoadGdn, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1270, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(96, 22);
		this.btnSlip.Text = "1859-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnPrintA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintA.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintA.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintA.Name = "btnPrintA";
		this.btnPrintA.Size = new System.Drawing.Size(105, 22);
		this.btnPrintA.Text = "1859-PrintA";
		this.btnPrintA.Click += new System.EventHandler(btn294APrint_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnLoadGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGrn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGrn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGrn.Name = "btnLoadGrn";
		this.btnLoadGrn.Size = new System.Drawing.Size(85, 22);
		this.btnLoadGrn.Text = "Load Grn";
		this.btnLoadGrn.Click += new System.EventHandler(btnLoadGrn_Click);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(99, 22);
		this.btnLoadGdn.Text = "Load Order";
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1276, 732);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 482);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1270, 247);
		this.panel17.TabIndex = 49;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1270, 220);
		this.panel19.TabIndex = 49;
		this.grdDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistory.DesignTimeLayout = grdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailHistory.DynamicFiltering = true;
		this.grdDetailHistory.FilterMode = (FilterMode)2;
		this.grdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Name = "grdDetailHistory";
		((ScrollableBase)this.grdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1270, 220);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 0;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1270, 27);
		this.panel18.TabIndex = 48;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(5, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(353, 19);
		this.label55.TabIndex = 35;
		this.label55.Text = "Detail Information of Above Selected Row";
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Controls.Add(this.panel5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1270, 425);
		this.panel10.TabIndex = 48;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 108);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1270, 317);
		this.panel8.TabIndex = 49;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1270, 317);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 81);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1270, 27);
		this.panel7.TabIndex = 48;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(72, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "History ";
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1270, 81);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory);
		this.groupBox4.Controls.Add(this.label72);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(9, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(873, 76);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(802, 46);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbCommissionAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance230).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance231).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance232).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Location = new System.Drawing.Point(596, 45);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Name = "CmbCommissionAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).TabIndex = 6;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(486, 51);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(108, 15);
		this.label72.TabIndex = 180;
		this.label72.Text = "Commission Agent";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(486, 22);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(86, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Supplier Name";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance197;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance202).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(596, 16);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 22);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance240;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 47);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(5, 51);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(91, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(383, 47);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 47);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(322, 51);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(322, 22);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(383, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1270, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(210, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Purchase Invoice History";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1270, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1270, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.label52.BackColor = System.Drawing.Color.Gainsboro;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(959, 4);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(120, 18);
		this.label52.TabIndex = 147;
		this.label52.Text = "Discount";
		this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(1082, 27);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(21, 18);
		this.label80.TabIndex = 146;
		this.label80.Text = "=";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(942, 27);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(15, 18);
		this.label85.TabIndex = 141;
		this.label85.Text = "-";
		this.label86.BackColor = System.Drawing.Color.Gainsboro;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(575, 25);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(238, 23);
		this.label86.TabIndex = 144;
		this.label86.Text = "Item Amount Without Discount";
		this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtDiscountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountHeader.Enabled = false;
		this.txtDiscountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountHeader.Location = new System.Drawing.Point(959, 25);
		this.txtDiscountHeader.Name = "txtDiscountHeader";
		this.txtDiscountHeader.Size = new System.Drawing.Size(120, 23);
		this.txtDiscountHeader.TabIndex = 142;
		this.txtDiscountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountHeader.Enabled = false;
		this.txtItemAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmountHeader.Location = new System.Drawing.Point(1104, 25);
		this.txtItemAmountHeader.Name = "txtItemAmountHeader";
		this.txtItemAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtItemAmountHeader.TabIndex = 1;
		this.txtItemAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(789, 201);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(24, 25);
		this.PanelMessage.TabIndex = 150;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(24, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.BackColor = System.Drawing.Color.LightGray;
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Add Total Bill Amount and System Will Generate the Discount Automatically";
		this.label68.BackColor = System.Drawing.Color.Gainsboro;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(575, 201);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(212, 25);
		this.label68.TabIndex = 148;
		this.label68.Text = "Add Total Bill Amount Manually";
		this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtManualTotalBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualTotalBillAmount.Font = new System.Drawing.Font("Verdana", 10.5f);
		this.txtManualTotalBillAmount.Location = new System.Drawing.Point(815, 201);
		this.txtManualTotalBillAmount.Name = "txtManualTotalBillAmount";
		this.txtManualTotalBillAmount.Size = new System.Drawing.Size(142, 25);
		this.txtManualTotalBillAmount.TabIndex = 11;
		this.txtManualTotalBillAmount.TabStop = false;
		this.txtManualTotalBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtManualTotalBillAmount.TextChanged += new System.EventHandler(txtManualTotalBillAmount_TextChanged);
		this.txtManualTotalBillAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtManualTotalBillAmount_KeyPress);
		this.txtManualTotalBillAmount.Leave += new System.EventHandler(txtManualTotalBillAmount_Leave);
		this.txtItemAmountWithoutDiscHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountWithoutDiscHeader.Enabled = false;
		this.txtItemAmountWithoutDiscHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmountWithoutDiscHeader.Location = new System.Drawing.Point(814, 25);
		this.txtItemAmountWithoutDiscHeader.Name = "txtItemAmountWithoutDiscHeader";
		this.txtItemAmountWithoutDiscHeader.Size = new System.Drawing.Size(122, 23);
		this.txtItemAmountWithoutDiscHeader.TabIndex = 0;
		this.txtItemAmountWithoutDiscHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label27.BackColor = System.Drawing.Color.Gainsboro;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(1104, 4);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(161, 18);
		this.label27.TabIndex = 145;
		this.label27.Text = "Item Amount";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtTaxAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmountHeader.Enabled = false;
		this.txtTaxAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmountHeader.Location = new System.Drawing.Point(1104, 49);
		this.txtTaxAmountHeader.Name = "txtTaxAmountHeader";
		this.txtTaxAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtTaxAmountHeader.TabIndex = 2;
		this.txtTaxAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label28.BackColor = System.Drawing.Color.Gainsboro;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(959, 50);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(120, 21);
		this.label28.TabIndex = 145;
		this.label28.Text = "Tax Amount";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOtherItemGridAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherItemGridAmountHeader.Enabled = false;
		this.txtOtherItemGridAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherItemGridAmountHeader.Location = new System.Drawing.Point(1104, 73);
		this.txtOtherItemGridAmountHeader.Name = "txtOtherItemGridAmountHeader";
		this.txtOtherItemGridAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtOtherItemGridAmountHeader.TabIndex = 3;
		this.txtOtherItemGridAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label30.BackColor = System.Drawing.Color.Gainsboro;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(959, 74);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(120, 21);
		this.label30.TabIndex = 145;
		this.label30.Text = "Other Item Amount";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPartyAddLessGridAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyAddLessGridAmountHeader.Enabled = false;
		this.txtPartyAddLessGridAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyAddLessGridAmountHeader.Location = new System.Drawing.Point(1104, 97);
		this.txtPartyAddLessGridAmountHeader.Name = "txtPartyAddLessGridAmountHeader";
		this.txtPartyAddLessGridAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtPartyAddLessGridAmountHeader.TabIndex = 4;
		this.txtPartyAddLessGridAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label34.BackColor = System.Drawing.Color.Gainsboro;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(959, 98);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(120, 21);
		this.label34.TabIndex = 145;
		this.label34.Text = "Party Add Less Amount";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(1082, 51);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(21, 18);
		this.label36.TabIndex = 146;
		this.label36.Text = "+";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1082, 75);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(21, 18);
		this.label39.TabIndex = 146;
		this.label39.Text = "+";
		this.labelFreightPlusIcon.AutoSize = true;
		this.labelFreightPlusIcon.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFreightPlusIcon.Location = new System.Drawing.Point(1082, 152);
		this.labelFreightPlusIcon.Name = "labelFreightPlusIcon";
		this.labelFreightPlusIcon.Size = new System.Drawing.Size(21, 18);
		this.labelFreightPlusIcon.TabIndex = 146;
		this.labelFreightPlusIcon.Text = "+";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1082, 99);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(21, 18);
		this.label45.TabIndex = 146;
		this.label45.Text = "+";
		this.txtCommissionAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAmountHeader.Enabled = false;
		this.txtCommissionAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionAmountHeader.Location = new System.Drawing.Point(1104, 123);
		this.txtCommissionAmountHeader.Name = "txtCommissionAmountHeader";
		this.txtCommissionAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtCommissionAmountHeader.TabIndex = 6;
		this.txtCommissionAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.labelCommAmtHeader.BackColor = System.Drawing.Color.Gainsboro;
		this.labelCommAmtHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelCommAmtHeader.Location = new System.Drawing.Point(959, 124);
		this.labelCommAmtHeader.Name = "labelCommAmtHeader";
		this.labelCommAmtHeader.Size = new System.Drawing.Size(120, 21);
		this.labelCommAmtHeader.TabIndex = 145;
		this.labelCommAmtHeader.Text = "Commission Amount";
		this.labelCommAmtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.labelCommAmtPlusIcon.AutoSize = true;
		this.labelCommAmtPlusIcon.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCommAmtPlusIcon.Location = new System.Drawing.Point(1082, 125);
		this.labelCommAmtPlusIcon.Name = "labelCommAmtPlusIcon";
		this.labelCommAmtPlusIcon.Size = new System.Drawing.Size(21, 18);
		this.labelCommAmtPlusIcon.TabIndex = 146;
		this.labelCommAmtPlusIcon.Text = "+";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1934, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1229, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1635, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(51, 30);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 761);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceDirectConcrete";
		this.Text = "Purchase Invocie";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdChargeToProduct).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPartyAddLs).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.CurrencyInfoBox.ResumeLayout(false);
		this.CurrencyInfoBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
