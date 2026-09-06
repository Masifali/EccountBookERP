using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class InvfrmInvSaleInvoiceDirect : Form
{
	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtBrandItem = new DataTable();

	private DataTable dtCommDrAc = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtJobLotWithJobOrderAndItems = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private bool BranchImplemented;

	public static string FlagLoader = "";

	public int FlagForm;

	private bool WagesStatus;

	private bool WagesActiveOrInActive;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private bool FreightDebitToExpenses;

	private double SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int Id;

	private int DocumentTypeId = 99;

	private bool Approved;

	private int BaseCurrency;

	private double BaseRate;

	private bool HasMultiCurrencyFeature;

	private bool GetItemagainstwarehouseId;

	private bool CommissionDebitToExpenses;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool SaleCostingJobOrderWise;

	private int OutwardFreightAccountId;

	private string ScreenName = "";

	private bool BranchFeature;

	private double LedgerBalance;

	private UserAccount UserAccount = new UserAccount();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private frmLoadGDN LoadGDN;

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

	private TextBox txtEmptyBagsTotal;

	private Label label16;

	private TextBox txtEmptybagsUnit;

	private Label label15;

	private TextBox txtAmount;

	private Label label14;

	private Label label9;

	private Label label3;

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

	internal UltraCombo comstockparty;

	internal UltraCombo compartyreference;

	internal UltraCombo comPackUOM;

	internal UltraCombo CmbJobLot;

	internal UltraCombo comCropYear;

	internal UltraCombo comPackingType;

	internal UltraCombo CmbItemName;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	private Label label27;

	private DateTimePicker deliverystartdate;

	private DateTimePicker validate;

	private Label label44;

	private Label label45;

	private TextBox txtdeliverydays;

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

	internal UltraCombo comborderno;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grd;

	private TextBox txtWeightCutTotal;

	private Label label52;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private CheckBox ChkBok;

	private TextBox txtBillAmount;

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

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdDetail;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	private Label label1;

	internal UltraCombo CmbTransporterAc;

	private Label label2;

	private TextBox txtFreightAmount;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	internal UltraCombo CmbReserveWarehouse;

	private Label lblReserveWH;

	private ToolStripButton btn294APrint;

	internal UltraCombo txtCity;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Panel panel15;

	private Panel panel12;

	private Label label23;

	private Label label53;

	private Label label37;

	private Panel panel17;

	private Label label54;

	private Panel panel18;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label55;

	private Panel panel19;

	private Label label43;

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

	private Label label56;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private DateTimePicker datStockDate;

	private Panel panel20;

	private GroupBox groupBox4;

	private Button btnShowHistory;

	private Label label51;

	private Label label57;

	private Label label58;

	private Label label64;

	private Label label65;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel21;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label LabelCommissionDrCaption;

	internal UltraCombo CmbCommissionDebitAccount;

	private ToolStripButton btnPrint294B;

	private CheckBox chkPrint294B;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label66;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabFreight;

	private TabPage tabExpense;

	private TabPage tabGL;

	internal GridEX grdFreight;

	private Panel panel4;

	private Label label67;

	private TextBox txtLedgerBalance;

	private TextBox txtLedgerBalanceAfterBill;

	private Label label68;

	private Label label69;

	private CheckBox chkPrintSlip294A;

	private CheckBox chkCustomAccounts;

	internal UltraCombo cmbOrderCategory2;

	private Panel panel5;

	private RadioButton RadBrandName;

	private RadioButton RadBrandCode;

	internal UltraCombo CmbBrandItemName;

	private Label label70;

	internal UltraCombo CmbCategory;

	private Panel panel6;

	private RadioButton RadCategory;

	private RadioButton RadType;

	private Label label72;

	private ToolStripButton BtnItemSlip;

	private Button btnRecordsUpdate;

	public InvfrmInvSaleInvoiceDirect(UserAccount user)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1300, 750);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceDirectBranchWise"));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			if (SaleCostingJobOrderWise)
			{
				dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			}
			((Control)(object)CmbTransporterAc).Visible = false;
			txtFreightAmount.Visible = false;
			label1.Visible = false;
			label2.Visible = false;
			lblReserveWH.Visible = false;
			((Control)(object)CmbReserveWarehouse).Visible = false;
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnSaleInvoiceDirect"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == DocumentTypeId);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
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
					txtFreightAmount.Visible = true;
					label1.Visible = true;
					label2.Visible = true;
					OutwardFreightAccountId = value;
				}
			}
			List<ConfigrationsAllocation> CommDrlist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DebitAmountChargetoExpenseAcOfCommission").ToList();
			if (CommDrlist.Count > 0)
			{
				CommissionDebitToExpenses = Conversion.ToBool((object)CommDrlist[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> FreightDrlist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DebitAmountChargetoExpenseAcFreightGrid").ToList();
			if (FreightDrlist.Count > 0)
			{
				FreightDebitToExpenses = Conversion.ToBool((object)FreightDrlist[0].ConfigKey.ToString());
			}
			if (CommissionDebitToExpenses)
			{
				((Control)(object)CmbCommissionDebitAccount).Visible = true;
				LabelCommissionDrCaption.Visible = true;
				txtCommissionRemarks.Height = 51;
			}
			else
			{
				((Control)(object)CmbCommissionDebitAccount).Visible = false;
				LabelCommissionDrCaption.Visible = false;
				txtCommissionRemarks.Height = 78;
			}
			dtViewAttachments.Columns.Add("AttchmentPath");
			ScreenName = base.Name;
			if (FlagForm == 1)
			{
				ScreenName = "SaleInvoicepartyStockReserve";
				btnLoadGdn.Visible = false;
				lblReserveWH.Visible = true;
				((Control)(object)CmbReserveWarehouse).Visible = true;
			}
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnSlip.Enabled = formright.DoHavePrintRights;
			btn294APrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
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
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtjob.Columns.Add("Id", typeof(int));
			dtjob.Columns.Add("JobLotDescription");
			dtjob.Columns.Add("BranchId", typeof(int));
			dtjob.Columns.Add("BranchName", typeof(string));
			dtCommDrAc.Columns.Add("GlAccountId", typeof(int));
			dtCommDrAc.Columns.Add("AccountTitle", typeof(string));
			dtCommDrAc.Columns.Add("SupplierCustomerId", typeof(int));
			CurrencyFill();
			CommissionDebitAccountFill();
			MultiCurrencyFeature();
			OtherItemsBind();
			OrderCategory_II();
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			TransporterAcFill();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("InventoryParentCategoriesId");
			dtitem.Columns.Add("ItemCategoryId");
			dtitem.Columns.Add("ItemCategory");
			dtitem.Columns.Add("ItemTypeId");
			dtitem.Columns.Add("ItemType");
			dtitem.Columns.Add("ProductionStageId");
			dtitem.Columns.Add("ProductionStage");
			dtBrandItem.Columns.Add("Id", typeof(int));
			dtBrandItem.Columns.Add("ItemName");
			dtBrandItem.Columns.Add("ItemCode");
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("InvGdnId", typeof(int));
			dtGrid.Columns.Add("InvGdnDetailId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("SaleOrderId", typeof(int));
			dtGrid.Columns.Add("SaleOrderNo", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("BrandItemId", typeof(int));
			dtGrid.Columns.Add("BrandItemCode");
			dtGrid.Columns.Add("BrandItemName");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("PackEquivalent", typeof(double));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("EmptyBags", typeof(double));
			dtGrid.Columns.Add("EmptyBagsTotal", typeof(double));
			dtGrid.Columns.Add("WeightCut", typeof(double));
			dtGrid.Columns.Add("WeightCutTotal", typeof(double));
			dtGrid.Columns.Add("AddLss", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutTotal", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(decimal));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse");
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("Freight", typeof(double));
			dtGrid.Columns.Add("PurchaseGLAC", typeof(double));
			dtGrid.Columns.Add("ReserveWarehouseId", typeof(int));
			dtGrid.Columns.Add("ReserveWarehouse");
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName");
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Columns.Add("InvGdnId");
			dtFreight.Columns.Add("GlAccountId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("GlAccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			bindWareHouse();
			BinCity();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			ItemCategoryOrTypeBind();
			ItemdtFillFromAll(Conversion.ToInt(CmbCategory.Value));
			ItemDetailFill();
			BrandDtFillFromGlobal();
			BrandItemNameBind();
			Project();
			PackingType();
			JobLotDtFillFromGlobalAndBind();
			braches();
			cropyear();
			DocumentNo();
			BranchSrNoFill();
			ConfigurationDefault();
			((UltraGridBase)comproject).Rows[0].Activate();
			((UltraGridBase)combranches).Rows[0].Activate();
			DocDate.Focus();
			ChkPrintslip.Checked = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			HistoryCombosBranchFill();
			HistoryCombosFill(ValidateBranch: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindWareHouse()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(comWarehouse.Value);
			int Idr = Conversion.ToInt(CmbReserveWarehouse.Value);
			DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = ((BranchFeature && BranchImplemented) ? UserAccount.BranchesId : 0)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comWarehouse, "Id", "WareHouseName", "WareHouse ", ZeroIndex: true);
				((UltraGridBase)comWarehouse).Rows[1].Activate();
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)comWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						comWarehouse.Value = Id;
					}
					else
					{
						((Control)(object)comWarehouse).Text = string.Empty;
					}
				}
				if (FlagForm != 1)
				{
					return;
				}
				DDL.BindDDL(dt, CmbReserveWarehouse, "Id", "WareHouseName", "Reserve Ware House ", ZeroIndex: true);
				((UltraGridBase)CmbReserveWarehouse).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbReserveWarehouse).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
				if (Idr > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idr))
					{
						CmbReserveWarehouse.Value = Idr;
					}
					else
					{
						((Control)(object)CmbReserveWarehouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)comWarehouse).DataSource = null;
				((Control)(object)comWarehouse).Text = string.Empty;
				((UltraGridBase)CmbReserveWarehouse).DataSource = null;
				((Control)(object)CmbReserveWarehouse).Text = string.Empty;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void BinCity()
	{
		try
		{
			int Id = Conversion.ToInt(txtCity.Value);
			DataTable dt = CommonServices.CityGetAllService();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, txtCity, "Id", "CityName", "City Name ", false);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> obj = (from r in clsGlobalVariables.globalSupplierCustomer.AsEnumerable()
				where r.CustomerGroupId != 7
				select r).ToList();
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in obj.AsEnumerable()
					where r.PartyTypeId == 2
					select r).ToList();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtSupplier.Rows.Add(Conversion.ToInt((object)lst[i].Id), Conversion.ToString((object)lst[i].CompanyName), Conversion.ToString((object)lst[i].PartyCode), Conversion.ToInt((object)lst[i].GlAccountId), Conversion.ToInt((object)lst[i].CityId), Conversion.ToString((object)lst[i].CityName), Conversion.ToString((object)lst[i].MobilePersonal));
					}
				}
			}
			else if (obj != null && obj.Count > 0)
			{
				for (int j = 0; j < obj.Count; j++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[j].Id), Conversion.ToString((object)obj[j].CompanyName), Conversion.ToString((object)obj[j].PartyCode), Conversion.ToInt((object)obj[j].GlAccountId), Conversion.ToInt((object)obj[j].CityId), Conversion.ToString((object)obj[j].CityName), Conversion.ToString((object)obj[j].MobilePersonal));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(comsupplier, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(comsupplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(combcommAgent, dtSupplier, "Id", "CompanyName", "Commission Agent", AllColumns: true, Conversion.ToInt(combcommAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dataTable = dtSupplier;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionDebitAccountFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCommissionDebitAccount.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = SupplierCustomer.GetVendorsAndCustomersForTransporter(UserAccount.OrganizationId, UserAccount.CompanyId);
				if (dt.Rows.Count > 0)
				{
					dtCommDrAc.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtCommDrAc.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToString(dt.Rows[i]["CompanyName"]), Conversion.ToInt(dt.Rows[i]["Id"]));
					}
				}
			}
			else
			{
				DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,14,20,21");
				if (dt2.Rows.Count > 0)
				{
					dtCommDrAc.Clear();
					for (int j = 0; j < dt2.Rows.Count; j++)
					{
						dtCommDrAc.Rows.Add(dt2.Rows[j]["Id"], dt2.Rows[j]["AccountTitle"], 0);
					}
				}
			}
			if (dtCommDrAc.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtCommDrAc, CmbCommissionDebitAccount, "GlAccountId", "AccountTitle", "Comm Dr Account ", ZeroIndex: true);
			((UltraGridBase)CmbCommissionDebitAccount).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (Id > 0)
			{
				if (dtCommDrAc.AsEnumerable().Any((DataRow row) => row.Field<int>("GlAccountId") == Id))
				{
					CmbCommissionDebitAccount.Value = Id;
				}
				else
				{
					((Control)(object)CmbCommissionDebitAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryOrTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			if (RadCategory.Checked)
			{
				foreach (var category in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemCategory) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemCategory)
					select new
					{
						Id = Conversion.ToInt((object)row.ItemCategoryId),
						Description = row.ItemCategory
					}).Distinct().ToList())
				{
					dt.Rows.Add(category.Id, category.Description);
				}
			}
			else if (RadType.Checked)
			{
				foreach (var type in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemType) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemType)
					select new
					{
						Id = Conversion.ToInt((object)row.ItemTypeId),
						Description = row.ItemType
					}).Distinct().ToList())
				{
					dt.Rows.Add(type.Id, type.Description);
				}
			}
			int ID = Conversion.ToInt(CmbCategory.Value);
			if (dt.Rows.Count > 0)
			{
				string DisplayName = (RadCategory.Checked ? "Item Category" : "ItemType");
				InfragisticsHelper.BindAndRetainSelection(CmbCategory, dt, "Id", "Description", DisplayName, AllColumns: true, ID, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbCategory).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFillFromAll(int CategoryOrTypeId = 0)
	{
		try
		{
			dtitem.Rows.Clear();
			if (GetItemagainstwarehouseId)
			{
				DataTable dt = CommonServices.GetItemsFromWarehouseAllocationByWareHouseId(Conversion.ToInt(comWarehouse.Value));
				if (dt.Rows.Count <= 0)
				{
					return;
				}
				{
					foreach (DataRow row2 in dt.Rows)
					{
						dtitem.Rows.Add(row2["Id"], row2["ItemName"], row2["ItemCodeNew"], row2["InventoryParentCategoriesId"], row2["ItemCategoryId"], row2["ItemCategory"], row2["ItemTypeId"], row2["ItemType"], row2["ItemProductionStageId"], row2["productionStageName"]);
					}
					return;
				}
			}
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row3 in dtItemAll)
			{
				int ItemTypeId = Conversion.ToInt((object)row3.ItemTypeId);
				int ItemCategoryId = Conversion.ToInt((object)row3.ItemCategoryId);
				if (CategoryOrTypeId == 0 || (RadCategory.Checked && CategoryOrTypeId == ItemCategoryId) || (RadType.Checked && CategoryOrTypeId == ItemTypeId))
				{
					dtitem.Rows.Add(row3.Id, row3.ItemName, row3.ItemCode, row3.InventoryParentCategoriesId, row3.ItemCategoryId, row3.ItemCategory, row3.ItemTypeId, row3.ItemType, row3.ItemProductionStageId, row3.productionStageName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtitem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtitem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[8].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemDetailFill();
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
			dt.Rows.Add(3, "Comm Weight");
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
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, combcommUOM, "Id", "type", "UOM", ZeroIndex: false);
				((UltraGridBase)combcommUOM).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingType()
	{
		try
		{
			int Id = Conversion.ToInt(comPackingType.Value);
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, comPackingType, "Id", "PackTypeDesc", "Pack Type ", true);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotDtFillFromGlobalAndBind()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		try
		{
			dtjob.Clear();
			if (SaleCostingJobOrderWise)
			{
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow dr in dtJobLotWithJobOrderAndItems.Rows)
				{
					int ID = Conversion.ToInt(dr["Id"]);
					if (!Ids.Contains(ID))
					{
						dtjob.Rows.Add(ID, dr["JobLotDescription"], dr["BranchId"], dr["BranchName"]);
						Ids.Add(ID);
					}
				}
			}
			else
			{
				DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = ((BranchFeature && BranchImplemented) ? UserAccount.BranchesId : 0)
				});
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr2 in dt.Rows)
					{
						int ID2 = Conversion.ToInt(dr2["Id"]);
						dtjob.Rows.Add(ID2, dr2["JobLotDescription"], dr2["BranchId"], dr2["BranchName"]);
					}
				}
			}
			int Id = Conversion.ToInt(CmbJobLot.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dtjob, "Id", "JobLotDescription", "JobLot", AllColumns: true, Id, insertDefaultRow: true);
			if (dtjob.Rows.Count > 0)
			{
				((UltraGridBase)CmbJobLot).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbJobLot).DisplayLayout.Bands[0].Columns[3].Hidden = !BranchFeature || (BranchFeature && BranchImplemented);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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

	private void PackUOM()
	{
		try
		{
			string Packuom = ((Control)(object)comPackUOM).Text;
			string Rateuom = ((Control)(object)comRateUOM).Text;
			((Control)(object)comPackUOM).Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
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
			MessageBox.Show(ex.Message);
		}
	}

	private void cropyear()
	{
		try
		{
			int Id = Conversion.ToInt(comCropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, comCropYear, "Id", "CropYear", "Crop Year", true);
			if (Id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comCropYear.Value = Id;
				}
				else
				{
					((Control)(object)comCropYear).Text = string.Empty;
				}
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
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

	private void BranchSrNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int code = InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, DocumentTypeId);
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
			dtAccountlst.Rows.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = SupplierCustomer.GetVendorsAndCustomersForTransporter(UserAccount.OrganizationId, UserAccount.CompanyId);
				if (dt.Rows.Count > 0)
				{
					dtAccountlst.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtAccountlst.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
					}
				}
				DDL.BindDDL(dtAccountlst, CmbTransporterAc, "SupplierCustomerId", "AccountTitle", "Account Title", ZeroIndex: false);
			}
			else
			{
				if (FreightDebitToExpenses)
				{
					DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
					if (dt2.Rows.Count > 0)
					{
						for (int j = 0; j < dt2.Rows.Count; j++)
						{
							dtAccountlst.Rows.Add(dt2.Rows[j]["Id"], 0, dt2.Rows[j]["AccountTitle"]);
						}
					}
				}
				else
				{
					DataTable dt3 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
					if (dt3.Rows.Count > 0)
					{
						for (int k = 0; k < dt3.Rows.Count; k++)
						{
							dtAccountlst.Rows.Add(dt3.Rows[k]["Id"], 0, dt3.Rows[k]["AccountTitle"]);
						}
					}
				}
				DDL.BindDDL(dtAccountlst, CmbTransporterAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
			((UltraGridBase)CmbTransporterAc).DisplayLayout.Bands[0].Columns[1].Hidden = true;
			((UltraGridBase)CmbTransporterAc).DisplayLayout.Bands[0].Columns[2].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryCombosFill();
				return;
			}
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((UltraGridBase)CmbCustomerHistory).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderCategory_II()
	{
		try
		{
			int ID = Conversion.ToInt(cmbOrderCategory2.Value);
			DataTable dt = CommonServices.GetLookupsByTypeIdDt(19);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbOrderCategory2, "Id", "LookupName", "Category-II", false);
				CommonServices.SetComboValue(cmbOrderCategory2, "Id", ID, dt, ActivateRow: true);
			}
			else
			{
				((Control)(object)cmbOrderCategory2).Text = string.Empty;
				((UltraGridBase)cmbOrderCategory2).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BrandDtFillFromGlobal()
	{
		try
		{
			dtBrandItem.Clear();
			List<getGlobalAllBrands> dtItemAll = clsGlobalVariables.getGlobalAllBrands.ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllBrands row in dtItemAll)
			{
				dtBrandItem.Rows.Add(row.Id, row.BrandName, row.BrandCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrandItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbBrandItemName.Value);
			string ValueMember = (RadBrandName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadBrandName.Checked ? "Brand Name" : "Brand Code");
			InfragisticsHelper.BindAndRetainSelection(CmbBrandItemName, dtBrandItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadBrandName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			BrandItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemdtFillFromAll(Conversion.ToInt(CmbCategory.Value));
			ItemDetailFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadCategory_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemCategoryOrTypeBind();
			ItemdtFillFromAll(Conversion.ToInt(CmbCategory.Value));
			ItemDetailFill();
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
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label59.Visible = false;
				label60.Visible = false;
				label61.Visible = false;
			}
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
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				comCropYear.Value = CropYearId;
			}
			List<ConfigrationsAllocation> lstPackingType = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (lstPackingType.Count > 0)
			{
				int PackingTypeId = Conversion.ToInt((object)lstPackingType[0].ConfigKey.ToString());
				comPackingType.Value = PackingTypeId;
			}
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
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0 && dtjob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobId))
			{
				CmbJobLot.Value = jobId;
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
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
			List<ConfigrationsAllocation> CommDrlist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DebitAmountChargetoExpenseAcOfCommission").ToList();
			if (CommDrlist.Count > 0)
			{
				CommissionDebitToExpenses = Conversion.ToBool((object)CommDrlist[0].ConfigKey.ToString());
			}
			if (CommissionDebitToExpenses)
			{
				((Control)(object)CmbCommissionDebitAccount).Visible = true;
				LabelCommissionDrCaption.Visible = true;
				txtCommissionRemarks.Height = 51;
			}
			else
			{
				((Control)(object)CmbCommissionDebitAccount).Visible = false;
				LabelCommissionDrCaption.Visible = false;
				txtCommissionRemarks.Height = 78;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1608",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
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
					double amount = Conversion.ToDouble(obj.Cells["ItemAmount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
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

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		try
		{
			double CurrStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(comWarehouse.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)comCropYear).Text.Trim()),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				PackingTypeId = Conversion.ToInt(comPackingType.Value),
				stockUOM = Conversion.ToInt(comPackUOM.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
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
			PackUOM();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		try
		{
			if (comborderno.Value != null)
			{
				Conversion.ToInt(comborderno.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "Delete";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "Add New";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "Add";
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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
			BillAmount();
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
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else if (e.Column.Key == "Amount" && item.Cells["Amount"].Value.ToString() != string.Empty)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				double Amount2 = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount2;
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
			FreightProportion();
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
		dtGrdGL.Rows.Add(0, 0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
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
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "Delete";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "Add New";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "Add";
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
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
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			item.BeginEdit();
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
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
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "AccountId")
			{
				if (!SubsidiaryAccountAllownOnVouchers && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Customer Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid();
			}
			item.EndEdit();
			grdGLedger.UpdateData();
			BillAmount();
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
				string selectedAcId = Conversion.ToString((object)r.Cells["AccountId"].Text);
				if (selectedAcId != "0" && selectedAcId != string.Empty)
				{
					DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
					if (matchingRows.Length != 0)
					{
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
						r.EndEdit();
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Selected Account Can not be Same As Supplier Account");
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = 0;
						r.Cells["AccountId"].Value = 0;
						r.EndEdit();
						return;
					}
				}
				else if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
				{
					MessageBox.Show("Selected Account Can not be Same As Supplier Account");
					r.BeginEdit();
					r.Cells["AccountId"].Value = 0;
					r.Cells["GlAccountId"].Value = 0;
					r.EndEdit();
					return;
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemIdExistsInJobLot(int itemId, string itemName, int jobLotId, string jobLotName, int rowNo = 0)
	{
		try
		{
			if (!SaleCostingJobOrderWise || dtJobLotWithJobOrderAndItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobLotId && row.Field<int>("ItemId") == itemId))
			{
				return;
			}
			throw new Exception((rowNo == 0) ? ("The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "'.") : $"The selected item '{itemName}' does not exist against the job lot '{jobLotName}' in detail grid row no: {rowNo}.");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			ItemIdExistsInJobLot(Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((Control)(object)CmbItemName).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text));
			if (grd.RowCount > 0 && GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "InvGdnId"))
			{
				throw new Exception("You can not add manual Record because record against Gdn exist in Grid");
			}
			if (!FormValidationDetila())
			{
				return;
			}
			int BranchId = 0;
			string BranchName = "";
			int JobBranchId = 0;
			string JobBranchName = "";
			if (((UltraGridBase)comWarehouse).ActiveRow != null && Conversion.ToInt(comWarehouse.Value) > 0)
			{
				BranchId = Conversion.ToInt(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchId"].Value);
				BranchName = Conversion.ToString(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchName"].Value);
			}
			if (((UltraGridBase)CmbJobLot).ActiveRow != null && Conversion.ToInt(CmbJobLot.Value) > 0)
			{
				JobBranchId = Conversion.ToInt(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchId"].Value);
				JobBranchName = Conversion.ToString(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchName"].Value);
			}
			if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
			{
				MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			int BrandId = Conversion.ToInt(CmbBrandItemName.Value);
			string BrandName = "";
			string BrandCode = "";
			if (((UltraGridBase)CmbBrandItemName).ActiveRow != null && BrandId > 0)
			{
				BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemCode"].Value);
				BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemName"].Value);
			}
			dtGrid.Rows.Add(0, 0, 0, 0, 0, 0, Conversion.ToInt(CmbItemName.Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), BrandId, BrandCode, BrandName, Conversion.ToString((object)((Control)(object)comCropYear).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToInt(comPackingType.Value), Conversion.ToString((object)((Control)(object)comPackingType).Text), Conversion.ToInt(comPackUOM.Value), Conversion.ToString((object)((Control)(object)comPackUOM).Text), Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtEmptybagsUnit.Text), Conversion.ToDouble((object)txtEmptyBagsTotal.Text), Conversion.ToDouble((object)txtwtcut.Text), Conversion.ToDouble((object)txtWeightCutTotal.Text), Conversion.ToDouble((object)txtAddLss.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtStockWeight.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToInt(comRateUOM.Value), Conversion.ToString((object)((Control)(object)comRateUOM).Text), Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtratecut.Text), Conversion.ToDouble((object)txtratecuttotal.Text), Conversion.ToDouble((object)txtAmount.Text), 0, Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString(), Conversion.ToInt((object)txtgatepassno.Text), Conversion.ToString((object)txtvehicleno.Text), 0, 0, 0, 0, 0, 0, Conversion.ToInt(CmbReserveWarehouse.Value), Conversion.ToString((object)((Control)(object)CmbReserveWarehouse).Text), Conversion.ToInt(txtCity.Value), Conversion.ToString((object)((Control)(object)txtCity).Text), BranchId, BranchName);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			TotalCommissionAmount();
			CommissionProportion();
			ExpProportion();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
			ResetDetail();
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				((Control)(object)CmbCategory).Text = "";
				CmbCategory_Leave(null, null);
				updateDetailIndex = r.RowIndex;
				if (Conversion.ToInt(r.Cells["InvGdnId"].Value) > 0)
				{
					DisableFields();
				}
				comWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				ItemdtFillFromAll();
				ItemDetailFill();
				CmbItemName.Value = r.Cells["ItemId"].Value.ToString();
				CmbBrandItemName.Value = r.Cells["BrandItemId"].Value.ToString();
				PackUOM();
				((Control)(object)comCropYear).Text = r.Cells["CropYear"].Text.ToString();
				CmbJobLot.Value = r.Cells["JobLotId"].Value.ToString();
				comPackingType.Value = r.Cells["PackingTypeId"].Value.ToString();
				txtQty.Text = Conversion.ToDouble(r.Cells["ItemQty"].Value).ToString("#,##0.##");
				comPackUOM.Value = r.Cells["PackUOMId"].Value.ToString();
				txtGrossWeight.Text = Conversion.ToDouble(r.Cells["GrossWeight"].Value).ToString("#,##0.##");
				txtEmptybagsUnit.Text = Conversion.ToDouble(r.Cells["EmptyBags"].Value).ToString("#,##0.##");
				txtEmptyBagsTotal.Text = Conversion.ToDouble(r.Cells["EmptyBagsTotal"].Value).ToString("#,##0.##");
				txtwtcut.Text = Conversion.ToDouble(r.Cells["WeightCut"].Value).ToString("#,##0.##");
				txtWeightCutTotal.Text = Conversion.ToDouble(r.Cells["WeightCutTotal"].Value).ToString("#,##0.##");
				txtAddLss.Text = Conversion.ToDouble(r.Cells["AddLss"].Value).ToString("#,##0.##");
				txtNetBillWeight.Text = Conversion.ToDouble(r.Cells["NetBillWeight"].Value).ToString("#,##0.##");
				txtStockWeight.Text = Conversion.ToDouble(r.Cells["StockWeight"].Value).ToString("#,##0.##");
				txtRate.Text = Conversion.ToDouble(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				if (Conversion.ToInt(r.Cells["RateUOMId"].Value) > 0)
				{
					comRateUOM.Value = Conversion.ToInt(r.Cells["RateUOMId"].Value);
				}
				txtratecut.Text = Conversion.ToDouble(r.Cells["RateCut"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtratecuttotal.Text = Conversion.ToDouble(r.Cells["RateCutTotal"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtAmount.Text = Conversion.ToDouble(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtgpdate.Text = r.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				CmbReserveWarehouse.Value = r.Cells["ReserveWarehouseId"].Value.ToString();
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
			ItemIdExistsInJobLot(Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((Control)(object)CmbItemName).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text));
			if (!FormValidationDetila())
			{
				return;
			}
			int BranchId = 0;
			string BranchName = "";
			int JobBranchId = 0;
			string JobBranchName = "";
			if (((UltraGridBase)comWarehouse).ActiveRow != null && Conversion.ToInt(comWarehouse.Value) > 0)
			{
				BranchId = Conversion.ToInt(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchId"].Value);
				BranchName = Conversion.ToString(((UltraDropDownBase)comWarehouse).SelectedRow.Cells["BranchName"].Value);
			}
			if (((UltraGridBase)CmbJobLot).ActiveRow != null && Conversion.ToInt(CmbJobLot.Value) > 0)
			{
				JobBranchId = Conversion.ToInt(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchId"].Value);
				JobBranchName = Conversion.ToString(((UltraDropDownBase)CmbJobLot).SelectedRow.Cells["BranchName"].Value);
			}
			if (BranchFeature && !BranchImplemented && BranchId != JobBranchId)
			{
				MessageBox.Show("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + BranchName + "' and JobLot is of Branch '" + JobBranchName + "'", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			int BrandId = Conversion.ToInt(CmbBrandItemName.Value);
			string BrandName = "";
			string BrandCode = "";
			if (((UltraGridBase)CmbBrandItemName).ActiveRow != null && BrandId > 0)
			{
				BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemCode"].Value);
				BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemName"].Value);
			}
			dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
			dtGrid.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
			dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
			dtGrid.Rows[updateDetailIndex]["BrandItemId"] = BrandId;
			dtGrid.Rows[updateDetailIndex]["BrandItemCode"] = BrandCode;
			dtGrid.Rows[updateDetailIndex]["BrandItemName"] = BrandName;
			dtGrid.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)comCropYear).Text);
			dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
			dtGrid.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
			dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(comPackingType.Value);
			dtGrid.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)comPackingType).Text);
			dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtQty.Text);
			dtGrid.Rows[updateDetailIndex]["PackUOMId"] = Conversion.ToInt(comPackUOM.Value);
			dtGrid.Rows[updateDetailIndex]["PackUOM"] = Conversion.ToString((object)((Control)(object)comPackUOM).Text);
			dtGrid.Rows[updateDetailIndex]["PackEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			dtGrid.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
			dtGrid.Rows[updateDetailIndex]["EmptyBags"] = Conversion.ToDouble((object)txtEmptybagsUnit.Text);
			dtGrid.Rows[updateDetailIndex]["EmptyBagsTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			dtGrid.Rows[updateDetailIndex]["WeightCut"] = Conversion.ToDouble((object)txtwtcut.Text);
			dtGrid.Rows[updateDetailIndex]["WeightCutTotal"] = Conversion.ToDouble((object)txtWeightCutTotal.Text);
			dtGrid.Rows[updateDetailIndex]["AddLss"] = Conversion.ToDouble((object)txtAddLss.Text);
			dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = Conversion.ToDouble((object)txtNetBillWeight.Text);
			dtGrid.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtStockWeight.Text);
			dtGrid.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRate.Text);
			dtGrid.Rows[updateDetailIndex]["RateUOMId"] = Conversion.ToInt(comRateUOM.Value);
			dtGrid.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToString((object)((Control)(object)comRateUOM).Text);
			dtGrid.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
			dtGrid.Rows[updateDetailIndex]["RateCut"] = Conversion.ToDouble((object)txtratecut.Text);
			dtGrid.Rows[updateDetailIndex]["RateCutTotal"] = Conversion.ToDouble((object)txtratecuttotal.Text);
			dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
			dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
			dtGrid.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)comWarehouse).Text);
			dtGrid.Rows[updateDetailIndex]["ReserveWarehouseId"] = Conversion.ToInt(CmbReserveWarehouse.Value);
			dtGrid.Rows[updateDetailIndex]["ReserveWarehouse"] = Conversion.ToString((object)((Control)(object)CmbReserveWarehouse).Text);
			dtGrid.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
			dtGrid.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)txtCity).Text);
			dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString();
			dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
			dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
			dtGrid.Rows[updateDetailIndex]["BranchId"] = BranchId;
			dtGrid.Rows[updateDetailIndex]["BranchName"] = BranchName;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			TotalCommissionAmount();
			CommissionProportion();
			ExpProportion();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvGdnId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["InvGdnDetailId"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["SaleOrderNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["BrandItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["ReserveWarehouseId"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["GdnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["BrandItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EmptyBags"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["EmptyBagsTotal"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["AddLss"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateCutTotal"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Journal"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grd.RootTable.Columns["ReserveWarehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			if (FlagForm != 1)
			{
				grd.RootTable.Columns["ReserveWarehouse"].Visible = false;
			}
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmptyBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBags"].FormatString = "#,##0.##";
			grd.RootTable.Columns["EmptyBags"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBagsTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBagsTotal"].FormatString = "#,##0.##";
			grd.RootTable.Columns["EmptyBagsTotal"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCut"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AddLss"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLss"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLss"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AddLss"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
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
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_EditingCell(object sender, EditingCellEventArgs e)
	{
	}

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			TotalCommissionAmount();
			FreightProportion();
			BillAmount();
			grd.UpdateData();
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
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				item.Delete();
			}
			grdGLedger.UpdateData();
			TotalCommissionAmount();
			FreightProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		try
		{
			dtFreight.Rows.Add(0, 0, 0, 0, 0, "");
			grdFreight.DataSource = dtFreight;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["InvGdnId"].Visible = false;
			if (!FreightDebitToExpenses)
			{
				grdFreight.RootTable.Columns["Debit"].Visible = false;
			}
			else
			{
				grdFreight.RootTable.Columns["Debit"].Visible = true;
			}
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns["Transporter"].Width = 120;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Add"].Position = 0;
			grdFreight.RootTable.Columns["Delete"].Position = 1;
			grdFreight.FrozenColumns = 2;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					int selectedAcId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
					if (selectedAcId != 0)
					{
						DataRow[] matchingRows = dtAccountlst.Select("Id=" + selectedAcId);
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["Transporter"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			SupCustIdUpdateforFrieghtGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				dtFreight.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
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
			FreightProportion();
			BillAmount();
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
					string selectedAcId = Conversion.ToString((object)item.Cells["Transporter"].Text);
					if (selectedAcId != string.Empty && selectedAcId != "0")
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
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

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Freight" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Freight"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "Transporter")
			{
				Conversion.ToInt(item.Cells["InvGdnId"].Value);
				_ = 0;
				SupCustIdUpdateforFrieghtGrid();
			}
			FreightProportion();
			BillAmount();
			grdFreight.UpdateData();
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
			if (e.KeyCode == Keys.F1)
			{
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter")
				{
					if (SubsidiaryAccountAllownOnVouchers)
					{
						GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						obj.BringToFront();
						r.Cells["Transporter"].Value = obj.ReturnId;
					}
					else
					{
						GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						r.Cells["Transporter"].Value = obj2.ReturnId;
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtGrdGL.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
					dtFreight.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Add")
				{
					AddRowInFreightGrid();
				}
				grdFreight.UpdateData();
				FreightProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtFreight.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
					grdFreight.UpdateData();
					FreightProportion();
					BillAmount();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInFreightGrid();
				grdFreight.UpdateData();
				FreightProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private bool FormValidation()
	{
		try
		{
			if (((UltraGridBase)combranches).ActiveRow == null || Conversion.ToInt(combranches.Value) == 0)
			{
				MessageBox.Show("Branch Field is Required");
				combranches.Focus();
				return false;
			}
			if (((UltraGridBase)comproject).ActiveRow == null || Conversion.ToInt(comproject.Value) == 0)
			{
				MessageBox.Show("Project Field is Required");
				comproject.Focus();
				return false;
			}
			if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
			{
				MessageBox.Show("CustomerName Field is Required");
				comsupplier.Focus();
				return false;
			}
			if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
			{
				MessageBox.Show("DocNo Field is Required");
				txtdocno.Focus();
				return false;
			}
			DataTable dtOrderCategory2 = (DataTable)((UltraGridBase)cmbOrderCategory2).DataSource;
			if (dtOrderCategory2 != null && dtOrderCategory2.Rows.Count > 0 && Conversion.ToInt(cmbOrderCategory2.Value) == 0)
			{
				MessageBox.Show("Other Category Field is Required");
				cmbOrderCategory2.Focus();
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
			if (txtduedays.Text.Trim() == "" || txtduedays.Text.Trim() == null)
			{
				MessageBox.Show("Due Days Field is Required");
				txtduedays.Focus();
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private bool FormValidationDetila()
	{
		try
		{
			if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
			{
				MessageBox.Show("Item Name Field is Required");
				CmbItemName.Focus();
				return false;
			}
			if (((UltraGridBase)comCropYear).ActiveRow == null || Conversion.ToInt(comCropYear.Value) == 0)
			{
				MessageBox.Show("Crop Year Field is Required");
				comCropYear.Focus();
				return false;
			}
			if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
			{
				MessageBox.Show("Job/Lot Field is Required");
				CmbJobLot.Focus();
				return false;
			}
			if (((UltraGridBase)comPackingType).ActiveRow == null || Conversion.ToInt(comPackingType.Value) == 0)
			{
				MessageBox.Show("Packing Type Field is Required");
				comPackingType.Focus();
				return false;
			}
			if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
			{
				MessageBox.Show("Qty Field is Required");
				txtQty.Focus();
				return false;
			}
			if (((UltraGridBase)comPackUOM).ActiveRow == null || Conversion.ToInt(comPackUOM.Value) == 0)
			{
				MessageBox.Show("Pack Unit Field is Required");
				comPackUOM.Focus();
				return false;
			}
			if (txtGrossWeight.Text.Trim() == "" || txtGrossWeight.Text.Trim() == "0")
			{
				MessageBox.Show("Gross Weight Field is Required");
				txtGrossWeight.Focus();
				return false;
			}
			if (txtNetBillWeight.Text.Trim() == "" || txtNetBillWeight.Text.Trim() == "0")
			{
				MessageBox.Show("Net Bill Weight Field is Required");
				txtNetBillWeight.Focus();
				return false;
			}
			if (txtStockWeight.Text.Trim() == "" || txtStockWeight.Text.Trim() == "0")
			{
				MessageBox.Show("Stock Weight Field is Required");
				txtStockWeight.Focus();
				return false;
			}
			if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
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
			if (FlagForm == 1 && (((UltraGridBase)CmbReserveWarehouse).ActiveRow == null || Conversion.ToInt(CmbReserveWarehouse.Value) == 0))
			{
				MessageBox.Show("Reserve Warehouse Field is Required");
				CmbReserveWarehouse.Focus();
				return false;
			}
			if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
			{
				MessageBox.Show("Warehouse Field is Required");
				comWarehouse.Focus();
				return false;
			}
			if (((UltraGridBase)txtCity).ActiveRow == null || Conversion.ToInt(txtCity.Value) == 0)
			{
				MessageBox.Show("city Field is Required");
				txtCity.Focus();
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = false;
			((Control)(object)CmbCategory).Enabled = false;
			((Control)(object)CmbItemName).Enabled = false;
			((Control)(object)comCropYear).Enabled = false;
			((Control)(object)comPackUOM).Enabled = false;
			((Control)(object)comPackingType).Enabled = false;
			((Control)(object)CmbJobLot).Enabled = false;
			txtQty.Enabled = false;
			txtGrossWeight.Enabled = false;
			txtEmptybagsUnit.Enabled = false;
			txtEmptyBagsTotal.Enabled = false;
			txtwtcut.Enabled = false;
			txtWeightCutTotal.Enabled = false;
			txtAddLss.Enabled = false;
			txtNetBillWeight.Enabled = false;
			txtStockWeight.Enabled = false;
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
			((Control)(object)CmbCategory).Enabled = true;
			((Control)(object)CmbItemName).Enabled = true;
			((Control)(object)comCropYear).Enabled = true;
			((Control)(object)comPackUOM).Enabled = true;
			((Control)(object)comPackingType).Enabled = true;
			((Control)(object)CmbJobLot).Enabled = true;
			txtQty.Enabled = true;
			txtGrossWeight.Enabled = true;
			txtEmptybagsUnit.Enabled = true;
			txtEmptyBagsTotal.Enabled = true;
			txtwtcut.Enabled = true;
			txtWeightCutTotal.Enabled = true;
			txtAddLss.Enabled = true;
			txtNetBillWeight.Enabled = true;
			txtStockWeight.Enabled = true;
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
			FlagLoader = "";
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			Id = 0;
			VoucherHeadId = 0;
			Approved = false;
			((Control)(object)comsupplier).Text = string.Empty;
			txtSupplierGLId.Text = string.Empty;
			((Control)(object)CmbCommissionDebitAccount).Text = string.Empty;
			((Control)(object)comstockparty).Text = string.Empty;
			((Control)(object)compartyreference).Text = string.Empty;
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
			txtdeliverydays.Text = string.Empty;
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtFreightAmount.Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)CmbItemName).Text = string.Empty;
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
			DocDate.Focus();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			dtFreight.Rows.Clear();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			DocumentNo();
			BranchSrNoFill();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			chkCustomAccounts.Checked = false;
			txtLedgerBalance.Text = "0";
			txtBillAmount.Text = "0";
			txtLedgerBalanceAfterBill.Text = "0";
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
			((Control)(object)CmbReserveWarehouse).Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comborderno).Text = string.Empty;
		((Control)(object)CmbItemName).Text = string.Empty;
		((Control)(object)comCropYear).Text = string.Empty;
		((Control)(object)CmbJobLot).Text = string.Empty;
		((Control)(object)comPackingType).Text = string.Empty;
		((Control)(object)comWarehouse).Text = string.Empty;
		((Control)(object)CmbReserveWarehouse).Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		CmbItemName.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == DocumentTypeId);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			List<string> allServices = new List<string> { "AllItems", "SupplierCustomerLists", "VendorsAndCustomersForTransporters" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			if (SaleCostingJobOrderWise)
			{
				dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			}
			OtherItemsBind();
			GridComboBind();
			bindWareHouse();
			BinCity();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			ItemCategoryOrTypeBind();
			ItemdtFillFromAll();
			ItemDetailFill();
			BrandDtFillFromGlobal();
			BrandItemNameBind();
			Project();
			PackingType();
			JobLotDtFillFromGlobalAndBind();
			braches();
			cropyear();
			CurrencyFill();
			ConfigurationDefault();
			CommissionDebitAccountFill();
			TransporterAcFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
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

	private bool Insert(bool IsAutoUpdate = false, int RecUpdateId = 0)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Expected O, but got Unknown
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Expected O, but got Unknown
		//IL_14b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b9: Expected O, but got Unknown
		//IL_16a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ae: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return false;
			}
			if (Conversion.ToDouble((object)txtFreightAmount.Text.Trim()) > 0.0 && ((UltraGridBase)CmbTransporterAc).ActiveRow == null)
			{
				throw new Exception("Transporter Account field Required");
			}
			InvSaleInvoice obj = new InvSaleInvoice();
			if (IsAutoUpdate)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				obj.Id = RecUpdateId;
				obj.AutoUpdate = 1;
			}
			else if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				obj.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if ((Conversion.ToDouble(r.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r.Cells["Debit"].Value) > 0) && Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return false;
				}
				if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["AccountId"].Value))
				{
					r.Cells["AccountId"].Value = 0;
					MessageBox.Show("Customer Account Not select In Party Add Less Grid row#" + (r.RowIndex + 1));
					return false;
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return false;
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0)
			{
				if (Conversion.ToInt(combcommAgent.Value) == 0)
				{
					combcommAgent.Focus();
					MessageBox.Show("Please Select Commission Agent Account First");
					return false;
				}
				if (CommissionDebitToExpenses && (((UltraGridBase)CmbCommissionDebitAccount).ActiveRow == null || Conversion.ToInt(CmbCommissionDebitAccount.Value) == 0))
				{
					CmbCommissionDebitAccount.Focus();
					MessageBox.Show("Please Select Commission Debit Account First");
					return false;
				}
			}
			else if (Conversion.ToDouble((object)txtcommamount.Text) == 0.0 && Conversion.ToDouble((object)txtcommrate.Text) > 0.0)
			{
				MessageBox.Show("Commission Amount is Required when Comm Rate is greater then 0");
				return false;
			}
			double FreightDebit = 0.0;
			double FreightCredit = 0.0;
			rows = grdFreight.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				FreightCredit += Conversion.ToDouble(r6.Cells["Freight"].Value);
				FreightDebit += Conversion.ToDouble(r6.Cells["Debit"].Value);
				if (Conversion.ToDouble(r6.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r6.Cells["Transporter"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against Freight First");
					return false;
				}
			}
			if (FreightDebitToExpenses && FreightCredit != FreightDebit)
			{
				MessageBox.Show("Freight Debit Amount not equal to Credit Amount Please Check");
				return false;
			}
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			obj.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			obj.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			obj.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			obj.CommissionType = ((Control)(object)combCommType).Text.Trim();
			obj.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			obj.UomScheduleIdCmRate = Conversion.ToString((object)((Control)(object)combcommUOM).Text);
			obj.PaymentTermId = Conversion.ToInt(combpttrm.Value);
			obj.DocDate = DocDate.Value;
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.DeliveryTerm = ((Control)(object)combdeliverytrm).Text;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.DocumentTypeId = DocumentTypeId;
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.ProjectsId = Conversion.ToInt(comproject.Value);
			obj.ReferencePartyId = Conversion.ToInt(compartyreference.Value);
			obj.OtherCategoryId = Conversion.ToInt(cmbOrderCategory2.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.OtherRemarks = obj.RemarksHeader;
			obj.StockPartyId = Conversion.ToInt(comstockparty.Value);
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				obj.TransporterId = Conversion.ToInt(CmbTransporterAc.Value);
				obj.TransporterCreditPartyId = 0;
			}
			else if (Conversion.ToInt(CmbTransporterAc.Value) > 0)
			{
				obj.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbTransporterAc).SelectedRow.Cells[0].Value);
				obj.TransporterCreditPartyId = Conversion.ToInt(((UltraDropDownBase)CmbTransporterAc).SelectedRow.Cells[1].Value);
			}
			obj.FreightAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			if (datStockDate.Visible)
			{
				obj.SupplierInvoiceDate = datStockDate.Value;
			}
			else
			{
				obj.SupplierInvoiceDate = obj.DocDate;
			}
			obj.SupplierInvoiceNo = Conversion.ToInt((object)txtduedays.Text);
			obj.SupplierReferenceNo = Conversion.ToString((object)txtSupplierReference.Text.Trim());
			obj.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ScreenName = Conversion.ToString((object)ScreenName);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.IsReserve = false;
			if (FlagForm == 1)
			{
				obj.IsReserve = true;
			}
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			obj.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			obj.InvoiceQty = Conversion.ToDecimal((object)txtInvoiceQty.Text);
			obj.InvoiceWeight = Conversion.ToDecimal((object)txtInvoiceWeight.Text);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.CustomAccounts = Conversion.ToBool((object)chkCustomAccounts.Checked);
			obj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			obj.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			obj.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			obj.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			obj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			double GrossWt = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
				detail.LineId = r7.RowIndex + 1;
				detail.Id = Conversion.ToInt(r7.Cells["Id"].Value);
				detail.InvGdnDetailId = Conversion.ToInt(r7.Cells["InvGdnDetailId"].Value);
				detail.InvGdnId = Conversion.ToInt(r7.Cells["InvGdnId"].Value);
				detail.SaleOrderId = Conversion.ToInt(r7.Cells["SaleOrderId"].Value);
				detail.ItemId = Conversion.ToInt(r7.Cells["ItemId"].Value);
				detail.ItemName = Conversion.ToString(r7.Cells["Item"].Value);
				detail.BrandItemId = Conversion.ToInt(r7.Cells["BrandItemId"].Value);
				detail.CropYear = Conversion.ToString(r7.Cells["CropYear"].Value);
				detail.JobLotId = Conversion.ToInt(r7.Cells["JobLotId"].Value);
				detail.JobLotDescription = Conversion.ToString(r7.Cells["JobLot"].Value);
				detail.PackingTypeId = Conversion.ToInt(r7.Cells["PackingTypeId"].Value);
				detail.ItemUOMId = Conversion.ToInt(r7.Cells["PackUOMId"].Value);
				detail.ItemQty = Conversion.ToDouble(r7.Cells["ItemQty"].Value);
				detail.GrossWeight = Conversion.ToDouble(r7.Cells["GrossWeight"].Value);
				GrossWt += detail.GrossWeight;
				detail.EBWeight = Conversion.ToDouble(r7.Cells["EmptyBags"].Value);
				detail.EBTotalWt = Conversion.ToDouble(r7.Cells["EmptyBagsTotal"].Value);
				detail.WeightCut = Conversion.ToSingle(r7.Cells["WeightCut"].Value);
				detail.WeightCutTotal = Conversion.ToSingle(r7.Cells["WeightCutTotal"].Value);
				detail.AdLsWeight = Conversion.ToDouble(r7.Cells["AddLss"].Value);
				detail.NetBillWeight = Conversion.ToDouble(r7.Cells["NetBillWeight"].Value);
				detail.NetStockWeight = Conversion.ToDouble(r7.Cells["StockWeight"].Value);
				if (detail.ItemId <= 0)
				{
					throw new Exception("Item filed is required in row # " + (r7.RowIndex + 1));
				}
				if (detail.CropYear == null || detail.CropYear == string.Empty)
				{
					throw new Exception("Crop Year filed is required in row # " + (r7.RowIndex + 1));
				}
				if (detail.JobLotId <= 0)
				{
					throw new Exception("JobLot filed is required in row # " + (r7.RowIndex + 1));
				}
				ItemIdExistsInJobLot(detail.ItemId, detail.ItemName, detail.JobLotId, detail.JobLotDescription);
				if (detail.GrossWeight <= 0.0)
				{
					throw new Exception("Gross weight filed is required in row # " + (r7.RowIndex + 1));
				}
				if (detail.NetBillWeight <= 0.0)
				{
					throw new Exception("Bill weight filed is required in row # " + (r7.RowIndex + 1));
				}
				if (detail.NetStockWeight <= 0.0)
				{
					throw new Exception("Stock weight filed is required in row # " + (r7.RowIndex + 1));
				}
				detail.ItemRate = Conversion.ToDouble(r7.Cells["Rate"].Value);
				if (detail.ItemRate <= 0.0)
				{
					throw new Exception("ItemRate filed is required in row # " + (r7.RowIndex + 1));
				}
				detail.ItemRateWOExp = Conversion.ToDecimal(r7.Cells["Rate"].Value);
				detail.UomScheduleIdRate = Conversion.ToInt(r7.Cells["RateUOMId"].Value);
				if (detail.UomScheduleIdRate <= 0)
				{
					throw new Exception("RateUom filed is required in row # " + (r7.RowIndex + 1));
				}
				detail.RateCut = Conversion.ToDouble(r7.Cells["RateCut"].Value);
				detail.RateCutAmount = Conversion.ToDouble(r7.Cells["RateCutTotal"].Value);
				detail.ItemAmount = Math.Round(Conversion.ToDouble(r7.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (detail.ItemAmount <= 0.0)
				{
					throw new Exception("ItemAmount filed is required in row # " + (r7.RowIndex + 1));
				}
				detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				detail.FcyAmount = Conversion.ToDecimal(r7.Cells["FcyAmount"].Value);
				detail.RateUOM = Conversion.ToDouble(r7.Cells["RateEquivalent"].Value);
				detail.WarehouseId = Conversion.ToInt(r7.Cells["WarehouseId"].Value);
				detail.WareHouseName = Conversion.ToString(r7.Cells["Warehouse"].Value);
				if (detail.WarehouseId <= 0)
				{
					throw new Exception("Warehouse filed is required in row # " + (r7.RowIndex + 1));
				}
				detail.GpDate = Conversion.ToDateTime(r7.Cells["GpDate"].Value);
				detail.GpNo = Conversion.ToInt(r7.Cells["GpNo"].Value);
				detail.VehicleNo = Conversion.ToString(r7.Cells["VehicleNo"].Value);
				detail.BillAmount = Math.Round(Conversion.ToDouble(r7.Cells["BillAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				detail.ExpenseAmount = Conversion.ToDouble(r7.Cells["Expense"].Value);
				detail.CommissionAmount = Conversion.ToDouble(r7.Cells["Commission"].Value);
				detail.FreightAmount = Conversion.ToDouble(r7.Cells["Freight"].Value);
				detail.CityId = Conversion.ToInt(r7.Cells["CityId"].Value);
				detail.BranchId = Conversion.ToInt(r7.Cells["BranchId"].Value);
				detail.BranchName = Conversion.ToString(r7.Cells["BranchName"].Value);
				if (FlagForm == 1)
				{
					detail.ReserveWareHouse = Conversion.ToInt(r7.Cells["ReserveWarehouseId"].Value);
				}
				else
				{
					detail.ReserveWareHouse = 0;
				}
				obj.InvSaleInvoiceDetaillist.Add(detail);
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r5.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r5.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r5.Cells["Amount"].Value);
					PE.CustomRemarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					PE.Remarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					if (PE.Qty > 0.0 && PE.Rate > 0.0)
					{
						PE.Remarks = "ItemName : " + Conversion.ToString((object)r5.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PE.Qty} " + $"Rate: {PE.Rate} " + PE.Remarks;
					}
					obj.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvSaleInvoiceFreight PF = new InvSaleInvoiceFreight();
					PF.InvGdnId = Conversion.ToInt(r4.Cells["InvGdnId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt(r4.Cells["Transporter"].Value);
					}
					else
					{
						PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PF.FreightAmount = Conversion.ToInt(r4.Cells["Freight"].Value);
					PF.Debit = Conversion.ToInt(r4.Cells["Debit"].Value);
					PF.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					if (PF.Remarks == null || PF.Remarks == string.Empty)
					{
						throw new Exception("Remarks Field Requried in Freight Grid");
					}
					obj.InvSaleInvoiceFreightList.Add(PF);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ2 = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					}
					else
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ2.JvRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					PJ2.JvPrcnt = Conversion.ToDouble(r3.Cells["Percentage"].Value);
					PJ2.JvQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					PJ2.JvRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PJ2.JvDebit = Conversion.ToDouble(r3.Cells["Debit"].Value);
					PJ2.JvCredit = Conversion.ToDouble(r3.Cells["Credit"].Value);
					if ((PJ2.JvDebit > 0.0 || PJ2.JvCredit > 0.0) && PJ2.JvRemarks == string.Empty)
					{
						((Control)(object)grdGLedger).Focus();
						throw new Exception("Remarks Field is Required in Party AddLess");
					}
					obj.InvSaleInvoiceJournalList.Add(PJ2);
				}
			}
			if (Conversion.ToInt(combcommAgent.Value) > 0 && Conversion.ToDouble((object)txtcommamount.Text) > 0.0)
			{
				InvSaleInvoiceCommission PJ = new InvSaleInvoiceCommission();
				PJ.Id = 0;
				PJ.SaleOrderId = 0;
				PJ.CommDebitSupCustId = 0;
				PJ.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
				if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Percent")
				{
					PJ.CommType = 2.0;
				}
				if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Flat")
				{
					PJ.CommType = 1.0;
				}
				if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Comm Weight")
				{
					PJ.CommType = 3.0;
				}
				PJ.Rate = Conversion.ToDouble((object)txtcommrate.Text);
				PJ.RateUom = Conversion.ToDouble((object)((Control)(object)combcommUOM).Text);
				PJ.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
				PJ.Remarks = Conversion.ToString((object)txtCommissionRemarks.Text);
				if (((UltraGridBase)CmbCommissionDebitAccount).ActiveRow != null && Conversion.ToInt(CmbCommissionDebitAccount.Value) > 0)
				{
					PJ.DebitAccountId = Conversion.ToInt(((UltraDropDownBase)CmbCommissionDebitAccount).SelectedRow.Cells["GlAccountId"].Value);
					PJ.CommDebitSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbCommissionDebitAccount).SelectedRow.Cells["SupplierCustomerId"].Value);
				}
				obj.InvSaleInvoiceCommission.Add(PJ);
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			int sucess = InvSaleInvoice.Save(obj);
			if (IsAutoUpdate)
			{
				Reset();
			}
			else
			{
				if (Id == 0)
				{
					MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(Id, base.Name, DocumentTypeId, 0);
					}
				}
				if (WagesStatus && WagesActiveOrInActive)
				{
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = DocumentTypeId;
					frmwagesBillHeader.RefDocId = sucess;
					frmwagesBillHeader.GrossWeightTotal = GrossWt;
					frmwagesBillHeader.ShowDialog();
					frmwagesBillHeader.BringToFront();
				}
				Reset();
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
				}
				if (ChkPrintslip.Checked)
				{
					CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", sucess);
				}
				if (chkPrint294B.Checked)
				{
					CommonServices.SaleInvoiceDirectSlip_294B(sucess);
				}
				if (chkPrintSlip294A.Checked)
				{
					CommonServices.SaleInvoiceDirectPartySlip_294(sucess);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		return true;
	}

	private void saveToolStripButton_Click(object sender, EventArgs e)
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
			InvSaleInvoice invPI = InvSaleInvoice.GetByID(Id);
			if (invPI == null)
			{
				return;
			}
			combranches.Value = invPI.BranchesId;
			comproject.Value = invPI.ProjectsId;
			txtdocno.Text = invPI.DocNo.ToString();
			txtBranchSrNo.Text = Conversion.ToString((object)invPI.BranchSrNo);
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			comsupplier.Value = invPI.SupplierCustomerId;
			compartyreference.Value = invPI.ReferencePartyId;
			comstockparty.Value = invPI.StockPartyId;
			if (invPI.TransporterId > 0)
			{
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					CmbTransporterAc.Value = invPI.TransporterId;
				}
				else
				{
					CmbTransporterAc.Value = invPI.TransporterCreditPartyId;
				}
			}
			txtFreightAmount.Text = Conversion.ToString((object)invPI.FreightAmount);
			txtSupplierReference.Text = Conversion.ToString((object)invPI.SupplierReferenceNo);
			txtbillno.Text = invPI.ManualBillNo;
			combcommAgent.Value = invPI.CommissionAgentId;
			if (invPI.InvSaleInvoiceCommission.Count > 0)
			{
				CmbCommissionDebitAccount.Value = invPI.InvSaleInvoiceCommission[0].DebitAccountId;
			}
			((Control)(object)combCommType).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = invPI.CommRate.ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)combcommUOM).Text = Conversion.ToString((object)invPI.UomScheduleIdCmRate);
			txtcommamount.Text = invPI.CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtCommissionRemarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			txtremarks.Text = Conversion.ToString((object)invPI.OtherRemarks);
			txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			if (invPI.PaymentTermId > 0)
			{
				combpttrm.Value = invPI.PaymentTermId;
			}
			txtduedays.Text = Conversion.ToString((object)invPI.SupplierInvoiceNo);
			duedate.Value = invPI.SupplierInvoiceDate;
			if (Conversion.ToString((object)invPI.DeliveryTerm) != null && Conversion.ToString((object)invPI.DeliveryTerm) != "")
			{
				((Control)(object)combdeliverytrm).Text = invPI.DeliveryTerm;
			}
			Approved = invPI.IsApproved;
			cmbCurrency.Value = invPI.CurrencyId;
			txtInvoiceQty.Text = invPI.InvoiceQty.ToString("#,##0.##");
			txtInvoiceWeight.Text = invPI.InvoiceWeight.ToString("#,##0.##");
			txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			if (invPI.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
			}
			else if (invPI.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			AttachmentsValues = invPI.AttachmentsValues;
			CustomAttachmentsValues = invPI.CustomAttachmentsValues;
			chkCustomAccounts.Checked = Conversion.ToBool((object)invPI.CustomAccounts);
			cmbOrderCategory2.Value = Conversion.ToInt((object)invPI.OtherCategoryId);
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int l = 0; l < invPI.InvSaleInvoiceDetaillist.Count; l++)
			{
				dtGrid.Rows.Add(invPI.InvSaleInvoiceDetaillist[l].Id, invPI.InvSaleInvoiceDetaillist[l].InvGdnId, invPI.InvSaleInvoiceDetaillist[l].InvGdnDetailId, invPI.InvSaleInvoiceDetaillist[l].ForwardingDocNo, 0, 0, invPI.InvSaleInvoiceDetaillist[l].ItemId, invPI.InvSaleInvoiceDetaillist[l].ItemCode, invPI.InvSaleInvoiceDetaillist[l].ItemName, invPI.InvSaleInvoiceDetaillist[l].BrandItemId, invPI.InvSaleInvoiceDetaillist[l].BrandItemCode, invPI.InvSaleInvoiceDetaillist[l].BrandItemName, invPI.InvSaleInvoiceDetaillist[l].CropYear, invPI.InvSaleInvoiceDetaillist[l].JobLotId, invPI.InvSaleInvoiceDetaillist[l].JobLotDescription, invPI.InvSaleInvoiceDetaillist[l].PackingTypeId, invPI.InvSaleInvoiceDetaillist[l].PackTypeDesc, invPI.InvSaleInvoiceDetaillist[l].ItemUOMId, invPI.InvSaleInvoiceDetaillist[l].UOMCodeItem, invPI.InvSaleInvoiceDetaillist[l].PackEquivalent, invPI.InvSaleInvoiceDetaillist[l].ItemQty, invPI.InvSaleInvoiceDetaillist[l].GrossWeight, invPI.InvSaleInvoiceDetaillist[l].EBWeight, Math.Round(invPI.InvSaleInvoiceDetaillist[l].EBTotalWt), invPI.InvSaleInvoiceDetaillist[l].WeightCut, Math.Round(invPI.InvSaleInvoiceDetaillist[l].WeightCutTotal), invPI.InvSaleInvoiceDetaillist[l].AdLsWeight, invPI.InvSaleInvoiceDetaillist[l].NetBillWeight, invPI.InvSaleInvoiceDetaillist[l].NetStockWeight, invPI.InvSaleInvoiceDetaillist[l].ItemRate, invPI.InvSaleInvoiceDetaillist[l].UomScheduleIdRate, invPI.InvSaleInvoiceDetaillist[l].UOMCodeRate, invPI.InvSaleInvoiceDetaillist[l].RateUOM, invPI.InvSaleInvoiceDetaillist[l].RateCut, invPI.InvSaleInvoiceDetaillist[l].RateCutAmount, invPI.InvSaleInvoiceDetaillist[l].ItemAmount, invPI.InvSaleInvoiceDetaillist[l].FcyAmount, invPI.InvSaleInvoiceDetaillist[l].WarehouseId, invPI.InvSaleInvoiceDetaillist[l].WareHouseName, invPI.InvSaleInvoiceDetaillist[l].GpDate, invPI.InvSaleInvoiceDetaillist[l].GpNo, invPI.InvSaleInvoiceDetaillist[l].VehicleNo, invPI.InvSaleInvoiceDetaillist[l].BillAmount, invPI.InvSaleInvoiceDetaillist[l].ExpenseAmount, invPI.InvSaleInvoiceDetaillist[l].JournalAmount, invPI.InvSaleInvoiceDetaillist[l].CommissionAmount, invPI.InvSaleInvoiceDetaillist[l].FreightAmount, invPI.InvSaleInvoiceDetaillist[l].SaleGLAC, invPI.InvSaleInvoiceDetaillist[l].ReserveWareHouse, invPI.InvSaleInvoiceDetaillist[l].ReserveWareHouseName, invPI.InvSaleInvoiceDetaillist[l].CityId, invPI.InvSaleInvoiceDetaillist[l].CityName, invPI.InvSaleInvoiceDetaillist[l].BranchId, invPI.InvSaleInvoiceDetaillist[l].BranchName);
				if (invPI.InvSaleInvoiceDetaillist[l].InvGdnId > 0)
				{
					DisableFields();
				}
				else
				{
					EnablesFields();
				}
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invPI.InvSaleInvoiceExpenseList.Count; k++)
			{
				dtInvExp.Rows.Add(invPI.InvSaleInvoiceExpenseList[k].InvRevExpItemId, invPI.InvSaleInvoiceExpenseList[k].Qty, invPI.InvSaleInvoiceExpenseList[k].Rate, invPI.InvSaleInvoiceExpenseList[k].Amount, invPI.InvSaleInvoiceExpenseList[k].CustomRemarks);
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invPI.InvSaleInvoiceJournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invPI.InvSaleInvoiceJournalList[j].TransporterSupCustId : invPI.InvSaleInvoiceJournalList[j].ChartofAccountId, invPI.InvSaleInvoiceJournalList[j].ChartofAccountId, invPI.InvSaleInvoiceJournalList[j].JvRemarks, invPI.InvSaleInvoiceJournalList[j].JvPrcnt, invPI.InvSaleInvoiceJournalList[j].JvQty, invPI.InvSaleInvoiceJournalList[j].JvRate, invPI.InvSaleInvoiceJournalList[j].JvDebit, invPI.InvSaleInvoiceJournalList[j].JvCredit);
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < invPI.InvSaleInvoiceFreightList.Count; i++)
			{
				dtFreight.Rows.Add(invPI.InvSaleInvoiceFreightList[i].InvGdnId, invPI.InvSaleInvoiceFreightList[i].TansporterId, invPI.InvSaleInvoiceFreightList[i].TransporterSupCustId, invPI.InvSaleInvoiceFreightList[i].FreightAmount, invPI.InvSaleInvoiceFreightList[i].Debit, invPI.InvSaleInvoiceFreightList[i].Remarks);
			}
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			gridGLSettings();
			grdFreightSettings();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			BillAmount();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvSaleInvoice.GetSaleInvoiceIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				bool shouldContinue = true;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int recId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(recId);
					shouldContinue = Insert(IsAutoUpdate: true, recId);
					if (!shouldContinue)
					{
						break;
					}
				}
				if (shouldContinue)
				{
					MessageBox.Show("Records updated successfully.");
				}
				else
				{
					MessageBox.Show("Update stopped before all records were processed.");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosBranchFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
			UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)cmbBranchName).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)cmbBranchName).Text = BranchName;
				}
				else
				{
					((Control)(object)cmbBranchName).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill(bool ValidateBranch = true)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			string BranchIds = "";
			if (dtBranch.Rows.Count > 0)
			{
				if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
				{
					string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
					obj.BranchesIds = BranchIds;
				}
				else
				{
					if (ValidateBranch)
					{
						cmbBranchName.Focus();
						throw new Exception("Select branch first");
					}
					obj.BranchesIds = UserAccount.BranchesId.ToString();
				}
			}
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(obj);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Customer", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.DocumentTypeId = DocumentTypeId;
			PI.CanViewAllRecord = true;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				PI.BranchesIds = BranchIds;
				DataTable dthistory = InvSaleInvoice.FormHistory(PI);
				if (dthistory.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("VoucherHeadId", typeof(int));
					dt.Columns.Add("GdnId", typeof(int));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("SupplierCustomerId", typeof(int));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("ManualBillNo", typeof(string));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("CommAgent", typeof(string));
					dt.Columns.Add("CommType", typeof(string));
					dt.Columns.Add("CommRate", typeof(double));
					dt.Columns.Add("CommAmount", typeof(double));
					dt.Columns.Add("CommRemarks", typeof(string));
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("OtherCategory", typeof(string));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("EntryUser", typeof(string));
					dt.Columns.Add("ModifyUser");
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUser", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("IsReserve", typeof(bool));
					dt.Columns.Add("NoOfAttachments", typeof(int));
					dt.Columns.Add("Remarks", typeof(string));
					for (int i = 0; i < dthistory.Rows.Count; i++)
					{
						dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["GdnId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["BranchSrNo"], dthistory.Rows[i]["BranchName"], dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["CustomerName"], dthistory.Rows[i]["ManualBillNo"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], dthistory.Rows[i]["CommAmount"], dthistory.Rows[i]["CommissionRemarks"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["OtherCategory"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["UserName"], dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUserName"], dthistory.Rows[i]["ApprovedDate"], dthistory.Rows[i]["IsReserve"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
					}
					grdHistory.DataSource = dt;
					grdHistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdHistory.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["IsReserve"].Visible = false;
			grdHistory.RootTable.Columns["GdnId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			grdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["OtherCategory"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["IsReserve"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Print";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Print";
			grdHistory.RootTable.Columns["View"].Width = 40;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "Party Slip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "Party Slip";
			grdHistory.RootTable.Columns["PartySlip"].Width = 65;
			grdHistory.RootTable.Columns.Add("ItemSlip");
			grdHistory.RootTable.Columns["ItemSlip"].Caption = "Item Slip";
			grdHistory.RootTable.Columns["ItemSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["ItemSlip"].ButtonText = "Item Slip";
			grdHistory.RootTable.Columns["ItemSlip"].Width = 65;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns["View"].Position = 0;
			grdHistory.RootTable.Columns["PartySlip"].Position = 1;
			grdHistory.RootTable.Columns["ItemSlip"].Position = 2;
			grdHistory.RootTable.Columns["Edit"].Position = 3;
			grdHistory.RootTable.Columns["Voucher"].Position = 4;
			grdHistory.FrozenColumns = 5;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
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
			FromDateHistory.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (FlagForm != 1 && Conversion.ToBool(grdHistory.CurrentRow.Cells["IsReserve"].Value))
			{
				throw new Exception("Record cannot be updated because this InvoiceType is Reserve");
			}
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
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
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Currid = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				if (FlagForm != 1 && Conversion.ToBool(grdHistory.CurrentRow.Cells["IsReserve"].Value))
				{
					throw new Exception("Record cannot be updated because this InvoiceType is Reserve");
				}
				Reset();
				Id = Currid;
				ReadById(Id);
			}
			else if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Currid);
			}
			else if (e.Column.Key == "PartySlip")
			{
				CommonServices.SaleInvoiceDirectPartySlip_294(Currid);
			}
			else if (e.Column.Key == "ItemSlip")
			{
				CommonServices.SaleInvoiceDirectItemSlip_294C(Currid);
			}
			else if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), DocumentTypeId);
			}
			else if (e.Column.Key == "Detail")
			{
				GetDetailGrdByHeadId();
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Currid, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("BrandItemCode");
			dtdetail.Columns.Add("BrandItemName");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("EbCut", typeof(double));
			dtdetail.Columns.Add("EBTotalWt", typeof(double));
			dtdetail.Columns.Add("WeightCut", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLess", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("FcyAmount", typeof(decimal));
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("GpDate", typeof(DateTime));
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("BillAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtdetail.Columns.Add("CityName");
			dtdetail.Columns.Add("BranchName");
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].ItemId, obj.InvSaleInvoiceDetaillist[i].ItemCode, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].BrandItemCode, obj.InvSaleInvoiceDetaillist[i].BrandItemName, obj.InvSaleInvoiceDetaillist[i].CropYear, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].PackTypeDesc, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].EBWeight, obj.InvSaleInvoiceDetaillist[i].EBTotalWt, obj.InvSaleInvoiceDetaillist[i].WeightCut, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].AdLsWeight, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].UOMCodeRate, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].GpDate.ToShortDateString(), obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].BillAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount, obj.InvSaleInvoiceDetaillist[i].CityName, obj.InvSaleInvoiceDetaillist[i].BranchName);
				}
				grdDetail.DataSource = dtdetail;
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
			throw new Exception(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.Columns["PackType"].Caption = "Packing Type";
			grdDetail.RootTable.Columns["ItemUOM"].Caption = "Pack Uom";
			grdDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["EbCut"].Caption = "Wt Cut";
			grdDetail.RootTable.Columns["EBTotalWt"].Caption = "Cut Total";
			grdDetail.RootTable.Columns["AddLess"].Caption = "AD/LS";
			grdDetail.RootTable.Columns["NetStockWeight"].Caption = "Stock Weight";
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["BrandItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			grdDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["EbCut"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["EBTotalWt"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["AddLess"].Width = Constants.InventoryConstants.AddLessConstant;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["NetStockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.FcyConstant;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ExpenseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.FreightConstant;
			grdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdDetail.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosBranchFill();
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
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
			CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Id);
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
			CommonServices.SaleInvoiceDirectPartySlip_294(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint294B_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id <= 0)
			{
				throw new Exception("Record not found for display");
			}
			CommonServices.SaleInvoiceDirectSlip_294B(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnItemSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceDirectItemSlip_294C(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	public void BillAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double BillAmount = 0.0;
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2));
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double JournalDebit = 0.0;
				double JournalCredit = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0)
					{
						JournalDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
						JournalCredit += Conversion.ToDouble((object)r.Cells["Credit"].Value.ToString());
						SupplierGLDebitAmount = JournalDebit;
						SupplierGLCreditAmount = JournalCredit;
					}
				}
				ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				ExpAmount = Math.Round(ExpAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalDebit = Math.Round(JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				JournalCredit = Math.Round(JournalCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double FreightAmount = Math.Round(Conversion.ToDouble((object)txtFreightAmount.Text.Trim()), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				BillAmount = ItemAmount + ExpAmount - FreightAmount;
				BillAmount = BillAmount + JournalCredit - JournalDebit;
				if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(combcommAgent.Value))
				{
					BillAmount -= Conversion.ToDouble((object)txtcommamount.Text.Trim());
					SupplierCommissionAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
				}
				else
				{
					SupplierCommissionAmount = 0.0;
				}
				txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				txtLedgerBalanceAfterBill.Text = Math.Round(LedgerBalance + BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatboth);
				txtInvoiceQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtInvoiceWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtFcyAmount.Text = (BillAmount / Conversion.ToDouble((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtInvoiceQty.Text = "0";
				txtInvoiceWeight.Text = "0";
				txtFcyAmount.Text = "0";
			}
			BillProportion();
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
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
				obj.Cells["Expense"].Value = Math.Round(TotalAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			BillProportion();
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
			foreach (GridEXRow r in rows)
			{
				double itemAmount = Math.Round(Conversion.ToDouble(r.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Commission = Math.Round(Conversion.ToDouble(r.Cells["Commission"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Expense = Math.Round(Conversion.ToDouble(r.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				r.BeginEdit();
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = itemAmount;
					}
					else
					{
						r.Cells["BillAmount"].Value = Math.Round(itemAmount - Commission, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Math.Round(itemAmount + Expense - Commission, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				r.EndEdit();
			}
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
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double CommPercent = Conversion.ToDouble((object)txtcommrate.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (((Control)(object)combCommType).Text == "Percent")
				{
					double ItemAmount = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString());
					r.Cells["Commission"].Value = ItemAmount * CommPercent / 100.0;
					r.EndEdit();
				}
				else
				{
					double Weight = Conversion.ToDouble((object)r.Cells["NetBillWeight"].Value.ToString());
					r.Cells["Commission"].Value = TotalAmount / NetWeight * Weight;
					r.EndEdit();
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
			if (FreightDebitToExpenses)
			{
				return;
			}
			double TotalAmount = ((grdFreight.RowCount > 0) ? Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString()) : 0.0);
			if (TotalAmount > 0.0)
			{
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["NetBillWeight"].Value);
					obj.Cells["Freight"].Value = Math.Round(TotalAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				BillProportion();
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freight"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = 0.0;
			double WeightCutTotal = 0.0;
			string AddLess = "0";
			if (((Control)(object)comPackUOM).Text != string.Empty && Conversion.ToInt(comPackUOM.Value) != 0)
			{
				Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim().ToString());
			}
			if (Conversion.ToDouble((object)txtEmptybagsUnit.Text) != 0.0)
			{
				EBUnit = Conversion.ToDouble((object)txtEmptybagsUnit.Text);
			}
			if (Conversion.ToDouble((object)txtEmptyBagsTotal.Text) != 0.0)
			{
				EBTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmUnit")
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.###");
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmTotal")
			{
				EBUnit = EBTotal / Qty;
				txtEmptybagsUnit.Text = EBUnit.ToString("#,##0.####");
			}
			else
			{
				EBUnit = EBTotal / Qty;
				txtEmptybagsUnit.Text = EBUnit.ToString("#,##0.####");
			}
			if (txtNetBillWeight.Text != string.Empty && txtNetBillWeight.Text != null)
			{
				Conversion.ToDouble((object)txtNetBillWeight.Text.Trim().ToString());
			}
			if (txtStockWeight.Text != string.Empty && txtStockWeight.Text != null)
			{
				Conversion.ToDouble((object)txtStockWeight.Text.Trim().ToString());
			}
			if (txtwtcut.Text != string.Empty && txtwtcut.Text != null)
			{
				WeightCut = Conversion.ToDouble((object)txtwtcut.Text.Trim().ToString());
				WeightCutTotal = Qty * WeightCut;
				txtWeightCutTotal.Text = WeightCutTotal.ToString("#,##0.###");
			}
			else
			{
				txtWeightCutTotal.Text = "0";
			}
			if (txtAddLss.Text != string.Empty && txtAddLss.Text != null)
			{
				AddLess = txtAddLss.Text.Trim().ToString();
			}
			if (AddLess == "-")
			{
				double Weight = GrossWeight - EBTotal - WeightCutTotal;
				txtNetBillWeight.Text = Math.Round(Weight, 2).ToString("#,##0.##");
				txtStockWeight.Text = Math.Round(Weight, 2).ToString("#,##0.##");
			}
			else
			{
				double Weight2 = GrossWeight - EBTotal - WeightCutTotal + Conversion.ToDouble((object)AddLess);
				txtNetBillWeight.Text = Math.Round(Weight2, 2).ToString("#,##0.##");
				txtStockWeight.Text = Math.Round(Weight2, 2).ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			if (((UltraGridBase)comPackUOM).ActiveRow != null && Conversion.ToInt(comPackUOM.Value) != 0)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (Conversion.ToDouble((object)txtQty.Text) != 0.0)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text);
			}
			if (Conversion.ToDouble((object)txtGrossWeight.Text) != 0.0)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
			}
			txtGrossWeight.Text = (PackUOM * Qty).ToString("#,##0.###");
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
			double RateUOM = 0.0;
			double Rate = 0.0;
			double RateCut = 0.0;
			double NetWeight = 0.0;
			double ItemAmount = 0.0;
			double RateCutTotal = 0.0;
			RateUOM = ((((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) == 0) ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value));
			if (Conversion.ToDouble((object)txtRate.Text) != 0.0)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (Conversion.ToDouble((object)txtratecut.Text) != 0.0)
			{
				RateCut = Conversion.ToDouble((object)txtratecut.Text);
			}
			if (Conversion.ToDouble((object)txtNetBillWeight.Text) != 0.0)
			{
				NetWeight = Conversion.ToDouble((object)txtNetBillWeight.Text);
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				RateCutTotal = NetWeight / RateUOM * RateCut;
				ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				RateCutTotal = Math.Round(RateCutTotal, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtratecuttotal.Text = RateCutTotal.ToString(clsGlobalVariables.stringFormatsingle);
				ItemAmount = Math.Round(ItemAmount - RateCutTotal, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtAmount.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = "0";
				txtBillAmount.Text = "0";
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
			if (txtcommrate.Text != string.Empty && ((Control)(object)combCommType).Text == "Flat")
			{
				double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				txtcommamount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (txtcommrate.Text != string.Empty && (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage"))
			{
				double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
					txtcommamount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			if (txtcommrate.Text != string.Empty && ((Control)(object)combCommType).Text == "Comm Weight")
			{
				double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
				if (Weight != null)
				{
					double num = Conversion.ToDouble((object)Weight.ToString());
					double UOM = Conversion.ToDouble((object)((Control)(object)combcommUOM).Text.Trim());
					double CommAmt = num / UOM * CommRate2;
					txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void combCommType_Leave(object sender, EventArgs e)
	{
		if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Flat")
		{
			txtcommrate.Text = Conversion.ToDouble((object)txtcommrate.Text).ToString("#,###");
		}
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)combCommType).Text)))
		{
			txtcommrate.Text = "";
		}
		else if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Flat")
		{
			txtcommrate.Text = Conversion.ToDouble((object)txtcommrate.Text).ToString("#,###");
		}
		TotalCommissionAmount();
		BillAmount();
	}

	private void combcommUOM_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			TotalWeight();
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

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		Total();
		AmountCaluculation();
	}

	private int GetCursorOffset(string formattedText, string originalText, int originalCursorPosition)
	{
		int length = formattedText.Length;
		int originalLength = originalText.Length;
		int cursorOffset = length - originalLength;
		if (originalCursorPosition == originalLength)
		{
			return cursorOffset;
		}
		int originalCursorOffset = originalLength - originalCursorPosition;
		return Math.Max(cursorOffset - originalCursorOffset, 0);
	}

	private void txtEmptybagsUnit_TextChanged(object sender, EventArgs e)
	{
		Total();
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtwtcut_TextChanged(object sender, EventArgs e)
	{
		Total();
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtAddLss_TextChanged(object sender, EventArgs e)
	{
		Total();
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
	{
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtratecut_TextChanged(object sender, EventArgs e)
	{
		AmountCaluculation();
		TotalCommissionAmount();
		BillAmount();
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
			AvailableStockGetByItem();
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
			txtLedgerBalance.Text = "0";
			int SupplierId = Conversion.ToInt(comsupplier.Value);
			LedgerBalance = 0.0;
			if (dtSupplier.Rows.Count > 0 && ((UltraGridBase)comsupplier).ActiveRow != null && SupplierId > 0)
			{
				int GLID = 0;
				DataRow[] dr = dtSupplier.Select("Id = " + SupplierId);
				if (dr.Length != 0)
				{
					GLID = Conversion.ToInt(dr[0]["GlAccountId"]);
					txtSupplierGLId.Text = Conversion.ToString((object)GLID);
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					DataTable Baldt = CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(GLID, clsGlobalVariables.ActiveYr.Start_Period, DocDate.Value);
					if (Baldt.Rows.Count > 0)
					{
						LedgerBalance = Conversion.ToDouble(Baldt.Rows[0]["ClosingBalance"]);
						txtLedgerBalance.Text = Math.Round(LedgerBalance).ToString("#,#;(#,#);0");
					}
				}
				else
				{
					DataTable Baldt2 = CommonServices.GetSubsidiaryLedgerBalance(SupplierId, DocDate.Value);
					if (Baldt2.Rows.Count > 0)
					{
						LedgerBalance = Conversion.ToDouble(Baldt2.Rows[0]["Balance"]);
						txtLedgerBalance.Text = Math.Round(LedgerBalance).ToString("#,#;(#,#);0");
					}
				}
			}
			BillAmount();
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
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void combCommType_Leave_1(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Flat")
			{
				txtcommrate.Text = Conversion.ToDouble((object)txtcommrate.Text).ToString("#,###");
			}
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			AmountCaluculation();
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void comItem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void combcommAgent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void comWarehouse_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemdtFillFromAll();
			ItemDetailFill();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				FreightProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comCropYear_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comPackingType_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void txtStockWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
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
			txtFreightAmount.Text = "0";
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
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)combCommType).Text)))
			{
				combCommType.Focus();
				throw new Exception("Please Select Commission type first..");
			}
			if (Conversion.ToString((object)((Control)(object)combCommType).Text) == "Flat")
			{
				CommonServices.OnlytextNumberFunction(sender, e);
			}
			else
			{
				CommonServices.OnlytextdecimelFunction(sender, e);
			}
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnLoadGdn_Click(object sender, EventArgs e)
	{
		try
		{
			FlagLoader = "SD";
			LoadGDN = new frmLoadGDN(UserAccount);
			LoadGDN.ShowDialog();
			LoadGDN.BringToFront();
			LoadInGridDetail();
			FlagLoader = "";
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
			dtGrid.Rows.Clear();
			string GdnIds = "";
			for (int i = 0; i < LoadGDN.dtGDNS.Rows.Count; i++)
			{
				GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadGDN.dtGDNS.Rows[i][0].ToString());
			}
			LoadDataDetailGridAgainstGP(GdnIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			LoadFreightData(GdnIds);
			if (dtFreight.Rows.Count == 0 || dtFreight == null)
			{
				AddRowInFreightGrid();
			}
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtInvExp.Clear();
			LoadExpData(GdnIds);
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.RetrieveStructure();
			TotalCommissionAmount();
			grdInvExpSettings();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridAgainstGP(string IDs)
	{
		try
		{
			DataTable invGdn = InvSaleInvoice.GdnLoadForSaleInvoiceDirect(IDs);
			if (invGdn.Rows.Count > 0)
			{
				DisableFields();
				comsupplier.Value = invGdn.Rows[0]["SupplierCustomerId"];
				((Control)(object)comsupplier).Text = Conversion.ToString(invGdn.Rows[0]["CustomerName"]);
				txtremarks.Text = invGdn.Rows[0]["RemarksHeader"].ToString();
				for (int i = 0; i < invGdn.Rows.Count; i++)
				{
					dtGrid.Rows.Add(0, invGdn.Rows[i]["InvGdnId"], invGdn.Rows[i]["Id"], invGdn.Rows[i]["DocNo"], 0, 0, invGdn.Rows[i]["ItemId"], invGdn.Rows[i]["ItemCode"], invGdn.Rows[i]["ItemName"], invGdn.Rows[i]["BrandItemId"], invGdn.Rows[i]["BrandItemCode"], invGdn.Rows[i]["BrandItemName"], invGdn.Rows[i]["CropYear"], invGdn.Rows[i]["JobLotId"], invGdn.Rows[i]["JobLotDescription"], invGdn.Rows[i]["PackingTypeId"], invGdn.Rows[i]["PackTypeDesc"], invGdn.Rows[i]["ItemUomId"], invGdn.Rows[i]["UOMCodeItem"], 0, invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["GrossWeight"], invGdn.Rows[i]["EBWPerUnit"], invGdn.Rows[i]["EBWTotal"], invGdn.Rows[i]["WtCut"], invGdn.Rows[i]["WtCutTotal"], invGdn.Rows[i]["AdLsWeight"], invGdn.Rows[i]["NetBillWeight"], invGdn.Rows[i]["StockWeight"], 0, 0, "", 0, 0, 0, 0, 0, invGdn.Rows[i]["WarehouseId"], invGdn.Rows[i]["WareHouseName"], DateTime.Now, invGdn.Rows[i]["GpNo"], invGdn.Rows[i]["VehicleNo"], 0, 0, 0, 0, 0, invGdn.Rows[i]["PurchaseGLAC"], 0, 0, invGdn.Rows[i]["CityId"], invGdn.Rows[i]["CityName"], invGdn.Rows[i]["BranchId"], invGdn.Rows[i]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string ID)
	{
		DataTable item = InvSaleInvoice.GetItemQtyAndItemRateFromSO(ID);
		if (item.Rows.Count > 0)
		{
			for (int i = 0; i < item.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(0, item.Rows[i]["ItemQty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["BagPrice"]), 0);
			}
		}
	}

	private void LoadFreightData(string ID)
	{
		try
		{
			DataTable item = InvSaleInvoice.GetTransporterAndFreightFromGdn(ID);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			dtFreight.Clear();
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (grdFreight.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdFreight.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["InvGdnId"].Value) == Conversion.ToInt(item.Rows[i]["MainId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && Conversion.ToInt(item.Rows[i]["CarriageAmount"]) > 0)
				{
					string Remarks = "";
					if (Conversion.ToInt(item.Rows[i]["GpNo"]) > 0)
					{
						Remarks = Remarks + "GpNo: " + Conversion.ToString(item.Rows[i]["GpNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["VehicleNo"]) != string.Empty)
					{
						Remarks = Remarks + "   VehicleNo:  " + Conversion.ToString(item.Rows[i]["VehicleNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["BiltyNo"]) != string.Empty)
					{
						Remarks = Remarks + "   BiltyNo: " + Conversion.ToString(item.Rows[i]["BiltyNo"]);
					}
					if (SubsidiaryAccountAllownOnVouchers)
					{
						dtFreight.Rows.Add(item.Rows[i]["MainId"], item.Rows[i]["Transporter"], item.Rows[i]["TransporterSupCustId"], item.Rows[i]["CarriageAmount"], 0, Remarks);
					}
					else
					{
						dtFreight.Rows.Add(item.Rows[i]["MainId"], item.Rows[i]["Transporter"], 0, item.Rows[i]["CarriageAmount"], 0, Remarks);
					}
				}
				flag = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
		try
		{
			datStockDate.Visible = false;
			btnRecordsUpdate.Visible = false;
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
					FromDateHistory.Focus();
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
			if (e.Control && e.Shift && e.KeyCode == Keys.F)
			{
				btnRecordsUpdate.Visible = true;
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					saveToolStripButton_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
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
						((Control)(object)grdInvExp).Focus();
					}
					if (((Control)(object)grdInvExp).Focused)
					{
						((Control)(object)grdGLedger).Focus();
					}
					else
					{
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
				if (e.Control && e.Shift && e.KeyCode == Keys.F)
				{
					datStockDate.Visible = true;
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNewHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
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
			dt.Rows.Add("Alt+1", "For Print Slip 294");
			dt.Rows.Add("Alt+2", "For Print Slip 294A");
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
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete" && item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				TotalCommissionAmount();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				TotalCommissionAmount();
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
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
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdGLedger.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn.Key == "Delete")
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
				else if (grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
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
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				BillAmount();
			}
			else if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				if (grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId")
				{
					if (SubsidiaryAccountAllownOnVouchers)
					{
						DataTable dt = CommonServices.GetVendorsAndCustomers();
						GrdPopUp obj = new GrdPopUp(dt, "Id", "CompanyName");
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						obj.BringToFront();
						Id = Conversion.ToInt((object)obj.ReturnId);
						if (Id > 0)
						{
							item.Cells["AccountId"].Value = Id;
							DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
							int GlAccountId = ((foundRow != null) ? Conversion.ToInt(foundRow["GlAccountId"]) : 0);
							item.Cells["GlAccountId"].Value = GlAccountId;
							if (Conversion.ToInt(comsupplier.Value) == Conversion.ToInt(item.Cells["AccountId"].Value))
							{
								MessageBox.Show("Selected Account Can not be Same As Supplier Account");
								item.Cells["AccountId"].Value = 0;
								item.Cells["GlAccountId"].Value = 0;
								return;
							}
						}
						else
						{
							item.Cells["AccountId"].Value = 0;
							item.Cells["GlAccountId"].Value = 0;
						}
					}
					else
					{
						GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						Id = Conversion.ToInt((object)obj2.ReturnId);
						if (Id > 0)
						{
							item.Cells["GlAccountId"].Value = Id;
							item.Cells["AccountId"].Value = Id;
							if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
							{
								MessageBox.Show("Selected Account Can not be Same As Supplier Account");
								item.Cells["GlAccountId"].Value = 0;
								item.Cells["AccountId"].Value = 0;
								return;
							}
						}
						else
						{
							item.Cells["GlAccountId"].Value = 0;
							item.Cells["AccountId"].Value = 0;
						}
						SupCustIdUpdateforGLGrid();
					}
				}
			}
			((Control)(object)grdGLedger).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInvExp.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn.Key == "Delete")
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
				}
				if (grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
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
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				txtExchangeRate_TextChanged(null, null);
			}
			((Control)(object)grdInvExp).Focus();
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
			int Currid = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					Id = Currid;
					ReadById(Id);
					comsupplier.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "View")
				{
					CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Currid);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "PartySlip")
				{
					CommonServices.SaleInvoiceDirectPartySlip_294(Currid);
				}
				if (grdHistory.CurrentColumn.Key == "ItemSlip")
				{
					CommonServices.SaleInvoiceDirectItemSlip_294C(Currid);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Currid, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Id = Currid;
				ReadById(Id);
				comsupplier.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Currid);
			}
			((Control)(object)grdHistory).Focus();
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
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee7: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8c: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d6: Expected O, but got Unknown
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1139: Expected O, but got Unknown
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bd: Expected O, but got Unknown
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Expected O, but got Unknown
		//IL_11ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f4: Expected O, but got Unknown
		//IL_1216: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Expected O, but got Unknown
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1333: Expected O, but got Unknown
		//IL_136b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1375: Expected O, but got Unknown
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1396: Expected O, but got Unknown
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Expected O, but got Unknown
		//IL_1bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc6: Expected O, but got Unknown
		//IL_1bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf4: Expected O, but got Unknown
		//IL_1f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7a: Expected O, but got Unknown
		//IL_1f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f91: Expected O, but got Unknown
		//IL_2238: Unknown result type (might be due to invalid IL or missing references)
		//IL_2242: Expected O, but got Unknown
		//IL_224f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2259: Expected O, but got Unknown
		//IL_2266: Unknown result type (might be due to invalid IL or missing references)
		//IL_2270: Expected O, but got Unknown
		//IL_2505: Unknown result type (might be due to invalid IL or missing references)
		//IL_250f: Expected O, but got Unknown
		//IL_251c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2526: Expected O, but got Unknown
		//IL_2533: Unknown result type (might be due to invalid IL or missing references)
		//IL_253d: Expected O, but got Unknown
		//IL_f5e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_f5f0: Expected O, but got Unknown
		//IL_f614: Unknown result type (might be due to invalid IL or missing references)
		//IL_f61e: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
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
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
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
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
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
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
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
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
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
		Appearance appearance269 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.InvfrmInvSaleInvoiceDirect));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabFreight = new System.Windows.Forms.TabPage();
		this.grdFreight = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label67 = new System.Windows.Forms.Label();
		this.tabExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.tabGL = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.chkPrint294B = new System.Windows.Forms.CheckBox();
		this.LabelCommissionDrCaption = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbCommissionDebitAccount = new UltraCombo();
		this.combcommAgent = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.combcommUOM = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.combCommType = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.chkPrintSlip294A = new System.Windows.Forms.CheckBox();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbTransporterAc = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCity = new UltraCombo();
		this.btnAdd = new System.Windows.Forms.Button();
		this.CmbCategory = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadType = new System.Windows.Forms.RadioButton();
		this.CmbReserveWarehouse = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.RadBrandName = new System.Windows.Forms.RadioButton();
		this.RadBrandCode = new System.Windows.Forms.RadioButton();
		this.label72 = new System.Windows.Forms.Label();
		this.lblReserveWH = new System.Windows.Forms.Label();
		this.CmbBrandItemName = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.comPackUOM = new UltraCombo();
		this.comCropYear = new UltraCombo();
		this.lblBalance = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtWeightCutTotal = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.comWarehouse = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtratecuttotal = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.comPackingType = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label15 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtratecut = new System.Windows.Forms.TextBox();
		this.txtEmptybagsUnit = new System.Windows.Forms.TextBox();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.comRateUOM = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbOrderCategory2 = new UltraCombo();
		this.txtLedgerBalance = new System.Windows.Forms.TextBox();
		this.txtLedgerBalanceAfterBill = new System.Windows.Forms.TextBox();
		this.label68 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.txtInvoiceQty = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.comsupplier = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.panel11 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.datStockDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.comproject = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.comstockparty = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.compartyreference = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.comborderno = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
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
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btn294APrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint294B = new System.Windows.Forms.ToolStripButton();
		this.BtnItemSlip = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label66 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.tabFreight.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel4.SuspendLayout();
		this.tabExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel12.SuspendLayout();
		this.tabGL.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReserveWarehouse).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderCategory2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel18.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel20.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1312, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1304, 685);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1298, 679);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabFreight);
		this.tabControl2.Controls.Add(this.tabExpense);
		this.tabControl2.Controls.Add(this.tabGL);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 400);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1298, 279);
		this.tabControl2.TabIndex = 35;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel16);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Size = new System.Drawing.Size(1290, 250);
		this.tabDetail.TabIndex = 3;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1290, 223);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1290, 27);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1249, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabFreight.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.tabFreight.Controls.Add(this.panel4);
		this.tabFreight.Location = new System.Drawing.Point(4, 25);
		this.tabFreight.Name = "tabFreight";
		this.tabFreight.Padding = new System.Windows.Forms.Padding(3);
		this.tabFreight.Size = new System.Drawing.Size(1290, 250);
		this.tabFreight.TabIndex = 0;
		this.tabFreight.Text = "Charge To Product";
		this.tabFreight.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1284, 222);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdFreight).Tag = "GrdFreight";
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label67);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1284, 22);
		this.panel4.TabIndex = 2;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.White;
		this.label67.Location = new System.Drawing.Point(3, 3);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(107, 14);
		this.label67.TabIndex = 40;
		this.label67.Text = "Transporter Grid";
		this.tabExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabExpense.Controls.Add(this.panel12);
		this.tabExpense.Location = new System.Drawing.Point(4, 25);
		this.tabExpense.Name = "tabExpense";
		this.tabExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabExpense.Size = new System.Drawing.Size(1290, 250);
		this.tabExpense.TabIndex = 1;
		this.tabExpense.Text = "Other Expense";
		this.tabExpense.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1284, 223);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label37);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1284, 21);
		this.panel12.TabIndex = 31;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(5, 2);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 17);
		this.label37.TabIndex = 36;
		this.label37.Text = "Expense Grid";
		this.tabGL.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabGL.Controls.Add(this.panel15);
		this.tabGL.Location = new System.Drawing.Point(4, 25);
		this.tabGL.Name = "tabGL";
		this.tabGL.Size = new System.Drawing.Size(1290, 250);
		this.tabGL.TabIndex = 2;
		this.tabGL.Text = "Party Add/Less";
		this.tabGL.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1290, 225);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label53);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1290, 25);
		this.panel15.TabIndex = 33;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(6, 4);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(147, 17);
		this.label53.TabIndex = 36;
		this.label53.Text = "Party Add/Less Grid";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Controls.Add(this.txtdeliverydays);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.label44);
		this.panel3.Controls.Add(this.validate);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add(this.deliverystartdate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comstockparty);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.compartyreference);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comborderno);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1298, 375);
		this.panel3.TabIndex = 29;
		this.groupBox3.Controls.Add(this.chkPrint294B);
		this.groupBox3.Controls.Add(this.LabelCommissionDrCaption);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.ChkBok);
		this.groupBox3.Controls.Add(this.txtCommissionRemarks);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.chkPrintSlip294A);
		this.groupBox3.Controls.Add(this.ChkPrintslip);
		this.groupBox3.Controls.Add(this.ChkResetOnSave);
		this.groupBox3.Controls.Add(this.txtcommrate);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterAc);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.txtFreightAmount);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.txtcommamount);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(791, 33);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(498, 184);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Commission && Other";
		this.chkPrint294B.AutoSize = true;
		this.chkPrint294B.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkPrint294B.Location = new System.Drawing.Point(283, 143);
		this.chkPrint294B.Margin = new System.Windows.Forms.Padding(2);
		this.chkPrint294B.Name = "chkPrint294B";
		this.chkPrint294B.Size = new System.Drawing.Size(105, 19);
		this.chkPrint294B.TabIndex = 12;
		this.chkPrint294B.Text = "Print Slip 294B";
		this.chkPrint294B.UseVisualStyleBackColor = true;
		this.LabelCommissionDrCaption.AutoSize = true;
		this.LabelCommissionDrCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.LabelCommissionDrCaption.Location = new System.Drawing.Point(4, 160);
		this.LabelCommissionDrCaption.Name = "LabelCommissionDrCaption";
		this.LabelCommissionDrCaption.Size = new System.Drawing.Size(42, 15);
		this.LabelCommissionDrCaption.TabIndex = 94;
		this.LabelCommissionDrCaption.Text = "Dr A/c";
		this.LabelCommissionDrCaption.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(4, 22);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(39, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Agent";
		this.CmbCommissionDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance239).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Location = new System.Drawing.Point(58, 154);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Name = "CmbCommissionDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Visible = false;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAccount).Leave += new System.EventHandler(combcommAgent_Leave);
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(58, 16);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(140, 78);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(4, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(32, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Type";
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBok.Location = new System.Drawing.Point(283, 161);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(98, 19);
		this.ChkBok.TabIndex = 13;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(58, 101);
		this.txtCommissionRemarks.Multiline = true;
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(220, 51);
		this.txtCommissionRemarks.TabIndex = 5;
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
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(58, 72);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(4, 103);
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
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(58, 45);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(140, 51);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Rate";
		this.chkPrintSlip294A.AutoSize = true;
		this.chkPrintSlip294A.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkPrintSlip294A.Location = new System.Drawing.Point(283, 125);
		this.chkPrintSlip294A.Margin = new System.Windows.Forms.Padding(2);
		this.chkPrintSlip294A.Name = "chkPrintSlip294A";
		this.chkPrintSlip294A.Size = new System.Drawing.Size(106, 19);
		this.chkPrintSlip294A.TabIndex = 11;
		this.chkPrintSlip294A.Text = "Print Slip 294A";
		this.chkPrintSlip294A.UseVisualStyleBackColor = true;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkPrintslip.Location = new System.Drawing.Point(283, 109);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(98, 19);
		this.ChkPrintslip.TabIndex = 10;
		this.ChkPrintslip.Text = "Print Slip 294";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkResetOnSave.Location = new System.Drawing.Point(283, 92);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(152, 19);
		this.ChkResetOnSave.TabIndex = 9;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(191, 47);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(87, 23);
		this.txtcommrate.TabIndex = 2;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(283, 70);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(44, 15);
		this.label2.TabIndex = 109;
		this.label2.Text = "Freight";
		this.CmbTransporterAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporterAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Location = new System.Drawing.Point(283, 39);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Name = "CmbTransporterAc";
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).TabIndex = 7;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(4, 78);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "UOM";
		this.txtFreightAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreightAmount.Location = new System.Drawing.Point(332, 66);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.Size = new System.Drawing.Size(163, 23);
		this.txtFreightAmount.TabIndex = 8;
		this.txtFreightAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmount.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.txtFreightAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmount_KeyPress);
		this.label1.BackColor = System.Drawing.Color.LightGray;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(283, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(212, 26);
		this.label1.TabIndex = 107;
		this.label1.Text = "Transporter Ac";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(191, 74);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(87, 23);
		this.txtcommamount.TabIndex = 4;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(1386, 99);
		this.txtdeliverydays.Multiline = true;
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(78, 26);
		this.txtdeliverydays.TabIndex = 1;
		this.txtdeliverydays.Visible = false;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.groupBox1.Controls.Add(this.panel6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse);
		this.groupBox1.Controls.Add(this.panel5);
		this.groupBox1.Controls.Add(this.label72);
		this.groupBox1.Controls.Add(this.lblReserveWH);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandItemName);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comCropYear);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtWeightCutTotal);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.txtgatepassno);
		this.groupBox1.Controls.Add(this.txtwtcut);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.txtratecuttotal);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackingType);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.txtratecut);
		this.groupBox1.Controls.Add(this.txtEmptybagsUnit);
		this.groupBox1.Controls.Add(this.txtStockWeight);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.txtAddLss);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 217);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1071, 156);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
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
		((AppearanceBase)appearance266).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(572, 126);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 25;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(932, 127);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(36, 24);
		this.btnAdd.TabIndex = 27;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(211, 36);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.panel6.Controls.Add(this.RadCategory);
		this.panel6.Controls.Add(this.RadType);
		this.panel6.Location = new System.Drawing.Point(243, 14);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(138, 21);
		this.panel6.TabIndex = 113;
		this.RadCategory.AutoSize = true;
		this.RadCategory.Checked = true;
		this.RadCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCategory.Location = new System.Drawing.Point(4, 1);
		this.RadCategory.Name = "RadCategory";
		this.RadCategory.Size = new System.Drawing.Size(72, 19);
		this.RadCategory.TabIndex = 35;
		this.RadCategory.TabStop = true;
		this.RadCategory.Text = "Category";
		this.RadCategory.UseVisualStyleBackColor = true;
		this.RadCategory.CheckedChanged += new System.EventHandler(RadCategory_CheckedChanged);
		this.RadType.AutoSize = true;
		this.RadType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadType.Location = new System.Drawing.Point(80, 1);
		this.RadType.Name = "RadType";
		this.RadType.Size = new System.Drawing.Size(50, 19);
		this.RadType.TabIndex = 36;
		this.RadType.Text = "Type";
		this.RadType.UseVisualStyleBackColor = true;
		this.CmbReserveWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbReserveWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReserveWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReserveWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReserveWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReserveWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse).Location = new System.Drawing.Point(753, 126);
		((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse).Name = "CmbReserveWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbReserveWarehouse).TabIndex = 26;
		this.panel5.Controls.Add(this.RadBrandName);
		this.panel5.Controls.Add(this.RadBrandCode);
		this.panel5.Location = new System.Drawing.Point(632, 13);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(118, 23);
		this.panel5.TabIndex = 130;
		this.RadBrandName.AutoSize = true;
		this.RadBrandName.Checked = true;
		this.RadBrandName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandName.Location = new System.Drawing.Point(3, 2);
		this.RadBrandName.Name = "RadBrandName";
		this.RadBrandName.Size = new System.Drawing.Size(57, 19);
		this.RadBrandName.TabIndex = 55;
		this.RadBrandName.TabStop = true;
		this.RadBrandName.Text = "Name";
		this.RadBrandName.UseVisualStyleBackColor = true;
		this.RadBrandName.CheckedChanged += new System.EventHandler(RadBrandName_CheckedChanged);
		this.RadBrandCode.AutoSize = true;
		this.RadBrandCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandCode.Location = new System.Drawing.Point(63, 2);
		this.RadBrandCode.Name = "RadBrandCode";
		this.RadBrandCode.Size = new System.Drawing.Size(52, 19);
		this.RadBrandCode.TabIndex = 56;
		this.RadBrandCode.TabStop = true;
		this.RadBrandCode.Text = "Code";
		this.RadBrandCode.UseVisualStyleBackColor = true;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(211, 17);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(32, 15);
		this.label72.TabIndex = 112;
		this.label72.Text = "Item";
		this.lblReserveWH.AutoSize = true;
		this.lblReserveWH.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReserveWH.Location = new System.Drawing.Point(753, 110);
		this.lblReserveWH.Name = "lblReserveWH";
		this.lblReserveWH.Size = new System.Drawing.Size(112, 15);
		this.lblReserveWH.TabIndex = 82;
		this.lblReserveWH.Text = "Reserve WareHouse";
		this.CmbBrandItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance317).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance318).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance319).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrandItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Location = new System.Drawing.Point(594, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Name = "CmbBrandItemName";
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).TabIndex = 3;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1001, 128);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 23);
		this.btnCancelUpdateDetial.TabIndex = 29;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(572, 110);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(594, 17);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(38, 15);
		this.label70.TabIndex = 129;
		this.label70.Text = "Brand";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(476, 15);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(423, 15);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(932, 127);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 24);
		this.btnUpdateDetail.TabIndex = 28;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(135, 82);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.comCropYear.AllowNull = (DefaultableBoolean)1;
		this.comCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.comCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCropYear).Location = new System.Drawing.Point(803, 36);
		((System.Windows.Forms.Control)(object)this.comCropYear).Name = "comCropYear";
		((System.Windows.Forms.Control)(object)this.comCropYear).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.comCropYear).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.comCropYear).Leave += new System.EventHandler(comCropYear_Leave);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(533, 17);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(135, 66);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "Pack UOM";
		this.txtWeightCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutTotal.Enabled = false;
		this.txtWeightCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightCutTotal.Location = new System.Drawing.Point(572, 84);
		this.txtWeightCutTotal.Name = "txtWeightCutTotal";
		this.txtWeightCutTotal.Size = new System.Drawing.Size(75, 23);
		this.txtWeightCutTotal.TabIndex = 13;
		this.txtWeightCutTotal.TabStop = false;
		this.txtWeightCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(3, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 19;
		this.label9.Text = "Packing Type";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(214, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
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
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(385, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(comItem_Leave);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(282, 84);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(102, 23);
		this.txtGrossWeight.TabIndex = 9;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(575, 66);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(54, 15);
		this.label52.TabIndex = 79;
		this.label52.Text = "Cut Total";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(385, 17);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(211, 84);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(3, 17);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(282, 66);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 49;
		this.label39.Text = "Gross Weight";
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(385, 128);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(55, 23);
		this.txtgatepassno.TabIndex = 23;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcut.Location = new System.Drawing.Point(516, 84);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(55, 23);
		this.txtwtcut.TabIndex = 12;
		this.txtwtcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwtcut.TextChanged += new System.EventHandler(txtwtcut_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(902, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(441, 110);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(803, 17);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(57, 15);
		this.label41.TabIndex = 53;
		this.label41.Text = "Crop Year";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(441, 128);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(130, 23);
		this.txtvehicleno.TabIndex = 24;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(902, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(64, 110);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(80, 15);
		this.label50.TabIndex = 74;
		this.label50.Text = "Rate Cut Total";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(385, 110);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(516, 66);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(44, 15);
		this.label46.TabIndex = 67;
		this.label46.Text = "Wt.Cut";
		this.txtratecuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecuttotal.Enabled = false;
		this.txtratecuttotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtratecuttotal.Location = new System.Drawing.Point(64, 128);
		this.txtratecuttotal.Name = "txtratecuttotal";
		this.txtratecuttotal.Size = new System.Drawing.Size(86, 23);
		this.txtratecuttotal.TabIndex = 20;
		this.txtratecuttotal.TabStop = false;
		this.txtratecuttotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(282, 110);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.comPackingType.AllowNull = (DefaultableBoolean)1;
		this.comPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.comPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackingType).Location = new System.Drawing.Point(3, 82);
		((System.Windows.Forms.Control)(object)this.comPackingType).Name = "comPackingType";
		((System.Windows.Forms.Control)(object)this.comPackingType).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.comPackingType).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.comPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(282, 128);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(102, 22);
		this.txtgpdate.TabIndex = 22;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(385, 66);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 15);
		this.label15.TabIndex = 23;
		this.label15.Text = "E.B /Unit";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(3, 110);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(51, 15);
		this.label49.TabIndex = 72;
		this.label49.Text = "Rate Cut";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(902, 66);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(30, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(441, 84);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(74, 23);
		this.txtEmptyBagsTotal.TabIndex = 11;
		this.txtEmptyBagsTotal.Tag = "PmTotal";
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.txtEmptyBagsTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptyBagsTotal_KeyPress);
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(151, 128);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(130, 23);
		this.txtAmount.TabIndex = 21;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(444, 66);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(52, 15);
		this.label16.TabIndex = 25;
		this.label16.Text = "E.B Total";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(151, 110);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(902, 84);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(83, 23);
		this.txtRate.TabIndex = 17;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtratecut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratecut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtratecut.Location = new System.Drawing.Point(3, 128);
		this.txtratecut.Name = "txtratecut";
		this.txtratecut.Size = new System.Drawing.Size(59, 23);
		this.txtratecut.TabIndex = 19;
		this.txtratecut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtratecut.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtratecut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.txtEmptybagsUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptybagsUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmptybagsUnit.Location = new System.Drawing.Point(385, 84);
		this.txtEmptybagsUnit.Name = "txtEmptybagsUnit";
		this.txtEmptybagsUnit.Size = new System.Drawing.Size(55, 23);
		this.txtEmptybagsUnit.TabIndex = 10;
		this.txtEmptybagsUnit.Tag = "PmUnit";
		this.txtEmptybagsUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptybagsUnit.TextChanged += new System.EventHandler(txtEmptybagsUnit_TextChanged);
		this.txtEmptybagsUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptybagsUnit_KeyPress);
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStockWeight.Location = new System.Drawing.Point(803, 84);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.Size = new System.Drawing.Size(97, 23);
		this.txtStockWeight.TabIndex = 16;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockWeight.TextChanged += new System.EventHandler(txtStockWeight_TextChanged);
		this.txtStockWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockWeight_KeyPress);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(987, 66);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(648, 66);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(38, 15);
		this.label30.TabIndex = 31;
		this.label30.Text = "Ad/Ls";
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(987, 82);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(704, 66);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 43;
		this.label36.Text = "Net Bill Weight";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Enabled = false;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetBillWeight.Location = new System.Drawing.Point(704, 84);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(97, 23);
		this.txtNetBillWeight.TabIndex = 15;
		this.txtNetBillWeight.TabStop = false;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLss.Location = new System.Drawing.Point(648, 84);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(55, 23);
		this.txtAddLss.TabIndex = 14;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(803, 66);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(73, 15);
		this.label34.TabIndex = 35;
		this.label34.Text = "Stock Wight";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1296, 72);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(66, 17);
		this.label45.TabIndex = 45;
		this.label45.Text = "Start Date";
		this.label45.Visible = false;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOrderCategory2);
		this.groupBox2.Controls.Add(this.txtLedgerBalance);
		this.groupBox2.Controls.Add(this.txtLedgerBalanceAfterBill);
		this.groupBox2.Controls.Add(this.label68);
		this.groupBox2.Controls.Add(this.label69);
		this.groupBox2.Controls.Add(this.txtBranchSrNo);
		this.groupBox2.Controls.Add(this.txtInvoiceWeight);
		this.groupBox2.Controls.Add(this.txtInvoiceQty);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtSupplierReference);
		this.groupBox2.Controls.Add(this.txtBillAmount);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(7, 33);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(782, 184);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.cmbOrderCategory2.AllowNull = (DefaultableBoolean)1;
		this.cmbOrderCategory2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOrderCategory2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOrderCategory2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOrderCategory2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOrderCategory2).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOrderCategory2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOrderCategory2).Location = new System.Drawing.Point(95, 99);
		((System.Windows.Forms.Control)(object)this.cmbOrderCategory2).Name = "cmbOrderCategory2";
		((System.Windows.Forms.Control)(object)this.cmbOrderCategory2).Size = new System.Drawing.Size(267, 26);
		((System.Windows.Forms.Control)(object)this.cmbOrderCategory2).TabIndex = 5;
		this.txtLedgerBalance.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalance.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalance.Location = new System.Drawing.Point(668, 17);
		this.txtLedgerBalance.Name = "txtLedgerBalance";
		this.txtLedgerBalance.ReadOnly = true;
		this.txtLedgerBalance.Size = new System.Drawing.Size(113, 24);
		this.txtLedgerBalance.TabIndex = 13;
		this.txtLedgerBalance.TabStop = false;
		this.txtLedgerBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLedgerBalanceAfterBill.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalanceAfterBill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalanceAfterBill.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalanceAfterBill.Location = new System.Drawing.Point(668, 74);
		this.txtLedgerBalanceAfterBill.Name = "txtLedgerBalanceAfterBill";
		this.txtLedgerBalanceAfterBill.ReadOnly = true;
		this.txtLedgerBalanceAfterBill.Size = new System.Drawing.Size(113, 24);
		this.txtLedgerBalanceAfterBill.TabIndex = 15;
		this.txtLedgerBalanceAfterBill.TabStop = false;
		this.txtLedgerBalanceAfterBill.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(569, 79);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(103, 15);
		this.label68.TabIndex = 134;
		this.label68.Text = "After Bill Balance";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(569, 22);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(92, 15);
		this.label69.TabIndex = 135;
		this.label69.Text = "Ledger Balance";
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(167, 18);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(78, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(454, 129);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.ShortcutsEnabled = false;
		this.txtInvoiceWeight.Size = new System.Drawing.Size(110, 23);
		this.txtInvoiceWeight.TabIndex = 12;
		this.txtInvoiceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInvoiceQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceQty.Enabled = false;
		this.txtInvoiceQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceQty.Location = new System.Drawing.Point(454, 102);
		this.txtInvoiceQty.Name = "txtInvoiceQty";
		this.txtInvoiceQty.ShortcutsEnabled = false;
		this.txtInvoiceQty.Size = new System.Drawing.Size(110, 23);
		this.txtInvoiceQty.TabIndex = 11;
		this.txtInvoiceQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(367, 133);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(88, 15);
		this.label62.TabIndex = 129;
		this.label62.Text = "Invoice Weight";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(367, 106);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(68, 15);
		this.label63.TabIndex = 130;
		this.label63.Text = "Invoice Qty";
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
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(668, 101);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(113, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(668, 156);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(113, 23);
		this.txtFcyAmount.TabIndex = 18;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(668, 129);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(113, 23);
		this.txtExchangeRate.TabIndex = 17;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(569, 160);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(72, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Fcy Amount";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(569, 133);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(84, 15);
		this.label60.TabIndex = 127;
		this.label60.Text = "Exchange Rate";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(569, 106);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(55, 15);
		this.label61.TabIndex = 128;
		this.label61.Text = "Fcy Code";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(569, 50);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(82, 17);
		this.label56.TabIndex = 125;
		this.label56.Text = "Bill Amount";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(71, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(246, 18);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(116, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(95, 75);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(130, 23);
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
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(95, 127);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(267, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(3, 107);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(87, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Other Category";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(454, 73);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 72);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 30);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual / \r\nParty Ref";
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(226, 75);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(136, 23);
		this.txtSupplierReference.TabIndex = 4;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtBillAmount.Location = new System.Drawing.Point(668, 46);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(113, 24);
		this.txtBillAmount.TabIndex = 14;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(95, 156);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(469, 23);
		this.txtremarks.TabIndex = 7;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(3, 160);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(94, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Invoice Remarks";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Party Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(3, 133);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Terms";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(454, 18);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(110, 23);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance284;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance285;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance177).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance293).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(95, 45);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(267, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 3;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(367, 22);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(367, 51);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(57, 15);
		this.label43.TabIndex = 99;
		this.label43.Text = "Due Date";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(454, 47);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(110, 23);
		this.duedate.TabIndex = 9;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(367, 79);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1296, 49);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(81, 17);
		this.label44.TabIndex = 44;
		this.label44.Text = "Validite Date";
		this.label44.Visible = false;
		this.validate.CustomFormat = "dd-MM-yy";
		this.validate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1386, 41);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(78, 25);
		this.validate.TabIndex = 19;
		this.validate.Visible = false;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.btnRecordsUpdate);
		this.panel11.Controls.Add(this.chkCustomAccounts);
		this.panel11.Controls.Add(this.datStockDate);
		this.panel11.Controls.Add(this.label17);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1298, 31);
		this.panel11.TabIndex = 110;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(1140, 5);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(153, 20);
		this.chkCustomAccounts.TabIndex = 513;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.datStockDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.datStockDate.CustomFormat = "dd-MM-yy";
		this.datStockDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datStockDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datStockDate.Location = new System.Drawing.Point(975, 3);
		this.datStockDate.Name = "datStockDate";
		this.datStockDate.Size = new System.Drawing.Size(143, 23);
		this.datStockDate.TabIndex = 131;
		this.datStockDate.Visible = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 6);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(228, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "SALE DIRECT INVOICE       ";
		this.deliverystartdate.CustomFormat = "dd-MM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(1386, 72);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(78, 25);
		this.deliverystartdate.TabIndex = 21;
		this.deliverystartdate.Visible = false;
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
		((AppearanceBase)appearance306).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1386, 162);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1296, 100);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(86, 17);
		this.label27.TabIndex = 49;
		this.label27.Text = "Delivery Days";
		this.label27.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance311;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1386, 194);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comstockparty.AllowNull = (DefaultableBoolean)1;
		this.comstockparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.comstockparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comstockparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comstockparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comstockparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comstockparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comstockparty).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comstockparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comstockparty).Location = new System.Drawing.Point(1386, 254);
		((System.Windows.Forms.Control)(object)this.comstockparty).Name = "comstockparty";
		((System.Windows.Forms.Control)(object)this.comstockparty).Size = new System.Drawing.Size(78, 27);
		((System.Windows.Forms.Control)(object)this.comstockparty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comstockparty).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1296, 200);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.compartyreference.AllowNull = (DefaultableBoolean)1;
		this.compartyreference.AutoCompleteMode = (AutoCompleteMode)4;
		this.compartyreference.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.compartyreference.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.compartyreference).DisplayLayout.Appearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.compartyreference).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance272;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance279).TextHAlignAsString = "Left";
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.compartyreference).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.compartyreference).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.compartyreference.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.compartyreference).Location = new System.Drawing.Point(1386, 226);
		((System.Windows.Forms.Control)(object)this.compartyreference).Name = "compartyreference";
		((System.Windows.Forms.Control)(object)this.compartyreference).Size = new System.Drawing.Size(78, 27);
		((System.Windows.Forms.Control)(object)this.compartyreference).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.compartyreference).Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1296, 168);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1346, 226);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1366, 226);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.comborderno.AllowNull = (DefaultableBoolean)1;
		this.comborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.comborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.comborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance244;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance250).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comborderno).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comborderno).Location = new System.Drawing.Point(1386, 130);
		((System.Windows.Forms.Control)(object)this.comborderno).Name = "comborderno";
		((System.Windows.Forms.Control)(object)this.comborderno).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comborderno).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comborderno).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Visible = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Leave += new System.EventHandler(comborderno_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1296, 130);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Po #";
		this.label3.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[19]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.btn294APrint, this.btnPrint294B, this.BtnItemSlip, this.btnPrint, this.toolStripSeparator5, this.btnLoadGdn, this.toolStripSeparator6, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1298, 25);
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
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
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
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(88, 22);
		this.btnSlip.Text = "294-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btn294APrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn294APrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn294APrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn294APrint.Name = "btn294APrint";
		this.btn294APrint.Size = new System.Drawing.Size(97, 22);
		this.btn294APrint.Text = "294A-Print";
		this.btn294APrint.Click += new System.EventHandler(btn294APrint_Click);
		this.btnPrint294B.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint294B.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint294B.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint294B.Name = "btnPrint294B";
		this.btnPrint294B.Size = new System.Drawing.Size(96, 22);
		this.btnPrint294B.Text = "294B-Print";
		this.btnPrint294B.ToolTipText = "294B-InvRptSaleBillDirectWithoutSO";
		this.btnPrint294B.Click += new System.EventHandler(btnPrint294B_Click);
		this.BtnItemSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnItemSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnItemSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemSlip.Name = "BtnItemSlip";
		this.BtnItemSlip.Size = new System.Drawing.Size(125, 22);
		this.BtnItemSlip.Text = "294C-Item Slip";
		this.BtnItemSlip.Click += new System.EventHandler(BtnItemSlip_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(88, 22);
		this.btnLoadGdn.Text = "Load Gdn";
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
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
		this.tabPage2.Size = new System.Drawing.Size(1304, 685);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 413);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1298, 269);
		this.panel17.TabIndex = 49;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1298, 242);
		this.panel19.TabIndex = 49;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1298, 242);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1298, 27);
		this.panel18.TabIndex = 48;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1257, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
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
		this.panel10.Controls.Add(this.panel21);
		this.panel10.Controls.Add(this.panel20);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1298, 356);
		this.panel10.TabIndex = 48;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 66);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1298, 290);
		this.panel21.TabIndex = 47;
		this.grdHistory.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1298, 290);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel20.Controls.Add(this.groupBox4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1298, 66);
		this.panel20.TabIndex = 0;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox4.Controls.Add(this.label66);
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add(this.label51);
		this.groupBox4.Controls.Add(this.label57);
		this.groupBox4.Controls.Add(this.label58);
		this.groupBox4.Controls.Add(this.label64);
		this.groupBox4.Controls.Add(this.label65);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(5, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1248, 61);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 29);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 91;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(6, 12);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(79, 15);
		this.label66.TabIndex = 90;
		this.label66.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1039, 34);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1039, 12);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(958, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(958, 34);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(885, 30);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(649, 14);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(58, 15);
		this.label51.TabIndex = 87;
		this.label51.Text = "Customer";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(569, 14);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(64, 15);
		this.label57.TabIndex = 86;
		this.label57.Text = "To Doc No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(489, 14);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(79, 15);
		this.label58.TabIndex = 85;
		this.label58.Text = "From Doc No";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(352, 14);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(48, 15);
		this.label64.TabIndex = 84;
		this.label64.Text = "To Date";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(215, 14);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(63, 15);
		this.label65.TabIndex = 83;
		this.label65.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance277).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance277).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance278;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance287).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance288).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(649, 30);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(569, 32);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(489, 32);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(352, 32);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(215, 32);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1298, 27);
		this.panel9.TabIndex = 47;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1257, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
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
		this.panel2.Size = new System.Drawing.Size(1298, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1298, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(73, 22);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(200, 6);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 514;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1312, 711);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "InvfrmInvSaleInvoiceDirect";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabFreight.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tabExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabGL.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReserveWarehouse).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderCategory2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
