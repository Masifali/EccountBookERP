using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lab;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Production
{

public class ProductionPreCostingAndJobOrder : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtPlannedOutput = new DataTable();

	private DataTable dtInputDetail = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtOH = new DataTable();

	private DataTable dtPM = new DataTable();

	private DataTable dtPlantdetail = new DataTable();

	private int updateDetailIndexoutput = -1;

	private int updateDetailIndexOH = -1;

	private int updateDetailIndexInput = -1;

	private int updateDetailIndexPM = -1;

	private int updatePlantDetailIndex = -1;

	private DataTable dtSupplier = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtLabourAc = new DataTable();

	private DataTable dtAllUomWithItemId = new DataTable();

	private DataTable dtLabDetail = new DataTable();

	private DataTable dtPartiesFromStandard = new DataTable();

	public Rightsobjects formright = new Rightsobjects();

	private bool UpdateMode;

	private bool JobOrderCreatewithoutRates;

	private bool CheckStockAndGiveWarningMessageOnJobOrder;

	private int RECID;

	private int GetLastWipAccountId;

	private int GetLastWipItemId;

	private int GetLastWipWareHouseId;

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private List<InvProductionJobOrderInput> lstRemoveInputRecord = new List<InvProductionJobOrderInput>();

	private List<InvProductionJobOrderOutput> lstRemoveOutputRecord = new List<InvProductionJobOrderOutput>();

	private List<InvProductionJobOrderOverHeads> lstRemoveOHRecord = new List<InvProductionJobOrderOverHeads>();

	private List<InvProductionJobOrderPackingMaterial> lstRemovePMRecord = new List<InvProductionJobOrderPackingMaterial>();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private TabPage tabPage2;

	private Panel panel4;

	private Label label10;

	private Label label6;

	private Label label1;

	private TextBox txtInvoiceReference;

	private Label label2;

	private Label label8;

	private Label label7;

	private TextBox txtPlanCode;

	private DateTimePicker txtPlanDate;

	private Panel panel6;

	private Label label13;

	private DateTimePicker txtstartDate;

	private Label label16;

	private DateTimePicker txtEndDate;

	private Label label18;

	internal UltraCombo cmbPlantFeader;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	internal GridEX grdPlannedOutput;

	internal GridEX grdInputDetail;

	private ToolStripButton btnattachment;

	internal UltraCombo cmbPlanType;

	internal UltraCombo cmbProductionType;

	internal UltraCombo cmbStatus;

	private CheckBox chkPrint;

	internal UltraCombo CmbWorkInProcess;

	private Label label15;

	internal UltraCombo CmbWipItem;

	private Label label23;

	internal UltraCombo cmbItem;

	private Label label37;

	internal UltraCombo cmbPackingType;

	private Label label35;

	private TextBox txtOuterqty;

	private Label label26;

	internal UltraCombo cmbouteruom;

	private Label label25;

	private TextBox txtremarks;

	private Label label27;

	private TextBox txtweight;

	private Label label39;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Button btnCancelUpdateDetial;

	internal UltraCombo cmbFiltersType;

	private Label cmbfiltertype;

	internal UltraCombo cmbcropyearinput;

	private Label label30;

	internal UltraCombo cmbItemInput;

	private Label label29;

	private TextBox txtweightinput;

	private Label label33;

	private TextBox txtqtyinput;

	private Label label31;

	internal UltraCombo cmbpacksizeinput;

	private Label label32;

	internal UltraCombo cmbjoblotinput;

	private Label label45;

	private TextBox txtremarksinput;

	private Label label36;

	internal UltraCombo cmbwarehouseinput;

	private Label label34;

	private Button btncancelgrdinput;

	private Button btnupdategrdinput;

	private Button btnadddetailinputgrd;

	private Label label40;

	internal UltraCombo cmbentrytype;

	private Panel panelinputgrd;

	private Panel paneloutputgrd;

	private Button btnProductionPlant;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel14;

	private TabPage tabPage4;

	private Panel panel15;

	private ToolStripButton btnRefresh;

	private Panel panel7;

	private Panel panel9;

	internal UltraCombo cmbWareHouseOutPut;

	private Label label12;

	private Label label14;

	internal UltraCombo cmbCropYearOutput;

	private Label label41;

	internal UltraCombo cmbJobLotOutput;

	private Label label42;

	internal UltraCombo cmbPackTypeInput;

	private TextBox txtAmountInPut;

	private TextBox txtRateInPut;

	private Label label9;

	private Label label3;

	private Label label19;

	internal UltraCombo CmbRateUomInPut;

	internal UltraCombo cmbDocNo;

	private Label label28;

	private TextBox txtItemAmountOutPut;

	private TextBox txtItemRateOutPut;

	private Label label44;

	private Label label46;

	private Label label47;

	internal UltraCombo CmbRateUomOutPut;

	private ToolStripButton btnLoadInvoices;

	private TabPage tabPage5;

	private Panel panel10;

	private TabPage tabPage6;

	private Panel panel16;

	private ToolStripButton btnGenerate;

	private GroupBox groupBox1;

	private TextBox txtTotalAmountBP;

	private TextBox txtTotalAmountInPut;

	private TextBox txtOverHeadsAmount;

	private TextBox txtPackingMaterialAmount;

	private TextBox txtTotalWeightBP;

	private TextBox txtTotalQtyBP;

	private TextBox txtInPutTotalWeight;

	private TextBox txtInPutTotalQty;

	private TextBox txtTotalAmountFG;

	private TextBox txtTotalWeightFG;

	private TextBox txtTotalQtyFG;

	private TextBox txtFGAvgRate;

	private TextBox txtFGRateWithoutExp;

	private Label label69;

	internal UltraCombo CmbEntryTypeOP;

	private Label label70;

	private GroupBox groupBox2;

	private Label label72;

	private Label label71;

	private Label label4;

	private GroupBox groupBox3;

	private Panel panel51;

	private Panel panel52;

	private Label label11;

	private Label label17;

	private Label label73;

	private Label label74;

	private Label label75;

	private Label label76;

	private Label label78;

	private Panel panel55;

	private Panel panel56;

	private Label label54;

	private Label label58;

	private Label label55;

	private Panel panel53;

	private Panel panel54;

	private Label label94;

	private Label label56;

	private Label label57;

	private GroupBox groupBox7;

	private Button btnAddOH;

	private Button BtnCancelOH;

	private Button btnUpdateOH;

	private Label lblRateOverHead;

	private Label lblRemarksOverHead;

	private TextBox txtLedgerRemarksOverHead;

	private TextBox txtItemRateOverHeads;

	private Label label59;

	private Label label60;

	internal UltraCombo cmbChartofAccountOverHead;

	private Label label61;

	private TextBox txtItemQtyOverHead;

	internal UltraCombo CmbBrandNameOverHead;

	private Label label62;

	internal UltraCombo CmbBrandUomOverHead;

	private Label lblAmountOverHead;

	private TextBox txtExpAmountOverHead;

	private GroupBox groupBox8;

	private Button btnAddDetailPm;

	private Button btnCancelDetailPm;

	private Button btnUpdateDetailPm;

	private Label label79;

	private TextBox txtremarksPackingMaterial;

	private Label lblRemarksPackingMaterial;

	internal UltraCombo CmbWarehousePackingMaterial;

	private TextBox txtPackingMaterialRate;

	internal UltraCombo cmbItemPackingMaterial;

	private Label label63;

	private Label label64;

	internal UltraCombo CmbBrandUomPackingMaterial;

	private Label lblRatePackingMaterial;

	internal UltraCombo CmbBrandNamePackingMaterial;

	private TextBox txtPackingMaterialQTY;

	private TextBox txtAmountPackingMaterial;

	private Label label65;

	private Label lblAmountPackingMaterial;

	private Label label66;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdOverHead;

	private Panel panel17;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label67;

	private TextBox txtWagesAmountInPut;

	private Label label43;

	private TextBox txtPmAmount;

	private TextBox txtWagesRateInPut;

	private Label label21;

	private TextBox txtWagesAmountOutPut;

	internal UltraCombo CmbLotAc;

	private Label label53;

	private Label label22;

	private Label label48;

	private TextBox txtPmRate;

	internal UltraCombo CmbLabourItemOutPut;

	private Label label52;

	private Label label50;

	private TextBox txtWagesRateOutPut;

	private Label label51;

	internal UltraCombo CmbLabourItemInPut;

	internal UltraCombo CmbPackingMaterial;

	private Label label20;

	private Label label49;

	internal GridEX grdPackingMaterial;

	private Panel panel18;

	private Label label68;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label77;

	private CtrlGrdBar ctrlGrdBar5;

	private TabPage tabPage7;

	internal GridEX grdPlantdetail;

	private Panel panel22;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label85;

	private Panel panel23;

	private GroupBox groupBox4;

	private Button btnAddPlantDetail;

	private Button btnCanclePlantdetail;

	private Button btnUpdatePlantDetail;

	private TextBox txtRemarksPlantDetail;

	private Label label87;

	private ToolStripButton BtnGenerateInPutRates;

	private TextBox txtOtherInstruction;

	private TextBox txtDeliveryInstruction;

	private TextBox txtPlantInstruction;

	private TextBox txtProductionInstruction;

	private ToolStripButton btnSaveAs;

	private TabControl tabControl3;

	private TabPage tabPage8;

	internal GridEX grdInputHistory;

	private Panel panel12;

	private Label label80;

	private TabPage tabPage9;

	internal GridEX grdOutPutHistory;

	private Panel panel5;

	private Label label81;

	private TabPage tabPage11;

	internal GridEX grdOverheadHistory;

	private Panel panel19;

	private Label label82;

	private TabPage tabPage10;

	internal GridEX grdPackingMaterialHistory;

	private Panel panel21;

	private Label label83;

	private Panel panel11;

	private Label label5;

	private Panel panel20;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label24;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label38;

	internal UltraCombo cmbperemeter;

	private Label label84;

	private Button btnshowHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label86;

	internal UltraCombo xmbjoborderno;

	private Label label88;

	private ToolStripButton BtnRefereshHistory;

	private ToolStripButton BtnShortcutKeys;

	private TextBox txtPrctOfInputforOutputQty;

	private Label label89;

	internal UltraCombo CmbWipWarehouse;

	private Label label90;

	private Button btnProductionTypeDefine;

	private Label lblStockWeight;

	private TabPage tabLabStandard;

	internal GridEX grdLabStandards;

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar7;

	private Panel panel24;

	private GroupBox groupBox5;

	private Panel panel25;

	private RadioButton RadBusinessName;

	private RadioButton RadNickName;

	private RadioButton radPartyCode;

	private Label label91;

	internal UltraCombo CmbBuyerName;

	internal UltraCombo CmbAnalysisGroup;

	private Label label93;

	internal UltraCombo CmbBrandNameLabStandard;

	private Label label95;

	private Button BtnDefineLabStandards;

	private Label label92;

	private TextBox txtAssumedPlantCapacity;

	private ContextMenuStrip contextMenuStrip1;

	public ProductionPreCostingAndJobOrder(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmProductionJobOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1080, 750);
			dtViewAttachments.Columns.Add("AttchmentPath");
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveAs.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			Print.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			if (dtSupplier.Columns.Count == 0)
			{
				dtSupplier.Columns.Add("Id", typeof(int));
				dtSupplier.Columns.Add("CompanyName", typeof(string));
				dtSupplier.Columns.Add("PartyCode", typeof(string));
				dtSupplier.Columns.Add("GlAccountId", typeof(int));
				dtSupplier.Columns.Add("CityName", typeof(string));
				dtSupplier.Columns.Add("MobileNo", typeof(string));
			}
			DataTable dataTable = dtLabDetail;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtLabDetail.Columns.Add("Id", typeof(int));
				dtLabDetail.Columns.Add("AnalysisGroupId", typeof(int));
				dtLabDetail.Columns.Add("AnalysisGroup", typeof(string));
				dtLabDetail.Columns.Add("AnalysisItemId", typeof(int));
				dtLabDetail.Columns.Add("AnalysisItem", typeof(string));
				dtLabDetail.Columns.Add("MinValue", typeof(double));
				dtLabDetail.Columns.Add("MaxValue", typeof(double));
				dtLabDetail.Columns.Add("Active", typeof(bool));
				dtLabDetail.Columns.Add("MainId", typeof(int));
			}
			BindGrids();
			JobOrderCreatewithoutRates = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("JobOrderCreatewithoutRates"));
			CheckStockAndGiveWarningMessageOnJobOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockAndGiveWarningMessageOnJobOrder"));
			if (JobOrderCreatewithoutRates)
			{
				txtRateInPut.Enabled = false;
				((Control)(object)CmbRateUomInPut).Enabled = false;
				txtItemRateOutPut.Enabled = false;
				((Control)(object)CmbRateUomOutPut).Enabled = false;
				txtItemRateOverHeads.Enabled = false;
				txtExpAmountOverHead.Enabled = false;
				txtPackingMaterialRate.Enabled = false;
				txtPackingMaterialAmount.Enabled = false;
				BtnGenerateInPutRates.Enabled = false;
			}
			dtPartiesFromStandardDbCall();
			SupplierDtFillFromStandard();
			BindSupplierName();
			AllUomWithItemFill();
			ItemFill();
			JobLotBind();
			AccountFills();
			PlanTypeBind();
			GeneratePlanCode();
			PlantFeader();
			ProductionTypeBind();
			StatusBind();
			EntryTypeFill();
			CropYear();
			WareHouseFill();
			ItemDetailFill();
			CmbPackingTypeFill();
			FilterTypeFill();
			WagesAccountFill();
			PackingMaterialItemBind();
			EntryTypeOutPut();
			ChartOfAccountOverHead();
			GetLastSaveAccount();
			if (GetLastWipAccountId > 0)
			{
				CmbWorkInProcess.Value = GetLastWipAccountId;
			}
			if (GetLastWipItemId > 0)
			{
				CmbWipItem.Value = GetLastWipItemId;
			}
			if (GetLastWipWareHouseId > 0)
			{
				CmbWipWarehouse.Value = GetLastWipWareHouseId;
			}
			ParameterFill();
			HistoryJobOrderNofill();
			dtPlannedOutput.Columns.Add("Id", typeof(int));
			dtPlannedOutput.Columns.Add("EntryTypeId", typeof(int));
			dtPlannedOutput.Columns.Add("EntryType", typeof(string));
			dtPlannedOutput.Columns.Add("WareHouseId", typeof(int));
			dtPlannedOutput.Columns.Add("WareHouseName", typeof(string));
			dtPlannedOutput.Columns.Add("JobLotId", typeof(int));
			dtPlannedOutput.Columns.Add("JobLotDescription", typeof(string));
			dtPlannedOutput.Columns.Add("ItemId", typeof(int));
			dtPlannedOutput.Columns.Add("ItemName", typeof(string));
			dtPlannedOutput.Columns.Add("CropYearId", typeof(int));
			dtPlannedOutput.Columns.Add("CropYear", typeof(string));
			dtPlannedOutput.Columns.Add("PackingTypeId", typeof(int));
			dtPlannedOutput.Columns.Add("PackingTypeName", typeof(string));
			dtPlannedOutput.Columns.Add("QtyOuter", typeof(double));
			dtPlannedOutput.Columns.Add("OuterPackSizeId", typeof(int));
			dtPlannedOutput.Columns.Add("OuterPackSizeName", typeof(string));
			dtPlannedOutput.Columns.Add("EquivalentPack", typeof(double));
			dtPlannedOutput.Columns.Add("Weight", typeof(double));
			dtPlannedOutput.Columns.Add("ItemRate", typeof(double));
			dtPlannedOutput.Columns.Add("RateUomId", typeof(int));
			dtPlannedOutput.Columns.Add("RateUom", typeof(string));
			dtPlannedOutput.Columns.Add("EquivalentRate", typeof(double));
			dtPlannedOutput.Columns.Add("ItemAmount", typeof(double));
			dtPlannedOutput.Columns.Add("OHExpense", typeof(double));
			dtPlannedOutput.Columns.Add("PMExpense", typeof(double));
			dtPlannedOutput.Columns.Add("OHGeneralExpense", typeof(double));
			dtPlannedOutput.Columns.Add("PMGeneralExpense", typeof(double));
			dtPlannedOutput.Columns.Add("NetRate", typeof(double));
			dtPlannedOutput.Columns.Add("NetAmount", typeof(double));
			dtPlannedOutput.Columns.Add("Remarks", typeof(string));
			grdPlannedOutput.DataSource = dtPlannedOutput;
			grdPlannedOutput.RetrieveStructure();
			PlannedOutputGridSetting();
			dtInputDetail.Columns.Add("Id", typeof(int));
			dtInputDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtInputDetail.Columns.Add("RefDocIdNo", typeof(int));
			dtInputDetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtInputDetail.Columns.Add("DocDate", typeof(DateTime));
			dtInputDetail.Columns.Add("DocNo", typeof(int));
			dtInputDetail.Columns.Add("WareHouseId", typeof(int));
			dtInputDetail.Columns.Add("WareHouse", typeof(string));
			dtInputDetail.Columns.Add("JobLotId", typeof(int));
			dtInputDetail.Columns.Add("JobLot", typeof(string));
			dtInputDetail.Columns.Add("ItemId", typeof(int));
			dtInputDetail.Columns.Add("ItemName", typeof(string));
			dtInputDetail.Columns.Add("CropYearId", typeof(int));
			dtInputDetail.Columns.Add("CropYear", typeof(string));
			dtInputDetail.Columns.Add("PackTypeId", typeof(int));
			dtInputDetail.Columns.Add("PackType", typeof(string));
			dtInputDetail.Columns.Add("BalanceQty", typeof(double));
			dtInputDetail.Columns.Add("BalanceWeight", typeof(double));
			dtInputDetail.Columns.Add("Qty", typeof(double));
			dtInputDetail.Columns.Add("PackSizeId", typeof(int));
			dtInputDetail.Columns.Add("PackSize", typeof(string));
			dtInputDetail.Columns.Add("PackSizeEquivalent", typeof(double));
			dtInputDetail.Columns.Add("Weight", typeof(double));
			dtInputDetail.Columns.Add("ItemRate", typeof(double));
			dtInputDetail.Columns.Add("RateUomId", typeof(int));
			dtInputDetail.Columns.Add("RateUom", typeof(string));
			dtInputDetail.Columns.Add("RateUomEquivalent", typeof(double));
			dtInputDetail.Columns.Add("ItemAmount", typeof(double));
			dtInputDetail.Columns.Add("Remarks", typeof(string));
			grdInputDetail.DataSource = dtInputDetail;
			grdInputDetail.RetrieveStructure();
			InputDetailGridSetting();
			dtOH.Columns.Add("Id", typeof(int));
			dtOH.Columns.Add("ChartOfAccountId", typeof(int));
			dtOH.Columns.Add("ChartOfAccount", typeof(string));
			dtOH.Columns.Add("BrandId", typeof(int));
			dtOH.Columns.Add("Brand", typeof(string));
			dtOH.Columns.Add("BrandUomId", typeof(int));
			dtOH.Columns.Add("BrandUom", typeof(string));
			dtOH.Columns.Add("ItemQty", typeof(double));
			dtOH.Columns.Add("ItemRate", typeof(double));
			dtOH.Columns.Add("Amount", typeof(double));
			dtOH.Columns.Add("Remarks", typeof(string));
			grdOverHead.DataSource = dtOH;
			grdOverHead.RetrieveStructure();
			grdDetailOverheadSettings();
			dtPM.Columns.Add("Id", typeof(int));
			dtPM.Columns.Add("WarehouseId", typeof(int));
			dtPM.Columns.Add("Warehouse", typeof(string));
			dtPM.Columns.Add("PackingItemId", typeof(int));
			dtPM.Columns.Add("PackingItem", typeof(string));
			dtPM.Columns.Add("BrandId", typeof(int));
			dtPM.Columns.Add("Brand", typeof(string));
			dtPM.Columns.Add("BrandUomId", typeof(int));
			dtPM.Columns.Add("BrandUom", typeof(string));
			dtPM.Columns.Add("ItemQty", typeof(double));
			dtPM.Columns.Add("ItemRate", typeof(double));
			dtPM.Columns.Add("Amount", typeof(double));
			dtPM.Columns.Add("Remarks", typeof(string));
			grdPackingMaterial.DataSource = dtPM;
			grdPackingMaterial.RetrieveStructure();
			grdDetailPackingMaterialSettings();
			dtPlantdetail.Columns.Add("Id");
			dtPlantdetail.Columns.Add("PlantId");
			dtPlantdetail.Columns.Add("PlantName");
			dtPlantdetail.Columns.Add("AssumedPlantCapacity", typeof(double));
			dtPlantdetail.Columns.Add("RemarksDetail");
			grdPlantdetail.DataSource = dtPlantdetail;
			grdPlantdetail.RetrieveStructure();
			grdPlantdetailSetting();
			btnAddPlantDetail.Visible = true;
			btnUpdatePlantDetail.Visible = false;
			btnCanclePlantdetail.Visible = false;
			btnAddOH.Visible = true;
			btnUpdateOH.Visible = false;
			BtnCancelOH.Visible = false;
			btnAddDetailPm.Visible = true;
			btnUpdateDetailPm.Visible = false;
			btnCancelDetailPm.Visible = false;
			((UltraGridBase)cmbStatus).Rows[0].Activate();
			txtPlanDate.Focus();
			txtRateInPut.Enabled = !JobOrderCreatewithoutRates;
			txtRateInPut.ReadOnly = false;
			((Control)(object)CmbRateUomInPut).Enabled = !JobOrderCreatewithoutRates;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLastSaveAccount()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetLastWipAccount(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 401
			});
			if (dt.Rows.Count > 0)
			{
				GetLastWipAccountId = Conversion.ToInt(dt.Rows[0]["AccountId"]);
				GetLastWipItemId = Conversion.ToInt(dt.Rows[0]["WipItemId"]);
				GetLastWipWareHouseId = Conversion.ToInt(dt.Rows[0]["WipWareHouseId"]);
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
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)cmbperemeter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllUomWithItemFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtAllUomWithItemId = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = PlantsAllocationToBranch.GetPlantsAllocatedToBranch(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPlantFeader, "PlantId", "PlantName", "PlantName ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilterTypeFill()
	{
		try
		{
			DataTable dtfiltertype = new DataTable();
			dtfiltertype.Columns.Add("Id");
			dtfiltertype.Columns.Add("name");
			dtfiltertype.Rows.Add(1, "SaleOrder");
			dtfiltertype.Rows.Add(2, "Export Contract");
			dtfiltertype.Rows.Add(3, "Item Type");
			dtfiltertype.Rows.Add(4, "Item Category");
			DDL.BindDDL(dtfiltertype, cmbFiltersType, "Id", "name", "Filter Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbFiltersType_Leave(object sender, EventArgs e)
	{
		try
		{
			cmbDocNo.Value = null;
			((Control)(object)cmbDocNo).Text = null;
			if (((Control)(object)cmbFiltersType).Text == "SaleOrder")
			{
				GetLocalSaleOrderNo();
			}
			if (((Control)(object)cmbFiltersType).Text == "Export Contract")
			{
				GetExportContractNo();
			}
			if (((Control)(object)cmbFiltersType).Text == "Item Type")
			{
				GetItemTypes();
			}
			if (((Control)(object)cmbFiltersType).Text == "Item Category")
			{
				GetItemCategory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDocNo_Leave(object sender, EventArgs e)
	{
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Expected O, but got Unknown
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbFiltersType).Text != null && ((Control)(object)cmbentrytype).Text == "Output")
			{
				if (((Control)(object)cmbFiltersType).Text == "Item Category" && Conversion.ToInt(cmbDocNo.Value) > 0)
				{
					DataTable dt4 = CommonServices.ItemGetAllServiceBind();
					DataTable dtItems4 = new DataTable();
					dtItems4.Columns.Add("Id");
					dtItems4.Columns.Add("ItemName");
					if (dt4.Rows.Count > 0)
					{
						((UltraGridBase)cmbItem).DataSource = null;
						((Control)(object)cmbItem).Text = string.Empty;
						for (int l = 0; l < dt4.Rows.Count; l++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt4.Rows[l]["ItemCategoryId"]))
							{
								dtItems4.Rows.Add(dt4.Rows[l]["Id"], dt4.Rows[l]["ItemName"]);
							}
						}
						DDL.BindDDLNew(dtItems4, cmbItem, "Id", "ItemName", "Item Name", false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "Item Type" && Conversion.ToInt(cmbDocNo.Value) > 0)
				{
					DataTable dt3 = CommonServices.ItemGetAllServiceBind();
					DataTable dtItems3 = new DataTable();
					dtItems3.Columns.Add("Id");
					dtItems3.Columns.Add("ItemName");
					if (dt3.Rows.Count > 0)
					{
						((UltraGridBase)cmbItem).DataSource = null;
						((Control)(object)cmbItem).Text = string.Empty;
						for (int k = 0; k < dt3.Rows.Count; k++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt3.Rows[k]["ItemTypeId"]))
							{
								dtItems3.Rows.Add(dt3.Rows[k]["Id"], dt3.Rows[k]["ItemName"]);
							}
						}
						DDL.BindDDLNew(dtItems3, cmbItem, "Id", "ItemName", "Item Name", false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "SaleOrder" && Conversion.ToInt(cmbDocNo.Value) > 0)
				{
					ItemDetailFillBySaleOrder();
				}
				if (((Control)(object)cmbFiltersType).Text == "Export Contract" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillByExportSaleContractId();
				}
			}
			else if (((Control)(object)cmbFiltersType).Text == null || ((Control)(object)cmbDocNo).Text == null || ((Control)(object)cmbFiltersType).Text == string.Empty || ((Control)(object)cmbDocNo).Text == string.Empty)
			{
				ItemDetailFill();
			}
			if (((Control)(object)cmbFiltersType).Text != null && ((Control)(object)cmbentrytype).Text == "Input")
			{
				if (((Control)(object)cmbFiltersType).Text == "Item Category" && Conversion.ToInt(cmbDocNo.Value) > 0)
				{
					DataTable dt2 = Item.ReadAllItems(new Item
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId
					});
					DataTable dtItems2 = new DataTable();
					dtItems2.Columns.Add("Id");
					dtItems2.Columns.Add("ItemName");
					if (dt2.Rows.Count > 0)
					{
						((UltraGridBase)cmbItemInput).DataSource = null;
						((Control)(object)cmbItemInput).Text = string.Empty;
						for (int j = 0; j < dt2.Rows.Count; j++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt2.Rows[j]["ItemCategoryId"]))
							{
								dtItems2.Rows.Add(dt2.Rows[j]["Id"], dt2.Rows[j]["ItemName"]);
							}
						}
						DDL.BindDDL(dtItems2, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "Item Type" && Conversion.ToInt(cmbDocNo.Value) > 0)
				{
					DataTable dt = Item.ReadAllItems(new Item
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId
					});
					DataTable dtItems = new DataTable();
					dtItems.Columns.Add("Id");
					dtItems.Columns.Add("ItemName");
					if (dt.Rows.Count > 0)
					{
						((UltraGridBase)cmbItemInput).DataSource = null;
						((Control)(object)cmbItemInput).Text = string.Empty;
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt.Rows[i]["ItemTypeId"]))
							{
								dtItems.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"]);
							}
						}
						DDL.BindDDL(dtItems, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "SaleOrder" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillBySaleOrderInput();
				}
				if (((Control)(object)cmbFiltersType).Text == "Export Contract" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillByExportSaleContractIdInput();
				}
			}
			else if (((Control)(object)cmbFiltersType).Text == null || ((Control)(object)cmbDocNo).Text == null || ((Control)(object)cmbFiltersType).Text == string.Empty || ((Control)(object)cmbDocNo).Text == string.Empty)
			{
				ItemDetailFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExportContractNo()
	{
		try
		{
			DataTable dtexportcontract = CommonServices.GetLcOrderNo();
			if (dtexportcontract.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtexportcontract, cmbDocNo, "Id", "LcOrderNo", "Export ContractNo", false);
			}
			else
			{
				((UltraGridBase)cmbDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLocalSaleOrderNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsaleorder = SaleOrder.GetSaleOrderNo(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsaleorder.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsaleorder, cmbDocNo, "Id", "DocNo", "SaleOrder", false);
			}
			else
			{
				((UltraGridBase)cmbDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemTypes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemTypes = ItemType.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemTypes.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemTypes, cmbDocNo, "Id", "TypeDescription", "ItemType", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemCategory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemCategory = ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, cmbDocNo, "Id", "CategoryDescription", "ItemCategory", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFills()
	{
		try
		{
			DataTable dtCoa = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4");
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbWorkInProcess, "Id", "AccountTitle", "WorkInProcess", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePlanCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvProductionJobOrder.GetGenerateCode(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 401,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtPlanCode.Text = code.ToString();
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
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind(6);
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, CmbWipItem, "Id", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWipItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
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
			RECID = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveInputRecord.Clear();
			lstRemoveOutputRecord.Clear();
			lstRemovePMRecord.Clear();
			lstRemoveOHRecord.Clear();
			txtInvoiceReference.Text = string.Empty;
			txtOtherInstruction.Text = string.Empty;
			txtPlantInstruction.Text = string.Empty;
			txtProductionInstruction.Text = string.Empty;
			txtDeliveryInstruction.Text = string.Empty;
			((Control)(object)CmbWorkInProcess).Text = string.Empty;
			((Control)(object)CmbWipItem).Text = string.Empty;
			((Control)(object)CmbLotAc).Text = string.Empty;
			((Control)(object)cmbProductionType).Text = "";
			((UltraGridBase)cmbStatus).Rows[0].Activate();
			((Control)(object)cmbPlanType).Text = "";
			dtPlannedOutput.Rows.Clear();
			dtInputDetail.Rows.Clear();
			dtOH.Rows.Clear();
			dtPM.Rows.Clear();
			dtPlantdetail.Rows.Clear();
			grdInputDetail.DataSource = null;
			grdPlannedOutput.DataSource = null;
			grdOverHead.DataSource = null;
			grdPackingMaterial.DataSource = null;
			grdPlantdetail.DataSource = null;
			updateDetailIndexoutput = -1;
			updateDetailIndexOH = -1;
			updateDetailIndexInput = -1;
			updateDetailIndexPM = -1;
			updatePlantDetailIndex = -1;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			GeneratePlanCode();
			txtPlanDate.Focus();
			((Control)(object)cmbJobLotOutput).Enabled = true;
			((Control)(object)cmbjoblotinput).Enabled = true;
			txtInPutTotalQty.Text = "0";
			txtInPutTotalWeight.Text = "0";
			txtTotalAmountInPut.Text = "0";
			txtTotalQtyBP.Text = "0";
			txtTotalWeightBP.Text = "0";
			txtTotalAmountBP.Text = "0";
			txtTotalQtyFG.Text = "0";
			txtTotalWeightFG.Text = "0";
			txtTotalAmountFG.Text = "0";
			txtFGAvgRate.Text = "0";
			txtFGRateWithoutExp.Text = "0";
			txtPackingMaterialAmount.Text = "0";
			txtOverHeadsAmount.Text = "0";
			dtLabDetail.Clear();
			BindGrids();
			GetLastSaveAccount();
			if (GetLastWipAccountId > 0 || GetLastWipItemId > 0)
			{
				CmbWipItem.Value = GetLastWipItemId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CheckStockAndGiveWarningMessageOnJobOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockAndGiveWarningMessageOnJobOrder"));
			ItemFill();
			JobLotBind();
			AccountFills();
			PlanTypeBind();
			ProductionTypeBind();
			PlantFeader();
			CropYear();
			WareHouseFill();
			ItemDetailFill();
			CmbPackingTypeFill();
			FilterTypeFill();
			AllUomWithItemFill();
			WagesAccountFill();
			dtPartiesFromStandardDbCall();
			SupplierDtFillFromStandard();
			BindSupplierName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ParameterFill();
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			((Control)(object)xmbjoborderno).Text = string.Empty;
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionPreCostingAndJobOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool FormValidation()
	{
		if (txtPlanCode.Text.Trim() == string.Empty || Conversion.ToInt((object)txtPlanCode.Text) == 0)
		{
			MessageBox.Show("Plan Code Field Required");
			txtPlanCode.Focus();
			return false;
		}
		if (txtInvoiceReference.Text == string.Empty || txtInvoiceReference.Text == "0")
		{
			MessageBox.Show("Manual ReportNo Field Required");
			txtInvoiceReference.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProductionType).ActiveRow == null || Conversion.ToInt(cmbProductionType.Value) == 0)
		{
			MessageBox.Show("Production Type Field Required");
			cmbProductionType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWorkInProcess).ActiveRow == null || Conversion.ToInt(CmbWorkInProcess.Value) == 0)
		{
			MessageBox.Show("WorkInProcess Field Required");
			CmbWorkInProcess.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWipItem).ActiveRow == null || Conversion.ToInt(CmbWipItem.Value) == 0)
		{
			MessageBox.Show("WIPItem Field Required");
			CmbWipItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWipWarehouse).ActiveRow == null || Conversion.ToInt(CmbWipWarehouse.Value) == 0)
		{
			MessageBox.Show("WIP Warehouse Field Required");
			CmbWipWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPlanType).ActiveRow == null || Conversion.ToInt(cmbPlanType.Value) == 0)
		{
			MessageBox.Show("Plan Type Field Required");
			cmbPlanType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null || Conversion.ToInt(cmbStatus.Value) == 0)
		{
			MessageBox.Show("Status Field Required");
			cmbStatus.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Expected O, but got Unknown
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Expected O, but got Unknown
		//IL_15ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b2: Expected O, but got Unknown
		//IL_195b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1962: Expected O, but got Unknown
		//IL_1a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a36: Expected O, but got Unknown
		try
		{
			if (grdPlannedOutput.GetRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Record Not Found");
			}
			if (grdInputDetail.GetRows().Count() == 0)
			{
				throw new Exception("Input Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (grdLabStandards.RowCount > 0 && !FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbAnalysisGroup, "Schedule Name", FormHelper.ValidType.INT),
				((Control)(object)CmbBrandNameLabStandard, "Brand Item", FormHelper.ValidType.INT)
			}))
			{
				tabControl2.SelectedIndex = 5;
				return;
			}
			InvProductionJobOrder invProductionJobOrder = new InvProductionJobOrder();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invProductionJobOrder.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			btnGenerate_Click(null, null);
			double RawTotalValue = Conversion.ToDouble((object)txtTotalAmountInPut.Text);
			double InputGridTotalValue = Conversion.ToDouble((object)grdInputDetail.GetTotal(grdInputDetail.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			if (Math.Round(RawTotalValue, 0) != Math.Round(InputGridTotalValue, 0) && MessageBox.Show($"Raw Value {RawTotalValue} In Header and Total InPut Amount {InputGridTotalValue} are not equal, Are you sure to Proceed?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double FinishTotalValue = Conversion.ToDouble((object)txtTotalAmountBP.Text) + Conversion.ToDouble((object)txtTotalAmountFG.Text);
			double OutPutGridTotalValue = Conversion.ToDouble((object)grdPlannedOutput.GetTotal(grdPlannedOutput.RootTable.Columns["NetAmount"], (AggregateFunction)2).ToString());
			if (Math.Round(FinishTotalValue, 0) != Math.Round(OutPutGridTotalValue, 0) && MessageBox.Show($"Finish Value {FinishTotalValue} In Header and Total OutPut Amount {OutPutGridTotalValue} are not equal, Are you sure to Proceed?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invProductionJobOrder.OrganizationId = UserAccount.OrganizationId;
			invProductionJobOrder.CompanyId = UserAccount.CompanyId;
			invProductionJobOrder.EntryUser = UserAccount.ID;
			invProductionJobOrder.ModifyUser = UserAccount.ID;
			invProductionJobOrder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invProductionJobOrder.DocumentTypeId = 401;
			invProductionJobOrder.PlanDate = txtPlanDate.Value;
			invProductionJobOrder.PlanCode = Conversion.ToInt((object)txtPlanCode.Text);
			invProductionJobOrder.InvProductionPlantId = Conversion.ToInt(cmbPlantFeader.Value);
			invProductionJobOrder.PlanType = Conversion.ToString((object)((Control)(object)cmbPlanType).Text);
			invProductionJobOrder.RefInvoiceNo = Conversion.ToString((object)txtInvoiceReference.Text);
			invProductionJobOrder.ProductionType = Conversion.ToString((object)((Control)(object)cmbProductionType).Text);
			invProductionJobOrder.WorkInProccessAcId = Conversion.ToInt(CmbWorkInProcess.Value);
			invProductionJobOrder.WipWareHouseId = Conversion.ToInt(CmbWipWarehouse.Value);
			invProductionJobOrder.JobLotId = Conversion.ToInt(CmbLotAc.Value);
			invProductionJobOrder.WipItemId = Conversion.ToInt(CmbWipItem.Value);
			invProductionJobOrder.StartDate = txtstartDate.Value;
			invProductionJobOrder.EndDate = txtEndDate.Value;
			invProductionJobOrder.PlanStatus = Conversion.ToString((object)((Control)(object)cmbStatus).Text);
			invProductionJobOrder.PackingInstructions = txtPlantInstruction.Text;
			invProductionJobOrder.ProductionInsturction = txtProductionInstruction.Text;
			invProductionJobOrder.DeliveryInstructions = txtDeliveryInstruction.Text;
			invProductionJobOrder.OtherInstructions = txtOtherInstruction.Text;
			invProductionJobOrder.IsApproved = false;
			invProductionJobOrder.SupplierCustomerId = 0;
			invProductionJobOrder.invProductionJobOrderOutput = new List<InvProductionJobOrderOutput>();
			invProductionJobOrder.invProductionJobOrderInput = new List<InvProductionJobOrderInput>();
			invProductionJobOrder.InvProductionJobOrderPlantslist = new List<InvProductionJobOrderPlant>();
			invProductionJobOrder.InvProductionJobOrderOverHeadsList = new List<InvProductionJobOrderOverHeads>();
			invProductionJobOrder.InvProductionJobOrderPackingMaterialList = new List<InvProductionJobOrderPackingMaterial>();
			invProductionJobOrder.InvProductionJobOrderLabStandardDetailList = new List<InvProductionJobOrderLabStandardDetail>();
			GridEXRow[] rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				InvProductionJobOrderOutput jobOrderOutputdetail = new InvProductionJobOrderOutput();
				if (r5.Cells["CropYearId"].Text == string.Empty)
				{
					MessageBox.Show($"CropYear Field Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (r5.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show($"WareHouse Field Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (r5.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show($"JobLot Field Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r5.Cells["ItemRate"].Value) == 0.0)
				{
					MessageBox.Show($"ItemRate Field is Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r5.Cells["ItemAmount"].Value) == 0.0)
				{
					MessageBox.Show($"ItemAmount Field is Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r5.Cells["NetRate"].Value) == 0.0)
				{
					MessageBox.Show($"NetRate Field is Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r5.Cells["NetAmount"].Value) == 0.0)
				{
					MessageBox.Show($"NetAmount Field is Required in Planned Output Detail Row No: {r5.RowIndex + 1}");
					return;
				}
				jobOrderOutputdetail.Id = ((RECID != 0) ? Conversion.ToInt(r5.Cells["Id"].Value) : 0);
				jobOrderOutputdetail.ActionTypeId = ((jobOrderOutputdetail.Id <= 0) ? 1 : 2);
				jobOrderOutputdetail.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
				jobOrderOutputdetail.InvPackingTypeId = Conversion.ToInt((object)r5.Cells["PackingTypeId"].Text);
				jobOrderOutputdetail.QtyInner = Conversion.ToDouble((object)r5.Cells["QtyOuter"].Text);
				jobOrderOutputdetail.QtyOuter = Conversion.ToDouble((object)r5.Cells["QtyOuter"].Text);
				jobOrderOutputdetail.UomSchIdInner = Conversion.ToInt((object)r5.Cells["OuterPackSizeId"].Text);
				jobOrderOutputdetail.UomSchIdOuter = Conversion.ToInt((object)r5.Cells["OuterPackSizeId"].Text);
				jobOrderOutputdetail.NetWeight = Conversion.ToDouble((object)r5.Cells["Weight"].Text);
				jobOrderOutputdetail.PackRemarks = Conversion.ToString((object)r5.Cells["Remarks"].Text);
				jobOrderOutputdetail.CropYearId = Conversion.ToInt((object)r5.Cells["CropYearId"].Text);
				jobOrderOutputdetail.WarehouseId = Conversion.ToInt((object)r5.Cells["WareHouseId"].Text);
				jobOrderOutputdetail.JobLotId = Conversion.ToInt((object)r5.Cells["JobLotId"].Text);
				jobOrderOutputdetail.ItemRate = Conversion.ToDouble(r5.Cells["ItemRate"].Value);
				jobOrderOutputdetail.RateUomId = Conversion.ToInt(r5.Cells["RateUomId"].Value);
				jobOrderOutputdetail.ItemAmount = Conversion.ToDouble(r5.Cells["ItemAmount"].Value);
				jobOrderOutputdetail.OHExpense = Conversion.ToDecimal(r5.Cells["OHExpense"].Value);
				jobOrderOutputdetail.PMExpense = Conversion.ToDecimal(r5.Cells["PMExpense"].Value);
				jobOrderOutputdetail.OHGeneralExpense = Conversion.ToDecimal(r5.Cells["OHGeneralExpense"].Value);
				jobOrderOutputdetail.PMGeneralExpense = Conversion.ToDecimal(r5.Cells["PMGeneralExpense"].Value);
				jobOrderOutputdetail.NetRate = Conversion.ToDecimal(r5.Cells["NetRate"].Value);
				jobOrderOutputdetail.NetAmount = Conversion.ToDecimal(r5.Cells["NetAmount"].Value);
				jobOrderOutputdetail.EntryTypeId = Conversion.ToInt(r5.Cells["EntryTypeId"].Value);
				invProductionJobOrder.invProductionJobOrderOutput.Add(jobOrderOutputdetail);
			}
			if (lstRemoveOutputRecord.Count > 0 && RECID > 0)
			{
				for (int l = 0; l < lstRemoveOutputRecord.Count; l++)
				{
					invProductionJobOrder.invProductionJobOrderOutput.Add(lstRemoveOutputRecord[l]);
				}
			}
			rows = grdInputDetail.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				InvProductionJobOrderInput jobOrderInputdetail = new InvProductionJobOrderInput();
				if (Conversion.ToInt(r4.Cells["ItemId"].Value) == 0 || r4.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show($"Item Field Require in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (r4.Cells["CropYear"].Text == string.Empty)
				{
					MessageBox.Show($"CropYear Field Require in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (Conversion.ToInt(r4.Cells["JobLotId"].Value) == 0 || r4.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show($"JobLot Field Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (Conversion.ToDouble((object)r4.Cells["Qty"].Text) == 0.0)
				{
					MessageBox.Show($"Quantity Field Require in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (r4.Cells["PackSizeId"].Text == string.Empty || Conversion.ToInt(r4.Cells["PackSizeId"].Value) == 0)
				{
					MessageBox.Show($"PackSize Field Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (Conversion.ToDouble((object)r4.Cells["Weight"].Text) == 0.0)
				{
					MessageBox.Show($"Weight Field Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (Conversion.ToInt(r4.Cells["WareHouseId"].Value) == 0 || r4.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show($"WareHouse Field Require in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (Conversion.ToInt(r4.Cells["PackTypeId"].Value) == 0 || r4.Cells["PackTypeId"].Text == string.Empty)
				{
					MessageBox.Show($"Packing Type Field Require in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r4.Cells["ItemRate"].Value) == 0.0)
				{
					MessageBox.Show($"ItemRate Field is Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToInt(r4.Cells["RateUomId"].Value) == 0)
				{
					MessageBox.Show($"RateUom Field is Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				if (!JobOrderCreatewithoutRates && Conversion.ToDouble(r4.Cells["ItemAmount"].Value) == 0.0)
				{
					MessageBox.Show($"ItemAmount Field is Required in Planned Input Detail Row No: {r4.RowIndex + 1}");
					return;
				}
				jobOrderInputdetail.Id = ((RECID != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
				jobOrderInputdetail.ActionTypeId = ((jobOrderInputdetail.Id <= 0) ? 1 : 2);
				jobOrderInputdetail.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				jobOrderInputdetail.CropYearId = Conversion.ToInt(r4.Cells["CropYearId"].Value);
				jobOrderInputdetail.BatchCrop = Conversion.ToString((object)r4.Cells["CropYear"].Text);
				jobOrderInputdetail.JobLotId = Conversion.ToInt(r4.Cells["JobLotId"].Value);
				jobOrderInputdetail.Qty = Conversion.ToDouble((object)r4.Cells["Qty"].Text);
				jobOrderInputdetail.UomSchIdQty = Conversion.ToInt(r4.Cells["PackSizeId"].Value);
				jobOrderInputdetail.NetWeight = Conversion.ToDouble((object)r4.Cells["Weight"].Text);
				jobOrderInputdetail.WarehouseId = Conversion.ToInt(r4.Cells["WareHouseId"].Value);
				jobOrderInputdetail.PackingTypeId = Conversion.ToInt(r4.Cells["PackTypeId"].Value);
				jobOrderInputdetail.ItemRate = Conversion.ToDouble(r4.Cells["ItemRate"].Value);
				jobOrderInputdetail.RateUomId = Conversion.ToInt(r4.Cells["RateUomId"].Value);
				jobOrderInputdetail.ItemAmount = Conversion.ToDouble(r4.Cells["ItemAmount"].Value);
				jobOrderInputdetail.RemarksInputDt = Conversion.ToString((object)r4.Cells["Remarks"].Text);
				jobOrderInputdetail.RefDocumentTypeId = Conversion.ToInt(r4.Cells["RefDocumentTypeId"].Value);
				jobOrderInputdetail.RefDocIdNo = Conversion.ToInt(r4.Cells["RefDocIdNo"].Value);
				jobOrderInputdetail.RefDocSubIdNo = Conversion.ToInt(r4.Cells["RefDocSubIdNo"].Value);
				jobOrderInputdetail.DocNo = Conversion.ToInt(r4.Cells["DocNo"].Value);
				jobOrderInputdetail.DocDate = Conversion.ToDateTime(r4.Cells["DocDate"].Value);
				invProductionJobOrder.invProductionJobOrderInput.Add(jobOrderInputdetail);
			}
			if (lstRemoveInputRecord.Count > 0 && RECID > 0)
			{
				for (int k = 0; k < lstRemoveInputRecord.Count; k++)
				{
					invProductionJobOrder.invProductionJobOrderInput.Add(lstRemoveInputRecord[k]);
				}
			}
			if (grdPackingMaterial.RowCount > 0)
			{
				rows = grdPackingMaterial.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					InvProductionJobOrderPackingMaterial PackingMaterialDetail = new InvProductionJobOrderPackingMaterial();
					if (Conversion.ToInt(r3.Cells["PackingItemId"].Value) > 0 && Conversion.ToInt(r3.Cells["PackingItemId"].Value) > 0)
					{
						if (r3.Cells["PackingItemId"].Text == string.Empty || Conversion.ToInt(r3.Cells["PackingItemId"].Value) == 0)
						{
							MessageBox.Show($"Packing Material Item Field Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						if (Conversion.ToInt(r3.Cells["BrandId"].Value) == 0 && Conversion.ToInt(r3.Cells["BrandUomId"].Value) != 0)
						{
							MessageBox.Show($"OutPut Item Field Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						if (Conversion.ToInt(r3.Cells["BrandUomId"].Value) == 0 && Conversion.ToInt(r3.Cells["BrandId"].Value) != 0)
						{
							MessageBox.Show($"OutPut UomField Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						if (r3.Cells["ItemQty"].Text == string.Empty || Conversion.ToDouble(r3.Cells["ItemQty"].Value) == 0.0)
						{
							MessageBox.Show($"ItemQty Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						if (!JobOrderCreatewithoutRates && (r3.Cells["ItemRate"].Text == string.Empty || Conversion.ToDouble(r3.Cells["ItemRate"].Value) == 0.0) && (r3.Cells["ItemQty"].Text != string.Empty || Conversion.ToDouble(r3.Cells["ItemQty"].Value) != 0.0) && formright.DoHaveCanRateandAmount)
						{
							MessageBox.Show($"ItemRate Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						if (!JobOrderCreatewithoutRates && (r3.Cells["Amount"].Text == string.Empty || Conversion.ToDouble(r3.Cells["Amount"].Value) == 0.0))
						{
							MessageBox.Show($"Amount Required in PM Detail Row No: {r3.RowIndex + 1}");
							return;
						}
						PackingMaterialDetail.Id = ((RECID != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
						PackingMaterialDetail.ActionTypeId = ((PackingMaterialDetail.Id <= 0) ? 1 : 2);
						PackingMaterialDetail.ItemId = Conversion.ToInt(r3.Cells["PackingItemId"].Value);
						PackingMaterialDetail.BrandId = Conversion.ToInt(r3.Cells["BrandId"].Value);
						PackingMaterialDetail.BrandUomId = Conversion.ToInt(r3.Cells["BrandUomId"].Value);
						PackingMaterialDetail.Qty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
						PackingMaterialDetail.Rate = Conversion.ToDouble(r3.Cells["ItemRate"].Value);
						PackingMaterialDetail.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
						PackingMaterialDetail.pmRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						PackingMaterialDetail.WarehouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
						invProductionJobOrder.InvProductionJobOrderPackingMaterialList.Add(PackingMaterialDetail);
					}
				}
			}
			if (lstRemovePMRecord.Count > 0 && RECID > 0)
			{
				for (int j = 0; j < lstRemovePMRecord.Count; j++)
				{
					invProductionJobOrder.InvProductionJobOrderPackingMaterialList.Add(lstRemovePMRecord[j]);
				}
			}
			if (grdOverHead.RowCount > 0)
			{
				rows = grdOverHead.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					InvProductionJobOrderOverHeads OverHeaddetail = new InvProductionJobOrderOverHeads();
					if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r2.Cells["ChartOfAccountId"].Value) > 0)
					{
						if (r2.Cells["ChartOfAccountId"].Text == string.Empty || Conversion.ToInt(r2.Cells["ChartOfAccountId"].Value) == 0)
						{
							MessageBox.Show($"OverHead Account Field Require in OH Detail Row No: {r2.RowIndex + 1}");
							return;
						}
						if (Conversion.ToInt(r2.Cells["BrandId"].Value) == 0 && Conversion.ToInt(r2.Cells["BrandUomId"].Value) != 0)
						{
							MessageBox.Show($"OutPut Item Field Required in OH Detail Row No: {r2.RowIndex + 1}");
							return;
						}
						if (Conversion.ToInt(r2.Cells["BrandUomId"].Value) == 0 && Conversion.ToInt(r2.Cells["BrandId"].Value) != 0)
						{
							MessageBox.Show($"OutPut UomField Require in OH Detail Row No: {r2.RowIndex + 1}");
							return;
						}
						if (!JobOrderCreatewithoutRates && (r2.Cells["Amount"].Text == string.Empty || Conversion.ToDouble(r2.Cells["Amount"].Value) == 0.0))
						{
							MessageBox.Show($"Amount Required in OH Detail Row No: {r2.RowIndex + 1}");
							return;
						}
						OverHeaddetail.Id = ((RECID != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
						OverHeaddetail.ActionTypeId = ((OverHeaddetail.Id <= 0) ? 1 : 2);
						OverHeaddetail.CharOfAccountId = Conversion.ToInt(r2.Cells["ChartOfAccountId"].Value);
						OverHeaddetail.BrandId = Conversion.ToInt(r2.Cells["BrandId"].Value);
						OverHeaddetail.BrandUomId = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
						OverHeaddetail.ItemQty = Conversion.ToDouble(r2.Cells["ItemQty"].Value);
						OverHeaddetail.ItemRate = Conversion.ToDouble(r2.Cells["ItemRate"].Value);
						OverHeaddetail.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						OverHeaddetail.ohRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						invProductionJobOrder.InvProductionJobOrderOverHeadsList.Add(OverHeaddetail);
					}
				}
			}
			if (lstRemoveOHRecord.Count > 0 && RECID > 0)
			{
				for (int i = 0; i < lstRemoveOHRecord.Count; i++)
				{
					invProductionJobOrder.InvProductionJobOrderOverHeadsList.Add(lstRemoveOHRecord[i]);
				}
			}
			if (grdPlantdetail.GetRows().Count() == 0)
			{
				throw new Exception("At Least One Detail Record Is Required In Plant Detail Grid");
			}
			rows = grdPlantdetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				InvProductionJobOrderPlant plant = new InvProductionJobOrderPlant();
				plant.Id = ((RECID != 0) ? Conversion.ToInt(item.Cells["Id"].Value) : 0);
				plant.PlantId = Conversion.ToInt(item.Cells["PlantId"].Value);
				plant.AssumedPlantCapacity = Conversion.ToDecimal(item.Cells["AssumedPlantCapacity"].Value);
				plant.RemarksDetail = Conversion.ToString(item.Cells["RemarksDetail"].Value);
				invProductionJobOrder.InvProductionJobOrderPlantslist.Add(plant);
			}
			rows = grdLabStandards.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvProductionJobOrderLabStandardDetail vd = new InvProductionJobOrderLabStandardDetail();
				vd.InvProductionJobOrderLabStandardDetailId = ((RECID != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.InvProductionJobOrderLabStandardDetailId <= 0) ? 1 : 2);
				FillLabGroupAnalysisStandardsFromGrid(vd, r);
				FormHelper.ValidateField(vd.LabAnalysisItemId, "Analysis Item", r.RowIndex);
				FormHelper.ValidateField(vd.MaxValue, "MaxValue", r.RowIndex);
				invProductionJobOrder.InvProductionJobOrderLabStandardDetailList.Add(vd);
			}
			invProductionJobOrder.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			invProductionJobOrder.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvProductionJobOrder>(invProductionJobOrder, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvProductionJobOrder.Save(invProductionJobOrder);
			if (RECID == 0)
			{
				MessageBox.Show("Save Successfully" + invProductionJobOrder.PlanCode);
			}
			else
			{
				MessageBox.Show("Update Successfully" + invProductionJobOrder.PlanCode);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, 401, base.Name);
				UpdateMode = false;
			}
			Reset();
			if (chkPrint.Checked)
			{
				CommonServices.PrintJobOrder630(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillLabGroupAnalysisStandardsFromGrid(InvProductionJobOrderLabStandardDetail standards, GridEXRow r)
	{
		standards.BrandItemId = Conversion.ToInt(CmbBrandNameLabStandard.Value);
		standards.BrandItemName = Conversion.ToString((object)((Control)(object)CmbBrandNameLabStandard).Text);
		standards.SupplierCustomerId = Conversion.ToInt(CmbBuyerName.Value);
		standards.PartyName = Conversion.ToString((object)((Control)(object)CmbBuyerName).Text);
		standards.LabStandardMainId = Conversion.ToInt(CmbAnalysisGroup.Value);
		standards.LabScheduleName = Conversion.ToString((object)((Control)(object)CmbAnalysisGroup).Text);
		standards.LabAnalysisItemId = Conversion.ToInt(r.Cells["AnalysisItemId"].Value);
		standards.LabAnalysisItemName = Conversion.ToString(r.Cells["AnalysisItem"].Value);
		standards.MinValue = Conversion.ToDecimal(r.Cells["MinValue"].Value);
		standards.MaxValue = Conversion.ToDecimal(r.Cells["MaxValue"].Value);
		standards.Active = true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
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
			if (RECID == 0)
			{
				throw new Exception("Record Not Update Because Record Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			Reset();
			InvProductionJobOrder head = new InvProductionJobOrder();
			RECID = ID;
			head = InvProductionJobOrder.GetByID(RECID);
			if (head != null)
			{
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
				tabControl1.SelectedIndex = 0;
				txtPlanDate.Value = head.PlanDate;
				txtPlanCode.Text = Conversion.ToString((object)head.PlanCode);
				if (head.InvProductionPlantId > 0)
				{
					cmbPlantFeader.Value = Conversion.ToInt((object)head.InvProductionPlantId);
				}
				CmbWorkInProcess.Value = Conversion.ToInt((object)head.WorkInProccessAcId);
				CmbWipWarehouse.Value = Conversion.ToInt((object)head.WipWareHouseId);
				CmbLotAc.Value = Conversion.ToInt((object)head.JobLotId);
				CmbWipItem.Value = Conversion.ToInt((object)head.WipItemId);
				((Control)(object)cmbPlanType).Text = Conversion.ToString((object)head.PlanType);
				txtInvoiceReference.Text = Conversion.ToString((object)head.RefInvoiceNo);
				((Control)(object)cmbProductionType).Text = Conversion.ToString((object)head.ProductionType);
				txtstartDate.Value = Conversion.ToDateTime((object)head.StartDate);
				txtEndDate.Value = Conversion.ToDateTime((object)head.EndDate);
				((Control)(object)cmbStatus).Text = Conversion.ToString((object)head.PlanStatus);
				txtProductionInstruction.Text = head.ProductionInsturction;
				txtDeliveryInstruction.Text = head.DeliveryInstructions;
				txtPlantInstruction.Text = head.PackingInstructions;
				txtOtherInstruction.Text = head.OtherInstructions;
				for (int m = 0; m < head.invProductionJobOrderInput.Count; m++)
				{
					dtInputDetail.Rows.Add(head.invProductionJobOrderInput[m].Id, head.invProductionJobOrderInput[m].RefDocumentTypeId, head.invProductionJobOrderInput[m].RefDocIdNo, head.invProductionJobOrderInput[m].RefDocSubIdNo, head.invProductionJobOrderInput[m].DocDate, head.invProductionJobOrderInput[m].DocNo, head.invProductionJobOrderInput[m].WarehouseId, head.invProductionJobOrderInput[m].WareHouseName, head.invProductionJobOrderInput[m].JobLotId, head.invProductionJobOrderInput[m].JobLotCode, head.invProductionJobOrderInput[m].ItemId, head.invProductionJobOrderInput[m].ItemName, head.invProductionJobOrderInput[m].CropYearId, head.invProductionJobOrderInput[m].BatchCrop, head.invProductionJobOrderInput[m].PackingTypeId, head.invProductionJobOrderInput[m].PackingType, head.invProductionJobOrderInput[m].Qty, head.invProductionJobOrderInput[m].NetWeight, head.invProductionJobOrderInput[m].Qty, head.invProductionJobOrderInput[m].UomSchIdQty, head.invProductionJobOrderInput[m].UOMCode, head.invProductionJobOrderInput[m].UOMCodeEquivalent, head.invProductionJobOrderInput[m].NetWeight, head.invProductionJobOrderInput[m].ItemRate, head.invProductionJobOrderInput[m].RateUomId, head.invProductionJobOrderInput[m].RateUom, head.invProductionJobOrderInput[m].RateUomEquivalent, head.invProductionJobOrderInput[m].ItemAmount, head.invProductionJobOrderInput[m].RemarksInputDt);
				}
				grdInputDetail.DataSource = dtInputDetail;
				grdInputDetail.RetrieveStructure();
				InputDetailGridSetting();
				for (int l = 0; l < head.invProductionJobOrderOutput.Count; l++)
				{
					dtPlannedOutput.Rows.Add(head.invProductionJobOrderOutput[l].Id, head.invProductionJobOrderOutput[l].EntryTypeId, head.invProductionJobOrderOutput[l].EntryType, head.invProductionJobOrderOutput[l].WarehouseId, head.invProductionJobOrderOutput[l].WareHouseName, head.invProductionJobOrderOutput[l].JobLotId, head.invProductionJobOrderOutput[l].JobLotDescription, head.invProductionJobOrderOutput[l].ItemId, head.invProductionJobOrderOutput[l].ItemName, head.invProductionJobOrderOutput[l].CropYearId, head.invProductionJobOrderOutput[l].CropYear, head.invProductionJobOrderOutput[l].InvPackingTypeId, head.invProductionJobOrderOutput[l].PackTypeDesc, head.invProductionJobOrderOutput[l].QtyOuter, head.invProductionJobOrderOutput[l].UomSchIdOuter, head.invProductionJobOrderOutput[l].outerUOMCode, head.invProductionJobOrderOutput[l].outerUOMCodeEquivalent, head.invProductionJobOrderOutput[l].NetWeight, head.invProductionJobOrderOutput[l].ItemRate, head.invProductionJobOrderOutput[l].RateUomId, head.invProductionJobOrderOutput[l].RateUom, head.invProductionJobOrderOutput[l].Equivalent, head.invProductionJobOrderOutput[l].ItemAmount, Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].OHExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].PMExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].OHGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].PMGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].NetRate), Conversion.ToDouble((object)head.invProductionJobOrderOutput[l].NetAmount), head.invProductionJobOrderOutput[l].PackRemarks);
				}
				grdPlannedOutput.DataSource = dtPlannedOutput;
				grdPlannedOutput.RetrieveStructure();
				PlannedOutputGridSetting();
				for (int k = 0; k < head.InvProductionJobOrderOverHeadsList.Count; k++)
				{
					dtOH.Rows.Add(head.InvProductionJobOrderOverHeadsList[k].Id, head.InvProductionJobOrderOverHeadsList[k].CharOfAccountId, head.InvProductionJobOrderOverHeadsList[k].AccountTitle, head.InvProductionJobOrderOverHeadsList[k].BrandId, head.InvProductionJobOrderOverHeadsList[k].BrandName, head.InvProductionJobOrderOverHeadsList[k].BrandUomId, head.InvProductionJobOrderOverHeadsList[k].BrandUom, head.InvProductionJobOrderOverHeadsList[k].ItemQty, head.InvProductionJobOrderOverHeadsList[k].ItemRate, head.InvProductionJobOrderOverHeadsList[k].Amount, head.InvProductionJobOrderOverHeadsList[k].ohRemarks);
				}
				grdOverHead.DataSource = dtOH;
				grdOverHead.RetrieveStructure();
				grdDetailOverheadSettings();
				for (int j = 0; j < head.InvProductionJobOrderPackingMaterialList.Count; j++)
				{
					dtPM.Rows.Add(head.InvProductionJobOrderPackingMaterialList[j].Id, head.InvProductionJobOrderPackingMaterialList[j].WarehouseId, head.InvProductionJobOrderPackingMaterialList[j].WareHouseName, head.InvProductionJobOrderPackingMaterialList[j].ItemId, head.InvProductionJobOrderPackingMaterialList[j].ItemName, head.InvProductionJobOrderPackingMaterialList[j].BrandId, head.InvProductionJobOrderPackingMaterialList[j].BrandName, head.InvProductionJobOrderPackingMaterialList[j].BrandUomId, head.InvProductionJobOrderPackingMaterialList[j].BrandUom, head.InvProductionJobOrderPackingMaterialList[j].Qty, head.InvProductionJobOrderPackingMaterialList[j].Rate, head.InvProductionJobOrderPackingMaterialList[j].Amount, head.InvProductionJobOrderPackingMaterialList[j].pmRemarks);
				}
				grdPackingMaterial.DataSource = dtPM;
				grdPackingMaterial.RetrieveStructure();
				grdDetailPackingMaterialSettings();
				for (int i = 0; i < head.InvProductionJobOrderPlantslist.Count; i++)
				{
					dtPlantdetail.Rows.Add(head.InvProductionJobOrderPlantslist[i].Id, head.InvProductionJobOrderPlantslist[i].PlantId, head.InvProductionJobOrderPlantslist[i].PlantName, head.InvProductionJobOrderPlantslist[i].AssumedPlantCapacity, head.InvProductionJobOrderPlantslist[i].RemarksDetail);
				}
				grdPlantdetail.DataSource = dtPlantdetail;
				grdPlantdetail.RetrieveStructure();
				grdPlantdetailSetting();
				InvProductionJobOrderLabStandardDetail item = ((head.InvProductionJobOrderLabStandardDetailList.Count <= 0) ? null : head.InvProductionJobOrderLabStandardDetailList?[0]);
				if (item != null)
				{
					CmbBrandNameLabStandard.Value = item.BrandItemId;
					CmbBuyerName.Value = item.SupplierCustomerId;
					CmbBuyerName_Leave(null, null);
					CmbAnalysisGroup.Value = item.LabStandardMainId;
					dtLabDetail.Rows.Clear();
					FillDetailFromList(head, dtLabDetail);
				}
				BindGrids();
				FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
				CostingCalculations();
				tabControl2_SelectedIndexChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromList(InvProductionJobOrder list, DataTable dt)
	{
		List<InvProductionJobOrderLabStandardDetail> items = ((list != null) ? list.InvProductionJobOrderLabStandardDetailList : null);
		if (items == null || items.Count == 0)
		{
			return;
		}
		dt.BeginLoadData();
		try
		{
			foreach (InvProductionJobOrderLabStandardDetail item in items)
			{
				DataRow dr = dt.NewRow();
				dr["Id"] = item.InvProductionJobOrderLabStandardDetailId;
				dr["AnalysisGroupId"] = item.LabAnalysisGroupId;
				dr["AnalysisGroup"] = item.LabAnalysisGroupDescription;
				dr["AnalysisItemId"] = item.LabAnalysisItemId;
				dr["AnalysisItem"] = item.LabAnalysisItemName;
				dr["MinValue"] = item.MinValue;
				dr["MaxValue"] = item.MaxValue;
				dr["Active"] = item.Active;
				dr["MainId"] = item.LabStandardMainId;
				dt.Rows.Add(dr);
			}
		}
		finally
		{
			dt.EndLoadData();
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EntryTypeFill()
	{
		try
		{
			DataTable dtentryType = new DataTable();
			dtentryType.Columns.Add("Id");
			dtentryType.Columns.Add("name");
			dtentryType.Rows.Add(1, "Input");
			dtentryType.Rows.Add(2, "Output");
			DDL.BindDDL(dtentryType, cmbentrytype, "Id", "name", "Entry Type", ZeroIndex: false);
			((UltraGridBase)cmbentrytype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbentrytype_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbentrytype).Text == "Input")
			{
				tabControl2.SelectedIndex = 1;
			}
			if (((Control)(object)cmbentrytype).Text == "Output")
			{
				tabControl2.SelectedIndex = 0;
			}
			cmbDocNo_Leave(null, null);
			cmbentrytype.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemInput_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbItemInput.Value);
			if (Id > 0)
			{
				bindPackSizeInput(Id);
				InPutRateBind();
			}
			decimal Stock = CommonServices.GetAvailableStockFromTransactionOrEvaluation(Id, txtPlanDate.Value, 0, 0, null, 0, 0, 2, 1).BalWeight;
			lblStockWeight.Text = ((Stock > 0m) ? Stock.ToString("#,##0.###") : "0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindPackSizeInput(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)cmbpacksizeinput).Text;
			string Rateuom = ((Control)(object)CmbRateUomInPut).Text;
			((Control)(object)cmbpacksizeinput).Text = string.Empty;
			((Control)(object)CmbRateUomInPut).Text = string.Empty;
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("Equivalent", typeof(double));
			foreach (DataRow r in dtAllUomWithItemId.Rows)
			{
				if (Conversion.ToInt(r["ItemId"]) == ItemId)
				{
					dtUom.Rows.Add(r["Id"], r["UOMCode"], r["Equivalent"]);
				}
			}
			if (dtUom == null || dtUom.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUom, cmbpacksizeinput, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
			DDL.BindDDL(dtUom, CmbRateUomInPut, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
			((UltraGridBase)cmbpacksizeinput).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)CmbRateUomInPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			if (!string.IsNullOrEmpty(Packuom))
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbpacksizeinput).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbpacksizeinput).Text = string.Empty;
				}
			}
			if (!string.IsNullOrEmpty(Rateuom))
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUomInPut).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUomInPut).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetailInPut()
	{
		if (((UltraGridBase)cmbwarehouseinput).ActiveRow == null || Conversion.ToInt(cmbwarehouseinput.Value) == 0)
		{
			MessageBox.Show("Warehouse is Required");
			cmbwarehouseinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbjoblotinput).ActiveRow == null || Conversion.ToInt(cmbjoblotinput.Value) == 0)
		{
			MessageBox.Show("Job Lot Field is Required");
			cmbjoblotinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemInput).ActiveRow == null || Conversion.ToInt(cmbItemInput.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItemInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcropyearinput).ActiveRow == null || Conversion.ToInt(cmbcropyearinput.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			cmbcropyearinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackTypeInput).ActiveRow == null || Conversion.ToInt(cmbPackTypeInput.Value) == 0)
		{
			MessageBox.Show("PackType Input is Required");
			cmbPackTypeInput.Focus();
			return false;
		}
		if (txtqtyinput.Text.Trim() == string.Empty || txtqtyinput.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtqtyinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbpacksizeinput).ActiveRow == null || Conversion.ToInt(cmbpacksizeinput.Value) == 0)
		{
			MessageBox.Show("Pack Size Field is Required");
			cmbpacksizeinput.Focus();
			return false;
		}
		if (txtweightinput.Text.Trim() == string.Empty || txtweightinput.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field is Required");
			txtweightinput.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && Conversion.ToDouble((object)txtRateInPut.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("ItemRate Field is Required");
			txtRateInPut.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && (((UltraGridBase)CmbRateUomInPut).ActiveRow == null || Conversion.ToInt(CmbRateUomInPut.Value) == 0))
		{
			MessageBox.Show("RateUom Field is Required");
			cmbpacksizeinput.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && Conversion.ToDouble((object)txtAmountInPut.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtAmountInPut.Focus();
			return false;
		}
		double Stock = Conversion.ToDouble((object)lblStockWeight.Text);
		if (CheckStockAndGiveWarningMessageOnJobOrder && Stock <= 0.0 && (Conversion.ToString((object)txtremarksinput.Text) == null || txtremarksinput.Text == string.Empty))
		{
			MessageBox.Show("Remarks Field is Required");
			txtremarksinput.Focus();
			return false;
		}
		return true;
	}

	private void ResetDetailInPut()
	{
		try
		{
			updateDetailIndexInput = -1;
			cmbwarehouseinput.Focus();
			((Control)(object)cmbItemInput).Text = string.Empty;
			((Control)(object)cmbpacksizeinput).Text = string.Empty;
			txtqtyinput.Text = string.Empty;
			txtweightinput.Text = string.Empty;
			txtremarksinput.Text = string.Empty;
			txtRateInPut.Text = string.Empty;
			((Control)(object)CmbRateUomInPut).Text = string.Empty;
			txtAmountInPut.Text = string.Empty;
			btnadddetailinputgrd.Visible = true;
			btnupdategrdinput.Visible = false;
			btncancelgrdinput.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputDetailGridSetting()
	{
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		try
		{
			grdInputDetail.AllowDelete = (InheritableBoolean)2;
			grdInputDetail.AllowEdit = (InheritableBoolean)1;
			grdInputDetail.RootTable.Columns["Id"].Visible = false;
			grdInputDetail.RootTable.Columns["ItemId"].Visible = false;
			grdInputDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdInputDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdInputDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdInputDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdInputDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdInputDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdInputDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInputDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdInputDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdInputDetail.RootTable.Columns["PackSizeEquivalent"].Visible = false;
			grdInputDetail.RootTable.Columns["RateUomEquivalent"].Visible = false;
			if (JobOrderCreatewithoutRates)
			{
				grdInputDetail.RootTable.Columns["ItemRate"].Visible = false;
				grdInputDetail.RootTable.Columns["RateUom"].Visible = false;
				grdInputDetail.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdInputDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column);
				if (column.Key == "Qty" || column.Key == "Weight")
				{
					column.EditType = (EditType)1;
				}
				else
				{
					column.EditType = (EditType)0;
				}
				column.FilterEditType = (FilterEditType)1;
			}
			grdInputDetail.RootTable.Columns["DocDate"].Width = 80;
			grdInputDetail.RootTable.Columns["DocNo"].Width = 70;
			grdInputDetail.RootTable.Columns["WareHouse"].Width = 120;
			grdInputDetail.RootTable.Columns["JobLot"].Width = 120;
			grdInputDetail.RootTable.Columns["ItemName"].Width = 120;
			grdInputDetail.RootTable.Columns["CropYear"].Width = 90;
			grdInputDetail.RootTable.Columns["PackType"].Width = 100;
			grdInputDetail.RootTable.Columns["BalanceQty"].Width = 80;
			grdInputDetail.RootTable.Columns["BalanceWeight"].Width = 80;
			grdInputDetail.RootTable.Columns["Qty"].Width = 70;
			grdInputDetail.RootTable.Columns["PackSize"].Width = 70;
			grdInputDetail.RootTable.Columns["PackSizeEquivalent"].Width = 70;
			grdInputDetail.RootTable.Columns["Weight"].Width = 80;
			grdInputDetail.RootTable.Columns["ItemRate"].Width = 70;
			grdInputDetail.RootTable.Columns["RateUom"].Width = 70;
			grdInputDetail.RootTable.Columns["RateUomEquivalent"].Width = 70;
			grdInputDetail.RootTable.Columns["ItemAmount"].Width = 90;
			grdInputDetail.RootTable.Columns["Remarks"].Width = 120;
			grdInputDetail.RootTable.Columns.Add("Delete");
			grdInputDetail.RootTable.Columns["Delete"].Caption = "X";
			grdInputDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputDetail.RootTable.Columns["Delete"].Width = 25;
			grdInputDetail.RootTable.Columns["Delete"].Position = 0;
			grdInputDetail.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnadddetailinputgrd_Click(object sender, EventArgs e)
	{
		try
		{
			double Stock = Conversion.ToDouble((object)lblStockWeight.Text);
			if ((!CheckStockAndGiveWarningMessageOnJobOrder || !(Stock <= 0.0) || (Conversion.ToString((object)txtremarksinput.Text) != null && !(txtremarksinput.Text == string.Empty)) || MessageBox.Show("No stock is available for this item. Do you want to continue adding it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No) && FormValidationDetailInPut())
			{
				dtInputDetail.Rows.Add(0, Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), DateTime.Now, Conversion.ToInt((object)0), Conversion.ToInt(cmbwarehouseinput.Value), Conversion.ToString((object)((Control)(object)cmbwarehouseinput).Text.Trim()), Conversion.ToInt(cmbjoblotinput.Value), Conversion.ToString((object)((Control)(object)cmbjoblotinput).Text.Trim()), Conversion.ToInt(cmbItemInput.Value), Conversion.ToString((object)((Control)(object)cmbItemInput).Text.Trim()), Conversion.ToInt(cmbcropyearinput.Value), Conversion.ToString((object)((Control)(object)cmbcropyearinput).Text.Trim()), Conversion.ToInt(cmbPackTypeInput.Value), Conversion.ToString((object)((Control)(object)cmbPackTypeInput).Text.Trim()), Conversion.ToDouble((object)txtqtyinput.Text.Trim()), Conversion.ToDouble((object)txtweightinput.Text.Trim()), Conversion.ToDouble((object)txtqtyinput.Text.Trim()), Conversion.ToInt(cmbpacksizeinput.Value), Conversion.ToString((object)((Control)(object)cmbpacksizeinput).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)cmbpacksizeinput).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtweightinput.Text.Trim()), Conversion.ToDouble((object)txtRateInPut.Text), Conversion.ToInt(CmbRateUomInPut.Value), Conversion.ToString((object)((Control)(object)CmbRateUomInPut).Text), (((UltraGridBase)CmbRateUomInPut).ActiveRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbRateUomInPut).SelectedRow.Cells[2].Value) : 0.0, Conversion.ToDouble((object)txtAmountInPut.Text), Conversion.ToString((object)txtremarksinput.Text.Trim()));
				grdInputDetail.DataSource = dtInputDetail;
				grdInputDetail.RetrieveStructure();
				InputDetailGridSetting();
				ResetDetailInPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			if (InvProductionJobOrder.CheckJobOrderIdRefToInvFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 80,
				JobOrderId = RECID
			}) > 0)
			{
				throw new Exception("Record Not Update Because Record has exist in Production InPut");
			}
			GridEXRow item = grdInputDetail.CurrentRow;
			if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0)
			{
				throw new Exception("Record cannot be edit because this record against invoices loader");
			}
			if (item != null)
			{
				ItemDetailFill();
				updateDetailIndexInput = item.RowIndex;
				cmbwarehouseinput.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbjoblotinput.Value = item.Cells["JobLotId"].Value.ToString();
				cmbItemInput.Value = item.Cells["ItemId"].Value.ToString();
				cmbItemInput_Leave(null, null);
				cmbcropyearinput.Value = item.Cells["CropYearId"].Value.ToString();
				bindPackSizeInput(Conversion.ToInt(cmbItemInput.Value));
				txtqtyinput.Text = item.Cells["Qty"].Value.ToString();
				cmbpacksizeinput.Value = item.Cells["PackSizeId"].Value.ToString();
				txtweightinput.Text = item.Cells["Weight"].Value.ToString();
				cmbPackTypeInput.Value = item.Cells["PackTypeId"].Value.ToString();
				txtRateInPut.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUomInPut.Value = Conversion.ToInt((object)item.Cells["RateUomId"].Value.ToString());
				txtAmountInPut.Text = item.Cells["ItemAmount"].Value.ToString();
				txtremarksinput.Text = item.Cells["Remarks"].Value.ToString();
				btnadddetailinputgrd.Visible = false;
				btnupdategrdinput.Visible = true;
				btncancelgrdinput.Visible = true;
				((UltraGridBase)cmbentrytype).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdategrdinput_Click(object sender, EventArgs e)
	{
		try
		{
			double Stock = Conversion.ToDouble((object)lblStockWeight.Text);
			if ((!CheckStockAndGiveWarningMessageOnJobOrder || !(Stock <= 0.0) || (Conversion.ToString((object)txtremarksinput.Text) != null && !(txtremarksinput.Text == string.Empty)) || MessageBox.Show("No stock is available for this item. Do you want to continue adding it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No) && FormValidationDetailInPut())
			{
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouseId"] = Conversion.ToInt(cmbwarehouseinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouse"] = Conversion.ToString((object)((Control)(object)cmbwarehouseinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["JobLotId"] = Conversion.ToInt(cmbjoblotinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbjoblotinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemId"] = Conversion.ToInt(cmbItemInput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItemInput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["CropYearId"] = Conversion.ToInt(cmbcropyearinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbcropyearinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["PackTypeId"] = Conversion.ToInt(cmbPackTypeInput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["PackType"] = Conversion.ToString((object)((Control)(object)cmbPackTypeInput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["Qty"] = Conversion.ToDouble((object)txtqtyinput.Text);
				dtInputDetail.Rows[updateDetailIndexInput]["PackSizeId"] = Conversion.ToInt(cmbpacksizeinput.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["PackSize"] = Conversion.ToString((object)((Control)(object)cmbpacksizeinput).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["PackSizeEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)cmbpacksizeinput).SelectedRow.Cells[2].Value);
				dtInputDetail.Rows[updateDetailIndexInput]["Weight"] = Conversion.ToDouble((object)txtweightinput.Text);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemRate"] = Conversion.ToDouble((object)txtRateInPut.Text);
				dtInputDetail.Rows[updateDetailIndexInput]["RateUomId"] = Conversion.ToInt(CmbRateUomInPut.Value);
				dtInputDetail.Rows[updateDetailIndexInput]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomInPut).Text);
				dtInputDetail.Rows[updateDetailIndexInput]["RateUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomInPut).SelectedRow.Cells[2].Value);
				dtInputDetail.Rows[updateDetailIndexInput]["ItemAmount"] = Conversion.ToDouble((object)txtAmountInPut.Text);
				dtInputDetail.Rows[updateDetailIndexInput]["Remarks"] = Conversion.ToString((object)txtremarksinput.Text);
				grdInputDetail.DataSource = dtInputDetail;
				btnadddetailinputgrd.Visible = true;
				btnupdategrdinput.Visible = false;
				btncancelgrdinput.Visible = false;
				ResetDetailInPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btncancelgrdinput_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetailInPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			try
			{
				grdInputDetail.UpdateData();
				GridEXRow item = grdInputDetail.CurrentRow;
				double PreviousAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				if (e.Column.Key == "Qty")
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Uom = Conversion.ToDouble((object)item.Cells["PackSizeEquivalent"].Value.ToString());
					item.Cells["Weight"].Value = Qty * Uom;
					double Weight2 = Qty * Uom;
					double Rate2 = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
					double RateUOM2 = Conversion.ToDouble((object)item.Cells["RateUomEquivalent"].Value.ToString());
					double Amount2 = Weight2 / RateUOM2 * Rate2;
					item.Cells["ItemAmount"].Value = Amount2;
				}
				if (e.Column.Key == "Weight")
				{
					double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
					double RateUOM = Conversion.ToDouble((object)item.Cells["RateUomEquivalent"].Value.ToString());
					double Amount = Weight / RateUOM * Rate;
					if (Amount == 0.0)
					{
						item.Cells["ItemAmount"].Value = PreviousAmount;
						throw new Exception("Amount cannot be equal to zero please check");
					}
					item.Cells["ItemAmount"].Value = Amount;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void grdInputDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteInputdetailRow(r);
				}
				grdInputDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteInputdetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexInput == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionJobOrderInput jobOrderInputdetail = new InvProductionJobOrderInput();
					jobOrderInputdetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					jobOrderInputdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					jobOrderInputdetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					jobOrderInputdetail.BatchCrop = Conversion.ToString((object)r.Cells["CropYear"].Text);
					jobOrderInputdetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					jobOrderInputdetail.Qty = Conversion.ToDouble((object)r.Cells["Qty"].Text);
					jobOrderInputdetail.UomSchIdQty = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					jobOrderInputdetail.NetWeight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					jobOrderInputdetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					jobOrderInputdetail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					jobOrderInputdetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					jobOrderInputdetail.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					jobOrderInputdetail.ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					jobOrderInputdetail.RemarksInputDt = Conversion.ToString((object)r.Cells["Remarks"].Text);
					jobOrderInputdetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					jobOrderInputdetail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					jobOrderInputdetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					jobOrderInputdetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					jobOrderInputdetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					jobOrderInputdetail.ActionTypeId = 3;
					lstRemoveInputRecord.Add(jobOrderInputdetail);
					dtInputDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdInputDetail.UpdateData();
				btnGenerate_Click(null, null);
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetweightInput()
	{
		try
		{
			if (((UltraGridBase)cmbpacksizeinput).ActiveRow != null)
			{
				double uom = Conversion.ToDouble(((UltraDropDownBase)cmbpacksizeinput).SelectedRow.Cells[2].Value);
				double qty = Conversion.ToDouble((object)txtqtyinput.Text.Trim());
				if (uom > 0.0 && qty > 0.0)
				{
					txtweightinput.Text = Conversion.ToString((object)(uom * qty));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InPutAmountCalculations()
	{
		try
		{
			if (JobOrderCreatewithoutRates)
			{
				return;
			}
			double RateUom = 0.0;
			double Rate = 0.0;
			double Weight = 0.0;
			double Amount = 0.0;
			txtAmountInPut.Text = "0";
			if (Conversion.ToDouble((object)txtweightinput.Text.Trim()) > 0.0 && ((UltraGridBase)CmbRateUomInPut).ActiveRow != null)
			{
				RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomInPut).SelectedRow.Cells[2].Value);
				Rate = Conversion.ToDouble((object)txtRateInPut.Text.Trim());
				Weight = Conversion.ToDouble((object)txtweightinput.Text.Trim());
				if (Weight > 0.0 && Rate > 0.0 && RateUom > 0.0)
				{
					Amount = Weight / RateUom * Rate;
					txtAmountInPut.Text = Conversion.ToString((object)Amount);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqtyinput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
			InPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbpacksizeinput_VisibleChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbpacksizeinput_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
			InPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InPutRateBind()
	{
		try
		{
			if (JobOrderCreatewithoutRates)
			{
				return;
			}
			double rate = 0.0;
			int ItemId = Conversion.ToInt(cmbItemInput.Value);
			if (ItemId > 0)
			{
				rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)txtPlanDate.Value), 401, RECID, Conversion.ToInt(cmbjoblotinput.Value), Conversion.ToInt(cmbcropyearinput.Value), null, 0, UserAccount.BranchesId);
			}
			if (rate > 0.0)
			{
				double Rateuom = 1.0;
				if (((UltraGridBase)CmbRateUomInPut).DataSource is DataTable dt && dt.Rows.Count > 0)
				{
					DataRow Row = dt.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<double>("Equivalent") == 40.0);
					if (Row == null)
					{
						Row = dt.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<double>("Equivalent") == 1.0);
					}
					if (Row != null)
					{
						Rateuom = Conversion.ToDouble(Row["Equivalent"]);
						CmbRateUomInPut.Value = Conversion.ToInt(Row["Id"]);
					}
				}
				txtRateInPut.Text = (rate * Rateuom).ToString("#,##.##");
			}
			else
			{
				txtRateInPut.Text = "0";
				((Control)(object)CmbRateUomInPut).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateInPut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			InPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnGenerateInPutRates_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdInputDetail.RowCount == 0)
			{
				throw new Exception("Input Grid is Empty");
			}
			DateTime DocDate = Conversion.ToDateTime((object)txtPlanDate.Value);
			GridEXRow[] dataRows = grdInputDetail.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				Conversion.ToInt(r.Cells["WareHouseId"].Value);
				string CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				r.BeginEdit();
				double rate = CommonServices.AvgRateOnlyForCGS(ItemId, DocDate, 401, RECID, JobLotId, 0, CropYear, 0, UserAccount.BranchesId);
				if (rate > 0.0)
				{
					double Rateuom = 1.0;
					double Rate = 0.0;
					if (dtAllUomWithItemId != null && dtAllUomWithItemId.Rows.Count > 0)
					{
						DataRow Row = dtAllUomWithItemId.AsEnumerable().FirstOrDefault((DataRow d) => d.Field<double>("Equivalent") == 40.0 && d.Field<int>("ItemId") == ItemId);
						if (Row == null)
						{
							Row = dtAllUomWithItemId.AsEnumerable().FirstOrDefault((DataRow d) => d.Field<double>("Equivalent") == 1.0 && d.Field<int>("ItemId") == ItemId);
						}
						if (Row != null)
						{
							Rateuom = Conversion.ToDouble(Row["Equivalent"]);
							r.Cells["RateUomId"].Value = Conversion.ToInt(Row["Id"]);
							r.Cells["RateUom"].Value = Conversion.ToString(Row["UOMCode"]);
							r.Cells["RateUomEquivalent"].Value = Rateuom;
						}
					}
					Rate = rate * Rateuom;
					r.Cells["ItemRate"].Value = Rate.ToString();
					double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					double Amount = ((Rateuom > 0.0) ? (Weight / Rateuom * Rate) : 0.0);
					r.Cells["ItemAmount"].Value = Conversion.ToString((object)Amount);
				}
				r.EndEdit();
			}
			btnGenerate_Click(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInputDetail).Name);
			if (fs != null)
			{
				grdInputDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void EntryTypeOutPut()
	{
		try
		{
			DataTable dtentryType = new DataTable();
			dtentryType.Columns.Add("Id");
			dtentryType.Columns.Add("name");
			dtentryType.Rows.Add(1, "ByProduct");
			dtentryType.Rows.Add(2, "FinishGoods");
			DDL.BindDDL(dtentryType, CmbEntryTypeOP, "Id", "name", "EntryType", ZeroIndex: false);
			((UltraGridBase)CmbEntryTypeOP).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbItem.Value) > 0)
			{
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)cmbouteruom).Text;
			string Rateuom = ((Control)(object)CmbRateUomOutPut).Text;
			((Control)(object)cmbouteruom).Text = string.Empty;
			((Control)(object)CmbRateUomOutPut).Text = string.Empty;
			if (ItemId <= 0)
			{
				return;
			}
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("Equivalent", typeof(double));
			foreach (DataRow r in dtAllUomWithItemId.Rows)
			{
				if (Conversion.ToInt(r["ItemId"]) == ItemId)
				{
					dtUom.Rows.Add(r["Id"], r["UOMCode"], r["Equivalent"]);
				}
			}
			if (dtUom == null || dtUom.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUom, cmbouteruom, "Id", "UOMCode", "Outer Uom", ZeroIndex: false);
			DDL.BindDDL(dtUom, CmbRateUomOutPut, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
			((UltraGridBase)cmbouteruom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)CmbRateUomOutPut).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			if (!string.IsNullOrEmpty(Packuom))
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbouteruom).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbouteruom).Text = string.Empty;
				}
			}
			if (!string.IsNullOrEmpty(Rateuom))
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUomOutPut).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUomOutPut).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetailOutPut()
	{
		try
		{
			updateDetailIndexoutput = -1;
			((Control)(object)cmbItem).Text = string.Empty;
			txtOuterqty.Text = string.Empty;
			((Control)(object)cmbouteruom).Text = string.Empty;
			txtweight.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtItemRateOutPut.Text = string.Empty;
			((Control)(object)CmbRateUomOutPut).Text = string.Empty;
			txtItemAmountOutPut.Text = string.Empty;
			CmbEntryTypeOP.Focus();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetailOutPut()
	{
		if (((UltraGridBase)CmbEntryTypeOP).ActiveRow == null || Conversion.ToInt(CmbEntryTypeOP.Value) == 0)
		{
			MessageBox.Show("EntryType Field is Required");
			CmbEntryTypeOP.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseOutPut).ActiveRow == null || Conversion.ToInt(cmbWareHouseOutPut.Value) == 0)
		{
			MessageBox.Show("WareHouse is Required");
			cmbWareHouseOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLotOutput).ActiveRow == null || Conversion.ToInt(cmbJobLotOutput.Value) == 0)
		{
			MessageBox.Show("JobLot is Required");
			cmbJobLotOutput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYearOutput).ActiveRow == null || Conversion.ToInt(cmbCropYearOutput.Value) == 0)
		{
			MessageBox.Show("CropYear is Required");
			cmbCropYearOutput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null || Conversion.ToInt(cmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			cmbPackingType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtOuterqty.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Outer Qty Field is Required");
			txtOuterqty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbouteruom).ActiveRow == null || Conversion.ToInt(cmbouteruom.Value) == 0)
		{
			MessageBox.Show("Outer UOM Field is Required");
			cmbouteruom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtweight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Weight is Required");
			txtweight.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && Conversion.ToDouble((object)txtItemRateOutPut.Text.Trim()) == 0.0 && ((Control)(object)CmbEntryTypeOP).Text != "AutoRate")
		{
			MessageBox.Show("ItemRate Field is Required");
			txtItemRateOutPut.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && (((UltraGridBase)CmbRateUomOutPut).ActiveRow == null || Conversion.ToInt(CmbRateUomOutPut.Value) == 0))
		{
			MessageBox.Show("Item Rate Uom Field is Required");
			CmbRateUomOutPut.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()) == 0.0 && ((Control)(object)CmbEntryTypeOP).Text != "AutoRate")
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtItemAmountOutPut.Focus();
			return false;
		}
		return true;
	}

	private void txtPrctOfInputforOutputQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!double.TryParse(txtPrctOfInputforOutputQty.Text, out var percentage) || percentage <= 0.0)
			{
				return;
			}
			if (percentage > 100.0)
			{
				percentage = 100.0;
				txtPrctOfInputforOutputQty.Text = percentage.ToString("#,##");
				txtPrctOfInputforOutputQty.SelectionStart = txtPrctOfInputforOutputQty.Text.Length;
			}
			if (grdInputDetail.RowCount == 0)
			{
				MessageBox.Show("Input Data Not found...");
				return;
			}
			double outputWeight = GridEX_Helper.GetColumnSum(grdInputDetail, "Weight") * (percentage / 100.0);
			double outerUom = 0.0;
			if (((UltraGridBase)cmbouteruom).ActiveRow != null)
			{
				outerUom = Conversion.ToDouble(((UltraDropDownBase)cmbouteruom).SelectedRow.Cells[2].Value);
			}
			double outputQty = ((outerUom > 0.0) ? (outputWeight / outerUom) : 0.0);
			txtOuterqty.Text = outputQty.ToString("#,##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Getweight()
	{
		try
		{
			if (((UltraGridBase)cmbouteruom).ActiveRow != null)
			{
				double outeruom = Conversion.ToDouble(((UltraDropDownBase)cmbouteruom).SelectedRow.Cells[2].Value);
				double outerqty = Conversion.ToDouble((object)txtOuterqty.Text.Trim());
				if (outeruom > 0.0 && outerqty > 0.0)
				{
					txtweight.Text = Conversion.ToString((object)(outeruom * outerqty));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutAmountCalculations()
	{
		try
		{
			double RateUom = 0.0;
			double Rate = 0.0;
			double Weight = 0.0;
			double Amount = 0.0;
			txtItemAmountOutPut.Text = "0";
			if (Conversion.ToDouble((object)txtweight.Text.Trim()) > 0.0 && ((UltraGridBase)CmbRateUomOutPut).ActiveRow != null)
			{
				RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells[2].Value);
				Rate = Conversion.ToDouble((object)txtItemRateOutPut.Text.Trim());
				Weight = Conversion.ToDouble((object)txtweight.Text.Trim());
				if (Weight > 0.0 && Rate > 0.0 && RateUom > 0.0)
				{
					Amount = Weight / RateUom * Rate;
					txtItemAmountOutPut.Text = Conversion.ToString((object)Amount);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOuterqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Getweight();
			OutPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbouteruom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			Getweight();
			OutPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUomOutPut_TextChanged(object sender, EventArgs e)
	{
		OutPutAmountCalculations();
	}

	private void CmbLabourItemOutPut_TextChanged(object sender, EventArgs e)
	{
		OutPutAmountCalculations();
	}

	private void txtItemRateOutPut_TextChanged(object sender, EventArgs e)
	{
		OutPutAmountCalculations();
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetailOutPut())
			{
				dtPlannedOutput.Rows.Add(0, Conversion.ToInt(CmbEntryTypeOP.Value), Conversion.ToString((object)((Control)(object)CmbEntryTypeOP).Text), Conversion.ToInt(cmbWareHouseOutPut.Value), Conversion.ToString((object)((Control)(object)cmbWareHouseOutPut).Text.Trim()), Conversion.ToInt(cmbJobLotOutput.Value), Conversion.ToString((object)((Control)(object)cmbJobLotOutput).Text.Trim()), Conversion.ToInt(cmbItem.Value), Conversion.ToString((object)((Control)(object)cmbItem).Text.Trim()), Conversion.ToInt(cmbCropYearOutput.Value), Conversion.ToString((object)((Control)(object)cmbCropYearOutput).Text.Trim()), Conversion.ToInt(cmbPackingType.Value), Conversion.ToString((object)((Control)(object)cmbPackingType).Text.Trim()), Conversion.ToDouble((object)txtOuterqty.Text.Trim()), Conversion.ToInt(cmbouteruom.Value), Conversion.ToString((object)((Control)(object)cmbouteruom).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)cmbouteruom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtItemRateOutPut.Text), Conversion.ToInt(CmbRateUomOutPut.Value), Conversion.ToString((object)((Control)(object)CmbRateUomOutPut).Text), JobOrderCreatewithoutRates ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()), 0, 0, 0, 0, Conversion.ToDouble((object)txtItemRateOutPut.Text), Conversion.ToDouble((object)txtItemAmountOutPut.Text.Trim()), Conversion.ToString((object)txtremarks.Text.Trim()));
				grdPlannedOutput.DataSource = dtPlannedOutput;
				grdPlannedOutput.RetrieveStructure();
				PlannedOutputGridSetting();
				ResetDetailOutPut();
				InsertPMInOutPutGrid();
				InsertOhInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlannedOutput_DoubleClick(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdPlannedOutput.CurrentRow;
			if (item != null)
			{
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0 && InvProductionJobOrder.CheckJobOrderIdRefToInvFoodProduction(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 112,
					JobOrderId = RECID
				}) > 0)
				{
					throw new Exception("Record Not Update Because Record has exist in Production OutPut");
				}
				updateDetailIndexoutput = item.RowIndex;
				CmbEntryTypeOP.Value = item.Cells["EntryTypeId"].Value.ToString();
				cmbWareHouseOutPut.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbJobLotOutput.Value = item.Cells["JobLotId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
				cmbCropYearOutput.Value = item.Cells["CropYearId"].Value.ToString();
				cmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtOuterqty.Text = item.Cells["QtyOuter"].Value.ToString();
				cmbouteruom.Value = item.Cells["OuterPackSizeId"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtItemRateOutPut.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUomOutPut.Value = item.Cells["RateUomId"].Value.ToString();
				txtItemAmountOutPut.Text = item.Cells["ItemAmount"].Value.ToString();
				txtremarks.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				((UltraGridBase)cmbentrytype).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetailOutPut();
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
			if (FormValidationDetailOutPut())
			{
				dtPlannedOutput.Rows[updateDetailIndexoutput]["EntryTypeId"] = Conversion.ToInt(CmbEntryTypeOP.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["EntryType"] = Conversion.ToString((object)((Control)(object)CmbEntryTypeOP).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["WareHouseId"] = Conversion.ToInt(cmbWareHouseOutPut.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["WareHouseName"] = Conversion.ToString((object)((Control)(object)cmbWareHouseOutPut).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["JobLotId"] = Conversion.ToInt(cmbJobLotOutput.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["JobLotDescription"] = Conversion.ToString((object)((Control)(object)cmbJobLotOutput).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItem).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["CropYearId"] = Conversion.ToInt(cmbCropYearOutput.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbCropYearOutput).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["PackingTypeId"] = Conversion.ToInt(cmbPackingType.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["PackingTypeName"] = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["QtyOuter"] = Conversion.ToDouble((object)txtOuterqty.Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["OuterPackSizeId"] = Conversion.ToInt(cmbouteruom.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["OuterPackSizeName"] = Conversion.ToString((object)((Control)(object)cmbouteruom).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["EquivalentPack"] = Conversion.ToDouble(((UltraDropDownBase)cmbouteruom).SelectedRow.Cells["Equivalent"].Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["Weight"] = Conversion.ToDouble((object)txtweight.Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemRate"] = Conversion.ToDouble((object)txtItemRateOutPut.Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["RateUomId"] = Conversion.ToInt(CmbRateUomOutPut.Value);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomOutPut).Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["EquivalentRate"] = (JobOrderCreatewithoutRates ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)CmbRateUomOutPut).SelectedRow.Cells["Equivalent"].Value));
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemAmount"] = Conversion.ToDouble((object)txtItemAmountOutPut.Text);
				dtPlannedOutput.Rows[updateDetailIndexoutput]["Remarks"] = Conversion.ToString((object)txtremarks.Text);
				grdPlannedOutput.DataSource = dtPlannedOutput;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetailOutPut();
				InsertOhInOutPutGrid();
				InsertPMInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PlannedOutputGridSetting()
	{
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		try
		{
			grdPlannedOutput.AllowDelete = (InheritableBoolean)2;
			grdPlannedOutput.AllowEdit = (InheritableBoolean)2;
			grdPlannedOutput.RootTable.Columns["Id"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EntryTypeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["ItemId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["CropYearId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["WareHouseId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["JobLotId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EquivalentPack"].Visible = false;
			grdPlannedOutput.RootTable.Columns["EquivalentRate"].Visible = false;
			grdPlannedOutput.RootTable.Columns["RateUomId"].Visible = false;
			if (JobOrderCreatewithoutRates)
			{
				grdPlannedOutput.RootTable.Columns["ItemRate"].Visible = false;
				grdPlannedOutput.RootTable.Columns["RateUom"].Visible = false;
				grdPlannedOutput.RootTable.Columns["ItemAmount"].Visible = false;
				grdPlannedOutput.RootTable.Columns["OHExpense"].Visible = false;
				grdPlannedOutput.RootTable.Columns["PMExpense"].Visible = false;
				grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].Visible = false;
				grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].Visible = false;
				grdPlannedOutput.RootTable.Columns["NetRate"].Visible = false;
				grdPlannedOutput.RootTable.Columns["NetAmount"].Visible = false;
			}
			grdPlannedOutput.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPlannedOutput.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdPlannedOutput.RootTable.Columns["EntryType"].Width = 120;
			grdPlannedOutput.RootTable.Columns["WareHouseName"].Width = 120;
			grdPlannedOutput.RootTable.Columns["JobLotDescription"].Width = 120;
			grdPlannedOutput.RootTable.Columns["ItemName"].Width = 120;
			grdPlannedOutput.RootTable.Columns["CropYear"].Width = 100;
			grdPlannedOutput.RootTable.Columns["PackingTypeName"].Width = 100;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].Width = 70;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeName"].Width = 80;
			grdPlannedOutput.RootTable.Columns["EquivalentPack"].Width = 80;
			grdPlannedOutput.RootTable.Columns["Weight"].Width = 80;
			grdPlannedOutput.RootTable.Columns["ItemRate"].Width = 80;
			grdPlannedOutput.RootTable.Columns["RateUom"].Width = 70;
			grdPlannedOutput.RootTable.Columns["EquivalentRate"].Width = 70;
			grdPlannedOutput.RootTable.Columns["ItemAmount"].Width = 100;
			grdPlannedOutput.RootTable.Columns["OHExpense"].Width = 100;
			grdPlannedOutput.RootTable.Columns["PMExpense"].Width = 100;
			grdPlannedOutput.RootTable.Columns["OHGeneralExpense"].Width = 100;
			grdPlannedOutput.RootTable.Columns["PMGeneralExpense"].Width = 100;
			grdPlannedOutput.RootTable.Columns["NetRate"].Width = 80;
			grdPlannedOutput.RootTable.Columns["NetAmount"].Width = 100;
			grdPlannedOutput.RootTable.Columns["Remarks"].Width = 150;
			grdPlannedOutput.RootTable.Columns.Add("Delete");
			grdPlannedOutput.RootTable.Columns["Delete"].Caption = "X";
			grdPlannedOutput.RootTable.Columns["Delete"].ButtonText = "X";
			grdPlannedOutput.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPlannedOutput.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPlannedOutput.RootTable.Columns["Delete"].Width = 25;
			grdPlannedOutput.RootTable.Columns["Delete"].Position = 0;
			grdPlannedOutput.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlannedOutput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdPlannedOutput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPlannedOutput.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				OutputDetailRowDelete(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutputDetailRowDelete(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexoutput == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionJobOrderOutput jobOrderOutputdetail = new InvProductionJobOrderOutput();
					jobOrderOutputdetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					jobOrderOutputdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					jobOrderOutputdetail.InvPackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					jobOrderOutputdetail.QtyInner = Conversion.ToDouble((object)r.Cells["QtyOuter"].Text);
					jobOrderOutputdetail.QtyOuter = Conversion.ToDouble((object)r.Cells["QtyOuter"].Text);
					jobOrderOutputdetail.UomSchIdInner = Conversion.ToInt((object)r.Cells["OuterPackSizeId"].Text);
					jobOrderOutputdetail.UomSchIdOuter = Conversion.ToInt((object)r.Cells["OuterPackSizeId"].Text);
					jobOrderOutputdetail.NetWeight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					jobOrderOutputdetail.PackRemarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					jobOrderOutputdetail.CropYearId = Conversion.ToInt((object)r.Cells["CropYearId"].Text);
					jobOrderOutputdetail.WarehouseId = Conversion.ToInt((object)r.Cells["WareHouseId"].Text);
					jobOrderOutputdetail.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
					jobOrderOutputdetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					jobOrderOutputdetail.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					jobOrderOutputdetail.ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					jobOrderOutputdetail.OHExpense = Conversion.ToDecimal(r.Cells["OHExpense"].Value);
					jobOrderOutputdetail.PMExpense = Conversion.ToDecimal(r.Cells["PMExpense"].Value);
					jobOrderOutputdetail.OHGeneralExpense = Conversion.ToDecimal(r.Cells["OHGeneralExpense"].Value);
					jobOrderOutputdetail.PMGeneralExpense = Conversion.ToDecimal(r.Cells["PMGeneralExpense"].Value);
					jobOrderOutputdetail.NetRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
					jobOrderOutputdetail.NetAmount = Conversion.ToDecimal(r.Cells["NetAmount"].Value);
					jobOrderOutputdetail.EntryTypeId = Conversion.ToInt(r.Cells["EntryTypeId"].Value);
					jobOrderOutputdetail.ActionTypeId = 3;
					lstRemoveOutputRecord.Add(jobOrderOutputdetail);
					dtPlannedOutput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdPlannedOutput.UpdateData();
				InsertPMInOutPutGrid();
				InsertOhInOutPutGrid();
				btnGenerate_Click(null, null);
				return;
			}
			throw new Exception("Reset Detail First");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPlannedOutput).Name);
			if (fs != null)
			{
				grdPlannedOutput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtPrctOfInputforOutputQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemRateOutPut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOuterqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqtyinput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtweightinput_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ChartOfAccountOverHead()
	{
		try
		{
			DataTable CoaList = CommonServices.CoaAllocationGetAllServiceBind();
			if (CoaList.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < CoaList.Rows.Count; i++)
			{
				if (Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 6 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 8 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 11 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 12 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 13 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 14 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 20 || Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) == 21)
				{
					dt.Rows.Add(CoaList.Rows[i]["Id"], CoaList.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbChartofAccountOverHead, "Id", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandNameOverHead_Leave(object sender, EventArgs e)
	{
		try
		{
			BindBrandItemsUOMFromOutPutGrid();
			CaluculatonOfOverHeads();
			CalculateQtyOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandUomOverHead_Leave(object sender, EventArgs e)
	{
		CaluculatonOfOverHeads();
		CalculateQtyOverHeads();
	}

	private DataTable GetBrandItems(bool onlyFinished = false)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		DataTable dt = new DataTable();
		dt.Columns.Add("BrandItemId", typeof(int));
		dt.Columns.Add("BrandItem", typeof(string));
		GridEXRow[] rows = grdPlannedOutput.GetRows();
		foreach (GridEXRow item in rows)
		{
			if ((int)item.RowType == 0 && (!onlyFinished || item.Cells["EntryTypeId"].Value as int? == 2))
			{
				dt.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Value);
			}
		}
		return dt;
	}

	private void BrandItemBindOnOhandPM(DataTable allItems)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBrandNameOverHead, allItems, "BrandItemId", "BrandItem", "Brand Item Name", AllColumns: true, Conversion.ToInt(CmbBrandNameOverHead.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrandNamePackingMaterial, allItems, "BrandItemId", "BrandItem", "Brand Item Name", AllColumns: true, Conversion.ToInt(CmbBrandNamePackingMaterial.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrandItemBindonLabStandard(DataTable finishedItems)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBrandNameLabStandard, finishedItems, "BrandItemId", "BrandItem", "Brand Item Name", AllColumns: true, Conversion.ToInt(CmbBrandNameLabStandard.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (finishedItems != null && finishedItems.Rows.Count > 0)
			{
				CmbBrandNameLabStandard_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindBrandItemsUOMFromOutPutGrid()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdPlannedOutput.CurrentRow != null && (int)grdPlannedOutput.CurrentRow.RowType == 0 && grdPlannedOutput.CurrentRow.Cells.Count > 0)
			{
				DataTable dtBrandItem = new DataTable();
				dtBrandItem.Columns.Add("BrandItemUOMId");
				dtBrandItem.Columns.Add("BrandItemUOM");
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(CmbBrandNameOverHead.Value) == Conversion.ToInt(item.Cells["ItemId"].Value))
					{
						dtBrandItem.Rows.Add(item.Cells["OuterPackSizeId"].Value, item.Cells["OuterPackSizeName"].Value);
					}
				}
				if (dtBrandItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtBrandItem, CmbBrandUomOverHead, "BrandItemUOMId", "BrandItemUOM", "BrandItem UOM", ZeroIndex: false);
					return;
				}
				((UltraGridBase)CmbBrandUomOverHead).DataSource = null;
				((Control)(object)CmbBrandUomOverHead).Text = string.Empty;
			}
			else
			{
				((UltraGridBase)CmbBrandUomOverHead).DataSource = null;
				((Control)(object)CmbBrandUomOverHead).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOfDetailPortionOverheads()
	{
		if (((UltraGridBase)cmbChartofAccountOverHead).ActiveRow == null || Conversion.ToInt(cmbChartofAccountOverHead.Value) == 0)
		{
			MessageBox.Show("OverHead Account Field Required");
			cmbChartofAccountOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNameOverHead).ActiveRow != null && Conversion.ToInt(CmbBrandNameOverHead.Value) == 0 && (((UltraGridBase)CmbBrandUomOverHead).ActiveRow == null || Conversion.ToInt(CmbBrandUomOverHead.Value) == 0))
		{
			MessageBox.Show("outPut ItemUOM Field Required");
			CmbBrandUomOverHead.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && (txtExpAmountOverHead.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()) == 0.0))
		{
			MessageBox.Show("Amount Field Required");
			txtExpAmountOverHead.Focus();
			return false;
		}
		return true;
	}

	private void ResetdetailOverHead()
	{
		try
		{
			updateDetailIndexOH = -1;
			cmbChartofAccountOverHead.Focus();
			txtLedgerRemarksOverHead.Text = string.Empty;
			txtItemQtyOverHead.Text = string.Empty;
			txtItemRateOverHeads.Text = string.Empty;
			txtExpAmountOverHead.Text = string.Empty;
			((Control)(object)CmbBrandNameOverHead).Text = string.Empty;
			((Control)(object)CmbBrandUomOverHead).Text = string.Empty;
			((Control)(object)cmbChartofAccountOverHead).Text = string.Empty;
			btnAddOH.Visible = true;
			btnUpdateOH.Visible = false;
			BtnCancelOH.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddOH_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOverheads())
			{
				dtOH.Rows.Add(0, Conversion.ToInt(cmbChartofAccountOverHead.Value), Conversion.ToString((object)((Control)(object)cmbChartofAccountOverHead).Text.Trim()), Conversion.ToInt(CmbBrandNameOverHead.Value), Conversion.ToString((object)((Control)(object)CmbBrandNameOverHead).Text.Trim()), Conversion.ToInt(CmbBrandUomOverHead.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomOverHead).Text.Trim()), Conversion.ToDouble((object)txtItemQtyOverHead.Text.Trim()), Conversion.ToDouble((object)txtItemRateOverHeads.Text.Trim()), Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()), Conversion.ToString((object)txtLedgerRemarksOverHead.Text.Trim()));
				grdOverHead.DataSource = dtOH;
				grdOverHead.RetrieveStructure();
				ResetdetailOverHead();
				grdDetailOverheadSettings();
				cmbChartofAccountOverHead.Focus();
				InsertOhInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOH_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOverheads())
			{
				dtOH.Rows[updateDetailIndexOH]["ChartOfAccountId"] = Conversion.ToInt(cmbChartofAccountOverHead.Value);
				dtOH.Rows[updateDetailIndexOH]["ChartOfAccount"] = Conversion.ToString((object)((Control)(object)cmbChartofAccountOverHead).Text);
				dtOH.Rows[updateDetailIndexOH]["BrandId"] = Conversion.ToInt(CmbBrandNameOverHead.Value);
				dtOH.Rows[updateDetailIndexOH]["Brand"] = Conversion.ToString((object)((Control)(object)CmbBrandNameOverHead).Text);
				dtOH.Rows[updateDetailIndexOH]["BrandUomId"] = Conversion.ToInt(CmbBrandUomOverHead.Value);
				dtOH.Rows[updateDetailIndexOH]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomOverHead).Text);
				dtOH.Rows[updateDetailIndexOH]["ItemQty"] = txtItemQtyOverHead.Text.Trim();
				dtOH.Rows[updateDetailIndexOH]["ItemRate"] = txtItemRateOverHeads.Text.Trim();
				dtOH.Rows[updateDetailIndexOH]["Amount"] = txtExpAmountOverHead.Text.Trim();
				dtOH.Rows[updateDetailIndexOH]["Remarks"] = txtLedgerRemarksOverHead.Text.Trim();
				ResetdetailOverHead();
				InsertOhInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOH_Click(object sender, EventArgs e)
	{
		try
		{
			ResetdetailOverHead();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailOverheadSettings()
	{
		try
		{
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["ChartOfAccountId"].Visible = false;
			grdOverHead.RootTable.Columns["BrandId"].Visible = false;
			grdOverHead.RootTable.Columns["BrandUomId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdOverHead, 2);
			grdOverHead.RootTable.Columns["ChartOfAccount"].Width = 150;
			grdOverHead.RootTable.Columns["Brand"].Width = 180;
			grdOverHead.RootTable.Columns["BrandUom"].Width = 70;
			grdOverHead.RootTable.Columns["ItemQty"].Width = 70;
			grdOverHead.RootTable.Columns["ItemRate"].Width = 70;
			grdOverHead.RootTable.Columns["Amount"].Width = 90;
			grdOverHead.RootTable.Columns["Remarks"].Width = 150;
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].Width = 25;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdOverHead.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOH = item.RowIndex;
				cmbChartofAccountOverHead.Value = item.Cells["ChartOfAccountId"].Value;
				((Control)(object)cmbChartofAccountOverHead).Text = item.Cells["ChartOfAccount"].Text.ToString();
				CmbBrandNameOverHead.Value = item.Cells["BrandId"].Value;
				((Control)(object)CmbBrandNameOverHead).Text = item.Cells["Brand"].Text.ToString();
				CmbBrandUomOverHead.Value = item.Cells["BrandUomId"].Value;
				((Control)(object)CmbBrandUomOverHead).Text = item.Cells["BrandUom"].Text.ToString();
				txtItemQtyOverHead.Text = item.Cells["ItemQty"].Text.ToString();
				txtItemRateOverHeads.Text = item.Cells["ItemRate"].Text.ToString();
				txtExpAmountOverHead.Text = item.Cells["Amount"].Text.ToString();
				txtLedgerRemarksOverHead.Text = item.Cells["Remarks"].Text.ToString();
				btnAddOH.Visible = false;
				btnUpdateOH.Visible = true;
				BtnCancelOH.Visible = true;
				cmbChartofAccountOverHead.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				OverHeadDetailRowDelete(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadDetailRowDelete(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexOH == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionJobOrderOverHeads OverHeaddetail = new InvProductionJobOrderOverHeads();
					OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					OverHeaddetail.CharOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
					OverHeaddetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					OverHeaddetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					OverHeaddetail.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					OverHeaddetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					OverHeaddetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					OverHeaddetail.ohRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					OverHeaddetail.ActionTypeId = 3;
					lstRemoveOHRecord.Add(OverHeaddetail);
					dtOH.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdOverHead.UpdateData();
				InsertOhInOutPutGrid();
				btnGenerate_Click(null, null);
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateQtyOverHeads()
	{
		try
		{
			double Qty = 0.0;
			Qty = GetBrandQtyByBrandUomAndBrnadUom(Conversion.ToInt(CmbBrandNameOverHead.Value), Conversion.ToInt(CmbBrandUomOverHead.Value));
			if (Qty != 0.0)
			{
				txtItemQtyOverHead.Text = Qty.ToString();
			}
			else
			{
				txtItemQtyOverHead.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CaluculatonOfOverHeads()
	{
		try
		{
			if (txtItemQtyOverHead.Text != string.Empty && txtItemRateOverHeads.Text != string.Empty)
			{
				double Amount = double.Parse(txtItemQtyOverHead.Text) * double.Parse(txtItemRateOverHeads.Text);
				txtExpAmountOverHead.Text = Math.Round(Amount).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQtyOverHead_Leave(object sender, EventArgs e)
	{
		try
		{
			CaluculatonOfOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemRateOverHeads_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CaluculatonOfOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateUomOverHeads_Leave(object sender, EventArgs e)
	{
		try
		{
			CaluculatonOfOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQtyOverHead_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemRateOverHeads_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOverHead).Name);
			if (fs != null)
			{
				grdOverHead.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BindBrandItemsUOMFromPackingMaterialGrid()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdPlannedOutput.CurrentRow != null && (int)grdPlannedOutput.CurrentRow.RowType == 0 && grdPlannedOutput.CurrentRow.Cells.Count > 0)
			{
				DataTable dtBrandItem = new DataTable();
				dtBrandItem.Columns.Add("BrandItemUOMId");
				dtBrandItem.Columns.Add("BrandItemUOM");
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(CmbBrandNamePackingMaterial.Value) == Conversion.ToInt(item.Cells["ItemId"].Value))
					{
						dtBrandItem.Rows.Add(item.Cells["OuterPackSizeId"].Value, item.Cells["OuterPackSizeName"].Value);
					}
				}
				if (dtBrandItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtBrandItem, CmbBrandUomPackingMaterial, "BrandItemUOMId", "BrandItemUOM", "BrandItem UOM", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)CmbBrandUomPackingMaterial).DataSource = null;
				((Control)(object)CmbBrandUomPackingMaterial).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandNamePackingMaterial_Leave(object sender, EventArgs e)
	{
		BindBrandItemsUOMFromPackingMaterialGrid();
		CalculateQtyPackingMaterial();
	}

	private bool FormValidationOfDetailPortionPackingMaterial()
	{
		if (((UltraGridBase)CmbWarehousePackingMaterial).ActiveRow == null || Conversion.ToInt(CmbWarehousePackingMaterial.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehousePackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemPackingMaterial).ActiveRow == null || Conversion.ToInt(cmbItemPackingMaterial.Value) == 0)
		{
			MessageBox.Show("Packing Material Item Field Required");
			cmbItemPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNamePackingMaterial).ActiveRow != null && Conversion.ToInt(CmbBrandNamePackingMaterial.Value) == 0 && (((UltraGridBase)CmbBrandUomPackingMaterial).ActiveRow == null || Conversion.ToInt(CmbBrandUomPackingMaterial.Value) == 0))
		{
			MessageBox.Show("outPut ItemUOM Field Required");
			CmbBrandUomPackingMaterial.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && (txtPackingMaterialRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim()) == 0.0))
		{
			MessageBox.Show("Rate Field Required");
			txtPackingMaterialRate.Focus();
			return false;
		}
		if (!JobOrderCreatewithoutRates && (txtAmountPackingMaterial.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim()) == 0.0))
		{
			MessageBox.Show("Amount Field Required");
			txtAmountPackingMaterial.Focus();
			return false;
		}
		return true;
	}

	private void ResetdetailPackingMaterial()
	{
		try
		{
			updateDetailIndexPM = -1;
			CmbWarehousePackingMaterial.Focus();
			txtremarksPackingMaterial.Text = string.Empty;
			txtPackingMaterialQTY.Text = string.Empty;
			txtPackingMaterialRate.Text = string.Empty;
			txtAmountPackingMaterial.Text = string.Empty;
			((Control)(object)CmbBrandNamePackingMaterial).Text = string.Empty;
			((Control)(object)CmbBrandUomPackingMaterial).Text = string.Empty;
			((Control)(object)cmbItemPackingMaterial).Text = string.Empty;
			btnAddDetailPm.Visible = true;
			btnUpdateDetailPm.Visible = false;
			btnCancelDetailPm.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetailPm_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionPackingMaterial())
			{
				dtPM.Rows.Add(0, Conversion.ToInt(CmbWarehousePackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbWarehousePackingMaterial).Text.Trim()), Conversion.ToInt(cmbItemPackingMaterial.Value), Conversion.ToString((object)((Control)(object)cmbItemPackingMaterial).Text.Trim()), Conversion.ToInt(CmbBrandNamePackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbBrandNamePackingMaterial).Text.Trim()), Conversion.ToInt(CmbBrandUomPackingMaterial.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomPackingMaterial).Text.Trim()), Conversion.ToDouble((object)txtPackingMaterialQTY.Text.Trim()), Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim()), Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim()), Conversion.ToString((object)txtremarksPackingMaterial.Text.Trim()));
				grdPackingMaterial.DataSource = dtPM;
				grdPackingMaterial.RetrieveStructure();
				ResetdetailPackingMaterial();
				grdDetailPackingMaterialSettings();
				CmbWarehousePackingMaterial.Focus();
				InsertPMInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailPm_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionPackingMaterial())
			{
				dtPM.Rows[updateDetailIndexPM]["WarehouseId"] = Conversion.ToInt(CmbWarehousePackingMaterial.Value);
				dtPM.Rows[updateDetailIndexPM]["Warehouse"] = Conversion.ToString((object)((Control)(object)CmbWarehousePackingMaterial).Text);
				dtPM.Rows[updateDetailIndexPM]["PackingItemId"] = Conversion.ToInt(cmbItemPackingMaterial.Value);
				dtPM.Rows[updateDetailIndexPM]["PackingItem"] = Conversion.ToString((object)((Control)(object)cmbItemPackingMaterial).Text);
				dtPM.Rows[updateDetailIndexPM]["BrandId"] = Conversion.ToInt(CmbBrandNamePackingMaterial.Value);
				dtPM.Rows[updateDetailIndexPM]["Brand"] = Conversion.ToString((object)((Control)(object)CmbBrandNamePackingMaterial).Text);
				dtPM.Rows[updateDetailIndexPM]["BrandUomId"] = Conversion.ToInt(CmbBrandUomPackingMaterial.Value);
				dtPM.Rows[updateDetailIndexPM]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomPackingMaterial).Text);
				dtPM.Rows[updateDetailIndexPM]["ItemQty"] = Conversion.ToDouble((object)txtPackingMaterialQTY.Text.Trim());
				dtPM.Rows[updateDetailIndexPM]["ItemRate"] = Conversion.ToDouble((object)txtPackingMaterialRate.Text.Trim());
				dtPM.Rows[updateDetailIndexPM]["Amount"] = Conversion.ToDouble((object)txtAmountPackingMaterial.Text.Trim());
				dtPM.Rows[updateDetailIndexPM]["Remarks"] = Conversion.ToString((object)txtremarksPackingMaterial.Text.Trim());
				ResetdetailPackingMaterial();
				InsertPMInOutPutGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetailPm_Click(object sender, EventArgs e)
	{
		ResetdetailPackingMaterial();
	}

	private void grdPackingMaterial_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPM = item.RowIndex;
				CmbWarehousePackingMaterial.Value = item.Cells["WarehouseId"].Value;
				((Control)(object)CmbWarehousePackingMaterial).Text = item.Cells["Warehouse"].Text.ToString();
				cmbItemPackingMaterial.Value = item.Cells["PackingItemId"].Value;
				((Control)(object)cmbItemPackingMaterial).Text = item.Cells["PackingItem"].Text.ToString();
				CmbBrandNamePackingMaterial.Value = item.Cells["BrandId"].Value;
				((Control)(object)CmbBrandNamePackingMaterial).Text = item.Cells["Brand"].Text.ToString();
				CmbBrandUomPackingMaterial.Value = item.Cells["BrandUomId"].Value;
				((Control)(object)CmbBrandUomPackingMaterial).Text = item.Cells["BrandUom"].Text.ToString();
				txtPackingMaterialQTY.Text = item.Cells["ItemQty"].Text.ToString();
				txtPackingMaterialRate.Text = item.Cells["ItemRate"].Text.ToString();
				txtAmountPackingMaterial.Text = item.Cells["Amount"].Text.ToString();
				txtremarksPackingMaterial.Text = item.Cells["Remarks"].Text.ToString();
				btnAddDetailPm.Visible = false;
				btnUpdateDetailPm.Visible = true;
				btnCancelDetailPm.Visible = true;
				CmbWarehousePackingMaterial.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailPackingMaterialSettings()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdPackingMaterial.RootTable.Columns["ItemRate"].Visible = false;
				grdPackingMaterial.RootTable.Columns["Amount"].Visible = false;
			}
			grdPackingMaterial.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterial.RootTable.Columns["PackingItemId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["WarehouseId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["BrandUomId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["BrandId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdPackingMaterial, 2);
			grdPackingMaterial.RootTable.Columns["Warehouse"].Width = 150;
			grdPackingMaterial.RootTable.Columns["PackingItem"].Width = 150;
			grdPackingMaterial.RootTable.Columns["Brand"].Width = 180;
			grdPackingMaterial.RootTable.Columns["BrandUom"].Width = 70;
			grdPackingMaterial.RootTable.Columns["ItemQty"].Width = 70;
			grdPackingMaterial.RootTable.Columns["ItemRate"].Width = 70;
			grdPackingMaterial.RootTable.Columns["Amount"].Width = 90;
			grdPackingMaterial.RootTable.Columns["Remarks"].Width = 150;
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 25;
			grdPackingMaterial.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterial.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				PackingMaterialDetailrowDelete(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialDetailrowDelete(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexoutput == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionJobOrderPackingMaterial PackingMaterialDetail = new InvProductionJobOrderPackingMaterial();
					PackingMaterialDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					PackingMaterialDetail.ItemId = Conversion.ToInt(r.Cells["PackingItemId"].Value);
					PackingMaterialDetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					PackingMaterialDetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					PackingMaterialDetail.Qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					PackingMaterialDetail.Rate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					PackingMaterialDetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PackingMaterialDetail.pmRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					PackingMaterialDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					PackingMaterialDetail.ActionTypeId = 3;
					lstRemovePMRecord.Add(PackingMaterialDetail);
					dtPM.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdPackingMaterial.UpdateData();
				InsertPMInOutPutGrid();
				btnGenerate_Click(null, null);
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateQtyPackingMaterial()
	{
		try
		{
			double Qty = 0.0;
			Qty = GetBrandQtyByBrandUomAndBrnadUom(Conversion.ToInt(CmbBrandNamePackingMaterial.Value), Conversion.ToInt(CmbBrandUomPackingMaterial.Value));
			if (Qty != 0.0)
			{
				txtPackingMaterialQTY.Text = Qty.ToString();
			}
			else
			{
				txtPackingMaterialQTY.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialQTY_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CaluculatonofPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CaluculatonofPackingMaterial()
	{
		try
		{
			if (txtPackingMaterialQTY.Text != string.Empty && txtPackingMaterialRate.Text != string.Empty)
			{
				double fAmount = double.Parse(txtPackingMaterialRate.Text) * double.Parse(txtPackingMaterialQTY.Text);
				txtAmountPackingMaterial.Text = Math.Round(fAmount).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CaluculatonofPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrandUomPackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateQtyPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingMaterial).Name);
			if (fs != null)
			{
				grdPackingMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtPackingMaterialQTY_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingMaterialRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void HistoryJobOrderNofill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dtorder = new DataTable();
			dtorder.Columns.Add("Id");
			dtorder.Columns.Add("name");
			DataTable dtjob = InvProductionJobOrder.JobOrderNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtjob.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtjob.Rows.Count; i++)
			{
				if (Conversion.ToString(dtjob.Rows[i]["Activity"]) == "JobOrderNo")
				{
					dtorder.Rows.Add(dtjob.Rows[i]["Id"], dtjob.Rows[i]["ReferenceName"]);
				}
			}
			if (dtorder.Rows.Count > 0)
			{
				DDL.BindDDL(dtorder, xmbjoborderno, "Id", "name", "Job Order No", ZeroIndex: false);
				((UltraGridBase)xmbjoborderno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefereshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryJobOrderNofill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				ToDateHistory.Value = DateTime.Today;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistoryGrid();
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
		else
		{
			txtPlanDate.Focus();
		}
	}

	private void BindHistoryGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters joborder = new ReportsParameters();
			joborder.OrganizationId = UserAccount.OrganizationId;
			joborder.CompanyId = UserAccount.CompanyId;
			joborder.DocumentTypeIds = "401";
			joborder.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			joborder.EntryUser = UserAccount.ID;
			joborder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			joborder.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
			joborder.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
			joborder.InvJobOrderId = Conversion.ToInt(xmbjoborderno.Value);
			DataTable dtGetAll = InvProductionJobOrder.FormHistoryNew(joborder);
			if (dtGetAll != null && dtGetAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("PlanType", typeof(string));
				dt.Columns.Add("JobOrderNo", typeof(string));
				dt.Columns.Add("ProductionType", typeof(string));
				dt.Columns.Add("StartDate", typeof(DateTime));
				dt.Columns.Add("EndDate", typeof(DateTime));
				dt.Columns.Add("PlanStatus", typeof(string));
				dt.Columns.Add("PackingInstruction", typeof(string));
				dt.Columns.Add("DeliveryInstruction", typeof(string));
				dt.Columns.Add("ProductionInstruction", typeof(string));
				dt.Columns.Add("OtherInstruction", typeof(string));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedStatus");
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtGetAll.Rows.Count; i++)
				{
					dt.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["PlanDate"], dtGetAll.Rows[i]["PlanCode"], dtGetAll.Rows[i]["PlanType"], dtGetAll.Rows[i]["RefInvoiceNo"], dtGetAll.Rows[i]["ProductionType"], dtGetAll.Rows[i]["StartDate"], dtGetAll.Rows[i]["EndDate"], dtGetAll.Rows[i]["PlanStatus"], dtGetAll.Rows[i]["PackingInstructions"], dtGetAll.Rows[i]["DeliveryInstructions"], dtGetAll.Rows[i]["ProductionInsturction"], dtGetAll.Rows[i]["OtherInstructions"], dtGetAll.Rows[i]["EntryUserName"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["ModifyUserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ApprovalStatus"], dtGetAll.Rows[i]["ApprovedUserName"], dtGetAll.Rows[i]["PostDate"], dtGetAll.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistorygridSetting();
			}
			else
			{
				grdInputHistory.ClearStructure();
				grdOutPutHistory.ClearStructure();
				grdOverheadHistory.ClearStructure();
				grdPackingMaterialHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistorygridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["StartDate"].FormatString = "dd-MM-yyyy";
			DataGridHistory.RootTable.Columns["EndDate"].FormatString = "dd-MM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["DocDate"].Width = 80;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 70;
			DataGridHistory.RootTable.Columns["PlanType"].Width = 120;
			DataGridHistory.RootTable.Columns["JobOrderNo"].Width = 120;
			DataGridHistory.RootTable.Columns["ProductionType"].Width = 100;
			DataGridHistory.RootTable.Columns["StartDate"].Width = 80;
			DataGridHistory.RootTable.Columns["EndDate"].Width = 80;
			DataGridHistory.RootTable.Columns["PlanStatus"].Width = 100;
			DataGridHistory.RootTable.Columns["PackingInstruction"].Width = 150;
			DataGridHistory.RootTable.Columns["DeliveryInstruction"].Width = 150;
			DataGridHistory.RootTable.Columns["ProductionInstruction"].Width = 150;
			DataGridHistory.RootTable.Columns["OtherInstruction"].Width = 150;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = 110;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 80;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = 110;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = 80;
			DataGridHistory.RootTable.Columns["ApprovedStatus"].Width = 80;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = 110;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = 80;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = 80;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 2);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("SaveAs");
			DataGridHistory.RootTable.Columns["SaveAs"].Key = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DataGridHistory.RootTable.Columns["SaveAs"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Key = "Print";
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.PrintJobOrder630(CurrId);
				}
				if (e.Column.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			BtnGenerateInPutRates_Click(null, null);
			btnsave.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 401);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputHistory.ClearStructure();
			grdOutPutHistory.ClearStructure();
			grdOverheadHistory.ClearStructure();
			grdPackingMaterialHistory.ClearStructure();
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || Conversion.ToInt((object)r.Cells.Count) <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (CurrId <= 0)
			{
				return;
			}
			InvProductionJobOrder head = InvProductionJobOrder.GetByID(CurrId);
			if (head != null)
			{
				DataTable dtOutput = new DataTable();
				dtOutput.Columns.Add("EntryType", typeof(string));
				dtOutput.Columns.Add("WareHouseName", typeof(string));
				dtOutput.Columns.Add("JobLotDescription", typeof(string));
				dtOutput.Columns.Add("ItemName", typeof(string));
				dtOutput.Columns.Add("CropYear", typeof(string));
				dtOutput.Columns.Add("PackingTypeName", typeof(string));
				dtOutput.Columns.Add("QtyOuter", typeof(double));
				dtOutput.Columns.Add("OuterPackSizeName", typeof(string));
				dtOutput.Columns.Add("Weight", typeof(double));
				dtOutput.Columns.Add("ItemRate", typeof(double));
				dtOutput.Columns.Add("RateUom", typeof(string));
				dtOutput.Columns.Add("ItemAmount", typeof(double));
				dtOutput.Columns.Add("OHExpense", typeof(double));
				dtOutput.Columns.Add("PMExpense", typeof(double));
				dtOutput.Columns.Add("OHGeneralExpense", typeof(double));
				dtOutput.Columns.Add("PMGeneralExpense", typeof(double));
				dtOutput.Columns.Add("NetRate", typeof(double));
				dtOutput.Columns.Add("NetAmount", typeof(double));
				dtOutput.Columns.Add("Remarks", typeof(string));
				DataTable dtinput = new DataTable();
				dtinput.Columns.Add("DocDate", typeof(DateTime));
				dtinput.Columns.Add("DocNo", typeof(int));
				dtinput.Columns.Add("WareHouse", typeof(string));
				dtinput.Columns.Add("JobLot", typeof(string));
				dtinput.Columns.Add("ItemName", typeof(string));
				dtinput.Columns.Add("CropYear", typeof(string));
				dtinput.Columns.Add("PackType", typeof(string));
				dtinput.Columns.Add("Qty", typeof(double));
				dtinput.Columns.Add("PackSize", typeof(string));
				dtinput.Columns.Add("Weight", typeof(double));
				dtinput.Columns.Add("ItemRate", typeof(double));
				dtinput.Columns.Add("RateUom", typeof(string));
				dtinput.Columns.Add("ItemAmount", typeof(double));
				dtinput.Columns.Add("Remarks", typeof(string));
				DataTable dtOverHead = new DataTable();
				dtOverHead.Columns.Add("ChartOfAccount", typeof(string));
				dtOverHead.Columns.Add("Brand", typeof(string));
				dtOverHead.Columns.Add("BrandUom", typeof(string));
				dtOverHead.Columns.Add("ItemQty", typeof(double));
				dtOverHead.Columns.Add("ItemRate", typeof(double));
				dtOverHead.Columns.Add("Amount", typeof(double));
				dtOverHead.Columns.Add("Remarks", typeof(string));
				DataTable dtPackingMaterial = new DataTable();
				dtPackingMaterial.Columns.Add("Warehouse", typeof(string));
				dtPackingMaterial.Columns.Add("PackingItem", typeof(string));
				dtPackingMaterial.Columns.Add("Brand", typeof(string));
				dtPackingMaterial.Columns.Add("BrandUom", typeof(string));
				dtPackingMaterial.Columns.Add("ItemQty", typeof(double));
				dtPackingMaterial.Columns.Add("ItemRate", typeof(double));
				dtPackingMaterial.Columns.Add("Amount", typeof(double));
				dtPackingMaterial.Columns.Add("Remarks", typeof(string));
				for (int l = 0; l < head.invProductionJobOrderInput.Count; l++)
				{
					dtinput.Rows.Add(head.invProductionJobOrderInput[l].DocDate, head.invProductionJobOrderInput[l].DocNo, head.invProductionJobOrderInput[l].WareHouseName, head.invProductionJobOrderInput[l].JobLotCode, head.invProductionJobOrderInput[l].ItemName, head.invProductionJobOrderInput[l].BatchCrop, head.invProductionJobOrderInput[l].PackingType, head.invProductionJobOrderInput[l].Qty, head.invProductionJobOrderInput[l].UOMCode, head.invProductionJobOrderInput[l].NetWeight, head.invProductionJobOrderInput[l].ItemRate, head.invProductionJobOrderInput[l].RateUom, head.invProductionJobOrderInput[l].ItemAmount, head.invProductionJobOrderInput[l].RemarksInputDt);
				}
				for (int k = 0; k < head.invProductionJobOrderOutput.Count; k++)
				{
					dtOutput.Rows.Add(head.invProductionJobOrderOutput[k].EntryType, head.invProductionJobOrderOutput[k].WareHouseName, head.invProductionJobOrderOutput[k].JobLotDescription, head.invProductionJobOrderOutput[k].ItemName, head.invProductionJobOrderOutput[k].CropYear, head.invProductionJobOrderOutput[k].PackTypeDesc, head.invProductionJobOrderOutput[k].QtyOuter, head.invProductionJobOrderOutput[k].outerUOMCode, head.invProductionJobOrderOutput[k].NetWeight, head.invProductionJobOrderOutput[k].ItemRate, head.invProductionJobOrderOutput[k].RateUom, head.invProductionJobOrderOutput[k].ItemAmount, Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].OHExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].PMExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].OHGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].PMGeneralExpense), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].NetRate), Conversion.ToDouble((object)head.invProductionJobOrderOutput[k].NetAmount), head.invProductionJobOrderOutput[k].PackRemarks);
				}
				for (int j = 0; j < head.InvProductionJobOrderOverHeadsList.Count; j++)
				{
					dtOverHead.Rows.Add(head.InvProductionJobOrderOverHeadsList[j].AccountTitle, head.InvProductionJobOrderOverHeadsList[j].BrandName, head.InvProductionJobOrderOverHeadsList[j].BrandUom, head.InvProductionJobOrderOverHeadsList[j].ItemQty, head.InvProductionJobOrderOverHeadsList[j].ItemRate, head.InvProductionJobOrderOverHeadsList[j].Amount, head.InvProductionJobOrderOverHeadsList[j].ohRemarks);
				}
				for (int i = 0; i < head.InvProductionJobOrderPackingMaterialList.Count; i++)
				{
					dtPackingMaterial.Rows.Add(head.InvProductionJobOrderPackingMaterialList[i].WareHouseName, head.InvProductionJobOrderPackingMaterialList[i].ItemName, head.InvProductionJobOrderPackingMaterialList[i].BrandName, head.InvProductionJobOrderPackingMaterialList[i].BrandUom, head.InvProductionJobOrderPackingMaterialList[i].Qty, head.InvProductionJobOrderPackingMaterialList[i].Rate, head.InvProductionJobOrderPackingMaterialList[i].Amount, head.InvProductionJobOrderPackingMaterialList[i].pmRemarks);
				}
				grdInputHistory.DataSource = dtinput;
				grdInputHistory.RetrieveStructure();
				GridInputHistorySetting();
				grdOutPutHistory.DataSource = dtOutput;
				grdOutPutHistory.RetrieveStructure();
				GridOutPutHistorySetting();
				grdOverheadHistory.DataSource = dtOverHead;
				grdOverheadHistory.RetrieveStructure();
				GridOverHeadsHistorySetting();
				grdPackingMaterialHistory.DataSource = dtPackingMaterial;
				grdPackingMaterialHistory.RetrieveStructure();
				GridPackingMaterialHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridInputHistorySetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			grdInputHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdInputHistory.RootTable.Columns["DocDate"].Width = 80;
			grdInputHistory.RootTable.Columns["DocNo"].Width = 70;
			grdInputHistory.RootTable.Columns["WareHouse"].Width = 120;
			grdInputHistory.RootTable.Columns["JobLot"].Width = 120;
			grdInputHistory.RootTable.Columns["ItemName"].Width = 120;
			grdInputHistory.RootTable.Columns["CropYear"].Width = 90;
			grdInputHistory.RootTable.Columns["PackType"].Width = 100;
			grdInputHistory.RootTable.Columns["Qty"].Width = 70;
			grdInputHistory.RootTable.Columns["PackSize"].Width = 70;
			grdInputHistory.RootTable.Columns["Weight"].Width = 80;
			grdInputHistory.RootTable.Columns["ItemRate"].Width = 70;
			grdInputHistory.RootTable.Columns["RateUom"].Width = 70;
			grdInputHistory.RootTable.Columns["ItemAmount"].Width = 90;
			grdInputHistory.RootTable.Columns["Remarks"].Width = 120;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridOutPutHistorySetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			grdOutPutHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOutPutHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdOutPutHistory.RootTable.Columns["EntryType"].Width = 120;
			grdOutPutHistory.RootTable.Columns["WareHouseName"].Width = 120;
			grdOutPutHistory.RootTable.Columns["JobLotDescription"].Width = 120;
			grdOutPutHistory.RootTable.Columns["ItemName"].Width = 120;
			grdOutPutHistory.RootTable.Columns["CropYear"].Width = 100;
			grdOutPutHistory.RootTable.Columns["PackingTypeName"].Width = 100;
			grdOutPutHistory.RootTable.Columns["QtyOuter"].Width = 70;
			grdOutPutHistory.RootTable.Columns["OuterPackSizeName"].Width = 80;
			grdOutPutHistory.RootTable.Columns["Weight"].Width = 80;
			grdOutPutHistory.RootTable.Columns["ItemRate"].Width = 80;
			grdOutPutHistory.RootTable.Columns["RateUom"].Width = 70;
			grdOutPutHistory.RootTable.Columns["ItemAmount"].Width = 100;
			grdOutPutHistory.RootTable.Columns["OHExpense"].Width = 100;
			grdOutPutHistory.RootTable.Columns["PMExpense"].Width = 100;
			grdOutPutHistory.RootTable.Columns["OHGeneralExpense"].Width = 100;
			grdOutPutHistory.RootTable.Columns["PMGeneralExpense"].Width = 100;
			grdOutPutHistory.RootTable.Columns["NetRate"].Width = 80;
			grdOutPutHistory.RootTable.Columns["NetAmount"].Width = 100;
			grdOutPutHistory.RootTable.Columns["Remarks"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridOverHeadsHistorySetting()
	{
		try
		{
			CommonServices.GridWrappingAndColumnSettings(grdOverheadHistory, 2);
			grdOverheadHistory.RootTable.Columns["ChartOfAccount"].Width = 150;
			grdOverheadHistory.RootTable.Columns["Brand"].Width = 180;
			grdOverheadHistory.RootTable.Columns["BrandUom"].Width = 70;
			grdOverheadHistory.RootTable.Columns["ItemQty"].Width = 70;
			grdOverheadHistory.RootTable.Columns["ItemRate"].Width = 70;
			grdOverheadHistory.RootTable.Columns["Amount"].Width = 90;
			grdOverheadHistory.RootTable.Columns["Remarks"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPackingMaterialHistorySetting()
	{
		try
		{
			CommonServices.GridWrappingAndColumnSettings(grdPackingMaterialHistory, 2);
			grdPackingMaterialHistory.RootTable.Columns["Warehouse"].Width = 150;
			grdPackingMaterialHistory.RootTable.Columns["PackingItem"].Width = 150;
			grdPackingMaterialHistory.RootTable.Columns["Brand"].Width = 180;
			grdPackingMaterialHistory.RootTable.Columns["BrandUom"].Width = 70;
			grdPackingMaterialHistory.RootTable.Columns["ItemQty"].Width = 70;
			grdPackingMaterialHistory.RootTable.Columns["ItemRate"].Width = 70;
			grdPackingMaterialHistory.RootTable.Columns["Amount"].Width = 90;
			grdPackingMaterialHistory.RootTable.Columns["Remarks"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(cmbwarehouseinput, dt, "Id", "Description", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWipWarehouse.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(cmbWareHouseOutPut, dt, "Id", "Description", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWipWarehouse.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbWarehousePackingMaterial, dt, "Id", "Description", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWipWarehouse.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbWipWarehouse, dt, "Id", "Description", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWipWarehouse.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			int ID = Conversion.ToInt(cmbcropyearinput.Value);
			int ID2 = Conversion.ToInt(cmbCropYearOutput.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbcropyearinput, "Id", "CropYear", "Crop Year", true);
			DDL.BindDDLNew(dt, cmbCropYearOutput, "Id", "CropYear", "Crop Year", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbcropyearinput.Value = ID;
				}
				else
				{
					((Control)(object)cmbcropyearinput).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbCropYearOutput.Value = ID2;
				}
				else
				{
					((Control)(object)cmbCropYearOutput).Text = string.Empty;
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.ReadAllItems(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItem, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				DDL.BindDDLNew(dt, cmbItemInput, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItemInput).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotBind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbjoblotinput.Value);
			int ID2 = Conversion.ToInt(cmbJobLotOutput.Value);
			DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbjoblotinput, "Id", "JobLotDescription", "Job Lot From", true);
			DDL.BindDDLNew(dt, cmbJobLotOutput, "Id", "JobLotDescription", "Job Lot To", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbjoblotinput.Value = ID;
				}
				else
				{
					((Control)(object)cmbjoblotinput).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbJobLotOutput.Value = ID2;
				}
				else
				{
					((Control)(object)cmbJobLotOutput).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillBySaleOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItem, "OrderItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillByExportSaleContractId()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbItem).Text = string.Empty;
			DataTable dtItem = ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemName");
			if (dtItem.Rows.Count > 0)
			{
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["ExImItemId"], dtItem.Rows[i]["ItemName"]);
				}
				DDL.BindDDLNew(dt, cmbItem, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillBySaleOrderInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemInput).DataSource = null;
				((Control)(object)cmbItemInput).Text = string.Empty;
				DDL.BindDDLNew(dtItem, cmbItemInput, "OrderItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillByExportSaleContractIdInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtItem = ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				((UltraGridBase)cmbItemInput).DataSource = null;
				((Control)(object)cmbItemInput).Text = string.Empty;
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["ExImItemId"], dtItem.Rows[i]["ItemName"]);
				}
				DDL.BindDDL(dt, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItemInput).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItemInput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingTypeFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = Conversion.ToInt(cmbPackingType.Value);
			int ID2 = Conversion.ToInt(cmbPackTypeInput.Value);
			new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			};
			dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtPackingType, cmbPackingType, "Id", "PackTypeDesc", "PackType", false);
			DDL.BindDDLNew(dtPackingType, cmbPackTypeInput, "Id", "PackTypeDesc", "PackType", false);
			((UltraGridBase)cmbPackingType).Rows[0].Activate();
			((UltraGridBase)cmbPackTypeInput).Rows[0].Activate();
			if (ID > 0)
			{
				if (dtPackingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbPackingType.Value = ID;
				}
				else
				{
					((Control)(object)cmbPackingType).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dtPackingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbPackTypeInput.Value = ID2;
				}
				else
				{
					((Control)(object)cmbPackTypeInput).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlanTypeBind()
	{
		try
		{
			DataTable dtpt = new DataTable();
			dtpt.Columns.Add("Id");
			dtpt.Columns.Add("Type");
			dtpt.Rows.Add(1, "Local");
			dtpt.Rows.Add(2, "Export");
			dtpt.Rows.Add(3, "Party");
			if (dtpt.Rows.Count > 0)
			{
				DDL.BindDDL(dtpt, cmbPlanType, "Id", "Type", "Plan Type ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionTypeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtprot = ProductionType.GetAll(new ProductionType());
			if (dtprot.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtprot, cmbProductionType, "Id", "ProductionTypeDescription", "Production Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusBind()
	{
		try
		{
			DataTable dtstat = new DataTable();
			dtstat.Columns.Add("Id");
			dtstat.Columns.Add("Status");
			dtstat.Rows.Add(1, "In Process");
			dtstat.Rows.Add(2, "Complete");
			dtstat.Rows.Add(3, "Cancel");
			DDL.BindDDL(dtstat, cmbStatus, "Id", "Status", "Status", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesAccountFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = Conversion.ToInt(CmbLabourItemInPut.Value);
			int ID2 = Conversion.ToInt(CmbLabourItemOutPut.Value);
			new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			};
			dtLabourAc = CommonServices.GetWagesAccount(null, 0, 1);
			if (dtLabourAc.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtLabourAc, CmbLabourItemInPut, "Id", "WagesAccountName", "Labour Item", false);
			DDL.BindDDLNew(dtLabourAc, CmbLabourItemOutPut, "Id", "WagesAccountName", "Labour Item", false);
			if (ID > 0)
			{
				if (dtLabourAc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					CmbLabourItemInPut.Value = ID;
				}
				else
				{
					((Control)(object)CmbLabourItemInPut).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dtLabourAc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					CmbLabourItemOutPut.Value = ID2;
				}
				else
				{
					((Control)(object)CmbLabourItemOutPut).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialItemBind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = Conversion.ToInt(CmbPackingMaterial.Value);
			int ID2 = Conversion.ToInt(cmbItemPackingMaterial.Value);
			new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			};
			DataTable dt = CommonServices.GetItemByItemTypeId("14");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbPackingMaterial, "Id", "ItemName", "Packing Item", false);
			DDL.BindDDLNew(dt, cmbItemPackingMaterial, "Id", "ItemName", "Packing Item", false);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					CmbPackingMaterial.Value = ID;
				}
				else
				{
					((Control)(object)CmbPackingMaterial).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbItemPackingMaterial.Value = ID2;
				}
				else
				{
					((Control)(object)cmbItemPackingMaterial).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetWagesRate(DateTime DocDate, double PackUom, int WagesAccoutId, int ContractorId = 0)
	{
		try
		{
			double WagesRate = 0.0;
			GetWagesSchedule d = CommonServices.GetWagesRate(DocDate, PackUom, WagesAccoutId, ContractorId);
			if (d != null)
			{
				WagesRate = d.WagesRate;
			}
			return WagesRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void frmProductionJobOrder_KeyDown(object sender, KeyEventArgs e)
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
					txtPlanDate.Focus();
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && Print.Visible && Print.Enabled)
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					btnLoadInvoices_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtPlanDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnSaveAs.Visible && btnSaveAs.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdInputDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtPlanDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdPlannedOutput).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdInputDetail).Focus();
					}
					else if (((Control)(object)grdInputDetail).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdOverHead).Focus();
					}
					else if (((Control)(object)grdOverHead).Focused)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPackingMaterial).Focus();
					}
					else if (((Control)(object)grdPackingMaterial).Focused)
					{
						tabControl2.SelectedIndex = 4;
						((Control)(object)grdPlantdetail).Focus();
					}
					else if (((Control)(object)grdPlantdetail).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdPlannedOutput).Focus();
					}
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshowHistory_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					tabControl3.SelectedIndex = 0;
					((Control)(object)grdOutPutHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdOutPutHistory).Focused)
					{
						tabControl3.SelectedIndex = 1;
						((Control)(object)grdInputHistory).Focus();
					}
					else if (((Control)(object)grdInputHistory).Focused)
					{
						tabControl3.SelectedIndex = 2;
						((Control)(object)grdOverheadHistory).Focus();
					}
					else if (((Control)(object)grdOverheadHistory).Focused)
					{
						tabControl3.SelectedIndex = 3;
						((Control)(object)grdPackingMaterial).Focus();
					}
					else if (((Control)(object)grdPackingMaterial).Focused)
					{
						tabControl3.SelectedIndex = 0;
						((Control)(object)grdOutPutHistory).Focus();
					}
				}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+L", "For Load Input");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInputDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdInputDetail.CurrentColumn.Key == "Edit")
				{
					grdInputDetail_DoubleClick(null, null);
				}
				if (grdInputDetail.CurrentColumn.Key == "Delete")
				{
					DeleteInputdetailRow(r);
				}
				grdInputDetail.UpdateData();
				btnGenerate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteInputdetailRow(r);
				grdInputDetail.UpdateData();
				btnGenerate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPutHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOutPutHistory.CurrentColumn.Key == "Delete")
				{
					OutputDetailRowDelete(r);
					grdOutPutHistory.UpdateData();
					btnGenerate_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				OutputDetailRowDelete(r);
				grdOutPutHistory.UpdateData();
				btnGenerate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdOverHead.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdOverHead.CurrentColumn.Key == "Delete")
				{
					OverHeadDetailRowDelete(r);
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					OverHeadDetailRowDelete(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackingMaterial.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPackingMaterial.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdPackingMaterial.CurrentColumn.Key == "Delete")
				{
					PackingMaterialDetailrowDelete(r);
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					PackingMaterialDetailrowDelete(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.PrintJobOrder630(CurrId);
				}
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					ReadById(CurrId);
					BtnGenerateInPutRates_Click(null, null);
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					btnSaveAs.Visible = true;
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlantdetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPlantdetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPlantdetail.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdPlantdetail.CurrentColumn.Key == "Delete")
				{
					PlantDetailRowDelete(r);
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					PlantDetailRowDelete(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PrintJobOrder630(RECID);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnProductionPlant_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlant(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grdInputDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						item.BeginEdit();
						double BalQty = Conversion.ToDouble(item.Cells["BalanceQty"].Value);
						double BalWeight = Conversion.ToDouble(item.Cells["BalanceWeight"].Value);
						item.Cells["BalanceWeight"].Value = Conversion.ToDouble((object)BalWeight) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]);
						item.Cells["BalanceQty"].Value = Conversion.ToDouble((object)BalQty) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]);
						item.EndEdit();
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					dtInputDetail.Rows.Add(0, Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]), Conversion.ToDateTime(dtpurchaseinvoice.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DocNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["WareHouseId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["WareHouse"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["JobLotId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["JobLotCode"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["ItemName"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["CropYearId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["CropYear"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["PackingTypeId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackTypeCode"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["ItemUOMId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackUom"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemUOM"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemRate"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RateUOMId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RateUOM"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Equivalent"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["ItemAmount"]), Conversion.ToString((object)""));
				}
			}
			grdInputDetail.DataSource = dtInputDetail;
			grdInputDetail.RetrieveStructure();
			InputDetailGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 0)
			{
				((UltraGridBase)cmbentrytype).Rows[1].Activate();
			}
			else if (tabControl2.SelectedIndex != 1)
			{
				if (tabControl2.SelectedIndex == 2)
				{
					BrandItemBindOnOhandPM(GetBrandItems());
					ChartOfAccountOverHead();
				}
				else if (tabControl2.SelectedIndex == 3)
				{
					BrandItemBindOnOhandPM(GetBrandItems());
				}
				else if (tabControl2.SelectedIndex == 4)
				{
					PlantFeader();
					cmbPlantFeader.Focus();
				}
			}
			BrandItemBindonLabStandard(GetBrandItems(onlyFinished: true));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			CostingCalculations();
			InsertPMInOutPutGrid();
			InsertOhInOutPutGrid();
			CalculateNEtRateAndamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CostingCalculations()
	{
		try
		{
			double RawQty = 0.0;
			double RawWeight = 0.0;
			double RawAmount = 0.0;
			double BPQty = 0.0;
			double BPWeight = 0.0;
			double BPAmount = 0.0;
			double FGQty = 0.0;
			double FGWeight = 0.0;
			double FGAmount = 0.0;
			double OHAmount = 0.0;
			double PMAmount = 0.0;
			double AvgRate = 0.0;
			double AvgRateWithoutExp = 0.0;
			if (grdInputDetail.GetRows().Length != 0)
			{
				RawQty = Conversion.ToDouble(grdInputDetail.GetTotal(grdInputDetail.RootTable.Columns["Qty"], (AggregateFunction)2));
				RawWeight = Conversion.ToDouble(grdInputDetail.GetTotal(grdInputDetail.RootTable.Columns["Weight"], (AggregateFunction)2));
				RawAmount = Conversion.ToDouble(grdInputDetail.GetTotal(grdInputDetail.RootTable.Columns["ItemAmount"], (AggregateFunction)2));
			}
			if (grdOverHead.GetRows().Length != 0)
			{
				OHAmount = Conversion.ToDouble(grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdPackingMaterial.GetRows().Length != 0)
			{
				PMAmount = Conversion.ToDouble(grdPackingMaterial.GetTotal(grdPackingMaterial.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdPlannedOutput.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item2 in rows)
				{
					if (Conversion.ToInt(item2.Cells["EntryTypeId"].Value) == 1)
					{
						BPQty += Conversion.ToDouble(item2.Cells["QtyOuter"].Value);
						BPWeight += Conversion.ToDouble(item2.Cells["Weight"].Value);
						BPAmount += Conversion.ToDouble(item2.Cells["ItemAmount"].Value);
					}
					else
					{
						FGQty += Conversion.ToDouble(item2.Cells["QtyOuter"].Value);
						FGWeight += Conversion.ToDouble(item2.Cells["Weight"].Value);
					}
				}
			}
			if (FGWeight > 0.0)
			{
				FGAmount = RawAmount + OHAmount + PMAmount - BPAmount;
				AvgRateWithoutExp = (RawAmount - BPAmount) / FGWeight;
				AvgRate = (RawAmount + OHAmount + PMAmount - BPAmount) / FGWeight;
			}
			if (grdPlannedOutput.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["EntryTypeId"].Value) == 2)
					{
						item.BeginEdit();
						double RateUom = 0.0;
						double Weight = 0.0;
						RateUom = Conversion.ToDouble(item.Cells["EquivalentRate"].Value);
						Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
						item.Cells["ItemRate"].Value = RateUom * AvgRate;
						double rate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
						if (Weight > 0.0 && RateUom > 0.0 && rate > 0.0)
						{
							item.Cells["ItemAmount"].Value = Weight / RateUom * rate;
						}
						else
						{
							item.Cells["ItemAmount"].Value = 0;
						}
						item.EndEdit();
					}
				}
			}
			txtInPutTotalQty.Text = RawQty.ToString("#,#");
			txtInPutTotalWeight.Text = RawWeight.ToString("#,#");
			txtTotalAmountInPut.Text = RawAmount.ToString("#,#");
			txtTotalQtyBP.Text = BPQty.ToString("#,#");
			txtTotalWeightBP.Text = BPWeight.ToString("#,#");
			txtTotalAmountBP.Text = BPAmount.ToString("#,#");
			txtTotalQtyFG.Text = FGQty.ToString("#,#");
			txtTotalWeightFG.Text = FGWeight.ToString("#,#");
			txtTotalAmountFG.Text = FGAmount.ToString("#,#");
			txtFGAvgRate.Text = AvgRate.ToString("#,#.###");
			txtFGRateWithoutExp.Text = AvgRateWithoutExp.ToString("#,#.###");
			txtPackingMaterialAmount.Text = PMAmount.ToString("#,#");
			txtOverHeadsAmount.Text = OHAmount.ToString("#,#");
			grdPlannedOutput.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetBrandQtyByBrandUomAndBrnadUom(int BrandId, int BrandUomId = 0)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			double Qty = 0.0;
			if (grdPlannedOutput.CurrentRow != null && (int)grdPlannedOutput.CurrentRow.RowType == 0 && grdPlannedOutput.CurrentRow.Cells.Count > 0)
			{
				GridEXRow[] rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (BrandId == Conversion.ToInt(item.Cells["ItemId"].Value) && (BrandUomId == 0 || BrandUomId == Conversion.ToInt(item.Cells["OuterPackSizeId"].Value)))
					{
						Qty += (double)Conversion.ToInt(item.Cells["QtyOuter"].Value);
					}
				}
			}
			return Qty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return 0.0;
		}
	}

	private void InsertPMInOutPutGrid()
	{
		try
		{
			double sumOfAmountForZeroBrandId = 0.0;
			Dictionary<Tuple<int, int>, double> amountByBrand = (from row in grdPackingMaterial.GetRows()
				select new
				{
					BrandId = Conversion.ToInt(row.Cells["BrandId"].Value),
					BrandUomId = Conversion.ToInt(row.Cells["BrandUomId"].Value),
					Amount = Conversion.ToDouble(row.Cells["Amount"].Value)
				} into row
				group row by Tuple.Create(row.BrandId, row.BrandUomId)).ToDictionary(group => group.Key, group => group.Sum(row => row.Amount));
			GridEXRow[] rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int brandId2 = Conversion.ToInt(obj.Cells["BrandId"].Value);
				double amount2 = Conversion.ToDouble(obj.Cells["Amount"].Value);
				if (brandId2 == 0)
				{
					sumOfAmountForZeroBrandId += amount2;
				}
			}
			rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				obj2.BeginEdit();
				obj2.Cells["PMExpense"].Value = 0.ToString();
				obj2.EndEdit();
			}
			foreach (KeyValuePair<Tuple<int, int>, double> kvp in amountByBrand)
			{
				int brandId = kvp.Key.Item1;
				int brandUomId = kvp.Key.Item2;
				double amount = kvp.Value;
				rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == brandId && Conversion.ToInt(r.Cells["OuterPackSizeId"].Value) == brandUomId)
					{
						amount += Conversion.ToDouble(r.Cells["PMExpense"].Value);
						r.Cells["PMExpense"].Value = amount;
					}
					r.EndEdit();
				}
			}
			double totalOutputWeight = 0.0;
			totalOutputWeight = Conversion.ToDouble(dtPlannedOutput.Compute("Sum(Weight)", "EntryType = 'AutoRate'"));
			rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				if (Conversion.ToString(item.Cells["EntryType"].Value) == "AutoRate")
				{
					double GeneralPM = Conversion.ToDouble(item.Cells["Weight"].Value) / totalOutputWeight * 100.0 / 100.0 * sumOfAmountForZeroBrandId;
					item.Cells["PMGeneralExpense"].Value = GeneralPM;
				}
				else
				{
					item.Cells["PMGeneralExpense"].Value = 0.ToString();
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertOhInOutPutGrid()
	{
		try
		{
			double sumOfAmountForZeroBrandId = 0.0;
			Dictionary<Tuple<int, int>, double> amountByBrand = (from row in grdOverHead.GetRows()
				select new
				{
					BrandId = Conversion.ToInt(row.Cells["BrandId"].Value),
					BrandUomId = Conversion.ToInt(row.Cells["BrandUomId"].Value),
					Amount = Conversion.ToDouble(row.Cells["Amount"].Value)
				} into row
				group row by Tuple.Create(row.BrandId, row.BrandUomId)).ToDictionary(group => group.Key, group => group.Sum(row => row.Amount));
			GridEXRow[] rows = grdOverHead.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int brandId2 = Conversion.ToInt(obj.Cells["BrandId"].Value);
				double amount2 = Conversion.ToDouble(obj.Cells["Amount"].Value);
				if (brandId2 == 0)
				{
					sumOfAmountForZeroBrandId += amount2;
				}
			}
			rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				obj2.BeginEdit();
				obj2.Cells["OHExpense"].Value = 0.ToString();
				obj2.EndEdit();
			}
			foreach (KeyValuePair<Tuple<int, int>, double> kvp in amountByBrand)
			{
				int brandId = kvp.Key.Item1;
				int brandUomId = kvp.Key.Item2;
				double amount = kvp.Value;
				rows = grdPlannedOutput.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == brandId && Conversion.ToInt(r.Cells["OuterPackSizeId"].Value) == brandUomId)
					{
						amount += Conversion.ToDouble(r.Cells["OHExpense"].Value);
						r.Cells["OHExpense"].Value = amount;
					}
					r.EndEdit();
				}
			}
			double totalOutputWeight = 0.0;
			totalOutputWeight = Conversion.ToDouble(dtPlannedOutput.Compute("Sum(Weight)", "EntryType = 'AutoRate'"));
			rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				if (Conversion.ToString(item.Cells["EntryType"].Value) == "AutoRate")
				{
					double GeneralOH = Conversion.ToDouble(item.Cells["Weight"].Value) / totalOutputWeight * 100.0 / 100.0 * sumOfAmountForZeroBrandId;
					item.Cells["OHGeneralExpense"].Value = GeneralOH;
				}
				else
				{
					item.Cells["OHGeneralExpense"].Value = 0.ToString();
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNEtRateAndamount()
	{
		try
		{
			double NetRate = 0.0;
			double NetAmount = 0.0;
			GridEXRow[] rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				if (!JobOrderCreatewithoutRates)
				{
					Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
					double ItemWeight = Conversion.ToDouble(item.Cells["Weight"].Value);
					double RateUom = Conversion.ToDouble(item.Cells["EquivalentRate"].Value);
					double OHExpenseItem = Conversion.ToDouble(item.Cells["OHExpense"].Value);
					double PMExpenseItem = Conversion.ToDouble(item.Cells["PMExpense"].Value);
					double OHExpenseItemGeneral = Conversion.ToDouble(item.Cells["OHGeneralExpense"].Value);
					double PMExpenseItemGeneral = Conversion.ToDouble(item.Cells["PMGeneralExpense"].Value);
					NetAmount = ItemAmount + OHExpenseItem + OHExpenseItemGeneral + PMExpenseItem + PMExpenseItemGeneral;
					NetRate = NetAmount / ItemWeight * RateUom;
					item.Cells["NetAmount"].Value = NetAmount;
					item.Cells["NetRate"].Value = NetRate;
				}
				else
				{
					item.Cells["NetAmount"].Value = 0;
					item.Cells["NetRate"].Value = 0;
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPlantDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationPlantDetail())
			{
				return;
			}
			if (grdPlantdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPlantdetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(cmbPlantFeader.Value) == Conversion.ToInt(item.Cells["PlantId"].Value))
					{
						throw new Exception("Same Plant Already Exist In Detail");
					}
				}
			}
			dtPlantdetail.Rows.Add(0, cmbPlantFeader.Value, ((Control)(object)cmbPlantFeader).Text.Trim(), Conversion.ToDouble((object)txtAssumedPlantCapacity.Text.Trim()), txtRemarksPlantDetail.Text.Trim());
			grdPlantdetail.DataSource = dtPlantdetail;
			grdPlantdetail.RetrieveStructure();
			grdPlantdetailSetting();
			((Control)(object)cmbPlantFeader).Text = string.Empty;
			txtAssumedPlantCapacity.Text = string.Empty;
			cmbPlantFeader.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidationPlantDetail()
	{
		if (((UltraGridBase)cmbPlantFeader).ActiveRow == null || ((Control)(object)cmbPlantFeader).Text.Trim() == "")
		{
			MessageBox.Show("Plant Feeder Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPlantFeader.Focus();
			return false;
		}
		return true;
	}

	private void grdPlantdetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = false;
			btnUpdatePlantDetail.Visible = true;
			btnCanclePlantdetail.Visible = true;
			GridEXRow item = grdPlantdetail.CurrentRow;
			if (item != null)
			{
				updatePlantDetailIndex = item.RowIndex;
				cmbPlantFeader.Value = Conversion.ToInt(item.Cells["PlantId"].Value);
				txtAssumedPlantCapacity.Text = Conversion.ToString(item.Cells["AssumedPlantCapacity"].Value);
				txtRemarksPlantDetail.Text = Conversion.ToString(item.Cells["RemarksDetail"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdatePlantDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationPlantDetail())
			{
				return;
			}
			if (grdPlantdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPlantdetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (updatePlantDetailIndex != item.RowIndex && Conversion.ToInt(cmbPlantFeader.Value) == Conversion.ToInt(item.Cells["PlantId"].Value))
					{
						throw new Exception("Same Plant Already Exist In Detail");
					}
				}
			}
			dtPlantdetail.Rows[updatePlantDetailIndex]["PlantId"] = cmbPlantFeader.Value;
			dtPlantdetail.Rows[updatePlantDetailIndex]["PlantName"] = ((Control)(object)cmbPlantFeader).Text.Trim();
			dtPlantdetail.Rows[updatePlantDetailIndex]["AssumedPlantCapacity"] = Conversion.ToDouble((object)txtAssumedPlantCapacity.Text.Trim());
			dtPlantdetail.Rows[updatePlantDetailIndex]["RemarksDetail"] = txtRemarksPlantDetail.Text.Trim();
			btnAddPlantDetail.Visible = true;
			btnUpdatePlantDetail.Visible = false;
			btnCanclePlantdetail.Visible = false;
			((Control)(object)cmbPlantFeader).Text = string.Empty;
			txtAssumedPlantCapacity.Text = string.Empty;
			cmbPlantFeader.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCanclePlantdetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddPlantDetail.Visible = true;
			btnUpdatePlantDetail.Visible = false;
			btnCanclePlantdetail.Visible = false;
			((Control)(object)cmbPlantFeader).Text = string.Empty;
			cmbPlantFeader.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPlantdetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grdPlantdetail.CurrentRow;
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				return;
			}
			if (!InvProductionJobOrder.CheckPlantExistinProductionagaintJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = RECID,
				PlantId = Conversion.ToInt(item.Cells["PlantId"].Value)
			}))
			{
				dtPlantdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grdPlantdetail.UpdateData();
				return;
			}
			throw new Exception("You cannot be delete " + Conversion.ToString(item.Cells["PlantName"].Value) + " because " + Conversion.ToString(item.Cells["PlantName"].Value) + " Has been referred to production");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PlantDetailRowDelete(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexoutput == -1)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					r.Delete();
					return;
				}
				if (!InvProductionJobOrder.CheckPlantExistinProductionagaintJobOrder(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					JobOrderId = RECID,
					PlantId = Conversion.ToInt(r.Cells["PlantId"].Value)
				}))
				{
					dtPlantdetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdPlantdetail.UpdateData();
					return;
				}
				throw new Exception("You cannot be delete " + Conversion.ToString(r.Cells["PlantName"].Value) + " because " + Conversion.ToString(r.Cells["PlantName"].Value) + " Has been referred to production");
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPlantdetailSetting()
	{
		try
		{
			grdPlantdetail.RootTable.Columns["Id"].Visible = false;
			grdPlantdetail.RootTable.Columns["PlantId"].Visible = false;
			grdPlantdetail.RootTable.Columns["PlantName"].Width = 200;
			grdPlantdetail.RootTable.Columns["AssumedPlantCapacity"].Width = 100;
			grdPlantdetail.RootTable.Columns["RemarksDetail"].Width = 300;
			grdPlantdetail.RootTable.Columns.Add("Delete");
			grdPlantdetail.RootTable.Columns["Delete"].Key = "Delete";
			grdPlantdetail.RootTable.Columns["Delete"].Caption = "X";
			grdPlantdetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPlantdetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPlantdetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdPlantdetail.RootTable.Columns["Delete"].Width = 30;
			grdPlantdetail.RootTable.Columns["Delete"].Position = 0;
			grdPlantdetail.FrozenColumns = 1;
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPlantdetail).Name);
			if (fs != null)
			{
				grdPlantdetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtweightinput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			InPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			OutPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUomInPut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			InPutAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcropyearinput_Leave(object sender, EventArgs e)
	{
		try
		{
			InPutRateBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbwarehouseinput_Leave(object sender, EventArgs e)
	{
	}

	private void cmbjoblotinput_Leave(object sender, EventArgs e)
	{
		try
		{
			InPutRateBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProductionTypeDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtPartiesFromStandardDbCall()
	{
		try
		{
			dtPartiesFromStandard.Clear();
			dtPartiesFromStandard = LabAnalysisScheduleItemAndPartyWise.LabAnalysisScheduleItemAndPartyWise_GetSupplierCustomer(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromStandard()
	{
		try
		{
			dtSupplier.Clear();
			if (dtPartiesFromStandard == null || dtPartiesFromStandard.Rows.Count == 0)
			{
				return;
			}
			bool useNickName = RadNickName.Checked;
			Dictionary<int, DataRow> customerLookup = (from r in dtPartiesFromStandard.AsEnumerable()
				where r["Id"] != DBNull.Value
				select r).ToDictionary((DataRow r) => Convert.ToInt32(r["Id"]));
			dtSupplier.BeginLoadData();
			foreach (DataRow item in dtPartiesFromStandard.Rows)
			{
				int id = ((item["Id"] != DBNull.Value) ? Convert.ToInt32(item["Id"]) : 0);
				string name = ((!useNickName) ? (item["CompanyName"]?.ToString() ?? string.Empty) : (item["NickName"]?.ToString() ?? string.Empty));
				bool num = item["IsSubSupCust"] != DBNull.Value && Convert.ToBoolean(item["IsSubSupCust"]);
				int parentId = ((item["ParentsSupCustId"] != DBNull.Value) ? Convert.ToInt32(item["ParentsSupCustId"]) : 0);
				if (num && parentId > 0 && customerLookup.TryGetValue(parentId, out var parent))
				{
					name = ((!useNickName) ? (parent["CompanyName"]?.ToString() ?? string.Empty) : (parent["NickName"]?.ToString() ?? string.Empty)) + " / " + name;
				}
				dtSupplier.Rows.Add(id, name, item["PartyCode"]?.ToString(), (item["GlAccountId"] != DBNull.Value) ? Convert.ToInt32(item["GlAccountId"]) : 0, item["CityName"]?.ToString(), item["MobilePersonal"]?.ToString());
			}
			dtSupplier.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			_ = radPartyCode.Checked;
			_ = radPartyCode.Checked;
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerName, dtSupplier, "Id", "CompanyName", "Party Name", AllColumns: true, Conversion.ToInt(CmbBuyerName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbBuyerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBuyerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnDefineLabStandards_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("frmLabAnalysisScheduleItemAndPartyWise", delegate
			{
				frmLabAnalysisScheduleItemAndPartyWise frmLabAnalysisScheduleItemAndPartyWise = new frmLabAnalysisScheduleItemAndPartyWise(UserAccount);
				frmLabAnalysisScheduleItemAndPartyWise.Show();
				frmLabAnalysisScheduleItemAndPartyWise.BringToFront();
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadBusinessName_CheckedChanged(object sender, EventArgs e)
	{
		SupplierDtFillFromStandard();
		BindSupplierName();
	}

	private void CmbBrandNameLabStandard_Leave(object sender, EventArgs e)
	{
		AnalysisGroupBind(AnalysisGroupDbCall(Conversion.ToInt(CmbBrandNameLabStandard.Value), Conversion.ToInt(CmbBuyerName.Value)));
	}

	private void CmbBuyerName_Leave(object sender, EventArgs e)
	{
		AnalysisGroupBind(AnalysisGroupDbCall(Conversion.ToInt(CmbBrandNameLabStandard.Value), Conversion.ToInt(CmbBuyerName.Value)));
	}

	private DataTable AnalysisGroupDbCall(int ItemId, int SupplierCustomerId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			return LabAnalysisScheduleItemAndPartyWise.ComboFill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BrandId = ItemId,
				SupplierCustomerId = SupplierCustomerId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AnalysisGroupBind(DataTable dt)
	{
		try
		{
			int previousVal = Conversion.ToInt(CmbAnalysisGroup.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisGroup, dt, "MainId", "ScheduleName", "Standard Name", AllColumns: false, previousVal, insertDefaultRow: false, null, null, ActivateRow: false);
			int NewVal = Conversion.ToInt(CmbAnalysisGroup.Value);
			if (NewVal > 0 && previousVal > 0 && NewVal != previousVal)
			{
				CmbAnalysisGroup_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisGroup_Leave(object sender, EventArgs e)
	{
		GridFillByAnalysisGroup(Conversion.ToInt(CmbAnalysisGroup.Value));
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtLabDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdLabStandards.DataSource = dtLabDetail;
				grdLabStandards.RetrieveStructure();
				grdLabStandardsSetting(grdLabStandards);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillByAnalysisGroup(int groupId)
	{
		try
		{
			if (groupId <= 0)
			{
				dtLabDetail.Clear();
				BindGrids();
				return;
			}
			LabAnalysisScheduleItemAndPartyWise obj = LabAnalysisScheduleItemAndPartyWise.ReadByMainId(groupId);
			List<LabAnalysisScheduleItemAndPartyWise> list = ((obj != null) ? obj.LabAnalysisScheduleItemAndPartyWiseList.Where((LabAnalysisScheduleItemAndPartyWise r) => r.Active).ToList() : null);
			if (list == null)
			{
				MessageBox.Show("No data found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			HashSet<string> sourceKeys = new HashSet<string>(list.Select((LabAnalysisScheduleItemAndPartyWise x) => $"{x.LabAnalysisGroupId}_{x.LabAnalysisItemId}"));
			for (int i = dtLabDetail.Rows.Count - 1; i >= 0; i--)
			{
				DataRow row2 = dtLabDetail.Rows[i];
				string key2 = string.Format("{0}_{1}", row2["AnalysisGroupId"], row2["AnalysisItemId"]);
				if (!sourceKeys.Contains(key2))
				{
					dtLabDetail.Rows.RemoveAt(i);
				}
			}
			HashSet<string> existingKeys = new HashSet<string>(from r in dtLabDetail.AsEnumerable()
				select string.Format("{0}_{1}", r["AnalysisGroupId"], r["AnalysisItemId"]));
			dtLabDetail.BeginLoadData();
			foreach (LabAnalysisScheduleItemAndPartyWise row in list)
			{
				string key = $"{row.LabAnalysisGroupId}_{row.LabAnalysisItemId}";
				if (!existingKeys.Contains(key))
				{
					dtLabDetail.Rows.Add(0, row.LabAnalysisGroupId, row.LabAnalysisGroupDescription ?? string.Empty, row.LabAnalysisItemId, row.LabAnalysisItemName ?? string.Empty, row.MinValue, row.MaxValue, row.Active);
				}
			}
			dtLabDetail.EndLoadData();
			BindGrids();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdLabStandardsSetting(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "AnalysisGroupId", "AnalysisItemId", "Active", "MainId" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == ((Control)(object)grdLabStandards).Name)
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"MinValue",
						(EditType)1
					},
					{
						"MaxValue",
						(EditType)1
					},
					{
						"Active",
						(EditType)8
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["AnalysisGroup"].Width = 180;
			gridEX.RootTable.Columns["AnalysisItem"].Width = 180;
			gridEX.RootTable.Columns["MinValue"].Width = 90;
			gridEX.RootTable.Columns["MaxValue"].Width = 90;
			gridEX.RootTable.Columns["Active"].Width = 70;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdLabStandards_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdLabStandards.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdLabStandards_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdLabStandards.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				grdLabStandards.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtLabDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			else
			{
				dtLabDetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAssumedPlantCapacity_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Expected O, but got Unknown
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Expected O, but got Unknown
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da7: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbd: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd3: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dff: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e15: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e62: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e78: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Expected O, but got Unknown
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8e: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e99: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea4: Expected O, but got Unknown
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eaf: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec5: Expected O, but got Unknown
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed0: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee6: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef1: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1d: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f33: Expected O, but got Unknown
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3e: Expected O, but got Unknown
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5f: Expected O, but got Unknown
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Expected O, but got Unknown
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f80: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f96: Expected O, but got Unknown
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa1: Expected O, but got Unknown
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb7: Expected O, but got Unknown
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Expected O, but got Unknown
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcd: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd8: Expected O, but got Unknown
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe3: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Expected O, but got Unknown
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff9: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_100f: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101a: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Expected O, but got Unknown
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Expected O, but got Unknown
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_105c: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1067: Expected O, but got Unknown
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107d: Expected O, but got Unknown
		//IL_107f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1088: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109e: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a9: Expected O, but got Unknown
		//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b4: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Expected O, but got Unknown
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d5: Expected O, but got Unknown
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e0: Expected O, but got Unknown
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Expected O, but got Unknown
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f6: Expected O, but got Unknown
		//IL_10f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1101: Expected O, but got Unknown
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Expected O, but got Unknown
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Expected O, but got Unknown
		//IL_1119: Unknown result type (might be due to invalid IL or missing references)
		//IL_1122: Expected O, but got Unknown
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_112d: Expected O, but got Unknown
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1143: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Expected O, but got Unknown
		//IL_1150: Unknown result type (might be due to invalid IL or missing references)
		//IL_1159: Expected O, but got Unknown
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1164: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_116f: Expected O, but got Unknown
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_117a: Expected O, but got Unknown
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1185: Expected O, but got Unknown
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_1190: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119b: Expected O, but got Unknown
		//IL_1216: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Expected O, but got Unknown
		//IL_124d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1257: Expected O, but got Unknown
		//IL_12a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12af: Expected O, but got Unknown
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Expected O, but got Unknown
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Expected O, but got Unknown
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f1: Expected O, but got Unknown
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Expected O, but got Unknown
		//IL_1313: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1333: Expected O, but got Unknown
		//IL_13ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b7: Expected O, but got Unknown
		//IL_13ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f9: Expected O, but got Unknown
		//IL_1426: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Expected O, but got Unknown
		//IL_1468: Unknown result type (might be due to invalid IL or missing references)
		//IL_1472: Expected O, but got Unknown
		//IL_1494: Unknown result type (might be due to invalid IL or missing references)
		//IL_149e: Expected O, but got Unknown
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ca: Expected O, but got Unknown
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Expected O, but got Unknown
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Expected O, but got Unknown
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_1522: Expected O, but got Unknown
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c7: Expected O, but got Unknown
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e8: Expected O, but got Unknown
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fe: Expected O, but got Unknown
		//IL_1620: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Expected O, but got Unknown
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ae: Expected O, but got Unknown
		//IL_16ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c4: Expected O, but got Unknown
		//IL_16db: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e5: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1748: Expected O, but got Unknown
		//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d7: Expected O, but got Unknown
		//IL_17f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Expected O, but got Unknown
		//IL_1830: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Expected O, but got Unknown
		//IL_1846: Unknown result type (might be due to invalid IL or missing references)
		//IL_1850: Expected O, but got Unknown
		//IL_1893: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Expected O, but got Unknown
		//IL_18ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d4: Expected O, but got Unknown
		//IL_18e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ea: Expected O, but got Unknown
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f5: Expected O, but got Unknown
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_194d: Expected O, but got Unknown
		//IL_1964: Unknown result type (might be due to invalid IL or missing references)
		//IL_196e: Expected O, but got Unknown
		//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Expected O, but got Unknown
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Expected O, but got Unknown
		//IL_1a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a13: Expected O, but got Unknown
		//IL_1a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Expected O, but got Unknown
		//IL_1c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Expected O, but got Unknown
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Expected O, but got Unknown
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7b: Expected O, but got Unknown
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c86: Expected O, but got Unknown
		//IL_1d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d57: Expected O, but got Unknown
		//IL_1ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Expected O, but got Unknown
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e28: Expected O, but got Unknown
		//IL_1e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e54: Expected O, but got Unknown
		//IL_1e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e80: Expected O, but got Unknown
		//IL_1ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eac: Expected O, but got Unknown
		//IL_1ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed8: Expected O, but got Unknown
		//IL_2ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2adc: Expected O, but got Unknown
		//IL_2ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af3: Expected O, but got Unknown
		//IL_5ef1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5efb: Expected O, but got Unknown
		//IL_5f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f12: Expected O, but got Unknown
		//IL_8d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d7c: Expected O, but got Unknown
		//IL_a7e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_a7ee: Expected O, but got Unknown
		//IL_c7d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_c7de: Expected O, but got Unknown
		//IL_d71b: Unknown result type (might be due to invalid IL or missing references)
		//IL_d725: Expected O, but got Unknown
		//IL_162c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_162ca: Expected O, but got Unknown
		//IL_162ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_162f8: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdPlannedOutput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance450 = new Appearance();
		Appearance appearance460 = new Appearance();
		Appearance appearance471 = new Appearance();
		Appearance appearance482 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance464 = new Appearance();
		Appearance appearance465 = new Appearance();
		Appearance appearance466 = new Appearance();
		Appearance appearance467 = new Appearance();
		Appearance appearance468 = new Appearance();
		Appearance appearance469 = new Appearance();
		Appearance appearance470 = new Appearance();
		Appearance appearance472 = new Appearance();
		Appearance appearance473 = new Appearance();
		Appearance appearance474 = new Appearance();
		Appearance appearance475 = new Appearance();
		Appearance appearance476 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance448 = new Appearance();
		Appearance appearance449 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance452 = new Appearance();
		Appearance appearance453 = new Appearance();
		Appearance appearance454 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance461 = new Appearance();
		Appearance appearance462 = new Appearance();
		Appearance appearance463 = new Appearance();
		Appearance appearance477 = new Appearance();
		Appearance appearance478 = new Appearance();
		Appearance appearance479 = new Appearance();
		Appearance appearance480 = new Appearance();
		Appearance appearance481 = new Appearance();
		Appearance appearance483 = new Appearance();
		Appearance appearance484 = new Appearance();
		Appearance appearance485 = new Appearance();
		Appearance appearance486 = new Appearance();
		Appearance appearance487 = new Appearance();
		Appearance appearance488 = new Appearance();
		Appearance appearance489 = new Appearance();
		Appearance appearance490 = new Appearance();
		Appearance appearance491 = new Appearance();
		Appearance appearance492 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdInputDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
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
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
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
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance441 = new Appearance();
		Appearance appearance442 = new Appearance();
		Appearance appearance443 = new Appearance();
		GridEXLayout grdPlantdetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance445 = new Appearance();
		Appearance appearance446 = new Appearance();
		Appearance appearance447 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance416 = new Appearance();
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
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance451 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance455 = new Appearance();
		Appearance appearance456 = new Appearance();
		Appearance appearance457 = new Appearance();
		Appearance appearance458 = new Appearance();
		Appearance appearance459 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance232 = new Appearance();
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
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		GridEXLayout grdOutPutHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverheadHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterialHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.ProductionPreCostingAndJobOrder));
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdPlannedOutput = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.paneloutputgrd = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.CmbEntryTypeOP = new UltraCombo();
		this.txtItemAmountOutPut = new System.Windows.Forms.TextBox();
		this.txtPrctOfInputforOutputQty = new System.Windows.Forms.TextBox();
		this.txtItemRateOutPut = new System.Windows.Forms.TextBox();
		this.label89 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.CmbRateUomOutPut = new UltraCombo();
		this.cmbWareHouseOutPut = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbCropYearOutput = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.cmbJobLotOutput = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbouteruom = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtOuterqty = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdInputDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panelinputgrd = new System.Windows.Forms.Panel();
		this.lblStockWeight = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.cmbPackTypeInput = new UltraCombo();
		this.txtAmountInPut = new System.Windows.Forms.TextBox();
		this.txtRateInPut = new System.Windows.Forms.TextBox();
		this.txtweightinput = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbItemInput = new UltraCombo();
		this.btnadddetailinputgrd = new System.Windows.Forms.Button();
		this.txtremarksinput = new System.Windows.Forms.TextBox();
		this.btnupdategrdinput = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.btncancelgrdinput = new System.Windows.Forms.Button();
		this.cmbwarehouseinput = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbcropyearinput = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbjoblotinput = new UltraCombo();
		this.txtqtyinput = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbRateUomInPut = new UltraCombo();
		this.cmbpacksizeinput = new UltraCombo();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdOverHead = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label67 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.btnAddOH = new System.Windows.Forms.Button();
		this.BtnCancelOH = new System.Windows.Forms.Button();
		this.btnUpdateOH = new System.Windows.Forms.Button();
		this.lblRateOverHead = new System.Windows.Forms.Label();
		this.lblRemarksOverHead = new System.Windows.Forms.Label();
		this.txtLedgerRemarksOverHead = new System.Windows.Forms.TextBox();
		this.txtItemRateOverHeads = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.cmbChartofAccountOverHead = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.txtItemQtyOverHead = new System.Windows.Forms.TextBox();
		this.CmbBrandNameOverHead = new UltraCombo();
		this.label62 = new System.Windows.Forms.Label();
		this.CmbBrandUomOverHead = new UltraCombo();
		this.lblAmountOverHead = new System.Windows.Forms.Label();
		this.txtExpAmountOverHead = new System.Windows.Forms.TextBox();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdPackingMaterial = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label68 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.btnAddDetailPm = new System.Windows.Forms.Button();
		this.btnCancelDetailPm = new System.Windows.Forms.Button();
		this.btnUpdateDetailPm = new System.Windows.Forms.Button();
		this.label79 = new System.Windows.Forms.Label();
		this.txtremarksPackingMaterial = new System.Windows.Forms.TextBox();
		this.lblRemarksPackingMaterial = new System.Windows.Forms.Label();
		this.CmbWarehousePackingMaterial = new UltraCombo();
		this.txtPackingMaterialRate = new System.Windows.Forms.TextBox();
		this.cmbItemPackingMaterial = new UltraCombo();
		this.label63 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.CmbBrandUomPackingMaterial = new UltraCombo();
		this.lblRatePackingMaterial = new System.Windows.Forms.Label();
		this.CmbBrandNamePackingMaterial = new UltraCombo();
		this.txtPackingMaterialQTY = new System.Windows.Forms.TextBox();
		this.txtAmountPackingMaterial = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.lblAmountPackingMaterial = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.grdPlantdetail = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label85 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label92 = new System.Windows.Forms.Label();
		this.txtAssumedPlantCapacity = new System.Windows.Forms.TextBox();
		this.btnAddPlantDetail = new System.Windows.Forms.Button();
		this.btnCanclePlantdetail = new System.Windows.Forms.Button();
		this.btnUpdatePlantDetail = new System.Windows.Forms.Button();
		this.txtRemarksPlantDetail = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.cmbPlantFeader = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.btnProductionPlant = new System.Windows.Forms.Button();
		this.tabLabStandard = new System.Windows.Forms.TabPage();
		this.grdLabStandards = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel24 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbBuyerName = new UltraCombo();
		this.BtnDefineLabStandards = new System.Windows.Forms.Button();
		this.CmbBrandNameLabStandard = new UltraCombo();
		this.label95 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.RadBusinessName = new System.Windows.Forms.RadioButton();
		this.RadNickName = new System.Windows.Forms.RadioButton();
		this.radPartyCode = new System.Windows.Forms.RadioButton();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbAnalysisGroup = new UltraCombo();
		this.label93 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtWagesAmountInPut = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbentrytype = new UltraCombo();
		this.cmbfiltertype = new System.Windows.Forms.Label();
		this.cmbFiltersType = new UltraCombo();
		this.cmbDocNo = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtPmAmount = new System.Windows.Forms.TextBox();
		this.txtWagesRateInPut = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnProductionTypeDefine = new System.Windows.Forms.Button();
		this.CmbWipWarehouse = new UltraCombo();
		this.label90 = new System.Windows.Forms.Label();
		this.txtInvoiceReference = new System.Windows.Forms.TextBox();
		this.cmbProductionType = new UltraCombo();
		this.txtOtherInstruction = new System.Windows.Forms.TextBox();
		this.txtDeliveryInstruction = new System.Windows.Forms.TextBox();
		this.txtPlantInstruction = new System.Windows.Forms.TextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.txtPlanDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.txtPlanCode = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbWipItem = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbWorkInProcess = new UltraCombo();
		this.cmbStatus = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbPlanType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtProductionInstruction = new System.Windows.Forms.TextBox();
		this.txtstartDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel55 = new System.Windows.Forms.Panel();
		this.panel56 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.txtFGAvgRate = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.txtFGRateWithoutExp = new System.Windows.Forms.TextBox();
		this.panel51 = new System.Windows.Forms.Panel();
		this.panel52 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.txtTotalAmountFG = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.txtInPutTotalQty = new System.Windows.Forms.TextBox();
		this.txtTotalWeightFG = new System.Windows.Forms.TextBox();
		this.txtInPutTotalWeight = new System.Windows.Forms.TextBox();
		this.txtTotalAmountInPut = new System.Windows.Forms.TextBox();
		this.txtTotalQtyFG = new System.Windows.Forms.TextBox();
		this.txtTotalQtyBP = new System.Windows.Forms.TextBox();
		this.txtTotalWeightBP = new System.Windows.Forms.TextBox();
		this.txtTotalAmountBP = new System.Windows.Forms.TextBox();
		this.panel53 = new System.Windows.Forms.Panel();
		this.panel54 = new System.Windows.Forms.Panel();
		this.label94 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtOverHeadsAmount = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.txtPackingMaterialAmount = new System.Windows.Forms.TextBox();
		this.txtWagesAmountOutPut = new System.Windows.Forms.TextBox();
		this.CmbLotAc = new UltraCombo();
		this.label53 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtPmRate = new System.Windows.Forms.TextBox();
		this.CmbLabourItemOutPut = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtWagesRateOutPut = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbLabourItemInPut = new UltraCombo();
		this.CmbPackingMaterial = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label70 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoadInvoices = new System.Windows.Forms.ToolStripButton();
		this.BtnGenerateInPutRates = new System.Windows.Forms.ToolStripButton();
		this.btnGenerate = new System.Windows.Forms.ToolStripButton();
		this.BtnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label24 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label84 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label86 = new System.Windows.Forms.Label();
		this.xmbjoborderno = new UltraCombo();
		this.label88 = new System.Windows.Forms.Label();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.grdOutPutHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.grdInputHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label80 = new System.Windows.Forms.Label();
		this.tabPage11 = new System.Windows.Forms.TabPage();
		this.grdOverheadHistory = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label82 = new System.Windows.Forms.Label();
		this.tabPage10 = new System.Windows.Forms.TabPage();
		this.grdPackingMaterialHistory = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label83 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label77 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshHistory = new System.Windows.Forms.ToolStripButton();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlannedOutput).BeginInit();
		this.panel9.SuspendLayout();
		this.paneloutputgrd.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryTypeOP).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouteruom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panelinputgrd.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackTypeInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouseinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropyearinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblotinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomInPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpacksizeinput).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel17.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomOverHead).BeginInit();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.panel18.SuspendLayout();
		this.panel16.SuspendLayout();
		this.groupBox8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).BeginInit();
		this.tabPage7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlantdetail).BeginInit();
		this.panel22.SuspendLayout();
		this.panel23.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).BeginInit();
		this.tabLabStandard.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabStandards).BeginInit();
		this.panel13.SuspendLayout();
		this.panel24.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameLabStandard).BeginInit();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel55.SuspendLayout();
		this.panel56.SuspendLayout();
		this.panel51.SuspendLayout();
		this.panel52.SuspendLayout();
		this.panel53.SuspendLayout();
		this.panel54.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLotAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabourItemOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabourItemInPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingMaterial).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xmbjoborderno).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPage9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPage8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverheadHistory).BeginInit();
		this.panel19.SuspendLayout();
		this.tabPage10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(250, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1363, 27);
		this.miniToolStrip.TabIndex = 3;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1125, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1125, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1117, 720);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1111, 714);
		this.panel2.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.tabControl2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 376);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1111, 338);
		this.panel6.TabIndex = 61;
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Controls.Add(this.tabPage7);
		this.tabControl2.Controls.Add(this.tabLabStandard);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.ShowToolTips = true;
		this.tabControl2.Size = new System.Drawing.Size(1111, 338);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel14);
		this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage3.Location = new System.Drawing.Point(4, 29);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1103, 305);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Planned Output Requirnment";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlannedOutput);
		this.panel14.Controls.Add(this.panel9);
		this.panel14.Controls.Add(this.paneloutputgrd);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1097, 299);
		this.panel14.TabIndex = 0;
		((ScrollableBase)this.grdPlannedOutput).BackColor = System.Drawing.Color.White;
		grdPlannedOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPlannedOutput.DesignTimeLayout = grdPlannedOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPlannedOutput.FilterMode = (FilterMode)2;
		this.grdPlannedOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPlannedOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Location = new System.Drawing.Point(0, 133);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Name = "grdPlannedOutput";
		((ScrollableBase)this.grdPlannedOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Size = new System.Drawing.Size(1097, 166);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).TabIndex = 1;
		this.grdPlannedOutput.TotalRow = (InheritableBoolean)1;
		this.grdPlannedOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlannedOutput).VisualStyle = (VisualStyle)4;
		this.grdPlannedOutput.CellUpdated += new ColumnActionEventHandler(grdPlannedOutput_CellUpdated);
		this.grdPlannedOutput.ColumnButtonClick += new ColumnActionEventHandler(grdPlannedOutput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).DoubleClick += new System.EventHandler(grdPlannedOutput_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutByProduct_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 110);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1097, 23);
		this.panel9.TabIndex = 206;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1052, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdPlannedOutput;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.paneloutputgrd.BackColor = System.Drawing.Color.White;
		this.paneloutputgrd.Controls.Add(this.label69);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP);
		this.paneloutputgrd.Controls.Add(this.txtItemAmountOutPut);
		this.paneloutputgrd.Controls.Add(this.txtPrctOfInputforOutputQty);
		this.paneloutputgrd.Controls.Add(this.txtItemRateOutPut);
		this.paneloutputgrd.Controls.Add(this.label89);
		this.paneloutputgrd.Controls.Add(this.label44);
		this.paneloutputgrd.Controls.Add(this.label46);
		this.paneloutputgrd.Controls.Add(this.label47);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut);
		this.paneloutputgrd.Controls.Add(this.label12);
		this.paneloutputgrd.Controls.Add(this.label14);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYearOutput);
		this.paneloutputgrd.Controls.Add(this.label41);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotOutput);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.paneloutputgrd.Controls.Add(this.label37);
		this.paneloutputgrd.Controls.Add(this.label25);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbouteruom);
		this.paneloutputgrd.Controls.Add(this.label35);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.paneloutputgrd.Controls.Add(this.label26);
		this.paneloutputgrd.Controls.Add(this.txtOuterqty);
		this.paneloutputgrd.Controls.Add(this.label39);
		this.paneloutputgrd.Controls.Add(this.txtweight);
		this.paneloutputgrd.Controls.Add(this.label27);
		this.paneloutputgrd.Controls.Add(this.txtremarks);
		this.paneloutputgrd.Controls.Add(this.btnplus);
		this.paneloutputgrd.Controls.Add(this.btnUpdateDetail);
		this.paneloutputgrd.Controls.Add(this.btnCancelUpdateDetial);
		this.paneloutputgrd.Dock = System.Windows.Forms.DockStyle.Top;
		this.paneloutputgrd.Location = new System.Drawing.Point(0, 0);
		this.paneloutputgrd.Name = "paneloutputgrd";
		this.paneloutputgrd.Size = new System.Drawing.Size(1097, 110);
		this.paneloutputgrd.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(4, 8);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(59, 15);
		this.label69.TabIndex = 207;
		this.label69.Text = "EntryType";
		this.CmbEntryTypeOP.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryTypeOP.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryTypeOP.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryTypeOP.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance172;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance437).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance437).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance437;
		((AppearanceBase)appearance450).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance450).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance450;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance460).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance460;
		((AppearanceBase)appearance471).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance471).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance471;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance482).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance482).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance482).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance482).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance482).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance482;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryTypeOP).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbEntryTypeOP.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP).Location = new System.Drawing.Point(4, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP).Name = "CmbEntryTypeOP";
		((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryTypeOP).TabIndex = 0;
		this.txtItemAmountOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmountOutPut.Location = new System.Drawing.Point(372, 79);
		this.txtItemAmountOutPut.Name = "txtItemAmountOutPut";
		this.txtItemAmountOutPut.ReadOnly = true;
		this.txtItemAmountOutPut.Size = new System.Drawing.Size(91, 23);
		this.txtItemAmountOutPut.TabIndex = 12;
		this.txtPrctOfInputforOutputQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPrctOfInputforOutputQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPrctOfInputforOutputQty.Location = new System.Drawing.Point(73, 79);
		this.txtPrctOfInputforOutputQty.Name = "txtPrctOfInputforOutputQty";
		this.txtPrctOfInputforOutputQty.Size = new System.Drawing.Size(67, 23);
		this.txtPrctOfInputforOutputQty.TabIndex = 8;
		this.txtPrctOfInputforOutputQty.TextChanged += new System.EventHandler(txtPrctOfInputforOutputQty_TextChanged);
		this.txtPrctOfInputforOutputQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPrctOfInputforOutputQty_KeyPress);
		this.txtItemRateOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemRateOutPut.Location = new System.Drawing.Point(232, 79);
		this.txtItemRateOutPut.Name = "txtItemRateOutPut";
		this.txtItemRateOutPut.Size = new System.Drawing.Size(66, 23);
		this.txtItemRateOutPut.TabIndex = 10;
		this.txtItemRateOutPut.TextChanged += new System.EventHandler(txtItemRateOutPut_TextChanged);
		this.txtItemRateOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemRateOutPut_KeyPress);
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label89.Location = new System.Drawing.Point(73, 58);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(63, 15);
		this.label89.TabIndex = 212;
		this.label89.Text = "% of Input";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(372, 58);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 211;
		this.label44.Text = "Amount";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(232, 58);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(30, 15);
		this.label46.TabIndex = 212;
		this.label46.Text = "Rate";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(300, 58);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(59, 15);
		this.label47.TabIndex = 210;
		this.label47.Text = "Rate Uom";
		this.CmbRateUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance464).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance464).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance464;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance465).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance465).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance465).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance465).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance465;
		((AppearanceBase)appearance466).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance466;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance467).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance467).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance467).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance467).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance467;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance468).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance468).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance468;
		((AppearanceBase)appearance469).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance469).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance469;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance470).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance470;
		((AppearanceBase)appearance472).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance472).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance472;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance473).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance473).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance473).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance473).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance473).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance473;
		((AppearanceBase)appearance474).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance474;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance475).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance475).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance475;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance476).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance476;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Location = new System.Drawing.Point(300, 77);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Name = "CmbRateUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Size = new System.Drawing.Size(71, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TextChanged += new System.EventHandler(CmbRateUomOutPut_TextChanged);
		this.cmbWareHouseOutPut.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance260).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Location = new System.Drawing.Point(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Name = "cmbWareHouseOutPut";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(730, 8);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(57, 15);
		this.label12.TabIndex = 204;
		this.label12.Text = "Crop Year";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(115, 8);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 206;
		this.label14.Text = "WareHouse";
		this.cmbCropYearOutput.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance315;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance327).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance327).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance349).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance360;
		((AppearanceBase)appearance371).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance371).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance382).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance382).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance382).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance393).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance404).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance404;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYearOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Location = new System.Drawing.Point(730, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Name = "cmbCropYearOutput";
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).TabIndex = 4;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(300, 8);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(48, 15);
		this.label41.TabIndex = 205;
		this.label41.Text = "Job/Lot";
		this.cmbJobLotOutput.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance448).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance448;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance449).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance449).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance449;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLotOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Location = new System.Drawing.Point(300, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Name = "cmbJobLotOutput";
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).TabIndex = 2;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance452).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance452).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance452).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance452).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance452;
		((AppearanceBase)appearance453).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance453;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance454).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance454).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance454).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance454).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance454;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance461).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance461;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance462).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance462).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance462;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance463).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance463;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(464, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(464, 8);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 178;
		this.label37.Text = "Item Name";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(4, 58);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(68, 15);
		this.label25.TabIndex = 180;
		this.label25.Text = "Outer UOM";
		this.cmbouteruom.AllowNull = (DefaultableBoolean)1;
		this.cmbouteruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbouteruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbouteruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance477).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance477).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance477;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance478).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance478).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance478).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance478).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance478;
		((AppearanceBase)appearance479).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance479;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance480).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance480).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance480).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance480).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance480;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance481).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance481).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance481;
		((AppearanceBase)appearance483).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance483).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance483;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance484).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance484;
		((AppearanceBase)appearance485).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance485).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance485;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance486).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance486).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance486).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance486).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance486).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance486;
		((AppearanceBase)appearance487).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance487;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance488).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance488).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance488;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance489).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance489;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbouteruom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Location = new System.Drawing.Point(4, 77);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Name = "cmbouteruom";
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).TabIndex = 7;
		this.cmbouteruom.ValueChanged += new System.EventHandler(cmbouteruom_ValueChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(828, 8);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 184;
		this.label35.Text = "PackingType";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance490).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance490).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance490;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance491).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance491).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance491).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance491).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance491;
		((AppearanceBase)appearance492).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance492;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(828, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 5;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(964, 6);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(59, 15);
		this.label26.TabIndex = 186;
		this.label26.Text = "Outer Qty";
		this.txtOuterqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOuterqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOuterqty.Location = new System.Drawing.Point(964, 27);
		this.txtOuterqty.Name = "txtOuterqty";
		this.txtOuterqty.Size = new System.Drawing.Size(72, 23);
		this.txtOuterqty.TabIndex = 6;
		this.txtOuterqty.TextChanged += new System.EventHandler(txtOuterqty_TextChanged);
		this.txtOuterqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOuterqty_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(141, 58);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 188;
		this.label39.Text = "Weight";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(141, 79);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(90, 23);
		this.txtweight.TabIndex = 9;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(464, 58);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 188;
		this.label27.Text = "Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(464, 79);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(405, 23);
		this.txtremarks.TabIndex = 13;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(873, 77);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(28, 26);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(903, 77);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(973, 77);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.tabPage4.Controls.Add(this.panel15);
		this.tabPage4.Location = new System.Drawing.Point(4, 29);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1103, 305);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Input Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputDetail);
		this.panel15.Controls.Add(this.panel7);
		this.panel15.Controls.Add(this.panelinputgrd);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1097, 299);
		this.panel15.TabIndex = 0;
		this.grdInputDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdInputDetail).BackColor = System.Drawing.Color.White;
		grdInputDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputDetail.DesignTimeLayout = grdInputDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInputDetail.FilterMode = (FilterMode)2;
		this.grdInputDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Location = new System.Drawing.Point(0, 133);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Name = "grdInputDetail";
		((ScrollableBase)this.grdInputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1097, 166);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 1;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		this.grdInputDetail.CellUpdated += new ColumnActionEventHandler(grdInputDetail_CellUpdated);
		this.grdInputDetail.ColumnButtonClick += new ColumnActionEventHandler(grdInputDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).DoubleClick += new System.EventHandler(grdInputDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 110);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1097, 23);
		this.panel7.TabIndex = 207;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1053, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdInputDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panelinputgrd.BackColor = System.Drawing.Color.White;
		this.panelinputgrd.Controls.Add(this.lblStockWeight);
		this.panelinputgrd.Controls.Add(this.label42);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackTypeInput);
		this.panelinputgrd.Controls.Add(this.txtAmountInPut);
		this.panelinputgrd.Controls.Add(this.txtRateInPut);
		this.panelinputgrd.Controls.Add(this.txtweightinput);
		this.panelinputgrd.Controls.Add(this.label29);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemInput);
		this.panelinputgrd.Controls.Add(this.btnadddetailinputgrd);
		this.panelinputgrd.Controls.Add(this.txtremarksinput);
		this.panelinputgrd.Controls.Add(this.btnupdategrdinput);
		this.panelinputgrd.Controls.Add(this.label36);
		this.panelinputgrd.Controls.Add(this.btncancelgrdinput);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbwarehouseinput);
		this.panelinputgrd.Controls.Add(this.label30);
		this.panelinputgrd.Controls.Add(this.label34);
		this.panelinputgrd.Controls.Add(this.label9);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropyearinput);
		this.panelinputgrd.Controls.Add(this.label3);
		this.panelinputgrd.Controls.Add(this.label45);
		this.panelinputgrd.Controls.Add(this.label33);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbjoblotinput);
		this.panelinputgrd.Controls.Add(this.txtqtyinput);
		this.panelinputgrd.Controls.Add(this.label19);
		this.panelinputgrd.Controls.Add(this.label32);
		this.panelinputgrd.Controls.Add(this.label31);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomInPut);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpacksizeinput);
		this.panelinputgrd.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelinputgrd.Location = new System.Drawing.Point(0, 0);
		this.panelinputgrd.Name = "panelinputgrd";
		this.panelinputgrd.Size = new System.Drawing.Size(1097, 110);
		this.panelinputgrd.TabIndex = 0;
		this.lblStockWeight.Location = new System.Drawing.Point(586, 7);
		this.lblStockWeight.Name = "lblStockWeight";
		this.lblStockWeight.Size = new System.Drawing.Size(83, 17);
		this.lblStockWeight.TabIndex = 205;
		this.lblStockWeight.Text = "0";
		this.lblStockWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(788, 9);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(74, 15);
		this.label42.TabIndex = 204;
		this.label42.Text = "PackingType";
		this.cmbPackTypeInput.AllowNull = (DefaultableBoolean)1;
		this.cmbPackTypeInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackTypeInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackTypeInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackTypeInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Location = new System.Drawing.Point(788, 28);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Name = "cmbPackTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).TabIndex = 4;
		this.txtAmountInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmountInPut.Location = new System.Drawing.Point(345, 79);
		this.txtAmountInPut.Name = "txtAmountInPut";
		this.txtAmountInPut.ReadOnly = true;
		this.txtAmountInPut.Size = new System.Drawing.Size(101, 23);
		this.txtAmountInPut.TabIndex = 10;
		this.txtRateInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateInPut.Location = new System.Drawing.Point(194, 79);
		this.txtRateInPut.Name = "txtRateInPut";
		this.txtRateInPut.ReadOnly = true;
		this.txtRateInPut.Size = new System.Drawing.Size(62, 23);
		this.txtRateInPut.TabIndex = 8;
		this.txtRateInPut.TextChanged += new System.EventHandler(txtRateInPut_TextChanged);
		this.txtweightinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweightinput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweightinput.Location = new System.Drawing.Point(93, 79);
		this.txtweightinput.Name = "txtweightinput";
		this.txtweightinput.Size = new System.Drawing.Size(98, 23);
		this.txtweightinput.TabIndex = 7;
		this.txtweightinput.TextChanged += new System.EventHandler(txtweightinput_TextChanged);
		this.txtweightinput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweightinput_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(437, 9);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 178;
		this.label29.Text = "Item Name";
		this.cmbItemInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(437, 28);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Leave += new System.EventHandler(cmbItemInput_Leave);
		this.btnadddetailinputgrd.BackColor = System.Drawing.Color.Teal;
		this.btnadddetailinputgrd.FlatAppearance.BorderSize = 0;
		this.btnadddetailinputgrd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnadddetailinputgrd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnadddetailinputgrd.ForeColor = System.Drawing.Color.White;
		this.btnadddetailinputgrd.Location = new System.Drawing.Point(850, 77);
		this.btnadddetailinputgrd.Name = "btnadddetailinputgrd";
		this.btnadddetailinputgrd.Size = new System.Drawing.Size(39, 26);
		this.btnadddetailinputgrd.TabIndex = 12;
		this.btnadddetailinputgrd.Text = "+";
		this.btnadddetailinputgrd.UseVisualStyleBackColor = false;
		this.btnadddetailinputgrd.Click += new System.EventHandler(btnadddetailinputgrd_Click);
		this.txtremarksinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksinput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksinput.Location = new System.Drawing.Point(451, 79);
		this.txtremarksinput.Name = "txtremarksinput";
		this.txtremarksinput.Size = new System.Drawing.Size(393, 23);
		this.txtremarksinput.TabIndex = 11;
		this.btnupdategrdinput.BackColor = System.Drawing.Color.Teal;
		this.btnupdategrdinput.FlatAppearance.BorderSize = 0;
		this.btnupdategrdinput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnupdategrdinput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdategrdinput.ForeColor = System.Drawing.Color.White;
		this.btnupdategrdinput.Location = new System.Drawing.Point(891, 77);
		this.btnupdategrdinput.Name = "btnupdategrdinput";
		this.btnupdategrdinput.Size = new System.Drawing.Size(68, 26);
		this.btnupdategrdinput.TabIndex = 13;
		this.btnupdategrdinput.Text = "Update";
		this.btnupdategrdinput.UseVisualStyleBackColor = false;
		this.btnupdategrdinput.Visible = false;
		this.btnupdategrdinput.Click += new System.EventHandler(btnupdategrdinput_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(454, 60);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(52, 15);
		this.label36.TabIndex = 202;
		this.label36.Text = "Remarks";
		this.btncancelgrdinput.BackColor = System.Drawing.Color.Teal;
		this.btncancelgrdinput.FlatAppearance.BorderSize = 0;
		this.btncancelgrdinput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btncancelgrdinput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btncancelgrdinput.ForeColor = System.Drawing.Color.White;
		this.btncancelgrdinput.Location = new System.Drawing.Point(960, 77);
		this.btncancelgrdinput.Name = "btncancelgrdinput";
		this.btncancelgrdinput.Size = new System.Drawing.Size(68, 26);
		this.btncancelgrdinput.TabIndex = 14;
		this.btncancelgrdinput.Text = "Cancel";
		this.btncancelgrdinput.UseVisualStyleBackColor = false;
		this.btncancelgrdinput.Visible = false;
		this.btncancelgrdinput.Click += new System.EventHandler(btncancelgrdinput_Click);
		this.cmbwarehouseinput.AllowNull = (DefaultableBoolean)1;
		this.cmbwarehouseinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbwarehouseinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbwarehouseinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbwarehouseinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Location = new System.Drawing.Point(8, 28);
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Name = "cmbwarehouseinput";
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Leave += new System.EventHandler(cmbwarehouseinput_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(671, 9);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 190;
		this.label30.Text = "Crop Year";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(8, 9);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(69, 15);
		this.label34.TabIndex = 200;
		this.label34.Text = "WareHouse";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(342, 60);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(51, 15);
		this.label9.TabIndex = 198;
		this.label9.Text = "Amount";
		this.cmbcropyearinput.AllowNull = (DefaultableBoolean)1;
		this.cmbcropyearinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropyearinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropyearinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcropyearinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Location = new System.Drawing.Point(671, 28);
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Name = "cmbcropyearinput";
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Leave += new System.EventHandler(cmbcropyearinput_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(191, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(30, 15);
		this.label3.TabIndex = 198;
		this.label3.Text = "Rate";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(203, 9);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 192;
		this.label45.Text = "Job/Lot";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(90, 60);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 198;
		this.label33.Text = "Weight";
		this.cmbjoblotinput.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblotinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblotinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblotinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbjoblotinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Location = new System.Drawing.Point(201, 28);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Name = "cmbjoblotinput";
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Leave += new System.EventHandler(cmbjoblotinput_Leave);
		this.txtqtyinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqtyinput.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqtyinput.Location = new System.Drawing.Point(914, 30);
		this.txtqtyinput.Name = "txtqtyinput";
		this.txtqtyinput.Size = new System.Drawing.Size(101, 23);
		this.txtqtyinput.TabIndex = 5;
		this.txtqtyinput.TextChanged += new System.EventHandler(txtqtyinput_TextChanged);
		this.txtqtyinput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqtyinput_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(261, 60);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(59, 15);
		this.label19.TabIndex = 195;
		this.label19.Text = "Rate Uom";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(8, 60);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(61, 15);
		this.label32.TabIndex = 195;
		this.label32.Text = "Pack Uom";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(914, 9);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(26, 15);
		this.label31.TabIndex = 196;
		this.label31.Text = "Qty";
		this.CmbRateUomInPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomInPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomInPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomInPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomInPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUomInPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Location = new System.Drawing.Point(258, 77);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Name = "CmbRateUomInPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInPut).TextChanged += new System.EventHandler(CmbRateUomInPut_TextChanged);
		this.cmbpacksizeinput.AllowNull = (DefaultableBoolean)1;
		this.cmbpacksizeinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpacksizeinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpacksizeinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpacksizeinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Location = new System.Drawing.Point(5, 77);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Name = "cmbpacksizeinput";
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).TabIndex = 6;
		this.cmbpacksizeinput.ValueChanged += new System.EventHandler(cmbpacksizeinput_ValueChanged);
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.tabPage5.Controls.Add(this.panel17);
		this.tabPage5.Controls.Add(this.panel10);
		this.tabPage5.Location = new System.Drawing.Point(4, 29);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1103, 305);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "OverHeads";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdOverHead.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOverHead.FilterMode = (FilterMode)2;
		this.grdOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 140);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(1103, 165);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 0;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).DoubleClick += new System.EventHandler(grdOverHead_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar2);
		this.panel17.Controls.Add(this.label67);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 120);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1103, 20);
		this.panel17.TabIndex = 13;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1063, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7);
		this.ctrlGrdBar2.MyGrid = this.grdOverHead;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.White;
		this.label67.Location = new System.Drawing.Point(3, 0);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(120, 17);
		this.label67.TabIndex = 6;
		this.label67.Text = "Over Heads Detail";
		this.panel10.BackColor = System.Drawing.Color.White;
		this.panel10.Controls.Add(this.groupBox7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1103, 120);
		this.panel10.TabIndex = 0;
		this.groupBox7.Controls.Add(this.btnAddOH);
		this.groupBox7.Controls.Add(this.BtnCancelOH);
		this.groupBox7.Controls.Add(this.btnUpdateOH);
		this.groupBox7.Controls.Add(this.lblRateOverHead);
		this.groupBox7.Controls.Add(this.lblRemarksOverHead);
		this.groupBox7.Controls.Add(this.txtLedgerRemarksOverHead);
		this.groupBox7.Controls.Add(this.txtItemRateOverHeads);
		this.groupBox7.Controls.Add(this.label59);
		this.groupBox7.Controls.Add(this.label60);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead);
		this.groupBox7.Controls.Add(this.label61);
		this.groupBox7.Controls.Add(this.txtItemQtyOverHead);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead);
		this.groupBox7.Controls.Add(this.label62);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead);
		this.groupBox7.Controls.Add(this.lblAmountOverHead);
		this.groupBox7.Controls.Add(this.txtExpAmountOverHead);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(1, 4);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(1041, 114);
		this.groupBox7.TabIndex = 0;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Detail";
		this.btnAddOH.BackColor = System.Drawing.Color.Teal;
		this.btnAddOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddOH.ForeColor = System.Drawing.Color.White;
		this.btnAddOH.Location = new System.Drawing.Point(802, 86);
		this.btnAddOH.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddOH.Name = "btnAddOH";
		this.btnAddOH.Size = new System.Drawing.Size(38, 26);
		this.btnAddOH.TabIndex = 7;
		this.btnAddOH.Text = "+";
		this.btnAddOH.UseVisualStyleBackColor = false;
		this.btnAddOH.Click += new System.EventHandler(btnAddOH_Click);
		this.BtnCancelOH.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelOH.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOH.Location = new System.Drawing.Point(910, 86);
		this.BtnCancelOH.Name = "BtnCancelOH";
		this.BtnCancelOH.Size = new System.Drawing.Size(67, 26);
		this.BtnCancelOH.TabIndex = 9;
		this.BtnCancelOH.Text = "Cancel";
		this.BtnCancelOH.UseVisualStyleBackColor = false;
		this.BtnCancelOH.Click += new System.EventHandler(BtnCancelOH_Click);
		this.btnUpdateOH.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOH.ForeColor = System.Drawing.Color.White;
		this.btnUpdateOH.Location = new System.Drawing.Point(840, 86);
		this.btnUpdateOH.Name = "btnUpdateOH";
		this.btnUpdateOH.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateOH.TabIndex = 8;
		this.btnUpdateOH.Text = "Update ";
		this.btnUpdateOH.UseVisualStyleBackColor = false;
		this.btnUpdateOH.Click += new System.EventHandler(btnUpdateOH_Click);
		this.lblRateOverHead.AutoSize = true;
		this.lblRateOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateOverHead.Location = new System.Drawing.Point(667, 19);
		this.lblRateOverHead.Name = "lblRateOverHead";
		this.lblRateOverHead.Size = new System.Drawing.Size(30, 15);
		this.lblRateOverHead.TabIndex = 5;
		this.lblRateOverHead.Text = "Rate";
		this.lblRemarksOverHead.AutoSize = true;
		this.lblRemarksOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarksOverHead.Location = new System.Drawing.Point(12, 68);
		this.lblRemarksOverHead.Name = "lblRemarksOverHead";
		this.lblRemarksOverHead.Size = new System.Drawing.Size(91, 15);
		this.lblRemarksOverHead.TabIndex = 62;
		this.lblRemarksOverHead.Text = "Ledger Remarks";
		this.txtLedgerRemarksOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerRemarksOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLedgerRemarksOverHead.Location = new System.Drawing.Point(12, 88);
		this.txtLedgerRemarksOverHead.Name = "txtLedgerRemarksOverHead";
		this.txtLedgerRemarksOverHead.Size = new System.Drawing.Size(786, 23);
		this.txtLedgerRemarksOverHead.TabIndex = 6;
		this.txtItemRateOverHeads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRateOverHeads.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRateOverHeads.Location = new System.Drawing.Point(663, 40);
		this.txtItemRateOverHeads.Name = "txtItemRateOverHeads";
		this.txtItemRateOverHeads.Size = new System.Drawing.Size(91, 23);
		this.txtItemRateOverHeads.TabIndex = 4;
		this.txtItemRateOverHeads.TextChanged += new System.EventHandler(txtItemRateOverHeads_TextChanged);
		this.txtItemRateOverHeads.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemRateOverHeads_KeyPress);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(475, 19);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(61, 15);
		this.label59.TabIndex = 124;
		this.label59.Text = "Item Uom";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(580, 19);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(54, 15);
		this.label60.TabIndex = 118;
		this.label60.Text = "Item Qty";
		this.cmbChartofAccountOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbChartofAccountOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbChartofAccountOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbChartofAccountOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbChartofAccountOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Location = new System.Drawing.Point(12, 38);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Name = "cmbChartofAccountOverHead";
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).TabIndex = 0;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(12, 19);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(108, 15);
		this.label61.TabIndex = 60;
		this.label61.Text = "OverHead Account";
		this.txtItemQtyOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQtyOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyOverHead.Location = new System.Drawing.Point(577, 40);
		this.txtItemQtyOverHead.Name = "txtItemQtyOverHead";
		this.txtItemQtyOverHead.Size = new System.Drawing.Size(83, 23);
		this.txtItemQtyOverHead.TabIndex = 3;
		this.txtItemQtyOverHead.TextChanged += new System.EventHandler(txtItemQtyOverHead_Leave);
		this.txtItemQtyOverHead.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQtyOverHead_KeyPress);
		this.txtItemQtyOverHead.Leave += new System.EventHandler(txtItemQtyOverHead_Leave);
		this.CmbBrandNameOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNameOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNameOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNameOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNameOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Location = new System.Drawing.Point(243, 38);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Name = "CmbBrandNameOverHead";
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Leave += new System.EventHandler(CmbBrandNameOverHead_Leave);
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(245, 19);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(108, 15);
		this.label62.TabIndex = 123;
		this.label62.Text = "OutPut Item Name";
		this.CmbBrandUomOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Location = new System.Drawing.Point(473, 38);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Name = "CmbBrandUomOverHead";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Leave += new System.EventHandler(CmbBrandUomOverHead_Leave);
		this.lblAmountOverHead.AutoSize = true;
		this.lblAmountOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountOverHead.Location = new System.Drawing.Point(761, 19);
		this.lblAmountOverHead.Name = "lblAmountOverHead";
		this.lblAmountOverHead.Size = new System.Drawing.Size(51, 15);
		this.lblAmountOverHead.TabIndex = 60;
		this.lblAmountOverHead.Text = "Amount";
		this.txtExpAmountOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpAmountOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpAmountOverHead.Location = new System.Drawing.Point(757, 40);
		this.txtExpAmountOverHead.Name = "txtExpAmountOverHead";
		this.txtExpAmountOverHead.Size = new System.Drawing.Size(113, 23);
		this.txtExpAmountOverHead.TabIndex = 5;
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.tabPage6.Controls.Add(this.panel18);
		this.tabPage6.Controls.Add(this.panel16);
		this.tabPage6.Location = new System.Drawing.Point(4, 29);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1103, 305);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Packing Material";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdPackingMaterial.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterial.FilterMode = (FilterMode)2;
		this.grdPackingMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 143);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(1103, 162);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 14;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).DoubleClick += new System.EventHandler(grdPackingMaterial_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterial_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar4);
		this.panel18.Controls.Add(this.label68);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 123);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1103, 20);
		this.panel18.TabIndex = 13;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1060, -2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdPackingMaterial;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar4.TabIndex = 22;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.White;
		this.label68.Location = new System.Drawing.Point(3, 0);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(152, 17);
		this.label68.TabIndex = 6;
		this.label68.Text = "Packing Material Detail";
		this.panel16.BackColor = System.Drawing.Color.White;
		this.panel16.Controls.Add(this.groupBox8);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1103, 123);
		this.panel16.TabIndex = 0;
		this.groupBox8.Controls.Add(this.btnAddDetailPm);
		this.groupBox8.Controls.Add(this.btnCancelDetailPm);
		this.groupBox8.Controls.Add(this.btnUpdateDetailPm);
		this.groupBox8.Controls.Add(this.label79);
		this.groupBox8.Controls.Add(this.txtremarksPackingMaterial);
		this.groupBox8.Controls.Add(this.lblRemarksPackingMaterial);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial);
		this.groupBox8.Controls.Add(this.txtPackingMaterialRate);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial);
		this.groupBox8.Controls.Add(this.label63);
		this.groupBox8.Controls.Add(this.label64);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial);
		this.groupBox8.Controls.Add(this.lblRatePackingMaterial);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial);
		this.groupBox8.Controls.Add(this.txtPackingMaterialQTY);
		this.groupBox8.Controls.Add(this.txtAmountPackingMaterial);
		this.groupBox8.Controls.Add(this.label65);
		this.groupBox8.Controls.Add(this.lblAmountPackingMaterial);
		this.groupBox8.Controls.Add(this.label66);
		this.groupBox8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox8.Location = new System.Drawing.Point(3, 5);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(1039, 111);
		this.groupBox8.TabIndex = 0;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Detail";
		this.btnAddDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetailPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnAddDetailPm.Location = new System.Drawing.Point(763, 81);
		this.btnAddDetailPm.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddDetailPm.Name = "btnAddDetailPm";
		this.btnAddDetailPm.Size = new System.Drawing.Size(38, 26);
		this.btnAddDetailPm.TabIndex = 8;
		this.btnAddDetailPm.Text = "+";
		this.btnAddDetailPm.UseVisualStyleBackColor = false;
		this.btnAddDetailPm.Click += new System.EventHandler(btnAddDetailPm_Click);
		this.btnCancelDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetailPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetailPm.Location = new System.Drawing.Point(871, 81);
		this.btnCancelDetailPm.Name = "btnCancelDetailPm";
		this.btnCancelDetailPm.Size = new System.Drawing.Size(67, 26);
		this.btnCancelDetailPm.TabIndex = 10;
		this.btnCancelDetailPm.Text = "Cancel";
		this.btnCancelDetailPm.UseVisualStyleBackColor = false;
		this.btnCancelDetailPm.Click += new System.EventHandler(btnCancelDetailPm_Click);
		this.btnUpdateDetailPm.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetailPm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetailPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailPm.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetailPm.Location = new System.Drawing.Point(801, 81);
		this.btnUpdateDetailPm.Name = "btnUpdateDetailPm";
		this.btnUpdateDetailPm.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateDetailPm.TabIndex = 9;
		this.btnUpdateDetailPm.Text = "Update ";
		this.btnUpdateDetailPm.UseVisualStyleBackColor = false;
		this.btnUpdateDetailPm.Click += new System.EventHandler(btnUpdateDetailPm_Click);
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label79.Location = new System.Drawing.Point(4, 17);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(67, 15);
		this.label79.TabIndex = 122;
		this.label79.Text = "Warehouse";
		this.txtremarksPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksPackingMaterial.Location = new System.Drawing.Point(8, 83);
		this.txtremarksPackingMaterial.Name = "txtremarksPackingMaterial";
		this.txtremarksPackingMaterial.Size = new System.Drawing.Size(750, 23);
		this.txtremarksPackingMaterial.TabIndex = 7;
		this.lblRemarksPackingMaterial.AutoSize = true;
		this.lblRemarksPackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarksPackingMaterial.Location = new System.Drawing.Point(8, 65);
		this.lblRemarksPackingMaterial.Name = "lblRemarksPackingMaterial";
		this.lblRemarksPackingMaterial.Size = new System.Drawing.Size(52, 15);
		this.lblRemarksPackingMaterial.TabIndex = 63;
		this.lblRemarksPackingMaterial.Text = "Remarks";
		this.CmbWarehousePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehousePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehousePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehousePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance365).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance365).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance365).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance366;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance367).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance367).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance367).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance368).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance368;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance369).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance370;
		((AppearanceBase)appearance372).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance372).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance373).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance373).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance373).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance373).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance373;
		((AppearanceBase)appearance374).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance376;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehousePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Location = new System.Drawing.Point(7, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Name = "CmbWarehousePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).TabIndex = 0;
		this.txtPackingMaterialRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialRate.Location = new System.Drawing.Point(811, 37);
		this.txtPackingMaterialRate.Name = "txtPackingMaterialRate";
		this.txtPackingMaterialRate.Size = new System.Drawing.Size(84, 23);
		this.txtPackingMaterialRate.TabIndex = 5;
		this.txtPackingMaterialRate.TextChanged += new System.EventHandler(txtPackingMaterialRate_TextChanged);
		this.txtPackingMaterialRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingMaterialRate_KeyPress);
		this.cmbItemPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbItemPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance378).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance378).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance378).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance379).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance379;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance380).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance380).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance380).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance381).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance381;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance383;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance384;
		((AppearanceBase)appearance385).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance385).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance386;
		((AppearanceBase)appearance387).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance388).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance388;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance389;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Location = new System.Drawing.Point(196, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Name = "cmbItemPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).TabIndex = 1;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(710, 19);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(54, 15);
		this.label63.TabIndex = 59;
		this.label63.Text = "Item Qty";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(197, 17);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(158, 15);
		this.label64.TabIndex = 55;
		this.label64.Text = "Packing Material Item Name";
		this.CmbBrandUomPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance390;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance391).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance391).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance391).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance391;
		((AppearanceBase)appearance392).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance392;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance394).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance394).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance394).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance395).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance397;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance398).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance399).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance399).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance399).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance399;
		((AppearanceBase)appearance400).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance400;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance401).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance401).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Location = new System.Drawing.Point(629, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Name = "CmbBrandUomPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Leave += new System.EventHandler(CmbBrandUomPackingMaterial_Leave);
		this.lblRatePackingMaterial.AutoSize = true;
		this.lblRatePackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRatePackingMaterial.Location = new System.Drawing.Point(812, 19);
		this.lblRatePackingMaterial.Name = "lblRatePackingMaterial";
		this.lblRatePackingMaterial.Size = new System.Drawing.Size(55, 15);
		this.lblRatePackingMaterial.TabIndex = 59;
		this.lblRatePackingMaterial.Text = "ItemRate";
		this.CmbBrandNamePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNamePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNamePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNamePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance430).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance430).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance430;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance431).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance431).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance431).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance431;
		((AppearanceBase)appearance432).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance432;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance433).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance433).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance433).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance433;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance434).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance434;
		((AppearanceBase)appearance435).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance435).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance435;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance436).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance436;
		((AppearanceBase)appearance439).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance439).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance439;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance440).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance440).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance440).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance440).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance440).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance440;
		((AppearanceBase)appearance441).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance441;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance442).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance442;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance443).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance443;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNamePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Location = new System.Drawing.Point(441, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Name = "CmbBrandNamePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Leave += new System.EventHandler(CmbBrandNamePackingMaterial_Leave);
		this.txtPackingMaterialQTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQTY.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQTY.Location = new System.Drawing.Point(709, 37);
		this.txtPackingMaterialQTY.Name = "txtPackingMaterialQTY";
		this.txtPackingMaterialQTY.Size = new System.Drawing.Size(99, 23);
		this.txtPackingMaterialQTY.TabIndex = 4;
		this.txtPackingMaterialQTY.TextChanged += new System.EventHandler(txtPackingMaterialQTY_TextChanged);
		this.txtPackingMaterialQTY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingMaterialQTY_KeyPress);
		this.txtAmountPackingMaterial.BackColor = System.Drawing.Color.White;
		this.txtAmountPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountPackingMaterial.Enabled = false;
		this.txtAmountPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountPackingMaterial.Location = new System.Drawing.Point(897, 37);
		this.txtAmountPackingMaterial.Name = "txtAmountPackingMaterial";
		this.txtAmountPackingMaterial.ReadOnly = true;
		this.txtAmountPackingMaterial.Size = new System.Drawing.Size(133, 23);
		this.txtAmountPackingMaterial.TabIndex = 6;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(443, 19);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(111, 15);
		this.label65.TabIndex = 118;
		this.label65.Text = "OutPut Item  Name";
		this.lblAmountPackingMaterial.AutoSize = true;
		this.lblAmountPackingMaterial.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountPackingMaterial.Location = new System.Drawing.Point(897, 19);
		this.lblAmountPackingMaterial.Name = "lblAmountPackingMaterial";
		this.lblAmountPackingMaterial.Size = new System.Drawing.Size(51, 15);
		this.lblAmountPackingMaterial.TabIndex = 61;
		this.lblAmountPackingMaterial.Text = "Amount";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(631, 19);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(61, 15);
		this.label66.TabIndex = 120;
		this.label66.Text = "Item Uom";
		this.tabPage7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlantdetail);
		this.tabPage7.Controls.Add(this.panel22);
		this.tabPage7.Controls.Add(this.panel23);
		this.tabPage7.Location = new System.Drawing.Point(4, 29);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Size = new System.Drawing.Size(1103, 305);
		this.tabPage7.TabIndex = 4;
		this.tabPage7.Text = "Plants Detail";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.grdPlantdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPlantdetail).BackColor = System.Drawing.Color.White;
		grdPlantdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPlantdetail.DesignTimeLayout = grdPlantdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPlantdetail.FilterMode = (FilterMode)2;
		this.grdPlantdetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPlantdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Location = new System.Drawing.Point(0, 97);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Name = "grdPlantdetail";
		((ScrollableBase)this.grdPlantdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).Size = new System.Drawing.Size(1103, 208);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).TabIndex = 0;
		this.grdPlantdetail.TotalRow = (InheritableBoolean)1;
		this.grdPlantdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlantdetail).VisualStyle = (VisualStyle)4;
		this.grdPlantdetail.ColumnButtonClick += new ColumnActionEventHandler(grdPlantdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).DoubleClick += new System.EventHandler(grdPlantdetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPlantdetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPlantdetail_KeyDown);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.ctrlGrdBar6);
		this.panel22.Controls.Add(this.label85);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 77);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1103, 20);
		this.panel22.TabIndex = 16;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1060, -2);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.grdPlantdetail;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar6.TabIndex = 22;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.White;
		this.label85.Location = new System.Drawing.Point(3, 2);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(81, 16);
		this.label85.TabIndex = 6;
		this.label85.Text = "Plant Detail";
		this.panel23.BackColor = System.Drawing.Color.White;
		this.panel23.Controls.Add(this.groupBox4);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1103, 77);
		this.panel23.TabIndex = 15;
		this.groupBox4.Controls.Add(this.label92);
		this.groupBox4.Controls.Add(this.txtAssumedPlantCapacity);
		this.groupBox4.Controls.Add(this.btnAddPlantDetail);
		this.groupBox4.Controls.Add(this.btnCanclePlantdetail);
		this.groupBox4.Controls.Add(this.btnUpdatePlantDetail);
		this.groupBox4.Controls.Add(this.txtRemarksPlantDetail);
		this.groupBox4.Controls.Add(this.label87);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantFeader);
		this.groupBox4.Controls.Add(this.label8);
		this.groupBox4.Controls.Add(this.btnProductionPlant);
		this.groupBox4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 5);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1039, 66);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail";
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.Location = new System.Drawing.Point(154, 18);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(104, 15);
		this.label92.TabIndex = 71;
		this.label92.Text = "Assumed Capacity";
		this.txtAssumedPlantCapacity.Location = new System.Drawing.Point(158, 37);
		this.txtAssumedPlantCapacity.Name = "txtAssumedPlantCapacity";
		this.txtAssumedPlantCapacity.Size = new System.Drawing.Size(100, 23);
		this.txtAssumedPlantCapacity.TabIndex = 2;
		this.txtAssumedPlantCapacity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAssumedPlantCapacity_KeyPress);
		this.btnAddPlantDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddPlantDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddPlantDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddPlantDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddPlantDetail.Location = new System.Drawing.Point(857, 36);
		this.btnAddPlantDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddPlantDetail.Name = "btnAddPlantDetail";
		this.btnAddPlantDetail.Size = new System.Drawing.Size(38, 26);
		this.btnAddPlantDetail.TabIndex = 4;
		this.btnAddPlantDetail.Text = "+";
		this.btnAddPlantDetail.UseVisualStyleBackColor = false;
		this.btnAddPlantDetail.Click += new System.EventHandler(btnAddPlantDetail_Click);
		this.btnCanclePlantdetail.BackColor = System.Drawing.Color.Teal;
		this.btnCanclePlantdetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCanclePlantdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCanclePlantdetail.ForeColor = System.Drawing.Color.White;
		this.btnCanclePlantdetail.Location = new System.Drawing.Point(965, 36);
		this.btnCanclePlantdetail.Name = "btnCanclePlantdetail";
		this.btnCanclePlantdetail.Size = new System.Drawing.Size(67, 26);
		this.btnCanclePlantdetail.TabIndex = 6;
		this.btnCanclePlantdetail.Text = "Cancel";
		this.btnCanclePlantdetail.UseVisualStyleBackColor = false;
		this.btnCanclePlantdetail.Click += new System.EventHandler(btnCanclePlantdetail_Click);
		this.btnUpdatePlantDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdatePlantDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatePlantDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatePlantDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdatePlantDetail.Location = new System.Drawing.Point(895, 36);
		this.btnUpdatePlantDetail.Name = "btnUpdatePlantDetail";
		this.btnUpdatePlantDetail.Size = new System.Drawing.Size(70, 26);
		this.btnUpdatePlantDetail.TabIndex = 5;
		this.btnUpdatePlantDetail.Text = "Update ";
		this.btnUpdatePlantDetail.UseVisualStyleBackColor = false;
		this.btnUpdatePlantDetail.Click += new System.EventHandler(btnUpdatePlantDetail_Click);
		this.txtRemarksPlantDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksPlantDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPlantDetail.Location = new System.Drawing.Point(261, 38);
		this.txtRemarksPlantDetail.Name = "txtRemarksPlantDetail";
		this.txtRemarksPlantDetail.Size = new System.Drawing.Size(589, 23);
		this.txtRemarksPlantDetail.TabIndex = 3;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(258, 20);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(52, 15);
		this.label87.TabIndex = 63;
		this.label87.Text = "Remarks";
		this.cmbPlantFeader.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantFeader.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantFeader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantFeader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance292).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance292).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance292).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance294).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPlantFeader.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Location = new System.Drawing.Point(9, 36);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Name = "cmbPlantFeader";
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(6, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 69;
		this.label8.Text = "Plant / Feader";
		this.btnProductionPlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnProductionPlant.Location = new System.Drawing.Point(132, 36);
		this.btnProductionPlant.Name = "btnProductionPlant";
		this.btnProductionPlant.Size = new System.Drawing.Size(23, 26);
		this.btnProductionPlant.TabIndex = 1;
		this.btnProductionPlant.TabStop = false;
		this.btnProductionPlant.UseVisualStyleBackColor = true;
		this.btnProductionPlant.Click += new System.EventHandler(btnProductionPlant_Click);
		this.tabLabStandard.Controls.Add((System.Windows.Forms.Control)(object)this.grdLabStandards);
		this.tabLabStandard.Controls.Add(this.panel13);
		this.tabLabStandard.Controls.Add(this.panel24);
		this.tabLabStandard.Location = new System.Drawing.Point(4, 29);
		this.tabLabStandard.Name = "tabLabStandard";
		this.tabLabStandard.Size = new System.Drawing.Size(1103, 305);
		this.tabLabStandard.TabIndex = 5;
		this.tabLabStandard.Text = "Lab Standards (Item And Party Wise)";
		this.tabLabStandard.UseVisualStyleBackColor = true;
		this.grdLabStandards.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdLabStandards).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLabStandards.FilterMode = (FilterMode)2;
		this.grdLabStandards.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLabStandards).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLabStandards.GroupByBoxVisible = false;
		this.grdLabStandards.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdLabStandards).Location = new System.Drawing.Point(0, 93);
		((System.Windows.Forms.Control)(object)this.grdLabStandards).Name = "grdLabStandards";
		((ScrollableBase)this.grdLabStandards).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdLabStandards).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdLabStandards).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLabStandards).Size = new System.Drawing.Size(1103, 212);
		((System.Windows.Forms.Control)(object)this.grdLabStandards).TabIndex = 1;
		this.grdLabStandards.TotalRow = (InheritableBoolean)1;
		this.grdLabStandards.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLabStandards).VisualStyle = (VisualStyle)4;
		this.grdLabStandards.ColumnButtonClick += new ColumnActionEventHandler(grdLabStandards_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdLabStandards).KeyDown += new System.Windows.Forms.KeyEventHandler(grdLabStandards_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar7);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 72);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1103, 21);
		this.panel13.TabIndex = 83;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1060, 0);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = this.grdLabStandards;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar7.TabIndex = 128;
		this.panel24.BackColor = System.Drawing.Color.White;
		this.panel24.Controls.Add(this.groupBox5);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1103, 72);
		this.panel24.TabIndex = 0;
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerName);
		this.groupBox5.Controls.Add(this.BtnDefineLabStandards);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard);
		this.groupBox5.Controls.Add(this.label95);
		this.groupBox5.Controls.Add(this.panel25);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup);
		this.groupBox5.Controls.Add(this.label93);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(3, 3);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(790, 67);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Base Information";
		this.CmbBuyerName.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance426).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance426;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance438).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance438).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance438).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance438;
		((AppearanceBase)appearance444).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance444;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance445).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance445).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance445).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance445).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance445;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance446).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance446).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance446;
		((AppearanceBase)appearance447).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance447).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance447;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Location = new System.Drawing.Point(229, 37);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Name = "CmbBuyerName";
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Leave += new System.EventHandler(CmbBuyerName_Leave);
		this.BtnDefineLabStandards.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineLabStandards.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineLabStandards.Location = new System.Drawing.Point(760, 37);
		this.BtnDefineLabStandards.Name = "BtnDefineLabStandards";
		this.BtnDefineLabStandards.Size = new System.Drawing.Size(25, 27);
		this.BtnDefineLabStandards.TabIndex = 3;
		this.BtnDefineLabStandards.TabStop = false;
		this.BtnDefineLabStandards.UseVisualStyleBackColor = true;
		this.BtnDefineLabStandards.Click += new System.EventHandler(BtnDefineLabStandards_Click);
		this.CmbBrandNameLabStandard.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNameLabStandard.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNameLabStandard.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNameLabStandard.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance403).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Appearance = (AppearanceBase)(object)appearance403;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance405).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance405).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance405).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance405;
		((AppearanceBase)appearance406).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance406;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance407).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance407).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance407).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance407).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance407;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance408).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance408).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance408;
		((AppearanceBase)appearance409).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance409).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance409;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance410;
		((AppearanceBase)appearance411).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance411).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance412).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance412).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance412).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance412).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance412;
		((AppearanceBase)appearance413).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance414).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance414;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance416;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNameLabStandard).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNameLabStandard.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).Location = new System.Drawing.Point(2, 37);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).Name = "CmbBrandNameLabStandard";
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameLabStandard).Leave += new System.EventHandler(CmbBrandNameLabStandard_Leave);
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.Location = new System.Drawing.Point(4, 20);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(73, 15);
		this.label95.TabIndex = 126;
		this.label95.Text = "OutPut Item";
		this.panel25.Controls.Add(this.RadBusinessName);
		this.panel25.Controls.Add(this.RadNickName);
		this.panel25.Controls.Add(this.radPartyCode);
		this.panel25.Controls.Add(this.label91);
		this.panel25.Location = new System.Drawing.Point(229, 12);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(327, 22);
		this.panel25.TabIndex = 124;
		this.RadBusinessName.AutoSize = true;
		this.RadBusinessName.Checked = true;
		this.RadBusinessName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBusinessName.Location = new System.Drawing.Point(45, 3);
		this.RadBusinessName.Name = "RadBusinessName";
		this.RadBusinessName.Size = new System.Drawing.Size(123, 17);
		this.RadBusinessName.TabIndex = 63;
		this.RadBusinessName.TabStop = true;
		this.RadBusinessName.Text = "Business Name";
		this.RadBusinessName.UseVisualStyleBackColor = true;
		this.RadBusinessName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.RadNickName.AutoSize = true;
		this.RadNickName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNickName.Location = new System.Drawing.Point(169, 3);
		this.RadNickName.Name = "RadNickName";
		this.RadNickName.Size = new System.Drawing.Size(94, 17);
		this.RadNickName.TabIndex = 65;
		this.RadNickName.Text = "Nick Name";
		this.RadNickName.UseVisualStyleBackColor = true;
		this.RadNickName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.radPartyCode.AutoSize = true;
		this.radPartyCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPartyCode.Location = new System.Drawing.Point(269, 4);
		this.radPartyCode.Name = "radPartyCode";
		this.radPartyCode.Size = new System.Drawing.Size(57, 17);
		this.radPartyCode.TabIndex = 65;
		this.radPartyCode.Text = "Code";
		this.radPartyCode.UseVisualStyleBackColor = true;
		this.radPartyCode.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.label91.Location = new System.Drawing.Point(3, 4);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(42, 13);
		this.label91.TabIndex = 121;
		this.label91.Text = "Party";
		this.CmbAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Location = new System.Drawing.Point(558, 37);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Name = "CmbAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Leave += new System.EventHandler(CmbAnalysisGroup_Leave);
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.Location = new System.Drawing.Point(559, 20);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(90, 15);
		this.label93.TabIndex = 28;
		this.label93.Text = "Standard Name";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtWagesAmountInPut);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.label43);
		this.panel4.Controls.Add(this.txtPmAmount);
		this.panel4.Controls.Add(this.txtWagesRateInPut);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtWagesAmountOutPut);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotAc);
		this.panel4.Controls.Add(this.label53);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.txtPmRate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut);
		this.panel4.Controls.Add(this.label52);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add(this.txtWagesRateOutPut);
		this.panel4.Controls.Add(this.label51);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingMaterial);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1111, 322);
		this.panel4.TabIndex = 60;
		this.txtWagesAmountInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmountInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesAmountInPut.Location = new System.Drawing.Point(1133, 283);
		this.txtWagesAmountInPut.Name = "txtWagesAmountInPut";
		this.txtWagesAmountInPut.ReadOnly = true;
		this.txtWagesAmountInPut.Size = new System.Drawing.Size(63, 23);
		this.txtWagesAmountInPut.TabIndex = 13;
		this.txtWagesAmountInPut.Visible = false;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbentrytype);
		this.groupBox3.Controls.Add(this.cmbfiltertype);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFiltersType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocNo);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(7, 179);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(194, 142);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters For Item";
		this.cmbentrytype.AllowNull = (DefaultableBoolean)1;
		this.cmbentrytype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbentrytype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbentrytype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance197;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance202).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbentrytype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Location = new System.Drawing.Point(73, 33);
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Name = "cmbentrytype";
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.cmbentrytype).TabIndex = 0;
		this.cmbentrytype.ValueChanged += new System.EventHandler(cmbentrytype_ValueChanged);
		this.cmbfiltertype.AutoSize = true;
		this.cmbfiltertype.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.cmbfiltertype.Location = new System.Drawing.Point(4, 74);
		this.cmbfiltertype.Name = "cmbfiltertype";
		this.cmbfiltertype.Size = new System.Drawing.Size(61, 15);
		this.cmbfiltertype.TabIndex = 178;
		this.cmbfiltertype.Text = "Filter Type";
		this.cmbFiltersType.AllowNull = (DefaultableBoolean)1;
		this.cmbFiltersType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFiltersType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFiltersType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance209;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance214).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbFiltersType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Location = new System.Drawing.Point(73, 68);
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Name = "cmbFiltersType";
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Leave += new System.EventHandler(cmbFiltersType_Leave);
		this.cmbDocNo.AllowNull = (DefaultableBoolean)1;
		this.cmbDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Location = new System.Drawing.Point(73, 102);
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Name = "cmbDocNo";
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Leave += new System.EventHandler(cmbDocNo_Leave);
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(4, 101);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 31);
		this.label28.TabIndex = 178;
		this.label28.Text = "Doc No / Other";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(4, 39);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(59, 15);
		this.label40.TabIndex = 204;
		this.label40.Text = "EntryType";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(1052, 290);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(51, 15);
		this.label43.TabIndex = 206;
		this.label43.Text = "Amount";
		this.label43.Visible = false;
		this.txtPmAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPmAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPmAmount.Location = new System.Drawing.Point(1133, 197);
		this.txtPmAmount.Name = "txtPmAmount";
		this.txtPmAmount.ReadOnly = true;
		this.txtPmAmount.Size = new System.Drawing.Size(63, 23);
		this.txtPmAmount.TabIndex = 19;
		this.txtPmAmount.Visible = false;
		this.txtWagesRateInPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesRateInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesRateInPut.Location = new System.Drawing.Point(1133, 257);
		this.txtWagesRateInPut.Name = "txtWagesRateInPut";
		this.txtWagesRateInPut.ReadOnly = true;
		this.txtWagesRateInPut.Size = new System.Drawing.Size(63, 23);
		this.txtWagesRateInPut.TabIndex = 12;
		this.txtWagesRateInPut.Visible = false;
		this.groupBox2.Controls.Add(this.btnProductionTypeDefine);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipWarehouse);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.txtInvoiceReference);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionType);
		this.groupBox2.Controls.Add(this.txtOtherInstruction);
		this.groupBox2.Controls.Add(this.txtDeliveryInstruction);
		this.groupBox2.Controls.Add(this.txtPlantInstruction);
		this.groupBox2.Controls.Add(this.label72);
		this.groupBox2.Controls.Add(this.label71);
		this.groupBox2.Controls.Add(this.txtPlanDate);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtPlanCode);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipItem);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlanType);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtEndDate);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtProductionInstruction);
		this.groupBox2.Controls.Add(this.txtstartDate);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1058, 176);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.btnProductionTypeDefine.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProductionTypeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnProductionTypeDefine.Location = new System.Drawing.Point(302, 70);
		this.btnProductionTypeDefine.Name = "btnProductionTypeDefine";
		this.btnProductionTypeDefine.Size = new System.Drawing.Size(25, 27);
		this.btnProductionTypeDefine.TabIndex = 216;
		this.btnProductionTypeDefine.TabStop = false;
		this.btnProductionTypeDefine.UseVisualStyleBackColor = true;
		this.btnProductionTypeDefine.Click += new System.EventHandler(btnProductionTypeDefine_Click);
		this.CmbWipWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWipWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance451).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance451).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance451;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance455).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance455).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance455;
		((AppearanceBase)appearance456).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance456).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance456;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance457).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance457;
		((AppearanceBase)appearance458).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance458).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance458;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance459).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance459).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance459).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance459).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance459).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance459;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Location = new System.Drawing.Point(859, 70);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Name = "CmbWipWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).TabIndex = 10;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(788, 64);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(67, 30);
		this.label90.TabIndex = 215;
		this.label90.Text = "Wip \r\nWarehouse";
		this.txtInvoiceReference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceReference.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInvoiceReference.Location = new System.Drawing.Point(113, 46);
		this.txtInvoiceReference.Name = "txtInvoiceReference";
		this.txtInvoiceReference.Size = new System.Drawing.Size(214, 23);
		this.txtInvoiceReference.TabIndex = 2;
		this.cmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Location = new System.Drawing.Point(113, 71);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Name = "cmbProductionType";
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).TabIndex = 3;
		this.txtOtherInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherInstruction.Location = new System.Drawing.Point(113, 149);
		this.txtOtherInstruction.Name = "txtOtherInstruction";
		this.txtOtherInstruction.Size = new System.Drawing.Size(924, 23);
		this.txtOtherInstruction.TabIndex = 14;
		this.txtDeliveryInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryInstruction.Location = new System.Drawing.Point(113, 124);
		this.txtDeliveryInstruction.Name = "txtDeliveryInstruction";
		this.txtDeliveryInstruction.Size = new System.Drawing.Size(924, 23);
		this.txtDeliveryInstruction.TabIndex = 13;
		this.txtPlantInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlantInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlantInstruction.Location = new System.Drawing.Point(113, 99);
		this.txtPlantInstruction.Name = "txtPlantInstruction";
		this.txtPlantInstruction.Size = new System.Drawing.Size(924, 23);
		this.txtPlantInstruction.TabIndex = 12;
		this.label72.AutoSize = true;
		this.label72.BackColor = System.Drawing.Color.White;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(4, 153);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(98, 15);
		this.label72.TabIndex = 213;
		this.label72.Text = "Other Instruction";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label71.AutoSize = true;
		this.label71.BackColor = System.Drawing.Color.White;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(4, 128);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(111, 15);
		this.label71.TabIndex = 210;
		this.label71.Text = "Delivery Instruction";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPlanDate.CustomFormat = "dd-MMM-yy";
		this.txtPlanDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPlanDate.Location = new System.Drawing.Point(195, 18);
		this.txtPlanDate.Name = "txtPlanDate";
		this.txtPlanDate.Size = new System.Drawing.Size(132, 23);
		this.txtPlanDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Doc No,Date";
		this.txtPlanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanCode.Enabled = false;
		this.txtPlanCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPlanCode.Location = new System.Drawing.Point(113, 18);
		this.txtPlanCode.Name = "txtPlanCode";
		this.txtPlanCode.Size = new System.Drawing.Size(81, 23);
		this.txtPlanCode.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(4, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 73;
		this.label1.Text = "Job Order No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(4, 77);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(94, 15);
		this.label13.TabIndex = 157;
		this.label13.Text = "Production Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(331, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 155;
		this.label4.Text = "Instruction";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWipItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWipItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWipItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Location = new System.Drawing.Point(859, 42);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Name = "CmbWipItem";
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(788, 43);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(58, 15);
		this.label23.TabIndex = 176;
		this.label23.Text = "WIP Item";
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance259;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(859, 14);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 8;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance278).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance280).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(402, 44);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 5;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(788, 17);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 168;
		this.label15.Text = "WIP A/c";
		this.cmbPlanType.AllowNull = (DefaultableBoolean)1;
		this.cmbPlanType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlanType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlanType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance299;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance302).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance305).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPlanType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Location = new System.Drawing.Point(402, 16);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Name = "cmbPlanType";
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(331, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 71;
		this.label2.Text = "Plan Type";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 103);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(95, 15);
		this.label6.TabIndex = 79;
		this.label6.Text = "Plant Instruction";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(652, 46);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(133, 23);
		this.txtEndDate.TabIndex = 7;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(331, 50);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 151;
		this.label10.Text = "Status";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(594, 50);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(55, 15);
		this.label18.TabIndex = 163;
		this.label18.Text = "End Date";
		this.txtProductionInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionInstruction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProductionInstruction.Location = new System.Drawing.Point(402, 73);
		this.txtProductionInstruction.Name = "txtProductionInstruction";
		this.txtProductionInstruction.Size = new System.Drawing.Size(383, 23);
		this.txtProductionInstruction.TabIndex = 11;
		this.txtstartDate.CustomFormat = "dd-MMM-yy";
		this.txtstartDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtstartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtstartDate.Location = new System.Drawing.Point(652, 18);
		this.txtstartDate.Name = "txtstartDate";
		this.txtstartDate.Size = new System.Drawing.Size(133, 23);
		this.txtstartDate.TabIndex = 6;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(594, 22);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 159;
		this.label16.Text = "Start Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(1052, 265);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 201;
		this.label21.Text = "Wages Rate";
		this.label21.Visible = false;
		this.groupBox1.Controls.Add(this.panel55);
		this.groupBox1.Controls.Add(this.panel51);
		this.groupBox1.Controls.Add(this.panel53);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(207, 179);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(839, 142);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Calculactions";
		this.panel55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel55.Controls.Add(this.panel56);
		this.panel55.Controls.Add(this.label58);
		this.panel55.Controls.Add(this.txtFGAvgRate);
		this.panel55.Controls.Add(this.label55);
		this.panel55.Controls.Add(this.txtFGRateWithoutExp);
		this.panel55.Location = new System.Drawing.Point(436, 81);
		this.panel55.Name = "panel55";
		this.panel55.Size = new System.Drawing.Size(397, 53);
		this.panel55.TabIndex = 212;
		this.panel56.BackColor = System.Drawing.Color.Teal;
		this.panel56.Controls.Add(this.label54);
		this.panel56.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel56.Location = new System.Drawing.Point(0, 0);
		this.panel56.Name = "panel56";
		this.panel56.Size = new System.Drawing.Size(395, 20);
		this.panel56.TabIndex = 12;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(106, 0);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(167, 17);
		this.label54.TabIndex = 6;
		this.label54.Text = "Finish Good Average Rate";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(2, 26);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(66, 19);
		this.label58.TabIndex = 8;
		this.label58.Text = "W/O Exp";
		this.txtFGAvgRate.Enabled = false;
		this.txtFGAvgRate.Font = new System.Drawing.Font("Verdana", 10.75f);
		this.txtFGAvgRate.Location = new System.Drawing.Point(265, 23);
		this.txtFGAvgRate.Name = "txtFGAvgRate";
		this.txtFGAvgRate.Size = new System.Drawing.Size(125, 25);
		this.txtFGAvgRate.TabIndex = 213;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(198, 26);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(66, 19);
		this.label55.TabIndex = 8;
		this.label55.Text = "With Exp";
		this.txtFGRateWithoutExp.Enabled = false;
		this.txtFGRateWithoutExp.Font = new System.Drawing.Font("Verdana", 10.75f);
		this.txtFGRateWithoutExp.Location = new System.Drawing.Point(68, 23);
		this.txtFGRateWithoutExp.Name = "txtFGRateWithoutExp";
		this.txtFGRateWithoutExp.Size = new System.Drawing.Size(125, 25);
		this.txtFGRateWithoutExp.TabIndex = 214;
		this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel51.Controls.Add(this.panel52);
		this.panel51.Controls.Add(this.label75);
		this.panel51.Controls.Add(this.label76);
		this.panel51.Controls.Add(this.txtTotalAmountFG);
		this.panel51.Controls.Add(this.label78);
		this.panel51.Controls.Add(this.txtInPutTotalQty);
		this.panel51.Controls.Add(this.txtTotalWeightFG);
		this.panel51.Controls.Add(this.txtInPutTotalWeight);
		this.panel51.Controls.Add(this.txtTotalAmountInPut);
		this.panel51.Controls.Add(this.txtTotalQtyFG);
		this.panel51.Controls.Add(this.txtTotalQtyBP);
		this.panel51.Controls.Add(this.txtTotalWeightBP);
		this.panel51.Controls.Add(this.txtTotalAmountBP);
		this.panel51.Location = new System.Drawing.Point(6, 22);
		this.panel51.Name = "panel51";
		this.panel51.Size = new System.Drawing.Size(426, 112);
		this.panel51.TabIndex = 211;
		this.panel52.BackColor = System.Drawing.Color.Teal;
		this.panel52.Controls.Add(this.label11);
		this.panel52.Controls.Add(this.label17);
		this.panel52.Controls.Add(this.label73);
		this.panel52.Controls.Add(this.label74);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(424, 24);
		this.panel52.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(2, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(79, 17);
		this.label11.TabIndex = 5;
		this.label11.Text = "Description";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(113, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 17);
		this.label17.TabIndex = 2;
		this.label17.Text = "ItemQty";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.ForeColor = System.Drawing.Color.White;
		this.label73.Location = new System.Drawing.Point(198, 4);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(75, 17);
		this.label73.TabIndex = 4;
		this.label73.Text = "NetWeight";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.White;
		this.label74.Location = new System.Drawing.Point(312, 4);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(58, 17);
		this.label74.TabIndex = 4;
		this.label74.Text = "Amount";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(6, 32);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(75, 15);
		this.label75.TabIndex = 1;
		this.label75.Text = "Raw Material";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(6, 59);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(65, 15);
		this.label76.TabIndex = 1;
		this.label76.Text = "By Product";
		this.txtTotalAmountFG.Enabled = false;
		this.txtTotalAmountFG.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmountFG.Location = new System.Drawing.Point(304, 82);
		this.txtTotalAmountFG.Name = "txtTotalAmountFG";
		this.txtTotalAmountFG.Size = new System.Drawing.Size(116, 23);
		this.txtTotalAmountFG.TabIndex = 212;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(6, 86);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(75, 15);
		this.label78.TabIndex = 1;
		this.label78.Text = "Finish Goods";
		this.txtInPutTotalQty.Enabled = false;
		this.txtInPutTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInPutTotalQty.Location = new System.Drawing.Point(104, 28);
		this.txtInPutTotalQty.Name = "txtInPutTotalQty";
		this.txtInPutTotalQty.Size = new System.Drawing.Size(82, 23);
		this.txtInPutTotalQty.TabIndex = 0;
		this.txtTotalWeightFG.Enabled = false;
		this.txtTotalWeightFG.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeightFG.Location = new System.Drawing.Point(192, 82);
		this.txtTotalWeightFG.Name = "txtTotalWeightFG";
		this.txtTotalWeightFG.Size = new System.Drawing.Size(107, 23);
		this.txtTotalWeightFG.TabIndex = 213;
		this.txtInPutTotalWeight.Enabled = false;
		this.txtInPutTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInPutTotalWeight.Location = new System.Drawing.Point(192, 28);
		this.txtInPutTotalWeight.Name = "txtInPutTotalWeight";
		this.txtInPutTotalWeight.Size = new System.Drawing.Size(107, 23);
		this.txtInPutTotalWeight.TabIndex = 0;
		this.txtTotalAmountInPut.Enabled = false;
		this.txtTotalAmountInPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmountInPut.Location = new System.Drawing.Point(304, 28);
		this.txtTotalAmountInPut.Name = "txtTotalAmountInPut";
		this.txtTotalAmountInPut.Size = new System.Drawing.Size(116, 23);
		this.txtTotalAmountInPut.TabIndex = 0;
		this.txtTotalQtyFG.Enabled = false;
		this.txtTotalQtyFG.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalQtyFG.Location = new System.Drawing.Point(104, 82);
		this.txtTotalQtyFG.Name = "txtTotalQtyFG";
		this.txtTotalQtyFG.Size = new System.Drawing.Size(82, 23);
		this.txtTotalQtyFG.TabIndex = 214;
		this.txtTotalQtyBP.Enabled = false;
		this.txtTotalQtyBP.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalQtyBP.Location = new System.Drawing.Point(104, 55);
		this.txtTotalQtyBP.Name = "txtTotalQtyBP";
		this.txtTotalQtyBP.Size = new System.Drawing.Size(82, 23);
		this.txtTotalQtyBP.TabIndex = 0;
		this.txtTotalWeightBP.Enabled = false;
		this.txtTotalWeightBP.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeightBP.Location = new System.Drawing.Point(192, 55);
		this.txtTotalWeightBP.Name = "txtTotalWeightBP";
		this.txtTotalWeightBP.Size = new System.Drawing.Size(107, 23);
		this.txtTotalWeightBP.TabIndex = 0;
		this.txtTotalAmountBP.Enabled = false;
		this.txtTotalAmountBP.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmountBP.Location = new System.Drawing.Point(304, 55);
		this.txtTotalAmountBP.Name = "txtTotalAmountBP";
		this.txtTotalAmountBP.Size = new System.Drawing.Size(116, 23);
		this.txtTotalAmountBP.TabIndex = 0;
		this.panel53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel53.Controls.Add(this.panel54);
		this.panel53.Controls.Add(this.label56);
		this.panel53.Controls.Add(this.txtOverHeadsAmount);
		this.panel53.Controls.Add(this.label57);
		this.panel53.Controls.Add(this.txtPackingMaterialAmount);
		this.panel53.Location = new System.Drawing.Point(436, 24);
		this.panel53.Name = "panel53";
		this.panel53.Size = new System.Drawing.Size(397, 52);
		this.panel53.TabIndex = 211;
		this.panel54.BackColor = System.Drawing.Color.Teal;
		this.panel54.Controls.Add(this.label94);
		this.panel54.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel54.Location = new System.Drawing.Point(0, 0);
		this.panel54.Name = "panel54";
		this.panel54.Size = new System.Drawing.Size(395, 20);
		this.panel54.TabIndex = 11;
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.White;
		this.label94.Location = new System.Drawing.Point(73, 0);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(233, 17);
		this.label94.TabIndex = 6;
		this.label94.Text = "OverHeads && Packing Mateial Totals";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(3, 26);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(58, 17);
		this.label56.TabIndex = 1;
		this.label56.Text = "O.Heads";
		this.txtOverHeadsAmount.Enabled = false;
		this.txtOverHeadsAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOverHeadsAmount.Location = new System.Drawing.Point(68, 23);
		this.txtOverHeadsAmount.Name = "txtOverHeadsAmount";
		this.txtOverHeadsAmount.Size = new System.Drawing.Size(125, 23);
		this.txtOverHeadsAmount.TabIndex = 0;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(198, 26);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(66, 17);
		this.label57.TabIndex = 1;
		this.label57.Text = "P.Material";
		this.txtPackingMaterialAmount.Enabled = false;
		this.txtPackingMaterialAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingMaterialAmount.Location = new System.Drawing.Point(265, 23);
		this.txtPackingMaterialAmount.Name = "txtPackingMaterialAmount";
		this.txtPackingMaterialAmount.Size = new System.Drawing.Size(125, 23);
		this.txtPackingMaterialAmount.TabIndex = 0;
		this.txtWagesAmountOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmountOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesAmountOutPut.Location = new System.Drawing.Point(1133, 111);
		this.txtWagesAmountOutPut.Name = "txtWagesAmountOutPut";
		this.txtWagesAmountOutPut.ReadOnly = true;
		this.txtWagesAmountOutPut.Size = new System.Drawing.Size(63, 23);
		this.txtWagesAmountOutPut.TabIndex = 16;
		this.txtWagesAmountOutPut.Visible = false;
		this.CmbLotAc.AllowNull = (DefaultableBoolean)1;
		this.CmbLotAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance312;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance313).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance313).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance316).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance318).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance319).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance319).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance319).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLotAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Location = new System.Drawing.Point(1133, 15);
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Name = "CmbLotAc";
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Size = new System.Drawing.Size(63, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotAc).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Visible = false;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(1052, 197);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(72, 15);
		this.label53.TabIndex = 218;
		this.label53.Text = "PM Amount";
		this.label53.Visible = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(1052, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(43, 15);
		this.label22.TabIndex = 174;
		this.label22.Text = "JobLot";
		this.label22.Visible = false;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(1052, 108);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(51, 15);
		this.label48.TabIndex = 218;
		this.label48.Text = "Amount";
		this.label48.Visible = false;
		this.txtPmRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPmRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPmRate.Location = new System.Drawing.Point(1133, 172);
		this.txtPmRate.Name = "txtPmRate";
		this.txtPmRate.ReadOnly = true;
		this.txtPmRate.Size = new System.Drawing.Size(63, 23);
		this.txtPmRate.TabIndex = 18;
		this.txtPmRate.Visible = false;
		this.CmbLabourItemOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLabourItemOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabourItemOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabourItemOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance324).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance324).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance324).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance325;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance328).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance328).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance328).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance329;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance330).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance331;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance332).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance333).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance333).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance333).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance333).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance333;
		((AppearanceBase)appearance334).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance335).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabourItemOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLabourItemOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).Location = new System.Drawing.Point(1133, 47);
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).Name = "CmbLabourItemOutPut";
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).Size = new System.Drawing.Size(63, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).Visible = false;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemOutPut).TextChanged += new System.EventHandler(CmbLabourItemOutPut_TextChanged);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(1052, 172);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(51, 15);
		this.label52.TabIndex = 216;
		this.label52.Text = "PM Rate";
		this.label52.Visible = false;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(1052, 50);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(72, 15);
		this.label50.TabIndex = 215;
		this.label50.Text = "Labour Item";
		this.label50.Visible = false;
		this.txtWagesRateOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesRateOutPut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesRateOutPut.Location = new System.Drawing.Point(1133, 76);
		this.txtWagesRateOutPut.Name = "txtWagesRateOutPut";
		this.txtWagesRateOutPut.ReadOnly = true;
		this.txtWagesRateOutPut.Size = new System.Drawing.Size(63, 23);
		this.txtWagesRateOutPut.TabIndex = 15;
		this.txtWagesRateOutPut.Visible = false;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(1052, 144);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(77, 15);
		this.label51.TabIndex = 215;
		this.label51.Text = "Packing Item";
		this.label51.Visible = false;
		this.CmbLabourItemInPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLabourItemInPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabourItemInPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabourItemInPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance337;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance339).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance339).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance339).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance339;
		((AppearanceBase)appearance340).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance340;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance341).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance341).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance341).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance342).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance343).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance345).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance345).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance346).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance346).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance346).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance347).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance348).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance350;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabourItemInPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLabourItemInPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).Location = new System.Drawing.Point(1133, 226);
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).Name = "CmbLabourItemInPut";
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).Size = new System.Drawing.Size(63, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbLabourItemInPut).Visible = false;
		this.CmbPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance351).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance352).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance352).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance352).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance352;
		((AppearanceBase)appearance353).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance353;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance354).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance354).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance354).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance355).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance355;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance356).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance357;
		((AppearanceBase)appearance358).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance358).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance359).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance359).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance359).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance359).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance359;
		((AppearanceBase)appearance361).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).Location = new System.Drawing.Point(1133, 138);
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).Name = "CmbPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).Size = new System.Drawing.Size(63, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbPackingMaterial).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(1052, 239);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(72, 15);
		this.label20.TabIndex = 200;
		this.label20.Text = "Labour Item";
		this.label20.Visible = false;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(1052, 79);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(69, 15);
		this.label49.TabIndex = 216;
		this.label49.Text = "Wages Rate";
		this.label49.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label70);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1111, 54);
		this.panel3.TabIndex = 0;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.White;
		this.label70.Location = new System.Drawing.Point(10, 31);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(231, 19);
		this.label70.TabIndex = 209;
		this.label70.Text = "Production Job Order          ";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnSaveAs, this.Print, this.btnattachment, this.btnLoadInvoices, this.BtnGenerateInPutRates, this.btnGenerate,
			this.BtnShortcutKeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1111, 27);
		this.toolStrip1.TabIndex = 57;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "&Print-630";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnLoadInvoices.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadInvoices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(122, 24);
		this.btnLoadInvoices.Text = "Load Invoices";
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.BtnGenerateInPutRates.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.BtnGenerateInPutRates.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnGenerateInPutRates.Name = "BtnGenerateInPutRates";
		this.BtnGenerateInPutRates.Size = new System.Drawing.Size(173, 24);
		this.BtnGenerateInPutRates.Text = "Generate Input Rates";
		this.BtnGenerateInPutRates.Click += new System.EventHandler(BtnGenerateInPutRates_Click);
		this.btnGenerate.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.Size = new System.Drawing.Size(139, 24);
		this.btnGenerate.Text = "Run Calculations";
		this.btnGenerate.Click += new System.EventHandler(btnGenerate_Click);
		this.BtnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortcutKeys.Name = "BtnShortcutKeys";
		this.BtnShortcutKeys.Size = new System.Drawing.Size(125, 24);
		this.BtnShortcutKeys.Text = "Shortcut Keys";
		this.BtnShortcutKeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1024, 30);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(84, 20);
		this.chkPrint.TabIndex = 14;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1117, 720);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 147);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1111, 229);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 61;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar5);
		this.panel11.Controls.Add(this.label5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 121);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1111, 26);
		this.panel11.TabIndex = 70;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = null;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1066, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(7);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar5.TabIndex = 211;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(132, 18);
		this.label5.TabIndex = 69;
		this.label5.Text = "Filtered Records";
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.rdapproveddate);
		this.panel20.Controls.Add(this.rdmodifydate);
		this.panel20.Controls.Add(this.drdocdate);
		this.panel20.Controls.Add(this.rdentrydate);
		this.panel20.Controls.Add(this.label24);
		this.panel20.Controls.Add(this.txtOrderNoTo);
		this.panel20.Controls.Add(this.txtOrderNoFrom);
		this.panel20.Controls.Add(this.label38);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel20.Controls.Add(this.label84);
		this.panel20.Controls.Add(this.btnshowHistory);
		this.panel20.Controls.Add(this.ToDateHistory);
		this.panel20.Controls.Add(this.FromDateHistory);
		this.panel20.Controls.Add(this.label86);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.xmbjoborderno);
		this.panel20.Controls.Add(this.label88);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 55);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1111, 66);
		this.panel20.TabIndex = 69;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(906, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(906, 11);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(825, 11);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(825, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(340, 41);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(45, 15);
		this.label24.TabIndex = 598;
		this.label24.Text = "Doc To";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(400, 37);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(89, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(400, 9);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(89, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(340, 13);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(60, 15);
		this.label38.TabIndex = 597;
		this.label38.Text = "Doc From";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(97, 7);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(4, 13);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(60, 15);
		this.label84.TabIndex = 594;
		this.label84.Text = "Date Type";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(747, 35);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 6;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(218, 37);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.Size = new System.Drawing.Size(119, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(97, 37);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.Size = new System.Drawing.Size(119, 23);
		this.FromDateHistory.TabIndex = 1;
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(5, 41);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(91, 15);
		this.label86.TabIndex = 146;
		this.label86.Text = "Date From && To";
		this.xmbjoborderno.AllowNull = (DefaultableBoolean)1;
		this.xmbjoborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.xmbjoborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.xmbjoborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance417).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance418).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance418).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance418).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance418;
		((AppearanceBase)appearance419).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance419;
		((SpecialBoxBase)((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance420).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance420).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance420;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance421).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance421).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance421;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance422).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance422;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance423).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance423;
		((AppearanceBase)appearance424).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance424).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance424;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance425).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance425).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance425).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance425).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance425;
		((AppearanceBase)appearance427).TextHAlignAsString = "Left";
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance427;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance428).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance429;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.xmbjoborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Location = new System.Drawing.Point(576, 7);
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Name = "xmbjoborderno";
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).TabIndex = 5;
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.Location = new System.Drawing.Point(490, 13);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(78, 15);
		this.label88.TabIndex = 144;
		this.label88.Text = "Job Order No";
		this.tabControl3.Controls.Add(this.tabPage9);
		this.tabControl3.Controls.Add(this.tabPage8);
		this.tabControl3.Controls.Add(this.tabPage11);
		this.tabControl3.Controls.Add(this.tabPage10);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 376);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1111, 341);
		this.tabControl3.TabIndex = 68;
		this.tabPage9.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutHistory);
		this.tabPage9.Controls.Add(this.panel5);
		this.tabPage9.Location = new System.Drawing.Point(4, 25);
		this.tabPage9.Name = "tabPage9";
		this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage9.Size = new System.Drawing.Size(1103, 312);
		this.tabPage9.TabIndex = 1;
		this.tabPage9.Text = "OutPut";
		this.tabPage9.UseVisualStyleBackColor = true;
		this.grdOutPutHistory.AllowEdit = (InheritableBoolean)2;
		grdOutPutHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutHistory.DesignTimeLayout = grdOutPutHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPutHistory.DynamicFiltering = true;
		this.grdOutPutHistory.FilterMode = (FilterMode)2;
		this.grdOutPutHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Name = "grdOutPutHistory";
		((ScrollableBase)this.grdOutPutHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).Size = new System.Drawing.Size(1097, 283);
		((System.Windows.Forms.Control)(object)this.grdOutPutHistory).TabIndex = 68;
		this.grdOutPutHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutPutHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutHistory).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label81);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1097, 23);
		this.panel5.TabIndex = 67;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.White;
		this.label81.Location = new System.Drawing.Point(4, 3);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(262, 17);
		this.label81.TabIndex = 8;
		this.label81.Text = "OutPut Detail Of the above Selected Row";
		this.tabPage8.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputHistory);
		this.tabPage8.Controls.Add(this.panel12);
		this.tabPage8.Location = new System.Drawing.Point(4, 25);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(1103, 312);
		this.tabPage8.TabIndex = 0;
		this.tabPage8.Text = "Input";
		this.tabPage8.UseVisualStyleBackColor = true;
		this.grdInputHistory.AllowEdit = (InheritableBoolean)2;
		grdInputHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputHistory.DesignTimeLayout = grdInputHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInputHistory.DynamicFiltering = true;
		this.grdInputHistory.FilterMode = (FilterMode)2;
		this.grdInputHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Name = "grdInputHistory";
		((ScrollableBase)this.grdInputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Size = new System.Drawing.Size(1097, 285);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).TabIndex = 66;
		this.grdInputHistory.TotalRow = (InheritableBoolean)1;
		this.grdInputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label80);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1097, 21);
		this.panel12.TabIndex = 65;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.White;
		this.label80.Location = new System.Drawing.Point(5, 2);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(251, 17);
		this.label80.TabIndex = 7;
		this.label80.Text = "Input Detail Of the above Selected Row";
		this.tabPage11.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverheadHistory);
		this.tabPage11.Controls.Add(this.panel19);
		this.tabPage11.Location = new System.Drawing.Point(4, 25);
		this.tabPage11.Name = "tabPage11";
		this.tabPage11.Size = new System.Drawing.Size(1103, 312);
		this.tabPage11.TabIndex = 3;
		this.tabPage11.Text = "Over Head";
		this.tabPage11.UseVisualStyleBackColor = true;
		this.grdOverheadHistory.AllowEdit = (InheritableBoolean)2;
		grdOverheadHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverheadHistory.DesignTimeLayout = grdOverheadHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOverheadHistory.DynamicFiltering = true;
		this.grdOverheadHistory.FilterMode = (FilterMode)2;
		this.grdOverheadHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverheadHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Name = "grdOverheadHistory";
		((ScrollableBase)this.grdOverheadHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).Size = new System.Drawing.Size(1103, 290);
		((System.Windows.Forms.Control)(object)this.grdOverheadHistory).TabIndex = 69;
		this.grdOverheadHistory.TotalRow = (InheritableBoolean)1;
		this.grdOverheadHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverheadHistory).VisualStyle = (VisualStyle)4;
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.label82);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1103, 22);
		this.panel19.TabIndex = 68;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.White;
		this.label82.Location = new System.Drawing.Point(3, 3);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(285, 17);
		this.label82.TabIndex = 9;
		this.label82.Text = "OverHeads Detail Of the above Selected Row";
		this.tabPage10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory);
		this.tabPage10.Controls.Add(this.panel21);
		this.tabPage10.Location = new System.Drawing.Point(4, 25);
		this.tabPage10.Name = "tabPage10";
		this.tabPage10.Size = new System.Drawing.Size(1103, 312);
		this.tabPage10.TabIndex = 2;
		this.tabPage10.Text = "Packing Material";
		this.tabPage10.UseVisualStyleBackColor = true;
		this.grdPackingMaterialHistory.AllowEdit = (InheritableBoolean)2;
		grdPackingMaterialHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialHistory.DesignTimeLayout = grdPackingMaterialHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterialHistory.DynamicFiltering = true;
		this.grdPackingMaterialHistory.FilterMode = (FilterMode)2;
		this.grdPackingMaterialHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Name = "grdPackingMaterialHistory";
		((ScrollableBase)this.grdPackingMaterialHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).Size = new System.Drawing.Size(1103, 290);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialHistory).TabIndex = 69;
		this.grdPackingMaterialHistory.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialHistory).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label83);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1103, 22);
		this.panel21.TabIndex = 68;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.White;
		this.label83.Location = new System.Drawing.Point(4, 3);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(321, 17);
		this.label83.TabIndex = 10;
		this.label83.Text = "Packing Material Detail Of the above Selected Row";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label77);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1111, 52);
		this.panel8.TabIndex = 0;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.White;
		this.label77.Location = new System.Drawing.Point(5, 30);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(244, 19);
		this.label77.TabIndex = 210;
		this.label77.Text = "Production Job Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.BtnRefereshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1111, 27);
		this.toolStrip2.TabIndex = 58;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefereshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefereshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshHistory.Name = "BtnRefereshHistory";
		this.BtnRefereshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshHistory.Text = "Refresh";
		this.BtnRefereshHistory.Click += new System.EventHandler(BtnRefereshHistory_Click);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1125, 749);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ProductionPreCostingAndJobOrder";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProductionPreCostingAndJobOrder_FormClosing);
		base.Load += new System.EventHandler(frmProductionJobOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionJobOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPlannedOutput).EndInit();
		this.panel9.ResumeLayout(false);
		this.paneloutputgrd.ResumeLayout(false);
		this.paneloutputgrd.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryTypeOP).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouteruom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panelinputgrd.ResumeLayout(false);
		this.panelinputgrd.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackTypeInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouseinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropyearinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblotinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomInPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpacksizeinput).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomOverHead).EndInit();
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).EndInit();
		this.tabPage7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPlantdetail).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel23.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).EndInit();
		this.tabLabStandard.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLabStandards).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameLabStandard).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.panel55.ResumeLayout(false);
		this.panel55.PerformLayout();
		this.panel56.ResumeLayout(false);
		this.panel56.PerformLayout();
		this.panel51.ResumeLayout(false);
		this.panel51.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		this.panel53.ResumeLayout(false);
		this.panel53.PerformLayout();
		this.panel54.ResumeLayout(false);
		this.panel54.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLotAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabourItemOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabourItemInPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingMaterial).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xmbjoborderno).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPage9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabPage8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverheadHistory).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.tabPage10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
