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

namespace Architecture.WinApp.pcc.Sale;

public class SaleInvoiceDirectConcrete : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable dtGrid = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtDiscType = new DataTable();

	private DataTable dtjoblot = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtItemForWages = new DataTable();

	private DataTable dtVarientForWages = new DataTable();

	private DataTable dtwareh = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable dtVehicleNos = new DataTable();

	private bool WagesStatus;

	private decimal SupplierGLCreditAmount;

	private decimal SupplierGLDebitAmount;

	private decimal SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int Id;

	private int ItemId;

	private bool Approved;

	private int BaseCurrency;

	private bool SubsidiaryAccountAllownOnVouchers;

	private decimal BaseRate;

	private bool SaleOrderExist;

	private bool GdnExist;

	private bool HasMultiCurrencyFeature;

	private bool GetItemagainstwarehouseId;

	private int OutwardFreightAccountId;

	private List<InvSaleInvoiceDetail> lstRemoveDetailRecord = new List<InvSaleInvoiceDetail>();

	private UserAccount UserAccount = new UserAccount();

	private LoadSaleOrderConcrete LoadTransactions;

	private LoadPendingGDNDirectConcrete LoadGdn;

	private bool ItemSearchWithCode;

	private bool PartySearchWithCode;

	private DataTable dtCustomerHistory = new DataTable();

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

	private ToolStripButton btnCustomerSlip;

	private CheckBox ChkBok;

	private TextBox txtBillAmountHeader;

	private TextBox txtremarks;

	private Label label24;

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

	private TextBox txtSupplierReference;

	private TextBox txtCommissionRemarks;

	private Label label6;

	private Label label25;

	private Label label12;

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

	private CheckBox ChkCustomerPrintslip;

	private CheckBox ChkResetOnSave;

	internal UltraCombo CmbTransporterAc;

	private TextBox txtFreightAmountHeader;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	private ToolStripButton btn1856APrint;

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

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel12;

	private Label label37;

	private TabPage tabPage5;

	private Panel panel15;

	private Label label53;

	internal GridEX grdContractorWages;

	private Panel panel6;

	private Panel panel4;

	private TextBox txtDiscountHeader;

	private TextBox txtItemAmountHeader;

	private TextBox txtItemNetAmountHeader;

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

	private Button btnDefinePlant;

	internal UltraCombo CmbVisitedbyName;

	private Label label22;

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

	internal UltraCombo CmbRefSalesMan;

	private Label label16;

	private TextBox txtDeliverydays;

	private Label label28;

	private DateTimePicker txtDeliveryStartDate;

	private Label label30;

	private Label label34;

	private Label label36;

	private TextBox txtRefPartyCellNo;

	private TextBox txtRefPartyAddress;

	private TextBox txtRefPartyName;

	private Label label41;

	private Label label43;

	private TextBox txtNetWeight;

	internal UltraCombo CmbOtherWagesAccount;

	private Label label52;

	private Label label79;

	private Label label80;

	private Label label81;

	private Label label83;

	private Label label85;

	private Label label86;

	private Label label87;

	private Label label88;

	private Label label89;

	private TextBox txtOtherWagesHeader;

	private Label label3;

	private ToolStripButton BtnRefreshHistory;

	private Label label46;

	private Label label84;

	private TextBox CommNetAmtHeader;

	internal UltraCombo cmbSettlementDiscountAccount;

	private Label label44;

	private Label label39;

	private Label label27;

	private TextBox txtBillamountWithoutCommission;

	private TextBox txtSettlementDiscountHeader;

	private Label label45;

	private Label label51;

	internal UltraCombo CmbCommissionDebitAccount;

	private CheckBox chkisFreeOfCost;

	private Label label57;

	private TextBox txtAddLess;

	private Label label56;

	private TextBox txtItemPrice;

	private TabPage tabPage6;

	internal UltraCombo CmbVehicleNo;

	private Panel panel13;

	private Button BtnUpdateConctratorInWages;

	private Label label49;

	private Button BtnGenerateWagesRows;

	private Label label50;

	internal UltraCombo CmbPackUom;

	private Label label58;

	private TextBox txtDistance;

	private ToolStripButton BtnLoadOrder;

	private Label label64;

	private Button btnUpdateDiscountInGrid;

	internal UltraCombo CmbContractorForUpdateInWagesGrid;

	internal UltraCombo CmbActivityforContractorUpdateInWagesGrid;

	private Label label66;

	private Label label65;

	private RadioButton RdPartyByCode;

	private RadioButton RdPartyByName;

	private RadioButton RdCustomerHistorySearchByCode;

	private RadioButton RdCustomerHistorySearchByName;

	private Label label67;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private Label label68;

	private TextBox txtManualTotalBillAmount;

	public SaleInvoiceDirectConcrete(UserAccount user)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 830);
			ItemSearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithCode)
			{
				rdbtnItemCode.Checked = true;
			}
			else
			{
				rdbtnItemName.Checked = true;
			}
			PartySearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SupplierCustomerDefaultFilterByPartyCode"));
			if (PartySearchWithCode)
			{
				RdPartyByCode.Checked = true;
			}
			else
			{
				RdPartyByName.Checked = true;
			}
			if (PartySearchWithCode)
			{
				RdCustomerHistorySearchByCode.Checked = true;
			}
			else
			{
				RdCustomerHistorySearchByName.Checked = true;
			}
			string ItemStatus = CommonServices.GetConfigurationByOrgCompandConfigDescription("GetItemsagainstWarehouse");
			if (ItemStatus != string.Empty)
			{
				GetItemagainstwarehouseId = Conversion.ToBool((object)ItemStatus);
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightOutwardAc").ToList();
			if (list.Count > 0)
			{
				int value = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (value > 0)
				{
					((Control)(object)CmbTransporterAc).Visible = true;
					txtFreightAmountHeader.Visible = true;
					OutwardFreightAccountId = value;
				}
			}
			dtViewAttachments.Columns.Add("AttchmentPath");
			string ScreenName = base.Name;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btn1856APrint.Enabled = formright.DoHavePrintRights;
			btnCustomerSlip.Enabled = formright.DoHavePrintRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ChkCustomerPrintslip.Enabled = formright.DoHavePrintRights;
			ChkCustomerPrintslip.Checked = formright.DoHavePrintRights;
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
			CurrencyFill();
			MultiCurrencyFeature();
			dtDiscType.Columns.Add("Id", typeof(int));
			dtDiscType.Columns.Add("DiscountType");
			dtVehicleNos.Columns.Add("Id", typeof(int));
			dtVehicleNos.Columns.Add("VehicleNo");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("ItemWeight", typeof(double));
			dtitem.Columns.Add("ItemRate", typeof(double));
			dtitem.Columns.Add("ScheduleId", typeof(int));
			dtitem.Columns.Add("WithVariant");
			dtItemForWages.Columns.Add("Id", typeof(int));
			dtItemForWages.Columns.Add("ItemName", typeof(string));
			dtItemForWages.Columns.Add("ItemCode", typeof(string));
			dtVarientForWages.Columns.Add("ItemAttributeId", typeof(int));
			dtVarientForWages.Columns.Add("ItemAttribute", typeof(string));
			dtVarientForWages.Columns.Add("ItemId", typeof(int));
			dtVarientForWages.Columns.Add("ItemName", typeof(string));
			dtVarientForWages.Columns.Add("VarientEquivalent", typeof(decimal));
			dtVarientForWages.Columns.Add("Qty", typeof(decimal));
			dtVarientForWages.Columns.Add("NetWeight", typeof(decimal));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("GdnId", typeof(int));
			dtGrid.Columns.Add("GdnDetailId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("OrderId", typeof(int));
			dtGrid.Columns.Add("OrderDetailId", typeof(int));
			dtGrid.Columns.Add("OrderNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse", typeof(string));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName", typeof(string));
			dtGrid.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("ScheduleId", typeof(string));
			dtGrid.Columns.Add("PackUomId", typeof(int));
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("PackUomEquivalent", typeof(double));
			dtGrid.Columns.Add("AttributeVarientId", typeof(int));
			dtGrid.Columns.Add("AttributeVarient", typeof(string));
			dtGrid.Columns.Add("VarientUnit", typeof(decimal));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(decimal));
			dtGrid.Columns.Add("BalQty", typeof(decimal));
			dtGrid.Columns.Add("ItemWeight", typeof(decimal));
			dtGrid.Columns.Add("NetWeight", typeof(decimal));
			dtGrid.Columns.Add("BalWeight", typeof(decimal));
			dtGrid.Columns.Add("ItemPrice", typeof(decimal));
			dtGrid.Columns.Add("AddLessRate", typeof(decimal));
			dtGrid.Columns.Add("Rate", typeof(decimal));
			dtGrid.Columns.Add("ItemAmount", typeof(decimal));
			dtGrid.Columns.Add("DiscountTypeId", typeof(int));
			dtGrid.Columns.Add("DiscountType", typeof(string));
			dtGrid.Columns.Add("DiscRate", typeof(decimal));
			dtGrid.Columns.Add("DiscAmount", typeof(decimal));
			dtGrid.Columns.Add("ItemAmountWithDisc", typeof(decimal));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("Expense", typeof(decimal));
			dtGrid.Columns.Add("SettleDisc", typeof(decimal));
			dtGrid.Columns.Add("Commission", typeof(decimal));
			dtGrid.Columns.Add("Freight", typeof(decimal));
			dtGrid.Columns.Add("Wages", typeof(decimal));
			dtGrid.Columns.Add("ItemNetAmount", typeof(decimal));
			dtGrid.Columns.Add("IsFOC", typeof(bool));
			dtGrid.Columns.Add("ItemWithVarient", typeof(bool));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("ItemName", typeof(string));
			dtInvExp.Columns.Add("Qty", typeof(decimal));
			dtInvExp.Columns.Add("Rate", typeof(decimal));
			dtInvExp.Columns.Add("Amount", typeof(decimal));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdChargeToProduct.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("GlAccountId", typeof(string));
			dtGrdGL.Columns.Add("AccountTitle", typeof(string));
			dtGrdGL.Columns.Add("Remarks", typeof(string));
			dtGrdGL.Columns.Add("Percentage", typeof(decimal));
			dtGrdGL.Columns.Add("Qty", typeof(decimal));
			dtGrdGL.Columns.Add("Rate", typeof(decimal));
			dtGrdGL.Columns.Add("Debit", typeof(decimal));
			dtGrdGL.Columns.Add("Credit", typeof(decimal));
			AddRowInGLGrid();
			grdPartyAddLs.RetrieveStructure();
			gridGLSettings();
			dtWagesDetail.Columns.Add("Id", typeof(int));
			dtWagesDetail.Columns.Add("GdnId", typeof(int));
			dtWagesDetail.Columns.Add("GdnWagesId", typeof(int));
			dtWagesDetail.Columns.Add("OrderId", typeof(int));
			dtWagesDetail.Columns.Add("OrderWagesId", typeof(int));
			dtWagesDetail.Columns.Add("ItemId", typeof(int));
			dtWagesDetail.Columns.Add("ItemName");
			dtWagesDetail.Columns.Add("AttributeVarientId", typeof(int));
			dtWagesDetail.Columns.Add("AttributeVarient");
			dtWagesDetail.Columns.Add("VarientEquivalent", typeof(decimal));
			dtWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtWagesDetail.Columns.Add("ContractorName");
			dtWagesDetail.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtWagesDetail.Columns.Add("ServiceActivity");
			dtWagesDetail.Columns.Add("ParentUomId", typeof(int));
			dtWagesDetail.Columns.Add("Qty", typeof(decimal));
			dtWagesDetail.Columns.Add("ItemNetWeight", typeof(decimal));
			dtWagesDetail.Columns.Add("Rate", typeof(decimal));
			dtWagesDetail.Columns.Add("Amount", typeof(decimal));
			dtWagesDetail.Columns.Add("AddLess", typeof(decimal));
			dtWagesDetail.Columns.Add("NetAmount", typeof(decimal));
			dtWagesDetail.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			DocumentNo();
			Project();
			braches();
			suppliercustomer();
			VisitedByFill();
			PaymentTerms();
			DeliveryTerm();
			RefSalesManFill();
			CommissionTypeFill();
			CommissionUOMFill();
			TransporterAcFill();
			OtherWagesAcFill();
			SettlementDiscAccountAndCommissionDebitAccountFill();
			bindWareHouse();
			item();
			JobLotBind();
			DiscountTypefill();
			BinCity();
			BindVehicles();
			dtForGridComboFill();
			ConfigurationDefault();
			DocDate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			dtCustomerHistory.Columns.Add("Id", typeof(int));
			dtCustomerHistory.Columns.Add("PartyName");
			dtCustomerHistory.Columns.Add("PartyCode");
			HistoryComboBind();
			ParameterFill();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
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
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1856,
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
				dtSupplier = CommonServices.GetVendorsAndCustomers(2);
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			}
			if (dtSupplier.Rows.Count > 0)
			{
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
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

	private void VisitedByFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(CmbVisitedbyName.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 2
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbVisitedbyName, "Id", "ReferencePartyName", "Visited By", ZeroIndex: true);
				((UltraGridBase)CmbVisitedbyName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbVisitedbyName.Value = ItId;
					}
					else
					{
						((Control)(object)CmbVisitedbyName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbVisitedbyName).Text = string.Empty;
				((UltraGridBase)CmbVisitedbyName).DataSource = null;
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
			dt.Rows.Add(1, "Factory Loading");
			dt.Rows.Add(2, "Delivery");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefSalesManFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(CmbRefSalesMan.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 3
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRefSalesMan, "Id", "ReferencePartyName", "RefSalesMan", ZeroIndex: true);
				((UltraGridBase)CmbRefSalesMan).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbRefSalesMan.Value = ItId;
					}
					else
					{
						((Control)(object)CmbRefSalesMan).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRefSalesMan).Text = string.Empty;
				((UltraGridBase)CmbRefSalesMan).DataSource = null;
			}
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
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,3");
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

	private void OtherWagesAcFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbOtherWagesAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,3");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbOtherWagesAccount, "Id", "AccountTitle", "OtherWages Account Title", true);
			if (ItId1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					CmbOtherWagesAccount.Value = ItId1;
				}
				else
				{
					((Control)(object)CmbOtherWagesAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SettlementDiscAccountAndCommissionDebitAccountFill()
	{
		try
		{
			int ItId = Conversion.ToInt(cmbSettlementDiscountAccount.Value);
			int ItId2 = Conversion.ToInt(CmbCommissionDebitAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,11,12,20,21");
			if (dt.Rows.Count > 0)
			{
				DataTable Data = (from row in dt.AsEnumerable()
					where row.Field<int>("Id") != Conversion.ToInt((object)txtSupplierGLId.Text)
					select row).DefaultIfEmpty().CopyToDataTable();
				DDL.BindDDLNew(Data, cmbSettlementDiscountAccount, "Id", "AccountTitle", "Settlement Account", true);
				DDL.BindDDLNew(dt, CmbCommissionDebitAccount, "Id", "AccountTitle", "CommissionDebit Account Title", true);
				if (ItId2 > 0)
				{
					if (Data.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						cmbSettlementDiscountAccount.Value = ItId;
					}
					else
					{
						((Control)(object)cmbSettlementDiscountAccount).Text = string.Empty;
					}
				}
				if (ItId2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId2))
					{
						CmbCommissionDebitAccount.Value = ItId2;
					}
					else
					{
						((Control)(object)CmbCommissionDebitAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCommissionDebitAccount).Text = string.Empty;
				((Control)(object)cmbSettlementDiscountAccount).Text = string.Empty;
				((UltraGridBase)CmbCommissionDebitAccount).DataSource = null;
				((UltraGridBase)cmbSettlementDiscountAccount).DataSource = null;
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			((Control)(object)comItem).Text = string.Empty;
			dtitem.Rows.Clear();
			DataTable dt = ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = DocDate.Value
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["WeightKgs"], dt.Rows[i]["PreviousRate"], dt.Rows[i]["ScheduleId"], dt.Rows[i]["ItemWithVarient"]);
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

	private void BindVehicles()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbVehicleNo.Value);
			dtVehicleNos.Clear();
			DataTable dtGrid = DefineVehicleWeight.ReadAll(new DefineVehicleWeight
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtVehicleNos.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["Id"]), dtGrid.Rows[i]["VehicleNo"]);
				}
				if (dtVehicleNos.Rows.Count > 0)
				{
					DDL.BindDDL(dtVehicleNos, CmbVehicleNo, "Id", "VehicleNo", "Vehicle No ", ZeroIndex: true);
					if (Id > 0)
					{
						if (dtVehicleNos.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbVehicleNo.Value = Id;
						}
						else
						{
							((Control)(object)CmbVehicleNo).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbVehicleNo).Text = string.Empty;
					((UltraGridBase)CmbVehicleNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbVehicleNo).Text = string.Empty;
				((UltraGridBase)CmbVehicleNo).DataSource = null;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("VehicleNo Not Found");
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 1856);
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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			dtContr.Clear();
			dtAccountlst.Clear();
			dtOtherItem.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
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

	private void GridItemPriceUpdateFromSchedule()
	{
		try
		{
			item();
			if (dtitem.Rows.Count <= 0 || grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				DataRow foundRow = dtitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value));
				decimal price = default(decimal);
				if (foundRow == null)
				{
					continue;
				}
				decimal Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				decimal unit = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
				Conversion.ToDecimal(r.Cells["PackUomEquivalent"].Value);
				decimal AddLessRate = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
				bool ItemWithVarient = Conversion.ToBool(r.Cells["ItemWithVarient"].Value);
				decimal ItemAmt = default(decimal);
				decimal DisAmt = default(decimal);
				decimal ItemAmountWithDisc = default(decimal);
				decimal DisRate = default(decimal);
				r.BeginEdit();
				price = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemRate"]) : 0m);
				r.Cells["ItemPrice"].Value = price.ToString(clsGlobalVariables.DecimalRateFormate);
				decimal NetRate = price + AddLessRate;
				r.Cells["Rate"].Value = NetRate.ToString(clsGlobalVariables.DecimalRateFormate);
				if (Qty > 0m)
				{
					if (ItemWithVarient)
					{
						ItemAmt = Qty * unit * NetRate;
						r.Cells["ItemAmount"].Value = ItemAmt;
					}
					else
					{
						ItemAmt = Qty * NetRate;
						r.Cells["ItemAmount"].Value = ItemAmt;
					}
				}
				else
				{
					ItemAmt = default(decimal);
					r.Cells["ItemAmount"].Value = ItemAmt;
				}
				if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) != 0)
				{
					DisRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
					if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 1)
					{
						DisAmt = DisRate;
						r.Cells["DiscAmount"].Value = DisAmt;
					}
					else if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 2)
					{
						if (DisRate >= 100m)
						{
							MessageBox.Show("DiscountRate Cannot be greater than 99 when Discount Type is Percent");
							DisRate = 99m;
							r.Cells["DiscRate"].Value = DisRate;
						}
						DisAmt = ItemAmt * DisRate / 100m;
						r.Cells["DiscAmount"].Value = DisAmt;
					}
				}
				else
				{
					DisAmt = default(decimal);
					r.Cells["DiscAmount"].Value = 0;
				}
				if (ItemAmt > 0m && DisAmt > 0m)
				{
					ItemAmountWithDisc = ItemAmt - DisAmt;
					r.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
				}
				else if (ItemAmt > 0m)
				{
					ItemAmountWithDisc = ItemAmt;
					r.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
				}
				else
				{
					r.Cells["ItemAmountWithDisc"].Value = default(decimal);
				}
				r.EndEdit();
			}
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindPackUom(Conversion.ToInt(comItem.Value));
			if (Conversion.ToInt(comItem.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemWeight"].Value);
				double price = 0.0;
				price = Conversion.ToDouble(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemRate"].Value);
				txtItemPrice.Text = price.ToString("#,##0.####");
				txtRate.Text = price.ToString("#,##0.####");
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

	public void bindPackUom(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(ItemId);
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
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
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label59.Visible = true;
				label60.Visible = true;
				label61.Visible = true;
				txtRefPartyAddress.Size = new Size(198, 23);
				txtremarks.Size = new Size(198, 51);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label59.Visible = false;
				label60.Visible = false;
				label61.Visible = false;
				txtRefPartyAddress.Size = new Size(384, 23);
				txtremarks.Size = new Size(384, 51);
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
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1856",
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
					decimal amount = Conversion.ToDecimal(obj.Cells["ItemAmountWithDisc"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
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

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && SaleOrderExist)
			{
				throw new Exception("You can not add manual Record beacause record against order exist in Grid");
			}
			if (grd.RowCount > 0 && GdnExist)
			{
				throw new Exception("You can not add manual Record beacause record against Gdn exist in Grid");
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int WareHouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(obj.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(obj.Cells["JobLotId"].Value);
				bool IsFOC = Conversion.ToBool(obj.Cells["IsFOC"].Value);
				if (Conversion.ToInt(comWarehouse.Value) == WareHouseId && Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId && chkisFreeOfCost.Checked == IsFOC)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists");
					return;
				}
			}
			if (FormValidationDetail())
			{
				Conversion.ToInt((object)grd.RowCount);
				dtGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ScheduleId"].Value), Conversion.ToString(CmbPackUom.Value), Conversion.ToString(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["UOMCode"].Value), Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value), (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? ((object)0) : CmbAttributeVarient.Value, (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? "" : ((Control)(object)CmbAttributeVarient).Text.Trim(), (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text.Trim()), Conversion.ToInt(comjobLot.Value), Conversion.ToString((object)((Control)(object)comjobLot).Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtItemWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToDecimal((object)txtItemPrice.Text), Conversion.ToDecimal((object)txtAddLess.Text), Conversion.ToDecimal((object)txtRate.Text), Conversion.ToDecimal((object)txtAmount.Text), Conversion.ToInt(CmbDiscountType.Value), Conversion.ToString((object)((Control)(object)CmbDiscountType).Text), Conversion.ToDecimal((object)txtDiscountRate.Text), Conversion.ToDecimal((object)txtDiscountAmount.Text), Conversion.ToDecimal((object)txtTotalAmount.Text), Conversion.ToDecimal((object)txtTotalAmount.Text), Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString(), Conversion.ToInt((object)txtgatepassno.Text), Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text), Conversion.ToInt(txtCity.Value), Conversion.ToString((object)((Control)(object)txtCity).Text), 0, 0, 0, 0, 0, 0, Conversion.ToBool((object)chkisFreeOfCost.Checked));
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				TotalCommissionAmount();
				CommissionProportion();
				WagesProportion();
				OtherItemsProportion();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
				WagesItemFillFromDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && !SaleOrderExist && !GdnExist)
			{
				updateDetailIndex = r.RowIndex;
				comWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				comItem.Value = r.Cells["ItemId"].Value.ToString();
				bindvarientunit(Conversion.ToInt(comItem.Value));
				bindPackUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (Conversion.ToInt(r.Cells["PackUomId"].Value) > 0)
				{
					CmbPackUom.Value = Conversion.ToInt(r.Cells["PackUomId"].Value);
				}
				if (Conversion.ToInt(r.Cells["AttributeVarientId"].Value) > 0)
				{
					CmbAttributeVarient.Value = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				}
				txtVarientEquivalent.Text = r.Cells["VarientUnit"].Value.ToString();
				comjobLot.Value = r.Cells["JobLotId"].Value.ToString();
				txtQty.Text = Conversion.ToDecimal(r.Cells["ItemQty"].Value).ToString("#,##0.##");
				txtItemWeight.Text = Conversion.ToDecimal(r.Cells["ItemWeight"].Value).ToString("#,##0.##");
				txtNetWeight.Text = Conversion.ToDecimal(r.Cells["NetWeight"].Value).ToString("#,##0.##");
				txtItemPrice.Text = Conversion.ToDecimal(r.Cells["ItemPrice"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAddLess.Text = Conversion.ToDecimal(r.Cells["AddLessRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtRate.Text = Conversion.ToDecimal(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDecimal(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				CmbDiscountType.Value = r.Cells["DiscountTypeId"].Value.ToString();
				txtDiscountRate.Text = Conversion.ToDecimal(r.Cells["DiscRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtDiscountAmount.Text = Conversion.ToDecimal(r.Cells["DiscAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Conversion.ToDecimal(r.Cells["ItemAmountWithDisc"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtgpdate.Value = Conversion.ToDateTime(r.Cells["GpDate"].Value);
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				((Control)(object)CmbVehicleNo).Text = r.Cells["VehicleNo"].Value.ToString();
				txtCity.Value = r.Cells["CityId"].Value.ToString();
				chkisFreeOfCost.Checked = Conversion.ToBool(r.Cells["IsFOC"].Value);
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

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.RowCount > 0 && SaleOrderExist)
			{
				throw new Exception("You can not Update manual Record beacause record against order exist in Grid");
			}
			if (grd.RowCount > 0 && GdnExist)
			{
				throw new Exception("You can not Update manual Record beacause record against Gdn exist in Grid");
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				int WareHouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(item.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
				if (Conversion.ToInt(comWarehouse.Value) == WareHouseId && Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists");
					return;
				}
			}
			this.ItemId = 0;
			dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
			dtGrid.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)comWarehouse).Text);
			dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
			dtGrid.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value);
			dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
			dtGrid.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ScheduleId"].Value);
			dtGrid.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
			dtGrid.Rows[updateDetailIndex]["PackUom"] = Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["UOMCode"].Value);
			dtGrid.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToString(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
			dtGrid.Rows[updateDetailIndex]["AttributeVarientId"] = ((!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? ((object)0) : CmbAttributeVarient.Value);
			dtGrid.Rows[updateDetailIndex]["AttributeVarient"] = ((!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? "" : ((Control)(object)CmbAttributeVarient).Text);
			dtGrid.Rows[updateDetailIndex]["VarientUnit"] = ((!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text));
			dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(comjobLot.Value);
			dtGrid.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)comjobLot).Text);
			dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
			dtGrid.Rows[updateDetailIndex]["ItemWeight"] = Conversion.ToDecimal((object)txtItemWeight.Text);
			dtGrid.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
			dtGrid.Rows[updateDetailIndex]["ItemPrice"] = Conversion.ToDecimal((object)txtItemPrice.Text);
			dtGrid.Rows[updateDetailIndex]["AddLessRate"] = Conversion.ToDecimal((object)txtAddLess.Text);
			dtGrid.Rows[updateDetailIndex]["Rate"] = Conversion.ToDecimal((object)txtRate.Text);
			dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDecimal((object)txtAmount.Text);
			dtGrid.Rows[updateDetailIndex]["DiscountTypeId"] = Conversion.ToInt(CmbDiscountType.Value);
			dtGrid.Rows[updateDetailIndex]["DiscountType"] = Conversion.ToString((object)((Control)(object)CmbDiscountType).Text);
			dtGrid.Rows[updateDetailIndex]["DiscRate"] = Conversion.ToDecimal((object)txtDiscountRate.Text);
			dtGrid.Rows[updateDetailIndex]["DiscAmount"] = Conversion.ToDecimal((object)txtDiscountAmount.Text);
			dtGrid.Rows[updateDetailIndex]["ItemAmountWithDisc"] = Conversion.ToDecimal((object)txtTotalAmount.Text);
			dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString();
			dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
			dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text);
			dtGrid.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
			dtGrid.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)txtCity).Text);
			dtGrid.Rows[updateDetailIndex]["IsFOC"] = Conversion.ToBool((object)chkisFreeOfCost.Checked);
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			TotalCommissionAmount();
			CommissionProportion();
			WagesProportion();
			OtherItemsProportion();
			FreightHeaderProportion();
			txtExchangeRate_TextChanged(null, null);
			WagesItemFillFromDetail();
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
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		try
		{
			WagesItemFillFromDetail();
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["GdnId"].Visible = false;
			grd.RootTable.Columns["GdnDetailId"].Visible = false;
			grd.RootTable.Columns["GdnNo"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["OrderNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["BalQty"].Visible = false;
			grd.RootTable.Columns["BalWeight"].Visible = false;
			grd.RootTable.Columns["DiscountTypeId"].Visible = false;
			grd.RootTable.Columns["Wages"].Visible = false;
			grd.RootTable.Columns["Freight"].Visible = false;
			grd.RootTable.Columns["Commission"].Visible = false;
			grd.RootTable.Columns["ItemWithVarient"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnNo"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemWithVarient"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemPrice"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLessRate"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscountTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscountType"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscRate"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmountWithDisc"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["GpDate"].EditType = (EditType)0;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grd.RootTable.Columns["CityId"].EditType = (EditType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["SettleDisc"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["Freight"].EditType = (EditType)0;
			grd.RootTable.Columns["Wages"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemNetAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["IsFOC"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmountWithDisc"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmountWithDisc"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmountWithDisc"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmountWithDisc"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["SettleDisc"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SettleDisc"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SettleDisc"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["SettleDisc"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Wages"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Wages"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Wages"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Wages"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			if (SaleOrderExist)
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
				grd.RootTable.Columns["GpNo"].EditType = (EditType)1;
				grd.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
				grd.RootTable.Columns["IsFOC"].EditType = (EditType)8;
				grd.RootTable.Columns["OrderNo"].Visible = SaleOrderExist;
				grd.RootTable.Columns["DiscRate"].EditType = (EditType)1;
				grd.RootTable.Columns["AddLessRate"].EditType = (EditType)0;
				grd.RootTable.Columns["DiscAmount"].EditType = (EditType)1;
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
			else if (GdnExist)
			{
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)0;
				grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
				grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
				grd.RootTable.Columns["IsFOC"].EditType = (EditType)0;
				grd.RootTable.Columns["GdnNo"].Visible = GdnExist;
				grd.RootTable.Columns["DiscRate"].EditType = (EditType)1;
				grd.RootTable.Columns["AddLessRate"].EditType = (EditType)1;
				grd.RootTable.Columns["DiscAmount"].EditType = (EditType)1;
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
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)0;
				grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
				grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
				grd.RootTable.Columns["AddLessRate"].EditType = (EditType)0;
				grd.RootTable.Columns["DiscRate"].EditType = (EditType)0;
				grd.RootTable.Columns["DiscAmount"].EditType = (EditType)0;
				grd.RootTable.Columns["OrderNo"].Visible = SaleOrderExist;
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!SaleOrderExist && !GdnExist)
			{
				return;
			}
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "ItemQty")
				{
					double Qty2 = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					double BalQty = Conversion.ToDouble(item.Cells["BalQty"].Value);
					double BalWt = Conversion.ToDouble(item.Cells["BalWeight"].Value);
					double ItemWt = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
					double unit2 = Conversion.ToDouble(item.Cells["VarientUnit"].Value);
					double PackUom = Conversion.ToDouble(item.Cells["PackUomEquivalent"].Value);
					double ItemPrice2 = Conversion.ToDouble(item.Cells["ItemPrice"].Value);
					double AddLessRate2 = Conversion.ToDouble(item.Cells["AddLessRate"].Value);
					double NetRate2 = 0.0;
					bool ItemWithVarient2 = Conversion.ToBool(item.Cells["ItemWithVarient"].Value);
					double NetWt = 0.0;
					double ItemAmt2 = 0.0;
					double DisRate3 = 0.0;
					double DisAmt3 = 0.0;
					if (Qty2 > BalQty)
					{
						MessageBox.Show("Item Qty Cannot be greater than Balance Qty " + BalQty + " Of this row");
						item.Cells["ItemQty"].Value = BalQty;
						NetWt = BalWt;
						item.Cells["NetWeight"].Value = NetWt;
					}
					else
					{
						if (PackUom > 0.0 && unit2 > 0.0)
						{
							NetWt = unit2 * Qty2 * PackUom * ItemWt;
							item.Cells["NetWeight"].Value = NetWt;
						}
						else if (PackUom > 0.0 && unit2 == 0.0)
						{
							NetWt = Qty2 * PackUom * ItemWt;
							item.Cells["NetWeight"].Value = NetWt;
						}
						else if (PackUom == 0.0 && unit2 > 0.0)
						{
							NetWt = Qty2 * unit2 * ItemWt;
							item.Cells["NetWeight"].Value = NetWt;
						}
						if (NetWt > BalWt)
						{
							NetWt = BalWt;
							item.Cells["NetWeight"].Value = NetWt;
						}
					}
					if (Qty2 > 0.0)
					{
						if (ItemWithVarient2)
						{
							NetRate2 = ItemPrice2 + AddLessRate2;
							item.Cells["Rate"].Value = NetRate2;
							ItemAmt2 = Qty2 * unit2 * NetRate2;
							item.Cells["ItemAmount"].Value = ItemAmt2;
						}
						else
						{
							NetRate2 = ItemPrice2 + AddLessRate2;
							item.Cells["Rate"].Value = NetRate2;
							ItemAmt2 = Qty2 * NetRate2;
							item.Cells["ItemAmount"].Value = ItemAmt2;
						}
					}
					else
					{
						ItemAmt2 = 0.0;
						item.Cells["ItemAmount"].Value = ItemAmt2;
					}
					if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) != 0)
					{
						DisRate3 = Conversion.ToDouble(item.Cells["DiscRate"].Value);
						if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 1)
						{
							DisAmt3 = DisRate3;
							item.Cells["DiscAmount"].Value = DisAmt3;
						}
						else if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 2)
						{
							if (DisRate3 >= 100.0)
							{
								MessageBox.Show("DiscountRate Cannot be greater than 99 when Discount Type is Percent");
								DisRate3 = 99.0;
								item.Cells["DiscRate"].Value = DisRate3;
							}
							DisAmt3 = ItemAmt2 * DisRate3 / 100.0;
							item.Cells["DiscAmount"].Value = DisAmt3;
						}
					}
					else
					{
						DisAmt3 = 0.0;
						item.Cells["DiscAmount"].Value = 0;
					}
					if (ItemAmt2 > 0.0 && DisAmt3 > 0.0)
					{
						double ItemAmountWithDisc2 = ItemAmt2 - DisAmt3;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
					}
					else if (ItemAmt2 > 0.0)
					{
						double ItemAmountWithDisc2 = ItemAmt2;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
					}
					else
					{
						double ItemAmountWithDisc2 = 0.0;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
					}
				}
				if (e.Column.Key == "AddLessRate")
				{
					double Qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					Conversion.ToDouble(item.Cells["BalQty"].Value);
					Conversion.ToDouble(item.Cells["BalWeight"].Value);
					Conversion.ToDouble(item.Cells["ItemWeight"].Value);
					Conversion.ToDouble(item.Cells["NetWeight"].Value);
					double unit = Conversion.ToDouble(item.Cells["VarientUnit"].Value);
					Conversion.ToDouble(item.Cells["PackUomEquivalent"].Value);
					double ItemPrice = Conversion.ToDouble(item.Cells["ItemPrice"].Value);
					double AddLessRate = Conversion.ToDouble(item.Cells["AddLessRate"].Value);
					bool ItemWithVarient = Conversion.ToBool(item.Cells["ItemWithVarient"].Value);
					double NetRate = 0.0;
					double ItemAmt = 0.0;
					double DisRate2 = 0.0;
					double DisAmt2 = 0.0;
					if (Qty > 0.0)
					{
						if (ItemWithVarient)
						{
							NetRate = ItemPrice + AddLessRate;
							item.Cells["Rate"].Value = NetRate;
							ItemAmt = Qty * unit * NetRate;
							item.Cells["ItemAmount"].Value = ItemAmt;
						}
						else
						{
							NetRate = ItemPrice + AddLessRate;
							item.Cells["Rate"].Value = NetRate;
							ItemAmt = Qty * NetRate;
							item.Cells["ItemAmount"].Value = ItemAmt;
						}
					}
					else
					{
						ItemAmt = 0.0;
						item.Cells["ItemAmount"].Value = ItemAmt;
					}
					if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) != 0)
					{
						DisRate2 = Conversion.ToDouble(item.Cells["DiscRate"].Value);
						if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 1)
						{
							DisAmt2 = DisRate2;
							item.Cells["DiscAmount"].Value = DisAmt2;
						}
						else if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 2)
						{
							if (DisRate2 >= 100.0)
							{
								MessageBox.Show("DiscountRate Cannot be greater than 99 when Discount Type is Percent");
								DisRate2 = 99.0;
								item.Cells["DiscRate"].Value = DisRate2;
							}
							DisAmt2 = ItemAmt * DisRate2 / 100.0;
							item.Cells["DiscAmount"].Value = DisAmt2;
						}
					}
					else
					{
						DisAmt2 = 0.0;
						item.Cells["DiscAmount"].Value = 0;
					}
					if (ItemAmt > 0.0 && DisAmt2 > 0.0)
					{
						double ItemAmountWithDisc = ItemAmt - DisAmt2;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
					}
					else if (ItemAmt > 0.0)
					{
						double ItemAmountWithDisc = ItemAmt;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
					}
					else
					{
						double ItemAmountWithDisc = 0.0;
						item.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
					}
				}
				if (e.Column.Key == "DiscountTypeId" || e.Column.Key == "DiscRate")
				{
					double AmtWithoutDisc = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
					double DisRate = Conversion.ToDouble(item.Cells["DiscRate"].Value);
					double DisAmt = 0.0;
					if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) != 0)
					{
						if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 1)
						{
							DisAmt = DisRate;
							item.Cells["DiscAmount"].Value = DisAmt;
						}
						else if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 2)
						{
							DisAmt = AmtWithoutDisc * DisRate / 100.0;
							item.Cells["DiscAmount"].Value = DisAmt;
						}
					}
					else
					{
						DisAmt = 0.0;
						item.Cells["DiscAmount"].Value = 0;
					}
					if (AmtWithoutDisc > 0.0 && DisAmt > 0.0)
					{
						item.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc - DisAmt;
					}
					else if (AmtWithoutDisc > 0.0)
					{
						item.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc;
					}
					else
					{
						item.Cells["ItemAmountWithDisc"].Value = 0;
					}
				}
			}
			grd.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grd);
			WagesItemFillFromDetail();
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
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.InvGdnDetailId = 0;
					detail.InvGdnId = 0;
					detail.InvGdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
					detail.InvGdnDetailId = Conversion.ToInt(r.Cells["GdnDetailId"].Value);
					detail.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					detail.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					detail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRateWithOutAddLess = Conversion.ToDecimal(r.Cells["ItemPrice"].Value);
					detail.RateAddLess = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.RateCut = detail.RateAddLess;
					detail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					detail.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountTypeId"].Value);
					detail.ItemDiscountRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
					detail.ItemDiscountAmount = Conversion.ToDecimal(r.Cells["DiscAmount"].Value);
					detail.ItemAmountWithDisc = Conversion.ToDecimal(r.Cells["ItemAmountWithDisc"].Value);
					detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					detail.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
					detail.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
					detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					detail.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
					detail.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
					detail.FreightAmount = Conversion.ToDecimal(r.Cells["Freight"].Value);
					detail.JournalAmount = Conversion.ToDecimal(r.Cells["SettleDisc"].Value);
					detail.WagesAmount = Conversion.ToDecimal(r.Cells["Wages"].Value);
					detail.ItemNetAmount = Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value);
					detail.RemarksDetail = "";
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtGrid.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			if (e.Column.Key == "Add")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null)
				{
					DataRow dr = dtGrid.Rows[item.RowIndex];
					dtGrid.Rows.Add(dr.ItemArray);
					dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
					grd.DataSource = dtGrid;
				}
			}
			WagesItemFillFromDetail();
			TotalCommissionAmount();
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "IsFOC")
			{
				grd.UpdateData();
				txtExchangeRate_TextChanged(null, null);
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		dtWagesDetail.Rows.Add(0, 0, 0, 0, 0, 0, "", 0, "", 0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, 0, "");
		grdContractorWages.DataSource = dtWagesDetail;
	}

	private void GenerateRowsInWagesGrid()
	{
		try
		{
			if (dtGrid.Rows.Count <= 0 || dtVarientForWages.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow d in dtVarientForWages.Rows)
			{
				int ItemId = Conversion.ToInt(d["ItemId"]);
				string ItemName = Conversion.ToString(d["ItemName"]);
				int ItemAttributeId = Conversion.ToInt(d["ItemAttributeId"]);
				string ItemAttribute = Conversion.ToString(d["ItemAttribute"]);
				decimal VarientEquivalent = Conversion.ToDecimal(d["VarientEquivalent"]);
				decimal Qty = Conversion.ToDecimal(d["Qty"]);
				decimal NetWeight = Conversion.ToDecimal(d["NetWeight"]);
				int ContractorId = 0;
				DataTable serviceActivityByItemId = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(d["ItemId"]), Conversion.ToDateTime((object)DocDate.Value), 0, ContractorId);
				bool flag = false;
				foreach (DataRow r in serviceActivityByItemId.Rows)
				{
					if (grdContractorWages.GetDataRows().Length != 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow item in dataRows)
						{
							if (Conversion.ToInt(item.Cells["ItemId"].Value) == ItemId && Conversion.ToInt(item.Cells["AttributeVarientId"].Value) == ItemAttributeId && Conversion.ToInt(item.Cells["ContractorWagesRateScheduleId"].Value) == Conversion.ToInt(r["ContractorWagesRateScheduleId"]))
							{
								item.BeginEdit();
								item.Cells["Qty"].Value = Qty;
								item.Cells["ItemNetWeight"].Value = NetWeight;
								decimal Amount2 = default(decimal);
								int ParentUomId2 = Conversion.ToInt(r["ParentUomId"]);
								decimal Rate2 = Conversion.ToDecimal(r["Rate"]);
								Amount2 = ParentUomId2 switch
								{
									2 => Rate2 * Qty, 
									3 => NetWeight / 1000m * Rate2, 
									_ => default(decimal), 
								};
								item.Cells["Amount"].Value = Amount2;
								decimal AddLess = Conversion.ToDecimal(item.Cells["AddLess"].Value);
								item.Cells["NetAmount"].Value = Amount2 + AddLess;
								item.EndEdit();
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						decimal Amount = default(decimal);
						int ParentUomId = Conversion.ToInt(r["ParentUomId"]);
						decimal Rate = Conversion.ToDecimal(r["Rate"]);
						Amount = ParentUomId switch
						{
							2 => Rate * Qty, 
							3 => NetWeight / 1000m * Rate, 
							_ => default(decimal), 
						};
						dtWagesDetail.Rows.Add(0, 0, 0, 0, 0, ItemId, ItemName, ItemAttributeId, ItemAttribute, VarientEquivalent, 0, "", Conversion.ToInt(r["ContractorWagesRateScheduleId"]), r["WagesActivity"], ParentUomId, Qty, NetWeight, Rate, Amount, 0, Amount, "");
					}
					flag = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesItemFillFromDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			dtItemForWages.Clear();
			dtVarientForWages.Clear();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (!dtItemForWages.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					dtItemForWages.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value));
				}
				if (!dtVarientForWages.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeId") == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && row.Field<int>("ItemId") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					decimal ItemQty = (from GridEXRow row in grd.GetRows()
						where Conversion.ToInt(row.Cells["AttributeVarientId"].Value) == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && Conversion.ToInt(row.Cells["ItemId"].Value) == Conversion.ToInt(r.Cells["ItemId"].Value)
						select (decimal)row.Cells["ItemQty"].Value).Sum();
					decimal NetWeight = (from GridEXRow row in grd.GetRows()
						where Conversion.ToInt(row.Cells["AttributeVarientId"].Value) == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && Conversion.ToInt(row.Cells["ItemId"].Value) == Conversion.ToInt(r.Cells["ItemId"].Value)
						select (decimal)row.Cells["NetWeight"].Value).Sum();
					dtVarientForWages.Rows.Add(Conversion.ToInt(r.Cells["AttributeVarientId"].Value), Conversion.ToString(r.Cells["AttributeVarient"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["VarientUnit"].Value), ItemQty, NetWeight);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["GdnId"].Visible = false;
			grdContractorWages.RootTable.Columns["GdnWagesId"].Visible = false;
			grdContractorWages.RootTable.Columns["OrderId"].Visible = false;
			grdContractorWages.RootTable.Columns["OrderWagesId"].Visible = false;
			grdContractorWages.RootTable.Columns["ItemId"].Visible = false;
			grdContractorWages.RootTable.Columns["AttributeVarientId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["ParentUomId"].Visible = false;
			grdContractorWages.RootTable.Columns["AttributeVarient"].Caption = "Varient";
			grdContractorWages.RootTable.Columns["VarientEquivalent"].Caption = "Unit";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].Caption = "Weight";
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OrderWagesId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ParentUomId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AddLess"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["NetAmount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["AddLess"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["AddLess"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Groups.Add("ServiceActivity");
			grdContractorWages.RootTable.GroupTotals = (GroupTotals)2;
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			grdContractorWages.RootTable.Columns.Add("DeleteAll");
			grdContractorWages.RootTable.Columns["DeleteAll"].Key = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].Caption = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonText = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].Width = 80;
			grdContractorWages.RootTable.Columns["DeleteAll"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 40;
			grdContractorWages.RootTable.Columns["Delete"].Position = 1;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 40;
			grdContractorWages.RootTable.Columns["Add"].Position = 2;
			grdContractorWages.FrozenColumns = 3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal NetAmount2 = default(decimal);
				decimal Qty = default(decimal);
				decimal Weight = default(decimal);
				decimal Rate = default(decimal);
				decimal AddLess2 = default(decimal);
				int ParentUomId = 0;
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				Qty = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				DataRow foundRow = dtitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(item.Cells["ItemId"].Value));
				decimal obj = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemWeight"]) : 0m);
				decimal VarientEquivalent = Conversion.ToDecimal((object)item.Cells["VarientEquivalent"].Text.ToString());
				Weight = obj * Qty * VarientEquivalent;
				item.Cells["ItemNetWeight"].Value = Weight;
				Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				AddLess2 = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				switch (Conversion.ToInt((object)item.Cells["ParentUomId"].Text.ToString()))
				{
				case 2:
					Amount = Rate * Qty;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				case 3:
					Amount = Weight / 1000m * Rate;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				default:
					item.Cells["Amount"].Value = 0;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
					return;
				}
			}
			if (e.Column.Key == "AddLess" && item != null && (int)item.RowType == 0)
			{
				decimal AddLess = default(decimal);
				decimal NetAmount = default(decimal);
				decimal num = Conversion.ToDecimal((object)item.Cells["Amount"].Text.ToString());
				AddLess = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				NetAmount = num + AddLess;
				item.Cells["NetAmount"].Value = NetAmount;
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			WagesProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdContractorWages.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtWagesDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "DeleteAll")
			{
				string Activity = Conversion.ToString(item.Cells["ServiceActivity"].Value);
				if (MessageBox.Show("Are you sure to Delete All Rows With Activity " + Activity + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (grdContractorWages.GetDataRows().Length != 0)
				{
					GridEXRow[] dataRows = grdContractorWages.GetDataRows();
					foreach (GridEXRow r in dataRows)
					{
						if (r != null && (int)r.RowType == 0 && Conversion.ToString(r.Cells["ServiceActivity"].Value) == Activity)
						{
							dtWagesDetail.Rows[r.RowIndex].Delete();
						}
					}
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			WagesProportion();
			txtExchangeRate_TextChanged(null, null);
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesValidationWithDetail()
	{
		try
		{
			if (grd.RowCount <= 0 || grdContractorWages.RowCount <= 0 || dtVarientForWages.Rows.Count <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				string item = Conversion.ToString(r.Cells["ItemName"].Value);
				Conversion.ToString(r.Cells["AttributeVarient"].Value);
				int attributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				int contractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				if (itemId != 0)
				{
					if (!dtGrid.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == itemId))
					{
						throw new Exception("Item " + item + " in Contractor Wages Grid  does not exist in Detail Grid ");
					}
					(from row in dtGrid.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("AttributeVarientId") == attributeVarientId
						select row).Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					(from row in dtWagesDetail.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("AttributeVarientId") == attributeVarientId && row.Field<int>("ContractorWagesRateScheduleId") == contractorWagesRateScheduleId
						select row).Sum((DataRow row) => row.Field<decimal>("Qty"));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnGenerateWagesRows_Click(object sender, EventArgs e)
	{
		try
		{
			WagesItemFillFromDetail();
			GenerateRowsInWagesGrid();
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillWagesGridFilterCombo()
	{
		try
		{
			int Id1 = Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value);
			string Id2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
			if (grdContractorWages.GetDataRows().Count() > 0 && dtWagesDetail.Rows.Count > 0)
			{
				DataTable distinctRowsActivity = (from row in dtWagesDetail.AsEnumerable()
					group row by row.Field<string>("ServiceActivity") into @group
					select @group.First()).CopyToDataTable();
				if (dtContr.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtContr, CmbContractorForUpdateInWagesGrid, "Id", "CompanyName", "ContractorName", true);
					if (Id1 > 0)
					{
						if (dtContr.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
						{
							CmbContractorForUpdateInWagesGrid.Value = Id1;
						}
						else
						{
							((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				}
				if (distinctRowsActivity.Rows.Count > 0)
				{
					DDL.BindDDLNew(distinctRowsActivity, CmbActivityforContractorUpdateInWagesGrid, "ContractorWagesRateScheduleId", "ServiceActivity", "ServiceActivity", true);
					if (Id2 != null && Id2 != string.Empty)
					{
						if (distinctRowsActivity.AsEnumerable().Any((DataRow row) => row.Field<string>("ServiceActivity") == Id2))
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = Id2;
						}
						else
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateConctratorInWages_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0)
			{
				GridEXRow[] dataRows;
				if ((((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) == 0) && (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) == 0))
				{
					GridEXRow[] GridRows = grdContractorWages.GetDataRows();
					if (Conversion.ToInt(GridRows[0].Cells["ContractorId"].Value) != 0)
					{
						int ContrId = 0;
						string ContrName = "";
						ContrId = Conversion.ToInt(GridRows[0].Cells["ContractorId"].Value);
						ContrName = Conversion.ToString(GridRows[0].Cells["ContractorName"].Value);
						dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow r in dataRows)
						{
							r.BeginEdit();
							r.Cells["ContractorId"].Value = ContrId;
							r.Cells["ContractorName"].Value = ContrName;
							decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
							string ServiceActivityId2 = Conversion.ToString(r.Cells["ServiceActivity"].Value);
							DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId);
							if (dt != null && dt.Rows.Count > 0)
							{
								DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId2);
								decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
								int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
								int ContractorWagesRateScheduleId = ((foundRow != null) ? Conversion.ToInt(foundRow["ContractorWagesRateScheduleId"]) : 0);
								r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
								r.Cells["Rate"].Value = Rate;
								r.Cells["ParentUomId"].Value = ParentUomId;
								decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
								switch (ParentUomId)
								{
								case 2:
								{
									decimal Amount = Rate * RowQty;
									r.Cells["Amount"].Value = Amount;
									decimal NetAmount = Amount + AddLess;
									r.Cells["NetAmount"].Value = NetAmount;
									break;
								}
								case 3:
								{
									decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
									r.Cells["Amount"].Value = Amount2;
									decimal NetAmount2 = Amount2 + AddLess;
									r.Cells["NetAmount"].Value = NetAmount2;
									break;
								}
								default:
								{
									r.Cells["ParentUomId"].Value = 0;
									r.Cells["Rate"].Value = 0;
									decimal Amount3 = default(decimal);
									r.Cells["Amount"].Value = Amount3;
									decimal NetAmount3 = Amount3 + AddLess;
									r.Cells["NetAmount"].Value = NetAmount3;
									break;
								}
								}
							}
							r.EndEdit();
						}
						return;
					}
					throw new Exception("ContractorName Field is Empty in Wages Grid Row No: 01");
				}
				if (((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) != 0 && (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) == 0))
				{
					int ContrId2 = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
					string ContrName2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
					dataRows = grdContractorWages.GetDataRows();
					foreach (GridEXRow r2 in dataRows)
					{
						r2.BeginEdit();
						r2.Cells["ContractorId"].Value = ContrId2;
						r2.Cells["ContractorName"].Value = ContrName2;
						decimal AddLess2 = Conversion.ToInt(r2.Cells["AddLess"].Value);
						string ServiceActivityId = Conversion.ToString(r2.Cells["ServiceActivity"].Value);
						DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r2.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId2);
						if (dt2 != null && dt2.Rows.Count > 0)
						{
							DataRow foundRow2 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
							decimal Rate2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Rate"]) : 0m);
							int ParentUomId2 = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ParentUOMId"]) : 0);
							int ContractorWagesRateScheduleId2 = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ContractorWagesRateScheduleId"]) : 0);
							r2.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId2;
							r2.Cells["Rate"].Value = Rate2;
							r2.Cells["ParentUomId"].Value = ParentUomId2;
							decimal RowQty2 = Conversion.ToDecimal(r2.Cells["Qty"].Value);
							switch (ParentUomId2)
							{
							case 2:
							{
								decimal Amount4 = Rate2 * RowQty2;
								r2.Cells["Amount"].Value = Amount4;
								decimal NetAmount4 = Amount4 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount4;
								break;
							}
							case 3:
							{
								decimal Amount5 = (decimal)Conversion.ToInt(r2.Cells["ItemNetWeight"].Value) / 1000m * Rate2;
								r2.Cells["Amount"].Value = Amount5;
								decimal NetAmount5 = Amount5 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount5;
								break;
							}
							default:
							{
								r2.Cells["ParentUomId"].Value = 0;
								r2.Cells["Rate"].Value = 0;
								decimal Amount6 = default(decimal);
								r2.Cells["Amount"].Value = Amount6;
								decimal NetAmount6 = Amount6 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount6;
								break;
							}
							}
						}
						r2.EndEdit();
					}
					return;
				}
				if ((((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) == 0) && ((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) != 0)
				{
					CmbContractorForUpdateInWagesGrid.Focus();
					throw new Exception("ContractorName Field is Empty");
				}
				int ContrId3 = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
				string ContrName3 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
				string ActivtyName = Conversion.ToString((object)((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text);
				dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow r3 in dataRows)
				{
					if (!(Conversion.ToString(r3.Cells["ServiceActivity"].Value) == ActivtyName))
					{
						continue;
					}
					r3.BeginEdit();
					r3.Cells["ContractorId"].Value = ContrId3;
					r3.Cells["ContractorName"].Value = ContrName3;
					decimal AddLess3 = Conversion.ToInt(r3.Cells["AddLess"].Value);
					DataTable dt3 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r3.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId3);
					if (dt3 != null && dt3.Rows.Count > 0)
					{
						DataRow foundRow3 = dt3.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == Conversion.ToString(r3.Cells["ServiceActivity"].Value));
						decimal Rate3 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Rate"]) : 0m);
						int ParentUomId3 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ParentUOMId"]) : 0);
						int ContractorWagesRateScheduleId3 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ContractorWagesRateScheduleId"]) : 0);
						r3.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId3;
						r3.Cells["Rate"].Value = Rate3;
						r3.Cells["ParentUomId"].Value = ParentUomId3;
						decimal RowQty3 = Conversion.ToDecimal(r3.Cells["Qty"].Value);
						switch (ParentUomId3)
						{
						case 2:
						{
							decimal Amount7 = Rate3 * RowQty3;
							r3.Cells["Amount"].Value = Amount7;
							decimal NetAmount7 = Amount7 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount7;
							break;
						}
						case 3:
						{
							decimal Amount8 = (decimal)Conversion.ToInt(r3.Cells["ItemNetWeight"].Value) / 1000m * Rate3;
							r3.Cells["Amount"].Value = Amount8;
							decimal NetAmount8 = Amount8 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount8;
							break;
						}
						default:
						{
							r3.Cells["ParentUomId"].Value = 0;
							r3.Cells["Rate"].Value = 0;
							decimal Amount9 = default(decimal);
							r3.Cells["Amount"].Value = Amount9;
							decimal NetAmount9 = Amount9 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount9;
							break;
						}
						}
					}
					r3.EndEdit();
				}
				return;
			}
			throw new Exception("Wages Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateWagesRateForAllRows()
	{
		try
		{
			if (grdContractorWages.GetDataRows().Length == 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				Conversion.ToString(r.Cells["ContractorName"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				if (ItemId <= 0)
				{
					continue;
				}
				r.BeginEdit();
				decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, ItemId, Conversion.ToDateTime((object)DocDate.Value), 0, ContrId);
				if (dt != null && dt.Rows.Count > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
					int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
					int ContractorWagesRateScheduleId = ((foundRow != null) ? Conversion.ToInt(foundRow["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					r.Cells["Rate"].Value = Rate;
					r.Cells["ParentUomId"].Value = ParentUomId;
					decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					switch (ParentUomId)
					{
					case 2:
					{
						decimal Amount = Rate * RowQty;
						r.Cells["Amount"].Value = Amount;
						decimal NetAmount = Amount + AddLess;
						r.Cells["NetAmount"].Value = NetAmount;
						break;
					}
					case 3:
					{
						decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
						r.Cells["Amount"].Value = Amount2;
						decimal NetAmount2 = Amount2 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount2;
						break;
					}
					default:
					{
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount3 = default(decimal);
						r.Cells["Amount"].Value = Amount3;
						decimal NetAmount3 = Amount3 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount3;
						break;
					}
					}
				}
				else
				{
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					decimal Amount4 = default(decimal);
					r.Cells["Amount"].Value = Amount4;
					decimal NetAmount4 = Amount4 + AddLess;
					r.Cells["NetAmount"].Value = NetAmount4;
				}
				r.EndEdit();
			}
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
		if (txtRefPartyName.Text.Trim() == "" || txtRefPartyName.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Name Field is Required");
			txtRefPartyName.Focus();
			return false;
		}
		if (txtRefPartyAddress.Text.Trim() == "" || txtRefPartyAddress.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Address Field is Required");
			txtRefPartyAddress.Focus();
			return false;
		}
		if (txtRefPartyCellNo.Text.Trim() == "" || txtRefPartyCellNo.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party CellNo Field is Required");
			txtRefPartyCellNo.Focus();
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
		if (Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit" && (txtduedays.Text.Trim() == "" || txtduedays.Text.Trim() == null))
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
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
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Uom field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value) && Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Variant field is required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value) && Conversion.ToInt(CmbAttributeVarient.Value) > 0)
		{
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
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
			Id = 0;
			VoucherHeadId = 0;
			ItemId = 0;
			chkisFreeOfCost.Checked = false;
			Approved = false;
			((Control)(object)comsupplier).Enabled = true;
			AT = new Attachment();
			((Control)(object)comsupplier).Text = string.Empty;
			txtDistance.Text = string.Empty;
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtOtherWagesHeader.Text = string.Empty;
			txtSupplierReference.Text = string.Empty;
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
			((Control)(object)CmbOtherWagesAccount).Text = string.Empty;
			((Control)(object)cmbSettlementDiscountAccount).Text = string.Empty;
			txtFreightAmountHeader.Text = string.Empty;
			txtItemAmountHeader.Text = string.Empty;
			txtDiscountHeader.Text = string.Empty;
			txtItemNetAmountHeader.Text = string.Empty;
			txtSettlementDiscountHeader.Text = string.Empty;
			((Control)(object)CmbCommissionDebitAccount).Text = string.Empty;
			txtBillamountWithoutCommission.Text = string.Empty;
			txtduedays.Text = "0";
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			lstRemoveDetailRecord.Clear();
			SaleOrderExist = false;
			GdnExist = false;
			((Control)(object)comItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtItemPrice.Text = string.Empty;
			txtAddLess.Text = string.Empty;
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
			txtManualTotalBillAmount.Text = string.Empty;
			ConfigurationDefault();
			EnableDiableCommissionFields();
			FillWagesGridFilterCombo();
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
			chkisFreeOfCost.Checked = false;
			txtQty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtItemPrice.Text = string.Empty;
			txtAddLess.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
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
		((Control)(object)CmbVehicleNo).Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		comItem.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CurrencyFill();
			BindVehicles();
			suppliercustomer();
			VisitedByFill();
			PaymentTerms();
			RefSalesManFill();
			TransporterAcFill();
			OtherWagesAcFill();
			SettlementDiscAccountAndCommissionDebitAccountFill();
			bindWareHouse();
			item();
			JobLotBind();
			BinCity();
			dtForGridComboFill();
			ConfigurationDefault();
			FillWagesGridFilterCombo();
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
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Expected O, but got Unknown
		//IL_14de: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e5: Expected O, but got Unknown
		//IL_16ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f6: Expected O, but got Unknown
		//IL_19d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Expected O, but got Unknown
		//IL_1f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f70: Expected O, but got Unknown
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
			if (Conversion.ToDecimal((object)txtOtherWagesHeader.Text.Trim()) > 0m)
			{
				int num2 = Conversion.ToInt(CmbOtherWagesAccount.Value);
				if (num2 == 0)
				{
					CmbOtherWagesAccount.Focus();
					throw new Exception("Labour Account field Required");
				}
				int SupGlId2 = Conversion.ToInt((object)txtSupplierGLId.Text);
				if (num2 == SupGlId2)
				{
					throw new Exception("Labour Account can not be same as Customer Please check");
				}
			}
			if (Conversion.ToDecimal((object)txtcommamount.Text.Trim()) > 0m)
			{
				int CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
				int CommissionDebitAccountId = Conversion.ToInt(CmbCommissionDebitAccount.Value);
				if (CommissionAgentId == 0)
				{
					MessageBox.Show("Please Select Commission Agent Account First");
					combcommAgent.Focus();
					return;
				}
				if (Conversion.ToInt(CmbRefSalesMan.Value) == 0)
				{
					MessageBox.Show("Please Select Ref Sales Man First");
					CmbRefSalesMan.Focus();
					return;
				}
				if (CommissionDebitAccountId == 0)
				{
					CmbCommissionDebitAccount.Focus();
					throw new Exception("CommissionDebitAccount Account field Required");
				}
				DataTable dt = CommonServices.GetSupplierCustomerInfoByGLAccountId(CommissionDebitAccountId);
				if (dt.Rows.Count > 0)
				{
					int SupGlId3 = Conversion.ToInt(dt.Rows[0][0]);
					if (CommissionAgentId == SupGlId3)
					{
						CmbCommissionDebitAccount.Focus();
						throw new Exception("CommissionDebit Account can not be same as Commission agent Please check");
					}
				}
			}
			InvSaleInvoice obj = new InvSaleInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToDecimal(r3.Cells["Credit"].Value) > 0m || Conversion.ToInt(r3.Cells["Debit"].Value) > 0)
				{
					int num3 = Conversion.ToInt(r3.Cells["AccountId"].Value);
					int GlAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
					if (num3 == 0 && GlAccountId == 0)
					{
						MessageBox.Show("Please Select an Account Party Addless Grid in Row no: " + (r3.RowIndex + 1));
						return;
					}
				}
			}
			rows = grdChargeToProduct.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToDecimal(r5.Cells["Amount"].Value) > 0m && Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			WagesValidationWithDetail();
			WagesProportion();
			SettlementDiscountProportion();
			OtherItemsProportion();
			FreightHeaderProportion();
			GridItemNetAmountCalculate();
			txtExchangeRate_TextChanged(null, null);
			if (Conversion.ToDecimal((object)txtItemAmountHeader.Text) == 0m || Conversion.ToDecimal((object)txtItemNetAmountHeader.Text) == 0m)
			{
				throw new Exception("ItemAmount and ItemNetAmount can not equal to zero Please check");
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1856;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.RefrenenceNo = txtSupplierReference.Text.Trim();
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			obj.VisitedById = Conversion.ToInt(CmbVisitedbyName.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.DeliveryStartDate = Conversion.ToDateTime((object)txtDeliveryStartDate.Value);
			obj.DeliveryDays = Conversion.ToInt((object)txtDeliverydays.Text);
			obj.ExpiryDate = Conversion.ToDateTime((object)txtDeliveryStartDate.Value).AddDays(Conversion.ToInt((object)txtDeliverydays.Text));
			obj.ReferencPartyName = Conversion.ToString((object)txtRefPartyName.Text);
			obj.ReferencPartyAddress = Conversion.ToString((object)txtRefPartyAddress.Text);
			obj.ReferencPartyCellNo = Conversion.ToString((object)txtRefPartyCellNo.Text);
			obj.Distance = Conversion.ToString((object)txtDistance.Text);
			obj.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			obj.RefSalesManId = Conversion.ToInt(CmbRefSalesMan.Value);
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
			obj.OtherWagesAccountId = Conversion.ToInt(CmbOtherWagesAccount.Value);
			obj.SettlementDiscountAccountId = Conversion.ToInt(cmbSettlementDiscountAccount.Value);
			obj.CommissionDebitAcId = Conversion.ToInt(CmbCommissionDebitAccount.Value);
			obj.ItemAmountHeader = Conversion.ToDecimal((object)txtItemAmountHeader.Text);
			obj.DiscountAmountHeader = Conversion.ToDecimal((object)txtDiscountHeader.Text);
			obj.ItemNetAmountHeader = Conversion.ToDecimal((object)txtItemNetAmountHeader.Text);
			obj.OtherWagesHeader = Conversion.ToDecimal((object)txtOtherWagesHeader.Text);
			obj.FrieghtAmountHeader = Conversion.ToDecimal((object)txtFreightAmountHeader.Text);
			obj.SettlementDiscountHeader = Conversion.ToDecimal((object)txtSettlementDiscountHeader.Text);
			obj.BillAmountWithoutDiscount = Conversion.ToDecimal((object)txtBillamountWithoutCommission.Text.Trim());
			obj.BillAmount = Conversion.ToDecimal((object)txtBillAmountHeader.Text.Trim());
			obj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			obj.InvSaleInvoiceWagesDetailList = new List<InvSaleInvoiceWagesDetail>();
			obj.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			obj.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			obj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			obj.AttachmentsList = new List<DMSAttachments>();
			decimal GrossWt = default(decimal);
			bool FOCExist = false;
			rows = grd.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
				detail.Id = Conversion.ToInt(r6.Cells["Id"].Value);
				if (detail.Id > 0)
				{
					detail.ActionTypeId = 2;
				}
				else
				{
					detail.ActionTypeId = 1;
				}
				detail.LineId = r6.RowIndex + 1;
				detail.InvGdnDetailId = 0;
				detail.InvGdnId = 0;
				detail.InvGdnId = Conversion.ToInt(r6.Cells["GdnId"].Value);
				detail.InvGdnDetailId = Conversion.ToInt(r6.Cells["GdnDetailId"].Value);
				detail.SaleOrderId = Conversion.ToInt(r6.Cells["OrderId"].Value);
				detail.SaleOrderDetailId = Conversion.ToInt(r6.Cells["OrderDetailId"].Value);
				detail.SaleOrderNo = Conversion.ToInt(r6.Cells["OrderNo"].Value);
				if (!FOCExist)
				{
					FOCExist = !Conversion.ToBool(r6.Cells["IsFoc"].Value);
				}
				if (Conversion.ToInt(r6.Cells["WarehouseId"].Value) == 0)
				{
					throw new Exception("Warehouse Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.WarehouseId = Conversion.ToInt(r6.Cells["WarehouseId"].Value);
				if (Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Item Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemId = Conversion.ToInt(r6.Cells["ItemId"].Value);
				if (Conversion.ToInt(r6.Cells["AttributeVarientId"].Value) == 0 && Conversion.ToInt(r6.Cells["PackUomId"].Value) == 0)
				{
					throw new Exception("AttributeVarient or PackUomId Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemUomId = Conversion.ToInt(r6.Cells["PackUomId"].Value);
				detail.ItemAttributeVarientId = Conversion.ToInt(r6.Cells["AttributeVarientId"].Value);
				if (Conversion.ToInt(r6.Cells["AttributeVarientId"].Value) > 0 && Conversion.ToDecimal(r6.Cells["VarientUnit"].Value) == 0m)
				{
					throw new Exception("VarientUnit Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.VarientEquivalent = Conversion.ToDecimal(r6.Cells["VarientUnit"].Value);
				if (SaleOrderExist)
				{
					if (Conversion.ToInt(r6.Cells["AttributeVarientId"].Value) == 0)
					{
						throw new Exception("AttributeVarient Required when Entry Against Order in Detail Grid And row No: " + (r6.RowIndex + 1));
					}
					if (Conversion.ToDecimal(r6.Cells["VarientUnit"].Value) == 0m)
					{
						throw new Exception("VarientUnit Required in Detail Grid And row No: " + (r6.RowIndex + 1));
					}
				}
				if (Conversion.ToInt(r6.Cells["JobLotId"].Value) == 0)
				{
					throw new Exception("JobLot Required when Entry Against Order in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.JobLotId = Conversion.ToInt(r6.Cells["JobLotId"].Value);
				if (Conversion.ToDecimal(r6.Cells["ItemQty"].Value) == 0m)
				{
					throw new Exception("ItemQty Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemQty = Conversion.ToDecimal(r6.Cells["ItemQty"].Value);
				if (Conversion.ToDecimal(r6.Cells["ItemWeight"].Value) == 0m)
				{
					throw new Exception("ItemWeight Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemWeight = Conversion.ToDecimal(r6.Cells["ItemWeight"].Value);
				if (Conversion.ToDecimal(r6.Cells["NetWeight"].Value) == 0m)
				{
					throw new Exception("NetWeight Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemNetWeight = Conversion.ToDecimal(r6.Cells["NetWeight"].Value);
				GrossWt += detail.ItemNetWeight;
				if (Conversion.ToDecimal(r6.Cells["ItemPrice"].Value) == 0m)
				{
					throw new Exception("ItemPrice Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemRateWithOutAddLess = Conversion.ToDecimal(r6.Cells["ItemPrice"].Value);
				if (Conversion.ToDecimal(r6.Cells["Rate"].Value) <= 0m)
				{
					throw new Exception("ItemRate Required in Detail Grid And row No: " + (r6.RowIndex + 1) + "\nItemRate Can not Less than Equal to 0");
				}
				detail.ItemRate = Conversion.ToDecimal(r6.Cells["Rate"].Value);
				detail.ItemRateWithOutAddLess = Conversion.ToDecimal(r6.Cells["ItemPrice"].Value);
				detail.RateAddLess = Conversion.ToDecimal(r6.Cells["AddLessRate"].Value);
				detail.RateCut = detail.RateAddLess;
				if (Conversion.ToDecimal(r6.Cells["ItemAmount"].Value) == 0m)
				{
					throw new Exception("ItemAmount Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemAmount = Math.Round(Conversion.ToDecimal(r6.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if ((Conversion.ToDecimal(r6.Cells["DiscRate"].Value) > 0m || Conversion.ToDecimal(r6.Cells["DiscAmount"].Value) > 0m) && Conversion.ToInt(r6.Cells["DiscountTypeId"].Value) == 0)
				{
					throw new Exception("DiscountType Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemDiscountTypeId = Conversion.ToInt(r6.Cells["DiscountTypeId"].Value);
				detail.ItemDiscountType = Conversion.ToString((object)r6.Cells["DiscountType"].Text);
				if ((Conversion.ToDecimal(r6.Cells["DiscRate"].Value) == 0m || Conversion.ToDecimal(r6.Cells["DiscAmount"].Value) == 0m) && Conversion.ToInt(r6.Cells["DiscountTypeId"].Value) > 0)
				{
					throw new Exception("Discount Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemDiscountRate = Conversion.ToDecimal(r6.Cells["DiscRate"].Value);
				detail.ItemDiscountAmount = Conversion.ToDecimal(r6.Cells["DiscAmount"].Value);
				if (Conversion.ToDecimal(r6.Cells["ItemAmountWithDisc"].Value) == 0m)
				{
					throw new Exception("ItemAmountWithDisc Required in Detail Grid And row No: " + (r6.RowIndex + 1));
				}
				detail.ItemAmountWithDisc = Math.Round(Conversion.ToDecimal(r6.Cells["ItemAmountWithDisc"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				detail.FcyAmount = Conversion.ToDecimal(r6.Cells["FcyAmount"].Value);
				detail.GpDate = Conversion.ToDateTime(r6.Cells["GpDate"].Value);
				detail.GpNo = Conversion.ToInt(r6.Cells["GpNo"].Value);
				detail.VehicleNo = Conversion.ToString(r6.Cells["VehicleNo"].Value);
				detail.CityId = Conversion.ToInt(r6.Cells["CityId"].Value);
				detail.ExpenseAmount = Conversion.ToDecimal(r6.Cells["Expense"].Value);
				detail.CommissionAmount = Conversion.ToDecimal(r6.Cells["Commission"].Value);
				detail.FreightAmount = Conversion.ToDecimal(r6.Cells["Freight"].Value);
				detail.JournalAmount = Conversion.ToDecimal(r6.Cells["SettleDisc"].Value);
				detail.WagesAmount = Conversion.ToDecimal(r6.Cells["Wages"].Value);
				detail.ItemNetAmount = Math.Round(Conversion.ToDecimal(r6.Cells["ItemNetAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				detail.RemarksDetail = "";
				detail.IsFOC = Conversion.ToBool(r6.Cells["IsFOC"].Value);
				obj.InvSaleInvoiceDetaillist.Add(detail);
			}
			if (obj.InvSaleInvoiceDetaillist.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					obj.InvSaleInvoiceDetaillist.Add(lstRemoveDetailRecord[j]);
				}
			}
			if (!FOCExist)
			{
				throw new Exception("Not all records can be FreeOfCost in Detail Grid: ");
			}
			rows = grdChargeToProduct.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0 || Conversion.ToDecimal((object)r4.Cells["Amount"].Value.ToString()) != 0m)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.Id = Conversion.ToInt(r4.Cells["Id"].Value);
					if (Conversion.ToInt(r4.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("Item Required in Charge to Product Grid And row No: " + (r4.RowIndex + 1));
					}
					PE.InvOtherItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r4.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDecimal(r4.Cells["Rate"].Value);
					if (Conversion.ToDecimal(r4.Cells["Amount"].Value) == 0m)
					{
						throw new Exception("Amount Required in Charge to Product Grid And row No: " + (r4.RowIndex + 1));
					}
					PE.Amount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					obj.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["AccountId"].Value) != 0 || Conversion.ToInt(r2.Cells["GlAccountId"].Value) != 0 || Conversion.ToInt(r2.Cells["Credit"].Value) != 0 || Conversion.ToInt(r2.Cells["Debit"].Value) != 0)
				{
					InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
					if (Conversion.ToInt(r2.Cells["AccountId"].Value) == 0 && Conversion.ToInt(r2.Cells["GlAccountId"].Value) == 0)
					{
						throw new Exception("Account Required in Party AddLess Grid And row No: " + (r2.RowIndex + 1));
					}
					PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
					PJ.TransporterSupCustId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					PJ.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDecimal(r2.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDecimal(r2.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDecimal(r2.Cells["Credit"].Value);
					if (PJ.JvDebit == 0m && PJ.JvCredit == 0m)
					{
						((Control)(object)grdPartyAddLs).Focus();
						throw new Exception("Credit or Debit is Required in Party AddLess Grid And row No: " + (r2.RowIndex + 1));
					}
					if ((PJ.JvDebit > 0m || PJ.JvCredit > 0m) && PJ.JvRemarks == string.Empty)
					{
						((Control)(object)grdPartyAddLs).Focus();
						throw new Exception("Remarks Field is Required in Party AddLess Grid And row No: " + (r2.RowIndex + 1));
					}
					obj.InvSaleInvoiceJournalList.Add(PJ);
				}
			}
			rows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0 || Conversion.ToInt(r.Cells["ContractorId"].Value) > 0 || Conversion.ToInt(r.Cells["ItemId"].Value) > 0 || Conversion.ToInt(r.Cells["AttributeVarientId"].Value) > 0)
				{
					InvSaleInvoiceWagesDetail d = new InvSaleInvoiceWagesDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.InvGdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
					d.InvGdnWagesId = Conversion.ToInt(r.Cells["GdnWagesId"].Value);
					d.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					d.SaleOrderWagesId = Conversion.ToInt(r.Cells["OrderWagesId"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (r.Cells["AttributeVarientId"].Text == string.Empty || Conversion.ToInt(r.Cells["AttributeVarientId"].Value) == 0)
					{
						MessageBox.Show("AttributeVarient  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					d.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ParentUomId = Conversion.ToInt((object)r.Cells["ParentUomId"].Text);
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.ItemNetWeight = Conversion.ToDecimal((object)r.Cells["ItemNetWeight"].Text);
					d.AddLess = Conversion.ToDecimal((object)r.Cells["AddLess"].Text);
					d.NetAmount = Conversion.ToDecimal((object)r.Cells["NetAmount"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					if (Conversion.ToString((object)r.Cells["Remarks"].Text) != string.Empty)
					{
						d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					}
					else
					{
						d.Remarks = "Activity: " + Conversion.ToString((object)r.Cells["ServiceActivity"].Text) + " Rate: " + Conversion.ToString((object)r.Cells["Rate"].Text) + " Amount: " + Conversion.ToString((object)r.Cells["Amount"].Text) + " AddLess: " + Conversion.ToString((object)r.Cells["AddLess"].Text) + " NetAmount: " + Conversion.ToString((object)r.Cells["NetAmount"].Text);
					}
					obj.InvSaleInvoiceWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int sucess = InvSaleInvoice.Save(obj);
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
			_ = WagesStatus;
			Reset();
			if (ChkBok.Checked)
			{
				Id = sucess;
				VoucherHeadIdGet();
				Id = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1856);
			}
			if (ChkCustomerPrintslip.Checked)
			{
				CommonServices.SaleInvoiceDirectConcreteCustomerSlip(sucess);
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
			Id = Conversion.ToInt((object)ID);
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			InvSaleInvoice invPI = InvSaleInvoice.GetByID(Id);
			if (invPI == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			DocDate.Value = Conversion.ToDateTime((object)invPI.DocDate);
			txtdocno.Text = Conversion.ToString((object)invPI.DocNo);
			txtbillno.Text = Conversion.ToString((object)invPI.ManualBillNo);
			txtSupplierReference.Text = Conversion.ToString((object)invPI.RefrenenceNo);
			comsupplier.Value = Conversion.ToInt((object)invPI.SupplierCustomerId);
			CmbVisitedbyName.Value = Conversion.ToInt((object)invPI.VisitedById);
			txtremarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
			combpttrm.Value = Conversion.ToInt((object)invPI.PaymentTermsId);
			duedate.Value = Conversion.ToDateTime((object)invPI.DueDate);
			txtduedays.Text = Conversion.ToString((object)invPI.DueDays);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)invPI.DeliveryTerm);
			txtDeliveryStartDate.Value = Conversion.ToDateTime((object)invPI.DeliveryStartDate);
			txtDeliverydays.Text = Conversion.ToString((object)invPI.DeliveryDays);
			txtDistance.Text = Conversion.ToString((object)invPI.Distance);
			txtRefPartyName.Text = Conversion.ToString((object)invPI.ReferencPartyName);
			txtRefPartyAddress.Text = Conversion.ToString((object)invPI.ReferencPartyAddress);
			txtRefPartyCellNo.Text = Conversion.ToString((object)invPI.ReferencPartyCellNo);
			combcommAgent.Value = Conversion.ToInt((object)invPI.CommissionAgentId);
			CmbRefSalesMan.Value = Conversion.ToInt((object)invPI.RefSalesManId);
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = Conversion.ToDecimal((object)invPI.CommissionRate).ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)combcommUOM).Text = Conversion.ToString((object)invPI.CommissionUom);
			txtcommamount.Text = Conversion.ToDecimal((object)invPI.CommissionAmount).ToString(clsGlobalVariables.stringFormatsingle);
			txtCommissionRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			txtInvoiceQty.Text = Conversion.ToDecimal((object)invPI.InvoiceQty).ToString("##,#.####");
			txtInvoiceWeight.Text = Conversion.ToDecimal((object)invPI.InvoiceWeight).ToString("##,#.####");
			txtExchangeRate.Text = Conversion.ToDecimal((object)invPI.ExchangeRate).ToString(clsGlobalVariables.DecimalFCYRateFormate);
			cmbCurrency.Value = Conversion.ToInt((object)invPI.CurrencyId);
			txtFcyAmount.Text = Conversion.ToDecimal((object)invPI.FcyAmount).ToString(clsGlobalVariables.stringFormatsingleForFcy);
			CmbTransporterAc.Value = Conversion.ToInt((object)invPI.TransporterId);
			CmbOtherWagesAccount.Value = Conversion.ToInt((object)invPI.OtherWagesAccountId);
			cmbSettlementDiscountAccount.Value = Conversion.ToInt((object)invPI.SettlementDiscountAccountId);
			CmbCommissionDebitAccount.Value = Conversion.ToInt((object)invPI.CommissionDebitAcId);
			txtItemAmountHeader.Text = Conversion.ToDecimal((object)invPI.ItemAmountHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtDiscountHeader.Text = Conversion.ToDecimal((object)invPI.DiscountAmountHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtItemNetAmountHeader.Text = Conversion.ToDecimal((object)invPI.ItemNetAmountHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtOtherWagesHeader.Text = Conversion.ToDecimal((object)invPI.OtherWagesHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtFreightAmountHeader.Text = Conversion.ToDecimal((object)invPI.FrieghtAmountHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtSettlementDiscountHeader.Text = Conversion.ToDecimal((object)invPI.SettlementDiscountHeader).ToString(clsGlobalVariables.stringFormatsingle);
			txtBillAmountHeader.Text = Conversion.ToDecimal((object)invPI.BillAmount).ToString(clsGlobalVariables.stringFormatsingle);
			txtManualTotalBillAmount.Text = Conversion.ToDecimal((object)invPI.BillAmount).ToString(clsGlobalVariables.stringFormatsingle);
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
			for (int l = 0; l < invPI.InvSaleInvoiceDetaillist.Count; l++)
			{
				dtGrid.Rows.Add(invPI.InvSaleInvoiceDetaillist[l].Id, invPI.InvSaleInvoiceDetaillist[l].InvGdnId, invPI.InvSaleInvoiceDetaillist[l].InvGdnDetailId, invPI.InvSaleInvoiceDetaillist[l].GdnNo, invPI.InvSaleInvoiceDetaillist[l].SaleOrderId, invPI.InvSaleInvoiceDetaillist[l].SaleOrderDetailId, invPI.InvSaleInvoiceDetaillist[l].SaleOrderNo, invPI.InvSaleInvoiceDetaillist[l].WarehouseId, invPI.InvSaleInvoiceDetaillist[l].WareHouseName, invPI.InvSaleInvoiceDetaillist[l].ItemId, invPI.InvSaleInvoiceDetaillist[l].ItemName, invPI.InvSaleInvoiceDetaillist[l].ItemCode, invPI.InvSaleInvoiceDetaillist[l].ScheduleId, invPI.InvSaleInvoiceDetaillist[l].ItemUomId, invPI.InvSaleInvoiceDetaillist[l].PackUom, Conversion.ToDecimal((object)invPI.InvSaleInvoiceDetaillist[l].PackEquivalent), invPI.InvSaleInvoiceDetaillist[l].ItemAttributeVarientId, invPI.InvSaleInvoiceDetaillist[l].ItemAttributeVarient, invPI.InvSaleInvoiceDetaillist[l].VarientEquivalent, invPI.InvSaleInvoiceDetaillist[l].JobLotId, invPI.InvSaleInvoiceDetaillist[l].JobLotDescription, invPI.InvSaleInvoiceDetaillist[l].ItemQty, invPI.InvSaleInvoiceDetaillist[l].ItemQty, invPI.InvSaleInvoiceDetaillist[l].ItemWeight, invPI.InvSaleInvoiceDetaillist[l].ItemNetWeight, invPI.InvSaleInvoiceDetaillist[l].ItemNetWeight, invPI.InvSaleInvoiceDetaillist[l].ItemRateWithOutAddLess, invPI.InvSaleInvoiceDetaillist[l].RateAddLess, invPI.InvSaleInvoiceDetaillist[l].ItemRate, invPI.InvSaleInvoiceDetaillist[l].ItemAmount, invPI.InvSaleInvoiceDetaillist[l].ItemDiscountTypeId, invPI.InvSaleInvoiceDetaillist[l].ItemDiscountType, invPI.InvSaleInvoiceDetaillist[l].ItemDiscountRate, invPI.InvSaleInvoiceDetaillist[l].ItemDiscountAmount, invPI.InvSaleInvoiceDetaillist[l].ItemAmountWithDisc, invPI.InvSaleInvoiceDetaillist[l].FcyAmount, invPI.InvSaleInvoiceDetaillist[l].GpDate, invPI.InvSaleInvoiceDetaillist[l].GpNo, invPI.InvSaleInvoiceDetaillist[l].VehicleNo, invPI.InvSaleInvoiceDetaillist[l].CityId, invPI.InvSaleInvoiceDetaillist[l].CityName, invPI.InvSaleInvoiceDetaillist[l].ExpenseAmount, invPI.InvSaleInvoiceDetaillist[l].JournalAmount, invPI.InvSaleInvoiceDetaillist[l].CommissionAmount, invPI.InvSaleInvoiceDetaillist[l].FreightAmount, invPI.InvSaleInvoiceDetaillist[l].WagesAmount, invPI.InvSaleInvoiceDetaillist[l].ItemNetAmount, invPI.InvSaleInvoiceDetaillist[l].IsFOC, invPI.InvSaleInvoiceDetaillist[l].ItemWithVarient);
				SaleOrderExist = invPI.InvSaleInvoiceDetaillist[l].SaleOrderId > 0;
				GdnExist = invPI.InvSaleInvoiceDetaillist[l].InvGdnId > 0;
				((Control)(object)comsupplier).Enabled = false;
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invPI.InvSaleInvoiceExpenseList.Count; k++)
			{
				dtInvExp.Rows.Add(invPI.InvSaleInvoiceExpenseList[k].Id, invPI.InvSaleInvoiceExpenseList[k].InvOtherItemId, invPI.InvSaleInvoiceExpenseList[k].OtherItemName, invPI.InvSaleInvoiceExpenseList[k].Qty, invPI.InvSaleInvoiceExpenseList[k].Rate, invPI.InvSaleInvoiceExpenseList[k].Amount, invPI.InvSaleInvoiceExpenseList[k].Remarks);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invPI.InvSaleInvoiceJournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(invPI.InvSaleInvoiceJournalList[j].TransporterSupCustId, invPI.InvSaleInvoiceJournalList[j].ChartofAccountId, (invPI.InvSaleInvoiceJournalList[j].TransporterSupCustId > 0) ? invPI.InvSaleInvoiceJournalList[j].SupplierCustomer : ((invPI.InvSaleInvoiceJournalList[j].ChartofAccountId > 0) ? invPI.InvSaleInvoiceJournalList[j].AccountTitle : ""), invPI.InvSaleInvoiceJournalList[j].JvRemarks, invPI.InvSaleInvoiceJournalList[j].JvPrcnt, invPI.InvSaleInvoiceJournalList[j].JvQty, invPI.InvSaleInvoiceJournalList[j].JvRate, invPI.InvSaleInvoiceJournalList[j].JvDebit, invPI.InvSaleInvoiceJournalList[j].JvCredit);
			}
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			if (invPI.InvSaleInvoiceWagesDetailList.Count > 0)
			{
				dtWagesDetail.Clear();
				for (int i = 0; i < invPI.InvSaleInvoiceWagesDetailList.Count; i++)
				{
					dtWagesDetail.Rows.Add(invPI.InvSaleInvoiceWagesDetailList[i].Id, invPI.InvSaleInvoiceWagesDetailList[i].InvGdnId, invPI.InvSaleInvoiceWagesDetailList[i].InvGdnWagesId, invPI.InvSaleInvoiceWagesDetailList[i].SaleOrderId, invPI.InvSaleInvoiceWagesDetailList[i].SaleOrderWagesId, invPI.InvSaleInvoiceWagesDetailList[i].ItemId, invPI.InvSaleInvoiceWagesDetailList[i].ItemName, invPI.InvSaleInvoiceWagesDetailList[i].ItemAttributeVarientId, invPI.InvSaleInvoiceWagesDetailList[i].ItemAttributeVarient, invPI.InvSaleInvoiceWagesDetailList[i].VarientEquivalent, invPI.InvSaleInvoiceWagesDetailList[i].ContractorId, invPI.InvSaleInvoiceWagesDetailList[i].ContractorName, invPI.InvSaleInvoiceWagesDetailList[i].ContractorWagesRateScheduleId, invPI.InvSaleInvoiceWagesDetailList[i].ServiceActivity, invPI.InvSaleInvoiceWagesDetailList[i].ParentUomId, invPI.InvSaleInvoiceWagesDetailList[i].Qty, invPI.InvSaleInvoiceWagesDetailList[i].ItemNetWeight, invPI.InvSaleInvoiceWagesDetailList[i].Rate, invPI.InvSaleInvoiceWagesDetailList[i].Amount, invPI.InvSaleInvoiceWagesDetailList[i].AddLess, invPI.InvSaleInvoiceWagesDetailList[i].NetAmount, invPI.InvSaleInvoiceWagesDetailList[i].Remarks);
				}
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			grdChargeToProduct.DataSource = dtInvExp;
			grdChargeToProduct.RetrieveStructure();
			grdInvExpSettings();
			grdPartyAddLs.DataSource = dtGrdGL;
			grdPartyAddLs.RetrieveStructure();
			gridGLSettings();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdContractorWages.DataSource = dtWagesDetail;
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			List<DMSAttachments> byID = DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
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
			FillWagesGridFilterCombo();
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
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvSaleInvoice.DeleteByID(UserAccount.OrganizationId, UserAccount.CompanyId, 1856, UserAccount.ID, Id);
				MessageBox.Show("Delete Record Successfully");
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

	private void HistoryComboBind()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		try
		{
			dtCustomerHistory.Rows.Clear();
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			int IdC = Conversion.ToInt(CmbCommissionAgentHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1856"
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
			DataTable dt = InvSaleInvoice.DropDownFillFromInvSaleInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomerHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"], dt.Rows[i]["OtherDescription"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CommissionAgent")
					{
						dtCommissionAgent.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
				if (dtCustomerHistory.Rows.Count > 0)
				{
					if (RdCustomerHistorySearchByName.Checked)
					{
						DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
					}
					else
					{
						DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
					}
					if (Id > 0)
					{
						if (dtCustomerHistory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
					((Control)(object)CmbCustomerHistory).Text = string.Empty;
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
					((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
					((UltraGridBase)CmbCommissionAgentHistory).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbCustomerHistory).Text = string.Empty;
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
				((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
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
			PI.DocumentTypeId = 1856;
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
			DataTable dthistory = InvSaleInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtSales = new DataTable();
				dtSales.Columns.Add("Id", typeof(int));
				dtSales.Columns.Add("DocumentTypeId", typeof(int));
				dtSales.Columns.Add("NoOfAttachments", typeof(int));
				dtSales.Columns.Add("DocNo", typeof(string));
				dtSales.Columns.Add("DocDate", typeof(DateTime));
				dtSales.Columns.Add("SupplierCustomerId", typeof(int));
				dtSales.Columns.Add("PartyName", typeof(string));
				dtSales.Columns.Add("PartyCode", typeof(string));
				dtSales.Columns.Add("RefrenenceNo", typeof(string));
				dtSales.Columns.Add("Distance", typeof(string));
				dtSales.Columns.Add("ManualBillNo", typeof(string));
				dtSales.Columns.Add("VisitedByName", typeof(string));
				dtSales.Columns.Add("ReferencPartyName", typeof(string));
				dtSales.Columns.Add("ReferencPartyAddress", typeof(string));
				dtSales.Columns.Add("ReferencPartyCellNo", typeof(string));
				dtSales.Columns.Add("CommissionAgent", typeof(string));
				dtSales.Columns.Add("RefSalesMan", typeof(string));
				dtSales.Columns.Add("CommissionAmount", typeof(decimal));
				dtSales.Columns.Add("PaymentTerm", typeof(string));
				dtSales.Columns.Add("DueDays", typeof(int));
				dtSales.Columns.Add("DeliveryTerm", typeof(string));
				dtSales.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dtSales.Columns.Add("IsApproved", typeof(bool));
				dtSales.Columns.Add("EntryDate", typeof(DateTime));
				dtSales.Columns.Add("EntryUserName", typeof(string));
				dtSales.Columns.Add("ModifyDate", typeof(DateTime));
				dtSales.Columns.Add("ModifyUserName", typeof(string));
				dtSales.Columns.Add("ApprovedDate", typeof(DateTime));
				dtSales.Columns.Add("ApprovedUserName", typeof(string));
				dtSales.Columns.Add("InvoiceQty", typeof(int));
				dtSales.Columns.Add("InvoiceWeight", typeof(decimal));
				dtSales.Columns.Add("TransporterName", typeof(string));
				dtSales.Columns.Add("FrieghtAmountHeader", typeof(decimal));
				dtSales.Columns.Add("OtherWagesAccount", typeof(string));
				dtSales.Columns.Add("OtherWagesHeader", typeof(decimal));
				dtSales.Columns.Add("BillAmount", typeof(decimal));
				dtSales.Rows.Clear();
				foreach (DataRow row in dthistory.Rows)
				{
					dtSales.Rows.Add(row["Id"], row["DocumentTypeId"], row["NoOfAttachments"], row["DocNo"], row["DocDate"], row["SupplierCustomerId"], row["CustomerName"], row["PartyCode"], row["RefrenenceNo"], row["Distance"], row["ManualBillNo"], row["VisitedByName"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["CommissionAgent"], row["RefSalesMan"], row["CommissionAmount"], row["PaymentTerm"], row["DueDays"], row["DeliveryTerm"], row["DeliveryStartDate"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["InvoiceQty"], row["InvoiceWeight"], row["TransporterName"], row["FrieghtAmountHeader"], row["OtherWagesAccount"], row["OtherWagesHeader"], row["BillAmount"]);
				}
				grdHistory.DataSource = dtSales;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["InvoiceQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["InvoiceQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["InvoiceQty"].FormatString = "##,##.##";
			grdHistory.RootTable.Columns["InvoiceQty"].TotalFormatString = "##,##.##";
			grdHistory.RootTable.Columns["InvoiceWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["InvoiceWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["InvoiceWeight"].FormatString = "##,##.##";
			grdHistory.RootTable.Columns["InvoiceWeight"].TotalFormatString = "##,##.##";
			grdHistory.RootTable.Columns["FrieghtAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FrieghtAmountHeader"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["FrieghtAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["FrieghtAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["OtherWagesHeader"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OtherWagesHeader"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OtherWagesHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["OtherWagesHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Customer Slip";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Customer Slip";
			grdHistory.RootTable.Columns["Print"].Width = 110;
			grdHistory.RootTable.Columns.Add("ItemSlip");
			grdHistory.RootTable.Columns["ItemSlip"].Caption = "Item Slip";
			grdHistory.RootTable.Columns["ItemSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonText = "Item Slip";
			grdHistory.RootTable.Columns["ItemSlip"].Width = 90;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Voucher"].Position = 1;
			grdHistory.RootTable.Columns["Print"].Position = 2;
			grdHistory.RootTable.Columns["ItemSlip"].Position = 3;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbDateTypeHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
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
		try
		{
			int RId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = RId;
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.SaleInvoiceDirectConcreteCustomerSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "ItemSlip")
			{
				CommonServices.SaleInvoiceDirectConcreteItemSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(RId, 1856)), 1856);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvSaleInvoice invPI = new InvSaleInvoice();
			invPI = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (invPI.InvSaleInvoiceDetaillist.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("Id", typeof(int));
				dtdetail.Columns.Add("GdnId", typeof(int));
				dtdetail.Columns.Add("GdnNo", typeof(int));
				dtdetail.Columns.Add("OrderId", typeof(int));
				dtdetail.Columns.Add("OrderNo", typeof(int));
				dtdetail.Columns.Add("Warehouse", typeof(string));
				dtdetail.Columns.Add("ItemId", typeof(int));
				dtdetail.Columns.Add("ItemName", typeof(string));
				dtdetail.Columns.Add("ItemCode", typeof(string));
				dtdetail.Columns.Add("AttributeVarient", typeof(string));
				dtdetail.Columns.Add("VarientUnit", typeof(decimal));
				dtdetail.Columns.Add("JobLot", typeof(string));
				dtdetail.Columns.Add("ItemQty", typeof(decimal));
				dtdetail.Columns.Add("ItemWeight", typeof(decimal));
				dtdetail.Columns.Add("NetWeight", typeof(decimal));
				dtdetail.Columns.Add("ItemPrice", typeof(decimal));
				dtdetail.Columns.Add("AddLessRate", typeof(decimal));
				dtdetail.Columns.Add("Rate", typeof(decimal));
				dtdetail.Columns.Add("ItemAmount", typeof(decimal));
				dtdetail.Columns.Add("DiscountType", typeof(string));
				dtdetail.Columns.Add("DiscRate", typeof(decimal));
				dtdetail.Columns.Add("DiscAmount", typeof(decimal));
				dtdetail.Columns.Add("ItemAmountWithDisc", typeof(decimal));
				dtdetail.Columns.Add("FcyAmount", typeof(decimal));
				dtdetail.Columns.Add("GpDate", typeof(DateTime));
				dtdetail.Columns.Add("GpNo", typeof(int));
				dtdetail.Columns.Add("VehicleNo");
				dtdetail.Columns.Add("CityName");
				dtdetail.Columns.Add("Expense", typeof(decimal));
				dtdetail.Columns.Add("Journal", typeof(decimal));
				dtdetail.Columns.Add("Commission", typeof(decimal));
				dtdetail.Columns.Add("Freight", typeof(decimal));
				dtdetail.Columns.Add("Wages", typeof(decimal));
				dtdetail.Columns.Add("ItemNetAmount", typeof(decimal));
				dtdetail.Columns.Add("IsFOC", typeof(bool));
				dtdetail.Rows.Clear();
				for (int i = 0; i < invPI.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(invPI.InvSaleInvoiceDetaillist[i].Id, invPI.InvSaleInvoiceDetaillist[i].InvGdnId, invPI.InvSaleInvoiceDetaillist[i].GdnNo, invPI.InvSaleInvoiceDetaillist[i].SaleOrderId, invPI.InvSaleInvoiceDetaillist[i].SaleOrderNo, invPI.InvSaleInvoiceDetaillist[i].WareHouseName, invPI.InvSaleInvoiceDetaillist[i].ItemId, invPI.InvSaleInvoiceDetaillist[i].ItemName, invPI.InvSaleInvoiceDetaillist[i].ItemCode, invPI.InvSaleInvoiceDetaillist[i].ItemAttributeVarient, invPI.InvSaleInvoiceDetaillist[i].VarientEquivalent, invPI.InvSaleInvoiceDetaillist[i].JobLotDescription, invPI.InvSaleInvoiceDetaillist[i].ItemQty, invPI.InvSaleInvoiceDetaillist[i].ItemWeight, invPI.InvSaleInvoiceDetaillist[i].ItemNetWeight, invPI.InvSaleInvoiceDetaillist[i].ItemRateWithOutAddLess, invPI.InvSaleInvoiceDetaillist[i].RateAddLess, invPI.InvSaleInvoiceDetaillist[i].ItemRate, invPI.InvSaleInvoiceDetaillist[i].ItemAmount, invPI.InvSaleInvoiceDetaillist[i].ItemDiscountType, invPI.InvSaleInvoiceDetaillist[i].ItemDiscountRate, invPI.InvSaleInvoiceDetaillist[i].ItemDiscountAmount, invPI.InvSaleInvoiceDetaillist[i].ItemAmountWithDisc, invPI.InvSaleInvoiceDetaillist[i].FcyAmount, invPI.InvSaleInvoiceDetaillist[i].GpDate, invPI.InvSaleInvoiceDetaillist[i].GpNo, invPI.InvSaleInvoiceDetaillist[i].VehicleNo, invPI.InvSaleInvoiceDetaillist[i].CityName, invPI.InvSaleInvoiceDetaillist[i].ExpenseAmount, invPI.InvSaleInvoiceDetaillist[i].JournalAmount, invPI.InvSaleInvoiceDetaillist[i].CommissionAmount, invPI.InvSaleInvoiceDetaillist[i].FreightAmount, invPI.InvSaleInvoiceDetaillist[i].WagesAmount, invPI.InvSaleInvoiceDetaillist[i].ItemNetAmount, invPI.InvSaleInvoiceDetaillist[i].IsFOC);
				}
				grdDetailHistory.DataSource = dtdetail;
				grdDetailHistory.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Expected O, but got Unknown
		try
		{
			grdDetailHistory.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetailHistory.RootTable.Columns["Id"].Visible = false;
			grdDetailHistory.RootTable.Columns["GdnId"].Visible = false;
			grdDetailHistory.RootTable.Columns["OrderId"].Visible = false;
			grdDetailHistory.RootTable.Columns["ItemId"].Visible = false;
			grdDetailHistory.RootTable.Columns["Rate"].Caption = "NetRate";
			if (Conversion.ToInt(grdDetailHistory.CurrentRow.Cells["OrderId"].Value) == 0)
			{
				grdDetailHistory.RootTable.Columns["OrderNo"].Visible = false;
			}
			if (Conversion.ToInt(grdDetailHistory.CurrentRow.Cells["GdnId"].Value) == 0)
			{
				grdDetailHistory.RootTable.Columns["GdnNo"].Visible = false;
			}
			grdDetailHistory.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["DiscRate"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["DiscRate"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["DiscRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["DiscRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemAmountWithDisc"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemAmountWithDisc"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemAmountWithDisc"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemAmountWithDisc"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetailHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetailHistory.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Wages"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["Wages"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Wages"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["Wages"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdDetailHistory.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grdDetailHistory.RootTable.FormatConditions.Add(fc);
			CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
			ctrlGrdBar3_Load(null, null);
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1856);
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
			CommonServices.SaleInvoiceDirectConcreteCustomerSlip(Id);
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
			CommonServices.SaleInvoiceDirectConcreteItemSlip(Id);
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

	private void CalculateDetailAmount()
	{
		try
		{
			decimal qty = default(decimal);
			decimal amount = default(decimal);
			decimal rate = default(decimal);
			decimal unit = default(decimal);
			decimal ItemWeight = default(decimal);
			decimal NetWeight = default(decimal);
			if (Conversion.ToDecimal((object)txtRate.Text) != 0m)
			{
				Conversion.ToDecimal((object)txtRate.Text);
			}
			qty = Conversion.ToDecimal((object)txtQty.Text);
			rate = Conversion.ToDecimal((object)txtRate.Text);
			unit = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			ItemWeight = Conversion.ToDecimal((object)txtItemWeight.Text);
			if (qty > 0m && Conversion.ToInt(comItem.Value) > 0)
			{
				if (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value))
				{
					unit = ((Conversion.ToInt(CmbPackUom.Value) <= 0) ? default(decimal) : Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value));
					NetWeight = unit * qty;
					txtNetWeight.Text = NetWeight.ToString("##,#.##");
					amount = Conversion.ToDecimal((object)(qty * rate));
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					txtAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
					txtTotalAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					NetWeight = unit * qty * ItemWeight;
					txtNetWeight.Text = NetWeight.ToString("##,#.##");
					amount = Conversion.ToDecimal((object)(unit * qty * rate));
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					txtAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
					txtTotalAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtAmount.Text = 0.ToString();
				txtTotalAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDiscountAndTotalAmount()
	{
		try
		{
			decimal ItemPercentage = Conversion.ToDecimal((object)txtDiscountRate.Text);
			decimal ItemAmount = Conversion.ToDecimal((object)txtAmount.Text);
			decimal ItemDiscount = Conversion.ToDecimal((object)txtDiscountAmount.Text);
			if (((Control)(object)CmbDiscountType).Text == "Percent")
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "DiscPercent")
				{
					if (ItemPercentage > 0m)
					{
						ItemDiscount = ItemAmount * ItemPercentage / 100m;
						ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtDiscountAmount.Text = "0";
					}
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "DiscAmount")
				{
					ItemPercentage = Math.Round(ItemDiscount * 100m / ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForRate, MidpointRounding.AwayFromZero);
					if (ItemPercentage > 99m)
					{
						ItemPercentage = 99m;
						txtDiscountRate.Text = ItemPercentage.ToString("#,##0.###");
						ItemDiscount = ItemAmount * ItemPercentage / 100m;
						ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
						return;
					}
					txtDiscountRate.Text = ItemPercentage.ToString("#,##0.###");
				}
				else
				{
					ItemDiscount = ItemAmount * ItemPercentage / 100m;
					ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else if (((Control)(object)CmbDiscountType).Text == "Flat")
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "DiscPercent")
				{
					ItemPercentage = Conversion.ToDecimal((object)txtDiscountRate.Text);
					txtDiscountAmount.Text = ItemPercentage.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "DiscAmount")
				{
					ItemDiscount = Conversion.ToDecimal((object)txtDiscountAmount.Text);
					txtDiscountRate.Text = ItemDiscount.ToString("#,##0.###");
				}
			}
			if (((Control)(object)CmbDiscountType).Text == "Percent" && Conversion.ToDecimal((object)txtDiscountRate.Text) > 99m)
			{
				MessageBox.Show("Discount Percentage Cannot greater than 99");
				decimal Percent = Conversion.ToDecimal((object)txtDiscountRate.Text);
				decimal i = Percent - 99m;
				txtDiscountRate.Text = (Percent - i).ToString(clsGlobalVariables.stringFormatsingle);
			}
			txtTotalAmount.Text = (ItemAmount - ItemDiscount).ToString();
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
			if (grd.GetRows().Count() > 0)
			{
				decimal BillAmount = default(decimal);
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				decimal ItemAmount = (from row in dtGrid.AsEnumerable()
					where row.Field<bool?>("IsFOC") == false || !row.Field<bool?>("IsFOC").HasValue
					select row).Sum((DataRow row) => row.Field<decimal>("ItemAmount"));
				decimal DiscountAmount = (from row in dtGrid.AsEnumerable()
					where row.Field<bool?>("IsFOC") == false || !row.Field<bool?>("IsFOC").HasValue
					select row).Sum((DataRow row) => row.Field<decimal>("DiscAmount"));
				decimal ItemAmountWithDisc = (from row in dtGrid.AsEnumerable()
					where row.Field<bool?>("IsFOC") == false || !row.Field<bool?>("IsFOC").HasValue
					select row).Sum((DataRow row) => row.Field<decimal>("ItemAmountWithDisc"));
				decimal ExpAmount = Conversion.ToDecimal((object)grdChargeToProduct.GetTotal(grdChargeToProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				decimal JournalDebit = default(decimal);
				decimal JournalCredit = default(decimal);
				decimal FrieghtHeader = default(decimal);
				GridEXRow[] rows = grdPartyAddLs.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0 || Conversion.ToInt((object)r.Cells["GlAccountId"].Value.ToString()) > 0)
					{
						JournalDebit += Conversion.ToDecimal((object)r.Cells["Debit"].Value.ToString());
						JournalCredit += Conversion.ToDecimal((object)r.Cells["Credit"].Value.ToString());
						SupplierGLDebitAmount = Conversion.ToDecimal((object)JournalDebit);
						SupplierGLCreditAmount = Conversion.ToDecimal((object)JournalCredit);
					}
				}
				ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				ExpAmount = Math.Round(ExpAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalDebit = Math.Round(JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalCredit = Math.Round(JournalCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				BillAmount = ItemAmountWithDisc + ExpAmount;
				BillAmount = BillAmount + JournalCredit - JournalDebit;
				txtItemAmountHeader.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtDiscountHeader.Text = DiscountAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtItemNetAmountHeader.Text = ItemAmountWithDisc.ToString(clsGlobalVariables.stringFormatsingle);
				FrieghtHeader = Conversion.ToDecimal((object)txtFreightAmountHeader.Text.Trim());
				if (Conversion.ToInt(CmbTransporterAc.Value) > 0)
				{
					DataTable dt3 = CommonServices.GetSupplierCustomerInfoByGLAccountId(Conversion.ToInt(CmbTransporterAc.Value));
					if (dt3.Rows.Count > 0)
					{
						int SupGlId3 = Conversion.ToInt(dt3.Rows[0][0]);
						if (Conversion.ToInt(comsupplier.Value) == SupGlId3)
						{
							BillAmount -= FrieghtHeader;
						}
						else
						{
							BillAmount += FrieghtHeader;
						}
					}
					else
					{
						BillAmount += FrieghtHeader;
					}
				}
				else
				{
					BillAmount += FrieghtHeader;
				}
				decimal OtherWagesHeader = Conversion.ToDecimal((object)txtOtherWagesHeader.Text.Trim());
				if (Conversion.ToInt(CmbOtherWagesAccount.Value) > 0)
				{
					DataTable dt2 = CommonServices.GetSupplierCustomerInfoByGLAccountId(Conversion.ToInt(CmbOtherWagesAccount.Value));
					if (dt2.Rows.Count > 0)
					{
						int SupGlId2 = Conversion.ToInt(dt2.Rows[0][0]);
						if (Conversion.ToInt(comsupplier.Value) == SupGlId2)
						{
							BillAmount -= OtherWagesHeader;
						}
						else
						{
							BillAmount += OtherWagesHeader;
						}
					}
					else
					{
						BillAmount += OtherWagesHeader;
					}
				}
				else
				{
					BillAmount += OtherWagesHeader;
				}
				txtBillamountWithoutCommission.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				if (((UltraGridBase)combcommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m)
				{
					if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
					{
						decimal CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
						BillAmount -= CommAmount;
						CommNetAmtHeader.Text = CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
						CommNetAmtHeader.Visible = true;
						label84.Visible = true;
					}
					else
					{
						CommNetAmtHeader.Text = 0.ToString();
						CommNetAmtHeader.Visible = false;
						label84.Visible = false;
					}
				}
				else
				{
					CommNetAmtHeader.Text = 0.ToString();
					CommNetAmtHeader.Visible = false;
					label84.Visible = false;
				}
				decimal SettlementDiscount = Conversion.ToDecimal((object)txtSettlementDiscountHeader.Text.Trim());
				if (Conversion.ToInt(cmbSettlementDiscountAccount.Value) > 0)
				{
					DataTable dt = CommonServices.GetSupplierCustomerInfoByGLAccountId(Conversion.ToInt(cmbSettlementDiscountAccount.Value));
					if (dt.Rows.Count > 0)
					{
						int SupGlId = Conversion.ToInt(dt.Rows[0][0]);
						if (Conversion.ToInt(comsupplier.Value) == SupGlId)
						{
							txtBillAmountHeader.Text = "0";
							throw new Exception("SettlementDiscountAccount can not be Equal to customer Account");
						}
						BillAmount -= SettlementDiscount;
					}
					else
					{
						BillAmount -= SettlementDiscount;
					}
				}
				else
				{
					BillAmount -= SettlementDiscount;
				}
				txtBillAmountHeader.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtInvoiceQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtInvoiceWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m && ItemAmountWithDisc > 0m)
				{
					txtFcyAmount.Text = (ItemAmountWithDisc / Conversion.ToDecimal((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
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
			if (dtSupplier.Rows.Count <= 0 || Conversion.ToInt(comsupplier.Value) <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(comsupplier.Value))
				{
					txtSupplierGLId.Text = dtSupplier.Rows[i]["GlAccountId"].ToString();
					break;
				}
			}
			SettlementDiscAccountAndCommissionDebitAccountFill();
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
		decimal ItemAmountWithDisc = Math.Round(Conversion.ToDecimal(r.Cells["ItemAmountWithDisc"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		decimal settleDiscount = Math.Round(Conversion.ToDecimal(r.Cells["SettleDisc"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		decimal Expense = Math.Round(Conversion.ToDecimal(r.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		if (Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CreditAmountInItemSaleGL")))
		{
			if (Conversion.ToInt(cmbSettlementDiscountAccount.Value) > 0)
			{
				r.Cells["ItemNetAmount"].Value = Math.Round(ItemAmountWithDisc, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			else
			{
				r.Cells["ItemNetAmount"].Value = Math.Round(ItemAmountWithDisc - settleDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
		}
		else if (Conversion.ToInt(cmbSettlementDiscountAccount.Value) > 0)
		{
			r.Cells["ItemNetAmount"].Value = Math.Round(ItemAmountWithDisc + Expense, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		}
		else
		{
			r.Cells["ItemNetAmount"].Value = Math.Round(ItemAmountWithDisc + Expense - settleDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		}
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
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)grdContractorWages.GetTotal(grdContractorWages.RootTable.Columns["NetAmount"], (AggregateFunction)2).ToString());
			decimal ItemQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				decimal Qty = Conversion.ToDecimal((object)obj.Cells["ItemQty"].Value.ToString());
				obj.Cells["Wages"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			GridItemNetAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
	}

	private void SettlementDiscountProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtSettlementDiscountHeader.Text);
			if (TotalAmount > 0m)
			{
				decimal ItemQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Qty = Conversion.ToDecimal(obj.Cells["ItemQty"].Value);
					obj.Cells["SettleDisc"].Value = Math.Round(TotalAmount / ItemQty * Qty, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["SettleDisc"].Value = 0;
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
				GridItemNetAmountCalculate();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSettlementDiscountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SettlementDiscountProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdjustDiscamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDiscountInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateDiscountTypeAndRateInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDiscountTypeAndRateInGrid()
	{
		try
		{
			if (Conversion.ToInt(CmbDiscountType.Value) == 0)
			{
				MessageBox.Show("DiscountType Required");
				CmbDiscountType.Focus();
			}
			else if (Conversion.ToDouble((object)txtDiscountRate.Text) == 0.0)
			{
				MessageBox.Show("Discount Rate Required");
				txtDiscountRate.Focus();
			}
			else if (grd.RowCount > 0)
			{
				int DiscId = Conversion.ToInt(CmbDiscountType.Value);
				string DiscType = Conversion.ToString((object)((Control)(object)CmbDiscountType).Text);
				double DisRate = Conversion.ToDouble((object)txtDiscountRate.Text);
				if (DiscId == 2 && DisRate >= 100.0)
				{
					MessageBox.Show("Discount Rate Cannot be greater than 99 when Discount Type is Percent");
					return;
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["DiscountTypeId"].Value = DiscId;
					r.Cells["DiscountType"].Value = DiscType;
					r.Cells["DiscRate"].Value = DisRate;
					double AmtWithoutDisc = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					double DisAmt = 0.0;
					switch (DiscId)
					{
					case 1:
						DisAmt = DisRate;
						r.Cells["DiscAmount"].Value = DisAmt;
						break;
					case 2:
						DisAmt = AmtWithoutDisc * DisRate / 100.0;
						r.Cells["DiscAmount"].Value = DisAmt;
						break;
					case 0:
						DisAmt = 0.0;
						r.Cells["DiscAmount"].Value = 0;
						break;
					}
					if (AmtWithoutDisc > 0.0 && DisAmt > 0.0)
					{
						r.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc - DisAmt;
					}
					else if (AmtWithoutDisc > 0.0)
					{
						r.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc;
					}
					else
					{
						r.Cells["ItemAmountWithDisc"].Value = 0;
					}
					r.EndEdit();
				}
				WagesItemFillFromDetail();
				TotalCommissionAmount();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			else
			{
				MessageBox.Show("Atlest one Record Required in Grid");
			}
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
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
			NetRateCalculation();
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDiscountType_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemPrice_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetRateCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLess_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetRateCalculation();
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
			if (Conversion.ToDouble((object)txtItemPrice.Text) > 0.0 || Conversion.ToDouble((object)txtRate.Text) > 0.0)
			{
				double ItemPrice = Conversion.ToDouble((object)txtItemPrice.Text);
				double AddLess = Conversion.ToDouble((object)txtAddLess.Text);
				double NetRate = Conversion.ToDouble((object)txtRate.Text);
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "NetRate")
				{
					txtAddLess.Text = (NetRate - ItemPrice).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "AddLess")
				{
					txtRate.Text = (ItemPrice + AddLess).ToString();
				}
				else
				{
					txtRate.Text = (ItemPrice + AddLess).ToString("#,##0.####");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDiscountRate.Text = Conversion.ToDecimal((object)txtDiscountRate.Text).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDiscountAmount.Text = Conversion.ToDecimal((object)txtDiscountAmount.Text).ToString("#,##0.###");
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
					decimal CommRate2 = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					txtcommamount.Text = Math.Round(CommRate2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				if (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage")
				{
					decimal CommRate = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					string total = txtBillamountWithoutCommission.Text;
					if (total != null)
					{
						decimal CommAmt = Conversion.ToDecimal((object)total.ToString()) * CommRate / 100m;
						txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDiableCommissionFields()
	{
		try
		{
			if (Conversion.ToInt(combcommAgent.Value) > 0)
			{
				((Control)(object)combCommType).Enabled = true;
				txtcommrate.Enabled = true;
				if (Conversion.ToInt(combCommType.Value) != 3)
				{
					((Control)(object)combcommUOM).Enabled = false;
				}
				else
				{
					((Control)(object)combcommUOM).Enabled = true;
				}
				txtcommamount.Enabled = true;
				combCommType.Focus();
			}
			else
			{
				((Control)(object)combCommType).Enabled = false;
				txtcommrate.Enabled = false;
				((Control)(object)combcommUOM).Enabled = false;
				txtcommamount.Enabled = false;
			}
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
			EnableDiableCommissionFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
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

	private void txtOtherWagesHeader_TextChanged(object sender, EventArgs e)
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
			UpdateWagesRateForAllRows();
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
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLess_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAdjustDiscamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDiscountAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherWagesHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnLoadOrder_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && Conversion.ToInt(item.Cells["OrderId"].Value) == 0)
				{
					MessageBox.Show("You cannot load Order because a record exists without Order in the grid");
					return;
				}
			}
			LoadTransactions = new LoadSaleOrderConcrete(UserAccount);
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
			for (int i = 0; i < LoadTransactions.dtLoader.Rows.Count; i++)
			{
				if (Conversion.ToInt(LoadTransactions.dtLoader.Rows[i]["Id"]) > 0)
				{
					OrderIds = Conversion.ToInt(LoadTransactions.dtLoader.Rows[i]["Id"]).ToString();
					break;
				}
			}
			comsupplier.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["SupplierCustomerId"].ToString());
			txtDistance.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["Distance"].ToString());
			txtSupplierReference.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["RefrenenceNo"].ToString());
			CmbVisitedbyName.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["VisitedById"].ToString());
			combpttrm.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["PaymentTermsId"].ToString());
			txtduedays.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["OrderDueDays"].ToString());
			duedate.Value = Conversion.ToDateTime((object)LoadTransactions.dtLoader.Rows[0]["OrderDueDate"].ToString());
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["DeliveryTerm"].ToString());
			txtDeliveryStartDate.Value = Conversion.ToDateTime((object)LoadTransactions.dtLoader.Rows[0]["DeliveryStartDate"].ToString());
			txtDeliverydays.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["DeliveryDays"].ToString());
			txtRefPartyName.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["ReferencPartyName"].ToString());
			txtRefPartyAddress.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["ReferencPartyAddress"].ToString());
			txtRefPartyCellNo.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["ReferencPartyCellNo"].ToString());
			txtremarks.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["RemarksHeader"].ToString());
			combcommAgent.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["CommissionAgentId"].ToString());
			CmbRefSalesMan.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["RefSalesManId"].ToString());
			((Control)(object)combCommType).Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["CommissionType"].ToString());
			txtcommrate.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["CommissionRate"].ToString());
			txtcommamount.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["CommissionAmount"].ToString());
			cmbCurrency.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["CurrencyId"].ToString());
			txtExchangeRate.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["ExchangeRate"].ToString());
			CmbOtherWagesAccount.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["OtherWagesAccountId"].ToString());
			txtOtherWagesHeader.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["OtherWages"].ToString());
			txtInvoiceQty.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["OrderQty"].ToString());
			txtInvoiceWeight.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["OrderWeight"].ToString());
			CmbTransporterAc.Value = Conversion.ToInt((object)LoadTransactions.dtLoader.Rows[0]["TransporterId"].ToString());
			txtFreightAmountHeader.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["FrieghtAmount"].ToString());
			txtSettlementDiscountHeader.Text = Conversion.ToString((object)LoadTransactions.dtLoader.Rows[0]["AdjustDiscountAmount"].ToString());
			((Control)(object)comsupplier).Enabled = false;
			LoadDataDetailfromPurchaseInvoivce(dtpurchaseinvoice);
			LoadExpData(OrderIds);
			GridItemNetAmountCalculate();
			TotalCommissionAmount();
			OtherItemsProportion();
			FreightHeaderProportion();
			SettlementDiscountProportion();
			CommissionProportion();
			WagesProportion();
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
			SaleOrderExist = true;
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
					dtGrid.Rows.Add(0, 0, 0, 0, dtpurchaseinvoice.Rows[i]["Id"], dtpurchaseinvoice.Rows[i]["DetailId"], dtpurchaseinvoice.Rows[i]["DocNo"], 0, "", dtpurchaseinvoice.Rows[i]["OrderItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemCode"], dtpurchaseinvoice.Rows[i]["ScheduleId"], 0, "", 0, dtpurchaseinvoice.Rows[i]["ItemAttributeVarientId"], dtpurchaseinvoice.Rows[i]["ItemArrtibuteVarient"], dtpurchaseinvoice.Rows[i]["VarientEquivalent"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotDescription"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["ItemWeight"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["ItemRateWithOutAddLess"], dtpurchaseinvoice.Rows[i]["RateAddLess"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["ItemAmount"], dtpurchaseinvoice.Rows[i]["ItemDiscountTypeId"], dtpurchaseinvoice.Rows[i]["ItemDiscountType"], dtpurchaseinvoice.Rows[i]["ItemDiscountRate"], dtpurchaseinvoice.Rows[i]["ItemDiscountAmount"], dtpurchaseinvoice.Rows[i]["BalanceAmount"], dtpurchaseinvoice.Rows[i]["BalanceAmount"], DateTime.Now, 0, "", dtpurchaseinvoice.Rows[i]["CityId"], dtpurchaseinvoice.Rows[i]["CityName"], 0, 0, 0, 0, 0, 0, false, dtpurchaseinvoice.Rows[i]["ItemWithVarient"]);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string ID)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable item = SaleOrder.SaleOrderWagesDetailByOrderIds(ID);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			GridEXRow rr = grdContractorWages.GetRow(0);
			if (grdContractorWages.GetDataRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["ItemId"].Value) == 0 && Conversion.ToDecimal(rr.Cells["Amount"].Value) == 0m)
			{
				dtWagesDetail.Clear();
			}
			bool flag = false;
			foreach (DataRow row in item.Rows)
			{
				if (grdContractorWages.GetDataRows().Length != 0)
				{
					GridEXRow[] dataRows = grdContractorWages.GetDataRows();
					for (int i = 0; i < dataRows.Length; i++)
					{
						if (Conversion.ToInt(dataRows[i].Cells["OrderWagesId"].Value) == Conversion.ToInt(row["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtWagesDetail.Rows.Add(0, 0, 0, row["SaleOrderId"], row["Id"], row["ItemId"], row["ItemName"], 0, 0, 0, row["ContractorId"], row["ContractorName"], row["ContractorWagesRateScheduleId"], row["ServiceActivity"], row["Qty"], 0, row["Rate"], row["Amount"], 0, 0, row["Remarks"]);
					flag = false;
				}
			}
			if (dtWagesDetail.Rows.Count == 0 || dtWagesDetail == null)
			{
				AddRowInContractorWagesGrid();
			}
			else
			{
				grdContractorWages.DataSource = dtWagesDetail;
			}
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
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
				if (item != null && Conversion.ToInt(item.Cells["GdnId"].Value) == 0)
				{
					MessageBox.Show("You cannot load Gdn because a record exists without Gdn in the grid");
					return;
				}
			}
			LoadGdn = new LoadPendingGDNDirectConcrete(UserAccount);
			LoadGdn.ShowDialog();
			LoadGdn.BringToFront();
			LoadInGridDetailFromGdn();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailFromGdn()
	{
		try
		{
			DataTable dtGdnData = LoadGdn.dtLoader;
			if (dtGdnData.Rows.Count <= 0)
			{
				return;
			}
			string OrderIds = "";
			string ReferenceNos = "";
			for (int j = 0; j < dtGdnData.Rows.Count; j++)
			{
				if (Conversion.ToInt(dtGdnData.Rows[j]["Id"]) > 0)
				{
					OrderIds = Conversion.ToInt(dtGdnData.Rows[j]["Id"]).ToString();
					break;
				}
			}
			((Control)(object)comsupplier).Enabled = false;
			GdnExist = true;
			bool flag = false;
			for (int i = 0; i < dtGdnData.Rows.Count; i++)
			{
				if (!string.IsNullOrEmpty(Conversion.ToString(dtGdnData.Rows[i]["ReferenceNo"])) && !ReferenceNos.Contains(Conversion.ToString(dtGdnData.Rows[i]["ReferenceNo"])))
				{
					ReferenceNos = ReferenceNos + "," + Conversion.ToString(dtGdnData.Rows[i]["ReferenceNo"]);
				}
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						if (Conversion.ToInt(comsupplier.Value) > 0 && Conversion.ToInt(comsupplier.Value) != Conversion.ToInt(dtGdnData.Rows[i]["SupplierCustomerId"]))
						{
							throw new Exception("Data against another Customer Already Exist");
						}
						if (Conversion.ToInt(obj.Cells["GdnDetailId"].Value) == Conversion.ToInt(dtGdnData.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, dtGdnData.Rows[i]["Id"], dtGdnData.Rows[i]["DetailId"], dtGdnData.Rows[i]["DocNo"], 0, 0, 0, dtGdnData.Rows[i]["WarehouseId"], dtGdnData.Rows[i]["WareHouseName"], dtGdnData.Rows[i]["ItemId"], dtGdnData.Rows[i]["ItemName"], dtGdnData.Rows[i]["ItemCode"], dtGdnData.Rows[i]["ScheduleId"], dtGdnData.Rows[i]["ItemuomId"], dtGdnData.Rows[i]["PackUom"], dtGdnData.Rows[i]["PackEquivalent"], dtGdnData.Rows[i]["ItemAttributeVarientId"], dtGdnData.Rows[i]["ItemArrtibuteVarient"], dtGdnData.Rows[i]["VarientEquivalent"], dtGdnData.Rows[i]["JobLotId"], dtGdnData.Rows[i]["JobLotDescription"], dtGdnData.Rows[i]["ItemQty"], dtGdnData.Rows[i]["ItemQty"], dtGdnData.Rows[i]["ItemWeight"], dtGdnData.Rows[i]["NetWeight"], dtGdnData.Rows[i]["NetWeight"], 0, 0, 0, 0, 0, "", 0, 0, 0, 0, dtGdnData.Rows[i]["GpDate"], dtGdnData.Rows[i]["GpNo"], dtGdnData.Rows[i]["VehicleNo"], dtGdnData.Rows[i]["CityId"], dtGdnData.Rows[i]["CityName"], 0, 0, 0, 0, 0, 0, dtGdnData.Rows[i]["IsFOC"], dtGdnData.Rows[i]["WithVariant"]);
				}
				flag = false;
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			GridItemPriceUpdateFromSchedule();
			comsupplier.Value = Conversion.ToInt((object)LoadGdn.dtLoader.Rows[0]["SupplierCustomerId"].ToString());
			txtDistance.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["Distance"].ToString());
			IEnumerable<string> DistinctReferenceNos = (from BT in ReferenceNos.Split(',')
				where !string.IsNullOrEmpty(BT)
				select BT).Distinct();
			txtSupplierReference.Text = string.Join(",", DistinctReferenceNos);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["DeliveryTerm"].ToString());
			txtRefPartyName.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["ReferencPartyName"].ToString());
			txtRefPartyAddress.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["ReferencPartyAddress"].ToString());
			txtRefPartyCellNo.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["ReferencPartyCellNo"].ToString());
			txtremarks.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["RemarksHeader"].ToString());
			CmbTransporterAc.Value = Conversion.ToInt((object)LoadGdn.dtLoader.Rows[0]["TransporterId"].ToString());
			txtFreightAmountHeader.Text = Conversion.ToString((object)LoadGdn.dtLoader.Rows[0]["FrieghtAmountHeader"].ToString());
			LoadExpDataFromGdn(OrderIds);
			GridItemNetAmountCalculate();
			TotalCommissionAmount();
			OtherItemsProportion();
			FreightHeaderProportion();
			SettlementDiscountProportion();
			CommissionProportion();
			WagesProportion();
			BillAmount();
			OrderIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromGdn(DataTable dtGdnData)
	{
	}

	private void LoadExpDataFromGdn(string ID)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable item = InvGdn.GdnWagesDetailByGdnIds(ID);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			GridEXRow rr = grdContractorWages.GetRow(0);
			if (grdContractorWages.GetDataRows().Length != 0 && rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["ItemId"].Value) == 0 && Conversion.ToDecimal(rr.Cells["Amount"].Value) == 0m)
			{
				dtWagesDetail.Clear();
			}
			bool flag = false;
			foreach (DataRow row in item.Rows)
			{
				if (grdContractorWages.GetDataRows().Length != 0)
				{
					GridEXRow[] dataRows = grdContractorWages.GetDataRows();
					for (int i = 0; i < dataRows.Length; i++)
					{
						if (Conversion.ToInt(dataRows[i].Cells["GdnWagesId"].Value) == Conversion.ToInt(row["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtWagesDetail.Rows.Add(0, row["InvGdnId"], row["Id"], 0, 0, row["ItemId"], row["ItemName"], row["ItemAttributeVarientId"], row["ItemAttributeVarient"], row["VarientEquivalent"], row["ContractorId"], row["ContractorName"], row["ContractorWagesRateScheduleId"], row["ServiceActivity"], row["ParentUomId"], row["Qty"], row["ItemNetWeight"], row["Rate"], row["Amount"], row["AddLess"], row["NetAmount"], row["Remarks"]);
					flag = false;
				}
			}
			if (dtWagesDetail.Rows.Count == 0 || dtWagesDetail == null)
			{
				AddRowInContractorWagesGrid();
			}
			else
			{
				grdContractorWages.DataSource = dtWagesDetail;
			}
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
		}
		catch (Exception ex)
		{
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					comsupplier.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					comsupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdChargeToProduct).Focus();
					}
					if (((Control)(object)grdChargeToProduct).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPartyAddLs).Focus();
					}
					if (((Control)(object)grdPartyAddLs).Focused)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdContractorWages).Focus();
					}
					else
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
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
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
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetailHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetailHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetailHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
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
					GridEXRow r2 = grd.CurrentRow;
					if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						InvSaleInvoiceDetail detail2 = new InvSaleInvoiceDetail();
						detail2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						detail2.InvGdnDetailId = 0;
						detail2.InvGdnId = 0;
						detail2.SaleOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
						detail2.SaleOrderDetailId = Conversion.ToInt(r2.Cells["OrderDetailId"].Value);
						detail2.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
						detail2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						detail2.ScheduleId = Conversion.ToInt(r2.Cells["ScheduleId"].Value);
						detail2.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
						detail2.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
						detail2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
						detail2.ItemQty = Conversion.ToDecimal(r2.Cells["ItemQty"].Value);
						detail2.ItemWeight = Conversion.ToDecimal(r2.Cells["ItemWeight"].Value);
						detail2.ItemNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
						detail2.ItemRateWithOutAddLess = Conversion.ToDecimal(r2.Cells["ItemPrice"].Value);
						detail2.RateAddLess = Conversion.ToDecimal(r2.Cells["AddLessRate"].Value);
						detail2.ItemRate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
						detail2.RateCut = detail2.RateAddLess;
						detail2.ItemAmount = Conversion.ToDecimal(r2.Cells["ItemAmount"].Value);
						detail2.ItemDiscountTypeId = Conversion.ToInt(r2.Cells["DiscountTypeId"].Value);
						detail2.ItemDiscountRate = Conversion.ToDecimal(r2.Cells["DiscRate"].Value);
						detail2.ItemDiscountAmount = Conversion.ToDecimal(r2.Cells["DiscAmount"].Value);
						detail2.ItemAmountWithDisc = Conversion.ToDecimal(r2.Cells["ItemAmountWithDisc"].Value);
						detail2.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
						detail2.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
						detail2.FcyAmount = Conversion.ToDecimal(r2.Cells["FcyAmount"].Value);
						detail2.GpDate = Conversion.ToDateTime(r2.Cells["GpDate"].Value);
						detail2.GpNo = Conversion.ToInt(r2.Cells["GpNo"].Value);
						detail2.VehicleNo = Conversion.ToString(r2.Cells["VehicleNo"].Value);
						detail2.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
						detail2.ExpenseAmount = Conversion.ToDecimal(r2.Cells["Expense"].Value);
						detail2.CommissionAmount = Conversion.ToDecimal(r2.Cells["Commission"].Value);
						detail2.FreightAmount = Conversion.ToDecimal(r2.Cells["Freight"].Value);
						detail2.JournalAmount = Conversion.ToDecimal(r2.Cells["Journal"].Value);
						detail2.WagesAmount = Conversion.ToDecimal(r2.Cells["Wages"].Value);
						detail2.ItemNetAmount = Conversion.ToDecimal(r2.Cells["ItemNetAmount"].Value);
						detail2.RemarksDetail = "";
						detail2.ActionTypeId = 3;
						lstRemoveDetailRecord.Add(detail2);
						dtGrid.Rows[r2.RowIndex].Delete();
						if (r2 != null && (int)r2.RowType == 0)
						{
							r2.Delete();
						}
					}
					else if (r2 != null && (int)r2.RowType == 0)
					{
						r2.Delete();
					}
					grd.UpdateData();
					TotalCommissionAmount();
					FreightHeaderProportion();
					txtExchangeRate_TextChanged(null, null);
					WagesItemFillFromDetail();
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete && !GdnExist)
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.InvGdnDetailId = 0;
					detail.InvGdnId = 0;
					detail.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					detail.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRateWithOutAddLess = Conversion.ToDecimal(r.Cells["ItemPrice"].Value);
					detail.RateAddLess = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.RateCut = detail.RateAddLess;
					detail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					detail.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountTypeId"].Value);
					detail.ItemDiscountRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
					detail.ItemDiscountAmount = Conversion.ToDecimal(r.Cells["DiscAmount"].Value);
					detail.ItemAmountWithDisc = Conversion.ToDecimal(r.Cells["ItemAmountWithDisc"].Value);
					detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					detail.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
					detail.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
					detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					detail.ExpenseAmount = Conversion.ToDecimal(r.Cells["Expense"].Value);
					detail.CommissionAmount = Conversion.ToDecimal(r.Cells["Commission"].Value);
					detail.FreightAmount = Conversion.ToDecimal(r.Cells["Freight"].Value);
					detail.JournalAmount = Conversion.ToDecimal(r.Cells["Journal"].Value);
					detail.WagesAmount = Conversion.ToDecimal(r.Cells["Wages"].Value);
					detail.ItemNetAmount = Conversion.ToDecimal(r.Cells["ItemNetAmount"].Value);
					detail.RemarksDetail = "";
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtGrid.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
				TotalCommissionAmount();
				FreightHeaderProportion();
				txtExchangeRate_TextChanged(null, null);
				WagesItemFillFromDetail();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse") && !GdnExist)
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
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot") && !GdnExist)
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
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CityName" || grd.CurrentColumn.Key == "CityId") && !GdnExist)
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
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "VehicleNo" && !GdnExist)
				{
					GrdPopUp grdPopUp4 = new GrdPopUp(dtVehicleNos, "Id", "VehicleNo");
					grdPopUp4.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp4.ShowDialog();
					grdPopUp4.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp4.ReturnId);
					Name = Conversion.ToString((object)grdPopUp4.ReturnName);
					item.Cells["VehicleNo"].Value = Name;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "DiscountTypeId" || grd.CurrentColumn.Key == "DiscountType"))
				{
					GrdPopUp grdPopUp5 = new GrdPopUp(dtDiscType, "Id", "DiscountType");
					grdPopUp5.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp5.ShowDialog();
					grdPopUp5.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp5.ReturnId);
					Name = Conversion.ToString((object)grdPopUp5.ReturnName);
					item.Cells["DiscountTypeId"].Value = Id;
					item.Cells["DiscountType"].Value = Name;
					if (Id > 0)
					{
						double AmtWithoutDisc = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
						double DisRate = Conversion.ToDouble(item.Cells["DiscRate"].Value);
						double DisAmt = 0.0;
						if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) != 0)
						{
							if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 1)
							{
								DisAmt = DisRate;
								item.Cells["DiscAmount"].Value = DisAmt;
							}
							else if (Conversion.ToInt(item.Cells["DiscountTypeId"].Value) == 2)
							{
								DisAmt = AmtWithoutDisc * DisRate / 100.0;
								item.Cells["DiscAmount"].Value = DisAmt;
							}
						}
						else
						{
							item.Cells["DiscAmount"].Value = 0;
						}
						if (AmtWithoutDisc > 0.0 && DisAmt > 0.0)
						{
							item.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc - DisAmt;
						}
						else if (AmtWithoutDisc > 0.0)
						{
							item.Cells["ItemAmountWithDisc"].Value = AmtWithoutDisc;
						}
						else
						{
							item.Cells["ItemAmountWithDisc"].Value = 0;
						}
					}
					else
					{
						item.Cells["DiscAmount"].Value = 0;
						item.Cells["ItemAmountWithDisc"].Value = 0;
					}
				}
				grd.UpdateData();
				CommonServices.GridAutoAdjustmentNew(grd);
			}
			((Control)(object)grd).Focus();
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
			int CurrId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					Id = CurrId;
					ReadById(Id);
					comsupplier.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.SaleInvoiceDirectConcreteCustomerSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "ItemSlip")
				{
					CommonServices.SaleInvoiceDirectConcreteItemSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(CurrId, 1856)), 1856);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1856);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Id = CurrId;
				ReadById(Id);
				comsupplier.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.SaleInvoiceDirectConcreteCustomerSlip(CurrId);
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdContractorWages.CurrentColumn.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
				if (grdContractorWages.CurrentColumn.Key == "DeleteAll")
				{
					string Activity = Conversion.ToString(r.Cells["ServiceActivity"].Value);
					if (MessageBox.Show("Are you sure to Delete All Rows With Activity " + Activity + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (grdContractorWages.GetDataRows().Length != 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow rr in dataRows)
						{
							if (rr != null && (int)rr.RowType == 0 && Conversion.ToString(rr.Cells["ServiceActivity"].Value) == Activity)
							{
								dtWagesDetail.Rows[rr.RowIndex].Delete();
							}
						}
					}
					if (dtWagesDetail.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtWagesDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtWagesDetail.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtWagesDetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
			{
				GrdPopUp obj4 = new GrdPopUp(dtContr, "Id", "CompanyName");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				Name = Conversion.ToString((object)obj4.ReturnName);
				r.Cells["ContractorId"].Value = Id;
				r.Cells["ContractorName"].Value = Name;
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, Id);
				if (dt2.Rows.Count > 0)
				{
					DataRow foundRow3 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal Rate3 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Rate"]) : 0m);
					int ParentUomId2 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ParentUOMId"]) : 0);
					int ContractorWagesRateScheduleId = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					r.Cells["Rate"].Value = Rate3;
					r.Cells["ParentUomId"].Value = ParentUomId2;
					decimal RowQty2 = Conversion.ToDecimal(r.Cells["Qty"].Value);
					decimal AddLess3 = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (ParentUomId2)
					{
					case 2:
					{
						decimal Amount9 = Rate3 * RowQty2;
						r.Cells["Amount"].Value = Amount9;
						decimal NetAmount6 = Amount9 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount6;
						break;
					}
					case 3:
					{
						decimal Amount10 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate3;
						r.Cells["Amount"].Value = Amount10;
						decimal NetAmount7 = Amount10 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount7;
						break;
					}
					default:
					{
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount11 = default(decimal);
						r.Cells["Amount"].Value = Amount11;
						decimal NetAmount8 = Amount11 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount8;
						MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
						return;
					}
					}
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ItemId" || grdContractorWages.CurrentColumn.Key == "ItemName"))
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(dtItemForWages, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = Name;
				if (Id != PreviousItemId)
				{
					r.Cells["AttributeVarientId"].Value = 0;
					r.Cells["AttributeVarient"].Value = "";
					r.Cells["VarientEquivalent"].Value = 0;
					r.Cells["Qty"].Value = 0;
					r.Cells["ItemNetWeight"].Value = 0;
					r.Cells["ContractorWagesRateScheduleId"].Value = 0;
					r.Cells["ServiceActivity"].Value = "";
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "AttributeVarientId" || grdContractorWages.CurrentColumn.Key == "AttributeVarient"))
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				DataRow[] matchingRows = dtVarientForWages.Select("ItemId=" + Conversion.ToInt(r.Cells["ItemId"].Value));
				if (matchingRows.Length == 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				GrdPopUp obj2 = new GrdPopUp(matchingRows.CopyToDataTable(), "ItemAttributeId", "ItemAttribute");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["AttributeVarientId"].Value = Id;
				r.Cells["AttributeVarient"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow2 = dtVarientForWages.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeId") == Id && row.Field<int>("ItemId") == Conversion.ToInt(r.Cells["ItemId"].Value));
					decimal VarientEquivalent = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["VarientEquivalent"]) : 0m);
					decimal CurrRowQty = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Qty"]) : 0m);
					decimal ItemNetWeight = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["NetWeight"]) : 0m);
					r.Cells["Qty"].Value = CurrRowQty;
					r.Cells["ItemNetWeight"].Value = ItemNetWeight;
					r.Cells["VarientEquivalent"].Value = VarientEquivalent;
					decimal Rate2 = Conversion.ToInt(r.Cells["Rate"].Value);
					decimal AddLess2 = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (Conversion.ToInt(r.Cells["ParentUomId"].Value))
					{
					case 2:
					{
						decimal Amount5 = Rate2 * CurrRowQty;
						r.Cells["Amount"].Value = Amount5;
						break;
					}
					case 3:
					{
						decimal Amount6 = ItemNetWeight / 1000m * Rate2;
						r.Cells["Amount"].Value = Amount6;
						break;
					}
					default:
					{
						decimal Amount7 = default(decimal);
						r.Cells["Amount"].Value = Amount7;
						decimal NetAmount4 = Amount7 + AddLess2;
						r.Cells["NetAmount"].Value = NetAmount4;
						break;
					}
					}
				}
				else
				{
					r.Cells["Qty"].Value = 0;
					r.Cells["ItemNetWeight"].Value = 0;
					decimal Amount8 = default(decimal);
					r.Cells["Amount"].Value = Amount8;
					decimal NetAmount5 = Amount8 - 0m;
					r.Cells["NetAmount"].Value = NetAmount5;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ServiceActivity" || grdContractorWages.CurrentColumn.Key == "ContractorWagesRateScheduleId"))
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				int ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContractorId);
				GrdPopUp obj = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				r.Cells["ContractorWagesRateScheduleId"].Value = Id;
				r.Cells["ServiceActivity"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
					decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
					int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
					r.Cells["Rate"].Value = Rate;
					r.Cells["ParentUomId"].Value = ParentUomId;
					decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (ParentUomId)
					{
					case 2:
					{
						decimal Amount = Rate * RowQty;
						r.Cells["Amount"].Value = Amount;
						decimal NetAmount = Amount + AddLess;
						r.Cells["NetAmount"].Value = NetAmount;
						break;
					}
					case 3:
					{
						decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
						r.Cells["Amount"].Value = Amount2;
						decimal NetAmount2 = Amount2 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount2;
						break;
					}
					default:
					{
						r.Cells["ContractorWagesRateScheduleId"].Value = 0;
						r.Cells["ServiceActivity"].Value = "";
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount3 = default(decimal);
						r.Cells["Amount"].Value = Amount3;
						decimal NetAmount3 = Amount3 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount3;
						MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
						return;
					}
					}
				}
				else
				{
					r.Cells["ContractorWagesRateScheduleId"].Value = 0;
					r.Cells["ServiceActivity"].Value = "";
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					decimal Amount4 = default(decimal);
					r.Cells["Amount"].Value = Amount4;
					r.Cells["NetAmount"].Value = Amount4;
				}
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			WagesProportion();
			txtExchangeRate_TextChanged(null, null);
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
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
			else
			{
				txtduedays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefinePlant_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineReferenceParties(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdPartyByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(comsupplier.Value) > 0)
				{
					Id = Conversion.ToInt(comsupplier.Value);
				}
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					comsupplier.Value = Id;
				}
				comsupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdCustomerHistorySearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtCustomerHistory.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbCustomerHistory.Value) > 0)
				{
					Id = Conversion.ToInt(CmbCustomerHistory.Value);
				}
				if (RdCustomerHistorySearchByName.Checked)
				{
					DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					CmbCustomerHistory.Value = Id;
				}
				CmbCustomerHistory.Focus();
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
			double TotalManualOrderAmount = Conversion.ToDouble((object)txtManualTotalBillAmount.Text);
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			double GridItemAmountWithoutDiscount = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			txtItemAmountHeader.Text = GridItemAmountWithoutDiscount.ToString(clsGlobalVariables.stringFormatsingle);
			GridEX_Helper.GetColumnSum(grdChargeToProduct, "Amount");
			double GridJvDebitAmount = 0.0;
			double GridJvCreditAmount = 0.0;
			GridEXRow[] rows = grdPartyAddLs.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0 || Conversion.ToInt((object)r.Cells["GlAccountId"].Value.ToString()) > 0)
				{
					GridJvDebitAmount += Conversion.ToDouble(r.Cells["Debit"].Value);
					GridJvCreditAmount += Conversion.ToDouble(r.Cells["Credit"].Value);
				}
			}
			double FreightFooter = Conversion.ToDouble((object)txtFreightAmountHeader.Text);
			double LoabourAmountFooter = Conversion.ToDouble((object)txtOtherWagesHeader.Text);
			double ActualBillAmount = GridItemAmountWithoutDiscount + LoabourAmountFooter + FreightFooter + (GridJvCreditAmount - GridJvDebitAmount);
			txtBillamountWithoutCommission.Text = ActualBillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
			{
				double CommissionAmountHeader = Conversion.ToDouble((object)CommNetAmtHeader.Text);
				ActualBillAmount -= CommissionAmountHeader;
			}
			double SettlementDiscountHeader = Conversion.ToDouble((object)txtSettlementDiscountHeader.Text);
			ActualBillAmount -= SettlementDiscountHeader;
			if (TotalManualOrderAmount == 0.0)
			{
				txtDiscountHeader.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = ActualBillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = ActualBillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else if (TotalManualOrderAmount > ActualBillAmount)
			{
				MessageBox.Show($"Manual Bill Amount {TotalManualOrderAmount} Can't be Greater Than Expected Bill Amount {ActualBillAmount}");
				txtDiscountHeader.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = ActualBillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtManualTotalBillAmount.Text = ActualBillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtDiscountHeader.Text = (ActualBillAmount - TotalManualOrderAmount).ToString(clsGlobalVariables.stringFormatsingle);
				txtBillAmountHeader.Text = TotalManualOrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			txtBillamountWithoutCommission.Text = (Conversion.ToDouble((object)txtBillamountWithoutCommission.Text) - Conversion.ToDouble((object)txtDiscountHeader.Text)).ToString(clsGlobalVariables.stringFormatsingle);
			PropotionateDiscountByManualOrderAmountInGrid();
			txtItemNetAmountHeader.Text = GridEX_Helper.GetColumnSum(grd, "ItemNetAmount").ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			decimal totalGridAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
			decimal totalCalculatedDiscount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				decimal RowItemAmount2 = Conversion.ToDecimal((object)r2.Cells["ItemAmount"].Value.ToString());
				decimal RowDiscountAmount = ((totalGridAmount > 0m) ? (RowItemAmount2 / totalGridAmount * 100m) : 0m) * FooterDisAmt / 100m;
				decimal DisRate2 = default(decimal);
				r2.BeginEdit();
				decimal ItemAmountWithDisc2 = default(decimal);
				r2.Cells["DiscAmount"].Value = Math.Round(RowDiscountAmount, 2);
				ItemAmountWithDisc2 = RowItemAmount2 - RowDiscountAmount;
				r2.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
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
					ItemAmountWithDisc2 = RowItemAmount2 - RowDiscountAmount;
					r2.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
				}
				else if (RowItemAmount2 > 0m)
				{
					ItemAmountWithDisc2 = RowItemAmount2;
					r2.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc2;
				}
				else
				{
					r2.Cells["ItemAmountWithDisc"].Value = 0;
				}
				CalculateRowNetAmount(r2);
				r2.EndEdit();
				totalCalculatedDiscount += RowDiscountAmount;
			}
			if (!(Math.Abs(totalCalculatedDiscount - FooterDisAmt) > 0.01m))
			{
				return;
			}
			decimal adjustmentFactor = FooterDisAmt / totalCalculatedDiscount;
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				decimal RowItemAmount = Conversion.ToDecimal((object)r.Cells["ItemAmount"].Value.ToString());
				decimal UpdatedDiscountAmount = Math.Round(Conversion.ToDecimal((object)r.Cells["DiscAmount"].Value.ToString()) * adjustmentFactor, 2);
				r.BeginEdit();
				r.Cells["DiscAmount"].Value = UpdatedDiscountAmount;
				r.Cells["ItemAmountWithDisc"].Value = RowItemAmount - UpdatedDiscountAmount;
				decimal DisRate = default(decimal);
				decimal ItemAmountWithDisc = default(decimal);
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
				ItemAmountWithDisc = ((RowItemAmount > 0m && UpdatedDiscountAmount > 0m) ? (RowItemAmount - UpdatedDiscountAmount) : ((!(RowItemAmount > 0m)) ? default(decimal) : RowItemAmount));
				r.Cells["ItemAmountWithDisc"].Value = ItemAmountWithDisc;
				CalculateRowNetAmount(r);
				r.EndEdit();
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
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Expected O, but got Unknown
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Expected O, but got Unknown
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bba: Expected O, but got Unknown
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Expected O, but got Unknown
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Expected O, but got Unknown
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Expected O, but got Unknown
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7d: Expected O, but got Unknown
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db4: Expected O, but got Unknown
		//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Expected O, but got Unknown
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0c: Expected O, but got Unknown
		//IL_0e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e43: Expected O, but got Unknown
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef3: Expected O, but got Unknown
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Expected O, but got Unknown
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f14: Expected O, but got Unknown
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Expected O, but got Unknown
		//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Expected O, but got Unknown
		//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Expected O, but got Unknown
		//IL_1007: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Expected O, but got Unknown
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1124: Expected O, but got Unknown
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1145: Expected O, but got Unknown
		//IL_11ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d4: Expected O, but got Unknown
		//IL_11e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ea: Expected O, but got Unknown
		//IL_1238: Unknown result type (might be due to invalid IL or missing references)
		//IL_1242: Expected O, but got Unknown
		//IL_129b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a5: Expected O, but got Unknown
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Expected O, but got Unknown
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Expected O, but got Unknown
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c3: Expected O, but got Unknown
		//IL_13fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1405: Expected O, but got Unknown
		//IL_145e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1468: Expected O, but got Unknown
		//IL_148a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1494: Expected O, but got Unknown
		//IL_14a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Expected O, but got Unknown
		//IL_1ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf1: Expected O, but got Unknown
		//IL_1cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d08: Expected O, but got Unknown
		//IL_2216: Unknown result type (might be due to invalid IL or missing references)
		//IL_2220: Expected O, but got Unknown
		//IL_222d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2237: Expected O, but got Unknown
		//IL_2244: Unknown result type (might be due to invalid IL or missing references)
		//IL_224e: Expected O, but got Unknown
		//IL_24d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e1: Expected O, but got Unknown
		//IL_24ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f8: Expected O, but got Unknown
		//IL_2505: Unknown result type (might be due to invalid IL or missing references)
		//IL_250f: Expected O, but got Unknown
		//IL_279b: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a5: Expected O, but got Unknown
		//IL_27b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27bc: Expected O, but got Unknown
		//IL_fd52: Unknown result type (might be due to invalid IL or missing references)
		//IL_fd5c: Expected O, but got Unknown
		//IL_fd80: Unknown result type (might be due to invalid IL or missing references)
		//IL_fd8a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdChargeToProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPartyAddLs_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
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
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
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
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
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
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.btnUpdateDiscountInGrid = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdChargeToProduct = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdPartyAddLs = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CmbContractorForUpdateInWagesGrid = new UltraCombo();
		this.BtnUpdateConctratorInWages = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.BtnGenerateWagesRows = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.CmbActivityforContractorUpdateInWagesGrid = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.label68 = new System.Windows.Forms.Label();
		this.txtManualTotalBillAmount = new System.Windows.Forms.TextBox();
		this.CmbCommissionDebitAccount = new UltraCombo();
		this.cmbSettlementDiscountAccount = new UltraCombo();
		this.CmbOtherWagesAccount = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CommNetAmtHeader = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.txtBillamountWithoutCommission = new System.Windows.Forms.TextBox();
		this.txtSettlementDiscountHeader = new System.Windows.Forms.TextBox();
		this.label89 = new System.Windows.Forms.Label();
		this.txtOtherWagesHeader = new System.Windows.Forms.TextBox();
		this.txtDiscountHeader = new System.Windows.Forms.TextBox();
		this.txtItemAmountHeader = new System.Windows.Forms.TextBox();
		this.txtItemNetAmountHeader = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbTransporterAc = new UltraCombo();
		this.txtFreightAmountHeader = new System.Windows.Forms.TextBox();
		this.txtBillAmountHeader = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbRefSalesMan = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
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
		this.CmbPackUom = new UltraCombo();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtAddLess = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtItemPrice = new System.Windows.Forms.TextBox();
		this.chkisFreeOfCost = new System.Windows.Forms.CheckBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtDiscountRate = new System.Windows.Forms.TextBox();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.CmbDiscountType = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbVehicleNo = new UltraCombo();
		this.CmbAttributeVarient = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
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
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.RdPartyByCode = new System.Windows.Forms.RadioButton();
		this.RdPartyByName = new System.Windows.Forms.RadioButton();
		this.txtDistance = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtRefPartyCellNo = new System.Windows.Forms.TextBox();
		this.txtRefPartyAddress = new System.Windows.Forms.TextBox();
		this.txtRefPartyName = new System.Windows.Forms.TextBox();
		this.txtDeliverydays = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtDeliveryStartDate = new System.Windows.Forms.DateTimePicker();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.CmbVisitedbyName = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtInvoiceQty = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.comsupplier = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkCustomerPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.combcommUOM = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnCustomerSlip = new System.Windows.Forms.ToolStripButton();
		this.btn1856APrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadOrder = new System.Windows.Forms.ToolStripButton();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label67 = new System.Windows.Forms.Label();
		this.RdCustomerHistorySearchByCode = new System.Windows.Forms.RadioButton();
		this.CmbCommissionAgentHistory = new UltraCombo();
		this.RdCustomerHistorySearchByName = new System.Windows.Forms.RadioButton();
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
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel13.SuspendLayout();
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
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).BeginInit();
		this.panel4.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSettlementDiscountAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefSalesMan).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVisitedbyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
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
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1318, 811);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1310, 782);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel13);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1304, 776);
		this.panel1.TabIndex = 7;
		this.panel13.Controls.Add(this.tabControl2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 325);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1304, 230);
		this.panel13.TabIndex = 34;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1304, 230);
		this.tabControl2.TabIndex = 33;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1296, 201);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1290, 174);
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
		this.panel16.Controls.Add(this.label64);
		this.panel16.Controls.Add(this.btnUpdateDiscountInGrid);
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1290, 21);
		this.panel16.TabIndex = 33;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label64.ForeColor = System.Drawing.Color.White;
		this.label64.Location = new System.Drawing.Point(413, 4);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(660, 14);
		this.label64.TabIndex = 39;
		this.label64.Text = "Click the Button To Update Same Discount Type as in Combo and Discount Rate as In text box In All rows";
		this.btnUpdateDiscountInGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnUpdateDiscountInGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDiscountInGrid.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDiscountInGrid.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDiscountInGrid.Location = new System.Drawing.Point(1080, -1);
		this.btnUpdateDiscountInGrid.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDiscountInGrid.Name = "btnUpdateDiscountInGrid";
		this.btnUpdateDiscountInGrid.Size = new System.Drawing.Size(163, 23);
		this.btnUpdateDiscountInGrid.TabIndex = 38;
		this.btnUpdateDiscountInGrid.Text = "Update Same Discount";
		this.btnUpdateDiscountInGrid.UseVisualStyleBackColor = false;
		this.btnUpdateDiscountInGrid.Click += new System.EventHandler(btnUpdateDiscountInGrid_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1250, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdChargeToProduct);
		this.tabPage4.Controls.Add(this.panel12);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1296, 201);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Debit To Customer (Other Item)";
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
		((System.Windows.Forms.Control)(object)this.grdChargeToProduct).Size = new System.Drawing.Size(1290, 174);
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
		this.panel12.Size = new System.Drawing.Size(1290, 21);
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
		this.tabPage5.Size = new System.Drawing.Size(1296, 201);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Customer Add/Less";
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
		((System.Windows.Forms.Control)(object)this.grdPartyAddLs).Size = new System.Drawing.Size(1296, 179);
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
		this.panel15.Size = new System.Drawing.Size(1296, 22);
		this.panel15.TabIndex = 34;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(6, 2);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(147, 17);
		this.label53.TabIndex = 36;
		this.label53.Text = "Party Add/Less Grid";
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.tabPage6.Controls.Add(this.panel6);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1296, 201);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Contractor Wages";
		this.tabPage6.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(1296, 173);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 1;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid);
		this.panel6.Controls.Add(this.BtnUpdateConctratorInWages);
		this.panel6.Controls.Add(this.label66);
		this.panel6.Controls.Add(this.label65);
		this.panel6.Controls.Add(this.label49);
		this.panel6.Controls.Add(this.BtnGenerateWagesRows);
		this.panel6.Controls.Add(this.label50);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1296, 28);
		this.panel6.TabIndex = 0;
		this.CmbContractorForUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorForUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorForUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorForUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance217;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorForUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Location = new System.Drawing.Point(1042, 1);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Name = "CmbContractorForUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.BtnUpdateConctratorInWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateConctratorInWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateConctratorInWages.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateConctratorInWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.Location = new System.Drawing.Point(1218, 1);
		this.BtnUpdateConctratorInWages.Name = "BtnUpdateConctratorInWages";
		this.BtnUpdateConctratorInWages.Size = new System.Drawing.Size(72, 26);
		this.BtnUpdateConctratorInWages.TabIndex = 3;
		this.BtnUpdateConctratorInWages.Text = "Update";
		this.BtnUpdateConctratorInWages.UseVisualStyleBackColor = false;
		this.BtnUpdateConctratorInWages.Click += new System.EventHandler(BtnUpdateConctratorInWages_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(963, 5);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(81, 19);
		this.label66.TabIndex = 155;
		this.label66.Text = "Contractor";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(768, 5);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(60, 19);
		this.label65.TabIndex = 155;
		this.label65.Text = "Activity";
		this.label49.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(324, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(446, 28);
		this.label49.TabIndex = 128;
		this.label49.Text = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.BtnGenerateWagesRows.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnGenerateWagesRows.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGenerateWagesRows.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnGenerateWagesRows.ForeColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.Location = new System.Drawing.Point(198, 1);
		this.BtnGenerateWagesRows.Name = "BtnGenerateWagesRows";
		this.BtnGenerateWagesRows.Size = new System.Drawing.Size(86, 26);
		this.BtnGenerateWagesRows.TabIndex = 0;
		this.BtnGenerateWagesRows.Text = "Generate";
		this.BtnGenerateWagesRows.UseVisualStyleBackColor = false;
		this.BtnGenerateWagesRows.Click += new System.EventHandler(BtnGenerateWagesRows_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.White;
		this.label50.Location = new System.Drawing.Point(3, 4);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(193, 16);
		this.label50.TabIndex = 129;
		this.label50.Text = "Click To Auto Generate Rows";
		this.CmbActivityforContractorUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivityforContractorUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivityforContractorUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Location = new System.Drawing.Point(829, 1);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Name = "CmbActivityforContractorUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.panel4.Controls.Add(this.PanelMessage);
		this.panel4.Controls.Add(this.label68);
		this.panel4.Controls.Add(this.txtManualTotalBillAmount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount);
		this.panel4.Controls.Add(this.label46);
		this.panel4.Controls.Add(this.label52);
		this.panel4.Controls.Add(this.label79);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.label80);
		this.panel4.Controls.Add(this.label81);
		this.panel4.Controls.Add(this.label83);
		this.panel4.Controls.Add(this.label84);
		this.panel4.Controls.Add(this.label85);
		this.panel4.Controls.Add(this.label86);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.CommNetAmtHeader);
		this.panel4.Controls.Add(this.label87);
		this.panel4.Controls.Add(this.label88);
		this.panel4.Controls.Add(this.txtBillamountWithoutCommission);
		this.panel4.Controls.Add(this.txtSettlementDiscountHeader);
		this.panel4.Controls.Add(this.label89);
		this.panel4.Controls.Add(this.txtOtherWagesHeader);
		this.panel4.Controls.Add(this.txtDiscountHeader);
		this.panel4.Controls.Add(this.txtItemAmountHeader);
		this.panel4.Controls.Add(this.txtItemNetAmountHeader);
		this.panel4.Controls.Add(this.label51);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterAc);
		this.panel4.Controls.Add(this.txtFreightAmountHeader);
		this.panel4.Controls.Add(this.txtBillAmountHeader);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 555);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1304, 221);
		this.panel4.TabIndex = 0;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(818, 186);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(24, 25);
		this.PanelMessage.TabIndex = 142;
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
		this.label68.Location = new System.Drawing.Point(587, 186);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(229, 25);
		this.label68.TabIndex = 140;
		this.label68.Text = "Add Total Bill Amount Manually";
		this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtManualTotalBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualTotalBillAmount.Font = new System.Drawing.Font("Verdana", 10.5f);
		this.txtManualTotalBillAmount.Location = new System.Drawing.Point(844, 186);
		this.txtManualTotalBillAmount.Name = "txtManualTotalBillAmount";
		this.txtManualTotalBillAmount.Size = new System.Drawing.Size(142, 25);
		this.txtManualTotalBillAmount.TabIndex = 141;
		this.txtManualTotalBillAmount.TabStop = false;
		this.txtManualTotalBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtManualTotalBillAmount.TextChanged += new System.EventHandler(txtManualTotalBillAmount_TextChanged);
		this.txtManualTotalBillAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtManualTotalBillAmount_KeyPress);
		this.txtManualTotalBillAmount.Leave += new System.EventHandler(txtManualTotalBillAmount_Leave);
		this.CmbCommissionDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Location = new System.Drawing.Point(844, 129);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Name = "CmbCommissionDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).TabIndex = 8;
		this.cmbSettlementDiscountAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbSettlementDiscountAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSettlementDiscountAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSettlementDiscountAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance277).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance277).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance278;
		((SpecialBoxBase)((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSettlementDiscountAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSettlementDiscountAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount).Location = new System.Drawing.Point(844, 157);
		((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount).Name = "cmbSettlementDiscountAccount";
		((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.cmbSettlementDiscountAccount).TabIndex = 10;
		this.CmbOtherWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Location = new System.Drawing.Point(844, 78);
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Name = "CmbOtherWagesAccount";
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).TabIndex = 5;
		this.label46.BackColor = System.Drawing.Color.Gainsboro;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(587, 129);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(255, 26);
		this.label46.TabIndex = 139;
		this.label46.Text = "Commission Account && Amout (Dr)";
		this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label52.BackColor = System.Drawing.Color.Gainsboro;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(988, 3);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(120, 18);
		this.label52.TabIndex = 138;
		this.label52.Text = "Discount";
		this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(1109, 189);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(21, 18);
		this.label79.TabIndex = 137;
		this.label79.Text = "=";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1109, 107);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(21, 18);
		this.label44.TabIndex = 135;
		this.label44.Text = "+";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1112, 161);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(15, 18);
		this.label39.TabIndex = 135;
		this.label39.Text = "-";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(1109, 25);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(21, 18);
		this.label80.TabIndex = 136;
		this.label80.Text = "=";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.Location = new System.Drawing.Point(1109, 82);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(21, 18);
		this.label81.TabIndex = 135;
		this.label81.Text = "+";
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(1109, 53);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(21, 18);
		this.label83.TabIndex = 134;
		this.label83.Text = "+";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(1112, 133);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(15, 18);
		this.label84.TabIndex = 132;
		this.label84.Text = "-";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(972, 25);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(15, 18);
		this.label85.TabIndex = 1;
		this.label85.Text = "-";
		this.label86.BackColor = System.Drawing.Color.Gainsboro;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(587, 23);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(255, 23);
		this.label86.TabIndex = 126;
		this.label86.Text = "Item Amount";
		this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label27.BackColor = System.Drawing.Color.Gainsboro;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(587, 157);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(255, 26);
		this.label27.TabIndex = 130;
		this.label27.Text = "Settle Discount Ac &&  Amount (Dr)";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CommNetAmtHeader.BackColor = System.Drawing.Color.White;
		this.CommNetAmtHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CommNetAmtHeader.Enabled = false;
		this.CommNetAmtHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CommNetAmtHeader.Location = new System.Drawing.Point(1134, 131);
		this.CommNetAmtHeader.Name = "CommNetAmtHeader";
		this.CommNetAmtHeader.ReadOnly = true;
		this.CommNetAmtHeader.Size = new System.Drawing.Size(161, 23);
		this.CommNetAmtHeader.TabIndex = 9;
		this.CommNetAmtHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label87.BackColor = System.Drawing.Color.Gainsboro;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label87.Location = new System.Drawing.Point(587, 78);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(255, 26);
		this.label87.TabIndex = 130;
		this.label87.Text = "Labour Account && Amount (Cr)";
		this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label88.BackColor = System.Drawing.Color.Gainsboro;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label88.Location = new System.Drawing.Point(1134, 3);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(161, 18);
		this.label88.TabIndex = 129;
		this.label88.Text = "Net Item Amount";
		this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtBillamountWithoutCommission.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillamountWithoutCommission.Enabled = false;
		this.txtBillamountWithoutCommission.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillamountWithoutCommission.Location = new System.Drawing.Point(1134, 105);
		this.txtBillamountWithoutCommission.Name = "txtBillamountWithoutCommission";
		this.txtBillamountWithoutCommission.Size = new System.Drawing.Size(161, 23);
		this.txtBillamountWithoutCommission.TabIndex = 7;
		this.txtBillamountWithoutCommission.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSettlementDiscountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSettlementDiscountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSettlementDiscountHeader.Location = new System.Drawing.Point(1134, 159);
		this.txtSettlementDiscountHeader.Name = "txtSettlementDiscountHeader";
		this.txtSettlementDiscountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtSettlementDiscountHeader.TabIndex = 11;
		this.txtSettlementDiscountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSettlementDiscountHeader.TextChanged += new System.EventHandler(txtSettlementDiscountHeader_TextChanged);
		this.txtSettlementDiscountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSettlementDiscountHeader_KeyPress);
		this.label89.BackColor = System.Drawing.Color.Gainsboro;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label89.Location = new System.Drawing.Point(587, 49);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(255, 26);
		this.label89.TabIndex = 128;
		this.label89.Text = "Carriage Account &&  Amount (Cr)";
		this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOtherWagesHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherWagesHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherWagesHeader.Location = new System.Drawing.Point(1134, 80);
		this.txtOtherWagesHeader.Name = "txtOtherWagesHeader";
		this.txtOtherWagesHeader.Size = new System.Drawing.Size(161, 23);
		this.txtOtherWagesHeader.TabIndex = 6;
		this.txtOtherWagesHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWagesHeader.TextChanged += new System.EventHandler(txtOtherWagesHeader_TextChanged);
		this.txtOtherWagesHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherWagesHeader_KeyPress);
		this.txtDiscountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountHeader.Enabled = false;
		this.txtDiscountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountHeader.Location = new System.Drawing.Point(988, 23);
		this.txtDiscountHeader.Name = "txtDiscountHeader";
		this.txtDiscountHeader.Size = new System.Drawing.Size(120, 23);
		this.txtDiscountHeader.TabIndex = 1;
		this.txtDiscountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountHeader.Enabled = false;
		this.txtItemAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmountHeader.Location = new System.Drawing.Point(844, 23);
		this.txtItemAmountHeader.Name = "txtItemAmountHeader";
		this.txtItemAmountHeader.Size = new System.Drawing.Size(122, 23);
		this.txtItemAmountHeader.TabIndex = 0;
		this.txtItemAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemNetAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemNetAmountHeader.Enabled = false;
		this.txtItemNetAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemNetAmountHeader.Location = new System.Drawing.Point(1134, 23);
		this.txtItemNetAmountHeader.Name = "txtItemNetAmountHeader";
		this.txtItemNetAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtItemNetAmountHeader.TabIndex = 2;
		this.txtItemNetAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.BackColor = System.Drawing.Color.Gainsboro;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(988, 187);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(120, 23);
		this.label51.TabIndex = 125;
		this.label51.Text = "Net Bill Amount";
		this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label45.BackColor = System.Drawing.Color.Gainsboro;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(988, 106);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(120, 20);
		this.label45.TabIndex = 125;
		this.label45.Text = "Bill Amount";
		this.label45.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.CmbTransporterAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporterAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Location = new System.Drawing.Point(844, 49);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Name = "CmbTransporterAc";
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).TabIndex = 3;
		this.txtFreightAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmountHeader.Location = new System.Drawing.Point(1134, 51);
		this.txtFreightAmountHeader.Name = "txtFreightAmountHeader";
		this.txtFreightAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtFreightAmountHeader.TabIndex = 4;
		this.txtFreightAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmountHeader.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.txtFreightAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmount_KeyPress);
		this.txtBillAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmountHeader.Location = new System.Drawing.Point(1134, 187);
		this.txtBillAmountHeader.Name = "txtBillAmountHeader";
		this.txtBillAmountHeader.ReadOnly = true;
		this.txtBillAmountHeader.Size = new System.Drawing.Size(161, 23);
		this.txtBillAmountHeader.TabIndex = 12;
		this.txtBillAmountHeader.TabStop = false;
		this.txtBillAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel3.BackColor = System.Drawing.Color.White;
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
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1304, 300);
		this.panel3.TabIndex = 29;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefSalesMan);
		this.groupBox3.Controls.Add(this.label16);
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
		this.groupBox3.Location = new System.Drawing.Point(1006, 28);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(283, 153);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Commission";
		this.CmbRefSalesMan.AllowNull = (DefaultableBoolean)1;
		this.CmbRefSalesMan.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefSalesMan.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefSalesMan.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Appearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance307).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance307).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance307;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance308;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRefSalesMan.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Location = new System.Drawing.Point(67, 44);
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Name = "CmbRefSalesMan";
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).TabIndex = 1;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(3, 50);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(53, 15);
		this.label16.TabIndex = 111;
		this.label16.Text = "SaleMan";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(3, 22);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Agent (Cr)";
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(67, 16);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 105);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(3, 78);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(32, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Type";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(67, 127);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(212, 23);
		this.txtCommissionRemarks.TabIndex = 5;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(3, 129);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 95;
		this.label25.Text = "Remarks";
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(67, 72);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(155, 78);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Rate";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(189, 74);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(90, 23);
		this.txtcommrate.TabIndex = 3;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(67, 101);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(212, 23);
		this.txtcommamount.TabIndex = 4;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox1.Controls.Add(this.label58);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add(this.txtAddLess);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.txtItemPrice);
		this.groupBox1.Controls.Add(this.chkisFreeOfCost);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtNetWeight);
		this.groupBox1.Controls.Add(this.label69);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.txtDiscountRate);
		this.groupBox1.Controls.Add(this.txtDiscountAmount);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtCommAgentId);
		this.groupBox1.Controls.Add(this.txtSupplierGLId);
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
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 181);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1283, 115);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance284;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance285;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance290).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance291).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance291).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance291).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance291;
		((AppearanceBase)appearance309).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(460, 35);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(86, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(460, 15);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(61, 15);
		this.label58.TabIndex = 155;
		this.label58.Text = "Pack Uom";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(1100, 15);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(80, 15);
		this.label57.TabIndex = 153;
		this.label57.Text = "Add Less Rate";
		this.txtAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLess.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLess.Location = new System.Drawing.Point(1103, 37);
		this.txtAddLess.Name = "txtAddLess";
		this.txtAddLess.Size = new System.Drawing.Size(74, 23);
		this.txtAddLess.TabIndex = 9;
		this.txtAddLess.Tag = "AddLess";
		this.txtAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.TextChanged += new System.EventHandler(txtAddLess_TextChanged);
		this.txtAddLess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLess_KeyPress);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(1024, 15);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(61, 15);
		this.label56.TabIndex = 151;
		this.label56.Text = "Item Price";
		this.txtItemPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemPrice.Enabled = false;
		this.txtItemPrice.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemPrice.Location = new System.Drawing.Point(1022, 37);
		this.txtItemPrice.Name = "txtItemPrice";
		this.txtItemPrice.Size = new System.Drawing.Size(80, 23);
		this.txtItemPrice.TabIndex = 8;
		this.txtItemPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemPrice.TextChanged += new System.EventHandler(txtItemPrice_TextChanged);
		this.chkisFreeOfCost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkisFreeOfCost.AutoSize = true;
		this.chkisFreeOfCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkisFreeOfCost.ForeColor = System.Drawing.Color.Black;
		this.chkisFreeOfCost.Location = new System.Drawing.Point(1108, 62);
		this.chkisFreeOfCost.Name = "chkisFreeOfCost";
		this.chkisFreeOfCost.Size = new System.Drawing.Size(102, 19);
		this.chkisFreeOfCost.TabIndex = 21;
		this.chkisFreeOfCost.Text = "Is Free Of Cost";
		this.chkisFreeOfCost.UseVisualStyleBackColor = true;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1104, 84);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(33, 27);
		this.btnAdd.TabIndex = 22;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(914, 15);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(68, 15);
		this.label43.TabIndex = 149;
		this.label43.Text = "Net Weight";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(914, 37);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeight.TabIndex = 7;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(228, 66);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(56, 15);
		this.label69.TabIndex = 146;
		this.label69.Text = "Disc Rate";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(309, 66);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(77, 15);
		this.label70.TabIndex = 147;
		this.label70.Text = "Disc Amount";
		this.txtDiscountRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountRate.Location = new System.Drawing.Point(228, 86);
		this.txtDiscountRate.Name = "txtDiscountRate";
		this.txtDiscountRate.Size = new System.Drawing.Size(80, 23);
		this.txtDiscountRate.TabIndex = 13;
		this.txtDiscountRate.Tag = "DiscPercent";
		this.txtDiscountRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountRate.TextChanged += new System.EventHandler(txtDiscountRate_TextChanged);
		this.txtDiscountRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountRate_KeyPress);
		this.txtDiscountRate.Leave += new System.EventHandler(txtDiscountRate_Leave);
		this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountAmount.Location = new System.Drawing.Point(309, 86);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(101, 23);
		this.txtDiscountAmount.TabIndex = 14;
		this.txtDiscountAmount.TabStop = false;
		this.txtDiscountAmount.Tag = "DiscAmount";
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmount.TextChanged += new System.EventHandler(txtDiscountAmount_TextChanged);
		this.txtDiscountAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountAmount_KeyPress);
		this.txtDiscountAmount.Leave += new System.EventHandler(txtDiscountAmount_Leave);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(411, 66);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(80, 15);
		this.label71.TabIndex = 145;
		this.label71.Text = "Total Amount";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(411, 86);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(109, 23);
		this.txtTotalAmount.TabIndex = 15;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(109, 84);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Leave += new System.EventHandler(CmbDiscountType_Leave);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(109, 66);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(83, 15);
		this.label15.TabIndex = 141;
		this.label15.Text = "Discount Type";
		this.CmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Location = new System.Drawing.Point(834, 84);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Name = "CmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(548, 35);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(820, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Item Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(658, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 132;
		this.label2.Text = "Varient Unit";
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1258, 36);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1258, 65);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(521, 84);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(521, 66);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(736, 37);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(82, 23);
		this.txtQty.TabIndex = 5;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(548, 15);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(94, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Attribute Varient";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(742, 15);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(820, 37);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(92, 23);
		this.txtItemWeight.TabIndex = 6;
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(652, 37);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(82, 23);
		this.txtVarientEquivalent.TabIndex = 4;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(294, 13);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(241, 13);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance258;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance264).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance265).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(949, 84);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 20;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(949, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(375, 15);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1173, 84);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(67, 27);
		this.btnCancelUpdateDetial.TabIndex = 24;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1103, 84);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetail.TabIndex = 23;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(781, 86);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(51, 23);
		this.txtgatepassno.TabIndex = 18;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(834, 66);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(781, 66);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(204, 35);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(680, 86);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(99, 23);
		this.txtgpdate.TabIndex = 17;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1179, 15);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(52, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Net Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(1178, 37);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(77, 23);
		this.txtRate.TabIndex = 10;
		this.txtRate.Tag = "NetRate";
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
		this.label35.Location = new System.Drawing.Point(680, 66);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(207, 15);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(3, 15);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(3, 86);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(104, 23);
		this.txtAmount.TabIndex = 11;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(5, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.groupBox2.Controls.Add(this.RdPartyByCode);
		this.groupBox2.Controls.Add(this.RdPartyByName);
		this.groupBox2.Controls.Add(this.txtDistance);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtRefPartyCellNo);
		this.groupBox2.Controls.Add(this.txtRefPartyAddress);
		this.groupBox2.Controls.Add(this.txtRefPartyName);
		this.groupBox2.Controls.Add(this.txtDeliverydays);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.txtDeliveryStartDate);
		this.groupBox2.Controls.Add(this.btnDefinePlant);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVisitedbyName);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.txtInvoiceWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtInvoiceQty);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtSupplierReference);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 28);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1001, 153);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.RdPartyByCode.AutoSize = true;
		this.RdPartyByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByCode.Location = new System.Drawing.Point(38, 53);
		this.RdPartyByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByCode.Name = "RdPartyByCode";
		this.RdPartyByCode.Size = new System.Drawing.Size(52, 19);
		this.RdPartyByCode.TabIndex = 145;
		this.RdPartyByCode.Text = "Code";
		this.RdPartyByCode.UseVisualStyleBackColor = true;
		this.RdPartyByName.AutoSize = true;
		this.RdPartyByName.Checked = true;
		this.RdPartyByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByName.Location = new System.Drawing.Point(38, 37);
		this.RdPartyByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByName.Name = "RdPartyByName";
		this.RdPartyByName.Size = new System.Drawing.Size(57, 19);
		this.RdPartyByName.TabIndex = 144;
		this.RdPartyByName.TabStop = true;
		this.RdPartyByName.Text = "Name";
		this.RdPartyByName.UseVisualStyleBackColor = true;
		this.RdPartyByName.CheckedChanged += new System.EventHandler(RdPartyByName_CheckedChanged);
		this.txtDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDistance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDistance.Location = new System.Drawing.Point(398, 15);
		this.txtDistance.Name = "txtDistance";
		this.txtDistance.Size = new System.Drawing.Size(165, 23);
		this.txtDistance.TabIndex = 7;
		this.txtDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(563, 47);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(45, 15);
		this.label30.TabIndex = 137;
		this.label30.Text = "Cell No";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(563, 74);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(49, 15);
		this.label34.TabIndex = 138;
		this.label34.Text = "Address";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(563, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(54, 15);
		this.label36.TabIndex = 139;
		this.label36.Text = "Ref Party";
		this.txtRefPartyCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyCellNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyCellNo.Location = new System.Drawing.Point(617, 43);
		this.txtRefPartyCellNo.Name = "txtRefPartyCellNo";
		this.txtRefPartyCellNo.Size = new System.Drawing.Size(198, 23);
		this.txtRefPartyCellNo.TabIndex = 15;
		this.txtRefPartyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyAddress.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyAddress.Location = new System.Drawing.Point(617, 70);
		this.txtRefPartyAddress.Name = "txtRefPartyAddress";
		this.txtRefPartyAddress.Size = new System.Drawing.Size(198, 23);
		this.txtRefPartyAddress.TabIndex = 16;
		this.txtRefPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyName.Location = new System.Drawing.Point(617, 15);
		this.txtRefPartyName.Name = "txtRefPartyName";
		this.txtRefPartyName.Size = new System.Drawing.Size(198, 23);
		this.txtRefPartyName.TabIndex = 14;
		this.txtDeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliverydays.Location = new System.Drawing.Point(515, 124);
		this.txtDeliverydays.Name = "txtDeliverydays";
		this.txtDeliverydays.Size = new System.Drawing.Size(48, 23);
		this.txtDeliverydays.TabIndex = 13;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(308, 19);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(53, 15);
		this.label41.TabIndex = 134;
		this.label41.Text = "Distance";
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(308, 120);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(88, 30);
		this.label28.TabIndex = 134;
		this.label28.Text = "Delivery Start Date and Days";
		this.txtDeliveryStartDate.CustomFormat = "dd-MMM-yy";
		this.txtDeliveryStartDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeliveryStartDate.Location = new System.Drawing.Point(398, 124);
		this.txtDeliveryStartDate.Name = "txtDeliveryStartDate";
		this.txtDeliveryStartDate.Size = new System.Drawing.Size(116, 23);
		this.txtDeliveryStartDate.TabIndex = 12;
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(277, 122);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(31, 26);
		this.btnDefinePlant.TabIndex = 6;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.btnDefinePlant.Click += new System.EventHandler(btnDefinePlant_Click);
		this.CmbVisitedbyName.AllowNull = (DefaultableBoolean)1;
		this.CmbVisitedbyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVisitedbyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVisitedbyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVisitedbyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Location = new System.Drawing.Point(98, 122);
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Name = "CmbVisitedbyName";
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).TabIndex = 5;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(4, 128);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(59, 15);
		this.label22.TabIndex = 131;
		this.label22.Text = "Visited By";
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(886, 43);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.ShortcutsEnabled = false;
		this.txtInvoiceWeight.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceWeight.TabIndex = 19;
		this.txtInvoiceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(886, 69);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(112, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(886, 124);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(112, 23);
		this.txtFcyAmount.TabIndex = 22;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInvoiceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceQty.Enabled = false;
		this.txtInvoiceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceQty.Location = new System.Drawing.Point(886, 15);
		this.txtInvoiceQty.Name = "txtInvoiceQty";
		this.txtInvoiceQty.ShortcutsEnabled = false;
		this.txtInvoiceQty.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceQty.TabIndex = 18;
		this.txtInvoiceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(886, 96);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(112, 23);
		this.txtExchangeRate.TabIndex = 21;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(816, 47);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(65, 15);
		this.label62.TabIndex = 129;
		this.label62.Text = "Invoice Wt";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(816, 128);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(72, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Fcy Amount";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(816, 100);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(58, 15);
		this.label60.TabIndex = 127;
		this.label60.Text = "Exch Rate";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(816, 19);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(68, 15);
		this.label63.TabIndex = 130;
		this.label63.Text = "Invoice Qty";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(816, 74);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(55, 15);
		this.label61.TabIndex = 128;
		this.label61.Text = "Fcy Code";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(98, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(92, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(192, 15);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(115, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(98, 96);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(209, 23);
		this.txtbillno.TabIndex = 4;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(398, 41);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 8;
		this.combpttrm.ValueChanged += new System.EventHandler(combpttrm_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(4, 74);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Reference No";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance191;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(398, 94);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(4, 100);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(98, 70);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(209, 23);
		this.txtSupplierReference.TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(617, 96);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(198, 51);
		this.txtremarks.TabIndex = 17;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(563, 100);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Remarks";
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(2, 39);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(34, 33);
		this.label7.TabIndex = 65;
		this.label7.Text = "Party (Dr)";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(308, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Term";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(398, 70);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(48, 23);
		this.txtduedays.TabIndex = 9;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance193;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(98, 41);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(308, 74);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days,Date";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(447, 70);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(116, 23);
		this.duedate.TabIndex = 10;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(308, 100);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(74, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Deliver Term";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkCustomerPrintslip);
		this.panel11.Controls.Add(this.ChkBok);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1304, 26);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(193, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "SALE DIRECT INVOICE";
		this.ChkCustomerPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkCustomerPrintslip.AutoSize = true;
		this.ChkCustomerPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkCustomerPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkCustomerPrintslip.Location = new System.Drawing.Point(1012, 4);
		this.ChkCustomerPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkCustomerPrintslip.Name = "ChkCustomerPrintslip";
		this.ChkCustomerPrintslip.Size = new System.Drawing.Size(157, 20);
		this.ChkCustomerPrintslip.TabIndex = 7;
		this.ChkCustomerPrintslip.Text = "Print Customer Slip";
		this.ChkCustomerPrintslip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1173, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(118, 20);
		this.ChkBok.TabIndex = 8;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(810, 4);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(196, 20);
		this.ChkResetOnSave.TabIndex = 6;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance298;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance304).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance305).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance305).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance305).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
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
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance202).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance319).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance321;
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
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnCustomerSlip,
			this.btn1856APrint, this.btnPrint, this.toolStripSeparator5, this.btnshortcutkeys, this.BtnLoadOrder, this.btnLoadGdn
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1304, 25);
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnCustomerSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCustomerSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnCustomerSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCustomerSlip.Name = "btnCustomerSlip";
		this.btnCustomerSlip.Size = new System.Drawing.Size(128, 22);
		this.btnCustomerSlip.Text = "1856-Customer";
		this.btnCustomerSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btn1856APrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn1856APrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn1856APrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn1856APrint.Name = "btn1856APrint";
		this.btn1856APrint.Size = new System.Drawing.Size(128, 22);
		this.btn1856APrint.Text = "1856A-ItemSlip";
		this.btn1856APrint.Click += new System.EventHandler(btn294APrint_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.BtnLoadOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadOrder.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadOrder.Name = "BtnLoadOrder";
		this.BtnLoadOrder.Size = new System.Drawing.Size(99, 22);
		this.BtnLoadOrder.Text = "Load Order";
		this.BtnLoadOrder.Click += new System.EventHandler(BtnLoadOrder_Click);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(88, 22);
		this.btnLoadGdn.Text = "Load Gdn";
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1310, 782);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 544);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1304, 235);
		this.panel17.TabIndex = 49;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1304, 208);
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
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1304, 208);
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
		this.panel18.Size = new System.Drawing.Size(1304, 27);
		this.panel18.TabIndex = 48;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1263, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
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
		this.panel10.Size = new System.Drawing.Size(1304, 487);
		this.panel10.TabIndex = 48;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 102);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1304, 385);
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1304, 385);
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
		this.panel7.Location = new System.Drawing.Point(0, 75);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1304, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1669, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(51, 30);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
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
		this.panel5.Size = new System.Drawing.Size(1304, 75);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label67);
		this.groupBox4.Controls.Add(this.RdCustomerHistorySearchByCode);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory);
		this.groupBox4.Controls.Add(this.RdCustomerHistorySearchByName);
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
		this.groupBox4.Location = new System.Drawing.Point(4, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1028, 70);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(968, 38);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(52, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(250, 18);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(48, 15);
		this.label67.TabIndex = 181;
		this.label67.Text = "Date To";
		this.RdCustomerHistorySearchByCode.AutoSize = true;
		this.RdCustomerHistorySearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByCode.Location = new System.Drawing.Point(659, 16);
		this.RdCustomerHistorySearchByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByCode.Name = "RdCustomerHistorySearchByCode";
		this.RdCustomerHistorySearchByCode.Size = new System.Drawing.Size(52, 19);
		this.RdCustomerHistorySearchByCode.TabIndex = 183;
		this.RdCustomerHistorySearchByCode.Text = "Code";
		this.RdCustomerHistorySearchByCode.UseVisualStyleBackColor = true;
		this.CmbCommissionAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance295;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance313).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance315).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance315).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance315).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance316).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Location = new System.Drawing.Point(765, 38);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Name = "CmbCommissionAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).TabIndex = 6;
		this.RdCustomerHistorySearchByName.AutoSize = true;
		this.RdCustomerHistorySearchByName.Checked = true;
		this.RdCustomerHistorySearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByName.Location = new System.Drawing.Point(601, 16);
		this.RdCustomerHistorySearchByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByName.Name = "RdCustomerHistorySearchByName";
		this.RdCustomerHistorySearchByName.Size = new System.Drawing.Size(57, 19);
		this.RdCustomerHistorySearchByName.TabIndex = 182;
		this.RdCustomerHistorySearchByName.TabStop = true;
		this.RdCustomerHistorySearchByName.Text = "Name";
		this.RdCustomerHistorySearchByName.UseVisualStyleBackColor = true;
		this.RdCustomerHistorySearchByName.CheckedChanged += new System.EventHandler(RdCustomerHistorySearchByName_CheckedChanged);
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(765, 18);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(108, 15);
		this.label72.TabIndex = 180;
		this.label72.Text = "Commission Agent";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(562, 18);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(34, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Party";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance268;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance274).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance275).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance279).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(562, 38);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 18);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance222).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(5, 38);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(250, 40);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(137, 18);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(462, 40);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(137, 40);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(462, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(363, 18);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(363, 40);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1304, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(176, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Sale Invoice History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1304, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1304, 25);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1318, 811);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "SaleInvoiceDirectConcrete";
		this.Text = "Sale Invoice Direct";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel13.ResumeLayout(false);
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
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSettlementDiscountAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefSalesMan).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVisitedbyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
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
		base.ResumeLayout(false);
	}
}
